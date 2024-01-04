using System;
using System.Management;
using System.Net.NetworkInformation;
using System.Text.RegularExpressions;
using Microsoft.Win32;

namespace MacChanger
{
    /// <summary>
    /// Represents a Windows network interface. Wrapper around the .NET API for network
    /// interfaces, as well as for the unmanaged device.
    /// </summary>
    /// <remarks>
    /// The <see cref="NetworkAdapter"/> implementation is adapted from <see href="https://github.com/sietseringers/MACAddressTool"/>.
    /// </remarks>
    public class NetworkAdapter : IDisposable
    {
        private ManagementObject _adapter;

        private bool disposedValue;

        public bool Enabled { get; }

        /// <summary>
        /// Get the MAC address as reported by the adapter.
        /// </summary>
        public PhysicalAddress MacAddress => GetMacAddress();

        /// <summary>
        /// Get the .NET managed adapter.
        /// </summary>
        /// <exception cref="NetworkInformationException" accessor="get"></exception>
        public NetworkInterface ManagedAdapter { get; }

        /// <summary>
        /// Get the registry key associated to this adapter.
        /// </summary>
        public string RegistryKey => $@"SYSTEM\ControlSet001\Control\Class\{{4D36E972-E325-11CE-BFC1-08002BE10318}}\{ExtractDeviceNumber():D4}";

        /// <summary>
        /// Get the NetworkAddress registry value of this adapter.
        /// </summary>B
        public string RegistryMac => FindRegistryMac();

        public NetworkAdapter(ManagementObject adapterObject, NetworkInterface networkInterface)
        {
            ManagedAdapter = networkInterface;
            _adapter = adapterObject;
            Enabled = GetEnabled();
        }

        /// <summary>
        /// Disables the network adapter
        /// If the adapter is already disabled, it will return false.
        /// If the disable operation fails, it will return false.
        /// </summary>
        /// <returns>
        /// True if the disable operation succees.
        /// False if the adapter is already disabled or the operation is failed.
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
        /// Enables the network adapter
        /// If the adapter is already enabled, it will return false.
        /// If the enable operation fails, it will return false.
        /// </summary>
        /// <returns>
        /// True if the enable operation succees.
        /// False if the adapter is already enabled or the operation is failed.
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

        /// <summary>
        /// Sets the NetworkAddress registry value of this adapter.
        /// </summary>
        /// <param name="value">The value. Should be EITHER a string of 12 hexadecimal digits, uppercase, without dashes, dots or anything else, OR an empty string (clears the registry value).</param>
        /// <returns>true if successful, false otherwise</returns>
        /// <exception cref="MacChangerException"></exception>
        public bool SetRegistryMac(string value)
        {
            var shouldReenable = false;

            try
            {
                return UpdateRegistryMac(RegistryKey, value, ManagedAdapter.Description, out shouldReenable);
            }
            catch (Exception ex)
            {
                throw new MacChangerException(ex.ToString());
                //return false;
            }
            finally
            {
                if (shouldReenable && !TryEnable())
                    throw new MacChangerException("Failed to re-enable network adapter.");
            }
        }

        private bool UpdateRegistryMac(string registryKey, string newMac, string description, out bool shouldReenable)
        {
            // If the value is not the empty string, we want to set NetworkAddress to it,
            // so it had better be valid
            if (newMac.Length > 0 && !MacChanger.MacAddress.IsValidMac(newMac))
                throw new MacChangerException(newMac + " is not a valid mac address");

            using (var regkey = Registry.LocalMachine.OpenSubKey(registryKey, true))
            {
                if (regkey == null)
                    throw new MacChangerException("Failed to open the registry key");

                // Sanity check
                if (regkey.GetValue("AdapterModel") as string != description
                    && regkey.GetValue("DriverDesc") as string != description)
                {
                    throw new MacChangerException("Adapter not found in registry");
                }

                // Attempt to disable the adapter
                if (!TryDisable())
                    throw new MacChangerException("Failed to disable network adapter.");

                // If we're here the adapter has been disabled, so we set the flag that will re-enable it in the finally block
                shouldReenable = true;

                // If we're here everything is OK; update or clear the registry value
                if (newMac.Length > 0)
                    regkey.SetValue("NetworkAddress", newMac, RegistryValueKind.String);
                else
                    regkey.DeleteValue("NetworkAddress");

                return true;
            }
        }

        public override string ToString() => $"{ManagedAdapter.Description} ({ManagedAdapter.Name})";

        private int ExtractDeviceNumber()
        {
            // Extract adapter number; this should correspond to the keys under
            // HKEY_LOCAL_MACHINE\SYSTEM\ControlSet001\Control\Class\{4d36e972-e325-11ce-bfc1-08002be10318}
            var deviceNumber = -1;
            try
            {
                var match = Regex.Match(_adapter.Path.RelativePath, "\\\"(\\d+)\\\"$");
                deviceNumber = int.Parse(match.Groups[1].Value);
            }
            catch
            {
            }

            return deviceNumber;
        }

        private string FindRegistryMac()
        {
            try
            {
                using (var regkey = Registry.LocalMachine.OpenSubKey(RegistryKey, false))
                {
                    return regkey.GetValue("NetworkAddress").ToString();
                }
            }
            catch
            {
                return null;
            }
        }

        private bool GetEnabled()
        {
            var enabled = _adapter["NetEnabled"];
            return enabled != null && (bool)enabled;
        }

        private PhysicalAddress GetMacAddress()
        {
            try
            {
                return ManagedAdapter.GetPhysicalAddress();
            }
            catch { return null; }
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
        #endregion
    }
}
