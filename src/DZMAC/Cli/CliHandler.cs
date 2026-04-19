using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Dzmac.Gui.Cli;
using Dzmac.Gui.Core;

namespace Dzmac.Gui
{
    internal static class CliHandler
    {
        private const string UnsupportedOptionMessage = "Option is intentionally unsupported in this reimplementation (see README product decisions).";

        public static int Run(string[] args)
        {
            try
            {
                if (args.Length == 0 || HasFlag(args, "-help"))
                {
                    ShowHelp();
                    return 0;
                }

                if (!TryParse(args, out var options, out var parseError))
                {
                    return ExitWithError(parseError);
                }

                if (options.UnsupportedOptions.Count > 0)
                {
                    return ExitWithError(string.Join(Environment.NewLine, options.UnsupportedOptions.Select(o => $"{o}: {UnsupportedOptionMessage}")));
                }

                if (string.IsNullOrWhiteSpace(options.ConnectionName))
                {
                    return ExitWithError("Missing required option: -n network_connection_name");
                }

                if (!TryGetAdapter(options.ConnectionName, out var adapter))
                {
                    return ExitWithError($"Network connection '{options.ConnectionName}' was not found.");
                }

                using (adapter)
                using (var vendorManager = new VendorList())
                {
                    if (options.RestoreOriginalRecord && !adapter.TrySetRegistryMac(null))
                    {
                        return ExitWithError("Failed to restore original MAC address record.");
                    }

                    if (options.MacOperation != null)
                    {
                        var mac = ResolveMac(options.MacOperation, vendorManager);
                        if (!adapter.TrySetRegistryMac(mac))
                        {
                            return ExitWithError("Failed to update MAC address.");
                        }
                    }

                    if (options.EnableDhcpV4 && !adapter.TryDhcpEnable())
                    {
                        return ExitWithError("Failed to enable DHCPv4.");
                    }

                    if (options.Ipv4Addresses.Count > 0 && !adapter.TrySetIPv4Addresses(options.Ipv4Addresses.Select(x => x.Address).ToArray(), options.Ipv4Addresses.Select(x => x.SubnetMask).ToArray()))
                    {
                        return ExitWithError("Failed to set IPv4 addresses.");
                    }

                    if (options.Ipv4Gateways.Count > 0 && !adapter.TrySetIPv4Gateways(options.Ipv4Gateways.Select(x => x.Address).ToArray(), options.Ipv4Gateways.Select(x => x.Metric).ToArray()))
                    {
                        return ExitWithError("Failed to set IPv4 gateways.");
                    }

                    if (options.Ipv4DnsServers.Count > 0 && !adapter.TrySetIPv4DnsServers(options.Ipv4DnsServers.ToArray()))
                    {
                        return ExitWithError("Failed to set IPv4 DNS servers.");
                    }

                    if (options.ReleaseDhcpV4 && !adapter.TryDhcpRelease(out var releaseMessage))
                    {
                        return ExitWithError($"Failed to release DHCPv4 lease. {releaseMessage}");
                    }

                    if (options.RenewDhcpV4 && !adapter.TryDhcpRenew(out var renewMessage))
                    {
                        return ExitWithError($"Failed to renew DHCPv4 lease. {renewMessage}");
                    }

                    if (options.DisableAdapter && !adapter.TryDisableAdapter())
                    {
                        return ExitWithError("Failed to disable network connection.");
                    }

                    if (options.EnableAdapter && !adapter.TryEnableAdapter())
                    {
                        return ExitWithError("Failed to enable network connection.");
                    }

                    if (options.ResetAdapter)
                    {
                        if (!adapter.TryDisableAdapter() || !adapter.TryEnableAdapter())
                        {
                            return ExitWithError("Failed to reset network connection.");
                        }
                    }
                }

                return 0;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
                return 2;
            }
        }

        private static int ExitWithError(string message)
        {
            Console.Error.WriteLine(message);
            return 1;
        }

        private static bool HasFlag(IEnumerable<string> args, string flag) => args.Any(a => string.Equals(a, flag, StringComparison.OrdinalIgnoreCase));

        private static MacAddress ResolveMac(MacOperation macOperation, VendorList manager)
        {
            if (macOperation.IsRestore)
            {
                return null;
            }

            MacAddress mac;
            if (macOperation.IsRandom)
            {
                var vendor = manager.GetRandom();
                mac = MacAddress.GetNewMac(vendor.Oui);
            }
            else
            {
                mac = new MacAddress(macOperation.MacAddress);
            }

            if (macOperation.Force02)
            {
                mac.SetAsLocallyAdministered();
            }

            return mac;
        }

        private static void ShowHelp() => Console.WriteLine(CommandLineHelpContent.Text);

