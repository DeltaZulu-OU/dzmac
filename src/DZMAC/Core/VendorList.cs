#nullable enable

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Dzmac.Gui.Core
{
    /// <summary>
    ///     A persistent readonly key-value store for vendor.
    ///     The data is downloaded from IEEE OUI list.
    /// </summary>
    public class VendorList : IDisposable, IReadOnlyList<Vendor>
    {
        private const string DatabaseFileName = "oui.csv";

        public int Count => _cache.Count;

        private readonly Cache _cache;
        private bool disposedValue;

        internal VendorList()
        {
            var databasePath = ResolveDatabasePath();
            _cache = new Cache(databasePath);

            Diagnostics.Info("vendor_cache_initialized", ("databasePath", databasePath), ("isEmpty", _cache.IsEmpty));
        }

        ///  <inheritdoc/>
        public Vendor? this[string oui] => Get(oui);

        ///  <inheritdoc/>
        public Vendor? this[int index] => _cache[index];

        Vendor IReadOnlyList<Vendor>.this[int index] => _cache[index] ?? default;

        public Vendor Get(string oui, bool useWildcard = false) => _cache.Get(oui, useWildcard);

        ///  <inheritdoc/>
        public IEnumerator<Vendor> GetEnumerator() => _cache.GetAll().GetEnumerator();

        /// <summary>
        ///     Downloads data from IEEE and writes to DB asynchronously.
        /// </summary>
        /// <exception cref="Dzmac.Gui.CoreException"></exception>
        public async Task RefreshAsync(CancellationToken cancellationToken)
        {
            // There must not be a possibility of empty cache but t is better to check
            if (_cache is null) throw new DZMACException("Cache object does not exist");

            Diagnostics.Info("vendor_refresh_started", ("cacheWasEmpty", _cache.IsEmpty));

            try
            {
                var downloaded = await Downloader.GetAllAsync(cancellationToken).ConfigureAwait(false);
                _cache.ReplaceAll(downloaded);

                Diagnostics.Info("vendor_cache_refreshed", ("recordCount", _cache.Count));
                Diagnostics.Info("vendor_refresh_succeeded", ("recordCount", _cache.Count));
            }
            catch (Exception ex)
            {
                Diagnostics.Error("vendor_refresh_failed", ex, "Failed to refresh vendor list from IEEE.");
                throw;
            }
        }

        /// <summary>
        ///     Checks if a vendor with the provided OUI exists.
        ///     There may be more than one vendor as OUIs are not unique.
        ///     Hence, it returns an enumerable.
        /// </summary>
        /// <param name="oui">IEEE assigned OUI</param>
        /// <param name="vendor">Matched vendor from IEEE records</param>
        /// <returns>If OUI exists in the IEEE database</returns>
        public bool TryGetValue(string oui, out Vendor vendor, bool useWildcard = false)
        {
            vendor = Get(oui, useWildcard);
            return !vendor.Equals(default);
        }

        private static string ResolveDatabasePath()
        {
            var envPath = Environment.GetEnvironmentVariable("Dzmac.Gui.Core_OUI_CACHE_PATH");
            var configuredPath = ConfigReader.Current.GetString(AppSettingKeys.OuiCachePath);
            var path = FirstNonEmpty(envPath, configuredPath);

            if (string.IsNullOrWhiteSpace(path))
            {
                var basePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Dzmac.Gui.Core");
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
                if (!string.IsNullOrWhiteSpace(value))
                {
                    return value;
                }
            }

            return null;
        }

        #region Dispose

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _cache.Dispose();
                }

                disposedValue = true;
            }
        }

        #endregion Dispose

        ///  <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator() => _cache.GetAll().GetEnumerator();
    }
}