#nullable enable
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
        ///     Collect all network adapters
        /// </summary>
        /// <param name="vendorManager">Proide this parameter if there is a need to query vendor name from registry MAC value</param>
        /// <returns>Instances of <see cref="NetworkAdapter"/>.</returns>
        /// <exception cref="NetworkInformationException"></exception>
        public static IEnumerable<NetworkAdapter> GetNetworkAdapters(VendorManager? vendorManager = null)
        {
            var managementObjects = new ManagementObjectSearcher("SELECT * FROM Win32_NetworkAdapter").Get().Cast<ManagementObject>();
            var networkInterfaces = NetworkInterface.GetAllNetworkInterfaces()
                                                     .Where(a => MacAddress.IsValidMac(a.GetPhysicalAddress().GetAddressBytes()))
                                                     .OrderByDescending(a => a.Name);

            foreach (var networkInterface in networkInterfaces)
            {
                var adapterObject = managementObjects.FirstOrDefault(obj => obj.GetPropertyValue("Name").Equals(networkInterface.Description));

                yield return new NetworkAdapter(adapterObject, networkInterface, vendorManager);
            }
        }
    }
}
