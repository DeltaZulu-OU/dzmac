#nullable enable

using System;
using System.Linq;
using System.Management;
using Microsoft.Win32;

namespace Dzmac.Core
{
    public interface IAdapterWmiClient
    {
        bool TryResolveByConfigId(string configId, out ManagementObject? adapter, out ManagementObject? adapterConfig);
    }

    public interface IAdapterRegistryClient
    {
        string? TryResolveRegistryKey(string registryClassKey, string configId);

        object? ReadValue(string registryKey, string valueName);

        bool TryValidateAdapterDescription(string registryKey, string description);

        void SetStringValue(string registryKey, string valueName, string value);

        void DeleteValue(string registryKey, string valueName);

        void DeleteKeyTree(string registryKey);

        void EnsureNetworkAddressParameter(string registryKey);
    }

    public sealed class AdapterWmiClient : IAdapterWmiClient
    {
        public bool TryResolveByConfigId(string configId, out ManagementObject? adapter, out ManagementObject? adapterConfig)
        {
            adapter = null;
            adapterConfig = null;

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
                return true;
            }
            catch
            {
                return false;
            }
        }
    }

    public sealed class AdapterRegistryClient : IAdapterRegistryClient
    {
        public string? TryResolveRegistryKey(string registryClassKey, string configId)
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
            catch
            {
                return null;
            }

            return null;
        }

        public object? ReadValue(string registryKey, string valueName)
        {
            using var key = Registry.LocalMachine.OpenSubKey(registryKey, false);
            return key?.GetValue(valueName);
        }

        public bool TryValidateAdapterDescription(string registryKey, string description)
        {
            using var key = Registry.LocalMachine.OpenSubKey(registryKey, false);
            if (key == null)
            {
                return false;
            }

            return string.Equals(key.GetValue("AdapterModel") as string, description, StringComparison.Ordinal)
                   || string.Equals(key.GetValue("DriverDesc") as string, description, StringComparison.Ordinal);
        }

        public void SetStringValue(string registryKey, string valueName, string value)
        {
            using var key = Registry.LocalMachine.OpenSubKey(registryKey, true) ?? throw new DZMACException("Failed to open the registry key");
            key.SetValue(valueName, value, RegistryValueKind.String);
        }

        public void DeleteValue(string registryKey, string valueName)
        {
            using var key = Registry.LocalMachine.OpenSubKey(registryKey, true) ?? throw new DZMACException("Failed to open the registry key");
            key.DeleteValue(valueName, false);
        }

        public void DeleteKeyTree(string registryKey)
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

        public void EnsureNetworkAddressParameter(string registryKey)
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