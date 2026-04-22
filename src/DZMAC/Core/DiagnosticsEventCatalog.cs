#nullable enable

using System;
using System.Collections.Generic;

namespace Dzmac.Core
{
    internal sealed class DiagnosticsEventDefinition
    {
        public DiagnosticsEventDefinition(int id, string template)
        {
            Id = id;
            Template = template;
        }

        public int Id { get; }

        public string Template { get; }
    }

    internal static class DiagnosticsEventCatalog
    {
        private static readonly IReadOnlyDictionary<string, DiagnosticsEventDefinition> EventDefinitions =
            new Dictionary<string, DiagnosticsEventDefinition>(StringComparer.OrdinalIgnoreCase)
            {
                ["dhcp_disable_skipped"] = new DiagnosticsEventDefinition(1001, "DHCP disable skipped."),
                ["dhcp_disable_failed_rollback_success"] = new DiagnosticsEventDefinition(1002, "DHCP disable failed but rollback succeeded."),
                ["dhcp_disable_failed_rollback_failed"] = new DiagnosticsEventDefinition(1003, "DHCP disable failed and rollback failed."),
                ["dhcp_disable_succeeded"] = new DiagnosticsEventDefinition(1004, "DHCP disable succeeded."),
                ["dhcp_enable_skipped"] = new DiagnosticsEventDefinition(1101, "DHCP enable skipped."),
                ["dhcp_enable_failed_rollback_success"] = new DiagnosticsEventDefinition(1102, "DHCP enable failed but rollback succeeded."),
                ["dhcp_enable_failed_rollback_failed"] = new DiagnosticsEventDefinition(1103, "DHCP enable failed and rollback failed."),
                ["dhcp_enable_succeeded"] = new DiagnosticsEventDefinition(1104, "DHCP enable succeeded."),
                ["adapter_disable_skipped"] = new DiagnosticsEventDefinition(1201, "Adapter disable skipped."),
                ["adapter_disable_failed"] = new DiagnosticsEventDefinition(1202, "Adapter disable failed."),
                ["adapter_disable_succeeded"] = new DiagnosticsEventDefinition(1203, "Adapter disable succeeded."),
                ["adapter_discovery_started"] = new DiagnosticsEventDefinition(1204, "Adapter discovery started."),
                ["adapter_discovery_completed"] = new DiagnosticsEventDefinition(1205, "Adapter discovery completed."),
                ["adapter_discovery_failed"] = new DiagnosticsEventDefinition(1206, "Adapter discovery failed."),
                ["adapter_discovery_raw_count"] = new DiagnosticsEventDefinition(1207, "Adapter discovery raw count."),
                ["adapter_discovery_inventory"] = new DiagnosticsEventDefinition(1208, "Adapter discovery inventory."),
                ["adapter_discovery_retained_disabled"] = new DiagnosticsEventDefinition(1209, "Adapter discovery retained disabled adapters."),
                ["adapter_enable_skipped"] = new DiagnosticsEventDefinition(1301, "Adapter enable skipped."),
                ["adapter_enable_failed"] = new DiagnosticsEventDefinition(1302, "Adapter enable failed."),
                ["adapter_enable_succeeded"] = new DiagnosticsEventDefinition(1303, "Adapter enable succeeded."),
                ["adapter_toggle_requested"] = new DiagnosticsEventDefinition(1304, "Adapter toggle requested from UI."),
                ["adapter_toggle_cancelled"] = new DiagnosticsEventDefinition(1305, "Adapter toggle cancelled by user."),
                ["adapter_toggle_succeeded"] = new DiagnosticsEventDefinition(1306, "Adapter toggle succeeded from UI."),
                ["adapter_toggle_failed"] = new DiagnosticsEventDefinition(1307, "Adapter toggle failed from UI."),
                ["registry_mac_update_started"] = new DiagnosticsEventDefinition(1401, "Registry MAC update started."),
                ["registry_mac_update_failed"] = new DiagnosticsEventDefinition(1402, "Registry MAC update failed."),
                ["vendor_cache_initialized"] = new DiagnosticsEventDefinition(1501, "Vendor cache initialized."),
                ["vendor_cache_refreshed"] = new DiagnosticsEventDefinition(1502, "Vendor cache refreshed."),
                ["vendor_refresh_started"] = new DiagnosticsEventDefinition(1503, "Vendor refresh started."),
                ["vendor_refresh_succeeded"] = new DiagnosticsEventDefinition(1504, "Vendor refresh succeeded."),
                ["vendor_refresh_failed"] = new DiagnosticsEventDefinition(1505, "Vendor refresh failed."),
                ["oui_download_attempt"] = new DiagnosticsEventDefinition(1601, "OUI download attempt."),
                ["oui_download_completed"] = new DiagnosticsEventDefinition(1602, "OUI download completed."),
                ["oui_download_cancelled"] = new DiagnosticsEventDefinition(1603, "OUI download cancelled."),
                ["oui_download_retry"] = new DiagnosticsEventDefinition(1604, "OUI download retry."),
                ["oui_download_failed"] = new DiagnosticsEventDefinition(1605, "OUI download failed."),
                ["oui_parse_start"] = new DiagnosticsEventDefinition(1701, "OUI parse started."),
                ["oui_parse_completed"] = new DiagnosticsEventDefinition(1702, "OUI parse completed."),
                ["admin_operation_completed"] = new DiagnosticsEventDefinition(1801, "Admin operation completed."),
                ["admin_operation_retry"] = new DiagnosticsEventDefinition(1802, "Admin operation retry."),
                ["tpf_load_start"] = new DiagnosticsEventDefinition(1901, "TPF file load started."),
                ["tpf_parse_start"] = new DiagnosticsEventDefinition(1902, "TPF parsing started."),
                ["tpf_parse_completed"] = new DiagnosticsEventDefinition(1903, "TPF parsing completed."),
                ["tpf_parse_warning"] = new DiagnosticsEventDefinition(1904, "TPF parsing warning."),
                ["tpf_parse_rejected"] = new DiagnosticsEventDefinition(1905, "TPF payload rejected."),
                ["tpf_preset_rejected"] = new DiagnosticsEventDefinition(1906, "TPF preset rejected."),
                ["tpf_save_start"] = new DiagnosticsEventDefinition(1907, "TPF file save started."),
                ["tpf_save_completed"] = new DiagnosticsEventDefinition(1908, "TPF file save completed."),
                ["application_start"] = new DiagnosticsEventDefinition(9001, "Application started."),
                ["application_stop"] = new DiagnosticsEventDefinition(9002, "Application stopped."),
                ["application_unhandled_exception"] = new DiagnosticsEventDefinition(9003, "Application unhandled exception."),
            };

        public static DiagnosticsEventDefinition Resolve(string eventName)
        {
            if (EventDefinitions.TryGetValue(eventName, out var definition))
            {
                return definition;
            }

            var fallbackId = Math.Abs(eventName.GetHashCode()) % 20000 + 30000;
            return new DiagnosticsEventDefinition(fallbackId, "Uncatalogued diagnostic event.");
        }
    }
}
