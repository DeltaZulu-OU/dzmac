using System;
using System.IO;
using System.Linq;
using System.Text;

namespace Dzmac.Core.Presets
{
    internal static class TpfSerializer
    {
        private static readonly Encoding Utf16Le = Encoding.Unicode;
        private const int MaxTpfFileBytes = 1024 * 1024;
        private const int MaxPresetNameChars = 128;
        private const int MaxCustomMacChars = 32;
        private const int MaxIpv4FieldChars = 64;

        public static TpfFile Load(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentNullException(nameof(path));
            }

            Diagnostics.Info("tpf_load_start", ("path", path));
            return Load(File.ReadAllBytes(path));
        }

        public static TpfFile Load(byte[] bytes)
        {
            if (bytes is null)
            {
                throw new ArgumentNullException(nameof(bytes));
            }

            Diagnostics.Info("tpf_parse_start", ("payloadLength", bytes.Length));

            if (bytes.Length > MaxTpfFileBytes)
            {
                Diagnostics.Warning("tpf_parse_rejected", "TPF payload exceeds configured size limit.", ("payloadLength", bytes.Length), ("sizeLimit", MaxTpfFileBytes));
                throw new InvalidDataException($"TPF file is too large. Maximum supported size is {MaxTpfFileBytes} bytes.");
            }

            if (bytes.Length < 4)
            {
                Diagnostics.Warning("tpf_parse_rejected", "TPF payload is below minimum header size.", ("payloadLength", bytes.Length));
                throw new InvalidDataException("TPF file is too small.");
            }

            var file = new TpfFile
            {
                Version = bytes[0],
                Reserved = bytes[2],
                SelectedPresetIndex = bytes[3]
            };

            var declaredCount = bytes[1];
            var offset = 4;

            for (var i = 0; i < declaredCount && offset < bytes.Length; i++)
            {
                var presetStart = offset;
                try
                {
                    var preset = ReadPreset(bytes, ref offset);
                    if (preset is not null)
                    {
                        file.Presets.Add(preset);
                    }

                    if (i < declaredCount - 1 && offset < bytes.Length)
                    {
                        var nextHeaderLength = ReadInt32At(bytes, offset);
                        var looksLikeNextPreset = nextHeaderLength > 0
                                                  && (nextHeaderLength % 2) == 0
                                                  && offset + 4 + nextHeaderLength + 5 <= bytes.Length;
                        if (!looksLikeNextPreset)
                        {
                            var nextOffset = FindNextPresetOffset(bytes, offset);
                            if (nextOffset > offset)
                            {
                                file.ParseWarnings.Add($"Skipped unsupported preset residual bytes at 0x{offset:X}.");
                                Diagnostics.Warning("tpf_parse_warning", "Skipped unsupported preset residual bytes.", ("offset", offset), ("nextOffset", nextOffset), ("presetIndex", i));
                            }

                            offset = nextOffset;
                        }
                    }
                }
                catch (Exception ex) when (ex is InvalidDataException || ex is EndOfStreamException)
                {
                    file.ParseWarnings.Add($"Preset index {i} ignored: {ex.Message}");
                    Diagnostics.Warning("tpf_preset_rejected", ex.Message,
                        ("presetIndex", i),
                        ("presetStart", presetStart),
                        ("failureOffset", offset),
                        ("exceptionType", ex.GetType().Name));

                    var searchStart = Math.Min(Math.Max(offset, presetStart + 1), bytes.Length);
                    var nextOffset = FindNextPresetOffset(bytes, searchStart);
                    Diagnostics.Warning("tpf_parse_warning", "Preset recovery scan executed.",
                        ("presetIndex", i),
                        ("searchStart", searchStart),
                        ("nextOffset", nextOffset));
                    offset = nextOffset;
                }
            }

            if (file.Presets.Count == 0)
            {
                file.SelectedPresetIndex = 0;
            }
            else if (file.SelectedPresetIndex >= file.Presets.Count)
            {
                file.SelectedPresetIndex = (byte)Math.Max(0, file.Presets.Count - 1);
            }

            Diagnostics.Info("tpf_parse_completed",
                ("declaredPresetCount", declaredCount),
                ("parsedPresetCount", file.Presets.Count),
                ("warningCount", file.ParseWarnings.Count));
            return file;
        }

        public static void Save(string path, TpfFile file)
        {
            if (path is null)
            {
                throw new ArgumentNullException(nameof(path));
            }

            if (file is null)
            {
                throw new ArgumentNullException(nameof(file));
            }

            File.WriteAllBytes(path, Save(file));
            Diagnostics.Info("tpf_save_completed", ("path", path), ("presetCount", file.Presets.Count));
        }

