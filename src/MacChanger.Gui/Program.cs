using System;
using System.Windows.Forms;
using MacChanger.Gui.Forms;

namespace MacChanger.Gui
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            AppDomain.CurrentDomain.UnhandledException += UnhandledExceptionHandler;
            Application.ApplicationExit += ApplicationExitHandler;

            Diagnostics.Info("application_start", ("host", "gui"));

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }

        private static void ApplicationExitHandler(object sender, EventArgs e) => Diagnostics.Info("application_stop", ("host", "gui"));

        // When logging is implemented, write the eror to log for diagnostics and exit.
        private static void UnhandledExceptionHandler(object sender, UnhandledExceptionEventArgs args)
        {
            var exception = (Exception)args.ExceptionObject;
            Diagnostics.Error("application_unhandled_exception", exception, "Unhandled exception in GUI host.", ("host", "gui"), ("isTerminating", args.IsTerminating));
            _ = MessageBox.Show($"Unhandled exception caught : {exception.Message}", "Unhandled exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
