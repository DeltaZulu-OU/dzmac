using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Net.NetworkInformation;

namespace Dzmac.Core
{
    internal enum AdapterClassificationSource
    {
        Unknown = 0,
        MsftNetAdapter = 1,
        Win32PhysicalAdapter = 2,
        PnpPrefixHeuristic = 3
    }

    internal readonly struct AdapterClassification
    {
        public bool IsPhysical { get; }
        public AdapterClassificationSource Source { get; }

        public AdapterClassification(bool isPhysical, AdapterClassificationSource source)
        {
            IsPhysical = isPhysical;
            Source = source;
        }
    }

    internal static class AdapterClassificationResolver
    {
        public static IReadOnlyDictionary<string, AdapterClassification> BuildByConfigIdMap(IReadOnlyDictionary<string, string> pnpByConfigId)
        {
            var map = new Dictionary<string, AdapterClassification>(StringComparer.OrdinalIgnoreCase);
            BuildFromMsftNetAdapter(map);
            BuildFromWin32PhysicalAdapter(map);
            BuildFromPnpPrefixes(map, pnpByConfigId);
            return map;
        }

        private static void BuildFromMsftNetAdapter(IDictionary<string, AdapterClassification> map)
        {
            try
            {
                var scope = new ManagementScope(@"\\.\root\StandardCimv2");
                scope.Connect();
                using var searcher = new ManagementObjectSearcher(
                    scope,
                    new ObjectQuery("SELECT InterfaceGuid, HardwareInterface FROM MSFT_NetAdapter WHERE InterfaceGuid IS NOT NULL"));
                foreach (var adapter in searcher.Get().Cast<ManagementObject>())
                {
                    var guid = adapter["InterfaceGuid"] as string;
                    if (string.IsNullOrWhiteSpace(guid) || map.ContainsKey(guid))
                    {
                        continue;
                    }

                    var hardwareInterface = adapter["HardwareInterface"];
                    if (hardwareInterface is bool isPhysical)
                    {
                        map[guid] = new AdapterClassification(isPhysical, AdapterClassificationSource.MsftNetAdapter);
                    }
                }
            }
            catch (ManagementException ex)
            {
                Diagnostics.Warning("adapter_discovery_msft_provider_unavailable", ex.Message);
            }
            catch (UnauthorizedAccessException ex)
            {
                Diagnostics.Warning("adapter_discovery_msft_provider_access_denied", ex.Message);
            }
        }

        private static void BuildFromWin32PhysicalAdapter(IDictionary<string, AdapterClassification> map)
        {
            try
            {
                using var searcher = new ManagementObjectSearcher(
                    "SELECT GUID, PhysicalAdapter FROM Win32_NetworkAdapter WHERE GUID IS NOT NULL AND PhysicalAdapter IS NOT NULL");
                foreach (var adapter in searcher.Get().Cast<ManagementObject>())
                {
                    var guid = adapter["GUID"] as string;
                    if (string.IsNullOrWhiteSpace(guid) || map.ContainsKey(guid))
                    {
                        continue;
                    }

                    var physicalAdapter = adapter["PhysicalAdapter"];
                    if (physicalAdapter is bool isPhysical)
                    {
                        map[guid] = new AdapterClassification(isPhysical, AdapterClassificationSource.Win32PhysicalAdapter);
                    }
                }
            }
            catch (ManagementException ex)
            {
                Diagnostics.Warning("adapter_discovery_win32_physical_flag_failed", ex.Message);
            }
        }

        private static void BuildFromPnpPrefixes(IDictionary<string, AdapterClassification> map, IReadOnlyDictionary<string, string> pnpByConfigId)
        {
            foreach (var pair in pnpByConfigId)
            {
                if (map.ContainsKey(pair.Key))
                {
                    continue;
                }

                map[pair.Key] = new AdapterClassification(
                    NetworkAdapterFactory.IsLikelyPhysicalAdapter(pair.Value),
                    AdapterClassificationSource.PnpPrefixHeuristic);
            }
        }
    }

    /// <summary>
    ///     Adapter data collection class
    /// </summary>
    internal static class NetworkAdapterFactory
    {
        private static readonly string[] PhysicalBusPrefixes = { "PCI\\", "USB\\", "ACPI\\" };

        /// <summary>
        ///     Collect all network adapters. Returns empty list if it cannot gather data.
        /// </summary>
        /// <param name="vendorManager">Proide this parameter if there is a need to query vendor name from registry MAC value</param>
        /// <returns>Instances of <see cref="NetworkAdapter"/>.</returns>
        public static IEnumerable<NetworkAdapter> GetNetworkAdapters(VendorList? vendorManager = null, bool physicalOnly = false)
        {
            Diagnostics.Info("adapter_discovery_started", ("vendorManagerProvided", vendorManager != null));
            var networkInterfaces = GetAll();
            var hardwareIdsByConfigId = GetPnpDeviceIdsByConfigId();
            var classificationByConfigId = AdapterClassificationResolver.BuildByConfigIdMap(hardwareIdsByConfigId);
            Diagnostics.Debug("adapter_discovery_raw_count", ("totalDiscovered", networkInterfaces.Length));
            var allAdapters = networkInterfaces
                .Select(networkInterface => new
                {
                    Interface = networkInterface,
                    HasValidMac = MacAddress.IsValidMac(networkInterface.GetPhysicalAddress().GetAddressBytes()),
                    HardwareId = ResolveHardwareId(networkInterface.Id, hardwareIdsByConfigId),
                    Classification = ResolveClassification(networkInterface.Id, classificationByConfigId, hardwareIdsByConfigId)
                })
                .ToList();

            var filtered = allAdapters
                .Where(adapter => adapter.HasValidMac || adapter.Classification.IsPhysical)
                .Select(adapter => adapter.Interface)
                .OrderByDescending(a => a.Name)
                .ToList();

            var ignored = allAdapters
                .Where(adapter => !adapter.HasValidMac && !adapter.Classification.IsPhysical)
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
            var result = filtered.Select(networkInterface =>
                new NetworkAdapter(
                    networkInterface,
                    vendorManager,
                    ResolveClassification(networkInterface.Id, classificationByConfigId, hardwareIdsByConfigId).IsPhysical));
            return physicalOnly ? result.Where(a => a.IsPhysicalAdapter) : result;
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

        private static AdapterClassification ResolveClassification(
            string configId,
            IReadOnlyDictionary<string, AdapterClassification> classificationByConfigId,
            IReadOnlyDictionary<string, string> hardwareIdsByConfigId)
        {
            if (classificationByConfigId.TryGetValue(configId, out var classification))
            {
                return classification;
            }

            return new AdapterClassification(
                IsLikelyPhysicalAdapter(ResolveHardwareId(configId, hardwareIdsByConfigId)),
                AdapterClassificationSource.PnpPrefixHeuristic);
        }
    }
}
