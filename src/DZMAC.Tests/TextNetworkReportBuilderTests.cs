using System;
using System.Collections.Generic;
using Dzmac.Gui.Core.Reporting;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Dzmac.Tests
{
    [TestClass]
    public class TextNetworkReportBuilderTests
    {
        [TestMethod]
        public void BuildReport_ShouldIncludeHeader_AndKeyFields()
        {
            var sut = new TextNetworkReportBuilder();
            var generatedAt = new DateTime(2026, 4, 18, 9, 30, 15);
            var entries = new List<NetworkReportEntry>
            {
                new NetworkReportEntry
                {
                    Name = "Ethernet",
                    Device = "Intel Adapter",
                    DeviceManufacturer = "Intel",
                    HardwareId = "PCI\\VEN_8086",
                    ConfigId = "CFG-1",
                    ActiveMac = "AA-BB-CC-DD-EE-FF",
                    ActiveVendor = "Intel Corp",
                    Speed = "1000 Mbps",
                    Enabled = true,
                    IPv4Status = "Enabled",
                    IPv6Status = "Disabled",
                    IsDhcpEnabled = true,
                    Ipv4Addresses = new List<NetworkReportIpv4Address>
                    {
                        new NetworkReportIpv4Address { Address = "192.168.1.20", SubnetMask = "255.255.255.0" }
                    },
                    Ipv4Gateways = new List<string> { "192.168.1.1" },
                    Ipv4DnsServers = new List<string> { "1.1.1.1", "8.8.8.8" }
                }
            };

            var report = sut.BuildReport(entries, generatedAt, "1.2.3");

            StringAssert.Contains(report, "DZMAC MAC Address Changer $1.2.3");
            StringAssert.Contains(report, "Date: Saturday, April 18, 2026  09:30:15");
            StringAssert.Contains(report, "Interface #1");
            StringAssert.Contains(report, "Connection Name                         Ethernet");
            StringAssert.Contains(report, "Link Status                             Up, Operational");
            StringAssert.Contains(report, "IPv4 Address                            192.168.1.20 (255.255.255.0)");
            StringAssert.Contains(report, "IPv4 Default Gateway                    192.168.1.1 (0)");
            StringAssert.Contains(report, "IPv4 DNS Server                         8.8.8.8");
        }

        [TestMethod]
        public void BuildReport_ShouldUseSafeDefaults_ForMissingValues()
        {
            var sut = new TextNetworkReportBuilder();
            var entries = new List<NetworkReportEntry>
            {
                new NetworkReportEntry
                {
                    Name = null,
                    Device = string.Empty,
                    Speed = "0 bps",
                    Enabled = false,
                    IPv4Status = "Disabled",
                    IPv6Status = "Disabled",
                    IsDhcpEnabled = false,
                    Ipv4Addresses = new List<NetworkReportIpv4Address>
                    {
                        new NetworkReportIpv4Address { Address = "10.0.0.10", SubnetMask = null }
                    },
                    Ipv4Gateways = new List<string> { "", "   " },
                    Ipv4DnsServers = new List<string>()
                }
            };

            var report = sut.BuildReport(entries, new DateTime(2026, 4, 18, 11, 0, 0), "9.9.9");

            StringAssert.Contains(report, "Connection Name                         N/A");
            StringAssert.Contains(report, "Device Name                             N/A");
            StringAssert.Contains(report, "Link Status                             Down, Non Operational");
            StringAssert.Contains(report, "IPv4 Address                            10.0.0.10 (0.0.0.0)");
            Assert.IsFalse(report.Contains("IPv4 Default Gateway"));
        }
    }
}
