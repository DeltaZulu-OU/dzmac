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
            Diagnostics.Info("adapter_discovery_started", ("vendorManagerProvided", vendorManager != null));
            var networkInterfaces = GetAll();
            Diagnostics.Debug("adapter_discovery_raw_count", ("totalDiscovered", networkInterfaces.Length));

            var filtered = networkInterfaces.Where(a => MacAddress.IsValidMac(a.GetPhysicalAddress().GetAddressBytes()))
                                            .OrderByDescending(a => a.Name)
                                            .ToList();

            if (!filtered.Any())
            {
                Diagnostics.Warning("adapter_discovery_completed", "No adapters with valid MAC addresses were found.", ("totalDiscovered", networkInterfaces.Length), ("usableAdapters", 0));
                return Array.Empty<NetworkAdapter>();
            }

            Diagnostics.Info("adapter_discovery_completed", ("totalDiscovered", networkInterfaces.Length), ("usableAdapters", filtered.Count));
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
            catch (Exception ex)
            {
                Diagnostics.Error("adapter_discovery_failed", ex, "Failed to enumerate network adapters.");
            }
            return networkInterfaces;
        }
    }
}
