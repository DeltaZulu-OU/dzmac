using BrightIdeasSoftware;

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
            this.components = new System.ComponentModel.Container();
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
            this.MainMenu = new System.Windows.Forms.MainMenu(this.components);
            this.FileMenu = new System.Windows.Forms.MenuItem();
            this.ExportReportItem = new System.Windows.Forms.MenuItem();
            this.menuItem14 = new System.Windows.Forms.MenuItem();
            this.OpenPresetItem = new System.Windows.Forms.MenuItem();
            this.SavePresetItem = new System.Windows.Forms.MenuItem();
            this.SavePresetAsItem = new System.Windows.Forms.MenuItem();
            this.menuItem15 = new System.Windows.Forms.MenuItem();
            this.ImportPresetItem = new System.Windows.Forms.MenuItem();
            this.ExportPresetItem = new System.Windows.Forms.MenuItem();
            this.menuItem16 = new System.Windows.Forms.MenuItem();
            this.AssociateItem = new System.Windows.Forms.MenuItem();
            this.menuItem17 = new System.Windows.Forms.MenuItem();
            this.ExitItem = new System.Windows.Forms.MenuItem();
            this.ActionMenu = new System.Windows.Forms.MenuItem();
            this.RefreshItem = new System.Windows.Forms.MenuItem();
            this.menuItem19 = new System.Windows.Forms.MenuItem();
            this.Dhcp4Item = new System.Windows.Forms.MenuItem();
            this.Dhcp4EnabledItem = new System.Windows.Forms.MenuItem();
            this.Dhcp4ReleaseIpItem = new System.Windows.Forms.MenuItem();
            this.Dhcp4RenewIpItem = new System.Windows.Forms.MenuItem();
            this.Dhcp6Item = new System.Windows.Forms.MenuItem();
            this.Dhcp6EnabledItem = new System.Windows.Forms.MenuItem();
            this.Dhcp6ReleaseIpItem = new System.Windows.Forms.MenuItem();
            this.Dhcp6RenewItem = new System.Windows.Forms.MenuItem();
            this.menuItem20 = new System.Windows.Forms.MenuItem();
            this.DeleteItem = new System.Windows.Forms.MenuItem();
            this.OptionsMenu = new System.Windows.Forms.MenuItem();
            this.HelpMenu = new System.Windows.Forms.MenuItem();
            this.HelpTopicsItem = new System.Windows.Forms.MenuItem();
            this.CliParamsHelpItem = new System.Windows.Forms.MenuItem();
            this.menuItem4 = new System.Windows.Forms.MenuItem();
            this.CheckUpdateItem = new System.Windows.Forms.MenuItem();
            this.UpdateOuiItem = new System.Windows.Forms.MenuItem();
            this.menuItem7 = new System.Windows.Forms.MenuItem();
            this.AboutItem = new System.Windows.Forms.MenuItem();
            this.MainStatusBar = new System.Windows.Forms.StatusBar();
            this.MainTableLayoutPanel.SuspendLayout();
            this.InfoTabs.SuspendLayout();
            this.InformationPage.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.ConnectionDetailsGroup.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ConnectionsGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // MainTableLayoutPanel
            // 
            this.MainTableLayoutPanel.AutoSize = true;
            this.MainTableLayoutPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.MainTableLayoutPanel.ColumnCount = 1;
            this.MainTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.MainTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.MainTableLayoutPanel.Controls.Add(this.InfoTabs, 0, 1);
            this.MainTableLayoutPanel.Controls.Add(this.ConnectionsGrid, 0, 0);
            this.MainTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainTableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.MainTableLayoutPanel.Name = "MainTableLayoutPanel";
            this.MainTableLayoutPanel.RowCount = 2;
            this.MainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.MainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 75F));
            this.MainTableLayoutPanel.Size = new System.Drawing.Size(884, 521);
            this.MainTableLayoutPanel.TabIndex = 0;
            // 
            // InfoTabs
            // 
            this.InfoTabs.Controls.Add(this.InformationPage);
            this.InfoTabs.Controls.Add(this.IPAddressPage);
            this.InfoTabs.Controls.Add(this.PresetsPage);
            this.InfoTabs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.InfoTabs.Location = new System.Drawing.Point(3, 133);
            this.InfoTabs.Name = "InfoTabs";
            this.InfoTabs.SelectedIndex = 0;
            this.InfoTabs.Size = new System.Drawing.Size(878, 385);
            this.InfoTabs.TabIndex = 0;
            // 
            // InformationPage
            // 
            this.InformationPage.Controls.Add(this.flowLayoutPanel1);
            this.InformationPage.Location = new System.Drawing.Point(4, 22);
            this.InformationPage.Name = "InformationPage";
            this.InformationPage.Padding = new System.Windows.Forms.Padding(3);
            this.InformationPage.Size = new System.Drawing.Size(870, 359);
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
            this.flowLayoutPanel1.Size = new System.Drawing.Size(864, 353);
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
            this.IPAddressPage.Size = new System.Drawing.Size(870, 359);
            this.IPAddressPage.TabIndex = 1;
            this.IPAddressPage.Text = "IP Address";
            this.IPAddressPage.UseVisualStyleBackColor = true;
            // 
            // PresetsPage
            // 
            this.PresetsPage.Location = new System.Drawing.Point(4, 22);
            this.PresetsPage.Name = "PresetsPage";
            this.PresetsPage.Padding = new System.Windows.Forms.Padding(3);
            this.PresetsPage.Size = new System.Drawing.Size(870, 359);
            this.PresetsPage.TabIndex = 2;
            this.PresetsPage.Text = "Presets";
            this.PresetsPage.UseVisualStyleBackColor = true;
            // 
            // ConnectionsGrid
            // 
            this.ConnectionsGrid.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ConnectionsGrid.CausesValidation = false;
            this.ConnectionsGrid.CellEditUseWholeCell = false;
            this.ConnectionsGrid.DataSource = null;
            this.ConnectionsGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ConnectionsGrid.FullRowSelect = true;
            this.ConnectionsGrid.HasCollapsibleGroups = false;
            this.ConnectionsGrid.HeaderUsesThemes = true;
            this.ConnectionsGrid.HideSelection = false;
            this.ConnectionsGrid.Location = new System.Drawing.Point(3, 3);
            this.ConnectionsGrid.MinimumSize = new System.Drawing.Size(800, 2);
            this.ConnectionsGrid.MultiSelect = false;
            this.ConnectionsGrid.Name = "ConnectionsGrid";
            this.ConnectionsGrid.RenderNonEditableCheckboxesAsDisabled = true;
            this.ConnectionsGrid.ShowGroups = false;
            this.ConnectionsGrid.Size = new System.Drawing.Size(878, 124);
            this.ConnectionsGrid.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.ConnectionsGrid.SpaceBetweenGroups = 1;
            this.ConnectionsGrid.TabIndex = 1;
            this.ConnectionsGrid.UseCompatibleStateImageBehavior = false;
            this.ConnectionsGrid.UseExplorerTheme = true;
            this.ConnectionsGrid.View = System.Windows.Forms.View.Details;
            this.ConnectionsGrid.SelectedIndexChanged += new System.EventHandler(this.ConnectionsGrid_SelectedIndexChanged);
            // 
            // MainMenu
            // 
            this.MainMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.FileMenu,
            this.ActionMenu,
            this.OptionsMenu,
            this.HelpMenu});
            // 
            // FileMenu
            // 
            this.FileMenu.Index = 0;
            this.FileMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.ExportReportItem,
            this.menuItem14,
            this.OpenPresetItem,
            this.SavePresetItem,
            this.SavePresetAsItem,
            this.menuItem15,
            this.ImportPresetItem,
            this.ExportPresetItem,
            this.menuItem16,
            this.AssociateItem,
            this.menuItem17,
            this.ExitItem});
            this.FileMenu.Text = "File";
            // 
            // ExportReportItem
            // 
            this.ExportReportItem.Index = 0;
            this.ExportReportItem.Text = "Export Text Report";
            this.ExportReportItem.Click += new System.EventHandler(this.ExportReportItem_Click);
            // 
            // menuItem14
            // 
            this.menuItem14.Index = 1;
            this.menuItem14.Text = "-";
            // 
            // OpenPresetItem
            // 
            this.OpenPresetItem.Index = 2;
            this.OpenPresetItem.Shortcut = System.Windows.Forms.Shortcut.CtrlO;
            this.OpenPresetItem.Text = "Open Preset";
            this.OpenPresetItem.Click += new System.EventHandler(this.OpenPresetItem_Click);
            // 
            // SavePresetItem
            // 
            this.SavePresetItem.Index = 3;
            this.SavePresetItem.Shortcut = System.Windows.Forms.Shortcut.CtrlS;
            this.SavePresetItem.Text = "Save Preset";
            this.SavePresetItem.Click += new System.EventHandler(this.SavePresetItem_Click);
            // 
            // SavePresetAsItem
            // 
            this.SavePresetAsItem.Index = 4;
            this.SavePresetAsItem.Text = "Save Preset As";
            this.SavePresetAsItem.Click += new System.EventHandler(this.SavePresetAsItem_Click);
            // 
            // menuItem15
            // 
            this.menuItem15.Index = 5;
            this.menuItem15.Text = "-";
            // 
            // ImportPresetItem
            // 
            this.ImportPresetItem.Index = 6;
            this.ImportPresetItem.Text = "Import Preset";
            this.ImportPresetItem.Click += new System.EventHandler(this.ImportPresetItem_Click);
            // 
            // ExportPresetItem
            // 
            this.ExportPresetItem.Index = 7;
            this.ExportPresetItem.Text = "Export Preset";
            this.ExportPresetItem.Click += new System.EventHandler(this.ExportPresetItem_Click);
            // 
            // menuItem16
            // 
            this.menuItem16.Index = 8;
            this.menuItem16.Text = "-";
            // 
            // AssociateItem
            // 
            this.AssociateItem.Index = 9;
            this.AssociateItem.Text = "Associate with Preset Files (.tpf)";
            this.AssociateItem.Click += new System.EventHandler(this.AssociateItem_Click);
            // 
            // menuItem17
            // 
            this.menuItem17.Index = 10;
            this.menuItem17.Text = "-";
            // 
            // ExitItem
            // 
            this.ExitItem.Index = 11;
            this.ExitItem.Shortcut = System.Windows.Forms.Shortcut.CtrlQ;
            this.ExitItem.Text = "Exit";
            this.ExitItem.Click += new System.EventHandler(this.ExitItem_Click);
            // 
            // ActionMenu
            // 
            this.ActionMenu.Index = 1;
            this.ActionMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.RefreshItem,
            this.menuItem19,
            this.Dhcp4Item,
            this.Dhcp6Item,
            this.menuItem20,
            this.DeleteItem});
            this.ActionMenu.Text = "Action";
            // 
            // RefreshItem
            // 
            this.RefreshItem.Index = 0;
            this.RefreshItem.Shortcut = System.Windows.Forms.Shortcut.F5;
            this.RefreshItem.Text = "Refresh";
            this.RefreshItem.Click += new System.EventHandler(this.RefreshItem_Click);
            // 
            // menuItem19
            // 
            this.menuItem19.Index = 1;
            this.menuItem19.Text = "-";
            // 
            // Dhcp4Item
            // 
            this.Dhcp4Item.Index = 2;
            this.Dhcp4Item.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.Dhcp4EnabledItem,
            this.Dhcp4ReleaseIpItem,
            this.Dhcp4RenewIpItem});
            this.Dhcp4Item.Text = "DHCPv4";
            // 
            // Dhcp4EnabledItem
            // 
            this.Dhcp4EnabledItem.Index = 0;
            this.Dhcp4EnabledItem.RadioCheck = true;
            this.Dhcp4EnabledItem.Text = "DHCPv4 Enabled";
            // 
            // Dhcp4ReleaseIpItem
            // 
            this.Dhcp4ReleaseIpItem.Enabled = false;
            this.Dhcp4ReleaseIpItem.Index = 1;
            this.Dhcp4ReleaseIpItem.Text = "Release IP";
            // 
            // Dhcp4RenewIpItem
            // 
            this.Dhcp4RenewIpItem.Enabled = false;
            this.Dhcp4RenewIpItem.Index = 2;
            this.Dhcp4RenewIpItem.Text = "Renew IP";
            // 
            // Dhcp6Item
            // 
            this.Dhcp6Item.Index = 3;
            this.Dhcp6Item.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.Dhcp6EnabledItem,
            this.Dhcp6ReleaseIpItem,
            this.Dhcp6RenewItem});
            this.Dhcp6Item.Text = "DHCPv6";
            // 
            // Dhcp6EnabledItem
            // 
            this.Dhcp6EnabledItem.Index = 0;
            this.Dhcp6EnabledItem.RadioCheck = true;
            this.Dhcp6EnabledItem.Text = "DHCPv6 Enabled";
            // 
            // Dhcp6ReleaseIpItem
            // 
            this.Dhcp6ReleaseIpItem.Enabled = false;
            this.Dhcp6ReleaseIpItem.Index = 1;
            this.Dhcp6ReleaseIpItem.Text = "Release IP";
            // 
            // Dhcp6RenewItem
            // 
            this.Dhcp6RenewItem.Enabled = false;
            this.Dhcp6RenewItem.Index = 2;
            this.Dhcp6RenewItem.Text = "Renew IP";
            // 
            // menuItem20
            // 
            this.menuItem20.Index = 4;
            this.menuItem20.Text = "-";
            // 
            // DeleteItem
            // 
            this.DeleteItem.Index = 5;
            this.DeleteItem.Text = "Delete Network Adapter from Registry";
            this.DeleteItem.Click += new System.EventHandler(this.DeleteItem_Click);
            // 
            // OptionsMenu
            // 
            this.OptionsMenu.Index = 2;
            this.OptionsMenu.Text = "Options";
            this.OptionsMenu.Click += new System.EventHandler(this.OptionsMenu_Click);
            // 
            // HelpMenu
            // 
            this.HelpMenu.Index = 3;
            this.HelpMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.HelpTopicsItem,
            this.CliParamsHelpItem,
            this.menuItem4,
            this.CheckUpdateItem,
            this.UpdateOuiItem,
            this.menuItem7,
            this.AboutItem});
            this.HelpMenu.Text = "Help";
            // 
            // HelpTopicsItem
            // 
            this.HelpTopicsItem.Index = 0;
            this.HelpTopicsItem.Shortcut = System.Windows.Forms.Shortcut.F1;
            this.HelpTopicsItem.Text = "Help Topics";
            this.HelpTopicsItem.Click += new System.EventHandler(this.HelpTopicsItem_Click);
            // 
            // CliParamsHelpItem
            // 
            this.CliParamsHelpItem.Index = 1;
            this.CliParamsHelpItem.Text = "Command Line Parameters Help";
            this.CliParamsHelpItem.Click += new System.EventHandler(this.CliParamsHelpItem_Click);
            // 
            // menuItem4
            // 
            this.menuItem4.Index = 2;
            this.menuItem4.Text = "-";
            // 
            // CheckUpdateItem
            // 
            this.CheckUpdateItem.Index = 3;
            this.CheckUpdateItem.Text = "Check For Software Updates";
            this.CheckUpdateItem.Click += new System.EventHandler(this.CheckUpdateItem_Click);
            // 
            // UpdateOuiItem
            // 
            this.UpdateOuiItem.Index = 4;
            this.UpdateOuiItem.Text = "Update Vendors List (OUI) from IEEE";
            this.UpdateOuiItem.Click += new System.EventHandler(this.UpdateOuiItem_Click);
            // 
            // menuItem7
            // 
            this.menuItem7.Index = 5;
            this.menuItem7.Text = "-";
            // 
            // AboutItem
            // 
            this.AboutItem.Index = 6;
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
            this.Menu = this.MainMenu;
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
        private System.Windows.Forms.MainMenu MainMenu;
        private System.Windows.Forms.MenuItem FileMenu;
        private System.Windows.Forms.MenuItem ExportReportItem;
        private System.Windows.Forms.MenuItem OpenPresetItem;
        private System.Windows.Forms.MenuItem SavePresetItem;
        private System.Windows.Forms.MenuItem SavePresetAsItem;
        private System.Windows.Forms.MenuItem ImportPresetItem;
        private System.Windows.Forms.MenuItem ExportPresetItem;
        private System.Windows.Forms.MenuItem ActionMenu;
        private System.Windows.Forms.MenuItem OptionsMenu;
        private System.Windows.Forms.MenuItem HelpMenu;
        private System.Windows.Forms.MenuItem menuItem14;
        private System.Windows.Forms.MenuItem menuItem15;
        private System.Windows.Forms.MenuItem menuItem16;
        private System.Windows.Forms.MenuItem AssociateItem;
        private System.Windows.Forms.MenuItem menuItem17;
        private System.Windows.Forms.MenuItem ExitItem;
        private System.Windows.Forms.MenuItem RefreshItem;
        private System.Windows.Forms.MenuItem menuItem19;
        private System.Windows.Forms.MenuItem Dhcp4Item;
        private System.Windows.Forms.MenuItem Dhcp6Item;
        private System.Windows.Forms.MenuItem menuItem20;
        private System.Windows.Forms.MenuItem DeleteItem;
        private System.Windows.Forms.MenuItem Dhcp4EnabledItem;
        private System.Windows.Forms.MenuItem Dhcp4ReleaseIpItem;
        private System.Windows.Forms.MenuItem Dhcp4RenewIpItem;
        private System.Windows.Forms.MenuItem Dhcp6EnabledItem;
        private System.Windows.Forms.MenuItem Dhcp6ReleaseIpItem;
        private System.Windows.Forms.MenuItem Dhcp6RenewItem;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.GroupBox ConnectionDetailsGroup;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox ChangeMacAddressGroup;
        private System.Windows.Forms.GroupBox PerformanceCounterGroup;
        private System.Windows.Forms.Label ConnectionNameLabel;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label DeviceLabel;
        private System.Windows.Forms.StatusBar MainStatusBar;
        private System.Windows.Forms.MenuItem HelpTopicsItem;
        private System.Windows.Forms.MenuItem CliParamsHelpItem;
        private System.Windows.Forms.MenuItem menuItem4;
        private System.Windows.Forms.MenuItem CheckUpdateItem;
        private System.Windows.Forms.MenuItem UpdateOuiItem;
        private System.Windows.Forms.MenuItem menuItem7;
        private System.Windows.Forms.MenuItem AboutItem;
    }
}

