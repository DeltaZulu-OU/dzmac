using System;
using System.Globalization;
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
    /// The <see cref="Adapter"/> implementation is adapted from <see href="https://github.com/sietseringers/MACAddressTool"/>.
    /// </remarks>
    public class Adapter : IDisposable
    {
        private static readonly Regex _regex = new Regex("^[0-9A-F]*$");

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

        public Adapter(ManagementObject adapter, NetworkInterface managedAdapter)
        {
            ManagedAdapter = managedAdapter;
            _adapter = adapter;
            Enabled = GetEnabled();
        }

        /// <summary>
        /// Get a random (locally administered) MAC address.
        /// </summary>
        /// <returns>A MAC address having 01 as the least significant bits of the first byte, but otherwise random.</returns>
        public static string GetNewMac()
        {
            var r = new Random();

            var bytes = new byte[6];
            r.NextBytes(bytes);

            // Set second bit to 1
            bytes[0] = (byte)(bytes[0] | 0x02);
            // Set first bit to 0
            bytes[0] = (byte)(bytes[0] & 0xfe);

            return MacToString(bytes);
        }

        /// <summary>
        /// Get a MAC address for the provided OUI.
        /// </summary>
        /// <returns>A MAC address with the specified OUI.</returns>
        public static string GetNewMac(string oui)
        {
            var ouiOctet = ConvertHexStringToByteArray(oui);

            var r = new Random();
            var nicSpecificOctet = new byte[3];
            r.NextBytes(nicSpecificOctet);

            var newMac = MergeByteArrays(ouiOctet, nicSpecificOctet);

            return MacToString(newMac);
        }

        /// <summary>
        /// Verifies that a given string is a valid MAC address.
        /// </summary>
        /// <param name="mac">The string.</param>
        /// <returns>true if the string is a valid MAC address, false otherwise.</returns>
        /// <exception cref="RegexMatchTimeoutException"></exception>
        public static bool IsValidMac(string mac)
        {
            // 6 bytes == 12 hex characters (without dashes/dots/anything else)
            if (mac.Length != 12)
                return false;

            // Should be uppercase
            if (mac != mac.ToUpper())
                return false;

            // Should not contain anything other than hexadecimal digits
            if (!_regex.IsMatch(mac))
                return false;

            return true;
        }

        /// <summary>
        /// Verifies that a given MAC address is valid.
        /// </summary>
        /// <param name="macAsBytes">The address.</param>
        /// <returns>true if valid, false otherwise.</returns>
        /// <exception cref="RegexMatchTimeoutException"></exception>
        public static bool IsValidMac(byte[] macAsBytes) => IsValidMac(Adapter.MacToString(macAsBytes));

        /// <summary>
        /// Converts a byte array of length 6 to a MAC address (i.e. string of hexadecimal digits).
        /// </summary>
        /// <param name="macAsBytes">The bytes to convert.</param>
        /// <returns>The MAC address.</returns>
        public static string MacToString(byte[] macAsBytes) => BitConverter.ToString(macAsBytes).Replace("-", "").ToUpper();

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
                // If the value is not the empty string, we want to set NetworkAddress to it,
                // so it had better be valid
                if (value.Length > 0 && !IsValidMac(value))
                    throw new MacChangerException(value + " is not a valid mac address");

                using (var regkey = Registry.LocalMachine.OpenSubKey(RegistryKey, true))
                {
                    if (regkey == null)
                        throw new MacChangerException("Failed to open the registry key");

                    // Sanity check
                    if (regkey.GetValue("AdapterModel") as string != ManagedAdapter.Description
                        && regkey.GetValue("DriverDesc") as string != ManagedAdapter.Description)
                    {
                        throw new MacChangerException("Adapter not found in registry");
                    }

                    // Attempt to disable the adapter
                    var result = (uint)_adapter.InvokeMethod("Disable", null);
                    if (result != 0)
                        throw new MacChangerException("Failed to disable network adapter.");

                    // If we're here the adapter has been disabled, so we set the flag that will re-enable it in the finally block
                    shouldReenable = true;

                    // If we're here everything is OK; update or clear the registry value
                    if (value.Length > 0)
                        regkey.SetValue("NetworkAddress", value, RegistryValueKind.String);
                    else
                        regkey.DeleteValue("NetworkAddress");

                    return true;
                }
            }
            catch (Exception ex)
            {
                throw new MacChangerException(ex.ToString());
                //return false;
            }
            finally
            {
                if (shouldReenable)
                {
                    var result = (uint)_adapter.InvokeMethod("Enable", null);
                    if (result != 0)
                        throw new MacChangerException("Failed to re-enable network adapter.");
                }
            }
        }

        public override string ToString() => $"{ManagedAdapter.Description} ({ManagedAdapter.Name})";

        /// <summary>
        ///     Gets a string comprised of hexadecimal values, and converts it into a byte array
        /// </summary>
        /// <param name="hexString">String consists of hexadecimal characters</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="FormatException"></exception>
        /// <exception cref="OverflowException"></exception>
        private static byte[] ConvertHexStringToByteArray(string hexString)
        {
            if (hexString.Length % 2 != 0)
            {
                throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "The binary key cannot have an odd number of digits: {0}", hexString));
            }

            var data = new byte[hexString.Length / 2];
            for (var index = 0; index < data.Length; index++)
            {
                var byteValue = hexString.Substring(index * 2, 2);
                data[index] = byte.Parse(byteValue, NumberStyles.HexNumber, CultureInfo.InvariantCulture);
            }

            return data;
        }

        /// <summary>
        ///     Fast byte array merging for when OUI is specified
        /// </summary>
        /// <param name="firstArray">The first byte array used for OUI</param>
        /// <param name="secondArray">Second byte array randomly generated</param>
        /// <returns></returns>
        private static byte[] MergeByteArrays(byte[] firstArray, byte[] secondArray)
        {
            var combinedArray = new byte[firstArray.Length + secondArray.Length];
            Buffer.BlockCopy(firstArray, 0, combinedArray, 0, firstArray.Length);
            Buffer.BlockCopy(secondArray, 0, combinedArray, firstArray.Length, secondArray.Length);
            return combinedArray;
        }

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
