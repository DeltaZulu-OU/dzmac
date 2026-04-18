#nullable enable

using System;
using System.Collections.Generic;
using System.Configuration;

namespace DZMACLib
{
    public interface IAppSettings
    {
        string GetString(string key);
        int GetInt(string key);
        bool GetBool(string key);
        void ValidateAndWarn();
    }

    public static class AppSettingKeys
    {
        public const string VerboseDiagnostics = "Dzmac.VerboseDiagnostics";
        public const string OuiCachePath = "Dzmac.OuiCachePath";
        public const string OuiEndpoint = "Dzmac.OuiEndpoint";
        public const string OuiIntegrityManifestEndpoint = "Dzmac.OuiIntegrityManifestEndpoint";
        public const string OuiDownloadTimeoutSeconds = "Dzmac.OuiDownloadTimeoutSeconds";
        public const string OuiDownloadRetryCount = "Dzmac.OuiDownloadRetryCount";
        public const string AdminOperationTimeoutSeconds = "Dzmac.AdminOperationTimeoutSeconds";
        public const string AdminOperationRetryCount = "Dzmac.AdminOperationRetryCount";

        public const string LegacyVerboseDiagnostics = "DZMACLib.VerboseDiagnostics";
        public const string LegacyOuiCachePath = "DZMACLib.OuiCachePath";
        public const string LegacyOuiEndpoint = "DZMACLib.OuiEndpoint";
        public const string LegacyOuiIntegrityManifestEndpoint = "DZMACLib.OuiIntegrityManifestEndpoint";
        public const string LegacyOuiDownloadTimeoutSeconds = "DZMACLib.OuiDownloadTimeoutSeconds";
        public const string LegacyOuiDownloadRetryCount = "DZMACLib.OuiDownloadRetryCount";
        public const string LegacyAdminOperationTimeoutSeconds = "DZMACLib.AdminOperationTimeoutSeconds";
        public const string LegacyAdminOperationRetryCount = "DZMACLib.AdminOperationRetryCount";
    }

    public sealed class ConfigReader : IAppSettings
    {
        private static readonly ConfigReader _instance = new ConfigReader();
        private readonly Dictionary<string, SettingDefinition> _definitions;

        public static ConfigReader Current => _instance;

        private ConfigReader()
        {
            _definitions = new Dictionary<string, SettingDefinition>(StringComparer.OrdinalIgnoreCase)
            {
                [AppSettingKeys.VerboseDiagnostics] = new SettingDefinition(AppSettingKeys.VerboseDiagnostics, "false", AppSettingKeys.LegacyVerboseDiagnostics),
                [AppSettingKeys.OuiCachePath] = new SettingDefinition(AppSettingKeys.OuiCachePath, string.Empty, AppSettingKeys.LegacyOuiCachePath),
                [AppSettingKeys.OuiEndpoint] = new SettingDefinition(AppSettingKeys.OuiEndpoint, "https://standards-oui.ieee.org/oui/oui.txt", AppSettingKeys.LegacyOuiEndpoint),
                [AppSettingKeys.OuiIntegrityManifestEndpoint] = new SettingDefinition(AppSettingKeys.OuiIntegrityManifestEndpoint, string.Empty, AppSettingKeys.LegacyOuiIntegrityManifestEndpoint),
                [AppSettingKeys.OuiDownloadTimeoutSeconds] = new SettingDefinition(AppSettingKeys.OuiDownloadTimeoutSeconds, "15", AppSettingKeys.LegacyOuiDownloadTimeoutSeconds),
                [AppSettingKeys.OuiDownloadRetryCount] = new SettingDefinition(AppSettingKeys.OuiDownloadRetryCount, "3", AppSettingKeys.LegacyOuiDownloadRetryCount),
                [AppSettingKeys.AdminOperationTimeoutSeconds] = new SettingDefinition(AppSettingKeys.AdminOperationTimeoutSeconds, "20", AppSettingKeys.LegacyAdminOperationTimeoutSeconds),
                [AppSettingKeys.AdminOperationRetryCount] = new SettingDefinition(AppSettingKeys.AdminOperationRetryCount, "1", AppSettingKeys.LegacyAdminOperationRetryCount)
            };
        }

