#nullable enable

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Dzmac.Gui.Core
{
    internal sealed class Cache : IDisposable
    {
        private static readonly Regex OuiPattern = new Regex("^[0-9A-F]{6}$");

        private readonly string _cacheFile;
        private readonly Lazy<List<Vendor>> _records;

        public int Count => Records.Count;

        public Vendor? this[int index] => GetByIndex(index);

        public Cache(string cacheFile)
        {
            _cacheFile = cacheFile ?? throw new ArgumentNullException(nameof(cacheFile));
            _records = new Lazy<List<Vendor>>(() => Load(_cacheFile), true);
        }

        public Vendor? this[string oui] => Get(oui);

        public void Add(string oui, string vendor)
        {
            Debug.WriteLine($"Updating cache (OUI: {oui}, Vendor: {vendor})...");
            Records.Add(CreateNormalizedVendor(oui, vendor));
            Save();
        }

        public void AddRange(IEnumerable<Vendor> vendors)
        {
            if (vendors == null)
            {
                throw new ArgumentNullException(nameof(vendors));
            }

            Debug.WriteLine("Populating cache...");
            Records.AddRange(vendors.Select(v => CreateNormalizedVendor(v.Oui, v.VendorName)));
            Save();
        }

        public void ReplaceAll(IEnumerable<Vendor> vendors)
        {
            if (vendors == null)
            {
                throw new ArgumentNullException(nameof(vendors));
            }

            Debug.WriteLine("Replacing cache content...");
            Records.Clear();
            Records.AddRange(vendors.Select(v => CreateNormalizedVendor(v.Oui, v.VendorName)));

            Save();
        }

        public Vendor Get(string oui, bool useWildcard = false)
        {
            var normalizedOui = NormalizeOui(oui);
            Debug.WriteLine($"Querying cache (OUI: {normalizedOui})...");

            return useWildcard
                ? Records.FirstOrDefault(v => MatchesWildcard(v.Oui, normalizedOui))
                : Records.FirstOrDefault(v => v.Oui == normalizedOui);
            
        }

        public IEnumerable<Vendor> GetAll()
        {
            Debug.WriteLine("Querying cache (ALL)...");
            return Records;
        }

        public void Clear()
        {
            Debug.WriteLine("Clearing cache...");
            Records.Clear();
            Save();
        }

        public bool IsEmpty
        {
            get
            {
                Debug.WriteLine("Querying cache if empty...");
                return Records.Count == 0;
            }
        }

        private Vendor? GetByIndex(int index)
        {
            Debug.WriteLine($"Querying cache (index: {index})...");

            if (index < 0 || index >= Count)
            {
                throw new IndexOutOfRangeException();
            }

            return Records[index];
        }

        private List<Vendor> Records => _records.Value;

        private static bool MatchesWildcard(string candidate, string pattern)
        {
            return candidate.Length == 6
                   && pattern.Length == 6
                   && candidate[0] == pattern[0]
                   && string.Equals(candidate.Substring(2), pattern.Substring(2), StringComparison.Ordinal);
        }

        private static Vendor CreateNormalizedVendor(string oui, string vendor)
        {
            return new Vendor(NormalizeOui(oui), NormalizeVendorName(vendor));
        }

        private static string NormalizeOui(string oui)
        {
            if (oui == null)
            {
                throw new ArgumentNullException(nameof(oui));
            }

            var normalized = oui.Trim().TrimStart('\uFEFF').ToUpperInvariant();
            if (!OuiPattern.IsMatch(normalized))
            {
                throw new ArgumentException(nameof(oui));
            }

            return normalized;
        }

        private static string NormalizeVendorName(string vendor)
        {
            if (vendor == null)
            {
                return string.Empty;
            }

            var sanitized = vendor.Replace("\r", string.Empty).Replace("\n", " ").Trim();
            sanitized = new string(sanitized
                .Where(c => !char.IsControl(c) || c == '\t')
                .Where(c => c != '\uFFFD')
                .ToArray());
            return sanitized;
        }

        private void Save()
        {
            var directory = Path.GetDirectoryName(_cacheFile);
            if (!string.IsNullOrWhiteSpace(directory))
            {
                Directory.CreateDirectory(directory);
            }

            using var writer = new StreamWriter(_cacheFile, false, Encoding.UTF8);
            foreach (var record in Records)
            {
                writer.Write(Escape(record.Oui));
                writer.Write(',');
                writer.Write(Escape(NormalizeVendorName(record.VendorName)));
                writer.WriteLine();
            }
        }

        private static List<Vendor> Load(string cacheFile)
        {
            if (!File.Exists(cacheFile))
            {
                return new List<Vendor>();
            }

            var records = new List<Vendor>();
            foreach (var line in ReadLinesWithEncodingFallback(cacheFile))
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    continue;
                }

                var fields = ParseRow(line);
                if (fields.Count < 2)
                {
                    continue;
                }

                if (!TryParseVendorRow(fields, out var vendor))
                {
                    continue;
                }

                records.Add(vendor);
            }

            return records;
        }

        private static bool TryParseVendorRow(IReadOnlyList<string> fields, out Vendor vendor)
        {
            vendor = default;
            if (fields == null || fields.Count < 2)
            {
                return false;
            }

            if (TryExtractOui(fields[0], out var oui))
            {
                var vendorName = fields.Count == 2 ? fields[1] : string.Join(",", fields.Skip(1));
                vendor = new Vendor(oui, NormalizeVendorName(vendorName));
                return true;
            }

            // IEEE CSV format: Registry,Assignment,Organization Name,...
            if (fields.Count >= 3 && TryExtractOui(fields[1], out oui))
            {
                vendor = new Vendor(oui, NormalizeVendorName(fields[2]));
                return true;
            }

            return false;
        }

        private static bool TryExtractOui(string raw, out string oui)
        {
            oui = string.Empty;
            if (string.IsNullOrWhiteSpace(raw))
            {
                return false;
            }

            var compact = new string(raw.Trim().TrimStart('﻿').Where(Uri.IsHexDigit).ToArray()).ToUpperInvariant();
            if (compact.Length != 6 || !OuiPattern.IsMatch(compact))
            {
                return false;
            }

            oui = compact;
            return true;
        }

        private static IEnumerable<string> ReadLinesWithEncodingFallback(string cacheFile)
        {
            try
            {
                var lines = new List<string>();
                using var reader = new StreamReader(cacheFile, new UTF8Encoding(false, true));
                string? line;
                while ((line = reader.ReadLine()) != null)
                {
                    lines.Add(line);
                }

                return lines;
            }
            catch (DecoderFallbackException)
            {
                return File.ReadAllLines(cacheFile, Encoding.Default);
            }
        }

        private static string Escape(string value)
        {
            value = value ?? string.Empty;
            return $"\"{value.Replace("\"", "\"\"").Replace("\r", string.Empty)}\"";
        }

        private static List<string> ParseRow(string line)
        {
            var values = new List<string>();
            var current = new StringBuilder();
            var inQuotes = false;

            for (var index = 0; index < line.Length; index++)
            {
                var item = line[index];
                if (item == '"')
                {
                    if (inQuotes && index + 1 < line.Length && line[index + 1] == '"')
                    {
                        current.Append('"');
                        index++;
                    }
                    else
                    {
                        inQuotes = !inQuotes;
                    }

                    continue;
                }

                if (item == ',' && !inQuotes)
                {
                    values.Add(current.ToString());
                    current.Clear();
                    continue;
                }

                current.Append(item);
            }

            values.Add(current.ToString());
            return values;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
