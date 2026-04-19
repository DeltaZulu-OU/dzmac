#nullable enable

using System;
using System.Collections.Generic;
using System.Diagnostics;
using BrightIdeasSoftware;
using Dzmac.Core;

namespace Dzmac.DTO
{
    /// <summary>
    ///     A DTO for DZMAC datagrid which includes all adapter information.
    /// </summary>
    [DebuggerDisplay("{" + nameof(GetDebuggerDisplay) + "(),nq}")]
    internal class NetworkConnection : IDisposable
    {
        [OLVColumn("Enabled", CheckBoxes = true, IsEditable = false, DisplayIndex = 0)]
        public bool Enabled
        {
            get => _enabled;
            set => _ = TrySetEnabled(value);
        }

        [OLVColumn("Network Connections", DisplayIndex = 1)]
        public string ConnectionName => Name;

        [OLVColumn("Changed", DisplayIndex = 2)]
        public string Changed => _adapter.Changed ? "Yes" : "No";

        [OLVColumn("Link Status", DisplayIndex = 4)]
        public string LinkStatus { get; }

        [OLVColumn("MAC Address", DisplayIndex = 3)]
        public string MacAddress => DisplayMac;

        [OLVColumn("Speed", DisplayIndex = 5)]
        public string Speed => ReadableSpeed(_adapter.Speed);

        internal bool ShowSpeedInKBytesPerSec { get; set; }
        internal string Name { get; }
        internal string Device { get; }
        internal string DeviceManufacturer { get; }
        internal string ConfigId { get; }
        internal string HardwareId { get; }
        internal bool IsDhcpEnabled { get; }
        internal string IPv4Status { get; }
        internal string IPv6Status { get; }
        internal string OriginalMac { get; }
        internal string OriginalVendor { get; }
        internal string ActiveMac { get; }
        internal string ActiveVendor { get; }
        internal IReadOnlyList<AdapterIpv4Address> Ipv4Addresses { get; }
        internal IReadOnlyList<AdapterIpv6Address> Ipv6Addresses { get; }
        internal IReadOnlyList<string> Ipv4Gateways { get; }
        internal IReadOnlyList<string> Ipv6Gateways { get; }
        internal IReadOnlyList<string> Ipv4DnsServers { get; }
        internal IReadOnlyList<string> Ipv6DnsServers { get; }

        internal string DisplayMac => _adapter.Changed ? $"{ActiveMac} (Changed)" : OriginalMac;

        internal NetworkAdapter Adapter => _adapter;

        private readonly NetworkAdapter _adapter;
        private readonly AdapterAdminService _adminService;
        private bool _enabled;
        private bool _disposedValue;

        public NetworkConnection(NetworkAdapter adapter, bool showSpeedInKBytesPerSec, AdapterAdminService adminService)
        {
            _adapter = adapter;
            _adminService = adminService;
            ShowSpeedInKBytesPerSec = showSpeedInKBytesPerSec;
            Name = adapter.Name;
            Device = adapter.DeviceDescription;
            DeviceManufacturer = adapter.DeviceManufacturer;
            ConfigId = adapter.ConfigId;
            _enabled = adapter.Enabled;
            HardwareId = adapter.HardwareId;
            IsDhcpEnabled = adapter.IsDhcpEnabled;
            IPv4Status = adapter.IsIPv4Enabled ? "Enabled" : "Disabled";
            IPv6Status = adapter.IsIPv6Enabled ? "Enabled" : "Disabled";
            LinkStatus = adapter.LinkStatus;
            OriginalMac = adapter.OriginalMacAddress.ToString(Core.MacAddress.MacDelimiter.Dash);
            OriginalVendor = adapter.OriginalVendor;
            ActiveVendor = adapter.ActiveVendor;
            ActiveMac = adapter.Changed ? adapter.ActiveMacAddress!.ToString(Core.MacAddress.MacDelimiter.Dash) : OriginalMac;
            Ipv4Addresses = adapter.GetIpv4Addresses();
            Ipv6Addresses = adapter.GetIpv6Addresses();
            Ipv4Gateways = adapter.GetIpv4Gateways();
            Ipv6Gateways = adapter.GetIpv6Gateways();
            Ipv4DnsServers = adapter.GetIpv4DnsServers();
            Ipv6DnsServers = adapter.GetIpv6DnsServers();
        }

        internal MacAddress GetRandom(string oui) => Core.MacAddress.GetNewMac(oui);

        internal AdapterAdminResult TrySetEnabled(bool shouldEnable)
        {
            if (_enabled == shouldEnable)
            {
                return AdapterAdminResult.Success("Adapter state already matches requested value.");
            }

            var result = _adminService.SetAdapterEnabled(_adapter, shouldEnable);
            if (result.IsSuccess)
            {
                _enabled = shouldEnable;
            }

            return result;
        }

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
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    _adapter?.Dispose();
                }

                _disposedValue = true;
            }
        }

        #endregion Dispose
    }
}
