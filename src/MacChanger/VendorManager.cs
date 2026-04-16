#nullable enable

using System;

namespace MacChanger
{
    /// <summary>
    ///     Downloads  OUI (Organizationally Unique Identifier) list from
    ///     IEEE and parses into a <see cref="VendorList"/> instance,
    ///     which is initiated lazily.
    /// </summary>
    public class VendorManager : IDisposable
    {
        private static VendorList? _vendors;
        private readonly Random _random = new Random();
        private bool disposedValue;
        internal VendorList Vendors => _vendors ??= new VendorList();

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
            if (_vendors == null)
            {
                throw new MacChangerException($"{nameof(_vendors)} cannot be null.");
            }

            var max = _vendors.Count;
            if (max == 0)
            {
                throw new MacChangerException("Vendor list is empty. Update the OUI list from the About menu.");
            }
            Vendor? selected = null;
            while (selected == null)
            {
                var offset = _random.Next(max);
                selected = _vendors[offset];
            }
            return selected.Value;
        }

        public VendorList GetVendorList() => Vendors;

        public void Refresh() => Vendors.Refresh();

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
                    _vendors?.Dispose();
                }

                _vendors = null;
                disposedValue = true;
            }
        }

        #endregion Dispose
    }
}