        public static byte[] Save(TpfFile file)
        {
            if (file is null)
            {
                throw new ArgumentNullException(nameof(file));
            }

            Diagnostics.Info("tpf_save_start", ("presetCount", file.Presets.Count));
            using var ms = new MemoryStream();
            if (file.Presets.Count > 255)
            {
                Diagnostics.Warning("tpf_preset_count_truncated", "Preset count exceeds 255; extra presets will not be saved.",
                    ("totalCount", file.Presets.Count), ("savedCount", 255));
            }

            var serializedPresetCount = Math.Min(255, file.Presets.Count);
            var selectedPresetIndex = serializedPresetCount == 0
                ? 0
                : Math.Min(file.SelectedPresetIndex, (byte)(serializedPresetCount - 1));
            ms.WriteByte(file.Version);
            ms.WriteByte((byte)serializedPresetCount);
            ms.WriteByte(file.Reserved);
            ms.WriteByte((byte)selectedPresetIndex);

            for (var i = 0; i < serializedPresetCount; i++)
            {
                WritePreset(ms, file.Presets[i]);
            }

            return ms.ToArray();
        }

        private static TpfPreset? ReadPreset(byte[] bytes, ref int offset)
        {
            var name = ReadUtf16String(bytes, ref offset, required: true);

            if (offset >= bytes.Length)
            {
                throw new EndOfStreamException("Unexpected end of data while reading MAC mode.");
            }

            var macMode = bytes[offset++];
            if (!IsSupportedMode(macMode))
            {
                throw new InvalidDataException($"Unsupported MAC mode: 0x{macMode:X2}");
            }

            var customMac = string.Empty;
            var customMacLength = ReadInt32(bytes, ref offset);
            if ((TpfMacMode)macMode == TpfMacMode.Custom)
            {
                if (customMacLength <= 0)
                {
                    throw new InvalidDataException("Custom mode preset has invalid custom MAC length.");
                }

                customMac = ReadUtf16String(bytes, ref offset, customMacLength);
            }
            else if (customMacLength != 0)
            {
                throw new InvalidDataException($"Unexpected custom MAC payload ({customMacLength} bytes) for non-custom preset.");
            }

            var preset = new TpfPreset
            {
                Name = name,
                MacMode = (TpfMacMode)macMode,
                CustomMac = customMac
            };

            if (TryReadIpv4(bytes, ref offset, out var ipv4))
            {
                preset.Ipv4 = ipv4;
            }

            ValidatePreset(preset);
            return preset;
        }

        private static void ValidatePreset(TpfPreset preset)
        {
            if (preset.Name.Length > MaxPresetNameChars || HasControlCharacters(preset.Name))
            {
                throw new InvalidDataException("Preset name is invalid.");
            }

            if (preset.MacMode == TpfMacMode.Custom)
            {
                if (preset.CustomMac.Length > MaxCustomMacChars)
                {
                    throw new InvalidDataException("Custom MAC value is too long.");
                }

                var normalizedMac = (preset.CustomMac ?? string.Empty).Replace("-", string.Empty).Replace(":", string.Empty).Trim();
                if (!MacAddress.IsValidMac(normalizedMac.ToUpperInvariant()))
                {
                    throw new InvalidDataException("Custom MAC value is not valid.");
                }

                preset.CustomMac = string.Join("-", Enumerable.Range(0, 6).Select(i => normalizedMac.Substring(i * 2, 2).ToUpperInvariant()));
            }

            if (preset.Ipv4 is null || !preset.Ipv4.Enabled)
            {
                return;
            }

            ValidateIpv4Field(preset.Ipv4.Address, "IPv4 address", validateSubnetMask: false, required: preset.Ipv4.IsStatic);
            ValidateIpv4Field(preset.Ipv4.SubnetMask, "IPv4 subnet mask", validateSubnetMask: true, required: preset.Ipv4.IsStatic);
            ValidateIpv4Field(preset.Ipv4.DefaultGateway, "IPv4 default gateway", validateSubnetMask: false, required: preset.Ipv4.GatewayEnabled);
            ValidateIpv4Field(preset.Ipv4.PrimaryDnsServer, "IPv4 DNS server", validateSubnetMask: false, required: preset.Ipv4.DnsEnabled);
        }

        private static void ValidateIpv4Field(string value, string fieldName, bool validateSubnetMask, bool required)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                if (required)
                {
                    throw new InvalidDataException($"{fieldName} is required.");
                }

