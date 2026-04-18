#nullable enable

using System;
using System.Linq;
using System.Management;
using Microsoft.Win32;

namespace Dzmac.Gui.Core
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
    }
}
