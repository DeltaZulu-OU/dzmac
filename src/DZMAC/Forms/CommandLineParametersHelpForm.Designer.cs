namespace Dzmac.Forms
{
    internal partial class CommandLineParametersHelpForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }

            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.HelpTextBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            //
            // HelpTextBox
            //
            this.HelpTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.HelpTextBox.Font = new System.Drawing.Font("Consolas", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.HelpTextBox.Location = new System.Drawing.Point(0, 0);
            this.HelpTextBox.Multiline = true;
            this.HelpTextBox.Name = "HelpTextBox";
            this.HelpTextBox.ReadOnly = true;
            this.HelpTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.HelpTextBox.Size = new System.Drawing.Size(844, 581);
            this.HelpTextBox.TabIndex = 0;
            this.HelpTextBox.WordWrap = false;
            //
            // CommandLineParametersHelpForm
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(844, 581);
            this.Controls.Add(this.HelpTextBox);
            this.MinimumSize = new System.Drawing.Size(700, 500);
            this.Name = "CommandLineParametersHelpForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Command Line Parameters Help - DZMAC";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.TextBox HelpTextBox;
    }
}
