using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text.RegularExpressions;
#nullable enable

namespace Dzmac.Core
{
    internal sealed class AdapterIpv4Address
    {
        public string Address { get; }
        public string SubnetMask { get; }

        public AdapterIpv4Address(string address, string subnetMask)
        {
            Address = address;
            SubnetMask = subnetMask;
        }
    }

    internal sealed class AdapterIpv6Address
    {
        public string Address { get; }
        public int PrefixLength { get; }

        public AdapterIpv6Address(string address, int prefixLength)
        {
            Address = address;
            PrefixLength = prefixLength;
        }
    }

    /// <summary>
    ///     Represents a Windows network interface. Wrapper around the .NET API for network
    ///     interfaces, as well as for the unmanaged device.
    /// </summary>
    /// <remarks>
    ///     The <see cref="NetworkAdapter" /> implementation is adapted from <see
    ///     href="https://github.com/sietseringers/MACAddressTool" />.
    /// </remarks>
    internal class NetworkAdapter : IDisposable
    {
        private const string UnknownVendorIdentifier = "Unknown Vendor";
        private const string RegistryClassKey = @"SYSTEM\CurrentControlSet\Control\Class\{4D36E972-E325-11CE-BFC1-08002BE10318}";
        private const string RegistryMacOverrideValueName = "NetworkAddress";
        private const string TmacOriginalMacValueName = "OriginalNetworkAddress";
        private static readonly MacAddress UnknownMacAddress = new MacAddress("000000000000");
        private static readonly HashSet<int> StandardSuccessCodes = new HashSet<int> { 0, 1 };

        private static readonly Regex _adapterNumberPattern = new Regex("\\\"(\\d+)\\\"$");

        private readonly VendorList? _manager;
        private readonly NetworkInterface _networkInterface;
        private readonly AdapterWmiClient _wmiClient;
        private readonly AdapterRegistryClient _registryClient;
        private readonly object _syncRoot = new object();

        private ManagementObject? _adapter;
        private ManagementObject? _adapterConfig;
        private bool _wmiResolved;
        private string? _hardwareId;
        private bool? _enabled;
        private string? _registryKey;
        private bool _registryResolved;

        private bool disposedValue;

        /// <summary>
        ///     Gets the NetworkAddress registry value of this adapter.
        /// </summary>
        public MacAddress? ActiveMacAddress { get; }

        /// <summary>
        ///     Gets the vendor of the network adapter from registry.
        /// </summary>
        public string? ActiveVendor => GetActiveVendor();

        /// <summary>
        ///     Checks if there is a registry MAC address defined to override vendor-provided address
        /// </summary>
        public bool Changed => ActiveMacAddress != null;

        /// <summary>
        ///     Gets the identifier of the network adapter.
        /// </summary>
        public string ConfigId => _networkInterface.Id;

        /// <summary>
        ///     Gets the device description
        /// </summary>
        public string DeviceDescription => _networkInterface.Description;

        /// <summary>
        ///     Gets the adapter manufacturer from WMI.
        /// </summary>
        public string DeviceManufacturer
        {
            get
            {
                EnsureWmiObjects();
                if (_adapter == null)
                {
                    return string.Empty;
                }

                try
                {
                    return _adapter.GetPropertyValue("Manufacturer") as string ?? string.Empty;
                }
                catch
                {
                    return string.Empty;
                }
            }
        }

        /// <summary>
        ///     Gets if the adapter is enabled or not.
        /// </summary>
        public bool Enabled
        {
            get
            {
                EnsureWmiObjects();
                return _enabled ?? false;
            }
        }

        /// <summary>
        ///     Gets the Win32 Plug and Play device ID of the logical device.
        /// </summary>
        public string HardwareId
        {
            get
            {
                EnsureWmiObjects();
                return _hardwareId ?? string.Empty;
            }
        }

        /// <summary>
        ///     Gets if the network adapter has DHCP for IPv4 enabled
        /// </summary>
        public bool IsDhcpEnabled
        {
            get
            {
                try
                {
                    return _networkInterface.GetIPProperties().GetIPv4Properties().IsDhcpEnabled;
                }
                catch
                {
                    Debug.WriteLine($"[{nameof(NetworkAdapter)}] Failed to read DHCPv4 status for adapter '{Name}'.");
                    return false;
                }
            }
        }

        public bool IsIPv4Enabled { get; }
        public bool IsIPv6Enabled { get; }
        public bool IsPhysicalAdapter { get; }

        /// <summary>
        ///     Gets the current operational state of the network connection.
        /// </summary>
        public string LinkStatus => _networkInterface.OperationalStatus.ToString();

        /// <summary>
        ///     Gets the name of the network adapter.
        /// </summary>
        public string Name { get; }

        /// <summary>
        ///     Gets the MAC address as reported by the adapter.
        /// </summary>
        public MacAddress OriginalMacAddress { get; }

