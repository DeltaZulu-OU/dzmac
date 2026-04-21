using System.Drawing;
using System.Windows.Forms;

namespace Dzmac.Forms
{
    internal sealed class TpfAssociationDialog : Form
    {
        public TpfAssociationDialog(string openCommand)
        {
            Text = "Associate .tpf Files";
            FormBorderStyle = FormBorderStyle.FixedDialog;
            StartPosition = FormStartPosition.CenterParent;
            MaximizeBox = false;
            MinimizeBox = false;
            ShowInTaskbar = false;
            ClientSize = new Size(460, 180);

            var messageLabel = new Label
            {
                AutoSize = false,
                Dock = DockStyle.Top,
                Height = 85,
                Text = "Associate .tpf preset files with DZMAC for the current Windows user.\r\n\r\n" +
                       "After association, double-clicking a .tpf file will open it in DZMAC.\r\n\r\n" +
                       $"Command: {openCommand}"
            };

            var buttonPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Bottom,
                FlowDirection = FlowDirection.RightToLeft,
                Height = 40,
                Padding = new Padding(0, 8, 0, 0)
            };

            var associateButton = new Button
            {
                Text = "Associate",
                DialogResult = DialogResult.OK,
                Width = 100
            };

            var cancelButton = new Button
            {
                Text = "Cancel",
                DialogResult = DialogResult.Cancel,
                Width = 100
            };

            buttonPanel.Controls.Add(associateButton);
            buttonPanel.Controls.Add(cancelButton);

            Controls.Add(messageLabel);
            Controls.Add(buttonPanel);

            AcceptButton = associateButton;
            CancelButton = cancelButton;
        }
    }
}
