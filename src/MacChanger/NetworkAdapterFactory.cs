#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;

namespace MacChanger
{
    /// <summary>
    ///     Adapter data collection class
    /// </summary>
    public static class NetworkAdapterFactory
    {
        /// <summary>
        ///     Collect all network adapters. Returns empty list if it cannot gather data.
        /// </summary>
        /// <param name="vendorManager">Proide this parameter if there is a need to query vendor name from registry MAC value</param>
        /// <returns>Instances of <see cref="NetworkAdapter"/>.</returns>
        public static IEnumerable<NetworkAdapter> GetNetworkAdapters(VendorManager? vendorManager = null)
        {
            var networkInterfaces = GetAll();

            var filtered = networkInterfaces.Where(a => MacAddress.IsValidMac(a.GetPhysicalAddress().GetAddressBytes()))
                                            .OrderByDescending(a => a.Name)
                                            .ToList();

            if (!filtered.Any())
            {
                return Array.Empty<NetworkAdapter>();
            }

            return filtered.Select(networkInterface => new NetworkAdapter(networkInterface, vendorManager));
        }

        private static NetworkInterface[] GetAll()
        {
            var networkInterfaces = Array.Empty<NetworkInterface>();

            try
            {
                var interfaces = NetworkInterface.GetAllNetworkInterfaces();
                if (interfaces != null)
                {
                    networkInterfaces = interfaces;
                }
            }
            catch (Exception)
            {
                // ignore
            }
            return networkInterfaces;
        }
    }
}
