using System.Collections.Generic;

namespace Dzmac.Core.Reporting
{
    public class NetworkReportEntry
    {
        public string Name { get; set; }
        public string Device { get; set; }
        public string DeviceManufacturer { get; set; }
        public string HardwareId { get; set; }
        public string ConfigId { get; set; }
        public string ActiveMac { get; set; }
        public string ActiveVendor { get; set; }
        public string Speed { get; set; }
        public bool Enabled { get; set; }
        public string IPv4Status { get; set; }
        public string IPv6Status { get; set; }
        public bool IsDhcpEnabled { get; set; }
        public IReadOnlyList<NetworkReportIpv4Address> Ipv4Addresses { get; set; } = new List<NetworkReportIpv4Address>();
        public IReadOnlyList<string> Ipv4Gateways { get; set; } = new List<string>();
        public IReadOnlyList<string> Ipv4DnsServers { get; set; } = new List<string>();
    }

    public class NetworkReportIpv4Address
    {
        public string Address { get; set; }
        public string SubnetMask { get; set; }
    }
}
