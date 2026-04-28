using System;
using System.Collections.Generic;

namespace Dzmac.Core.Presets
{
    internal enum TpfMacMode : byte
    {
        Random = 0x01,
        Original = 0x02,
        Custom = 0x03,
        RandomWith02 = 0x04
    }

    internal sealed class TpfIpv4Settings
    {
        public bool Enabled { get; set; }
        public bool IsStatic { get; set; }
        public string Address { get; set; } = string.Empty;
        public string SubnetMask { get; set; } = string.Empty;
        public bool GatewayEnabled { get; set; }
        public string DefaultGateway { get; set; } = string.Empty;
        public int GatewayMetric { get; set; }
        public bool DnsEnabled { get; set; }
        public string PrimaryDnsServer { get; set; } = string.Empty;
    }

    internal sealed class TpfPreset
    {
        public string Name { get; set; } = string.Empty;
        public TpfMacMode MacMode { get; set; }
        public string CustomMac { get; set; } = string.Empty;
        public TpfIpv4Settings? Ipv4 { get; set; }
    }

    internal sealed class TpfFile
    {
        public byte Version { get; set; } = 0x01;
        public byte Reserved { get; set; }
        public byte SelectedPresetIndex { get; set; }
        public List<TpfPreset> Presets { get; } = new List<TpfPreset>();
        public List<string> ParseWarnings { get; } = new List<string>();
    }

    internal static class TpfPresetFormatter
    {
        public static string ToMacDisplay(TpfPreset preset)
        {
            if (preset is null)
            {
                throw new ArgumentNullException(nameof(preset));
            }

            switch (preset.MacMode)
            {
                case TpfMacMode.Random:
                    return "Use Random";
                case TpfMacMode.Original:
                    return "Use Original";
                case TpfMacMode.Custom:
                    return $"Use Custom ({preset.CustomMac})";
                case TpfMacMode.RandomWith02:
                    return "Use Random (with 0x02 as first octet)";
                default:
                    return $"Unknown mode 0x{((byte)preset.MacMode):X2}";
            }
        }

        public static IEnumerable<KeyValuePair<string, string>> ToProperties(TpfPreset preset)
        {
            if (preset is null)
            {
                throw new ArgumentNullException(nameof(preset));
            }

            yield return new KeyValuePair<string, string>("MAC Address", ToMacDisplay(preset));

            if (preset.Ipv4 is not null && preset.Ipv4.Enabled)
            {
                if (!string.IsNullOrWhiteSpace(preset.Ipv4.Address) || !string.IsNullOrWhiteSpace(preset.Ipv4.SubnetMask))
                {
                    yield return new KeyValuePair<string, string>("IPv4 Address", $"{preset.Ipv4.Address} ({preset.Ipv4.SubnetMask})");
                }

                if (preset.Ipv4.GatewayEnabled && !string.IsNullOrWhiteSpace(preset.Ipv4.DefaultGateway))
                {
                    yield return new KeyValuePair<string, string>("IPv4 Default Gateway", $"{preset.Ipv4.DefaultGateway} ({preset.Ipv4.GatewayMetric})");
                }

                if (preset.Ipv4.DnsEnabled && !string.IsNullOrWhiteSpace(preset.Ipv4.PrimaryDnsServer))
                {
                    yield return new KeyValuePair<string, string>("IPv4 DNS Server", preset.Ipv4.PrimaryDnsServer);
                }
            }
        }
    }
}