        /// <summary>
        ///     Gets the vendor of the network adapter.
        /// </summary>
        public string? OriginalVendor { get; }

        /// <summary>
        ///     Gets the speed of the network interface.
        /// </summary>
        public long Speed => _networkInterface.Speed;

        public NetworkAdapter(NetworkInterface networkInterface, VendorList? vendorManager = null, bool isPhysicalAdapter = false, AdapterWmiClient? wmiClient = null, AdapterRegistryClient? registryClient = null)
        {
            _networkInterface = networkInterface;
            _manager = vendorManager;
            _wmiClient = wmiClient ?? new AdapterWmiClient();
            _registryClient = registryClient ?? new AdapterRegistryClient();
            IsPhysicalAdapter = isPhysicalAdapter;
            Name = _networkInterface.Name;

            var props = _networkInterface.GetIPProperties();
            var ads = props.UnicastAddresses;
            IsIPv4Enabled = ads.Any(a => a.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork);
            IsIPv6Enabled = ads.Any(a => a.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6);
            OriginalMacAddress = GetOriginalMacAddress();
            OriginalVendor = GetOriginalVendor();
            ActiveMacAddress = GetActiveMac();
        }

        public NetworkAdapter(ManagementObject? adapterObject, ManagementObject? adapterConfig, NetworkInterface networkInterface, VendorList? vendorManager = null, bool isPhysicalAdapter = false, AdapterWmiClient? wmiClient = null, AdapterRegistryClient? registryClient = null)
            : this(networkInterface, vendorManager, isPhysicalAdapter, wmiClient, registryClient)
        {
            _adapter = adapterObject;
            _adapterConfig = adapterConfig;
            _enabled = GetEnabled();
            _hardwareId = GetHardwareId();
            _wmiResolved = true;
        }

        ///  <inheritdoc/>
        public override string ToString() => $"{_networkInterface.Description} ({_networkInterface.Name})";

        /// <summary>
        ///     Tries to disable the DHCP of network adapter by setting existing IP address as static.
        /// </summary>
        /// <returns>
        ///     <c>true</c> if the operation succeeds.
        ///     <c>false</c> if the operation fails.
        /// </returns>
        /// <exception cref="Dzmac.CoreException"></exception>
        public bool TryDhcpDisable()
        {
            EnsureWmiObjects();
            if (_adapterConfig == null)
            {
                Diagnostics.Warning("dhcp_disable_skipped", "Adapter configuration object is unavailable.", ("adapter", Name));
                return false;
            }

            if (!TryExtractGateway(out var gateway)
                || !TryExtractIPConfig(out var ipAddress)
                || !TryExtractDnsConfig(out var dnsConfig))
            {
                Diagnostics.Warning("dhcp_disable_skipped", "Required WMI values are incomplete.", ("adapter", Name));
                return false;
            }

            var enableStaticResult = TryInvokeAdapterConfigMethodAndCheckSuccess("EnableStatic", ipAddress, out var enableStaticCode);
            var setGatewayResult = TryInvokeAdapterConfigMethodAndCheckSuccess("SetGateways", gateway, out var setGatewayCode);
            var setDnsResult = TryInvokeAdapterConfigMethodAndCheckSuccess("SetDNSServerSearchOrder", dnsConfig, out var setDnsCode);

            var success = enableStaticResult && setGatewayResult && setDnsResult;

            // Rollback
            if (!success)
            {
                // Set new values
                var newDnsConfig = _adapterConfig.GetMethodParameters("SetDNSServerSearchOrder");
                newDnsConfig["DNSServerSearchOrder"] = Array.Empty<string>();

                // Run command
                var enableDhcpResult = TryInvokeAdapterConfigMethodAndCheckSuccess("EnableDHCP", null, out var enableDhcpCode);
                var emptyDnsResult = TryInvokeAdapterConfigMethodAndCheckSuccess("SetDNSServerSearchOrder", newDnsConfig, out var emptyDnsCode);

                var rollbackSuccess = enableDhcpResult && emptyDnsResult;
                if (rollbackSuccess)
                {
                    Diagnostics.Warning("dhcp_disable_failed_rollback_success", "DHCP disable failed and automatic rollback succeeded.", ("adapter", Name));
                    return false;
                }
                else
                {
                    // something we cannot handle here.
                    Diagnostics.Error("dhcp_disable_failed_rollback_failed", null, "DHCP disable failed and rollback did not complete.", ("adapter", Name));

                    // Display an error dialog to the user
                    throw new DZMACException("Failed to disable DHCP");
                }
            }

            Diagnostics.Info("dhcp_disable_succeeded", ("adapter", Name));
            return true;
        }

