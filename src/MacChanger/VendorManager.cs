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
        private static VendorList _vendors;
        private bool disposedValue;

        public VendorList GetVendorList() => _vendors ?? (_vendors = new VendorList());

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

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
