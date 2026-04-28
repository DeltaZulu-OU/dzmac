using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Dzmac.Cli;
using Dzmac.Core;
using Dzmac.Forms;
using Dzmac.Properties;

namespace Dzmac
{
    internal static class Program
    {
        private const uint AttachParentProcess = 0xFFFFFFFF;

        [STAThread]
        private static int Main(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException += UnhandledExceptionHandler;
            Application.ApplicationExit += ApplicationExitHandler;
            ConfigReader.Current.ValidateAndWarn();

            if (args is null || args.Length == 0)
            {
                Diagnostics.Info("application_start", ("host", "gui"));
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new MainForm());
                return 0;
            }

            if (args.Length == 1 && args[0].EndsWith(".tpf", StringComparison.OrdinalIgnoreCase))
            {
                Diagnostics.Info("application_start", ("host", "gui"), ("source", "file_association"));
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new MainForm(args[0]));
                return 0;
            }

            Diagnostics.Info("application_start", ("host", "cli"));
            EnsureConsole();
            return CliHandler.Run(args);
        }

        private static void ApplicationExitHandler(object sender, EventArgs e)
        {
            var host = GetConsoleWindow() == IntPtr.Zero ? "gui" : "cli";
            Diagnostics.Info("application_stop", ("host", host));
        }

        private static void EnsureConsole()
        {
            if (GetConsoleWindow() != IntPtr.Zero)
            {
                return;
            }

            if (!AttachConsole(AttachParentProcess))
            {
                _ = AllocConsole();
            }
        }

        // When logging is implemented, write the error to log for diagnostics and exit.
        private static void UnhandledExceptionHandler(object sender, UnhandledExceptionEventArgs args)
        {
            var exception = (Exception)args.ExceptionObject;
            Diagnostics.Error("application_unhandled_exception", exception, "Unhandled exception in host.", ("isTerminating", args.IsTerminating));

            if (GetConsoleWindow() != IntPtr.Zero)
            {
                Console.Error.WriteLine($"Unhandled exception: {exception.Message}");
                return;
            }

            _ = MessageBox.Show($"Unhandled exception caught : {exception.Message}", Resources.UnhandledException_Title, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool AttachConsole(uint dwProcessId);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool AllocConsole();

        [DllImport("kernel32.dll")]
        private static extern IntPtr GetConsoleWindow();
    }
}
