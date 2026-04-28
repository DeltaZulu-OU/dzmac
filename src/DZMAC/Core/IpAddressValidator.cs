using System;
using System.Linq;
using System.Net;

namespace Dzmac.Core
{
    internal static class IpAddressValidator
    {
        public static bool TryValidateIpv4Address(string value, out string normalized)
        {
            normalized = string.Empty;
            if (string.IsNullOrWhiteSpace(value))
            {
                return false;
            }

            var parts = value.Split('.');
            if (parts.Length != 4)
            {
                return false;
            }

            var normalizedParts = new string[4];
            for (var i = 0; i < parts.Length; i++)
            {
                var part = parts[i];
                if (part.Length == 0 || part.Length > 3)
                {
                    return false;
                }

                if (part.Length > 1 && part[0] == '0')
                {
                    return false;
                }

                if (!byte.TryParse(part, out var octet))
                {
                    return false;
                }

                normalizedParts[i] = octet.ToString();
            }

            normalized = string.Join(".", normalizedParts);
            return true;
        }

        public static bool TryValidateIpv4SubnetMask(string value, out string normalized)
        {
            normalized = string.Empty;
            if (!TryValidateIpv4Address(value, out var address))
            {
                return false;
            }

            var bytes = IPAddress.Parse(address).GetAddressBytes();
            var bitString = string.Concat(bytes.Select(b => Convert.ToString(b, 2).PadLeft(8, '0')));
            var firstZero = bitString.IndexOf('0');
            if (firstZero >= 0 && bitString.IndexOf('1', firstZero) >= 0)
            {
                return false;
            }

            normalized = address;
            return true;
        }
    }
}
