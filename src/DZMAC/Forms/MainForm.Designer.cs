#nullable enable

using System.Windows.Forms;
using BrightIdeasSoftware;

namespace Dzmac.Forms
{
    internal partial class MainForm
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
            this.MainTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this._loadingPanel = new System.Windows.Forms.Panel();
            this.LoadingLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this._loadingProgressBar = new System.Windows.Forms.ProgressBar();
            this.InfoTabs = new System.Windows.Forms.TabControl();
            this.InformationPage = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.ConnectionDetailsGroup = new System.Windows.Forms.GroupBox();
            this.ActiveMacVendorTextbox = new System.Windows.Forms.Label();
            this.ActiveMacValueTextbox = new System.Windows.Forms.Label();
            this.ActiveMacLabel = new System.Windows.Forms.Label();
            this.OriginalMacVendorTextbox = new System.Windows.Forms.Label();
            this.OriginalMacValueTextbox = new System.Windows.Forms.Label();
            this.OriginalMacLabel = new System.Windows.Forms.Label();
            this.Ipv6ValueTextbox = new System.Windows.Forms.Label();
            this.Ipv6Label = new System.Windows.Forms.Label();
            this.Ipv4ValueTextbox = new System.Windows.Forms.Label();
            this.Ipv4Label = new System.Windows.Forms.Label();
            this.ConfigIdValueTextbox = new System.Windows.Forms.Label();
            this.ConfigIdLabel = new System.Windows.Forms.Label();
            this.HardwareIdValueTextbox = new System.Windows.Forms.Label();
            this.HardwareIdLabel = new System.Windows.Forms.Label();
            this.DeviceValueTextbox = new System.Windows.Forms.Label();
            this.DeviceLabel = new System.Windows.Forms.Label();
            this.ConnectionValueTextbox = new System.Windows.Forms.Label();
            this.ConnectionLabel = new System.Windows.Forms.Label();
            this.PerformanceCounterGroup = new System.Windows.Forms.GroupBox();
            this.PerformancePanel = new System.Windows.Forms.TableLayoutPanel();
            this._performanceSentSpeedLabel = new System.Windows.Forms.Label();
            this._performanceSentLabel = new System.Windows.Forms.Label();
            this._performanceReceivedSpeedLabel = new System.Windows.Forms.Label();
            this._performanceReceivedLabel = new System.Windows.Forms.Label();
            this._performanceGraphPanel = new System.Windows.Forms.Panel();
            this.ChangeMacAddressGroup = new System.Windows.Forms.GroupBox();
            this.WikiLink = new System.Windows.Forms.LinkLabel();
            this.RestoreMacButton = new System.Windows.Forms.Button();
            this.ChangeMacButton = new System.Windows.Forms.Button();
            this.ZeroTwoCheckBox = new System.Windows.Forms.CheckBox();
            this.PersistentAddressCheckBox = new System.Windows.Forms.CheckBox();
            this.AutoStartCheckBox = new System.Windows.Forms.CheckBox();
            this.VendorComboBox = new System.Windows.Forms.ComboBox();
            this.RandomMacButton = new System.Windows.Forms.Button();
            this.macTextBox = new Dzmac.Controls.MacTextBox();
            this.IPAddressPage = new System.Windows.Forms.TabPage();
            this.IpAddressLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.Ipv4ColumnLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.Ipv4AddressGroupBox = new System.Windows.Forms.GroupBox();
            this.Ipv4AddressListView = new System.Windows.Forms.ListView();
            this.Ipv4AddressColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Ipv4SubnetMaskColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Ipv4GatewayGroupBox = new System.Windows.Forms.GroupBox();
            this.Ipv4GatewayListView = new System.Windows.Forms.ListView();
            this.Ipv4GatewayColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Ipv4DnsGroupBox = new System.Windows.Forms.GroupBox();
            this.Ipv4DnsListView = new System.Windows.Forms.ListView();
            this.Ipv4DnsColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Ipv6ColumnLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.Ipv6AddressGroupBox = new System.Windows.Forms.GroupBox();
            this.Ipv6AddressListView = new System.Windows.Forms.ListView();
            this.Ipv6AddressColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Ipv6PrefixColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Ipv6GatewayGroupBox = new System.Windows.Forms.GroupBox();
            this.Ipv6GatewayListView = new System.Windows.Forms.ListView();
            this.Ipv6GatewayColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Ipv6DnsGroupBox = new System.Windows.Forms.GroupBox();
            this.Ipv6DnsListView = new System.Windows.Forms.ListView();
            this.Ipv6DnsColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.PresetsPage = new System.Windows.Forms.TabPage();
            this.PresetRootLayout = new System.Windows.Forms.TableLayoutPanel();
            this._presetListBox = new System.Windows.Forms.ListBox();
            this._presetPropertyListView = new System.Windows.Forms.ListView();
            this.PresetPropertyColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.PresetValueColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.PresetButtonFlowPanel = new System.Windows.Forms.FlowLayoutPanel();
            this._presetNewButton = new System.Windows.Forms.Button();
            this._presetEditButton = new System.Windows.Forms.Button();
            this._presetDeleteButton = new System.Windows.Forms.Button();
            this._presetApplyButton = new System.Windows.Forms.Button();
            this.ConnectionsGrid = new BrightIdeasSoftware.DataListView();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
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
            this.toolStripSeparator9 = new System.Windows.Forms.ToolStripSeparator();
            this.AssociateItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.ExitItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ActionMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.RefreshItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.ToggleAdapterEnabledItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
            this.Dhcp4Item = new System.Windows.Forms.ToolStripMenuItem();
            this.DhcpEnabledItem = new System.Windows.Forms.ToolStripMenuItem();
            this.DhcpReleaseIpItem = new System.Windows.Forms.ToolStripMenuItem();
            this.DhcpRenewIpItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.DeleteItem = new System.Windows.Forms.ToolStripMenuItem();
            this.OptionsMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.NetworkConnectionsItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ShowSpeedInKBytesPerSecItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ShowAllAdaptersItem = new System.Windows.Forms.ToolStripMenuItem();
            this.HelpMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.HelpTopicsItem = new System.Windows.Forms.ToolStripMenuItem();
            this.CliParamsHelpItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.CheckUpdateItem = new System.Windows.Forms.ToolStripMenuItem();
            this.UpdateOuiItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.AboutItem = new System.Windows.Forms.ToolStripMenuItem();
            this.MainStatusBar = new System.Windows.Forms.StatusBar();
            this._loadingLabel = new System.Windows.Forms.Label();
            this.MainTableLayoutPanel.SuspendLayout();
            this._loadingPanel.SuspendLayout();
            this.LoadingLayoutPanel.SuspendLayout();
            this.InfoTabs.SuspendLayout();
            this.InformationPage.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.ConnectionDetailsGroup.SuspendLayout();
            this.PerformanceCounterGroup.SuspendLayout();
            this.PerformancePanel.SuspendLayout();
            this.ChangeMacAddressGroup.SuspendLayout();
            this.IPAddressPage.SuspendLayout();
            this.IpAddressLayoutPanel.SuspendLayout();
            this.Ipv4ColumnLayoutPanel.SuspendLayout();
            this.Ipv4AddressGroupBox.SuspendLayout();
            this.Ipv4GatewayGroupBox.SuspendLayout();
            this.Ipv4DnsGroupBox.SuspendLayout();
            this.Ipv6ColumnLayoutPanel.SuspendLayout();
            this.Ipv6AddressGroupBox.SuspendLayout();
            this.Ipv6GatewayGroupBox.SuspendLayout();
            this.Ipv6DnsGroupBox.SuspendLayout();
            this.PresetsPage.SuspendLayout();
            this.PresetRootLayout.SuspendLayout();
            this.PresetButtonFlowPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ConnectionsGrid)).BeginInit();
            this.Toolbar.SuspendLayout();
            this.SuspendLayout();
            // 
            // MainTableLayoutPanel
            // 
            this.MainTableLayoutPanel.CausesValidation = false;
            this.MainTableLayoutPanel.ColumnCount = 1;
            this.MainTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.MainTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.MainTableLayoutPanel.Controls.Add(this._loadingPanel, 0, 0);
            this.MainTableLayoutPanel.Controls.Add(this.InfoTabs, 0, 1);
            this.MainTableLayoutPanel.Controls.Add(this.ConnectionsGrid, 0, 0);
            this.MainTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainTableLayoutPanel.Location = new System.Drawing.Point(0, 24);
            this.MainTableLayoutPanel.Name = "MainTableLayoutPanel";
            this.MainTableLayoutPanel.RowCount = 2;
            this.MainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.MainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 75F));
            this.MainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.MainTableLayoutPanel.Size = new System.Drawing.Size(962, 528);
            this.MainTableLayoutPanel.TabIndex = 0;
            // 
            // _loadingPanel
            // 
            this._loadingPanel.BackColor = System.Drawing.SystemColors.Control;
            this._loadingPanel.Controls.Add(this.LoadingLayoutPanel);
            this._loadingPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._loadingPanel.Location = new System.Drawing.Point(0, 127);
            this._loadingPanel.Margin = new System.Windows.Forms.Padding(0);
            this._loadingPanel.Name = "_loadingPanel";
            this._loadingPanel.Size = new System.Drawing.Size(962, 381);
            this._loadingPanel.TabIndex = 2;
            this._loadingPanel.Visible = false;
            // 
            // LoadingLayoutPanel
            // 
            this.LoadingLayoutPanel.ColumnCount = 1;
            this.LoadingLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.LoadingLayoutPanel.Controls.Add(this._loadingProgressBar, 0, 0);
            this.LoadingLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LoadingLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.LoadingLayoutPanel.Name = "LoadingLayoutPanel";
            this.LoadingLayoutPanel.RowCount = 1;
            this.LoadingLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.LoadingLayoutPanel.Size = new System.Drawing.Size(962, 381);
            this.LoadingLayoutPanel.TabIndex = 0;
            // 
            // _loadingProgressBar
            // 
            this._loadingProgressBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._loadingProgressBar.Location = new System.Drawing.Point(0, 0);
            this._loadingProgressBar.Margin = new System.Windows.Forms.Padding(0);
            this._loadingProgressBar.MarqueeAnimationSpeed = 20;
            this._loadingProgressBar.Name = "_loadingProgressBar";
            this._loadingProgressBar.Size = new System.Drawing.Size(962, 6);
            this._loadingProgressBar.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this._loadingProgressBar.TabIndex = 1;
            // 
            // InfoTabs
            // 
            this.InfoTabs.Controls.Add(this.InformationPage);
            this.InfoTabs.Controls.Add(this.IPAddressPage);
            this.InfoTabs.Controls.Add(this.PresetsPage);
            this.InfoTabs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.InfoTabs.Location = new System.Drawing.Point(3, 511);
            this.InfoTabs.Name = "InfoTabs";
            this.InfoTabs.SelectedIndex = 0;
            this.InfoTabs.Size = new System.Drawing.Size(956, 14);
            this.InfoTabs.TabIndex = 0;
            // 
            // InformationPage
            // 
            this.InformationPage.Controls.Add(this.tableLayoutPanel1);
            this.InformationPage.Location = new System.Drawing.Point(4, 22);
            this.InformationPage.Name = "InformationPage";
            this.InformationPage.Padding = new System.Windows.Forms.Padding(3);
            this.InformationPage.Size = new System.Drawing.Size(948, 0);
            this.InformationPage.TabIndex = 0;
            this.InformationPage.Text = "Information";
            this.InformationPage.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.ConnectionDetailsGroup, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.PerformanceCounterGroup, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.ChangeMacAddressGroup, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 41.89944F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 58.10056F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(942, 0);
            this.tableLayoutPanel1.TabIndex = 3;
            // 
            // ConnectionDetailsGroup
            // 
            this.ConnectionDetailsGroup.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.ConnectionDetailsGroup, 2);
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
            this.ConnectionDetailsGroup.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ConnectionDetailsGroup.Location = new System.Drawing.Point(3, 3);
            this.ConnectionDetailsGroup.Name = "ConnectionDetailsGroup";
            this.ConnectionDetailsGroup.Size = new System.Drawing.Size(936, 1);
            this.ConnectionDetailsGroup.TabIndex = 0;
            this.ConnectionDetailsGroup.TabStop = false;
            this.ConnectionDetailsGroup.Text = "Connection Details";
            // 
            // ActiveMacVendorTextbox
            // 
            this.ActiveMacVendorTextbox.AutoEllipsis = true;
            this.ActiveMacVendorTextbox.Location = new System.Drawing.Point(580, 107);
            this.ActiveMacVendorTextbox.Name = "ActiveMacVendorTextbox";
            this.ActiveMacVendorTextbox.Size = new System.Drawing.Size(350, 13);
            this.ActiveMacVendorTextbox.TabIndex = 17;
            this.ActiveMacVendorTextbox.Text = "...";
            // 
            // ActiveMacValueTextbox
            // 
            this.ActiveMacValueTextbox.AutoEllipsis = true;
            this.ActiveMacValueTextbox.Location = new System.Drawing.Point(474, 107);
            this.ActiveMacValueTextbox.Name = "ActiveMacValueTextbox";
            this.ActiveMacValueTextbox.Size = new System.Drawing.Size(100, 13);
            this.ActiveMacValueTextbox.TabIndex = 16;
            this.ActiveMacValueTextbox.Text = "...";
            // 
            // ActiveMacLabel
            // 
            this.ActiveMacLabel.AutoSize = true;
            this.ActiveMacLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ActiveMacLabel.Location = new System.Drawing.Point(471, 84);
            this.ActiveMacLabel.Name = "ActiveMacLabel";
            this.ActiveMacLabel.Size = new System.Drawing.Size(120, 13);
            this.ActiveMacLabel.TabIndex = 15;
            this.ActiveMacLabel.Text = "Active Mac Address";
            // 
            // OriginalMacVendorTextbox
            // 
            this.OriginalMacVendorTextbox.AutoEllipsis = true;
            this.OriginalMacVendorTextbox.Location = new System.Drawing.Point(580, 41);
            this.OriginalMacVendorTextbox.Name = "OriginalMacVendorTextbox";
            this.OriginalMacVendorTextbox.Size = new System.Drawing.Size(350, 13);
            this.OriginalMacVendorTextbox.TabIndex = 14;
            this.OriginalMacVendorTextbox.Text = "...";
            // 
            // OriginalMacValueTextbox
            // 
            this.OriginalMacValueTextbox.AutoEllipsis = true;
            this.OriginalMacValueTextbox.Location = new System.Drawing.Point(474, 41);
            this.OriginalMacValueTextbox.Name = "OriginalMacValueTextbox";
            this.OriginalMacValueTextbox.Size = new System.Drawing.Size(100, 13);
            this.OriginalMacValueTextbox.TabIndex = 13;
            this.OriginalMacValueTextbox.Text = "...";
            // 
            // OriginalMacLabel
            // 
            this.OriginalMacLabel.AutoSize = true;
            this.OriginalMacLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.OriginalMacLabel.Location = new System.Drawing.Point(471, 16);
            this.OriginalMacLabel.Name = "OriginalMacLabel";
            this.OriginalMacLabel.Size = new System.Drawing.Size(127, 13);
            this.OriginalMacLabel.TabIndex = 12;
            this.OriginalMacLabel.Text = "Original Mac Address";
            // 
            // Ipv6ValueTextbox
            // 
            this.Ipv6ValueTextbox.AutoEllipsis = true;
            this.Ipv6ValueTextbox.Location = new System.Drawing.Point(303, 110);
            this.Ipv6ValueTextbox.Name = "Ipv6ValueTextbox";
            this.Ipv6ValueTextbox.Size = new System.Drawing.Size(100, 14);
            this.Ipv6ValueTextbox.TabIndex = 5;
            this.Ipv6ValueTextbox.Text = "...";
            // 
            // Ipv6Label
            // 
            this.Ipv6Label.AutoSize = true;
            this.Ipv6Label.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Ipv6Label.Location = new System.Drawing.Point(221, 110);
            this.Ipv6Label.Name = "Ipv6Label";
            this.Ipv6Label.Size = new System.Drawing.Size(63, 13);
            this.Ipv6Label.TabIndex = 10;
            this.Ipv6Label.Text = "TCP/IPv6";
            // 
            // Ipv4ValueTextbox
            // 
            this.Ipv4ValueTextbox.AutoEllipsis = true;
            this.Ipv4ValueTextbox.Location = new System.Drawing.Point(103, 110);
            this.Ipv4ValueTextbox.Name = "Ipv4ValueTextbox";
            this.Ipv4ValueTextbox.Size = new System.Drawing.Size(100, 14);
            this.Ipv4ValueTextbox.TabIndex = 5;
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
            this.ConfigIdValueTextbox.AutoEllipsis = true;
            this.ConfigIdValueTextbox.Location = new System.Drawing.Point(103, 84);
            this.ConfigIdValueTextbox.Name = "ConfigIdValueTextbox";
            this.ConfigIdValueTextbox.Size = new System.Drawing.Size(300, 14);
            this.ConfigIdValueTextbox.TabIndex = 5;
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
            this.HardwareIdValueTextbox.AutoEllipsis = true;
            this.HardwareIdValueTextbox.Location = new System.Drawing.Point(103, 62);
            this.HardwareIdValueTextbox.Name = "HardwareIdValueTextbox";
            this.HardwareIdValueTextbox.Size = new System.Drawing.Size(300, 14);
            this.HardwareIdValueTextbox.TabIndex = 5;
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
            this.DeviceValueTextbox.AutoEllipsis = true;
            this.DeviceValueTextbox.Location = new System.Drawing.Point(103, 41);
            this.DeviceValueTextbox.Name = "DeviceValueTextbox";
            this.DeviceValueTextbox.Size = new System.Drawing.Size(300, 14);
            this.DeviceValueTextbox.TabIndex = 18;
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
            this.ConnectionValueTextbox.AutoEllipsis = true;
            this.ConnectionValueTextbox.Location = new System.Drawing.Point(103, 20);
            this.ConnectionValueTextbox.Name = "ConnectionValueTextbox";
            this.ConnectionValueTextbox.Size = new System.Drawing.Size(300, 14);
            this.ConnectionValueTextbox.TabIndex = 1;
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
            // PerformanceCounterGroup
            // 
            this.PerformanceCounterGroup.Controls.Add(this.PerformancePanel);
            this.PerformanceCounterGroup.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PerformanceCounterGroup.Location = new System.Drawing.Point(474, 3);
            this.PerformanceCounterGroup.Name = "PerformanceCounterGroup";
            this.PerformanceCounterGroup.Size = new System.Drawing.Size(465, 1);
            this.PerformanceCounterGroup.TabIndex = 2;
            this.PerformanceCounterGroup.TabStop = false;
            this.PerformanceCounterGroup.Text = " ";
            // 
            // PerformancePanel
            // 
            this.PerformancePanel.BackColor = System.Drawing.Color.White;
            this.PerformancePanel.ColumnCount = 1;
            this.PerformancePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.PerformancePanel.Controls.Add(this._performanceSentSpeedLabel, 0, 4);
            this.PerformancePanel.Controls.Add(this._performanceSentLabel, 0, 3);
            this.PerformancePanel.Controls.Add(this._performanceReceivedSpeedLabel, 0, 2);
            this.PerformancePanel.Controls.Add(this._performanceReceivedLabel, 0, 1);
            this.PerformancePanel.Controls.Add(this._performanceGraphPanel, 0, 0);
            this.PerformancePanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PerformancePanel.Location = new System.Drawing.Point(3, 16);
            this.PerformancePanel.Name = "PerformancePanel";
            this.PerformancePanel.Padding = new System.Windows.Forms.Padding(6);
            this.PerformancePanel.RowCount = 5;
            this.PerformancePanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 78F));
            this.PerformancePanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.PerformancePanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.PerformancePanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.PerformancePanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.PerformancePanel.Size = new System.Drawing.Size(459, 0);
            this.PerformancePanel.TabIndex = 0;
            // 
            // _performanceSentSpeedLabel
            // 
            this._performanceSentSpeedLabel.AutoSize = true;
            this._performanceSentSpeedLabel.BackColor = System.Drawing.Color.White;
            this._performanceSentSpeedLabel.Dock = System.Windows.Forms.DockStyle.Top;
            this._performanceSentSpeedLabel.Font = new System.Drawing.Font("Consolas", 8.25F);
            this._performanceSentSpeedLabel.ForeColor = System.Drawing.Color.Green;
            this._performanceSentSpeedLabel.Location = new System.Drawing.Point(9, -6);
            this._performanceSentSpeedLabel.Name = "_performanceSentSpeedLabel";
            this._performanceSentSpeedLabel.Size = new System.Drawing.Size(441, 13);
            this._performanceSentSpeedLabel.TabIndex = 4;
            this._performanceSentSpeedLabel.Text = "-Speed  : 0 bps (Peak 0 bps)";
            // 
            // _performanceSentLabel
            // 
            this._performanceSentLabel.AutoSize = true;
            this._performanceSentLabel.BackColor = System.Drawing.Color.White;
            this._performanceSentLabel.Dock = System.Windows.Forms.DockStyle.Top;
            this._performanceSentLabel.Font = new System.Drawing.Font("Consolas", 8.25F);
            this._performanceSentLabel.ForeColor = System.Drawing.Color.Green;
            this._performanceSentLabel.Location = new System.Drawing.Point(9, -19);
            this._performanceSentLabel.Name = "_performanceSentLabel";
            this._performanceSentLabel.Size = new System.Drawing.Size(441, 13);
            this._performanceSentLabel.TabIndex = 3;
            this._performanceSentLabel.Text = "Sent    : 0 B";
            // 
            // _performanceReceivedSpeedLabel
            // 
            this._performanceReceivedSpeedLabel.AutoSize = true;
            this._performanceReceivedSpeedLabel.BackColor = System.Drawing.Color.White;
            this._performanceReceivedSpeedLabel.Dock = System.Windows.Forms.DockStyle.Top;
            this._performanceReceivedSpeedLabel.Font = new System.Drawing.Font("Consolas", 8.25F);
            this._performanceReceivedSpeedLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this._performanceReceivedSpeedLabel.Location = new System.Drawing.Point(9, -32);
            this._performanceReceivedSpeedLabel.Name = "_performanceReceivedSpeedLabel";
            this._performanceReceivedSpeedLabel.Size = new System.Drawing.Size(441, 13);
            this._performanceReceivedSpeedLabel.TabIndex = 2;
            this._performanceReceivedSpeedLabel.Text = "-Speed  : 0 bps (Peak 0 bps)";
            // 
            // _performanceReceivedLabel
            // 
            this._performanceReceivedLabel.AutoSize = true;
            this._performanceReceivedLabel.BackColor = System.Drawing.Color.White;
            this._performanceReceivedLabel.Dock = System.Windows.Forms.DockStyle.Top;
            this._performanceReceivedLabel.Font = new System.Drawing.Font("Consolas", 8.25F);
            this._performanceReceivedLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this._performanceReceivedLabel.Location = new System.Drawing.Point(9, -45);
            this._performanceReceivedLabel.Name = "_performanceReceivedLabel";
            this._performanceReceivedLabel.Size = new System.Drawing.Size(441, 13);
            this._performanceReceivedLabel.TabIndex = 1;
            this._performanceReceivedLabel.Text = "Received: 0 B";
            // 
            // _performanceGraphPanel
            // 
            this._performanceGraphPanel.BackColor = System.Drawing.Color.White;
            this._performanceGraphPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._performanceGraphPanel.Location = new System.Drawing.Point(9, 9);
            this._performanceGraphPanel.Name = "_performanceGraphPanel";
            this._performanceGraphPanel.Size = new System.Drawing.Size(441, 1);
            this._performanceGraphPanel.TabIndex = 0;
            this._performanceGraphPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.PerformanceGraphPanel_Paint);
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
            this.ChangeMacAddressGroup.Controls.Add(this.macTextBox);
            this.ChangeMacAddressGroup.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ChangeMacAddressGroup.Location = new System.Drawing.Point(3, 3);
            this.ChangeMacAddressGroup.Name = "ChangeMacAddressGroup";
            this.ChangeMacAddressGroup.Size = new System.Drawing.Size(465, 1);
            this.ChangeMacAddressGroup.TabIndex = 1;
            this.ChangeMacAddressGroup.TabStop = false;
            this.ChangeMacAddressGroup.Text = "Change Mac Address";
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
            // macTextBox
            // 
            this.macTextBox.BackColor = System.Drawing.SystemColors.Window;
            this.macTextBox.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.macTextBox.Location = new System.Drawing.Point(6, 19);
            this.macTextBox.Name = "macTextBox";
            this.macTextBox.Size = new System.Drawing.Size(171, 24);
            this.macTextBox.TabIndex = 0;
            // 
            // IPAddressPage
            // 
            this.IPAddressPage.Controls.Add(this.IpAddressLayoutPanel);
            this.IPAddressPage.Location = new System.Drawing.Point(4, 22);
            this.IPAddressPage.Name = "IPAddressPage";
            this.IPAddressPage.Padding = new System.Windows.Forms.Padding(3);
            this.IPAddressPage.Size = new System.Drawing.Size(948, 0);
            this.IPAddressPage.TabIndex = 1;
            this.IPAddressPage.Text = "IP Address";
            this.IPAddressPage.UseVisualStyleBackColor = true;
            // 
            // IpAddressLayoutPanel
            // 
            this.IpAddressLayoutPanel.ColumnCount = 2;
            this.IpAddressLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.IpAddressLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.IpAddressLayoutPanel.Controls.Add(this.Ipv4ColumnLayoutPanel, 0, 0);
            this.IpAddressLayoutPanel.Controls.Add(this.Ipv6ColumnLayoutPanel, 1, 0);
            this.IpAddressLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.IpAddressLayoutPanel.Location = new System.Drawing.Point(3, 3);
            this.IpAddressLayoutPanel.Name = "IpAddressLayoutPanel";
            this.IpAddressLayoutPanel.RowCount = 1;
            this.IpAddressLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.IpAddressLayoutPanel.Size = new System.Drawing.Size(942, 0);
            this.IpAddressLayoutPanel.TabIndex = 0;
            // 
            // Ipv4ColumnLayoutPanel
            // 
            this.Ipv4ColumnLayoutPanel.ColumnCount = 1;
            this.Ipv4ColumnLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.Ipv4ColumnLayoutPanel.Controls.Add(this.Ipv4AddressGroupBox, 0, 0);
            this.Ipv4ColumnLayoutPanel.Controls.Add(this.Ipv4GatewayGroupBox, 0, 1);
            this.Ipv4ColumnLayoutPanel.Controls.Add(this.Ipv4DnsGroupBox, 0, 2);
            this.Ipv4ColumnLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Ipv4ColumnLayoutPanel.Location = new System.Drawing.Point(3, 3);
            this.Ipv4ColumnLayoutPanel.Name = "Ipv4ColumnLayoutPanel";
            this.Ipv4ColumnLayoutPanel.RowCount = 3;
            this.Ipv4ColumnLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 30.96591F));
            this.Ipv4ColumnLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 26.42045F));
            this.Ipv4ColumnLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 42.61364F));
            this.Ipv4ColumnLayoutPanel.Size = new System.Drawing.Size(465, 1);
            this.Ipv4ColumnLayoutPanel.TabIndex = 0;
            // 
            // Ipv4AddressGroupBox
            // 
            this.Ipv4AddressGroupBox.Controls.Add(this.Ipv4AddressListView);
            this.Ipv4AddressGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Ipv4AddressGroupBox.Location = new System.Drawing.Point(3, 3);
            this.Ipv4AddressGroupBox.Name = "Ipv4AddressGroupBox";
            this.Ipv4AddressGroupBox.Size = new System.Drawing.Size(459, 1);
            this.Ipv4AddressGroupBox.TabIndex = 0;
            this.Ipv4AddressGroupBox.TabStop = false;
            this.Ipv4AddressGroupBox.Text = "Internet Protocol v4 (DHCPv4)";
            // 
            // Ipv4AddressListView
            // 
            this.Ipv4AddressListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.Ipv4AddressColumnHeader,
            this.Ipv4SubnetMaskColumnHeader});
            this.Ipv4AddressListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Ipv4AddressListView.FullRowSelect = true;
            this.Ipv4AddressListView.GridLines = true;
            this.Ipv4AddressListView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.Ipv4AddressListView.HideSelection = false;
            this.Ipv4AddressListView.Location = new System.Drawing.Point(3, 16);
            this.Ipv4AddressListView.Name = "Ipv4AddressListView";
            this.Ipv4AddressListView.Size = new System.Drawing.Size(453, 0);
            this.Ipv4AddressListView.TabIndex = 0;
            this.Ipv4AddressListView.UseCompatibleStateImageBehavior = false;
            this.Ipv4AddressListView.View = System.Windows.Forms.View.Details;
            // 
            // Ipv4AddressColumnHeader
            // 
            this.Ipv4AddressColumnHeader.Text = "IP Address";
            this.Ipv4AddressColumnHeader.Width = 25;
            // 
            // Ipv4SubnetMaskColumnHeader
            // 
            this.Ipv4SubnetMaskColumnHeader.Text = "Subnet Mask";
            this.Ipv4SubnetMaskColumnHeader.Width = 25;
            // 
            // Ipv4GatewayGroupBox
            // 
            this.Ipv4GatewayGroupBox.Controls.Add(this.Ipv4GatewayListView);
            this.Ipv4GatewayGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Ipv4GatewayGroupBox.Location = new System.Drawing.Point(3, 3);
            this.Ipv4GatewayGroupBox.Name = "Ipv4GatewayGroupBox";
            this.Ipv4GatewayGroupBox.Size = new System.Drawing.Size(459, 1);
            this.Ipv4GatewayGroupBox.TabIndex = 1;
            this.Ipv4GatewayGroupBox.TabStop = false;
            this.Ipv4GatewayGroupBox.Text = "Gateway";
            // 
            // Ipv4GatewayListView
            // 
            this.Ipv4GatewayListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.Ipv4GatewayColumnHeader});
            this.Ipv4GatewayListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Ipv4GatewayListView.FullRowSelect = true;
            this.Ipv4GatewayListView.GridLines = true;
            this.Ipv4GatewayListView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.Ipv4GatewayListView.HideSelection = false;
            this.Ipv4GatewayListView.Location = new System.Drawing.Point(3, 16);
            this.Ipv4GatewayListView.Name = "Ipv4GatewayListView";
            this.Ipv4GatewayListView.Size = new System.Drawing.Size(453, 0);
            this.Ipv4GatewayListView.TabIndex = 0;
            this.Ipv4GatewayListView.UseCompatibleStateImageBehavior = false;
            this.Ipv4GatewayListView.View = System.Windows.Forms.View.Details;
            // 
            // Ipv4GatewayColumnHeader
            // 
            this.Ipv4GatewayColumnHeader.Text = "Gateway";
            this.Ipv4GatewayColumnHeader.Width = 25;
            // 
            // Ipv4DnsGroupBox
            // 
            this.Ipv4DnsGroupBox.Controls.Add(this.Ipv4DnsListView);
            this.Ipv4DnsGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Ipv4DnsGroupBox.Location = new System.Drawing.Point(3, 3);
            this.Ipv4DnsGroupBox.Name = "Ipv4DnsGroupBox";
            this.Ipv4DnsGroupBox.Size = new System.Drawing.Size(459, 1);
            this.Ipv4DnsGroupBox.TabIndex = 2;
            this.Ipv4DnsGroupBox.TabStop = false;
            this.Ipv4DnsGroupBox.Text = "DNS Server";
            // 
            // Ipv4DnsListView
            // 
            this.Ipv4DnsListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.Ipv4DnsColumnHeader});
            this.Ipv4DnsListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Ipv4DnsListView.FullRowSelect = true;
            this.Ipv4DnsListView.GridLines = true;
            this.Ipv4DnsListView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.Ipv4DnsListView.HideSelection = false;
            this.Ipv4DnsListView.Location = new System.Drawing.Point(3, 16);
            this.Ipv4DnsListView.Name = "Ipv4DnsListView";
            this.Ipv4DnsListView.Size = new System.Drawing.Size(453, 0);
            this.Ipv4DnsListView.TabIndex = 0;
            this.Ipv4DnsListView.UseCompatibleStateImageBehavior = false;
            this.Ipv4DnsListView.View = System.Windows.Forms.View.Details;
            // 
            // Ipv4DnsColumnHeader
            // 
            this.Ipv4DnsColumnHeader.Text = "DNS Server";
            this.Ipv4DnsColumnHeader.Width = 25;
            // 
            // Ipv6ColumnLayoutPanel
            // 
            this.Ipv6ColumnLayoutPanel.ColumnCount = 1;
            this.Ipv6ColumnLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.Ipv6ColumnLayoutPanel.Controls.Add(this.Ipv6AddressGroupBox, 0, 0);
            this.Ipv6ColumnLayoutPanel.Controls.Add(this.Ipv6GatewayGroupBox, 0, 1);
            this.Ipv6ColumnLayoutPanel.Controls.Add(this.Ipv6DnsGroupBox, 0, 2);
            this.Ipv6ColumnLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Ipv6ColumnLayoutPanel.Location = new System.Drawing.Point(474, 3);
            this.Ipv6ColumnLayoutPanel.Name = "Ipv6ColumnLayoutPanel";
            this.Ipv6ColumnLayoutPanel.RowCount = 3;
            this.Ipv6ColumnLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 31.25F));
            this.Ipv6ColumnLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 26.42045F));
            this.Ipv6ColumnLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 42.61364F));
            this.Ipv6ColumnLayoutPanel.Size = new System.Drawing.Size(465, 1);
            this.Ipv6ColumnLayoutPanel.TabIndex = 1;
            // 
            // Ipv6AddressGroupBox
            // 
            this.Ipv6AddressGroupBox.Controls.Add(this.Ipv6AddressListView);
            this.Ipv6AddressGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Ipv6AddressGroupBox.Location = new System.Drawing.Point(3, 3);
            this.Ipv6AddressGroupBox.Name = "Ipv6AddressGroupBox";
            this.Ipv6AddressGroupBox.Size = new System.Drawing.Size(459, 1);
            this.Ipv6AddressGroupBox.TabIndex = 0;
            this.Ipv6AddressGroupBox.TabStop = false;
            this.Ipv6AddressGroupBox.Text = "Internet Protocol v6 (Stateless)";
            // 
            // Ipv6AddressListView
            // 
            this.Ipv6AddressListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.Ipv6AddressColumnHeader,
            this.Ipv6PrefixColumnHeader});
            this.Ipv6AddressListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Ipv6AddressListView.FullRowSelect = true;
            this.Ipv6AddressListView.GridLines = true;
            this.Ipv6AddressListView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.Ipv6AddressListView.HideSelection = false;
            this.Ipv6AddressListView.Location = new System.Drawing.Point(3, 16);
            this.Ipv6AddressListView.Name = "Ipv6AddressListView";
            this.Ipv6AddressListView.Size = new System.Drawing.Size(453, 0);
            this.Ipv6AddressListView.TabIndex = 0;
            this.Ipv6AddressListView.UseCompatibleStateImageBehavior = false;
            this.Ipv6AddressListView.View = System.Windows.Forms.View.Details;
            // 
            // Ipv6AddressColumnHeader
            // 
            this.Ipv6AddressColumnHeader.Text = "Unicast IPv6 Address";
            this.Ipv6AddressColumnHeader.Width = 300;
            // 
            // Ipv6PrefixColumnHeader
            // 
            this.Ipv6PrefixColumnHeader.Text = "Prefix";
            this.Ipv6PrefixColumnHeader.Width = 200;
            // 
            // Ipv6GatewayGroupBox
            // 
            this.Ipv6GatewayGroupBox.Controls.Add(this.Ipv6GatewayListView);
            this.Ipv6GatewayGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Ipv6GatewayGroupBox.Location = new System.Drawing.Point(3, 3);
            this.Ipv6GatewayGroupBox.Name = "Ipv6GatewayGroupBox";
            this.Ipv6GatewayGroupBox.Size = new System.Drawing.Size(459, 1);
            this.Ipv6GatewayGroupBox.TabIndex = 1;
            this.Ipv6GatewayGroupBox.TabStop = false;
            this.Ipv6GatewayGroupBox.Text = "Gateway/Next Hop";
            // 
            // Ipv6GatewayListView
            // 
            this.Ipv6GatewayListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.Ipv6GatewayColumnHeader});
            this.Ipv6GatewayListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Ipv6GatewayListView.FullRowSelect = true;
            this.Ipv6GatewayListView.GridLines = true;
            this.Ipv6GatewayListView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.Ipv6GatewayListView.HideSelection = false;
            this.Ipv6GatewayListView.Location = new System.Drawing.Point(3, 16);
            this.Ipv6GatewayListView.Name = "Ipv6GatewayListView";
            this.Ipv6GatewayListView.Size = new System.Drawing.Size(453, 0);
            this.Ipv6GatewayListView.TabIndex = 0;
            this.Ipv6GatewayListView.UseCompatibleStateImageBehavior = false;
            this.Ipv6GatewayListView.View = System.Windows.Forms.View.Details;
            // 
            // Ipv6GatewayColumnHeader
            // 
            this.Ipv6GatewayColumnHeader.Text = "Gateway/Next Hop";
            this.Ipv6GatewayColumnHeader.Width = 420;
            // 
            // Ipv6DnsGroupBox
            // 
            this.Ipv6DnsGroupBox.Controls.Add(this.Ipv6DnsListView);
            this.Ipv6DnsGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Ipv6DnsGroupBox.Location = new System.Drawing.Point(3, 3);
            this.Ipv6DnsGroupBox.Name = "Ipv6DnsGroupBox";
            this.Ipv6DnsGroupBox.Size = new System.Drawing.Size(459, 1);
            this.Ipv6DnsGroupBox.TabIndex = 2;
            this.Ipv6DnsGroupBox.TabStop = false;
            this.Ipv6DnsGroupBox.Text = "DNS Server";
            // 
            // Ipv6DnsListView
            // 
            this.Ipv6DnsListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.Ipv6DnsColumnHeader});
            this.Ipv6DnsListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Ipv6DnsListView.FullRowSelect = true;
            this.Ipv6DnsListView.GridLines = true;
            this.Ipv6DnsListView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.Ipv6DnsListView.HideSelection = false;
            this.Ipv6DnsListView.Location = new System.Drawing.Point(3, 16);
            this.Ipv6DnsListView.Name = "Ipv6DnsListView";
            this.Ipv6DnsListView.Size = new System.Drawing.Size(453, 0);
            this.Ipv6DnsListView.TabIndex = 0;
            this.Ipv6DnsListView.UseCompatibleStateImageBehavior = false;
            this.Ipv6DnsListView.View = System.Windows.Forms.View.Details;
            // 
            // Ipv6DnsColumnHeader
            // 
            this.Ipv6DnsColumnHeader.Text = "DNS Server";
            this.Ipv6DnsColumnHeader.Width = 420;
            // 
            // PresetsPage
            // 
            this.PresetsPage.Controls.Add(this.PresetRootLayout);
            this.PresetsPage.Location = new System.Drawing.Point(4, 22);
            this.PresetsPage.Name = "PresetsPage";
            this.PresetsPage.Padding = new System.Windows.Forms.Padding(3);
            this.PresetsPage.Size = new System.Drawing.Size(948, 0);
            this.PresetsPage.TabIndex = 2;
            this.PresetsPage.Text = "Presets";
            this.PresetsPage.UseVisualStyleBackColor = true;
            // 
            // PresetRootLayout
            // 
            this.PresetRootLayout.ColumnCount = 2;
            this.PresetRootLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 260F));
            this.PresetRootLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.PresetRootLayout.Controls.Add(this._presetListBox, 0, 0);
            this.PresetRootLayout.Controls.Add(this._presetPropertyListView, 1, 0);
            this.PresetRootLayout.Controls.Add(this.PresetButtonFlowPanel, 0, 1);
            this.PresetRootLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PresetRootLayout.Location = new System.Drawing.Point(3, 3);
            this.PresetRootLayout.Name = "PresetRootLayout";
            this.PresetRootLayout.RowCount = 2;
            this.PresetRootLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.PresetRootLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.PresetRootLayout.Size = new System.Drawing.Size(942, 0);
            this.PresetRootLayout.TabIndex = 0;
            // 
            // _presetListBox
            // 
            this._presetListBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this._presetListBox.FormattingEnabled = true;
            this._presetListBox.Location = new System.Drawing.Point(3, 3);
            this._presetListBox.Name = "_presetListBox";
            this._presetListBox.Size = new System.Drawing.Size(254, 1);
            this._presetListBox.TabIndex = 0;
            // 
            // _presetPropertyListView
            // 
            this._presetPropertyListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.PresetPropertyColumnHeader,
            this.PresetValueColumnHeader});
            this._presetPropertyListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this._presetPropertyListView.FullRowSelect = true;
            this._presetPropertyListView.GridLines = true;
            this._presetPropertyListView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this._presetPropertyListView.HideSelection = false;
            this._presetPropertyListView.Location = new System.Drawing.Point(263, 3);
            this._presetPropertyListView.Name = "_presetPropertyListView";
            this._presetPropertyListView.Size = new System.Drawing.Size(676, 1);
            this._presetPropertyListView.TabIndex = 1;
            this._presetPropertyListView.UseCompatibleStateImageBehavior = false;
            this._presetPropertyListView.View = System.Windows.Forms.View.Details;
            // 
            // PresetPropertyColumnHeader
            // 
            this.PresetPropertyColumnHeader.Text = "Property";
            this.PresetPropertyColumnHeader.Width = 180;
            // 
            // PresetValueColumnHeader
            // 
            this.PresetValueColumnHeader.Text = "Value";
            this.PresetValueColumnHeader.Width = 420;
            // 
            // PresetButtonFlowPanel
            // 
            this.PresetRootLayout.SetColumnSpan(this.PresetButtonFlowPanel, 2);
            this.PresetButtonFlowPanel.Controls.Add(this._presetNewButton);
            this.PresetButtonFlowPanel.Controls.Add(this._presetEditButton);
            this.PresetButtonFlowPanel.Controls.Add(this._presetDeleteButton);
            this.PresetButtonFlowPanel.Controls.Add(this._presetApplyButton);
            this.PresetButtonFlowPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PresetButtonFlowPanel.Location = new System.Drawing.Point(3, -36);
            this.PresetButtonFlowPanel.Name = "PresetButtonFlowPanel";
            this.PresetButtonFlowPanel.Size = new System.Drawing.Size(936, 34);
            this.PresetButtonFlowPanel.TabIndex = 2;
            // 
            // _presetNewButton
            // 
            this._presetNewButton.Location = new System.Drawing.Point(3, 3);
            this._presetNewButton.Name = "_presetNewButton";
            this._presetNewButton.Size = new System.Drawing.Size(70, 23);
            this._presetNewButton.TabIndex = 0;
            this._presetNewButton.Text = "New";
            this._presetNewButton.UseVisualStyleBackColor = true;
            // 
            // _presetEditButton
            // 
            this._presetEditButton.Location = new System.Drawing.Point(79, 3);
            this._presetEditButton.Name = "_presetEditButton";
            this._presetEditButton.Size = new System.Drawing.Size(70, 23);
            this._presetEditButton.TabIndex = 1;
            this._presetEditButton.Text = "Edit";
            this._presetEditButton.UseVisualStyleBackColor = true;
            // 
            // _presetDeleteButton
            // 
            this._presetDeleteButton.Location = new System.Drawing.Point(155, 3);
            this._presetDeleteButton.Name = "_presetDeleteButton";
            this._presetDeleteButton.Size = new System.Drawing.Size(70, 23);
            this._presetDeleteButton.TabIndex = 2;
            this._presetDeleteButton.Text = "Delete";
            this._presetDeleteButton.UseVisualStyleBackColor = true;
            // 
            // _presetApplyButton
            // 
            this._presetApplyButton.Location = new System.Drawing.Point(231, 3);
            this._presetApplyButton.Name = "_presetApplyButton";
            this._presetApplyButton.Size = new System.Drawing.Size(70, 23);
            this._presetApplyButton.TabIndex = 3;
            this._presetApplyButton.Text = "Apply";
            this._presetApplyButton.UseVisualStyleBackColor = true;
            // 
            // ConnectionsGrid
            // 
            this.ConnectionsGrid.Alignment = System.Windows.Forms.ListViewAlignment.SnapToGrid;
            this.ConnectionsGrid.AllowColumnReorder = true;
            this.ConnectionsGrid.CausesValidation = false;
            this.ConnectionsGrid.CellEditUseWholeCell = false;
            this.ConnectionsGrid.DataSource = null;
            this.ConnectionsGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ConnectionsGrid.FullRowSelect = true;
            this.ConnectionsGrid.HideSelection = false;
            this.ConnectionsGrid.Location = new System.Drawing.Point(3, 3);
            this.ConnectionsGrid.MinimumSize = new System.Drawing.Size(800, 4);
            this.ConnectionsGrid.MultiSelect = false;
            this.ConnectionsGrid.Name = "ConnectionsGrid";
            this.ConnectionsGrid.ShowGroups = false;
            this.ConnectionsGrid.ShowItemToolTips = true;
            this.ConnectionsGrid.Size = new System.Drawing.Size(956, 121);
            this.ConnectionsGrid.SpaceBetweenGroups = 1;
            this.ConnectionsGrid.TabIndex = 1;
            this.ConnectionsGrid.UseCompatibleStateImageBehavior = false;
            this.ConnectionsGrid.View = System.Windows.Forms.View.Details;
            this.ConnectionsGrid.SelectedIndexChanged += new System.EventHandler(this.ConnectionsGrid_SelectedIndexChanged);
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(200, 100);
            this.flowLayoutPanel1.TabIndex = 0;
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
            this.Toolbar.Size = new System.Drawing.Size(962, 24);
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
            this.toolStripSeparator9,
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
            // toolStripSeparator9
            // 
            this.toolStripSeparator9.Name = "toolStripSeparator9";
            this.toolStripSeparator9.Size = new System.Drawing.Size(237, 6);
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
            this.ToggleAdapterEnabledItem,
            this.toolStripSeparator8,
            this.Dhcp4Item,
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
            // ToggleAdapterEnabledItem
            // 
            this.ToggleAdapterEnabledItem.Name = "ToggleAdapterEnabledItem";
            this.ToggleAdapterEnabledItem.Size = new System.Drawing.Size(274, 22);
            this.ToggleAdapterEnabledItem.Text = "Disable Adapter";
            this.ToggleAdapterEnabledItem.Click += new System.EventHandler(this.ToggleAdapterEnabledItem_Click);
            // 
            // toolStripSeparator8
            // 
            this.toolStripSeparator8.Name = "toolStripSeparator8";
            this.toolStripSeparator8.Size = new System.Drawing.Size(271, 6);
            // 
            // Dhcp4Item
            // 
            this.Dhcp4Item.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.DhcpEnabledItem,
            this.DhcpReleaseIpItem,
            this.DhcpRenewIpItem});
            this.Dhcp4Item.Name = "Dhcp4Item";
            this.Dhcp4Item.Size = new System.Drawing.Size(274, 22);
            this.Dhcp4Item.Text = "DHCP";
            // 
            // DhcpEnabledItem
            // 
            this.DhcpEnabledItem.Name = "DhcpEnabledItem";
            this.DhcpEnabledItem.Size = new System.Drawing.Size(163, 22);
            this.DhcpEnabledItem.Text = "DHCPv4 Enabled";
            this.DhcpEnabledItem.Click += new System.EventHandler(this.Dhcp4EnabledItem_Click);
            // 
            // DhcpReleaseIpItem
            // 
            this.DhcpReleaseIpItem.Enabled = false;
            this.DhcpReleaseIpItem.Name = "DhcpReleaseIpItem";
            this.DhcpReleaseIpItem.Size = new System.Drawing.Size(163, 22);
            this.DhcpReleaseIpItem.Text = "Release IP";
            this.DhcpReleaseIpItem.Click += new System.EventHandler(this.DhcpReleaseIpItem_Click);
            // 
            // DhcpRenewIpItem
            // 
            this.DhcpRenewIpItem.Enabled = false;
            this.DhcpRenewIpItem.Name = "DhcpRenewIpItem";
            this.DhcpRenewIpItem.Size = new System.Drawing.Size(163, 22);
            this.DhcpRenewIpItem.Text = "Renew IP";
            this.DhcpRenewIpItem.Click += new System.EventHandler(this.DhcpRenewIpItem_Click);
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
            this.OptionsMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.NetworkConnectionsItem,
            this.ShowSpeedInKBytesPerSecItem,
            this.ShowAllAdaptersItem});
            this.OptionsMenu.Name = "OptionsMenu";
            this.OptionsMenu.Size = new System.Drawing.Size(61, 20);
            this.OptionsMenu.Text = "&Options";
            // 
            // NetworkConnectionsItem
            // 
            this.NetworkConnectionsItem.Name = "NetworkConnectionsItem";
            this.NetworkConnectionsItem.Size = new System.Drawing.Size(211, 22);
            this.NetworkConnectionsItem.Text = "Network Connections";
            this.NetworkConnectionsItem.Click += new System.EventHandler(this.NetworkConnectionsItem_Click);
            // 
            // ShowSpeedInKBytesPerSecItem
            // 
            this.ShowSpeedInKBytesPerSecItem.CheckOnClick = true;
            this.ShowSpeedInKBytesPerSecItem.Name = "ShowSpeedInKBytesPerSecItem";
            this.ShowSpeedInKBytesPerSecItem.Size = new System.Drawing.Size(211, 22);
            this.ShowSpeedInKBytesPerSecItem.Text = "Show Speed In KBytes/sec";
            this.ShowSpeedInKBytesPerSecItem.CheckedChanged += new System.EventHandler(this.ShowSpeedInKBytesPerSecItem_CheckedChanged);
            // 
            // ShowAllAdaptersItem
            // 
            this.ShowAllAdaptersItem.CheckOnClick = true;
            this.ShowAllAdaptersItem.Name = "ShowAllAdaptersItem";
            this.ShowAllAdaptersItem.Size = new System.Drawing.Size(211, 22);
            this.ShowAllAdaptersItem.Text = "Show All Adapters";
            this.ShowAllAdaptersItem.CheckedChanged += new System.EventHandler(this.ShowAllAdaptersItem_CheckedChanged);
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
            this.UpdateOuiItem.Click += new System.EventHandler(this.UpdateOuiItem_ClickAsync);
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
            this.AboutItem.Text = "About DZMAC";
            this.AboutItem.Click += new System.EventHandler(this.AboutItem_Click);
            // 
            // MainStatusBar
            // 
            this.MainStatusBar.Location = new System.Drawing.Point(0, 552);
            this.MainStatusBar.Name = "MainStatusBar";
            this.MainStatusBar.Size = new System.Drawing.Size(962, 23);
            this.MainStatusBar.SizingGrip = false;
            this.MainStatusBar.TabIndex = 1;
            this.MainStatusBar.Text = "Ready";
            // 
            // _loadingLabel
            // 
            this._loadingLabel.Anchor = System.Windows.Forms.AnchorStyles.None;
            this._loadingLabel.AutoSize = true;
            this._loadingLabel.Location = new System.Drawing.Point(403, 59);
            this._loadingLabel.Name = "_loadingLabel";
            this._loadingLabel.Size = new System.Drawing.Size(156, 13);
            this._loadingLabel.TabIndex = 0;
            this._loadingLabel.Text = "Loading network adapters...";
            this._loadingLabel.Visible = false;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(962, 575);
            this.Controls.Add(this.MainTableLayoutPanel);
            this.Controls.Add(this.MainStatusBar);
            this.Controls.Add(this.Toolbar);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.KeyPreview = true;
            this.MainMenuStrip = this.Toolbar;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "DZMAC";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_LoadAsync);
            this.Resize += new System.EventHandler(this.MainForm_Resize);
            this.MainTableLayoutPanel.ResumeLayout(false);
            this._loadingPanel.ResumeLayout(false);
            this.LoadingLayoutPanel.ResumeLayout(false);
            this.InfoTabs.ResumeLayout(false);
            this.InformationPage.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ConnectionDetailsGroup.ResumeLayout(false);
            this.ConnectionDetailsGroup.PerformLayout();
            this.PerformanceCounterGroup.ResumeLayout(false);
            this.PerformancePanel.ResumeLayout(false);
            this.PerformancePanel.PerformLayout();
            this.ChangeMacAddressGroup.ResumeLayout(false);
            this.IPAddressPage.ResumeLayout(false);
            this.IpAddressLayoutPanel.ResumeLayout(false);
            this.Ipv4ColumnLayoutPanel.ResumeLayout(false);
            this.Ipv4AddressGroupBox.ResumeLayout(false);
            this.Ipv4GatewayGroupBox.ResumeLayout(false);
            this.Ipv4DnsGroupBox.ResumeLayout(false);
            this.Ipv6ColumnLayoutPanel.ResumeLayout(false);
            this.Ipv6AddressGroupBox.ResumeLayout(false);
            this.Ipv6GatewayGroupBox.ResumeLayout(false);
            this.Ipv6DnsGroupBox.ResumeLayout(false);
            this.PresetsPage.ResumeLayout(false);
            this.PresetRootLayout.ResumeLayout(false);
            this.PresetButtonFlowPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ConnectionsGrid)).EndInit();
            this.Toolbar.ResumeLayout(false);
            this.Toolbar.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private TableLayoutPanel? MainTableLayoutPanel;
        private TabControl? InfoTabs;
        private TabPage? InformationPage;
        private TabPage? IPAddressPage;
        private TableLayoutPanel? IpAddressLayoutPanel;
        private TableLayoutPanel? Ipv4ColumnLayoutPanel;
        private GroupBox? Ipv4AddressGroupBox;
        private ListView? Ipv4AddressListView;
        private ColumnHeader? Ipv4AddressColumnHeader;
        private ColumnHeader? Ipv4SubnetMaskColumnHeader;
        private GroupBox? Ipv4GatewayGroupBox;
        private ListView? Ipv4GatewayListView;
        private ColumnHeader? Ipv4GatewayColumnHeader;
        private GroupBox? Ipv4DnsGroupBox;
        private ListView? Ipv4DnsListView;
        private ColumnHeader? Ipv4DnsColumnHeader;
        private TableLayoutPanel? Ipv6ColumnLayoutPanel;
        private GroupBox? Ipv6AddressGroupBox;
        private ListView? Ipv6AddressListView;
        private ColumnHeader? Ipv6AddressColumnHeader;
        private ColumnHeader? Ipv6PrefixColumnHeader;
        private GroupBox? Ipv6GatewayGroupBox;
        private ListView? Ipv6GatewayListView;
        private ColumnHeader? Ipv6GatewayColumnHeader;
        private GroupBox? Ipv6DnsGroupBox;
        private ListView? Ipv6DnsListView;
        private ColumnHeader? Ipv6DnsColumnHeader;
        private TabPage? PresetsPage;
        private DataListView? ConnectionsGrid;
        private MenuStrip? Toolbar;
        private ToolStripMenuItem? FileMenu;
        private ToolStripMenuItem? ExportReportItem;
        private ToolStripMenuItem? OpenPresetItem;
        private ToolStripMenuItem? SavePresetItem;
        private ToolStripMenuItem? SavePresetAsItem;
        private ToolStripMenuItem? ImportPresetItem;
        private ToolStripMenuItem? ExportPresetItem;
        private ToolStripMenuItem? ActionMenu;
        private ToolStripMenuItem? OptionsMenu;
        private ToolStripMenuItem? NetworkConnectionsItem;
        private ToolStripMenuItem? ShowSpeedInKBytesPerSecItem;
        private ToolStripMenuItem? ShowAllAdaptersItem;
        private ToolStripMenuItem? HelpMenu;
        private ToolStripMenuItem? AssociateItem;
        private ToolStripMenuItem? ExitItem;
        private ToolStripMenuItem? RefreshItem;
        private ToolStripMenuItem? ToggleAdapterEnabledItem;
        private ToolStripMenuItem? Dhcp4Item;
        private ToolStripMenuItem? DeleteItem;
        private ToolStripMenuItem? DhcpEnabledItem;
        private ToolStripMenuItem? DhcpReleaseIpItem;
        private ToolStripMenuItem? DhcpRenewIpItem;
        private FlowLayoutPanel? flowLayoutPanel1;
        private GroupBox? ConnectionDetailsGroup;
        private Label? ConnectionLabel;
        private GroupBox? ChangeMacAddressGroup;
        private GroupBox? PerformanceCounterGroup;
        private Label? ConnectionValueTextbox;
        private Label? DeviceLabel;
        private Label? DeviceValueTextbox;
        private StatusBar? MainStatusBar;
        private ToolStripMenuItem? HelpTopicsItem;
        private ToolStripMenuItem? CliParamsHelpItem;
        private ToolStripMenuItem? CheckUpdateItem;
        private ToolStripMenuItem? UpdateOuiItem;
        private ToolStripMenuItem? AboutItem;
        private ToolStripSeparator? toolStripSeparator1;
        private ToolStripSeparator? toolStripSeparator2;
        private ToolStripSeparator? toolStripSeparator3;
        private ToolStripSeparator? toolStripSeparator4;
        private ToolStripSeparator? toolStripSeparator8;
        private ToolStripSeparator? toolStripSeparator6;
        private ToolStripSeparator? toolStripSeparator5;
        private ToolStripSeparator? toolStripSeparator7;
        private Label? HardwareIdLabel;
        private Label? ConfigIdValueTextbox;
        private Label? ConfigIdLabel;
        private Label? HardwareIdValueTextbox;
        private Label? Ipv4ValueTextbox;
        private Label? Ipv4Label;
        private Label? Ipv6ValueTextbox;
        private Label? Ipv6Label;
        private Label? OriginalMacVendorTextbox;
        private Label? OriginalMacValueTextbox;
        private Label? OriginalMacLabel;
        private Label? ActiveMacVendorTextbox;
        private Label? ActiveMacValueTextbox;
        private Label? ActiveMacLabel;
        private Button? RestoreMacButton;
        private Button? ChangeMacButton;
        private CheckBox? ZeroTwoCheckBox;
        private CheckBox? PersistentAddressCheckBox;
        private CheckBox? AutoStartCheckBox;
        private ComboBox? VendorComboBox;
        private Button? RandomMacButton;
        private Controls.MacTextBox? macTextBox;
        private LinkLabel? WikiLink;
        private TableLayoutPanel? tableLayoutPanel1;
        private TableLayoutPanel? PerformancePanel;
        private Label? _performanceSentSpeedLabel;
        private Label? _performanceSentLabel;
        private Label? _performanceReceivedSpeedLabel;
        private Label? _performanceReceivedLabel;
        private Panel? _performanceGraphPanel;
        private Panel? _loadingPanel;
        private TableLayoutPanel? LoadingLayoutPanel;
        private Label? _loadingLabel;
        private ProgressBar? _loadingProgressBar;
        private ToolStripSeparator? toolStripSeparator9;
        private TableLayoutPanel? PresetRootLayout;
        private ListBox? _presetListBox;
        private ListView? _presetPropertyListView;
        private ColumnHeader? PresetPropertyColumnHeader;
        private ColumnHeader? PresetValueColumnHeader;
        private FlowLayoutPanel? PresetButtonFlowPanel;
        private Button? _presetNewButton;
        private Button? _presetEditButton;
        private Button? _presetDeleteButton;
        private Button? _presetApplyButton;
    }
}
