using System.Collections.Generic;
using System.Linq;
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
        public static IEnumerable<Adapter> GetAdapters() => from adapter in NetworkInterface.GetAllNetworkInterfaces()
                                                     .Where(a => Adapter.IsValidMac(a.GetPhysicalAddress().GetAddressBytes()))
                                                     .OrderByDescending(a => a.Speed)
                                                     select new Adapter(adapter);
    }
}
