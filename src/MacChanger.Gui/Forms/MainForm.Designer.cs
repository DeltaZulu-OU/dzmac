using BrightIdeasSoftware;
using System.Drawing;
using System.Windows.Forms;

namespace MacChanger.Gui.Forms
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
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
            this.MainTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.InfoTabs = new System.Windows.Forms.TabControl();
            this.InformationPage = new System.Windows.Forms.TabPage();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.ConnectionDetailsGroup = new System.Windows.Forms.GroupBox();
            this.ActiveMacVendorTextbox = new System.Windows.Forms.TextBox();
            this.ActiveMacValueTextbox = new System.Windows.Forms.TextBox();
            this.ActiveMacLabel = new System.Windows.Forms.Label();
            this.OriginalMacVendorTextbox = new System.Windows.Forms.TextBox();
            this.OriginalMacValueTextbox = new System.Windows.Forms.TextBox();
            this.OriginalMacLabel = new System.Windows.Forms.Label();
            this.Ipv6ValueTextbox = new System.Windows.Forms.TextBox();
            this.Ipv6Label = new System.Windows.Forms.Label();
            this.Ipv4ValueTextbox = new System.Windows.Forms.TextBox();
            this.Ipv4Label = new System.Windows.Forms.Label();
            this.ConfigIdValueTextbox = new System.Windows.Forms.TextBox();
            this.ConfigIdLabel = new System.Windows.Forms.Label();
            this.HardwareIdValueTextbox = new System.Windows.Forms.TextBox();
            this.HardwareIdLabel = new System.Windows.Forms.Label();
            this.DeviceValueTextbox = new System.Windows.Forms.TextBox();
            this.DeviceLabel = new System.Windows.Forms.Label();
            this.ConnectionValueTextbox = new System.Windows.Forms.TextBox();
            this.ConnectionLabel = new System.Windows.Forms.Label();
            this.ChangeMacAddressGroup = new System.Windows.Forms.GroupBox();
            this.RestoreMacButton = new System.Windows.Forms.Button();
            this.ChangeMacButton = new System.Windows.Forms.Button();
            this.ZeroTwoCheckBox = new System.Windows.Forms.CheckBox();
            this.PersistentAddressCheckBox = new System.Windows.Forms.CheckBox();
            this.AutoStartCheckBox = new System.Windows.Forms.CheckBox();
            this.VendorComboBox = new System.Windows.Forms.ComboBox();
            this.RandomMacButton = new System.Windows.Forms.Button();
            this.macTextBox1 = new MacChanger.Gui.Controls.MacTextBox();
            this.PerformanceCounterGroup = new System.Windows.Forms.GroupBox();
            this.IPAddressPage = new System.Windows.Forms.TabPage();
            this.PresetsPage = new System.Windows.Forms.TabPage();
            this.ConnectionsGrid = new BrightIdeasSoftware.DataListView();
            this.Toolbar = new System.Windows.Forms.MenuStrip();
            this.FileMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.ExportReportItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.OpenPresetItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SavePresetItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SavePresetAsItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.ImportPresetItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ExportPresetItem = new System.Windows.Forms.ToolStripMenuItem();
            this.AssociateItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.ExitItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ActionMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.RefreshItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.Dhcp4Item = new System.Windows.Forms.ToolStripMenuItem();
            this.Dhcp4EnabledItem = new System.Windows.Forms.ToolStripMenuItem();
            this.Dhcp4ReleaseIpItem = new System.Windows.Forms.ToolStripMenuItem();
            this.Dhcp4RenewIpItem = new System.Windows.Forms.ToolStripMenuItem();
            this.Dhcp6Item = new System.Windows.Forms.ToolStripMenuItem();
            this.Dhcp6EnabledItem = new System.Windows.Forms.ToolStripMenuItem();
            this.Dhcp6ReleaseIpItem = new System.Windows.Forms.ToolStripMenuItem();
            this.Dhcp6RenewItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.DeleteItem = new System.Windows.Forms.ToolStripMenuItem();
            this.OptionsMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.HelpMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.HelpTopicsItem = new System.Windows.Forms.ToolStripMenuItem();
            this.CliParamsHelpItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.CheckUpdateItem = new System.Windows.Forms.ToolStripMenuItem();
            this.UpdateOuiItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.AboutItem = new System.Windows.Forms.ToolStripMenuItem();
            this.MainStatusBar = new System.Windows.Forms.StatusBar();
            this.WikiLink = new System.Windows.Forms.LinkLabel();
            this.MainTableLayoutPanel.SuspendLayout();
            this.InfoTabs.SuspendLayout();
            this.InformationPage.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.ConnectionDetailsGroup.SuspendLayout();
            this.ChangeMacAddressGroup.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ConnectionsGrid)).BeginInit();
            this.Toolbar.SuspendLayout();
            this.SuspendLayout();
            // 
            // MainTableLayoutPanel
            // 
            this.MainTableLayoutPanel.AutoSize = true;
            this.MainTableLayoutPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.MainTableLayoutPanel.CausesValidation = false;
            this.MainTableLayoutPanel.ColumnCount = 1;
            this.MainTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.MainTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.MainTableLayoutPanel.Controls.Add(this.InfoTabs, 0, 1);
            this.MainTableLayoutPanel.Controls.Add(this.ConnectionsGrid, 0, 0);
            this.MainTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainTableLayoutPanel.Location = new System.Drawing.Point(0, 24);
            this.MainTableLayoutPanel.Name = "MainTableLayoutPanel";
            this.MainTableLayoutPanel.RowCount = 2;
            this.MainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.MainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 75F));
            this.MainTableLayoutPanel.Size = new System.Drawing.Size(884, 524);
            this.MainTableLayoutPanel.TabIndex = 0;
            // 
            // InfoTabs
            // 
            this.InfoTabs.Controls.Add(this.InformationPage);
            this.InfoTabs.Controls.Add(this.IPAddressPage);
            this.InfoTabs.Controls.Add(this.PresetsPage);
            this.InfoTabs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.InfoTabs.Location = new System.Drawing.Point(3, 134);
            this.InfoTabs.Name = "InfoTabs";
            this.InfoTabs.SelectedIndex = 0;
            this.InfoTabs.Size = new System.Drawing.Size(878, 387);
            this.InfoTabs.TabIndex = 0;
            // 
            // InformationPage
            // 
            this.InformationPage.Controls.Add(this.flowLayoutPanel1);
            this.InformationPage.Location = new System.Drawing.Point(4, 22);
            this.InformationPage.Name = "InformationPage";
            this.InformationPage.Padding = new System.Windows.Forms.Padding(3);
            this.InformationPage.Size = new System.Drawing.Size(870, 361);
            this.InformationPage.TabIndex = 0;
            this.InformationPage.Text = "Information";
            this.InformationPage.UseVisualStyleBackColor = true;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.ConnectionDetailsGroup);
            this.flowLayoutPanel1.Controls.Add(this.ChangeMacAddressGroup);
            this.flowLayoutPanel1.Controls.Add(this.PerformanceCounterGroup);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(3, 3);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(864, 355);
            this.flowLayoutPanel1.TabIndex = 0;
            // 
            // ConnectionDetailsGroup
            // 
            this.ConnectionDetailsGroup.Controls.Add(this.ActiveMacVendorTextbox);
            this.ConnectionDetailsGroup.Controls.Add(this.ActiveMacValueTextbox);
            this.ConnectionDetailsGroup.Controls.Add(this.ActiveMacLabel);
            this.ConnectionDetailsGroup.Controls.Add(this.OriginalMacVendorTextbox);
            this.ConnectionDetailsGroup.Controls.Add(this.OriginalMacValueTextbox);
            this.ConnectionDetailsGroup.Controls.Add(this.OriginalMacLabel);
            this.ConnectionDetailsGroup.Controls.Add(this.Ipv6ValueTextbox);
            this.ConnectionDetailsGroup.Controls.Add(this.Ipv6Label);
            this.ConnectionDetailsGroup.Controls.Add(this.Ipv4ValueTextbox);
            this.ConnectionDetailsGroup.Controls.Add(this.Ipv4Label);
            this.ConnectionDetailsGroup.Controls.Add(this.ConfigIdValueTextbox);
            this.ConnectionDetailsGroup.Controls.Add(this.ConfigIdLabel);
            this.ConnectionDetailsGroup.Controls.Add(this.HardwareIdValueTextbox);
            this.ConnectionDetailsGroup.Controls.Add(this.HardwareIdLabel);
            this.ConnectionDetailsGroup.Controls.Add(this.DeviceValueTextbox);
            this.ConnectionDetailsGroup.Controls.Add(this.DeviceLabel);
            this.ConnectionDetailsGroup.Controls.Add(this.ConnectionValueTextbox);
            this.ConnectionDetailsGroup.Controls.Add(this.ConnectionLabel);
            this.ConnectionDetailsGroup.Location = new System.Drawing.Point(3, 3);
            this.ConnectionDetailsGroup.Name = "ConnectionDetailsGroup";
            this.ConnectionDetailsGroup.Size = new System.Drawing.Size(858, 135);
            this.ConnectionDetailsGroup.TabIndex = 0;
            this.ConnectionDetailsGroup.TabStop = false;
            this.ConnectionDetailsGroup.Text = "Connection Details";
            // 
            // ActiveMacVendorTextbox
            // 
            this.ActiveMacVendorTextbox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.ActiveMacVendorTextbox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.ActiveMacVendorTextbox.Location = new System.Drawing.Point(551, 107);
            this.ActiveMacVendorTextbox.Name = "ActiveMacVendorTextbox";
            this.ActiveMacVendorTextbox.ReadOnly = true;
            this.ActiveMacVendorTextbox.Size = new System.Drawing.Size(300, 13);
            this.ActiveMacVendorTextbox.TabIndex = 17;
            this.ActiveMacVendorTextbox.TabStop = false;
            this.ActiveMacVendorTextbox.Text = "...";
            // 
            // ActiveMacValueTextbox
            // 
            this.ActiveMacValueTextbox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.ActiveMacValueTextbox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.ActiveMacValueTextbox.Location = new System.Drawing.Point(421, 107);
            this.ActiveMacValueTextbox.Name = "ActiveMacValueTextbox";
            this.ActiveMacValueTextbox.ReadOnly = true;
            this.ActiveMacValueTextbox.Size = new System.Drawing.Size(124, 13);
            this.ActiveMacValueTextbox.TabIndex = 16;
            this.ActiveMacValueTextbox.TabStop = false;
            this.ActiveMacValueTextbox.Text = "...";
            // 
            // ActiveMacLabel
            // 
            this.ActiveMacLabel.AutoSize = true;
            this.ActiveMacLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ActiveMacLabel.Location = new System.Drawing.Point(418, 84);
            this.ActiveMacLabel.Name = "ActiveMacLabel";
            this.ActiveMacLabel.Size = new System.Drawing.Size(120, 13);
            this.ActiveMacLabel.TabIndex = 15;
            this.ActiveMacLabel.Text = "Active Mac Address";
            // 
            // OriginalMacVendorTextbox
            // 
            this.OriginalMacVendorTextbox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.OriginalMacVendorTextbox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.OriginalMacVendorTextbox.Location = new System.Drawing.Point(551, 41);
            this.OriginalMacVendorTextbox.Name = "OriginalMacVendorTextbox";
            this.OriginalMacVendorTextbox.ReadOnly = true;
            this.OriginalMacVendorTextbox.Size = new System.Drawing.Size(300, 13);
            this.OriginalMacVendorTextbox.TabIndex = 14;
            this.OriginalMacVendorTextbox.TabStop = false;
            this.OriginalMacVendorTextbox.Text = "...";
            // 
            // OriginalMacValueTextbox
            // 
            this.OriginalMacValueTextbox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.OriginalMacValueTextbox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.OriginalMacValueTextbox.Location = new System.Drawing.Point(421, 41);
            this.OriginalMacValueTextbox.Name = "OriginalMacValueTextbox";
            this.OriginalMacValueTextbox.ReadOnly = true;
            this.OriginalMacValueTextbox.Size = new System.Drawing.Size(124, 13);
            this.OriginalMacValueTextbox.TabIndex = 13;
            this.OriginalMacValueTextbox.TabStop = false;
            this.OriginalMacValueTextbox.Text = "...";
            // 
            // OriginalMacLabel
            // 
            this.OriginalMacLabel.AutoSize = true;
            this.OriginalMacLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.OriginalMacLabel.Location = new System.Drawing.Point(418, 16);
            this.OriginalMacLabel.Name = "OriginalMacLabel";
            this.OriginalMacLabel.Size = new System.Drawing.Size(127, 13);
            this.OriginalMacLabel.TabIndex = 12;
            this.OriginalMacLabel.Text = "Original Mac Address";
            // 
            // Ipv6ValueTextbox
            // 
            this.Ipv6ValueTextbox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.Ipv6ValueTextbox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.Ipv6ValueTextbox.Location = new System.Drawing.Point(260, 110);
            this.Ipv6ValueTextbox.Name = "Ipv6ValueTextbox";
            this.Ipv6ValueTextbox.ReadOnly = true;
            this.Ipv6ValueTextbox.Size = new System.Drawing.Size(60, 13);
            this.Ipv6ValueTextbox.TabIndex = 5;
            this.Ipv6ValueTextbox.TabStop = false;
            this.Ipv6ValueTextbox.Text = "...";
            // 
            // Ipv6Label
            // 
            this.Ipv6Label.AutoSize = true;
            this.Ipv6Label.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Ipv6Label.Location = new System.Drawing.Point(181, 110);
            this.Ipv6Label.Name = "Ipv6Label";
            this.Ipv6Label.Size = new System.Drawing.Size(63, 13);
            this.Ipv6Label.TabIndex = 10;
            this.Ipv6Label.Text = "TCP/IPv6";
            // 
            // Ipv4ValueTextbox
            // 
            this.Ipv4ValueTextbox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.Ipv4ValueTextbox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.Ipv4ValueTextbox.Location = new System.Drawing.Point(103, 110);
            this.Ipv4ValueTextbox.Name = "Ipv4ValueTextbox";
            this.Ipv4ValueTextbox.ReadOnly = true;
            this.Ipv4ValueTextbox.Size = new System.Drawing.Size(60, 13);
            this.Ipv4ValueTextbox.TabIndex = 5;
            this.Ipv4ValueTextbox.TabStop = false;
            this.Ipv4ValueTextbox.Text = "...";
            // 
            // Ipv4Label
            // 
            this.Ipv4Label.AutoSize = true;
            this.Ipv4Label.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Ipv4Label.Location = new System.Drawing.Point(24, 110);
            this.Ipv4Label.Name = "Ipv4Label";
            this.Ipv4Label.Size = new System.Drawing.Size(63, 13);
            this.Ipv4Label.TabIndex = 8;
            this.Ipv4Label.Text = "TCP/IPv4";
            // 
            // ConfigIdValueTextbox
            // 
            this.ConfigIdValueTextbox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.ConfigIdValueTextbox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.ConfigIdValueTextbox.Location = new System.Drawing.Point(103, 84);
            this.ConfigIdValueTextbox.Name = "ConfigIdValueTextbox";
            this.ConfigIdValueTextbox.ReadOnly = true;
            this.ConfigIdValueTextbox.Size = new System.Drawing.Size(300, 13);
            this.ConfigIdValueTextbox.TabIndex = 5;
            this.ConfigIdValueTextbox.TabStop = false;
            this.ConfigIdValueTextbox.Text = "...";
            // 
            // ConfigIdLabel
            // 
            this.ConfigIdLabel.AutoSize = true;
            this.ConfigIdLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ConfigIdLabel.Location = new System.Drawing.Point(24, 84);
            this.ConfigIdLabel.Name = "ConfigIdLabel";
            this.ConfigIdLabel.Size = new System.Drawing.Size(60, 13);
            this.ConfigIdLabel.TabIndex = 6;
            this.ConfigIdLabel.Text = "Config ID";
            // 
            // HardwareIdValueTextbox
            // 
            this.HardwareIdValueTextbox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.HardwareIdValueTextbox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.HardwareIdValueTextbox.Location = new System.Drawing.Point(103, 62);
            this.HardwareIdValueTextbox.Name = "HardwareIdValueTextbox";
            this.HardwareIdValueTextbox.ReadOnly = true;
            this.HardwareIdValueTextbox.Size = new System.Drawing.Size(300, 13);
            this.HardwareIdValueTextbox.TabIndex = 5;
            this.HardwareIdValueTextbox.TabStop = false;
            this.HardwareIdValueTextbox.Text = "...";
            // 
            // HardwareIdLabel
            // 
            this.HardwareIdLabel.AutoSize = true;
            this.HardwareIdLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.HardwareIdLabel.Location = new System.Drawing.Point(6, 62);
            this.HardwareIdLabel.Name = "HardwareIdLabel";
            this.HardwareIdLabel.Size = new System.Drawing.Size(78, 13);
            this.HardwareIdLabel.TabIndex = 4;
            this.HardwareIdLabel.Text = "Hardware ID";
            // 
            // DeviceValueTextbox
            // 
            this.DeviceValueTextbox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.DeviceValueTextbox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.DeviceValueTextbox.Location = new System.Drawing.Point(103, 41);
            this.DeviceValueTextbox.Name = "DeviceValueTextbox";
            this.DeviceValueTextbox.ReadOnly = true;
            this.DeviceValueTextbox.Size = new System.Drawing.Size(300, 13);
            this.DeviceValueTextbox.TabIndex = 18;
            this.DeviceValueTextbox.TabStop = false;
            this.DeviceValueTextbox.Text = "...";
            // 
            // DeviceLabel
            // 
            this.DeviceLabel.AutoSize = true;
            this.DeviceLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DeviceLabel.Location = new System.Drawing.Point(37, 41);
            this.DeviceLabel.Name = "DeviceLabel";
            this.DeviceLabel.Size = new System.Drawing.Size(47, 13);
            this.DeviceLabel.TabIndex = 2;
            this.DeviceLabel.Text = "Device";
            // 
            // ConnectionValueTextbox
            // 
            this.ConnectionValueTextbox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.ConnectionValueTextbox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.ConnectionValueTextbox.Location = new System.Drawing.Point(103, 20);
            this.ConnectionValueTextbox.Name = "ConnectionValueTextbox";
            this.ConnectionValueTextbox.ReadOnly = true;
            this.ConnectionValueTextbox.Size = new System.Drawing.Size(300, 13);
            this.ConnectionValueTextbox.TabIndex = 1;
            this.ConnectionValueTextbox.TabStop = false;
            this.ConnectionValueTextbox.Text = "...";
            // 
            // ConnectionLabel
            // 
            this.ConnectionLabel.AutoSize = true;
            this.ConnectionLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ConnectionLabel.Location = new System.Drawing.Point(13, 20);
            this.ConnectionLabel.Name = "ConnectionLabel";
            this.ConnectionLabel.Size = new System.Drawing.Size(71, 13);
            this.ConnectionLabel.TabIndex = 0;
            this.ConnectionLabel.Text = "Connection";
            // 
            // ChangeMacAddressGroup
            // 
            this.ChangeMacAddressGroup.Controls.Add(this.WikiLink);
            this.ChangeMacAddressGroup.Controls.Add(this.RestoreMacButton);
            this.ChangeMacAddressGroup.Controls.Add(this.ChangeMacButton);
            this.ChangeMacAddressGroup.Controls.Add(this.ZeroTwoCheckBox);
            this.ChangeMacAddressGroup.Controls.Add(this.PersistentAddressCheckBox);
            this.ChangeMacAddressGroup.Controls.Add(this.AutoStartCheckBox);
            this.ChangeMacAddressGroup.Controls.Add(this.VendorComboBox);
            this.ChangeMacAddressGroup.Controls.Add(this.RandomMacButton);
            this.ChangeMacAddressGroup.Controls.Add(this.macTextBox1);
            this.ChangeMacAddressGroup.Location = new System.Drawing.Point(3, 144);
            this.ChangeMacAddressGroup.Name = "ChangeMacAddressGroup";
            this.ChangeMacAddressGroup.Size = new System.Drawing.Size(400, 193);
            this.ChangeMacAddressGroup.TabIndex = 1;
            this.ChangeMacAddressGroup.TabStop = false;
            this.ChangeMacAddressGroup.Text = "Change Mac Address";
            // 
            // RestoreMacButton
            // 
            this.RestoreMacButton.Location = new System.Drawing.Point(210, 161);
            this.RestoreMacButton.Name = "RestoreMacButton";
            this.RestoreMacButton.Size = new System.Drawing.Size(184, 23);
            this.RestoreMacButton.TabIndex = 7;
            this.RestoreMacButton.Text = "Restore &Original";
            this.RestoreMacButton.UseVisualStyleBackColor = true;
            this.RestoreMacButton.Click += new System.EventHandler(this.RestoreMacButton_Click);
            // 
            // ChangeMacButton
            // 
            this.ChangeMacButton.Location = new System.Drawing.Point(6, 161);
            this.ChangeMacButton.Name = "ChangeMacButton";
            this.ChangeMacButton.Size = new System.Drawing.Size(184, 23);
            this.ChangeMacButton.TabIndex = 6;
            this.ChangeMacButton.Text = "&Change Now!";
            this.ChangeMacButton.UseVisualStyleBackColor = true;
            this.ChangeMacButton.Click += new System.EventHandler(this.ChangeMacButton_Click);
            // 
            // ZeroTwoCheckBox
            // 
            this.ZeroTwoCheckBox.Location = new System.Drawing.Point(6, 138);
            this.ZeroTwoCheckBox.Name = "ZeroTwoCheckBox";
            this.ZeroTwoCheckBox.Size = new System.Drawing.Size(388, 17);
            this.ZeroTwoCheckBox.TabIndex = 5;
            this.ZeroTwoCheckBox.Text = "Use \'02\' as first octet of MAC address";
            this.ZeroTwoCheckBox.UseVisualStyleBackColor = true;
            this.ZeroTwoCheckBox.CheckedChanged += new System.EventHandler(this.ZeroTwoCheckBox_CheckedChanged);
            // 
            // PersistentAddressCheckBox
            // 
            this.PersistentAddressCheckBox.Location = new System.Drawing.Point(6, 115);
            this.PersistentAddressCheckBox.Name = "PersistentAddressCheckBox";
            this.PersistentAddressCheckBox.Size = new System.Drawing.Size(388, 17);
            this.PersistentAddressCheckBox.TabIndex = 4;
            this.PersistentAddressCheckBox.Text = "Make new MAC address persistent";
            this.PersistentAddressCheckBox.UseVisualStyleBackColor = true;
            this.PersistentAddressCheckBox.CheckedChanged += new System.EventHandler(this.PersistentAddressCheckBox_CheckedChanged);
            // 
            // AutoStartCheckBox
            // 
            this.AutoStartCheckBox.Location = new System.Drawing.Point(7, 92);
            this.AutoStartCheckBox.Name = "AutoStartCheckBox";
            this.AutoStartCheckBox.Size = new System.Drawing.Size(387, 17);
            this.AutoStartCheckBox.TabIndex = 3;
            this.AutoStartCheckBox.Text = "Automatically restart network connection to apply changes";
            this.AutoStartCheckBox.UseVisualStyleBackColor = true;
            this.AutoStartCheckBox.CheckedChanged += new System.EventHandler(this.AutoStartCheckBox_CheckedChanged);
            // 
            // VendorComboBox
            // 
            this.VendorComboBox.FormattingEnabled = true;
            this.VendorComboBox.Location = new System.Drawing.Point(6, 58);
            this.VendorComboBox.Name = "VendorComboBox";
            this.VendorComboBox.Size = new System.Drawing.Size(388, 21);
            this.VendorComboBox.TabIndex = 2;
            // 
            // RandomMacButton
            // 
            this.RandomMacButton.Location = new System.Drawing.Point(184, 19);
            this.RandomMacButton.Name = "RandomMacButton";
            this.RandomMacButton.Size = new System.Drawing.Size(210, 24);
            this.RandomMacButton.TabIndex = 1;
            this.RandomMacButton.Text = "Random MAC Address";
            this.RandomMacButton.UseVisualStyleBackColor = true;
            this.RandomMacButton.Click += new System.EventHandler(this.RandomMacButton_Click);
            // 
            // macTextBox1
            // 
            this.macTextBox1.BackColor = System.Drawing.SystemColors.Window;
            this.macTextBox1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.macTextBox1.Location = new System.Drawing.Point(6, 19);
            this.macTextBox1.Name = "macTextBox1";
            this.macTextBox1.Size = new System.Drawing.Size(171, 24);
            this.macTextBox1.TabIndex = 0;
            // 
            // PerformanceCounterGroup
            // 
            this.PerformanceCounterGroup.Location = new System.Drawing.Point(409, 144);
            this.PerformanceCounterGroup.Name = "PerformanceCounterGroup";
            this.PerformanceCounterGroup.Size = new System.Drawing.Size(452, 193);
            this.PerformanceCounterGroup.TabIndex = 2;
            this.PerformanceCounterGroup.TabStop = false;
            // 
            // IPAddressPage
            // 
            this.IPAddressPage.Location = new System.Drawing.Point(4, 22);
            this.IPAddressPage.Name = "IPAddressPage";
            this.IPAddressPage.Padding = new System.Windows.Forms.Padding(3);
            this.IPAddressPage.Size = new System.Drawing.Size(870, 361);
            this.IPAddressPage.TabIndex = 1;
            this.IPAddressPage.Text = "IP Address";
            this.IPAddressPage.UseVisualStyleBackColor = true;
            // 
            // PresetsPage
            // 
            this.PresetsPage.Location = new System.Drawing.Point(4, 22);
            this.PresetsPage.Name = "PresetsPage";
            this.PresetsPage.Padding = new System.Windows.Forms.Padding(3);
            this.PresetsPage.Size = new System.Drawing.Size(870, 361);
            this.PresetsPage.TabIndex = 2;
            this.PresetsPage.Text = "Presets";
            this.PresetsPage.UseVisualStyleBackColor = true;
            // 
            // ConnectionsGrid
            // 
            this.ConnectionsGrid.Alignment = System.Windows.Forms.ListViewAlignment.SnapToGrid;
            this.ConnectionsGrid.CausesValidation = false;
            this.ConnectionsGrid.CellEditUseWholeCell = false;
            this.ConnectionsGrid.DataSource = null;
            this.ConnectionsGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ConnectionsGrid.FullRowSelect = true;
            this.ConnectionsGrid.HasCollapsibleGroups = false;
            this.ConnectionsGrid.HeaderUsesThemes = true;
            this.ConnectionsGrid.HideSelection = false;
            this.ConnectionsGrid.Location = new System.Drawing.Point(3, 3);
            this.ConnectionsGrid.MinimumSize = new System.Drawing.Size(800, 4);
            this.ConnectionsGrid.MultiSelect = false;
            this.ConnectionsGrid.Name = "ConnectionsGrid";
            this.ConnectionsGrid.RenderNonEditableCheckboxesAsDisabled = true;
            this.ConnectionsGrid.ShowGroups = false;
            this.ConnectionsGrid.ShowItemToolTips = true;
            this.ConnectionsGrid.Size = new System.Drawing.Size(878, 125);
            this.ConnectionsGrid.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.ConnectionsGrid.SpaceBetweenGroups = 1;
            this.ConnectionsGrid.TabIndex = 1;
            this.ConnectionsGrid.UseCompatibleStateImageBehavior = false;
            this.ConnectionsGrid.UseExplorerTheme = true;
            this.ConnectionsGrid.View = System.Windows.Forms.View.Details;
            this.ConnectionsGrid.SelectedIndexChanged += new System.EventHandler(this.ConnectionsGrid_SelectedIndexChanged);
            // 
            // Toolbar
            // 
            this.Toolbar.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.Toolbar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.FileMenu,
            this.ActionMenu,
            this.OptionsMenu,
            this.HelpMenu});
            this.Toolbar.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.Toolbar.Location = new System.Drawing.Point(0, 0);
            this.Toolbar.Name = "Toolbar";
            this.Toolbar.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.Toolbar.Size = new System.Drawing.Size(884, 24);
            this.Toolbar.TabIndex = 0;
            // 
            // FileMenu
            // 
            this.FileMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ExportReportItem,
            this.toolStripSeparator1,
            this.OpenPresetItem,
            this.SavePresetItem,
            this.SavePresetAsItem,
            this.toolStripSeparator2,
            this.ImportPresetItem,
            this.ExportPresetItem,
            this.AssociateItem,
            this.toolStripSeparator3,
            this.ExitItem});
            this.FileMenu.Name = "FileMenu";
            this.FileMenu.Size = new System.Drawing.Size(37, 20);
            this.FileMenu.Text = "&File";
            // 
            // ExportReportItem
            // 
            this.ExportReportItem.Name = "ExportReportItem";
            this.ExportReportItem.Size = new System.Drawing.Size(240, 22);
            this.ExportReportItem.Text = "Export Text Report";
            this.ExportReportItem.Click += new System.EventHandler(this.ExportReportItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(237, 6);
            // 
            // OpenPresetItem
            // 
            this.OpenPresetItem.Name = "OpenPresetItem";
            this.OpenPresetItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.OpenPresetItem.Size = new System.Drawing.Size(240, 22);
            this.OpenPresetItem.Text = "Open Preset";
            this.OpenPresetItem.Click += new System.EventHandler(this.OpenPresetItem_Click);
            // 
            // SavePresetItem
            // 
            this.SavePresetItem.Name = "SavePresetItem";
            this.SavePresetItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.SavePresetItem.Size = new System.Drawing.Size(240, 22);
            this.SavePresetItem.Text = "Save Preset";
            this.SavePresetItem.Click += new System.EventHandler(this.SavePresetItem_Click);
            // 
            // SavePresetAsItem
            // 
            this.SavePresetAsItem.Name = "SavePresetAsItem";
            this.SavePresetAsItem.Size = new System.Drawing.Size(240, 22);
            this.SavePresetAsItem.Text = "Save Preset As";
            this.SavePresetAsItem.Click += new System.EventHandler(this.SavePresetAsItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(237, 6);
            // 
            // ImportPresetItem
            // 
            this.ImportPresetItem.Name = "ImportPresetItem";
            this.ImportPresetItem.Size = new System.Drawing.Size(240, 22);
            this.ImportPresetItem.Text = "Import Preset";
            this.ImportPresetItem.Click += new System.EventHandler(this.ImportPresetItem_Click);
            // 
            // ExportPresetItem
            // 
            this.ExportPresetItem.Name = "ExportPresetItem";
            this.ExportPresetItem.Size = new System.Drawing.Size(240, 22);
            this.ExportPresetItem.Text = "Export Preset";
            this.ExportPresetItem.Click += new System.EventHandler(this.ExportPresetItem_Click);
            // 
            // AssociateItem
            // 
            this.AssociateItem.Name = "AssociateItem";
            this.AssociateItem.Size = new System.Drawing.Size(240, 22);
            this.AssociateItem.Text = "Associate with Preset Files (.tpf)";
            this.AssociateItem.Click += new System.EventHandler(this.AssociateItem_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(237, 6);
            // 
            // ExitItem
            // 
            this.ExitItem.Name = "ExitItem";
            this.ExitItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Q)));
            this.ExitItem.Size = new System.Drawing.Size(240, 22);
            this.ExitItem.Text = "Exit";
            this.ExitItem.Click += new System.EventHandler(this.ExitItem_Click);
            // 
            // ActionMenu
            // 
            this.ActionMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.RefreshItem,
            this.toolStripSeparator4,
            this.Dhcp4Item,
            this.Dhcp6Item,
            this.toolStripSeparator7,
            this.DeleteItem});
            this.ActionMenu.Name = "ActionMenu";
            this.ActionMenu.Size = new System.Drawing.Size(54, 20);
            this.ActionMenu.Text = "&Action";
            // 
            // RefreshItem
            // 
            this.RefreshItem.Name = "RefreshItem";
            this.RefreshItem.ShortcutKeys = System.Windows.Forms.Keys.F5;
            this.RefreshItem.Size = new System.Drawing.Size(274, 22);
            this.RefreshItem.Text = "Refresh";
            this.RefreshItem.Click += new System.EventHandler(this.RefreshItem_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(271, 6);
            // 
            // Dhcp4Item
            // 
            this.Dhcp4Item.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Dhcp4EnabledItem,
            this.Dhcp4ReleaseIpItem,
            this.Dhcp4RenewIpItem});
            this.Dhcp4Item.Name = "Dhcp4Item";
            this.Dhcp4Item.Size = new System.Drawing.Size(274, 22);
            this.Dhcp4Item.Text = "DHCPv4";
            // 
            // Dhcp4EnabledItem
            // 
            this.Dhcp4EnabledItem.Checked = true;
            this.Dhcp4EnabledItem.CheckOnClick = true;
            this.Dhcp4EnabledItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.Dhcp4EnabledItem.Name = "Dhcp4EnabledItem";
            this.Dhcp4EnabledItem.Size = new System.Drawing.Size(163, 22);
            this.Dhcp4EnabledItem.Text = "DHCPv4 Enabled";
            // 
            // Dhcp4ReleaseIpItem
            // 
            this.Dhcp4ReleaseIpItem.Enabled = false;
            this.Dhcp4ReleaseIpItem.Name = "Dhcp4ReleaseIpItem";
            this.Dhcp4ReleaseIpItem.Size = new System.Drawing.Size(163, 22);
            this.Dhcp4ReleaseIpItem.Text = "Release IP";
            // 
            // Dhcp4RenewIpItem
            // 
            this.Dhcp4RenewIpItem.Enabled = false;
            this.Dhcp4RenewIpItem.Name = "Dhcp4RenewIpItem";
            this.Dhcp4RenewIpItem.Size = new System.Drawing.Size(163, 22);
            this.Dhcp4RenewIpItem.Text = "Renew IP";
            // 
            // Dhcp6Item
            // 
            this.Dhcp6Item.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Dhcp6EnabledItem,
            this.Dhcp6ReleaseIpItem,
            this.Dhcp6RenewItem});
            this.Dhcp6Item.Name = "Dhcp6Item";
            this.Dhcp6Item.Size = new System.Drawing.Size(274, 22);
            this.Dhcp6Item.Text = "DHCPv6";
            // 
            // Dhcp6EnabledItem
            // 
            this.Dhcp6EnabledItem.Checked = true;
            this.Dhcp6EnabledItem.CheckOnClick = true;
            this.Dhcp6EnabledItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.Dhcp6EnabledItem.Name = "Dhcp6EnabledItem";
            this.Dhcp6EnabledItem.Size = new System.Drawing.Size(163, 22);
            this.Dhcp6EnabledItem.Text = "DHCPv6 Enabled";
            // 
            // Dhcp6ReleaseIpItem
            // 
            this.Dhcp6ReleaseIpItem.Enabled = false;
            this.Dhcp6ReleaseIpItem.Name = "Dhcp6ReleaseIpItem";
            this.Dhcp6ReleaseIpItem.Size = new System.Drawing.Size(163, 22);
            this.Dhcp6ReleaseIpItem.Text = "Release IP";
            // 
            // Dhcp6RenewItem
            // 
            this.Dhcp6RenewItem.Enabled = false;
            this.Dhcp6RenewItem.Name = "Dhcp6RenewItem";
            this.Dhcp6RenewItem.Size = new System.Drawing.Size(163, 22);
            this.Dhcp6RenewItem.Text = "Renew IP";
            // 
            // toolStripSeparator7
            // 
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            this.toolStripSeparator7.Size = new System.Drawing.Size(271, 6);
            // 
            // DeleteItem
            // 
            this.DeleteItem.Name = "DeleteItem";
            this.DeleteItem.Size = new System.Drawing.Size(274, 22);
            this.DeleteItem.Text = "Delete Network Adapter from Registry";
            this.DeleteItem.Click += new System.EventHandler(this.DeleteItem_Click);
            // 
            // OptionsMenu
            // 
            this.OptionsMenu.Name = "OptionsMenu";
            this.OptionsMenu.Size = new System.Drawing.Size(61, 20);
            this.OptionsMenu.Text = "&Options";
            this.OptionsMenu.Click += new System.EventHandler(this.OptionsMenu_Click);
            // 
            // HelpMenu
            // 
            this.HelpMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.HelpTopicsItem,
            this.CliParamsHelpItem,
            this.toolStripSeparator6,
            this.CheckUpdateItem,
            this.UpdateOuiItem,
            this.toolStripSeparator5,
            this.AboutItem});
            this.HelpMenu.Name = "HelpMenu";
            this.HelpMenu.Size = new System.Drawing.Size(44, 20);
            this.HelpMenu.Text = "&Help";
            // 
            // HelpTopicsItem
            // 
            this.HelpTopicsItem.Name = "HelpTopicsItem";
            this.HelpTopicsItem.ShortcutKeys = System.Windows.Forms.Keys.F1;
            this.HelpTopicsItem.Size = new System.Drawing.Size(262, 22);
            this.HelpTopicsItem.Text = "Help Topics";
            this.HelpTopicsItem.Click += new System.EventHandler(this.HelpTopicsItem_Click);
            // 
            // CliParamsHelpItem
            // 
            this.CliParamsHelpItem.Name = "CliParamsHelpItem";
            this.CliParamsHelpItem.Size = new System.Drawing.Size(262, 22);
            this.CliParamsHelpItem.Text = "Command Line Parameters Help";
            this.CliParamsHelpItem.Click += new System.EventHandler(this.CliParamsHelpItem_Click);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(259, 6);
            // 
            // CheckUpdateItem
            // 
            this.CheckUpdateItem.Name = "CheckUpdateItem";
            this.CheckUpdateItem.Size = new System.Drawing.Size(262, 22);
            this.CheckUpdateItem.Text = "Check For Software Updates";
            this.CheckUpdateItem.Click += new System.EventHandler(this.CheckUpdateItem_Click);
            // 
            // UpdateOuiItem
            // 
            this.UpdateOuiItem.Name = "UpdateOuiItem";
            this.UpdateOuiItem.Size = new System.Drawing.Size(262, 22);
            this.UpdateOuiItem.Text = "Update Vendors List (OUI) from IEEE";
            this.UpdateOuiItem.Click += new System.EventHandler(this.UpdateOuiItem_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(259, 6);
            // 
            // AboutItem
            // 
            this.AboutItem.Name = "AboutItem";
            this.AboutItem.Size = new System.Drawing.Size(262, 22);
            this.AboutItem.Text = "About MacChanger";
            this.AboutItem.Click += new System.EventHandler(this.AboutItem_Click);
            // 
            // MainStatusBar
            // 
            this.MainStatusBar.Location = new System.Drawing.Point(0, 526);
            this.MainStatusBar.Name = "MainStatusBar";
            this.MainStatusBar.Size = new System.Drawing.Size(884, 22);
            this.MainStatusBar.TabIndex = 1;
            this.MainStatusBar.Text = "Ready";
            // 
            // WikiLink
            // 
            this.WikiLink.Location = new System.Drawing.Point(207, 139);
            this.WikiLink.Name = "WikiLink";
            this.WikiLink.Size = new System.Drawing.Size(67, 15);
            this.WikiLink.TabIndex = 8;
            this.WikiLink.TabStop = true;
            this.WikiLink.Text = "Why?";
            this.WikiLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.WikiLink_LinkClicked);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(884, 548);
            this.Controls.Add(this.MainStatusBar);
            this.Controls.Add(this.MainTableLayoutPanel);
            this.Controls.Add(this.Toolbar);
            this.MainMenuStrip = this.Toolbar;
            this.Name = "MainForm";
            this.Text = "MacChanger GUI";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.Resize += new System.EventHandler(this.MainForm_Resize);
            this.MainTableLayoutPanel.ResumeLayout(false);
            this.InfoTabs.ResumeLayout(false);
            this.InformationPage.ResumeLayout(false);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.ConnectionDetailsGroup.ResumeLayout(false);
            this.ConnectionDetailsGroup.PerformLayout();
            this.ChangeMacAddressGroup.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ConnectionsGrid)).EndInit();
            this.Toolbar.ResumeLayout(false);
            this.Toolbar.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private TableLayoutPanel MainTableLayoutPanel;
        private TabControl InfoTabs;
        private TabPage InformationPage;
        private TabPage IPAddressPage;
        private TabPage PresetsPage;
        private DataListView ConnectionsGrid;
        private MenuStrip Toolbar;
        private ToolStripMenuItem FileMenu;
        private ToolStripMenuItem ExportReportItem;
        private ToolStripMenuItem OpenPresetItem;
        private ToolStripMenuItem SavePresetItem;
        private ToolStripMenuItem SavePresetAsItem;
        private ToolStripMenuItem ImportPresetItem;
        private ToolStripMenuItem ExportPresetItem;
        private ToolStripMenuItem ActionMenu;
        private ToolStripMenuItem OptionsMenu;
        private ToolStripMenuItem HelpMenu;
        private ToolStripMenuItem AssociateItem;
        private ToolStripMenuItem ExitItem;
        private ToolStripMenuItem RefreshItem;
        private ToolStripMenuItem Dhcp4Item;
        private ToolStripMenuItem Dhcp6Item;
        private ToolStripMenuItem DeleteItem;
        private ToolStripMenuItem Dhcp4EnabledItem;
        private ToolStripMenuItem Dhcp4ReleaseIpItem;
        private ToolStripMenuItem Dhcp4RenewIpItem;
        private ToolStripMenuItem Dhcp6EnabledItem;
        private ToolStripMenuItem Dhcp6ReleaseIpItem;
        private ToolStripMenuItem Dhcp6RenewItem;
        private FlowLayoutPanel flowLayoutPanel1;
        private GroupBox ConnectionDetailsGroup;
        private Label ConnectionLabel;
        private GroupBox ChangeMacAddressGroup;
        private GroupBox PerformanceCounterGroup;
        private TextBox ConnectionValueTextbox;
        private Label DeviceLabel;
        private TextBox DeviceValueTextbox;
        private StatusBar MainStatusBar;
        private ToolStripMenuItem HelpTopicsItem;
        private ToolStripMenuItem CliParamsHelpItem;
        private ToolStripMenuItem CheckUpdateItem;
        private ToolStripMenuItem UpdateOuiItem;
        private ToolStripMenuItem AboutItem;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripSeparator toolStripSeparator2;
        private ToolStripSeparator toolStripSeparator3;
        private ToolStripSeparator toolStripSeparator4;
        private ToolStripSeparator toolStripSeparator6;
        private ToolStripSeparator toolStripSeparator5;
        private ToolStripSeparator toolStripSeparator7;
        private Label HardwareIdLabel;
        private TextBox ConfigIdValueTextbox;
        private Label ConfigIdLabel;
        private TextBox HardwareIdValueTextbox;
        private TextBox Ipv4ValueTextbox;
        private Label Ipv4Label;
        private TextBox Ipv6ValueTextbox;
        private Label Ipv6Label;
        private TextBox OriginalMacVendorTextbox;
        private TextBox OriginalMacValueTextbox;
        private Label OriginalMacLabel;
        private TextBox ActiveMacVendorTextbox;
        private TextBox ActiveMacValueTextbox;
        private Label ActiveMacLabel;
        private Button RestoreMacButton;
        private Button ChangeMacButton;
        private CheckBox ZeroTwoCheckBox;
        private CheckBox PersistentAddressCheckBox;
        private CheckBox AutoStartCheckBox;
        private ComboBox VendorComboBox;
        private Button RandomMacButton;
        private Controls.MacTextBox macTextBox1;
        private LinkLabel WikiLink;
    }
}

