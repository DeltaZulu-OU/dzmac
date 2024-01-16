#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
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
            var networkAdapterObjects = new ManagementObjectSearcher("SELECT * FROM Win32_NetworkAdapter").Get().Cast<ManagementObject>();
            var networkAdapterConfigurationObjects = new ManagementObjectSearcher("SELECT * FROM Win32_NetworkAdapterConfiguration").Get().Cast<ManagementObject>();

            var networkInterfaces = GetAll();

            var filtered = networkInterfaces.Where(a => MacAddress.IsValidMac(a.GetPhysicalAddress().GetAddressBytes()))
                                                     .OrderByDescending(a => a.Name);

            if (!filtered.Any())
            {
                foreach (var item in Array.Empty<NetworkAdapter>())
                {
                    yield return item;
                }
            }

            foreach (var networkInterface in filtered)
            {
                var adapterObject = networkAdapterObjects.FirstOrDefault(obj => obj.GetPropertyValue("Name").Equals(networkInterface.Description));
                var configObject = networkAdapterConfigurationObjects.FirstOrDefault(obj => obj.GetPropertyValue("SettingID").Equals(networkInterface.Id));
                yield return new NetworkAdapter(adapterObject, configObject, networkInterface, vendorManager);
            }
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
