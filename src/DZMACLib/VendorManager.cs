#nullable enable

using System;

namespace DZMACLib
{
    /// <summary>
    ///     Downloads  OUI (Organizationally Unique Identifier) list from
    ///     IEEE and parses into a <see cref="VendorList"/> instance,
    ///     which is initiated lazily.
    /// </summary>
    public class VendorManager : IDisposable
    {
        private readonly object _sync = new object();
        private VendorList? _vendors;
        private readonly Random _random = new Random();
        private bool disposedValue;
        internal VendorList Vendors
        {
            get
            {
                lock (_sync)
                {
                    return _vendors ?? (_vendors = new VendorList());
                }
            }
        }

        /// <summary>
        ///     Checks if a vendor with the provided OUI exists.
        ///     There may be more than one vendors as OUIs are not unique.
        ///     Hence, it returns an enumerable.
        /// </summary>
        /// <param name="macAddress">MAC address of the adapter</param>
        /// <returns>List of possible vendors or an empty list.</returns>
        public Vendor? FindByMac(string macAddress, bool useWildcard = false)
        {
            var oui = macAddress.Substring(0, 6);
            return Vendors.Get(oui, useWildcard);
        }

        /// <summary>
        ///     Checks if a vendor with the provided OUI exists.
        ///     There may be more than one vendors as OUIs are not unique.
        ///     Hence, it returns an enumerable.
        /// </summary>
        /// <param name="macAddress">MAC address of the adapter</param>
        /// <returns>List of possible vendors or an empty list.</returns>
        public Vendor? FindByMac(MacAddress macAddress, bool useWildcard = false) => FindByMac(macAddress.ToString(), useWildcard);

        public Vendor GetRandom()
        {
            var vendors = Vendors;
            var max = vendors.Count;
            if (max == 0)
            {
                throw new DZMACLibException("Vendor list is empty. Update the OUI list from the About menu.");
            }
            Vendor? selected = null;
            while (selected == null)
            {
                var offset = _random.Next(max);
                selected = vendors[offset];
            }
            return selected.Value;
        }

        public VendorList GetVendorList() => Vendors;

        public void Refresh()
        {
            lock (_sync)
            {
                var replacement = new VendorList();
                replacement.Refresh();

                var previous = _vendors;
                _vendors = replacement;
                previous?.Dispose();
            }
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
                    lock (_sync)
                    {
                        _vendors?.Dispose();
                        _vendors = null;
                    }
                }
                disposedValue = true;
            }
        }

        #endregion Dispose
    }
}