        /// <summary>
        ///     Tries to enable the DHCP of network adapter.
        /// </summary>
        /// <returns>
        /// <returns>
        ///     <c>true</c> if the operation succeeds.
        ///     <c>false</c> if the operation fails.
        /// </returns>
        /// <exception cref="Dzmac.CoreException"></exception>
        public bool TryDhcpEnable()
        {
            EnsureWmiObjects();
            if (_adapterConfig == null)
            {
                Diagnostics.Warning("dhcp_enable_skipped", "Adapter configuration object is unavailable.", ("adapter", Name));
                return false;
            }

            // Get old values for rollback
            if (!TryExtractGateway(out var oldGateway)
                || !TryExtractIPConfig(out var oldIpAddress)
                || !TryExtractDnsConfig(out var oldDnsConfig1))
            {
                Diagnostics.Warning("dhcp_enable_skipped", "Required WMI values are incomplete.", ("adapter", Name));
                return false;
            }

            // Set new values
            var newDnsConfig = _adapterConfig.GetMethodParameters("SetDNSServerSearchOrder");
            newDnsConfig["DNSServerSearchOrder"] = Array.Empty<string>();

            // Run command
            var enableDhcpResult = TryInvokeAdapterConfigMethodAndCheckSuccess("EnableDHCP", null, out var enableDhcpCode);
            var emptyDnsResult = TryInvokeAdapterConfigMethodAndCheckSuccess("SetDNSServerSearchOrder", newDnsConfig, out var emptyDnsCode);

            var success = enableDhcpResult && emptyDnsResult;

            // Rollback
            if (!success)
            {
                var rollbackStaticResult = TryInvokeAdapterConfigMethodAndCheckSuccess("EnableStatic", oldIpAddress, out var rollbackStaticCode);
                var rollbackGatewayResult = TryInvokeAdapterConfigMethodAndCheckSuccess("SetGateways", oldGateway, out var rollbackGatewayCode);
                var rollbackDnsResult = TryInvokeAdapterConfigMethodAndCheckSuccess("SetDNSServerSearchOrder", oldDnsConfig1, out var rollbackDnsCode);

                var rollbackSuccess = rollbackStaticResult && rollbackGatewayResult && rollbackDnsResult;
                if (rollbackSuccess)
                {
                    Diagnostics.Warning("dhcp_enable_failed_rollback_success", "DHCP enable failed and automatic rollback succeeded.", ("adapter", Name));
                    return false;
                }
                else
                {
                    // something we cannot handle here.
                    Diagnostics.Error("dhcp_enable_failed_rollback_failed", null, "DHCP enable failed and rollback did not complete.", ("adapter", Name));

                    // Display an error dialog to the user
                    throw new DZMACException("Failed to enable DHCP");
                }
            }

            Diagnostics.Info("dhcp_enable_succeeded", ("adapter", Name));
            return true;
        }

        /// <summary>
        ///     Tries to set static IPv4 address list for the adapter.
        /// </summary>
        public bool TrySetIPv4Addresses(string[] ipAddresses, string[] subnetMasks)
        {
            EnsureWmiObjects();
            if (_adapterConfig == null || ipAddresses == null || subnetMasks == null || ipAddresses.Length == 0 || ipAddresses.Length != subnetMasks.Length)
            {
                return false;
            }

            var parameters = _adapterConfig.GetMethodParameters("EnableStatic");
            parameters["IPAddress"] = ipAddresses;
            parameters["SubnetMask"] = subnetMasks;
            return TryInvokeAdapterConfigMethodAndCheckSuccess("EnableStatic", parameters, out _);
        }

        /// <summary>
        ///     Tries to set IPv4 default gateway list for the adapter.
        /// </summary>
        public bool TrySetIPv4Gateways(string[] gateways, int[] metrics)
        {
            EnsureWmiObjects();
            if (_adapterConfig == null || gateways == null || metrics == null || gateways.Length == 0 || gateways.Length != metrics.Length)
            {
                return false;
            }

            var parameters = _adapterConfig.GetMethodParameters("SetGateways");
            parameters["DefaultIPGateway"] = gateways;
            parameters["GatewayCostMetric"] = metrics;
            return TryInvokeAdapterConfigMethodAndCheckSuccess("SetGateways", parameters, out _);
        }

        /// <summary>
        ///     Tries to set IPv4 DNS server list for the adapter.
        /// </summary>
        public bool TrySetIPv4DnsServers(string[] dnsServers)
        {
            EnsureWmiObjects();
            if (_adapterConfig == null)
            {
                return false;
            }

            var parameters = _adapterConfig.GetMethodParameters("SetDNSServerSearchOrder");
            parameters["DNSServerSearchOrder"] = dnsServers ?? Array.Empty<string>();
            return TryInvokeAdapterConfigMethodAndCheckSuccess("SetDNSServerSearchOrder", parameters, out _);
        }

