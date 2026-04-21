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
            var kindsByConfigId = GetAdapterKindsByConfigId();
            Diagnostics.Debug("adapter_discovery_raw_count", ("totalDiscovered", networkInterfaces.Length));

            var allAdapters = networkInterfaces
                .Select(networkInterface => new
                {
                    Interface = networkInterface,
                    HasValidMac = MacAddress.IsValidMac(networkInterface.GetPhysicalAddress().GetAddressBytes()),
                    Kind = ResolveKind(networkInterface.Id, kindsByConfigId)
                })
                .ToList();

            var filtered = allAdapters
                .Where(adapter =>
                    adapter.HasValidMac
                    || adapter.Kind == AdapterKind.Physical
                    || adapter.Kind == AdapterKind.VirtualOrLogical)
                .Select(adapter => adapter.Interface)
                .OrderByDescending(a => a.Name)
                .ToList();

            var ignored = allAdapters
                .Where(adapter => !adapter.HasValidMac && adapter.Kind == AdapterKind.Unknown)
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
            {
                var kind = ResolveKind(networkInterface.Id, kindsByConfigId);
                var hasValidMac = MacAddress.IsValidMac(networkInterface.GetPhysicalAddress().GetAddressBytes());
                var isPhysical = ShouldTreatAsPhysical(kind, hasValidMac);

                return new NetworkAdapter(
                    networkInterface,
                    vendorManager,
                    isPhysical);
            });

            return physicalOnly ? result.Where(a => a.IsPhysicalAdapter) : result;
        }

        internal static bool ShouldTreatAsPhysical(AdapterKind kind, bool hasValidMac)
        {
            return kind == AdapterKind.Physical
                   || (kind == AdapterKind.Unknown && hasValidMac);
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

        private static AdapterKind ResolveKind(string configId, IReadOnlyDictionary<string, AdapterKind> kindsByConfigId)
        {
            return kindsByConfigId.TryGetValue(configId, out var kind)
                ? kind
                : AdapterKind.Unknown;
        }

        private static Dictionary<string, AdapterKind> GetAdapterKindsByConfigId()
        {
            // Classification precedence is intentional:
            // 1) MSFT_NetAdapter (modern StandardCimv2 model)
            // 2) Win32_NetworkAdapter.PhysicalAdapter
            // 3) PNPDeviceID prefix as a last-resort heuristic
            //
            // References:
            // - Win32_NetworkAdapter is deprecated in favor of MSFT_NetAdapter:
            //   https://learn.microsoft.com/windows/win32/cimwin32prov/win32-networkadapter
            // - MSFT_NetAdapter class (Virtual/IMFilter/EndPointInterface/HardwareInterface):
            //   https://learn.microsoft.com/windows/win32/fwp/wmi/netadaptercimprov/msft-netadapter
            // - Win32_NetworkAdapter.PhysicalAdapter and PNPDeviceID semantics:
            //   https://learn.microsoft.com/windows/win32/cimwin32prov/win32-networkadapter
            var kinds = GetMsftNetAdapterKinds();

            foreach (var row in GetWin32RowsByGuid())
            {
                if (kinds.TryGetValue(row.Key, out var existing) && existing != AdapterKind.Unknown)
                {
                    continue;
                }

                if (row.Value.PhysicalAdapter.HasValue)
                {
                    kinds[row.Key] = ResolveWin32Kind(row.Value.PhysicalAdapter, row.Value.PnpDeviceId);
                    continue;
                }

                if (!string.IsNullOrWhiteSpace(row.Value.PnpDeviceId))
                {
                    kinds[row.Key] = ResolveWin32Kind(row.Value.PhysicalAdapter, row.Value.PnpDeviceId);
                }
            }

            return kinds;
        }

        private static Dictionary<string, AdapterKind> GetMsftNetAdapterKinds()
        {
            var map = new Dictionary<string, AdapterKind>(StringComparer.OrdinalIgnoreCase);

            try
            {
                // Use SELECT * so the returned ManagementObject is a full WMI object.
                // This keeps the object compatible with method invocations where needed.
                using var searcher = new ManagementObjectSearcher(
                    @"root\StandardCimv2",
                    "SELECT * FROM MSFT_NetAdapter");

                foreach (var adapter in searcher.Get().Cast<ManagementObject>())
                {
                    var guid = NormalizeGuid(adapter["InterfaceGuid"] as string);
                    if (string.IsNullOrWhiteSpace(guid))
                    {
                        continue;
                    }

                    var hardwareInterface = adapter["HardwareInterface"] as bool?;
                    var virtualInterface = adapter["Virtual"] as bool?;
                    var imFilter = adapter["IMFilter"] as bool?;
                    var endPointInterface = adapter["EndPointInterface"] as bool?;

                    map[guid] = ResolveMsftKind(hardwareInterface, virtualInterface, imFilter, endPointInterface);
                }
            }
            catch (Exception ex)
            {
                Diagnostics.Warning("adapter_discovery_msft_map_failed", ex.Message);
            }

            return map;
        }

        internal static AdapterKind ResolveMsftKind(bool? hardwareInterface, bool? virtualInterface, bool? imFilter, bool? endPointInterface)
        {
            if (endPointInterface == true || imFilter == true || virtualInterface == true)
            {
                return AdapterKind.VirtualOrLogical;
            }

            if (hardwareInterface == true)
            {
                return AdapterKind.Physical;
            }

            return AdapterKind.Unknown;
        }

        internal static AdapterKind ResolveWin32Kind(bool? physicalAdapter, string? pnpDeviceId)
        {
            if (physicalAdapter.HasValue)
            {
                return physicalAdapter.Value
                    ? AdapterKind.Physical
                    : AdapterKind.VirtualOrLogical;
            }

            // PNPDeviceID is an identifier of the logical device, not a definitive
            // physical/virtual truth source. Prefix matching is fallback only.
            return IsLikelyPhysicalAdapter(pnpDeviceId)
                ? AdapterKind.Physical
                : AdapterKind.VirtualOrLogical;
        }

        private static Dictionary<string, (bool? PhysicalAdapter, string? PnpDeviceId)> GetWin32RowsByGuid()
        {
            var map = new Dictionary<string, (bool? PhysicalAdapter, string? PnpDeviceId)>(StringComparer.OrdinalIgnoreCase);

            try
            {
                // Use SELECT * for full WMI object compatibility (method invocation scenarios).
                using var searcher = new ManagementObjectSearcher(
                    "SELECT * FROM Win32_NetworkAdapter WHERE GUID IS NOT NULL");

                foreach (var adapter in searcher.Get().Cast<ManagementObject>())
                {
                    var guid = NormalizeGuid(adapter["GUID"] as string);
                    if (string.IsNullOrWhiteSpace(guid))
                    {
                        continue;
                    }

                    map[guid] = (
                        adapter["PhysicalAdapter"] as bool?,
                        adapter["PNPDeviceID"] as string);
                }
            }
            catch (Exception ex)
            {
                Diagnostics.Warning("adapter_discovery_win32_map_failed", ex.Message);
            }

            return map;
        }

        private static string? NormalizeGuid(string? rawGuid)
        {
            if (string.IsNullOrWhiteSpace(rawGuid))
            {
                return null;
            }

            var trimmed = rawGuid.Trim();
            if (trimmed.StartsWith("{", StringComparison.Ordinal) && trimmed.EndsWith("}", StringComparison.Ordinal))
            {
                return trimmed.Substring(1, trimmed.Length - 2);
            }

            return trimmed;
        }
    }
}
