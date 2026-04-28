using System.Windows.Forms;
using Dzmac.Cli;
using Dzmac.Core;

namespace Dzmac.Forms
{
    internal partial class CommandLineParametersHelpForm : Form
    {
        public CommandLineParametersHelpForm()
        {
            InitializeComponent();
            Icon = AppIconProvider.GetIcon();
            HelpTextBox!.Text = CommandLineHelpContent.Text;
        }
    }
}
