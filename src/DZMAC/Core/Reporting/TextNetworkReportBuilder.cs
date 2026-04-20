using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Dzmac.Core.Reporting
{
    internal class TextNetworkReportBuilder
    {
        public string BuildReport(IReadOnlyList<NetworkReportEntry> entries, DateTime generatedAt, string productVersion)
        {
            if (entries == null)
            {
                throw new ArgumentNullException(nameof(entries));
            }

            var report = new StringBuilder();
            report.AppendLine($"DZMAC MAC Address Changer v{productVersion}");
            report.AppendLine("===================================================");
            report.AppendLine();
            report.AppendLine($"Date: {generatedAt.ToString("dddd, MMMM d, yyyy  HH:mm:ss", CultureInfo.InvariantCulture)}");
            report.AppendLine();
            report.AppendLine("Text Report");
            report.AppendLine("===========");
            report.AppendLine();

            for (var index = 0; index < entries.Count; index++)
            {
                var entry = entries[index];
                report.AppendLine($"Interface #{index + 1}");
                report.AppendLine("=============");
                AppendField(report, "Connection Name", entry.Name);
                AppendField(report, "Device Name", entry.Device);
                AppendField(report, "Device Manufacturer", entry.DeviceManufacturer);
                AppendField(report, "Hardware ID", entry.HardwareId);
                AppendField(report, "Configuration ID", entry.ConfigId);
                AppendField(report, "Active MAC Address", entry.ActiveMac);
                AppendField(report, "Active MAC Address Vendor", entry.ActiveVendor);
                AppendField(report, "Link Speed", entry.Speed?.ToLowerInvariant());
                AppendField(report, "Link Status", FormatLinkStatus(entry));
                AppendField(report, "TCP/IPv4", (entry.IPv4Status == "Enabled").ToString());
                AppendField(report, "TCP/IPv6", (entry.IPv6Status == "Enabled").ToString());
                AppendField(report, "DHCPv4 Enabled", entry.IsDhcpEnabled.ToString());
                AppendIpv4AddressFields(report, entry.Ipv4Addresses);
                AppendMultiValueField(report, "IPv4 Default Gateway", entry.Ipv4Gateways, includeMetricPlaceholder: true);
                AppendMultiValueField(report, "IPv4 DNS Server", entry.Ipv4DnsServers, includeMetricPlaceholder: false);
                AppendField(report, "DHCPv6 Enabled", bool.FalseString);
                report.AppendLine();
            }

            return report.ToString();
        }

        private static void AppendIpv4AddressFields(StringBuilder report, IReadOnlyList<NetworkReportIpv4Address> addresses)
        {
            if (addresses == null || addresses.Count == 0)
            {
                return;
            }

            foreach (var address in addresses)
            {
                var subnetMask = string.IsNullOrWhiteSpace(address.SubnetMask) ? "0.0.0.0" : address.SubnetMask;
                AppendField(report, "IPv4 Address", $"{address.Address} ({subnetMask})");
            }
        }

        private static void AppendMultiValueField(StringBuilder report, string label, IReadOnlyList<string> values, bool includeMetricPlaceholder)
        {
            if (values == null || values.Count == 0)
            {
                return;
            }

            foreach (var value in values.Where(v => !string.IsNullOrWhiteSpace(v)))
            {
                var formattedValue = includeMetricPlaceholder ? $"{value} (0)" : value;
                AppendField(report, label, formattedValue);
            }
        }

        private static void AppendField(StringBuilder report, string label, string value)
        {
            var safeValue = string.IsNullOrWhiteSpace(value) ? "N/A" : value;
            report.AppendLine($"{label,-40}{safeValue}");
        }

        private static string FormatLinkStatus(NetworkReportEntry entry)
        {
            var connectionState = entry.Enabled ? "Up" : "Down";
            var operationalState = entry.Enabled && !string.Equals(entry.Speed, "0 bps", StringComparison.OrdinalIgnoreCase)
                ? "Operational"
                : "Non Operational";
            return $"{connectionState}, {operationalState}";
        }
    }
}
