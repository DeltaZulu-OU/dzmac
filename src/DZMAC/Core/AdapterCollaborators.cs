#nullable enable

using System;
using System.IO;
using System.Linq;
using System.Management;
using System.Security;
using Microsoft.Win32;

namespace Dzmac.Core
{
    internal class AdapterWmiClient
    {
        public virtual bool TryResolveByConfigId(string configId, out ManagementObject? adapter, out ManagementObject? adapterConfig)
        {
            adapter = null;
            adapterConfig = null;

            if (string.IsNullOrWhiteSpace(configId))
            {
                Diagnostics.Warning("adapter_wmi_resolve_invalid_config_id", "Adapter config id is empty.");
                return false;
            }

            try
            {
                var escapedId = configId.Replace("'", "''");
                using var adapterSearcher = new ManagementObjectSearcher($"SELECT * FROM Win32_NetworkAdapter WHERE GUID = '{escapedId}'");
                using var adapterResults = adapterSearcher.Get();
                var adapterResult = adapterResults.Cast<ManagementObject>().FirstOrDefault();
                adapter = NetworkAdapter.CreateBoundManagementObject(adapterResult);

                using var configSearcher = new ManagementObjectSearcher($"SELECT * FROM Win32_NetworkAdapterConfiguration WHERE SettingID = '{escapedId}'");
                using var configResults = configSearcher.Get();
                var configResult = configResults.Cast<ManagementObject>().FirstOrDefault();
                adapterConfig = NetworkAdapter.CreateBoundManagementObject(configResult);
                var resolved = adapter != null || adapterConfig != null;
                if (!resolved)
                {
                    Diagnostics.Warning("adapter_wmi_resolve_not_found", "No Win32 adapter objects were resolved.", ("configId", configId));
                }

                return resolved;
            }
            catch (ManagementException ex)
            {
                Diagnostics.Warning("adapter_wmi_resolve_management_exception", ex.Message, ("configId", configId));
                return false;
            }
            catch (UnauthorizedAccessException ex)
            {
                Diagnostics.Warning("adapter_wmi_resolve_access_denied", ex.Message, ("configId", configId));
                return false;
            }
            catch (Exception ex)
            {
                Diagnostics.Warning("adapter_wmi_resolve_exception", ex.Message, ("configId", configId));
                return false;
            }
        }
    }

    internal class AdapterRegistryClient
    {
        public virtual string? TryResolveRegistryKey(string registryClassKey, string configId)
        {
            try
            {
                using var baseKey = Registry.LocalMachine.OpenSubKey(registryClassKey, false);
                if (baseKey == null)
                {
                    return null;
                }

                foreach (var name in baseKey.GetSubKeyNames())
                {
                    using var adapterKey = baseKey.OpenSubKey(name, false);
                    var keyConfigId = adapterKey?.GetValue("NetCfgInstanceId") as string;
                    if (string.Equals(keyConfigId, configId, StringComparison.OrdinalIgnoreCase))
                    {
                        return $"{registryClassKey}\\{name}";
                    }
                }
            }
            catch (SecurityException ex)
            {
                Diagnostics.Warning("adapter_registry_resolve_security_exception", ex.Message, ("configId", configId));
                return null;
            }
            catch (UnauthorizedAccessException ex)
            {
                Diagnostics.Warning("adapter_registry_resolve_access_denied", ex.Message, ("configId", configId));
                return null;
            }
            catch (IOException ex)
            {
                Diagnostics.Warning("adapter_registry_resolve_io_exception", ex.Message, ("configId", configId));
                return null;
            }
            catch (Exception ex)
            {
                Diagnostics.Warning("adapter_registry_resolve_exception", ex.Message, ("configId", configId));
                return null;
            }

            return null;
        }

        public virtual object? ReadValue(string registryKey, string valueName)
        {
            using var key = Registry.LocalMachine.OpenSubKey(registryKey, false);
            return key?.GetValue(valueName);
        }

        public virtual bool TryValidateAdapterDescription(string registryKey, string description)
        {
            using var key = Registry.LocalMachine.OpenSubKey(registryKey, false);
            if (key == null)
            {
                return false;
            }

            return string.Equals(key.GetValue("AdapterModel") as string, description, StringComparison.Ordinal)
                   || string.Equals(key.GetValue("DriverDesc") as string, description, StringComparison.Ordinal);
        }

        public virtual void SetStringValue(string registryKey, string valueName, string value)
        {
            using var key = Registry.LocalMachine.OpenSubKey(registryKey, true) ?? throw new DZMACException("Failed to open the registry key");
            key.SetValue(valueName, value, RegistryValueKind.String);
        }

        public virtual void DeleteValue(string registryKey, string valueName)
        {
            using var key = Registry.LocalMachine.OpenSubKey(registryKey, true) ?? throw new DZMACException("Failed to open the registry key");
            key.DeleteValue(valueName, false);
        }

        public virtual void DeleteKeyTree(string registryKey)
        {
            var separatorIndex = registryKey.LastIndexOf('\\');
            if (separatorIndex <= 0 || separatorIndex >= registryKey.Length - 1)
            {
                throw new DZMACException("Invalid registry key path.");
            }

            var parentPath = registryKey.Substring(0, separatorIndex);
            var subKeyName = registryKey.Substring(separatorIndex + 1);
            if (subKeyName.Length != 4 || !int.TryParse(subKeyName, out _))
            {
                throw new DZMACException("Invalid adapter registry key name.");
            }

            using var parentKey = Registry.LocalMachine.OpenSubKey(parentPath, true) ?? throw new DZMACException("Failed to open the parent registry key");
            parentKey.DeleteSubKeyTree(subKeyName, false);
        }

        public virtual void EnsureNetworkAddressParameter(string registryKey)
        {
            using var adapterKey = Registry.LocalMachine.OpenSubKey(registryKey, true) ?? throw new DZMACException("Failed to open the adapter registry key.");
            using var ndiKey = adapterKey.CreateSubKey("Ndi");
            using var paramsKey = ndiKey?.CreateSubKey("params");
            using var networkAddressKey = paramsKey?.CreateSubKey("NetworkAddress") ?? throw new DZMACException("Failed to create NetworkAddress registry parameter key.");
            networkAddressKey.SetValue("ParamDesc", "Network Address", RegistryValueKind.String);
            networkAddressKey.SetValue("type", "edit", RegistryValueKind.String);
            networkAddressKey.SetValue("LimitText", "12", RegistryValueKind.String);
            networkAddressKey.SetValue("UpperCase", "1", RegistryValueKind.String);
        }
    }
}
