using System;
using System.Diagnostics;
using MacChanger.Gui.Utils;

namespace MacChanger.Gui.DTO
{
    /// <summary>
    ///     A DTO for MacChanger GUI detailed information
    /// </summary>
    [DebuggerDisplay("{" + nameof(GetDebuggerDisplay) + "(),nq}")]
    internal class NetworkConnectionDetail : IDisposable
    {
        private readonly NetworkAdapter _adapter;
        private readonly MacFormatter formatter = new MacFormatter();
        private bool disposedValue;
        public string ActiveMac => Changed == "Yes" ? string.Format(formatter, "{0}", _adapter.RegistryMacAddress) + " (Changed)" : OriginalMac;
        public string Changed => _adapter.RegistryMacAddress != null ? "Yes" : "No";
        public string ConfigId => _adapter.ConfigId;
        public string Device => _adapter.DeviceDescription;
        public bool Enabled => _adapter.Enabled;
        public string HardwareId => _adapter.HardwareId;
        public string LinkStatus => _adapter.LinkStatus;
        public string Name => _adapter.Name;
        public string OriginalMac => string.Format(formatter, "{0}", _adapter.OriginalMacAddress);
        public string Speed => ReadableSpeed(_adapter.Speed);
        public string OriginalVendor => _adapter.OriginalVendor;
        public string ActiveVendor => Changed == "Yes" ? string.Empty : OriginalVendor; // TODO: Query the vendor

        public string IPv4Status => _adapter.IsIPv4Enabled ? "Enabled" : "Disabled";
        public string IPv6Status => _adapter.IsIPv6Enabled ? "Enabled" : "Disabled";

        public NetworkConnectionDetail(NetworkAdapter adapter)
        {
            _adapter = adapter;
        }

        private string GetDebuggerDisplay() => Name;

        private string ReadableSpeed(long speed)
        {
            if (speed >= 1000000000)
            {
                float v = speed / 1000000000;
                return $"{v:F2} Gbps";
            }
            else if (speed >= 1000000)
            {
                float v = speed / 1000000;
                return $"{v:F2} Mbps";
            }
            else if (speed >= 1000)
            {
                float v = speed / 1000;
                return $"{v:F2} Kbps";
            }
            else if (speed == -1)
            {
                return "0 bps";
            }
            else
            {
                return $"{speed} bps";
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
                    _adapter?.Dispose();
                }

                disposedValue = true;
            }
        }
        #endregion Dispose
    }
}
