using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Net.NetworkInformation;

namespace MacChanger
{
    /// <summary>
    ///     Adapter data collection class
    /// </summary>
    public static class AdapterFactory
    {
        /// <summary>
        ///     Collect all network adapters
        /// </summary>
        /// <returns></returns>
        /// /// <exception cref="NetworkInformationException"></exception>
        // ExceptionAdjustment: M:System.Net.NetworkInformation.NetworkInterface.GetAllNetworkInterfaces -T:System.Net.NetworkInformation.NetworkInformationException
        public static IEnumerable<Adapter> GetAdapters()
        {
            var managementObjects = new ManagementObjectSearcher("SELECT * FROM Win32_NetworkAdapter").Get().Cast<ManagementObject>();
            var networkInterfaces = NetworkInterface.GetAllNetworkInterfaces()
                                                     .Where(a => Adapter.IsValidMac(a.GetPhysicalAddress().GetAddressBytes()))
                                                     .OrderByDescending(a => a.Description);

            foreach (var networkInterface in networkInterfaces)
            {
                var adapter = managementObjects.FirstOrDefault(obj => obj.GetPropertyValue("Name").Equals(networkInterface.Description));

                yield return new Adapter(adapter, networkInterface);
            }
        }
    }
}
