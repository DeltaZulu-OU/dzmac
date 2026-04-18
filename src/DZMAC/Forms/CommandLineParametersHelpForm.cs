using System.Drawing;
using System.Windows.Forms;
using Dzmac.Gui.Cli;

namespace Dzmac.Gui.Forms
{
    public sealed class CommandLineParametersHelpForm : Form
    {
        public CommandLineParametersHelpForm()
        {
            Text = "Command Line Parameters Help - DZMAC";
            StartPosition = FormStartPosition.CenterParent;
            Size = new Size(860, 620);
            MinimumSize = new Size(700, 500);

            var helpTextbox = new TextBox
            {
                Multiline = true,
                ReadOnly = true,
                ScrollBars = ScrollBars.Vertical,
                WordWrap = false,
                Dock = DockStyle.Fill,
                Font = new Font("Consolas", 10F, FontStyle.Regular, GraphicsUnit.Point),
                Text = CommandLineHelpContent.Text
            };

            Controls.Add(helpTextbox);
        }
    }
}