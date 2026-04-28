#nullable enable

namespace Dzmac.Forms
{
    internal partial class AboutBox
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer? components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components is not null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AboutBox));
            this.RootLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.logoPictureBox = new System.Windows.Forms.PictureBox();
            this.ProductNameLabel = new System.Windows.Forms.Label();
            this.VersionLabel = new System.Windows.Forms.Label();
            this.CopyrightLabel = new System.Windows.Forms.Label();
            this.DescriptionLabel = new System.Windows.Forms.Label();
            this.ProjectLinkLabel = new System.Windows.Forms.LinkLabel();
            this.CloseButton = new System.Windows.Forms.Button();
            this.RootLayoutPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.logoPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // RootLayoutPanel
            // 
            this.RootLayoutPanel.ColumnCount = 1;
            this.RootLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.RootLayoutPanel.Controls.Add(this.ProductNameLabel, 0, 1);
            this.RootLayoutPanel.Controls.Add(this.VersionLabel, 0, 2);
            this.RootLayoutPanel.Controls.Add(this.CopyrightLabel, 0, 3);
            this.RootLayoutPanel.Controls.Add(this.DescriptionLabel, 0, 4);
            this.RootLayoutPanel.Controls.Add(this.ProjectLinkLabel, 0, 5);
            this.RootLayoutPanel.Controls.Add(this.CloseButton, 0, 6);
            this.RootLayoutPanel.Controls.Add(this.logoPictureBox, 0, 0);
            this.RootLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RootLayoutPanel.Location = new System.Drawing.Point(12, 12);
            this.RootLayoutPanel.Name = "RootLayoutPanel";
            this.RootLayoutPanel.RowCount = 7;
            this.RootLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 64F));
            this.RootLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.RootLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.RootLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.RootLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.RootLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.RootLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.RootLayoutPanel.Size = new System.Drawing.Size(416, 236);
            this.RootLayoutPanel.TabIndex = 0;
            // 
            // logoPictureBox
            // 
            this.logoPictureBox.Dock = System.Windows.Forms.DockStyle.Right;
            this.logoPictureBox.Image = ((System.Drawing.Image)(resources.GetObject("logoPictureBox.Image")));
            this.logoPictureBox.Location = new System.Drawing.Point(381, 3);
            this.logoPictureBox.Name = "logoPictureBox";
            this.logoPictureBox.Size = new System.Drawing.Size(32, 58);
            this.logoPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.logoPictureBox.TabIndex = 0;
            this.logoPictureBox.TabStop = false;
            // 
            // ProductNameLabel
            // 
            this.ProductNameLabel.AutoSize = true;
            this.ProductNameLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold);
            this.ProductNameLabel.Location = new System.Drawing.Point(3, 64);
            this.ProductNameLabel.Margin = new System.Windows.Forms.Padding(3, 0, 3, 10);
            this.ProductNameLabel.Name = "ProductNameLabel";
            this.ProductNameLabel.Size = new System.Drawing.Size(66, 18);
            this.ProductNameLabel.TabIndex = 1;
            this.ProductNameLabel.Text = "DZMAC";
            // 
            // VersionLabel
            // 
            this.VersionLabel.AutoSize = true;
            this.VersionLabel.Location = new System.Drawing.Point(3, 92);
            this.VersionLabel.Margin = new System.Windows.Forms.Padding(3, 0, 3, 8);
            this.VersionLabel.Name = "VersionLabel";
            this.VersionLabel.Size = new System.Drawing.Size(42, 13);
            this.VersionLabel.TabIndex = 2;
            this.VersionLabel.Text = "Version";
            // 
            // CopyrightLabel
            // 
            this.CopyrightLabel.AutoSize = true;
            this.CopyrightLabel.Location = new System.Drawing.Point(3, 113);
            this.CopyrightLabel.Margin = new System.Windows.Forms.Padding(3, 0, 3, 8);
            this.CopyrightLabel.Name = "CopyrightLabel";
            this.CopyrightLabel.Size = new System.Drawing.Size(51, 13);
            this.CopyrightLabel.TabIndex = 3;
            this.CopyrightLabel.Text = "Copyright";
            // 
            // DescriptionLabel
            // 
            this.DescriptionLabel.AutoSize = true;
            this.DescriptionLabel.Location = new System.Drawing.Point(3, 134);
            this.DescriptionLabel.MaximumSize = new System.Drawing.Size(410, 0);
            this.DescriptionLabel.Name = "DescriptionLabel";
            this.DescriptionLabel.Size = new System.Drawing.Size(266, 13);
            this.DescriptionLabel.TabIndex = 4;
            this.DescriptionLabel.Text = "Change and manage network adapter MAC addresses.";
            // 
            // ProjectLinkLabel
            // 
            this.ProjectLinkLabel.AutoSize = true;
            this.ProjectLinkLabel.Location = new System.Drawing.Point(3, 186);
            this.ProjectLinkLabel.Margin = new System.Windows.Forms.Padding(3, 12, 3, 8);
            this.ProjectLinkLabel.Name = "ProjectLinkLabel";
            this.ProjectLinkLabel.Size = new System.Drawing.Size(201, 13);
            this.ProjectLinkLabel.TabIndex = 5;
            this.ProjectLinkLabel.TabStop = true;
            this.ProjectLinkLabel.Text = "https://github.com/DeltaZulu-OU/dzmac";
            this.ProjectLinkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.ProjectLinkLabel_LinkClicked);
            // 
            // CloseButton
            // 
            this.CloseButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.CloseButton.Location = new System.Drawing.Point(338, 210);
            this.CloseButton.Name = "CloseButton";
            this.CloseButton.Size = new System.Drawing.Size(75, 23);
            this.CloseButton.TabIndex = 6;
            this.CloseButton.Text = "Close";
            this.CloseButton.UseVisualStyleBackColor = true;
            this.CloseButton.Click += new System.EventHandler(this.CloseButton_Click);
            // 
            // AboutBox
            // 
            this.AcceptButton = this.CloseButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(440, 260);
            this.Controls.Add(this.RootLayoutPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AboutBox";
            this.Padding = new System.Windows.Forms.Padding(12);
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "About";
            this.RootLayoutPanel.ResumeLayout(false);
            this.RootLayoutPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.logoPictureBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel? RootLayoutPanel;
        private System.Windows.Forms.PictureBox? logoPictureBox;
        private System.Windows.Forms.Label? ProductNameLabel;
        private System.Windows.Forms.Label? VersionLabel;
        private System.Windows.Forms.Label? CopyrightLabel;
        private System.Windows.Forms.Label? DescriptionLabel;
        private System.Windows.Forms.LinkLabel? ProjectLinkLabel;
        private System.Windows.Forms.Button? CloseButton;
    }
}
