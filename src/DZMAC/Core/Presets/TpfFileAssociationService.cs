using System;
using System.Runtime.InteropServices;
using Microsoft.Win32;

namespace Dzmac.Core.Presets
{
    internal static class TpfFileAssociationService
    {
        private const uint SHCNE_ASSOCCHANGED = 0x08000000;
        private const uint SHCNF_IDLIST = 0x0000;
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
                if (extensionKey == null)
                {
                    throw new InvalidOperationException($"Failed to create registry key '{ExtensionKeyPath}'.");
                }

                extensionKey.SetValue(string.Empty, ClassName, RegistryValueKind.String);
            }

            using (var classKey = Registry.CurrentUser.CreateSubKey(ClassKeyPath))
            {
                if (classKey == null)
                {
                    throw new InvalidOperationException($"Failed to create registry key '{ClassKeyPath}'.");
                }

                classKey.SetValue(string.Empty, "Preset File", RegistryValueKind.String);
            }

            using (var openCommandKey = Registry.CurrentUser.CreateSubKey(OpenCommandKeyPath))
            {
                if (openCommandKey == null)
                {
                    throw new InvalidOperationException($"Failed to create registry key '{OpenCommandKeyPath}'.");
                }

                openCommandKey.SetValue(string.Empty, commandValue, RegistryValueKind.String);
            }

            using (var defaultIconKey = Registry.CurrentUser.CreateSubKey(DefaultIconKeyPath))
            {
                if (defaultIconKey == null)
                {
                    throw new InvalidOperationException($"Failed to create registry key '{DefaultIconKeyPath}'.");
                }

                defaultIconKey.SetValue(string.Empty, $"\"{executablePath}\",0", RegistryValueKind.String);
            }

            SHChangeNotify(SHCNE_ASSOCCHANGED, SHCNF_IDLIST, IntPtr.Zero, IntPtr.Zero);
        }

        [DllImport("shell32.dll")]
        private static extern void SHChangeNotify(uint wEventId, uint uFlags, IntPtr dwItem1, IntPtr dwItem2);
    }
}
