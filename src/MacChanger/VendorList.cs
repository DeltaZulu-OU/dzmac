#nullable enable
using System;
using System.Collections;
using System.Collections.Generic;

namespace MacChanger
{
    /// <summary>
    ///     A persistent readonly key-value store for vendors.
    ///     The data is downloaded from IEEE OUI list.
    /// </summary>
    public class VendorList : IDisposable, IReadOnlyList<Vendor>
    {
        public int Count => _cache.Count;
        private const string _databaseFile = "oui.db";
        private static Cache? _cache;
        private bool disposedValue;

        internal VendorList()
        {
            _cache = new Cache(_databaseFile);
            if (_cache.IsEmpty)
            {
                _cache.AddRange(Downloader.GetAll());
            }
        }
        ///  <inheritdoc/>
        public Vendor? this[string oui] => Get(oui);
        ///  <inheritdoc/>
        public Vendor? this[int index] => _cache[index];
        Vendor IReadOnlyList<Vendor>.this[int index] => _cache[index] ?? default;
        public Vendor? Get(string oui, bool useWildcard = false) => _cache.Get(oui, useWildcard);

        ///  <inheritdoc/>
        public IEnumerator<Vendor> GetEnumerator() => _cache.GetAll().GetEnumerator();

        /// <summary>
        ///     Downloads data from IEEE and writes to DB
        /// </summary>
        /// <exception cref="MacChangerException"></exception>
        public void Refresh()
        {
            // There must not be a possibility of empty cache but t is better to check
            if (_cache is null) throw new MacChangerException("Cache object does not exist");

            var downloaded = Downloader.GetAll();
            if (!_cache.IsEmpty)
            {
                _cache.Clear();
            }
            _cache.AddRange(downloaded);
        }

        /// <summary>
        ///     Checks if a vendor with the provided OUI exists.
        ///     There may be more than one vendors as OUIs are not unique.
        ///     Hence, it returns an enumerable.
        /// </summary>
        /// <param name="oui">IEEE assigned OUI</param>
        /// <param name="vendors">Matched vendors from IEEE records</param>
        /// <returns>If OUI exists in the IEEE database</returns>
        public bool TryGetValue(string oui, out Vendor? vendors, bool useWildcard = false)
        {
            vendors = Get(oui, useWildcard);
            return vendors != null;
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
                    _cache?.Dispose();
                }

                _cache = null;
                disposedValue = true;
            }
        }
        #endregion Dispose

        ///  <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator() => _cache.GetAll().GetEnumerator();
    }
}