        /// <summary>
        ///     Tries to release the IP address on specific DHCP-enabled network adapters.
        /// </summary>
        /// <param name="message">Message obtained from return code of the operation.</param>
        /// <returns>
        ///     <c>true</c> if the operation succeeds.
        ///     <c>false</c> if the operation fails.
        /// </returns>
        public bool TryDhcpRelease(out string message)
        {
            EnsureWmiObjects();
            var success = TryInvokeAdapterConfigMethodAndCheckSuccess("ReleaseDHCPLease", null, out var code);
            message = HResult.TranslateErrorCode(code);
            return success;
        }

        /// <summary>
        ///     Tries to renew the IP address on specific DHCP-enabled network adapters.
        /// </summary>
        /// <param name="message">Message obtained from return code of the operation.</param>
        /// <returns>
        ///     <c>true</c> if the operation succeeds.
        ///     <c>false</c> if the operation fails.
        /// </returns>
        public bool TryDhcpRenew(out string message)
        {
            EnsureWmiObjects();
            var success = TryInvokeAdapterConfigMethodAndCheckSuccess("RenewDHCPLease", null, out var code);
            message = HResult.TranslateErrorCode(code);
            return success;
        }

        public IReadOnlyList<AdapterIpv4Address> GetIpv4Addresses()
        {
            var addresses = new List<AdapterIpv4Address>();
            try
            {
                var props = _networkInterface.GetIPProperties();
                foreach (var address in props.UnicastAddresses)
                {
                    if (address.Address.AddressFamily != AddressFamily.InterNetwork)
                    {
                        continue;
                    }

                    var subnetMask = address.IPv4Mask?.ToString() ?? string.Empty;
                    addresses.Add(new AdapterIpv4Address(address.Address.ToString(), subnetMask));
                }
            }
            catch
            {
                Debug.WriteLine($"[{nameof(NetworkAdapter)}] Failed to read IPv4 addresses for adapter '{Name}'.");
            }

            return addresses;
        }

        public IReadOnlyList<AdapterIpv6Address> GetIpv6Addresses()
        {
            var addresses = new List<AdapterIpv6Address>();
            try
            {
                var props = _networkInterface.GetIPProperties();
                foreach (var address in props.UnicastAddresses)
                {
                    if (address.Address.AddressFamily != AddressFamily.InterNetworkV6)
                    {
                        continue;
                    }

                    var normalizedAddress = NormalizeIpAddress(address.Address);
                    if (addresses.Any(a => string.Equals(a.Address, normalizedAddress, StringComparison.OrdinalIgnoreCase)))
                    {
                        continue;
                    }

                    addresses.Add(new AdapterIpv6Address(normalizedAddress, address.PrefixLength));
                }
            }
            catch
            {
                Debug.WriteLine($"[{nameof(NetworkAdapter)}] Failed to read IPv6 addresses for adapter '{Name}'.");
            }

            return addresses;
        }

        public IReadOnlyList<string> GetIpv4Gateways() => GetGateways(AddressFamily.InterNetwork);

        public IReadOnlyList<string> GetIpv6Gateways() => GetGateways(AddressFamily.InterNetworkV6);

        public IReadOnlyList<string> GetIpv4DnsServers() => GetDnsServers(AddressFamily.InterNetwork);

        public IReadOnlyList<string> GetIpv6DnsServers() => GetDnsServers(AddressFamily.InterNetworkV6);

        public bool TryDisableAdapter() => TrySetAdapterEnabled(false);

        public bool TryEnableAdapter() => TrySetAdapterEnabled(true);

        private bool TrySetAdapterEnabled(bool enable)
        {
            var action = enable ? "enable" : "disable";
            EnsureWmiObjects();
            if (_adapter == null)
            {
                Diagnostics.Warning($"adapter_{action}_skipped", "Adapter WMI object is unavailable.", ("adapter", Name));
                return false;
            }

            var method = enable ? "Enable" : "Disable";
            var result = _adapter.InvokeMethod(method, null);
            if (result == null)
            {
                Diagnostics.Warning($"adapter_{action}_failed", $"{(enable ? "Enable" : "Disable")} command returned null.", ("adapter", Name));
                return false;
            }

            var returnCode = SafeConvertToInt(result);
            var success = IsSuccessfulWmiReturnCode(method, returnCode);
            if (success)
            {
                Diagnostics.Info($"adapter_{action}_succeeded", ("adapter", Name));
            }
            else
            {
                Diagnostics.Warning($"adapter_{action}_failed", $"{(enable ? "Enable" : "Disable")} command returned non-zero.", ("adapter", Name));
            }

            return success;
        }

        public bool IsAdapterEnabled()
        {
            EnsureWmiObjects();
            if (_adapter == null)
            {
                return false;
            }

            try
            {
                _adapter.Get();
            }
            catch
            {
                return false;
            }

            return GetEnabled();
        }

