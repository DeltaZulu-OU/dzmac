using System;
using System.Diagnostics;
using System.Globalization;
using System.Net.NetworkInformation;
using System.Text.RegularExpressions;

namespace Dzmac.Core
{
    /// <summary>
    ///     A class to wrap MAC address with formatting and validation logic.
    /// </summary>
    [DebuggerDisplay("{" + nameof(GetDebuggerDisplay) + "(),nq}")]
    public partial class MacAddress
    {
        /// <summary>
        ///     Use 02 as the first octet to mark the MAC address as locally administered.
        ///     <see href="https://github.com/zbalkan/DZMAC/wiki/Help#why-does-setting-the-first-octet-to-02-help-with-some-wi-fi-mac-changes"/>
        /// </summary>
        private const char locallyAdministeredOctet = '2';

        /// <summary>
        ///     6 bytes == 12 hex characters (without dashes/dots/anything else)
        ///     Should be uppercase
        ///     Should not contain anything other than hexadecimal digits
        /// </summary>
        private static readonly Regex _macAddressPattern = new Regex("^[0-9A-F]{12}$", RegexOptions.Compiled);

        /// <summary>
        ///     Internally we keep the address with no puncuation marks.
        /// </summary>
        private string _macAddress;

        /// <summary>
        ///     Create an instance of <see cref="MacAddress"/> using a string.
        /// </summary>
        /// <param name="macAddress">Mac address with no punctuation marks.</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public MacAddress(string macAddress)
        {
            if (macAddress == null)
            {
                throw new ArgumentNullException(nameof(macAddress));
            }

            macAddress = macAddress.Replace("-", string.Empty).Replace(":", string.Empty);

            if (!IsValidMac(macAddress))
            {
                throw new ArgumentException(nameof(macAddress));
            }

            _macAddress = macAddress;
        }

        /// <summary>
        ///     Create an instance of <see cref="MacAddress"/> using an instance of
        ///     <see cref="PhysicalAddress"/>.
        /// </summary>
        /// <param name="macAddress">Mac address with no punctuation marks.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public MacAddress(PhysicalAddress macAddress)
        {
            if (macAddress == null)
            {
                throw new ArgumentNullException(nameof(macAddress));
            }

            _macAddress = macAddress.ToString().ToUpperInvariant().Replace("-", string.Empty);
        }

        /// <summary>
        ///     Get a random (locally administered) MAC address.
        /// </summary>
        /// <returns>A MAC address having 01 as the least significant bits of the first byte, but otherwise random.</returns>
        public static MacAddress GetNewMac()
        {
            var r = new Random();

            var bytes = new byte[6];
            r.NextBytes(bytes);

            // Set second bit to 1
            bytes[0] = (byte)(bytes[0] | 0x02);
            // Set first bit to 0
            bytes[0] = (byte)(bytes[0] & 0xfe);

            return new MacAddress(MacToString(bytes));
        }

        /// <summary>
        ///     Get a MAC address for the provided OUI.
        /// </summary>
        /// <param name="oui">OUI of the vendor</param>
        /// <returns>A MAC address with the specified OUI.</returns>
        public static MacAddress GetNewMac(string oui)
        {
            var ouiOctet = ConvertHexStringToByteArray(oui);

            var r = new Random();
            var nicSpecificOctet = new byte[3];
            r.NextBytes(nicSpecificOctet);

            var newMac = MergeByteArrays(ouiOctet, nicSpecificOctet);
            return new MacAddress(MacToString(newMac));
        }

        /// <summary>
        ///     Verifies that a given string is a valid MAC address.
        /// </summary>
        /// <param name="mac">MAC address as string.</param>
        /// <returns>true if the string is a valid MAC address, false otherwise.</returns>
        public static bool IsValidMac(string mac)
        {
            try
            {
                return _macAddressPattern.IsMatch(mac);
            }
            catch (RegexMatchTimeoutException ex)
            {
                Debug.WriteLine($"Regex pattern timed out. Likely the text is too long for a MAC address. {ex}");
                return false;
            }
        }

