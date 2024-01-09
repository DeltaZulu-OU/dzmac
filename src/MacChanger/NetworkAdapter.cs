using System;
using System.Linq;
using System.Management;
using System.Net.NetworkInformation;
using System.Text.RegularExpressions;
using Microsoft.Win32;

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
        private readonly VendorManager _manager;
        private readonly NetworkInterface _networkInterface;
        private ManagementObject _adapter;
        private bool disposedValue;

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
        public string OriginalMacAddress { get; }
        /// <summary>
        ///     Gets the vendor of the network adapter.
        /// </summary>
        public string OriginalVendor { get; }

        /// <summary>
        ///     Gets the NetworkAddress registry value of this adapter.
        /// </summary>
        public string RegistryMacAddress { get; }

        /// <summary>
        ///     Gets the vendor of the network adapter from registry.
        /// </summary>
        public string RegistryVendor { get; }

        /// <summary>
        ///     Gets the speed of the network interface.
        /// </summary>
        public long Speed => _networkInterface.Speed;

        public NetworkAdapter(ManagementObject adapterObject, NetworkInterface networkInterface, VendorManager vendorManager = null)
        {
            _adapter = adapterObject;
            _networkInterface = networkInterface;
            _manager = vendorManager;

            Enabled = GetEnabled();
            Name = _networkInterface.Name;
            HardwareId = GetHardwareId();
            var props = _networkInterface.GetIPProperties();
            var ads = props.UnicastAddresses;
            IsIPv4Enabled = ads.Any(a => a.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork);
            IsIPv6Enabled = ads.Any(a => a.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6);
            OriginalMacAddress = GetOriginalMacAddress();
            OriginalVendor = GetOriginalVendor();
            RegistryMacAddress = GetRegistryMac();
            RegistryVendor = GetRegistryVendor();
        }

        /// <summary>
        ///     Sets the NetworkAddress registry value of this adapter.
        /// </summary>
        /// <param name="value">
        ///     The value. Should be EITHER a string of 12 hexadecimal digits, uppercase, without
        ///     dashes, dots or anything else, OR an empty string (clears the registry value).
        /// </param>
        /// <returns> true if successful, false otherwise </returns>
        /// <exception cref="MacChangerException"> </exception>
        public bool SetRegistryMac(string value)
        {
            var shouldReenable = false;

            try
            {
                return UpdateRegistryMac(GetregistryKey(), value, _networkInterface.Description, out shouldReenable);
            }
            catch (Exception ex)
            {
                throw new MacChangerException(ex.Message, ex);
            }
            finally
            {
                if (shouldReenable && !TryEnable())
                {
                    throw new MacChangerException("Failed to re-enable network adapter.");
                }
            }
        }

        public override string ToString() => $"{_networkInterface.Description} ({_networkInterface.Name})";

        /// <summary>
        ///     Disables the network adapter If the adapter is already disabled, it will return
        ///     false. If the disable operation fails, it will return false.
        /// </summary>
        /// <returns>
        ///     True if the disable operation succees. False if the adapter is already disabled or
        ///     the operation is failed.
        /// </returns>
        public bool TryDisable()
        {
            if (!Enabled)
            {
                return false;
            }

            var result = _adapter.InvokeMethod("Disable", null);
            return (int)result == 0;
        }

        /// <summary>
        ///     Enables the network adapter If the adapter is already enabled, it will return false.
        ///     If the enable operation fails, it will return false.
        /// </summary>
        /// <returns>
        ///     True if the enable operation succees. False if the adapter is already enabled or the
        ///     operation is failed.
        /// </returns>
        public bool TryEnable()
        {
            if (Enabled)
            {
                return false;
            }

            var result = _adapter.InvokeMethod("Enable", null);
            return (int)result == 0;
        }

        private int ExtractDeviceNumber()
        {
            // Extract adapter number; this should correspond to the keys under HKEY_LOCAL_MACHINE\SYSTEM\ControlSet001\Control\Class\{4d36e972-e325-11ce-bfc1-08002be10318}

            var match = Regex.Match(_adapter.Path.RelativePath, "\\\"(\\d+)\\\"$");
            if (int.TryParse(match.Groups[1].Value, out var deviceNumber))
            {
                return deviceNumber;
            }
            else
            {
                return -1;
            }
        }

        private bool GetEnabled()
        {
            var enabled = _adapter["NetEnabled"];
            return enabled != null && (bool)enabled;
        }

        private string GetHardwareId()
        {
            var hardwareId = _adapter["PNPDeviceID"];
            return hardwareId == null ? string.Empty : (string)hardwareId;
        }

        private string GetOriginalMacAddress() => _networkInterface.GetPhysicalAddress()?.ToString();

        private string GetOriginalVendor()
        {
            var vendor = _adapter["Manufacturer"];
            return vendor == null ? "Unknown" : (string)vendor;
        }

        /// <summary>
        ///     Gets the registry key associated to this adapter.
        /// </summary>
        private string GetregistryKey() => $@"SYSTEM\ControlSet001\Control\Class\{{4D36E972-E325-11CE-BFC1-08002BE10318}}\{ExtractDeviceNumber():D4}";

        private string GetRegistryMac()
        {
            using (var regkey = Registry.LocalMachine.OpenSubKey(GetregistryKey(), false))
            {
                if (regkey == null)
                {
                    return null;
                }

                var address = regkey.GetValue("NetworkAddress");
                return address?.ToString();
            }
        }

        private string GetRegistryVendor()
        {
            if (RegistryMacAddress == null) return null;
            if (_manager == null) return null;
            var vendors = _manager.FindByMac(RegistryMacAddress);
            return string.Join(", ", vendors);
        }
        private bool UpdateRegistryMac(string registryKey, string newMac, string description, out bool shouldReenable)
        {
            // If the value is not the empty string, we want to set NetworkAddress to it, so it had
            // better be valid
            if (newMac.Length > 0 && !MacAddress.IsValidMac(newMac))
            {
                throw new MacChangerException(newMac + " is not a valid mac address");
            }

            using (var regkey = Registry.LocalMachine.OpenSubKey(registryKey, true))
            {
                if (regkey == null)
                {
                    throw new MacChangerException("Failed to open the registry key");
                }

                // Sanity check
                if (regkey.GetValue("AdapterModel") as string != description
                    && regkey.GetValue("DriverDesc") as string != description)
                {
                    throw new MacChangerException("Adapter not found in registry");
                }

                // Attempt to disable the adapter
                if (!TryDisable())
                {
                    throw new MacChangerException("Failed to disable network adapter.");
                }

                // If we're here the adapter has been disabled, so we set the flag that will
                // re-enable it in the finally block
                shouldReenable = true;

                // If we're here everything is OK; update or clear the registry value
                if (newMac.Length > 0)
                {
                    regkey.SetValue("NetworkAddress", newMac, RegistryValueKind.String);
                }
                else
                {
                    regkey.DeleteValue("NetworkAddress");
                }

                return true;
            }
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
                }

                _adapter = null;

                disposedValue = true;
            }
        }

        #endregion Dispose
    }
}