using BrightIdeasSoftware;
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
            this.DeviceLabel = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.ConnectionNameLabel = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.ChangeMacAddressGroup = new System.Windows.Forms.GroupBox();
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
            this.MainTableLayoutPanel.SuspendLayout();
            this.InfoTabs.SuspendLayout();
            this.InformationPage.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.ConnectionDetailsGroup.SuspendLayout();
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
            this.MainTableLayoutPanel.Size = new System.Drawing.Size(884, 497);
            this.MainTableLayoutPanel.TabIndex = 0;
            // 
            // InfoTabs
            // 
            this.InfoTabs.Controls.Add(this.InformationPage);
            this.InfoTabs.Controls.Add(this.IPAddressPage);
            this.InfoTabs.Controls.Add(this.PresetsPage);
            this.InfoTabs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.InfoTabs.Location = new System.Drawing.Point(3, 127);
            this.InfoTabs.Name = "InfoTabs";
            this.InfoTabs.SelectedIndex = 0;
            this.InfoTabs.Size = new System.Drawing.Size(878, 367);
            this.InfoTabs.TabIndex = 0;
            // 
            // InformationPage
            // 
            this.InformationPage.Controls.Add(this.flowLayoutPanel1);
            this.InformationPage.Location = new System.Drawing.Point(4, 22);
            this.InformationPage.Name = "InformationPage";
            this.InformationPage.Padding = new System.Windows.Forms.Padding(3);
            this.InformationPage.Size = new System.Drawing.Size(870, 341);
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
            this.flowLayoutPanel1.Size = new System.Drawing.Size(864, 335);
            this.flowLayoutPanel1.TabIndex = 0;
            // 
            // ConnectionDetailsGroup
            // 
            this.ConnectionDetailsGroup.Controls.Add(this.DeviceLabel);
            this.ConnectionDetailsGroup.Controls.Add(this.label2);
            this.ConnectionDetailsGroup.Controls.Add(this.ConnectionNameLabel);
            this.ConnectionDetailsGroup.Controls.Add(this.label1);
            this.ConnectionDetailsGroup.Location = new System.Drawing.Point(3, 3);
            this.ConnectionDetailsGroup.Name = "ConnectionDetailsGroup";
            this.ConnectionDetailsGroup.Size = new System.Drawing.Size(858, 151);
            this.ConnectionDetailsGroup.TabIndex = 0;
            this.ConnectionDetailsGroup.TabStop = false;
            this.ConnectionDetailsGroup.Text = "Connection Details";
            // 
            // DeviceLabel
            // 
            this.DeviceLabel.AutoSize = true;
            this.DeviceLabel.Location = new System.Drawing.Point(84, 37);
            this.DeviceLabel.Name = "DeviceLabel";
            this.DeviceLabel.Size = new System.Drawing.Size(16, 13);
            this.DeviceLabel.TabIndex = 3;
            this.DeviceLabel.Text = "...";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(6, 37);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(47, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Device";
            // 
            // ConnectionNameLabel
            // 
            this.ConnectionNameLabel.AutoSize = true;
            this.ConnectionNameLabel.Location = new System.Drawing.Point(84, 20);
            this.ConnectionNameLabel.Name = "ConnectionNameLabel";
            this.ConnectionNameLabel.Size = new System.Drawing.Size(16, 13);
            this.ConnectionNameLabel.TabIndex = 1;
            this.ConnectionNameLabel.Text = "...";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(6, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Connection";
            // 
            // ChangeMacAddressGroup
            // 
            this.ChangeMacAddressGroup.Location = new System.Drawing.Point(3, 160);
            this.ChangeMacAddressGroup.Name = "ChangeMacAddressGroup";
            this.ChangeMacAddressGroup.Size = new System.Drawing.Size(400, 175);
            this.ChangeMacAddressGroup.TabIndex = 1;
            this.ChangeMacAddressGroup.TabStop = false;
            this.ChangeMacAddressGroup.Text = "Change Mac Address";
            // 
            // PerformanceCounterGroup
            // 
            this.PerformanceCounterGroup.Location = new System.Drawing.Point(409, 160);
            this.PerformanceCounterGroup.Name = "PerformanceCounterGroup";
            this.PerformanceCounterGroup.Size = new System.Drawing.Size(452, 175);
            this.PerformanceCounterGroup.TabIndex = 2;
            this.PerformanceCounterGroup.TabStop = false;
            // 
            // IPAddressPage
            // 
            this.IPAddressPage.Location = new System.Drawing.Point(4, 22);
            this.IPAddressPage.Name = "IPAddressPage";
            this.IPAddressPage.Padding = new System.Windows.Forms.Padding(3);
            this.IPAddressPage.Size = new System.Drawing.Size(870, 341);
            this.IPAddressPage.TabIndex = 1;
            this.IPAddressPage.Text = "IP Address";
            this.IPAddressPage.UseVisualStyleBackColor = true;
            // 
            // PresetsPage
            // 
            this.PresetsPage.Location = new System.Drawing.Point(4, 22);
            this.PresetsPage.Name = "PresetsPage";
            this.PresetsPage.Padding = new System.Windows.Forms.Padding(3);
            this.PresetsPage.Size = new System.Drawing.Size(870, 341);
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
            this.ConnectionsGrid.Size = new System.Drawing.Size(878, 118);
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
            this.MainStatusBar.Location = new System.Drawing.Point(0, 499);
            this.MainStatusBar.Name = "MainStatusBar";
            this.MainStatusBar.Size = new System.Drawing.Size(884, 22);
            this.MainStatusBar.TabIndex = 1;
            this.MainStatusBar.Text = "Ready";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(884, 521);
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
            ((System.ComponentModel.ISupportInitialize)(this.ConnectionsGrid)).EndInit();
            this.Toolbar.ResumeLayout(false);
            this.Toolbar.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel MainTableLayoutPanel;
        private System.Windows.Forms.TabControl InfoTabs;
        private System.Windows.Forms.TabPage InformationPage;
        private System.Windows.Forms.TabPage IPAddressPage;
        private System.Windows.Forms.TabPage PresetsPage;
        private DataListView ConnectionsGrid;
        private System.Windows.Forms.MenuStrip Toolbar;
        private System.Windows.Forms.ToolStripMenuItem FileMenu;
        private System.Windows.Forms.ToolStripMenuItem ExportReportItem;
        private System.Windows.Forms.ToolStripMenuItem OpenPresetItem;
        private System.Windows.Forms.ToolStripMenuItem SavePresetItem;
        private System.Windows.Forms.ToolStripMenuItem SavePresetAsItem;
        private System.Windows.Forms.ToolStripMenuItem ImportPresetItem;
        private System.Windows.Forms.ToolStripMenuItem ExportPresetItem;
        private System.Windows.Forms.ToolStripMenuItem ActionMenu;
        private System.Windows.Forms.ToolStripMenuItem OptionsMenu;
        private System.Windows.Forms.ToolStripMenuItem HelpMenu;
        private System.Windows.Forms.ToolStripMenuItem AssociateItem;
        private System.Windows.Forms.ToolStripMenuItem ExitItem;
        private System.Windows.Forms.ToolStripMenuItem RefreshItem;
        private System.Windows.Forms.ToolStripMenuItem Dhcp4Item;
        private System.Windows.Forms.ToolStripMenuItem Dhcp6Item;
        private System.Windows.Forms.ToolStripMenuItem DeleteItem;
        private System.Windows.Forms.ToolStripMenuItem Dhcp4EnabledItem;
        private System.Windows.Forms.ToolStripMenuItem Dhcp4ReleaseIpItem;
        private System.Windows.Forms.ToolStripMenuItem Dhcp4RenewIpItem;
        private System.Windows.Forms.ToolStripMenuItem Dhcp6EnabledItem;
        private System.Windows.Forms.ToolStripMenuItem Dhcp6ReleaseIpItem;
        private System.Windows.Forms.ToolStripMenuItem Dhcp6RenewItem;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.GroupBox ConnectionDetailsGroup;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox ChangeMacAddressGroup;
        private System.Windows.Forms.GroupBox PerformanceCounterGroup;
        private System.Windows.Forms.Label ConnectionNameLabel;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label DeviceLabel;
        private System.Windows.Forms.StatusBar MainStatusBar;
        private System.Windows.Forms.ToolStripMenuItem HelpTopicsItem;
        private System.Windows.Forms.ToolStripMenuItem CliParamsHelpItem;
        private System.Windows.Forms.ToolStripMenuItem CheckUpdateItem;
        private System.Windows.Forms.ToolStripMenuItem UpdateOuiItem;
        private System.Windows.Forms.ToolStripMenuItem AboutItem;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripSeparator toolStripSeparator2;
        private ToolStripSeparator toolStripSeparator3;
        private ToolStripSeparator toolStripSeparator4;
        private ToolStripSeparator toolStripSeparator6;
        private ToolStripSeparator toolStripSeparator5;
        private ToolStripSeparator toolStripSeparator7;
    }
}

