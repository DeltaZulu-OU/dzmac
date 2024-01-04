using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace MacChanger
{
    public static class MacAddress
    {
        // 6 bytes == 12 hex characters (without dashes/dots/anything else)
        // Should be uppercase
        // Should not contain anything other than hexadecimal digits
        private static readonly Regex _macAddressPattern = new Regex("^[0-9A-F]{12}$", RegexOptions.Compiled);

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
        public static bool IsValidMac(string mac) => _macAddressPattern.IsMatch(mac);

        /// <summary>
        /// Verifies that a given MAC address is valid.
        /// </summary>
        /// <param name="macAsBytes">The address.</param>
        /// <returns>true if valid, false otherwise.</returns>
        /// <exception cref="RegexMatchTimeoutException"></exception>
        public static bool IsValidMac(byte[] macAsBytes) => IsValidMac(MacAddress.MacToString(macAsBytes));

        /// <summary>
        /// Converts a byte array of length 6 to a MAC address (i.e. string of hexadecimal digits).
        /// </summary>
        /// <param name="macAsBytes">The bytes to convert.</param>
        /// <returns>The MAC address.</returns>
        public static string MacToString(byte[] macAsBytes) => BitConverter.ToString(macAsBytes).Replace("-", "").ToUpper();

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
    }
}