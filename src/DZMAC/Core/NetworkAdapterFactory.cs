#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Net.NetworkInformation;

namespace Dzmac.Core
{
    /// <summary>
    ///     Adapter data collection class
    /// </summary>
    public static class NetworkAdapterFactory
    {
        private static readonly string[] PhysicalBusPrefixes = { "PCI\\", "USB\\", "ACPI\\" };

        /// <summary>
        ///     Collect all network adapters. Returns empty list if it cannot gather data.
        /// </summary>
        /// <param name="vendorManager">Proide this parameter if there is a need to query vendor name from registry MAC value</param>
        /// <returns>Instances of <see cref="NetworkAdapter"/>.</returns>
        public static IEnumerable<NetworkAdapter> GetNetworkAdapters(VendorList? vendorManager = null)
        {
            Diagnostics.Info("adapter_discovery_started", ("vendorManagerProvided", vendorManager != null));
            var networkInterfaces = GetAll();
            var hardwareIdsByConfigId = GetPnpDeviceIdsByConfigId();
            Diagnostics.Debug("adapter_discovery_raw_count", ("totalDiscovered", networkInterfaces.Length));
            var allAdapters = networkInterfaces
                .Select(networkInterface => new
                {
                    Interface = networkInterface,
                    HasValidMac = MacAddress.IsValidMac(networkInterface.GetPhysicalAddress().GetAddressBytes()),
                    HardwareId = ResolveHardwareId(networkInterface.Id, hardwareIdsByConfigId)
                })
                .ToList();

            var filtered = allAdapters
                .Where(adapter => adapter.HasValidMac || IsLikelyPhysicalAdapter(adapter.HardwareId))
                .Select(adapter => adapter.Interface)
                .OrderByDescending(a => a.Name)
                .ToList();

            var ignored = allAdapters
                .Where(adapter => !adapter.HasValidMac && !IsLikelyPhysicalAdapter(adapter.HardwareId))
                .Select(adapter => adapter.Interface)
                .OrderBy(a => a.Name, StringComparer.OrdinalIgnoreCase)
                .ToList();

            Diagnostics.Info(
                "adapter_discovery_inventory",
                ("allAdapters", string.Join(", ", allAdapters.Select(a => $"{a.Interface.Name}[{a.Interface.Id}]"))),
                ("ignoredAdapters", string.Join(", ", ignored.Select(a => $"{a.Name}[{a.Id}]"))),
                ("ignoredCount", ignored.Count));

            if (!filtered.Any())
            {
                Diagnostics.Warning("adapter_discovery_completed", "No compatible adapters were found.", ("totalDiscovered", networkInterfaces.Length), ("usableAdapters", 0));
                return Array.Empty<NetworkAdapter>();
            }

            Diagnostics.Info("adapter_discovery_completed", ("totalDiscovered", networkInterfaces.Length), ("usableAdapters", filtered.Count));
            return filtered.Select(networkInterface =>
                new NetworkAdapter(
                    networkInterface,
                    vendorManager,
                    IsLikelyPhysicalAdapter(ResolveHardwareId(networkInterface.Id, hardwareIdsByConfigId))));
        }

        /// <summary>
        /// Returns true when PNP device id suggests that adapter is backed by a physical bus.
        /// </summary>
        public static bool IsLikelyPhysicalAdapter(string? pnpDeviceId)
        {
            if (string.IsNullOrWhiteSpace(pnpDeviceId))
            {
                return false;
            }

            return PhysicalBusPrefixes.Any(prefix => pnpDeviceId.StartsWith(prefix, StringComparison.OrdinalIgnoreCase));
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

        private static Dictionary<string, string> GetPnpDeviceIdsByConfigId()
        {
            var map = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

            try
            {
                using var searcher = new ManagementObjectSearcher("SELECT GUID,PNPDeviceID FROM Win32_NetworkAdapter WHERE GUID IS NOT NULL AND PNPDeviceID IS NOT NULL");
                foreach (var adapter in searcher.Get().Cast<ManagementObject>())
                {
                    var guid = adapter["GUID"] as string;
                    var pnpDeviceId = adapter["PNPDeviceID"] as string;

                    if (!string.IsNullOrWhiteSpace(guid) && !string.IsNullOrWhiteSpace(pnpDeviceId))
                    {
                        map[guid] = pnpDeviceId;
                    }
                }
            }
            catch (Exception ex)
            {
                Diagnostics.Warning("adapter_discovery_pnp_map_failed", ex.Message);
            }

            return map;
        }

        private static string? ResolveHardwareId(string configId, IReadOnlyDictionary<string, string> hardwareIdsByConfigId)
        {
            if (hardwareIdsByConfigId.TryGetValue(configId, out var hardwareId))
            {
                return hardwareId;
            }

            return null;
        }
    }
}