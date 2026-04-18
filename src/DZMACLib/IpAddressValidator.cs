#nullable enable

using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;

namespace DZMACLib
{
    public static class IpAddressValidator
    {
        public static bool TryValidateIpv4Address(string value, out string normalized)
        {
            normalized = string.Empty;
            if (!IPAddress.TryParse(value, out var parsed) || parsed.AddressFamily != AddressFamily.InterNetwork)
            {
                return false;
            }

            normalized = parsed.ToString();
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
