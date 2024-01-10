using System;
using System.Diagnostics;

namespace MacChanger.Gui.DTO
{
    /// <summary>
    ///     A DTO for MacChanger GUI detailed information
    /// </summary>
    [DebuggerDisplay("{" + nameof(GetDebuggerDisplay) + "(),nq}")]
    internal class NetworkConnectionDetail : IDisposable
    {
        private readonly NetworkAdapter _adapter;
        private bool? _isChanged;
        private bool disposedValue;
        public string ActiveMac
        {
            get
            {
                if (IsChanged)
                {
                    return _adapter.RegistryMacAddress.ToString(MacAddress.MacDelimiter.Dash) + " (Changed)";
                }
                else
                {
                    return OriginalMac;
                }
            }
        }

        public string ActiveVendor => IsChanged ? string.Empty : OriginalVendor;
        public string Changed => IsChanged ? "Yes" : "No";
        public string ConfigId => _adapter.ConfigId;
        public string Device => _adapter.DeviceDescription;
        public bool Enabled => _adapter.Enabled;
        public string HardwareId => _adapter.HardwareId;
        public string IPv4Status => _adapter.IsIPv4Enabled ? "Enabled" : "Disabled";
        public string IPv6Status => _adapter.IsIPv6Enabled ? "Enabled" : "Disabled";
        public string LinkStatus => _adapter.LinkStatus;
        public string Name => _adapter.Name;
        public string OriginalMac => _adapter.OriginalMacAddress.ToString(MacAddress.MacDelimiter.Dash);
        public string OriginalVendor => _adapter.OriginalVendor;
        public string Speed => ReadableSpeed(_adapter.Speed);
        internal bool IsChanged => _isChanged ?? (_isChanged = Test()).Value;

        private bool Test()
        {
            if (_adapter.RegistryMacAddress != null)
            {
                if (_adapter.RegistryMacAddress == _adapter.OriginalMacAddress)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                return false;
            }
        }

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