        public MacAddress? GetLiveLinkAddress()
        {
            try
            {
                var liveInterface = NetworkInterface.GetAllNetworkInterfaces()
                    .FirstOrDefault(i => string.Equals(i.Id, _networkInterface.Id, StringComparison.OrdinalIgnoreCase));
                if (liveInterface == null)
                {
                    return null;
                }

                var physicalAddressBytes = liveInterface.GetPhysicalAddress().GetAddressBytes();
                if (physicalAddressBytes.Length != 6)
                {
                    return null;
                }
                
                return new MacAddress(MacAddress.MacToString(physicalAddressBytes));
            }
            catch
            {
                return null;
            }
        }

        public void EnsureNetworkAddressRegistryParameter()
        {
            var registryKey = GetRegistryKey();
            _registryClient.EnsureNetworkAddressParameter(registryKey);
        }

        /// <summary>
        ///     Sets and resets the NetworkAddress registry value of this adapter.
        /// </summary>
        /// <param name="mac">
        ///     The nullable value either an instance of <see cref="MacAddress"/>.
        ///     or null (to restore).
        /// </param>
        /// <returns>
        ///     <c>true</c> if the operation succeeds.
        ///     <c>false</c> if the operation fails.
        /// </returns>
        /// <exception cref="Dzmac.CoreException"> </exception>
        public bool TrySetRegistryMac(MacAddress? mac, bool persistOriginalRecord = true)
        {
            var shouldReenable = false;

            try
            {
                var targetMac = mac != null ? mac.ToString() : string.Empty;
                Diagnostics.Info("registry_mac_update_started", ("adapter", Name), ("targetMac", string.IsNullOrEmpty(targetMac) ? "<restore>" : targetMac));
                return UpdateRegistryMac(GetRegistryKey(), targetMac, _networkInterface.Description, persistOriginalRecord, out shouldReenable);
            }
            catch (Exception ex)
            {
                Diagnostics.Error("registry_mac_update_failed", ex, null, ("adapter", Name));
                throw new DZMACException(ex.Message, ex);
            }
            finally
            {
                if (shouldReenable && !TryEnableAdapter())
                {
                    Diagnostics.Error("adapter_reenable_failed", null, "Failed to re-enable network adapter.", ("adapter", Name));
                }
            }
        }