        public string GetString(string key)
        {
            var definition = GetDefinition(key);
            if (TryResolveValue(definition, out var resolved))
            {
                return resolved;
            }

            return definition.DefaultValue;
        }

        public int GetInt(string key)
        {
            var definition = GetDefinition(key);
            var value = GetString(key);
            if (int.TryParse(value, out var parsed))
            {
                return parsed;
            }

            if (int.TryParse(definition.DefaultValue, out var fallback))
            {
                return fallback;
            }

            return 0;
        }

        public bool GetBool(string key)
        {
            var definition = GetDefinition(key);
            var value = GetString(key);
            if (bool.TryParse(value, out var parsed))
            {
                return parsed;
            }

            if (bool.TryParse(definition.DefaultValue, out var fallback))
            {
                return fallback;
            }

            return false;
        }

        public void ValidateAndWarn()
        {
            ValidateInt(AppSettingKeys.OuiDownloadTimeoutSeconds, minInclusive: 1);
            ValidateInt(AppSettingKeys.OuiDownloadRetryCount, minInclusive: 1);
            ValidateInt(AppSettingKeys.AdminOperationTimeoutSeconds, minInclusive: 1);
            ValidateInt(AppSettingKeys.AdminOperationRetryCount, minInclusive: 1);

            var endpoint = GetString(AppSettingKeys.OuiEndpoint);
            if (!Uri.TryCreate(endpoint, UriKind.Absolute, out _))
            {
                Diagnostics.Warning("config_invalid", "Invalid OUI endpoint; default value will be used.", ("key", AppSettingKeys.OuiEndpoint), ("value", endpoint));
            }

            var manifestEndpoint = GetString(AppSettingKeys.OuiIntegrityManifestEndpoint);
            if (!string.IsNullOrWhiteSpace(manifestEndpoint) && !Uri.TryCreate(manifestEndpoint, UriKind.Absolute, out _))
            {
                Diagnostics.Warning("config_invalid", "Invalid OUI integrity manifest endpoint; integrity verification is disabled for this run.", ("key", AppSettingKeys.OuiIntegrityManifestEndpoint), ("value", manifestEndpoint));
            }
        }

        private void ValidateInt(string key, int minInclusive)
        {
            var definition = GetDefinition(key);
            if (!TryResolveValue(definition, out var value))
            {
                Diagnostics.Warning("config_missing", "Required setting was not found; default value will be used.", ("key", key), ("default", definition.DefaultValue));
                return;
            }

            if (!int.TryParse(value, out var parsed) || parsed < minInclusive)
            {
                Diagnostics.Warning("config_invalid", "Invalid numeric setting; default value will be used.", ("key", key), ("value", value), ("default", definition.DefaultValue));
            }
        }

        private bool TryResolveValue(SettingDefinition definition, out string value)
        {
            var canonical = ConfigurationManager.AppSettings[definition.Key];
            if (!string.IsNullOrWhiteSpace(canonical))
            {
                value = canonical;
                return true;
            }

            if (!string.IsNullOrWhiteSpace(definition.LegacyAliasKey))
            {
                var legacy = ConfigurationManager.AppSettings[definition.LegacyAliasKey];
                if (!string.IsNullOrWhiteSpace(legacy))
                {
                    Diagnostics.Warning("config_legacy_alias", "Legacy appSetting key is in use; migrate to canonical Dzmac.* key.", ("legacyKey", definition.LegacyAliasKey), ("canonicalKey", definition.Key));
                    value = legacy;
                    return true;
                }
            }

            value = definition.DefaultValue;
            return false;
        }

        private SettingDefinition GetDefinition(string key)
        {
            if (!_definitions.TryGetValue(key, out var definition))
            {
                throw new DZMACLibException($"Unknown application setting key: {key}");
            }

            return definition;
        }

        private sealed class SettingDefinition
        {
            public string Key { get; }
            public string DefaultValue { get; }
            public string LegacyAliasKey { get; }

            public SettingDefinition(string key, string defaultValue, string legacyAliasKey)
            {
                Key = key;
                DefaultValue = defaultValue;
                LegacyAliasKey = legacyAliasKey;
            }
        }
    }
}
