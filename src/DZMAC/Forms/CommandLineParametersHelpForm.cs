using System.Windows.Forms;
using Dzmac.Cli;

namespace Dzmac.Forms
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