        /// <summary>
        ///     Verifies that a given MAC address is valid.
        /// </summary>
        /// <param name="macAsBytes">The address.</param>
        /// <returns>true if valid, false otherwise.</returns>
        public static bool IsValidMac(byte[] macAsBytes) => IsValidMac(MacToString(macAsBytes));

        /// <summary>
        ///     Converts a byte array of length 6 to a MAC address (i.e. string of hexadecimal digits).
        /// </summary>
        /// <param name="macAsBytes">MAC address in the format of <see cref="Array"/> of <see cref="byte"/>s</param>
        /// <returns>The MAC address as <see cref="string"/> without delimiters.</returns>
        public static string MacToString(byte[] macAsBytes) => BitConverter.ToString(macAsBytes).Replace("-", string.Empty).ToUpper();

        public static bool operator !=(MacAddress obj1, MacAddress obj2) => !(obj1 == obj2);

        public static bool operator ==(MacAddress obj1, MacAddress obj2)
        {
            if (ReferenceEquals(obj1, obj2))
            {
                return true;
            }

            if (obj1 is null)
            {
                return false;
            }

            if (obj2 is null)
            {
                return false;
            }

            return obj1.Equals(obj2);
        }

        public override bool Equals(object obj) => obj is MacAddress macAddress && Equals(macAddress);

        public override int GetHashCode() => _macAddress.GetHashCode();

        /// <summary>
        ///     Mark the MAC address as locally administered.
        ///     <see href="https://github.com/zbalkan/DZMAC/wiki/Help#why-does-setting-the-first-octet-to-02-help-with-some-wi-fi-mac-changes"/>
        /// </summary>
        public void SetAsLocallyAdministered()
        {
            var charArray = _macAddress.ToCharArray();
            charArray[1] = locallyAdministeredOctet;
            _macAddress = new string(charArray);
        }

        /// <inheritdoc/>
        public override string ToString() => ToString(MacDelimiter.None);

        /// <summary>
        ///     Format the MAC as dash or colon delimited octets
        /// </summary>
        public string ToString(MacDelimiter delimiter) => delimiter switch
        {
            MacDelimiter.Dash  => FormatMac('-'),
            MacDelimiter.Colon => FormatMac(':'),
            _                  => _macAddress
        };

        private string FormatMac(char sep) =>
            $"{_macAddress.Substring(0, 2)}{sep}{_macAddress.Substring(2, 2)}{sep}{_macAddress.Substring(4, 2)}{sep}{_macAddress.Substring(6, 2)}{sep}{_macAddress.Substring(8, 2)}{sep}{_macAddress.Substring(10, 2)}";

        /// <summary>
        ///     Gets a string comprised of hexadecimal values, and converts it into a byte array
        /// </summary>
        /// <param name="hexString">String consists of hexadecimal characters</param>
        /// <returns>MAC address as byte array</returns>
        /// <exception cref="ArgumentException"></exception>
        private static byte[] ConvertHexStringToByteArray(string hexString)
        {
            if (hexString.Length % 2 != 0)
            {
                throw new ArgumentException($"The binary key cannot have an odd number of digits: {hexString}");
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
        /// <returns>A generated MAC address as byte array</returns>
        private static byte[] MergeByteArrays(byte[] firstArray, byte[] secondArray)
        {
            var combinedArray = new byte[firstArray.Length + secondArray.Length];
            Buffer.BlockCopy(firstArray, 0, combinedArray, 0, firstArray.Length);
            Buffer.BlockCopy(secondArray, 0, combinedArray, firstArray.Length, secondArray.Length);
            return combinedArray;
        }

        private bool Equals(MacAddress other)
        {
            if (other is null)
                return false;
            if (ReferenceEquals(this, other))
                return true;
            return _macAddress.Equals(other._macAddress);
        }

        private string GetDebuggerDisplay() => ToString();
    }
}