#nullable enable

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace Dzmac.Gui.Core
{
    /// <summary>
    ///     A persistent key-value store for vendor data downloaded from the IEEE OUI list.
    ///     Handles CSV storage, OUI lookups, and async refresh coordination.
    /// </summary>
    public class VendorList : IDisposable, IReadOnlyList<Vendor>
    {
        private const string DatabaseFileName = "oui.csv";

        private static readonly Regex OuiPattern = new Regex("^[0-9A-F]{6}$");

        private readonly string _csvPath;
        private readonly object _sync = new object();
        private readonly object _refreshSync = new object();
        private readonly Random _random = new Random();
        private CancellationTokenSource? _refreshCancellation;
        private List<Vendor>? _records;
        private bool _disposedValue;

        public VendorList(string? csvPath = null)
        {
            _csvPath = csvPath ?? ResolveDatabasePath();
            Diagnostics.Info("vendor_list_initialized", ("csvPath", _csvPath), ("isEmpty", IsEmpty));
        }

        // ── IReadOnlyList<Vendor> ────────────────────────────────────────────────

        public int Count => Records.Count;

        Vendor IReadOnlyList<Vendor>.this[int index] => GetByIndex(index) ?? default;

        public IEnumerator<Vendor> GetEnumerator() => Records.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => Records.GetEnumerator();

        // ── Public API ──────────────────────────────────────────────────────────

        public bool IsEmpty => Records.Count == 0;

        public Vendor? this[int index] => GetByIndex(index);

        public Vendor? this[string oui] => Get(oui);

        public Vendor Get(string oui, bool useWildcard = false)
        {
            var normalized = NormalizeOui(oui);
            Debug.WriteLine($"Querying vendor list (OUI: {normalized})...");
            var records = Records;
            return useWildcard
                ? records.FirstOrDefault(v => MatchesWildcard(v.Oui, normalized))
                : records.FirstOrDefault(v => v.Oui == normalized);
        }

        public bool TryGetValue(string oui, out Vendor vendor, bool useWildcard = false)
        {
            vendor = Get(oui, useWildcard);
            return !vendor.Equals(default);
        }

        public Vendor? FindByMac(string macAddress, bool useWildcard = false)
            => Get(macAddress.Substring(0, 6), useWildcard);

        public Vendor? FindByMac(MacAddress macAddress, bool useWildcard = false)
            => FindByMac(macAddress.ToString(), useWildcard);

        public Vendor GetRandom()
        {
            var records = Records;
            if (records.Count == 0)
            {
                throw new DZMACException("Vendor list is empty. Update the OUI list from the About menu.");
            }

            Vendor? selected = null;
            while (selected == null)
            {
                selected = GetByIndex(_random.Next(records.Count));
            }

            return selected.Value;
        }

        public void Refresh() => RefreshAsync(CancellationToken.None).GetAwaiter().GetResult();

        public Task RefreshAsync(CancellationToken cancellationToken)
        {
            lock (_refreshSync)
            {
                _refreshCancellation?.Cancel();
                _refreshCancellation?.Dispose();
                _refreshCancellation = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
                var token = _refreshCancellation.Token;
                return RefreshCoreAsync(token);
            }
        }

        /// <summary>
        ///     Adds normalized vendors to the in-memory list and persists to CSV.
        ///     Normalization (sanitization) is applied here, making it safe to call after download.
        /// </summary>
        public void AddRange(IEnumerable<Vendor> vendors)
        {
            if (vendors == null) throw new ArgumentNullException(nameof(vendors));

            Debug.WriteLine("Adding to vendor list...");
            var records = Records;
            var seenOui = new HashSet<string>(records.Select(v => v.Oui), StringComparer.OrdinalIgnoreCase);

            foreach (var input in vendors)
            {
                var normalized = CreateNormalizedVendor(input.Oui, input.VendorName);

                if (string.IsNullOrWhiteSpace(normalized.VendorName))
                {
                    Debug.WriteLine($"Skipping vendor with empty name for OUI '{normalized.Oui}' during AddRange.");
                    continue;
                }

                if (!seenOui.Add(normalized.Oui))
                {
                    Debug.WriteLine($"Skipping duplicate vendor with OUI '{normalized.Oui}' during AddRange.");
                    continue;
                }

                records.Add(normalized);
            }

            Save(records);
        }

        /// <summary>
        ///     Replaces all records with the provided vendors after normalizing them, then persists to CSV.
        ///     Normalization (sanitization) is applied here — not when loading back from CSV.
        /// </summary>
        public void ReplaceAll(IEnumerable<Vendor> vendors)
        {
            if (vendors == null) throw new ArgumentNullException(nameof(vendors));

            Debug.WriteLine("Replacing vendor list content...");
            var newList = new List<Vendor>();
            var seenOui = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            foreach (var input in vendors)
            {
                var normalized = CreateNormalizedVendor(input.Oui, input.VendorName);

                if (string.IsNullOrWhiteSpace(normalized.VendorName))
                {
                    Debug.WriteLine($"Skipping vendor with empty name for OUI '{normalized.Oui}' during ReplaceAll.");
                    continue;
                }

                if (!seenOui.Add(normalized.Oui))
                {
                    Debug.WriteLine($"Skipping duplicate vendor with OUI '{normalized.Oui}' during ReplaceAll.");
                    continue;
                }

                newList.Add(normalized);
            }

            lock (_sync)
            {
                _records = newList;
            }

            Save(newList);
            Diagnostics.Info("vendor_list_replaced", ("recordCount", newList.Count));
        }

        public void Clear()
        {
            Debug.WriteLine("Clearing vendor list...");
            var records = Records;
            lock (_sync)
            {
                records.Clear();
            }

            Save(records);
        }

        // ── Private implementation ───────────────────────────────────────────────

        private async Task RefreshCoreAsync(CancellationToken cancellationToken)
        {
            Diagnostics.Info("vendor_refresh_started", ("wasEmpty", IsEmpty));
            try
            {
                var downloaded = await Downloader.GetAllAsync(cancellationToken).ConfigureAwait(false);
                ReplaceAll(downloaded);
                Diagnostics.Info("vendor_refresh_succeeded", ("recordCount", Count));
            }
            catch (OperationCanceledException)
            {
                Diagnostics.Warning("vendor_refresh_cancelled", "Vendor refresh was cancelled.");
                throw;
            }
            catch (Exception ex)
            {
                Diagnostics.Error("vendor_refresh_failed", ex, "Failed to refresh vendor list from IEEE.");
                throw;
            }
        }

        private Vendor? GetByIndex(int index)
        {
            var records = Records;
            if (index < 0 || index >= records.Count)
            {
                throw new IndexOutOfRangeException();
            }

            return records[index];
        }

        private List<Vendor> Records
        {
            get
            {
                lock (_sync)
                {
                    return _records ??= Load(_csvPath);
                }
            }
        }

        private static bool MatchesWildcard(string candidate, string pattern) => candidate.Length == 6
                   && pattern.Length == 6
                   && candidate[0] == pattern[0]
                   && string.Equals(candidate.Substring(2), pattern.Substring(2), StringComparison.Ordinal);

        private static Vendor CreateNormalizedVendor(string oui, string vendor)
            => new Vendor(NormalizeOui(oui), NormalizeVendorName(vendor));

        private static string NormalizeOui(string oui)
        {
            if (oui == null) throw new ArgumentNullException(nameof(oui));

            var normalized = oui.Trim().TrimStart('\uFEFF').ToUpperInvariant();
            if (!OuiPattern.IsMatch(normalized))
            {
                throw new ArgumentException(nameof(oui));
            }

            return normalized;
        }

        private static string NormalizeVendorName(string vendor)
        {
            if (vendor == null) return string.Empty;

            var sanitized = vendor.Replace("\r", string.Empty).Replace("\n", " ").Trim();
            sanitized = new string(sanitized
                .Where(c => !char.IsControl(c) || c == '\t')
                .Where(c => c != '\uFFFD')
                .ToArray());
            return sanitized;
        }

        // ── CSV persistence ──────────────────────────────────────────────────────

        private void Save(List<Vendor> records)
        {
            var directory = Path.GetDirectoryName(_csvPath);
            if (!string.IsNullOrWhiteSpace(directory))
            {
                Directory.CreateDirectory(directory);
            }

            using var writer = new StreamWriter(_csvPath, false, Encoding.UTF8);
            foreach (var record in records)
            {
                writer.Write(Escape(record.Oui));
                writer.Write(',');
                writer.Write(Escape(record.VendorName));
                writer.WriteLine();
            }
        }

        private static List<Vendor> Load(string csvPath)
        {
            if (!File.Exists(csvPath))
            {
                return new List<Vendor>();
            }

            var records = new List<Vendor>();
            var seenOui = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            foreach (var line in ReadLinesWithEncodingFallback(csvPath))
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    continue;
                }

                var fields = ParseRow(line);
                if (fields.Count < 2)
                {
                    Debug.WriteLine($"Skipping vendor row with too few columns: '{line}'");
                    continue;
                }

                if (!TryParseVendorRow(fields, out var vendor, out var reason))
                {
                    Debug.WriteLine($"Skipping vendor row: {reason}. Raw: '{line}'");
                    continue;
                }

                if (!seenOui.Add(vendor.Oui))
                {
                    Debug.WriteLine($"Skipping duplicate vendor row for OUI '{vendor.Oui}'. Raw: '{line}'");
                    continue;
                }

                records.Add(vendor);
            }

            return records;
        }

        private static bool TryParseVendorRow(IReadOnlyList<string> fields, out Vendor vendor, out string reason)
        {
            vendor = default;
            reason = string.Empty;

            if (fields == null || fields.Count < 2)
            {
                reason = "too few columns";
                return false;
            }

            if (TryExtractOui(fields[0], out var oui))
            {
                var vendorName = fields.Count == 2 ? fields[1] : string.Join(",", fields.Skip(1));
                vendorName = NormalizeVendorName(vendorName);

                if (string.IsNullOrWhiteSpace(vendorName))
                {
                    reason = $"empty vendor name for OUI '{oui}'";
                    return false;
                }

                vendor = new Vendor(oui, vendorName);
                return true;
            }

            if (fields.Count >= 3 && TryExtractOui(fields[1], out oui))
            {
                var vendorName = NormalizeVendorName(fields[2]);

                if (string.IsNullOrWhiteSpace(vendorName))
                {
                    reason = $"empty vendor name for OUI '{oui}'";
                    return false;
                }

                vendor = new Vendor(oui, vendorName);
                return true;
            }

            reason = "no valid OUI found";
            return false;
        }

        private static bool TryExtractOui(string raw, out string oui)
        {
            oui = string.Empty;
            if (string.IsNullOrWhiteSpace(raw)) return false;

            var compact = new string(raw.Trim().TrimStart('﻿').Where(Uri.IsHexDigit).ToArray()).ToUpperInvariant();
            if (compact.Length != 6 || !OuiPattern.IsMatch(compact)) return false;

            oui = compact;
            return true;
        }

        private static IEnumerable<string> ReadLinesWithEncodingFallback(string csvPath)
        {
            try
            {
                var lines = new List<string>();
                using var reader = new StreamReader(csvPath, new UTF8Encoding(false, true));
                string? line;
                while ((line = reader.ReadLine()) != null)
                {
                    lines.Add(line);
                }

                return lines;
            }
            catch (DecoderFallbackException)
            {
                return File.ReadAllLines(csvPath, Encoding.Default);
            }
        }

        private static string Escape(string value)
        {
            value ??= string.Empty;
            return $"\"{value.Replace("\"", "\"\"").Replace("\r", string.Empty)}\"";
        }

        private static List<string> ParseRow(string line)
        {
            var values = new List<string>();
            var current = new StringBuilder();
            var inQuotes = false;

            for (var i = 0; i < line.Length; i++)
            {
                var c = line[i];
                if (c == '"')
                {
                    if (inQuotes && i + 1 < line.Length && line[i + 1] == '"')
                    {
                        current.Append('"');
                        i++;
                    }
                    else
                    {
                        inQuotes = !inQuotes;
                    }
                    continue;
                }

                if (c == ',' && !inQuotes)
                {
                    values.Add(current.ToString());
                    current.Clear();
                    continue;
                }

                current.Append(c);
            }

            values.Add(current.ToString());
            return values;
        }

        // ── Path resolution ──────────────────────────────────────────────────────

        private static string ResolveDatabasePath()
        {
            var envPath = Environment.GetEnvironmentVariable("Dzmac.Gui.Core_OUI_CACHE_PATH");
            var configuredPath = ConfigReader.Current.GetString(AppSettingKeys.OuiCachePath);
            var path = FirstNonEmpty(envPath, configuredPath);

            if (string.IsNullOrWhiteSpace(path))
            {
                var basePath = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                    "Dzmac.Gui.Core");
                path = Path.Combine(basePath, DatabaseFileName);
            }

            var fullPath = Path.GetFullPath(path);
            var directory = Path.GetDirectoryName(fullPath);

            if (string.IsNullOrWhiteSpace(directory))
            {
                throw new DZMACException("Resolved OUI cache path has no parent directory.");
            }

            try
            {
                Directory.CreateDirectory(directory);
            }
            catch (Exception ex)
            {
                throw new DZMACException($"Failed to prepare OUI cache directory: {directory}", ex);
            }

            return fullPath;
        }

        private static string? FirstNonEmpty(params string?[] values)
        {
            foreach (var value in values)
            {
                if (!string.IsNullOrWhiteSpace(value)) return value;
            }
            return null;
        }

        // ── Dispose ──────────────────────────────────────────────────────────────

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    lock (_refreshSync)
                    {
                        _refreshCancellation?.Cancel();
                        _refreshCancellation?.Dispose();
                        _refreshCancellation = null;
                    }
                }
                _disposedValue = true;
            }
        }
    }
}
