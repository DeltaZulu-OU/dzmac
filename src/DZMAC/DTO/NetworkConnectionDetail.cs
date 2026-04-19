using System;
using System.Collections.Generic;
using System.Diagnostics;
using Dzmac.Core;

namespace Dzmac.DTO
{
    /// <summary>
    ///     A DTO for DZMAC detailed information
    /// </summary>
    [DebuggerDisplay("{" + nameof(GetDebuggerDisplay) + "(),nq}")]
    internal class NetworkConnectionDetail : IDisposable
    {
        public bool ShowSpeedInKBytesPerSec { get; set; }

        public string ActiveMac { get; }
        public string ActiveVendor { get; }
        public string Changed => IsChanged ? "Yes" : "No";
        public string ConfigId { get; }
        public string Device { get; }
        public string DeviceManufacturer { get; }
        public bool Enabled { get; }
        public string HardwareId { get; }
        public bool IsDhcpEnabled { get; }
        public string IPv4Status { get; }
        public string IPv6Status { get; }
        public string LinkStatus { get; }
        public string Name { get; }
        public string OriginalMac { get; }
        public string OriginalVendor { get; }
        public IReadOnlyList<AdapterIpv4Address> Ipv4Addresses { get; }
        public IReadOnlyList<AdapterIpv6Address> Ipv6Addresses { get; }
        public IReadOnlyList<string> Ipv4Gateways { get; }
        public IReadOnlyList<string> Ipv6Gateways { get; }
        public IReadOnlyList<string> Ipv4DnsServers { get; }
        public IReadOnlyList<string> Ipv6DnsServers { get; }
        public string Speed => ReadableSpeed(RawSpeed);
        internal bool IsChanged { get; }
        private long RawSpeed { get; }
        private readonly NetworkAdapter _adapter;
        private bool disposedValue;

        public NetworkConnectionDetail(NetworkAdapter adapter, bool showSpeedInKBytesPerSec = false)
        {
            _adapter = adapter;
            ShowSpeedInKBytesPerSec = showSpeedInKBytesPerSec;
            Name = _adapter.Name;
            Device = _adapter.DeviceDescription;
            DeviceManufacturer = _adapter.DeviceManufacturer;
            ConfigId = _adapter.ConfigId;
            Enabled = _adapter.Enabled;
            HardwareId = _adapter.HardwareId;
            IsDhcpEnabled = _adapter.IsDhcpEnabled;
            IPv4Status = _adapter.IsIPv4Enabled ? "Enabled" : "Disabled";
            IPv6Status = _adapter.IsIPv6Enabled ? "Enabled" : "Disabled";
            LinkStatus = _adapter.LinkStatus;
            OriginalMac = _adapter.OriginalMacAddress.ToString(MacAddress.MacDelimiter.Dash);
            OriginalVendor = _adapter.OriginalVendor;
            ActiveVendor = _adapter.ActiveVendor;
            IsChanged = _adapter.Changed;
            ActiveMac = GetActiveMac();
            RawSpeed = _adapter.Speed;
            Ipv4Addresses = _adapter.GetIpv4Addresses();
            Ipv6Addresses = _adapter.GetIpv6Addresses();
            Ipv4Gateways = _adapter.GetIpv4Gateways();
            Ipv6Gateways = _adapter.GetIpv6Gateways();
            Ipv4DnsServers = _adapter.GetIpv4DnsServers();
            Ipv6DnsServers = _adapter.GetIpv6DnsServers();
        }

        internal NetworkAdapter Adapter => _adapter;

        public MacAddress GetRandom(string oui) => MacAddress.GetNewMac(oui);

        private string GetActiveMac() => IsChanged ? _adapter.ActiveMacAddress.ToString(MacAddress.MacDelimiter.Dash) : OriginalMac;

        private string GetDebuggerDisplay() => Name;

        private string ReadableSpeed(long speed)
        {
            if (ShowSpeedInKBytesPerSec)
            {
                if (speed <= 0)
                {
                    return "0 KB/s";
                }

                var speedInKBytesPerSecond = speed / 8192f;
                return $"{speedInKBytesPerSecond:F2} KB/s";
            }

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