        /// <summary>
        ///     Deletes this adapter's registry key from the network adapter class hive.
        /// </summary>
        /// <returns>
        ///     A tuple where <c>Success</c> indicates whether deletion succeeded and
        ///     <c>Message</c> contains a user-facing result message.
        /// </returns>
        public (bool Success, string Message) TryDeleteFromRegistry()
        {
            try
            {
                var registryKey = GetRegistryKey();
                if (string.IsNullOrWhiteSpace(registryKey))
                {
                    return (false, "Could not locate the adapter's registry key.");
                }

                if (!_registryClient.TryValidateAdapterDescription(registryKey, _networkInterface.Description))
                {
                    return (false, "Adapter validation failed for the selected registry key.");
                }

                _registryClient.DeleteKeyTree(registryKey);
                return (true, "Deleted adapter registry key.");
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        private int ExtractDeviceNumber()
        {
            if (_adapter == null)
            {
                return -1;
            }

            // Extract adapter number; this should correspond to the keys under HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Class\{4d36e972-e325-11ce-bfc1-08002be10318}
            Match? match;
            try
            {
                match = _adapterNumberPattern.Match(_adapter.Path.RelativePath);
            }
            catch (RegexMatchTimeoutException)
            {
                return -1;
            }

            if (match == null || !int.TryParse(match.Groups[1].Value, out var deviceNumber))
            {
                return -1;
            }

            return deviceNumber;
        }

        private bool TryExtractDnsConfig(out ManagementBaseObject? dnsParams)
        {
            dnsParams = null;
            if (_adapterConfig == null)
            {
                Debug.WriteLine($"[{nameof(NetworkAdapter)}] DNS extraction failed for adapter '{Name}': adapter config object is null.");
                return false;
            }

            var dnsServerSearchOrder = _adapterConfig.GetPropertyValue("DNSServerSearchOrder") as string[] ?? Array.Empty<string>();
            dnsParams = _adapterConfig.GetMethodParameters("SetDNSServerSearchOrder");
            dnsParams["DNSServerSearchOrder"] = dnsServerSearchOrder;
            return true;
        }

        private bool TryExtractGateway(out ManagementBaseObject? gatewayParams)
        {
            gatewayParams = null;
            if (_adapterConfig == null)
            {
                Debug.WriteLine($"[{nameof(NetworkAdapter)}] Gateway extraction failed for adapter '{Name}': adapter config object is null.");
                return false;
            }

            var gatewayAddress = _adapterConfig.GetPropertyValue("DefaultIPGateway") as string[] ?? Array.Empty<string>();
            gatewayParams = _adapterConfig.GetMethodParameters("SetGateways");
            gatewayParams["DefaultIPGateway"] = gatewayAddress;
            return true;
        }

        private bool TryExtractIPConfig(out ManagementBaseObject? ipParams)
        {
            ipParams = null;
            if (_adapterConfig == null)
            {
                Debug.WriteLine($"[{nameof(NetworkAdapter)}] IP extraction failed for adapter '{Name}': adapter config object is null.");
                return false;
            }
            if (!(_adapterConfig.GetPropertyValue("IPAddress") is string[] ipAddresses) || !(_adapterConfig.GetPropertyValue("IPSubnet") is string[] subnetMasks) || ipAddresses.Length == 0 || subnetMasks.Length == 0)
            {
                Debug.WriteLine($"[{nameof(NetworkAdapter)}] IP extraction failed for adapter '{Name}': missing IPAddress/IPSubnet values.");
                return false;
            }

            var ipAddress = ipAddresses[0];
            var subnetMask = subnetMasks[0];

            ipParams = _adapterConfig.GetMethodParameters("EnableStatic");
            ipParams["IPAddress"] = new string[] { ipAddress };
            ipParams["SubnetMask"] = new string[] { subnetMask };
            return true;
        }

        private MacAddress? GetActiveMac()
        {
            object address;
            try
            {
                var registryKey = GetRegistryKey();
                if (string.IsNullOrWhiteSpace(registryKey))
                {
                    return null;
                }

                address = _registryClient.ReadValue(registryKey, RegistryMacOverrideValueName);
                if (address == null)
                {
                    return null;
                }
            }
            catch
            {
                Debug.WriteLine($"[{nameof(NetworkAdapter)}] Failed to read active MAC from registry for adapter '{Name}'.");
                return null;
            }
            var macString = address.ToString().Replace("-", string.Empty).ToUpperInvariant();
            return new MacAddress(macString);
        }

        private string? GetActiveVendor()
        {
            if (ActiveMacAddress == null)
            {
                return OriginalVendor;
            }

            if (_manager == null)
            {
                return null;
            }

            var vendor = _manager.FindByMac(ActiveMacAddress, true);
            if (vendor == null)
            {
                return UnknownVendorIdentifier;
            }

            return vendor.Value.VendorName;
        }

        private bool GetEnabled()
        {
            var enabled = _adapter?["NetEnabled"];
            if (enabled is bool netEnabled)
            {
                return netEnabled;
            }

            // CIM_NetworkAdapter fallback: 2 = Enabled
            var enabledState = _adapter?["EnabledState"];
            if (enabledState != null && ushort.TryParse(enabledState.ToString(), out var state))
            {
                return state == 2;
            }

            return false;
        }

        private string GetHardwareId()
        {
            var hardwareId = _adapter?["PNPDeviceID"];
            if (hardwareId is string pnpDeviceId)
            {
                return pnpDeviceId;
            }

            // CIM_NetworkAdapter fallback
            var deviceId = _adapter?["DeviceID"];
            if (deviceId is string cimDeviceId)
            {
                return cimDeviceId;
            }

            return string.Empty;
        }

        private MacAddress GetOriginalMacAddress()
        {
            // The Registry key value "OriginalNetworkAddress" is created by TMAC,
            // Therefore, we need to check if it exists first, then query the value from
            // internal objects.
            // Ref: https://blog.technitium.com/2014/06/fixing-wrong-original-mac-address-in.html
            try
            {
                var registryKey = GetRegistryKey();
                if (string.IsNullOrWhiteSpace(registryKey))
                {
                    return TryGetPhysicalMacAddressOrUnknown();
                }

                var address = _registryClient.ReadValue(registryKey, TmacOriginalMacValueName);
                if (address != null)
                {
                    var macString = address.ToString().Replace("-", string.Empty).ToUpperInvariant();
                    return new MacAddress(macString);
                }
            }
            catch
            {
                Debug.WriteLine($"[{nameof(NetworkAdapter)}] Failed to read original MAC from registry for adapter '{Name}'. Falling back to physical address.");
                return TryGetPhysicalMacAddressOrUnknown();
            }

            return TryGetPhysicalMacAddressOrUnknown();
        }

        private MacAddress TryGetPhysicalMacAddressOrUnknown()
        {
            var physicalAddressBytes = _networkInterface.GetPhysicalAddress().GetAddressBytes();
            if (physicalAddressBytes.Length != 6)
            {
                return UnknownMacAddress;
            }

            return new MacAddress(MacAddress.MacToString(physicalAddressBytes));
        }

        private string? GetOriginalVendor()
        {
            if (OriginalMacAddress == null)
            {
                return null;
            }

            if (_manager == null)
            {
                return null;
            }

            var vendor = _manager.FindByMac(OriginalMacAddress);

            if (vendor == null)
            {
                return UnknownVendorIdentifier;
            }

            return vendor?.VendorName;
        }

        /// <summary>
        ///     Gets the registry key associated to this adapter.
        /// </summary>
        private string GetRegistryKey()
        {
            lock (_syncRoot)
            {
                if (_registryResolved)
                {
                    return _registryKey ?? string.Empty;
                }

                _registryResolved = true;
                _registryKey = TryResolveRegistryKey();

                if (string.IsNullOrWhiteSpace(_registryKey) && !_wmiResolved)
                {
                    EnsureWmiObjects();
                    var deviceNumber = ExtractDeviceNumber();
                    if (deviceNumber >= 0)
                    {
                        _registryKey = $@"{RegistryClassKey}\{deviceNumber:D4}";
                    }
                }

                return _registryKey ?? string.Empty;
            }
        }

        private string? TryResolveRegistryKey()
        {
            var registryKey = _registryClient.TryResolveRegistryKey(RegistryClassKey, ConfigId);
            if (!string.IsNullOrWhiteSpace(registryKey))
            {
                return registryKey;
            }

            Debug.WriteLine($"[{nameof(NetworkAdapter)}] Failed to resolve registry key from class map for adapter '{Name}'.");
            return null;
        }

        private void EnsureWmiObjects()
        {
            lock (_syncRoot)
            {
                if (_wmiResolved)
                {
                    return;
                }

                _wmiResolved = true;
                if (!_wmiClient.TryResolveByConfigId(ConfigId, out var adapterObject, out var adapterConfigObject))
                {
                    Debug.WriteLine($"[{nameof(NetworkAdapter)}] Failed to resolve WMI objects for adapter '{Name}'.");
                    _adapter = null;
                    _adapterConfig = null;
                }
                else
                {
                    _adapter = adapterObject;
                    _adapterConfig = adapterConfigObject;
                }

                _enabled = GetEnabled();
                _hardwareId = GetHardwareId();
            }
        }

        internal static ManagementObject? CreateBoundManagementObject(ManagementObject? sourceObject)
        {
            if (sourceObject == null)
            {
                return null;
            }

            try
            {
                var boundObject = new ManagementObject(sourceObject.Path.Path);
                boundObject.Get();
                return boundObject;
            }
            catch
            {
                return null;
            }
        }

        private int SafeConvertToInt(object obj)
        {
            try
            {
                return Convert.ToInt32(obj);
            }
            catch
            {
                Debug.WriteLine($"[{nameof(NetworkAdapter)}] Failed to convert WMI return code to int.");
                return -1;
            }
        }

        private bool TryInvokeAdapterConfigMethod(string methodName, ManagementBaseObject? inParameters, out int returnCode)
        {
            returnCode = -1;
            if (_adapterConfig == null)
            {
                Diagnostics.Warning("adapter_config_method_skipped", "Adapter configuration object is unavailable.", ("adapter", Name), ("method", methodName));
                return false;
            }

            try
            {
                var result = _adapterConfig.InvokeMethod(methodName, inParameters, null);
                if (result == null)
                {
                    Diagnostics.Warning("adapter_config_method_failed", "WMI method returned null result.", ("adapter", Name), ("method", methodName));
                    return false;
                }

                returnCode = SafeConvertToInt(result.GetPropertyValue("ReturnValue"));
                return true;
            }
            catch (InvalidOperationException ex)
            {
                Diagnostics.Warning("adapter_config_method_invalid_operation", ex.Message, ("adapter", Name), ("method", methodName));
                return false;
            }
            catch (ManagementException ex)
            {
                Diagnostics.Warning("adapter_config_method_management_exception", ex.Message, ("adapter", Name), ("method", methodName));
                return false;
            }
        }

        private bool TryInvokeAdapterConfigMethodAndCheckSuccess(string methodName, ManagementBaseObject? inParameters, out int returnCode)
        {
            if (!TryInvokeAdapterConfigMethod(methodName, inParameters, out returnCode))
            {
                return false;
            }

            if (IsSuccessfulWmiReturnCode(methodName, returnCode))
            {
                return true;
            }

            Diagnostics.Warning(
                "adapter_config_method_non_success_code",
                "WMI method returned non-success code.",
                ("adapter", Name),
                ("method", methodName),
                ("returnCode", returnCode));
            return false;
        }

        private static bool IsSuccessfulWmiReturnCode(string methodName, int returnCode)
        {
            if (StandardSuccessCodes.Contains(returnCode))
            {
                return true;
            }

            // Some providers can report "already in requested state" during Enable/Disable.
            if ((string.Equals(methodName, "Enable", StringComparison.OrdinalIgnoreCase)
                || string.Equals(methodName, "Disable", StringComparison.OrdinalIgnoreCase))
                && returnCode == 5)
            {
                return true;
            }

            return false;
        }

        private IReadOnlyList<string> GetGateways(AddressFamily family)
        {
            var gateways = new List<string>();
            try
            {
                var props = _networkInterface.GetIPProperties();
                foreach (var gatewayAddress in props.GatewayAddresses)
                {
                    if (gatewayAddress.Address.AddressFamily == family)
                    {
                        var normalizedAddress = NormalizeIpAddress(gatewayAddress.Address);
                        if (!gateways.Any(g => string.Equals(g, normalizedAddress, StringComparison.OrdinalIgnoreCase)))
                        {
                            gateways.Add(normalizedAddress);
                        }
                    }
                }
            }
            catch
            {
                Debug.WriteLine($"[{nameof(NetworkAdapter)}] Failed to read gateway addresses for adapter '{Name}'.");
            }

            return gateways;
        }

        private IReadOnlyList<string> GetDnsServers(AddressFamily family)
        {
            var dnsServers = new List<string>();
            try
            {
                var props = _networkInterface.GetIPProperties();
                foreach (var dnsAddress in props.DnsAddresses)
                {
                    if (dnsAddress.AddressFamily == family)
                    {
                        var normalizedAddress = NormalizeIpAddress(dnsAddress);
                        if (!dnsServers.Any(d => string.Equals(d, normalizedAddress, StringComparison.OrdinalIgnoreCase)))
                        {
                            dnsServers.Add(normalizedAddress);
                        }
                    }
                }
            }
            catch
            {
                Debug.WriteLine($"[{nameof(NetworkAdapter)}] Failed to read DNS server addresses for adapter '{Name}'.");
            }

            return dnsServers;
        }

        private static string NormalizeIpAddress(IPAddress address)
        {
            var formatted = address.ToString();

            // Link-local IPv6 addresses may include a scope ID suffix (e.g. "%12").
            // TMAC-style display should show the raw address without interface scope marker.
            if (address.AddressFamily == AddressFamily.InterNetworkV6)
            {
                var scopeSeparator = formatted.IndexOf('%');
                if (scopeSeparator >= 0)
                {
                    return formatted.Substring(0, scopeSeparator);
                }
            }

            return formatted;
        }

        /// <summary>
        ///     Registry helper function
        /// </summary>
        /// <param name="registryKey">Registry key path to access</param>
        /// <param name="newMac">New mac address without delimiters or empty string to reset.</param>
        /// <param name="description">Network adapter description to find the adapter.</param>
        /// <param name="shouldReenable">A bool value to trigger Disable/Enable commands</param>
        /// <exception cref="Dzmac.CoreException"></exception>
        /// <exception cref="System.Security.SecurityException"></exception>
        /// <exception cref="System.IO.IOException"></exception>
        /// <exception cref="UnauthorizedAccessException"></exception>
        private bool UpdateRegistryMac(string registryKey, string newMac, string description, bool persistOriginalRecord, out bool shouldReenable)
        {
            // If the value is not the empty string, we want to set NetworkAddress to it, so it had
            // better be valid
            if (newMac.Length > 0 && !MacAddress.IsValidMac(newMac))
            {
                throw new DZMACException(newMac + " is not a valid mac address");
            }

            // Sanity check
            if (!_registryClient.TryValidateAdapterDescription(registryKey, description))
            {
                throw new DZMACException("Adapter not found in registry");
            }

            // Attempt to disable the adapter
            if (!TryDisableAdapter())
            {
                throw new DZMACException("Failed to disable network adapter.");
            }

            // If we're here the adapter has been disabled, so we set the flag that will
            // re-enable it in the finally block
            shouldReenable = true;

            ApplyRegistryMacValues(registryKey, newMac, persistOriginalRecord);
            return true;
        }

        public bool TryUpdateRegistryMacValue(MacAddress? mac, bool persistOriginalRecord = true)
        {
            var targetMac = mac != null ? mac.ToString() : string.Empty;
            var registryKey = GetRegistryKey();

            if (!_registryClient.TryValidateAdapterDescription(registryKey, _networkInterface.Description))
            {
                throw new DZMACException("Adapter not found in registry");
            }

            if (targetMac.Length > 0 && !MacAddress.IsValidMac(targetMac))
            {
                throw new DZMACException(targetMac + " is not a valid mac address");
            }

            ApplyRegistryMacValues(registryKey, targetMac, persistOriginalRecord);
            return true;
        }

        private void ApplyRegistryMacValues(string registryKey, string targetMac, bool persistOriginalRecord)
        {
            if (targetMac.Length == 0)
            {
                _registryClient.DeleteValue(registryKey, RegistryMacOverrideValueName);
                _registryClient.DeleteValue(registryKey, TmacOriginalMacValueName);
                return;
            }

            if (persistOriginalRecord)
            {
                _registryClient.SetStringValue(registryKey, TmacOriginalMacValueName, OriginalMacAddress.ToString());
            }
            else
            {
                _registryClient.DeleteValue(registryKey, TmacOriginalMacValueName);
            }

            _registryClient.SetStringValue(registryKey, RegistryMacOverrideValueName, targetMac);
        }

        #region Dispose

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _adapter?.Dispose();
                    _adapterConfig?.Dispose();
                }

                _adapter = null;
                _adapterConfig = null;

                disposedValue = true;
            }
        }

        #endregion Dispose
    }
}