                return;
            }

            if (value.Length > MaxIpv4FieldChars || HasControlCharacters(value))
            {
                throw new InvalidDataException($"{fieldName} is invalid.");
            }

            var valid = validateSubnetMask
                ? IpAddressValidator.TryValidateIpv4SubnetMask(value, out _)
                : IpAddressValidator.TryValidateIpv4Address(value, out _);
            if (!valid)
            {
                throw new InvalidDataException($"{fieldName} is invalid.");
            }
        }

        private static bool HasControlCharacters(string value)
        {
            for (var i = 0; i < value.Length; i++)
            {
                if (char.IsControl(value[i]))
                {
                    return true;
                }
            }

            return false;
        }

        private static void WritePreset(Stream stream, TpfPreset preset)
        {
            if (preset is null)
            {
                throw new ArgumentNullException(nameof(preset));
            }

            WriteUtf16String(stream, preset.Name);
            stream.WriteByte((byte)preset.MacMode);

            if (preset.MacMode == TpfMacMode.Custom)
            {
                WriteInt32(stream, Utf16Le.GetByteCount(preset.CustomMac ?? string.Empty));
                WriteRawUtf16(stream, preset.CustomMac ?? string.Empty);
            }
            else
            {
                WriteInt32(stream, 0);
            }

            WriteIpv4(stream, preset.Ipv4);
        }

        private static bool TryReadIpv4(byte[] bytes, ref int offset, out TpfIpv4Settings? ipv4)
        {
            ipv4 = null;
            if (offset + 2 > bytes.Length)
            {
                return false;
            }

            var start = offset;
            var include = ReadUInt16(bytes, ref offset);
            if (include == 0)
            {
                return true;
            }

            if (include != 1)
            {
                offset = start;
                return false;
            }

            if (offset + 2 > bytes.Length)
            {
                offset = start;
                return false;
            }

            var staticFlag = ReadUInt16(bytes, ref offset);
            if (staticFlag != 0 && staticFlag != 1)
            {
                offset = start;
                return false;
            }

            var address = ReadUtf16String(bytes, ref offset, required: false);
            var subnetMask = ReadUtf16String(bytes, ref offset, required: false);

            var gatewayEnabled = ReadUInt16(bytes, ref offset);
            if (gatewayEnabled != 0 && gatewayEnabled != 1)
            {
                offset = start;
                return false;
            }

            var gateway = string.Empty;
            if (gatewayEnabled == 1)
            {
                gateway = ReadUtf16String(bytes, ref offset, required: false);
            }

            var metric = ReadInt32(bytes, ref offset);
            var dnsEnabled = ReadUInt16(bytes, ref offset);
            if (dnsEnabled != 0 && dnsEnabled != 1)
            {
                offset = start;
                return false;
            }

            var dns = string.Empty;
            if (dnsEnabled == 1)
            {
                dns = ReadUtf16String(bytes, ref offset, required: false);
            }

            ipv4 = new TpfIpv4Settings
            {
                Enabled = true,
                IsStatic = staticFlag == 1,
                Address = address,
                SubnetMask = subnetMask,
                GatewayEnabled = gatewayEnabled == 1,
                DefaultGateway = gateway,
                GatewayMetric = metric,
                DnsEnabled = dnsEnabled == 1,
                PrimaryDnsServer = dns
            };

            return true;
        }

        private static void WriteIpv4(Stream stream, TpfIpv4Settings? ipv4)
        {
            if (ipv4 is null || !ipv4.Enabled)
            {
                WriteUInt16(stream, 0);
                return;
            }

            WriteUInt16(stream, 1);
            WriteUInt16(stream, ipv4.IsStatic ? (ushort)1 : (ushort)0);
            WriteUtf16String(stream, ipv4.Address ?? string.Empty);
            WriteUtf16String(stream, ipv4.SubnetMask ?? string.Empty);
            WriteUInt16(stream, ipv4.GatewayEnabled ? (ushort)1 : (ushort)0);
            if (ipv4.GatewayEnabled)
            {
                WriteUtf16String(stream, ipv4.DefaultGateway ?? string.Empty);
            }

            WriteInt32(stream, ipv4.GatewayMetric);
            WriteUInt16(stream, ipv4.DnsEnabled ? (ushort)1 : (ushort)0);
            if (ipv4.DnsEnabled)
            {
                WriteUtf16String(stream, ipv4.PrimaryDnsServer ?? string.Empty);
            }
        }

        private static bool IsSupportedMode(byte mode)
        {
            return mode == (byte)TpfMacMode.Random
                   || mode == (byte)TpfMacMode.Original
                   || mode == (byte)TpfMacMode.Custom
                   || mode == (byte)TpfMacMode.RandomWith02;
        }

        private static int FindNextPresetOffset(byte[] data, int searchStart)
        {
            var minimumBytesForPreset = 4 + 2 + 1 + 4;
            var maxStart = data.Length - minimumBytesForPreset;

            for (var candidate = Math.Max(searchStart, 4); candidate <= maxStart; candidate++)
            {
                var nameByteLength = ReadInt32At(data, candidate);
                if (nameByteLength <= 0 || (nameByteLength % 2) != 0)
                {
                    continue;
                }

                var nameStart = candidate + 4;
                var nameEnd = nameStart + nameByteLength;
                if (nameEnd + 5 > data.Length)
                {
                    continue;
                }

                if (!LooksMostlyUtf16LeText(data, nameStart, nameByteLength))
                {
                    continue;
                }

                return candidate;
            }

            return data.Length;
        }

        private static int ReadInt32At(byte[] data, int offset)
        {
            if (offset + 4 > data.Length)
            {
                return 0;
            }

            return data[offset]
                | (data[offset + 1] << 8)
                | (data[offset + 2] << 16)
                | (data[offset + 3] << 24);
        }

        private static bool LooksMostlyUtf16LeText(byte[] data, int offset, int byteLength)
        {
            if (byteLength < 2 || (byteLength % 2) != 0 || offset + byteLength > data.Length)
            {
                return false;
            }

            var chars = byteLength / 2;
            var plausible = 0;
            for (var i = 0; i < chars; i++)
            {
                var lo = data[offset + (i * 2)];
                var hi = data[offset + (i * 2) + 1];
                if (hi != 0x00)
                {
                    return false;
                }

                var c = (char)lo;
                if (char.IsLetterOrDigit(c) || c == ' ' || c == '-' || c == '_' || c == '.' || c == '(' || c == ')')
                {
                    plausible++;
                }
            }

            return plausible >= Math.Max(3, (chars * 3) / 4);
        }

        private static string ReadUtf16String(byte[] bytes, ref int offset, bool required)
        {
            var byteLength = ReadInt32(bytes, ref offset);
            return ReadUtf16String(bytes, ref offset, byteLength, required);
        }

        private static string ReadUtf16String(byte[] bytes, ref int offset, int byteLength, bool required = false)
        {
            if (byteLength < 0 || (byteLength % 2) != 0)
            {
                throw new InvalidDataException($"Invalid UTF-16 byte length: {byteLength}");
            }

            if (required && byteLength == 0)
            {
                throw new InvalidDataException("Required UTF-16 string payload was empty.");
            }

            if (offset + byteLength > bytes.Length)
            {
                throw new EndOfStreamException("Unexpected end of data while reading UTF-16 string.");
            }

            var value = Utf16Le.GetString(bytes, offset, byteLength);
            offset += byteLength;
            return value;
        }

        private static int ReadInt32(byte[] bytes, ref int offset)
        {
            if (offset + 4 > bytes.Length)
            {
                throw new EndOfStreamException("Unexpected end of data while reading Int32.");
            }

            var value = bytes[offset]
                        | (bytes[offset + 1] << 8)
                        | (bytes[offset + 2] << 16)
                        | (bytes[offset + 3] << 24);
            offset += 4;
            return value;
        }

        private static ushort ReadUInt16(byte[] bytes, ref int offset)
        {
            if (offset + 2 > bytes.Length)
            {
                throw new EndOfStreamException("Unexpected end of data while reading UInt16.");
            }

            var value = (ushort)(bytes[offset] | (bytes[offset + 1] << 8));
            offset += 2;
            return value;
        }

        private static void WriteInt32(Stream stream, int value)
        {
            stream.WriteByte((byte)(value & 0xFF));
            stream.WriteByte((byte)((value >> 8) & 0xFF));
            stream.WriteByte((byte)((value >> 16) & 0xFF));
            stream.WriteByte((byte)((value >> 24) & 0xFF));
        }

        private static void WriteUInt16(Stream stream, ushort value)
        {
            stream.WriteByte((byte)(value & 0xFF));
            stream.WriteByte((byte)((value >> 8) & 0xFF));
        }

        private static void WriteUtf16String(Stream stream, string value)
        {
            var safe = value ?? string.Empty;
            var bytes = Utf16Le.GetBytes(safe);
            WriteInt32(stream, bytes.Length);
            stream.Write(bytes, 0, bytes.Length);
        }

        private static void WriteRawUtf16(Stream stream, string value)
        {
            var bytes = Utf16Le.GetBytes(value);
            stream.Write(bytes, 0, bytes.Length);
        }
    }
}