        private static bool TryGetAdapter(string connectionName, out NetworkAdapter adapter)
        {
            adapter = NetworkAdapterFactory.GetNetworkAdapters()
                .FirstOrDefault(a => string.Equals(a.Name, connectionName, StringComparison.OrdinalIgnoreCase)
                                     || string.Equals(a.DeviceDescription, connectionName, StringComparison.OrdinalIgnoreCase));
            return adapter != null;
        }

        private static bool TryParse(IReadOnlyList<string> args, out CliOptions options, out string error)
        {
            options = new CliOptions();
            error = string.Empty;
            var macOptionSeen = false;

            for (var i = 0; i < args.Count; i++)
            {
                var token = args[i];
                var normalizedToken = token.ToLowerInvariant();

                switch (normalizedToken)
                {
                    case "-n":
                        if (!TryReadValue(args, ref i, out var name, out error))
                        {
                            return false;
                        }

                        options.ConnectionName = name;
                        break;

                    case "-m":
                        if (!TrySetMacOperation(args, ref i, ref macOptionSeen, options, force02: false, out error))
                        {
                            return false;
                        }

                        break;

                    case "-nm":
                        if (!TrySetMacOperation(args, ref i, ref macOptionSeen, options, force02: false, out error))
                        {
                            return false;
                        }

                        break;

                    case "-r":
                    case "-nr":
                        if (macOptionSeen)
                        {
                            error = "Only one MAC address option can be specified at a time.";
                            return false;
                        }

                        options.MacOperation = new MacOperation(isRandom: true, force02: false);
                        macOptionSeen = true;
                        break;

                    case "-r02":
                    case "-nr02":
                        if (macOptionSeen)
                        {
                            error = "Only one MAC address option can be specified at a time.";
                            return false;
                        }

                        options.MacOperation = new MacOperation(isRandom: true, force02: true);
                        macOptionSeen = true;
                        break;

                    case "-i":
                        if (!TryReadValue(args, ref i, out var ipv4, out error))
                        {
                            return false;
                        }

                        if (!TryParseIpv4Addresses(ipv4, out var addresses, out error))
                        {
                            return false;
                        }

                        options.Ipv4Addresses.AddRange(addresses);
                        break;

                    case "-g":
                        if (!TryReadValue(args, ref i, out var gateways, out error))
                        {
                            return false;
                        }

                        if (!TryParseIpv4Gateways(gateways, out var parsedGateways, out error))
                        {
                            return false;
                        }

                        options.Ipv4Gateways.AddRange(parsedGateways);
                        break;

                    case "-d":
                        if (!TryReadValue(args, ref i, out var dns, out error))
                        {
                            return false;
                        }

                        var dnsServers = dns.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()).Where(x => x.Length > 0).ToList();
                        if (dnsServers.Count == 0)
                        {
                            error = "Invalid -d value. Expected at least one DNS server.";
                            return false;
                        }

                        foreach (var dnsServer in dnsServers)
                        {
                            if (!IpAddressValidator.TryValidateIpv4Address(dnsServer, out var normalizedDns))
                            {
                                error = $"Invalid DNS server '{dnsServer}'. Expected an IPv4 address.";
                                return false;
                            }

                            options.Ipv4DnsServers.Add(normalizedDns);
                        }

                        break;

                    case "-h":
                        options.EnableDhcpV4 = true;
                        break;

                    case "-rl":
                        options.ReleaseDhcpV4 = true;
                        break;

                    case "-rn":
                        options.RenewDhcpV4 = true;
                        break;

                    case "-re":
                        options.ResetAdapter = true;
                        break;

                    case "-di":
                        options.DisableAdapter = true;
                        break;

                    case "-en":
                        options.EnableAdapter = true;
                        break;

                    case "-ro":
                        options.RestoreOriginalRecord = true;
                        break;

                    case "-s":
                        break;

                    case "-i6":
                    case "-g6":
                    case "-d6":
                    case "-h6":
                    case "-rl6":
                    case "-rn6":
                    case "-pxy":
                    case "-f":
                    case "-p":
                        options.UnsupportedOptions.Add(normalizedToken);
                        if ((normalizedToken == "-pxy" || normalizedToken == "-f" || normalizedToken == "-p" || normalizedToken == "-i6" || normalizedToken == "-g6" || normalizedToken == "-d6")
                            && i + 1 < args.Count && !args[i + 1].StartsWith("-", StringComparison.Ordinal))
                        {
                            i++;
                        }

                        break;

                    default:
                        error = $"Unknown option: {token}";
                        return false;
                }
            }

            return true;
        }

