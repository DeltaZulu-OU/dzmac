using System;
using System.Collections.Generic;

namespace MacChanger
{
    /// <summary>
    ///     Downloads  OUI (Organizationally Unique Identifier) list from
    ///     IEEE and parses into a <see cref="VendorList"/> instance,
    ///     which is initiated lazily.
    /// </summary>
    public class VendorManager : IDisposable
    {
        internal VendorList Vendors => _vendors ?? (_vendors = new VendorList());
        private static VendorList _vendors;
        private bool disposedValue;

        public VendorList GetVendorList() => Vendors;

        public IEnumerable<Vendor> FindByMac(string macAddress)
        {
            var oui = macAddress.Substring(0, 6);
            return Vendors.Get(oui);
        }

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
        #endregion
    }
}
