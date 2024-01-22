using System;
using System.Linq;
using System.Management;
using System.Net.NetworkInformation;
using System.Text.RegularExpressions;
using Microsoft.Win32;

#nullable enable
namespace MacChanger
{
    /// <summary>
    ///     Represents a Windows network interface. Wrapper around the .NET API for network
    ///     interfaces, as well as for the unmanaged device.
    /// </summary>
    /// <remarks>
    ///     The <see cref="NetworkAdapter" /> implementation is adapted from <see
    ///     href="https://github.com/sietseringers/MACAddressTool" />.
    /// </remarks>
    public class NetworkAdapter : IDisposable
    {
        private const string UnknownVendorIdentifier = "Unkown Vendor";

        private static readonly Regex _adapterNumberPattern = new Regex("\\\"(\\d+)\\\"$");

        private readonly ManagementObject? _adapterConfig;

        private readonly VendorManager? _manager;

        private readonly NetworkInterface _networkInterface;

        private readonly string _registryKey;

        private ManagementObject? _adapter;

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
        ///     Gets if the adapter is enabled or not.
        /// </summary>
        public bool Enabled { get; }

        /// <summary>
        ///     Gets the Win32 Plug and Play device ID of the logical device.
        /// </summary>
        public string HardwareId { get; }

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
                    return false;
                }
            }
        }

        public bool IsIPv4Enabled { get; }
        public bool IsIPv6Enabled { get; }
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
        public NetworkAdapter(ManagementObject adapterObject, ManagementObject adapterConfig, NetworkInterface networkInterface, VendorManager? vendorManager = null)
        {
            _adapter = adapterObject;
            _adapterConfig = adapterConfig;
            _networkInterface = networkInterface;
            _manager = vendorManager;

            _registryKey = GetRegistryKey();

            Enabled = GetEnabled();
            Name = _networkInterface.Name;
            HardwareId = GetHardwareId();

            var props = _networkInterface.GetIPProperties();
            var ads = props.UnicastAddresses;
            IsIPv4Enabled = ads.Any(a => a.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork);
            IsIPv6Enabled = ads.Any(a => a.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6);
            OriginalMacAddress = GetOriginalMacAddress();
            OriginalVendor = GetOriginalVendor();
            ActiveMacAddress = GetActiveMac();
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
        /// <exception cref="MacChangerException"></exception>
        public bool TryDhcpDisable()
        {
            if (_adapterConfig == null)
            {
                return false;
            }

            var gateway = ExtractGateway();
            var ipAddress = ExtractIPConfig();
            var dnsConfig = ExtractDnsConfig();

            var enableStaticResult = SafeConvertToInt(_adapterConfig.InvokeMethod("EnableStatic", ipAddress, null).GetPropertyValue("ReturnValue")) == 0;
            var setGatewayResult = SafeConvertToInt(_adapterConfig.InvokeMethod("SetGateways", gateway, null).GetPropertyValue("ReturnValue")) == 0;
            var setDnsResult = SafeConvertToInt(_adapterConfig.InvokeMethod("SetDNSServerSearchOrder", dnsConfig, null).GetPropertyValue("ReturnValue")) == 0;

            var success = enableStaticResult && setGatewayResult && setDnsResult;

            // Rollback
            if (!success)
            {
                // Set new values
                var newDnsConfig = _adapterConfig.GetMethodParameters("SetDNSServerSearchOrder");
                newDnsConfig["DNSServerSearchOrder"] = Array.Empty<string>();

                // Run command
                var enableDhcpResult = SafeConvertToInt(_adapterConfig.InvokeMethod("EnableDHCP", null, null).GetPropertyValue("ReturnValue")) == 0;
                var emptyDnsResult = SafeConvertToInt(_adapterConfig.InvokeMethod("SetDNSServerSearchOrder", newDnsConfig, null).GetPropertyValue("ReturnValue")) == 0;

                var rollbackSuccess = enableDhcpResult && emptyDnsResult;
                if (rollbackSuccess)
                {
                    return false;
                }
                else
                {
                    // something we cannot handle here.
                    throw new MacChangerException("Failed to disable DHCP");
                }
            }

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
        /// <exception cref="MacChangerException"></exception>
        public bool TryDhcpEnable()
        {
            if (_adapterConfig == null)
            {
                return false;
            }

            // Get old values for rollback
            var oldGateway = ExtractGateway();
            var oldIpAddress = ExtractIPConfig();
            var oldDnsConfig1 = ExtractDnsConfig();

            // Set new values
            var newDnsConfig = _adapterConfig.GetMethodParameters("SetDNSServerSearchOrder");
            newDnsConfig["DNSServerSearchOrder"] = Array.Empty<string>();

            // Run command
            var enableDhcpResult = SafeConvertToInt(_adapterConfig.InvokeMethod("EnableDHCP", null, null).GetPropertyValue("ReturnValue")) == 0;
            var emptyDnsResult = SafeConvertToInt(_adapterConfig.InvokeMethod("SetDNSServerSearchOrder", newDnsConfig, null).GetPropertyValue("ReturnValue")) == 0;

            var success = enableDhcpResult && emptyDnsResult;

            // Rollback
            if (!success)
            {
                var rollbackStaticResult = SafeConvertToInt(_adapterConfig.InvokeMethod("EnableStatic", oldIpAddress, null).GetPropertyValue("ReturnValue")) == 0;
                var rollbackGatewayResult = SafeConvertToInt(_adapterConfig.InvokeMethod("SetGateways", oldGateway, null).GetPropertyValue("ReturnValue")) == 0;
                var rollbackDnsResult = SafeConvertToInt(_adapterConfig.InvokeMethod("SetDNSServerSearchOrder", oldDnsConfig1, null).GetPropertyValue("ReturnValue")) == 0;

                var rollbackSuccess = rollbackStaticResult && rollbackGatewayResult && rollbackDnsResult;
                if (rollbackSuccess)
                {
                    return false;
                }
                else
                {
                    // something we cannot handle here.
                    throw new MacChangerException("Failed to enable DHCP");
                }
            }

            return true;
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
            message = string.Empty;
            if (_adapter == null)
            {
                return false;
            }

            var resultObject = _adapter.InvokeMethod("ReleaseDHCPLease", null);
            if (resultObject == null)
            {
                return false;
            }

            var resultCode = SafeConvertToInt(resultObject);

            message = HResult.TranslateErrorCode(resultCode);

            return resultCode == 0 || resultCode == 1;
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
            message = string.Empty;
            if (_adapter == null)
            {
                return false;
            }

            var resultObject = _adapter.InvokeMethod("RenewDHCPLease", null);
            if (resultObject == null)
            {
                return false;
            }

            var resultCode = SafeConvertToInt(resultObject);

            message = HResult.TranslateErrorCode(resultCode);

            return resultCode == 0 || resultCode == 1;
        }

        /// <summary>
        ///     Tries to disable the network adapter.
        /// </summary>
        /// <returns>
        ///     <c>true</c> if the operation succeeds.
        ///     <c>false</c> if the operation fails.
        /// </returns>
        public bool TryDisableAdapter()
        {
            if (_adapter == null)
            {
                return false;
            }

            var result = _adapter.InvokeMethod("Disable", null);
            if (result == null)
            {
                return false;
            }

            return SafeConvertToInt(result) == 0;
        }

        /// <summary>
        ///     Tries to enable the network adapter.
        /// </summary>
        /// <returns>
        ///     <c>true</c> if the operation succeeds.
        ///     <c>false</c> if the operation fails.
        /// </returns>
        public bool TryEnableAdapter()
        {
            if (_adapter == null)
            {
                return false;
            }

            var result = _adapter.InvokeMethod("Enable", null);
            if (result == null)
            {
                return false;
            }

            return SafeConvertToInt(result) == 0;
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
        /// <exception cref="MacChangerException"> </exception>
        public bool TrySetRegistryMac(MacAddress? mac)
        {
            var shouldReenable = false;

            try
            {
                var targetMac = mac != null ? mac.ToString() : string.Empty;
                return UpdateRegistryMac(_registryKey, targetMac, _networkInterface.Description, out shouldReenable);
            }
            catch (Exception ex)
            {
                throw new MacChangerException(ex.Message, ex);
            }
            finally
            {
                if (shouldReenable && !TryEnableAdapter())
                {
                    throw new MacChangerException("Failed to re-enable network adapter.");
                }
            }
        }

        private int ExtractDeviceNumber()
        {
            if (_adapter == null)
            {
                return -1;
            }

            // Extract adapter number; this should correspond to the keys under HKEY_LOCAL_MACHINE\SYSTEM\ControlSet001\Control\Class\{4d36e972-e325-11ce-bfc1-08002be10318}
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

        private ManagementBaseObject ExtractDnsConfig()
        {
            // Get current values
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            var dnsServerSearchOrder = (string[])_adapterConfig.GetPropertyValue("DNSServerSearchOrder");
#pragma warning restore CS8602 // Dereference of a possibly null reference.

            // Create parameter object
            var dnsParams = _adapterConfig.GetMethodParameters("SetDNSServerSearchOrder");
            dnsParams["DNSServerSearchOrder"] = dnsServerSearchOrder;
            return dnsParams;
        }

        private ManagementBaseObject ExtractGateway()
        {
            // Get current values
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            var gatewayAddress = (string[])_adapterConfig.GetPropertyValue("DefaultIPGateway");
#pragma warning restore CS8602 // Dereference of a possibly null reference.

            // Create parameter object
            var gatewayParams = _adapterConfig.GetMethodParameters("SetGateways");
            gatewayParams["DefaultIPGateway"] = gatewayAddress;
            return gatewayParams;
        }

        private ManagementBaseObject ExtractIPConfig()
        {
            // Get current values
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            var ipAddress = ((string[])_adapterConfig.GetPropertyValue("IPAddress"))[0];
            var subnetMask = ((string[])_adapterConfig.GetPropertyValue("IPSubnet"))[0];
#pragma warning restore CS8602 // Dereference of a possibly null reference.

            // Create parameter object
            var ipParams = _adapterConfig.GetMethodParameters("EnableStatic");
            ipParams["IPAddress"] = new string[] { ipAddress };
            ipParams["SubnetMask"] = new string[] { subnetMask };
            return ipParams;
        }

        private MacAddress? GetActiveMac()
        {
            object address;
            try
            {
                using var regkey = Registry.LocalMachine.OpenSubKey(_registryKey, false);
                if (regkey == null)
                {
                    return null;
                }

                address = regkey.GetValue("NetworkAddress");
                if (address == null)
                {
                    return null;
                }
            }
            catch
            {
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

            return vendor.ToString();
        }

        private bool GetEnabled()
        {
            var enabled = _adapter?["NetEnabled"];
            return enabled != null && (bool)enabled;
        }

        private string GetHardwareId()
        {
            var hardwareId = _adapter?["PNPDeviceID"];
            return hardwareId == null ? string.Empty : (string)hardwareId;
        }

        private MacAddress GetOriginalMacAddress()
        {
            // The Registry key value "OriginalNetworkAddress" is created by TMAC,
            // Therefore, we need to check if it exists first, then query the value from
            // internal objects.
            // Ref: https://blog.technitium.com/2014/06/fixing-wrong-original-mac-address-in.html
            try
            {
                using var regkey = Registry.LocalMachine.OpenSubKey(_registryKey, false);
                var address = regkey.GetValue("OriginalNetworkAddress");
                if (address != null)
                {
                    var macString = address.ToString().Replace("-", string.Empty).ToUpperInvariant();
                    return new MacAddress(macString);
                }
            }
            catch
            {
                return new MacAddress(_networkInterface.GetPhysicalAddress());
            }

            return new MacAddress(_networkInterface.GetPhysicalAddress());
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

            return vendor.ToString();
        }

        /// <summary>
        ///     Gets the registry key associated to this adapter.
        /// </summary>
        private string GetRegistryKey() => $@"SYSTEM\ControlSet001\Control\Class\{{4D36E972-E325-11CE-BFC1-08002BE10318}}\{ExtractDeviceNumber():D4}";

        private int SafeConvertToInt(object obj)
        {
            try
            {
                return Convert.ToInt32(obj);
            }
            catch
            {
                return -1;
            }
        }
        /// <summary>
        ///     Registry helper function
        /// </summary>
        /// <param name="registryKey">Registry key path to access</param>
        /// <param name="newMac">New mac address without delimiters or empty string to reset.</param>
        /// <param name="description">Network adapter description to find the adapter.</param>
        /// <param name="shouldReenable">A bool value to trigger Disable/Enable commands</param>
        /// <exception cref="MacChangerException"></exception>
        /// <exception cref="System.Security.SecurityException"></exception>
        /// <exception cref="System.IO.IOException"></exception>
        /// <exception cref="UnauthorizedAccessException"></exception>
        private bool UpdateRegistryMac(string registryKey, string newMac, string description, out bool shouldReenable)
        {
            // If the value is not the empty string, we want to set NetworkAddress to it, so it had
            // better be valid
            if (newMac.Length > 0 && !MacAddress.IsValidMac(newMac))
            {
                throw new MacChangerException(newMac + " is not a valid mac address");
            }

            using var regkey = Registry.LocalMachine.OpenSubKey(registryKey, true) ?? throw new MacChangerException("Failed to open the registry key");

            // Sanity check
            if (regkey.GetValue("AdapterModel") as string != description
                && regkey.GetValue("DriverDesc") as string != description)
            {
                throw new MacChangerException("Adapter not found in registry");
            }

            // Attempt to disable the adapter
            if (!TryDisableAdapter())
            {
                throw new MacChangerException("Failed to disable network adapter.");
            }

            // If we're here the adapter has been disabled, so we set the flag that will
            // re-enable it in the finally block
            shouldReenable = true;

            // If we're here everything is OK; update or clear the registry value
            if (newMac.Length > 0)
            {
                // rollback value
                regkey.SetValue("OriginalNetworkAddress", OriginalMacAddress.ToString(), RegistryValueKind.String);

                // active value
                regkey.SetValue("NetworkAddress", newMac, RegistryValueKind.String);
            }
            else
            {
                regkey.DeleteValue("NetworkAddress");
                regkey.DeleteValue("OriginalNetworkAddress");
            }

            return true;
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

                disposedValue = true;
            }
        }

        #endregion Dispose
    }
}