        private static bool TryParseIpv4Addresses(string input, out List<Ipv4AddressSpec> items, out string error)
        {
            items = new List<Ipv4AddressSpec>();
            error = string.Empty;

            foreach (var entry in input.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                var parts = entry.Split('/');
                if (parts.Length != 2)
                {
                    error = $"Invalid -i value '{entry}'. Expected ip/subnet format.";
                    return false;
                }

                var ipText = parts[0].Trim();
                var subnetText = parts[1].Trim();

                if (!IpAddressValidator.TryValidateIpv4Address(ipText, out var normalizedIp))
                {
                    error = $"Invalid IPv4 address '{ipText}'.";
                    return false;
                }

                if (!IpAddressValidator.TryValidateIpv4SubnetMask(subnetText, out var normalizedSubnet))
                {
                    error = $"Invalid subnet mask '{subnetText}'.";
                    return false;
                }

                items.Add(new Ipv4AddressSpec(normalizedIp, normalizedSubnet));
            }

            return true;
        }

        private static bool TryParseIpv4Gateways(string input, out List<Ipv4GatewaySpec> items, out string error)
        {
            items = new List<Ipv4GatewaySpec>();
            error = string.Empty;

            foreach (var entry in input.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                var parts = entry.Split('/');
                if (parts.Length != 2)
                {
                    error = $"Invalid -g value '{entry}'. Expected gateway/metric format.";
                    return false;
                }

                if (!int.TryParse(parts[1], NumberStyles.Integer, CultureInfo.InvariantCulture, out var metric) || metric < 0)
                {
                    error = $"Invalid gateway metric in '{entry}'.";
                    return false;
                }

                var gatewayText = parts[0].Trim();
                if (!IpAddressValidator.TryValidateIpv4Address(gatewayText, out var normalizedGateway))
                {
                    error = $"Invalid gateway IPv4 address '{gatewayText}'.";
                    return false;
                }

                items.Add(new Ipv4GatewaySpec(normalizedGateway, metric));
            }

            return true;
        }

        private static bool TryReadOptionalValue(IReadOnlyList<string> args, ref int index, out string value)
        {
            value = string.Empty;
            if (index + 1 >= args.Count)
            {
                return false;
            }

            var candidate = args[index + 1];
            if (candidate.StartsWith("-", StringComparison.Ordinal))
            {
                return false;
            }

            index++;
            value = candidate;
            return true;
        }

        private static bool TryReadValue(IReadOnlyList<string> args, ref int index, out string value, out string error)
        {
            value = string.Empty;
            error = string.Empty;

            if (index + 1 >= args.Count)
            {
                error = $"Missing value for option '{args[index]}'.";
                return false;
            }

            value = args[++index];
            return true;
        }

        private static bool TrySetMacOperation(IReadOnlyList<string> args, ref int index, ref bool macOptionSeen, CliOptions options, bool force02, out string error)
        {
            error = string.Empty;
            if (macOptionSeen)
            {
                error = "Only one MAC address option can be specified at a time.";
                return false;
            }

            if (TryReadOptionalValue(args, ref index, out var macAddress))
            {
                options.MacOperation = new MacOperation(macAddress, force02);
            }
            else
            {
                options.MacOperation = new MacOperation(string.Empty, force02);
            }

            macOptionSeen = true;
            return true;
        }

        private sealed class CliOptions
        {
            public string ConnectionName { get; set; }
            public bool DisableAdapter { get; set; }
            public bool EnableAdapter { get; set; }
            public bool EnableDhcpV4 { get; set; }
            public List<Ipv4AddressSpec> Ipv4Addresses { get; } = new List<Ipv4AddressSpec>();
            public List<string> Ipv4DnsServers { get; } = new List<string>();
            public List<Ipv4GatewaySpec> Ipv4Gateways { get; } = new List<Ipv4GatewaySpec>();
            public MacOperation MacOperation { get; set; }
            public bool ReleaseDhcpV4 { get; set; }
            public bool RenewDhcpV4 { get; set; }
            public bool ResetAdapter { get; set; }
            public bool RestoreOriginalRecord { get; set; }
            public List<string> UnsupportedOptions { get; } = new List<string>();
        }

        private sealed class Ipv4AddressSpec
        {
            public Ipv4AddressSpec(string address, string subnetMask)
            {
                Address = address;
                SubnetMask = subnetMask;
            }

            public string Address { get; }
            public string SubnetMask { get; }
        }

        private sealed class Ipv4GatewaySpec
        {
            public Ipv4GatewaySpec(string address, int metric)
            {
                Address = address;
                Metric = metric;
            }

            public string Address { get; }
            public int Metric { get; }
        }

        private sealed class MacOperation
        {
            public MacOperation(string macAddress, bool force02)
            {
                MacAddress = macAddress;
                Force02 = force02;
            }

            public MacOperation(bool isRandom, bool force02)
            {
                IsRandom = isRandom;
                Force02 = force02;
            }

            public bool Force02 { get; }
            public bool IsRandom { get; }
            public bool IsRestore => !IsRandom && string.IsNullOrWhiteSpace(MacAddress);
            public string MacAddress { get; }
        }
    }
}