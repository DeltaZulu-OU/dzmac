#nullable enable
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace MacChanger
{
    /// <summary>
    ///     A persistent readonly key-value store for vendors.
    ///     The data is downloaded from IEEE OUI list.
    /// </summary>
    public class VendorList : IDisposable, IReadOnlyList<Vendor>
    {
        private const string _databaseFile = "oui.db";
        private static Cache? _cache;
        private bool disposedValue;

#pragma warning disable CS8602 // Dereference of a possibly null reference.
        public int Count => _cache.Count;
#pragma warning restore CS8602 // Dereference of a possibly null reference.

#pragma warning disable CS8602 // Dereference of a possibly null reference.
        Vendor IReadOnlyList<Vendor>.this[int index] => _cache[index];
#pragma warning restore CS8602 // Dereference of a possibly null reference.

#pragma warning disable CS8602 // Dereference of a possibly null reference.
        public Vendor this[int index] => _cache[index];
#pragma warning restore CS8602 // Dereference of a possibly null reference.

        public IEnumerable<Vendor> this[string oui] => Get(oui);

        internal VendorList()
        {
            _cache = new Cache(_databaseFile);
            if (_cache.IsEmpty)
            {
                _cache.AddRange(Downloader.GetAll());
            }
        }

        public void Refresh()
        {
            // There must not be a possibility of empty cache but t is better to check
            if (_cache is null) throw new MacChangerException("Cache object does not exist");

            if (!_cache.IsEmpty)
            {
                _cache.Clear();
            }
            _cache.AddRange(Downloader.GetAll());
        }

#pragma warning disable CS8602 // Dereference of a possibly null reference.
        public IEnumerable<Vendor> Get(string oui) => _cache.Get(oui);
#pragma warning restore CS8602 // Dereference of a possibly null reference.

#pragma warning disable CS8602 // Dereference of a possibly null reference.
        public IEnumerator<Vendor> GetEnumerator() => _cache.GetAll().GetEnumerator();
#pragma warning restore CS8602 // Dereference of a possibly null reference.

#pragma warning disable CS8602 // Dereference of a possibly null reference.
        IEnumerator IEnumerable.GetEnumerator() => _cache.GetAll().GetEnumerator();
#pragma warning restore CS8602 // Dereference of a possibly null reference.

        /// <summary>
        ///     Checks if a vendor with the provided OUI exists.
        ///     There may be more than one vendors as OUIs are not unique.
        ///     Hence, it returns an enumerable.
        /// </summary>
        /// <param name="oui">IEEE assigned OUI</param>
        /// <param name="vendors">Matched vendors from IEEE records</param>
        /// <returns>If OUI exists in the IEEE database</returns>
        public bool TryGetValue(string oui, out IEnumerable<Vendor> vendors)
        {
            vendors = Get(oui);
            return vendors.Any();
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

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
