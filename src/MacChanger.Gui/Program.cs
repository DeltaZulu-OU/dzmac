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

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }

        // When logging is implemented, write the eror to log for diagnostics and exit.
        private static void UnhandledExceptionHandler(object sender, UnhandledExceptionEventArgs args) => _ = MessageBox.Show($"Unhandled exception caught : {((Exception)args.ExceptionObject).Message}", "Unhandled exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }
}