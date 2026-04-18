using System.Windows.Forms;
using Dzmac.Gui.Cli;

namespace Dzmac.Gui.Forms
{
    internal partial class CommandLineParametersHelpForm : Form
    {
        public CommandLineParametersHelpForm()
        {
            InitializeComponent();
            HelpTextBox.Text = CommandLineHelpContent.Text;
        }
    }
}
