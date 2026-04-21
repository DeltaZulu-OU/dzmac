using System;
using Microsoft.Win32;

namespace Dzmac.Core.Presets
{
    internal static class TpfFileAssociationService
    {
        private const string ExtensionKeyPath = @"Software\Classes\.tpf";
        private const string ClassKeyPath = @"Software\Classes\DZMAC.TpfPreset";
        private const string OpenCommandKeyPath = @"Software\Classes\DZMAC.TpfPreset\shell\open\command";
        private const string DefaultIconKeyPath = @"Software\Classes\DZMAC.TpfPreset\DefaultIcon";
        private const string ClassName = "DZMAC.TpfPreset";

        public static string BuildOpenCommand(string executablePath)
        {
            if (string.IsNullOrWhiteSpace(executablePath))
            {
                throw new ArgumentException("Executable path cannot be empty.", nameof(executablePath));
            }

            return $"\"{executablePath}\" \"%1\"";
        }

        public static void AssociateWithCurrentUser(string executablePath)
        {
            var commandValue = BuildOpenCommand(executablePath);

            using (var extensionKey = Registry.CurrentUser.CreateSubKey(ExtensionKeyPath))
            {
                extensionKey?.SetValue(string.Empty, ClassName, RegistryValueKind.String);
            }

            using (var classKey = Registry.CurrentUser.CreateSubKey(ClassKeyPath))
            {
                classKey?.SetValue(string.Empty, "Preset File", RegistryValueKind.String);
            }

            using (var openCommandKey = Registry.CurrentUser.CreateSubKey(OpenCommandKeyPath))
            {
                openCommandKey?.SetValue(string.Empty, commandValue, RegistryValueKind.String);
            }

            using (var defaultIconKey = Registry.CurrentUser.CreateSubKey(DefaultIconKeyPath))
            {
                defaultIconKey?.SetValue(string.Empty, $"\"{executablePath}\",0", RegistryValueKind.String);
            }
        }
    }
}
