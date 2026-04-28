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
                ["dhcp_action_failed"] = new DiagnosticsEventDefinition(1005, "DHCP action failed."),
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
                ["adapter_discovery_retained_disabled"] = new DiagnosticsEventDefinition(1209, "Adapter discovery retained disabled adapters."),
                ["adapter_discovery_msft_provider_unavailable"] = new DiagnosticsEventDefinition(1210, "Adapter discovery MSFT provider unavailable."),
                ["adapter_discovery_msft_provider_access_denied"] = new DiagnosticsEventDefinition(1211, "Adapter discovery MSFT provider access denied."),
                ["adapter_discovery_pnp_map_failed"] = new DiagnosticsEventDefinition(1212, "Adapter PnP map resolution failed."),
                ["adapter_discovery_win32_physical_flag_failed"] = new DiagnosticsEventDefinition(1213, "Adapter Win32 physical flag check failed."),
                ["adapter_enable_skipped"] = new DiagnosticsEventDefinition(1301, "Adapter enable skipped."),
                ["adapter_enable_failed"] = new DiagnosticsEventDefinition(1302, "Adapter enable failed."),
                ["adapter_enable_succeeded"] = new DiagnosticsEventDefinition(1303, "Adapter enable succeeded."),
                ["adapter_toggle_requested"] = new DiagnosticsEventDefinition(1304, "Adapter toggle requested from UI."),
                ["adapter_toggle_cancelled"] = new DiagnosticsEventDefinition(1305, "Adapter toggle cancelled by user."),
                ["adapter_toggle_succeeded"] = new DiagnosticsEventDefinition(1306, "Adapter toggle succeeded from UI."),
                ["adapter_toggle_failed"] = new DiagnosticsEventDefinition(1307, "Adapter toggle failed from UI."),
                ["adapter_enable_toggle_failed"] = new DiagnosticsEventDefinition(1308, "Adapter enable toggle failed."),
                ["adapter_reenable_failed"] = new DiagnosticsEventDefinition(1309, "Adapter re-enable failed."),
                ["adapter_config_method_skipped"] = new DiagnosticsEventDefinition(1350, "Adapter configuration method skipped."),
                ["adapter_config_method_failed"] = new DiagnosticsEventDefinition(1351, "Adapter configuration method failed."),
                ["adapter_config_method_non_success_code"] = new DiagnosticsEventDefinition(1352, "Adapter configuration method returned non-success code."),
                ["adapter_config_method_invalid_operation"] = new DiagnosticsEventDefinition(1353, "Adapter configuration method raised invalid operation."),
                ["adapter_config_method_management_exception"] = new DiagnosticsEventDefinition(1354, "Adapter configuration method raised management exception."),
                ["adapter_wmi_get_timeout"] = new DiagnosticsEventDefinition(1360, "Adapter WMI Get timed out."),
                ["adapter_wmi_resolve_invalid_config_id"] = new DiagnosticsEventDefinition(1361, "Adapter WMI resolve skipped: empty config ID."),
                ["adapter_wmi_resolve_not_found"] = new DiagnosticsEventDefinition(1362, "Adapter WMI object not found."),
                ["adapter_wmi_resolve_management_exception"] = new DiagnosticsEventDefinition(1363, "Adapter WMI resolve raised management exception."),
                ["adapter_wmi_resolve_access_denied"] = new DiagnosticsEventDefinition(1364, "Adapter WMI resolve access denied."),
                ["adapter_wmi_resolve_exception"] = new DiagnosticsEventDefinition(1365, "Adapter WMI resolve raised unexpected exception."),
                ["adapter_registry_resolve_security_exception"] = new DiagnosticsEventDefinition(1370, "Adapter registry resolve raised security exception."),
                ["adapter_registry_resolve_access_denied"] = new DiagnosticsEventDefinition(1371, "Adapter registry resolve access denied."),
                ["adapter_registry_resolve_io_exception"] = new DiagnosticsEventDefinition(1372, "Adapter registry resolve raised I/O exception."),
                ["adapter_registry_resolve_exception"] = new DiagnosticsEventDefinition(1373, "Adapter registry resolve raised unexpected exception."),
                ["adapter_registry_delete_requested"] = new DiagnosticsEventDefinition(1374, "Adapter registry key deletion requested."),
                ["adapter_registry_delete_cancelled"] = new DiagnosticsEventDefinition(1375, "Adapter registry key deletion cancelled."),
                ["adapter_registry_delete_succeeded"] = new DiagnosticsEventDefinition(1376, "Adapter registry key deletion succeeded."),
                ["adapter_registry_delete_failed"] = new DiagnosticsEventDefinition(1377, "Adapter registry key deletion failed."),
                ["registry_mac_update_started"] = new DiagnosticsEventDefinition(1401, "Registry MAC update started."),
                ["registry_mac_update_failed"] = new DiagnosticsEventDefinition(1402, "Registry MAC update failed."),
                ["mac_revert_failed"] = new DiagnosticsEventDefinition(1403, "MAC address revert failed."),
                ["vendor_list_initialized"] = new DiagnosticsEventDefinition(1501, "Vendor list initialized."),
                ["vendor_list_replaced"] = new DiagnosticsEventDefinition(1502, "Vendor list replaced."),
                ["vendor_refresh_started"] = new DiagnosticsEventDefinition(1503, "Vendor refresh started."),
                ["vendor_refresh_succeeded"] = new DiagnosticsEventDefinition(1504, "Vendor refresh succeeded."),
                ["vendor_refresh_failed"] = new DiagnosticsEventDefinition(1505, "Vendor refresh failed."),
                ["vendor_refresh_cancelled"] = new DiagnosticsEventDefinition(1506, "Vendor refresh cancelled."),
                ["oui_download_attempt"] = new DiagnosticsEventDefinition(1601, "OUI download attempt."),
                ["oui_download_completed"] = new DiagnosticsEventDefinition(1602, "OUI download completed."),
                ["oui_download_cancelled"] = new DiagnosticsEventDefinition(1603, "OUI download cancelled."),
                ["oui_download_retry"] = new DiagnosticsEventDefinition(1604, "OUI download retry."),
                ["oui_download_failed"] = new DiagnosticsEventDefinition(1605, "OUI download failed."),
                ["oui_integrity_manifest_fetch_start"] = new DiagnosticsEventDefinition(1606, "OUI integrity manifest fetch started."),
                ["oui_integrity_manifest_invalid_uri"] = new DiagnosticsEventDefinition(1607, "OUI integrity manifest endpoint URI is invalid."),
                ["oui_integrity_manifest_invalid"] = new DiagnosticsEventDefinition(1608, "OUI integrity manifest is invalid."),
                ["oui_integrity_mismatch"] = new DiagnosticsEventDefinition(1609, "OUI integrity checksum mismatch."),
                ["oui_integrity_verified"] = new DiagnosticsEventDefinition(1610, "OUI integrity verified."),
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
                ["tpf_preset_count_truncated"] = new DiagnosticsEventDefinition(1909, "TPF preset count exceeds limit."),
                ["config_missing"] = new DiagnosticsEventDefinition(2001, "Configuration setting missing."),
                ["config_invalid"] = new DiagnosticsEventDefinition(2002, "Configuration setting invalid."),
                ["config_legacy_alias"] = new DiagnosticsEventDefinition(2003, "Legacy configuration key in use."),
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
