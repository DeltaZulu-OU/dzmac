#nullable enable

namespace Dzmac.Forms
{
    internal sealed partial class PresetEditorDialog
    {
        private System.ComponentModel.IContainer? components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components is not null))
            {
                components.Dispose();
            }

            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.MainLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.EditorTabControl = new System.Windows.Forms.TabControl();
            this.MacTabPage = new System.Windows.Forms.TabPage();
            this.MacLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.PresetNameGroupBox = new System.Windows.Forms.GroupBox();
            this.PresetNameFlowPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.PresetNameLabel = new System.Windows.Forms.Label();
            this._presetNameTextBox = new System.Windows.Forms.TextBox();
            this.MacGroupBox = new System.Windows.Forms.GroupBox();
            this.MacInnerLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this._includeMacCheckBox = new System.Windows.Forms.CheckBox();
            this._useRandomMacRadio = new System.Windows.Forms.RadioButton();
            this._useRandom02MacRadio = new System.Windows.Forms.RadioButton();
            this._useOriginalMacRadio = new System.Windows.Forms.RadioButton();
            this.CustomMacFlowPanel = new System.Windows.Forms.FlowLayoutPanel();
            this._useCustomMacRadio = new System.Windows.Forms.RadioButton();
            this._customMacTextBox = new System.Windows.Forms.TextBox();
            this.ProxyGroupBox = new System.Windows.Forms.GroupBox();
            this.ProxyPlaceholderLabel = new System.Windows.Forms.Label();
            this.Ipv4TabPage = new System.Windows.Forms.TabPage();
            this.Ipv4LayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.Ipv4LeftGroupBox = new System.Windows.Forms.GroupBox();
            this.Ipv4LeftLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this._includeIpv4CheckBox = new System.Windows.Forms.CheckBox();
            this._dhcpIpv4CheckBox = new System.Windows.Forms.CheckBox();
            this._ipv4AddressCheckBox = new System.Windows.Forms.CheckBox();
            this._ipv4GatewayCheckBox = new System.Windows.Forms.CheckBox();
            this._ipv4DnsCheckBox = new System.Windows.Forms.CheckBox();
            this._ipv4RightGroup = new System.Windows.Forms.GroupBox();
            this.Ipv4RightLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this._ipv4AddressGroup = new System.Windows.Forms.GroupBox();
            this._ipv4GatewayGroup = new System.Windows.Forms.GroupBox();
            this._ipv4DnsGroup = new System.Windows.Forms.GroupBox();
            this.Ipv6TabPage = new System.Windows.Forms.TabPage();
            this.Ipv6PlaceholderLabel = new System.Windows.Forms.Label();
            this.ButtonFlowPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.CancelDialogButton = new System.Windows.Forms.Button();
            this.SaveButton = new System.Windows.Forms.Button();
            this._ipv4AddressGrid = CreateIpv4Grid(("IP Address", 0.68F), ("Subnet Mask", 0.32F));
            this._ipv4GatewayGrid = CreateIpv4Grid(("Gateway", 0.72F), ("Metric", 0.28F));
            this._ipv4DnsGrid = CreateIpv4Grid(("DNS Server", 1F));
            this.MainLayoutPanel.SuspendLayout();
            this.EditorTabControl.SuspendLayout();
            this.MacTabPage.SuspendLayout();
            this.MacLayoutPanel.SuspendLayout();
            this.PresetNameGroupBox.SuspendLayout();
            this.PresetNameFlowPanel.SuspendLayout();
            this.MacGroupBox.SuspendLayout();
            this.MacInnerLayoutPanel.SuspendLayout();
            this.CustomMacFlowPanel.SuspendLayout();
            this.ProxyGroupBox.SuspendLayout();
            this.Ipv4TabPage.SuspendLayout();
            this.Ipv4LayoutPanel.SuspendLayout();
            this.Ipv4LeftGroupBox.SuspendLayout();
            this.Ipv4LeftLayoutPanel.SuspendLayout();
            this._ipv4RightGroup.SuspendLayout();
            this.Ipv4RightLayoutPanel.SuspendLayout();
            this._ipv4AddressGroup.SuspendLayout();
            this._ipv4GatewayGroup.SuspendLayout();
            this._ipv4DnsGroup.SuspendLayout();
            this.Ipv6TabPage.SuspendLayout();
            this.ButtonFlowPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._ipv4AddressGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._ipv4GatewayGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._ipv4DnsGrid)).BeginInit();
            this.SuspendLayout();
            //
            // MainLayoutPanel
            //
            this.MainLayoutPanel.ColumnCount = 1;
            this.MainLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.MainLayoutPanel.Controls.Add(this.EditorTabControl, 0, 0);
            this.MainLayoutPanel.Controls.Add(this.ButtonFlowPanel, 0, 1);
            this.MainLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainLayoutPanel.Location = new System.Drawing.Point(8, 8);
            this.MainLayoutPanel.Name = "MainLayoutPanel";
            this.MainLayoutPanel.RowCount = 2;
            this.MainLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.MainLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 44F));
            this.MainLayoutPanel.Size = new System.Drawing.Size(744, 454);
            this.MainLayoutPanel.TabIndex = 0;
            //
            // EditorTabControl
            //
            this.EditorTabControl.Controls.Add(this.MacTabPage);
            this.EditorTabControl.Controls.Add(this.Ipv4TabPage);
            this.EditorTabControl.Controls.Add(this.Ipv6TabPage);
            this.EditorTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.EditorTabControl.Location = new System.Drawing.Point(3, 3);
            this.EditorTabControl.Name = "EditorTabControl";
            this.EditorTabControl.SelectedIndex = 0;
            this.EditorTabControl.Size = new System.Drawing.Size(738, 404);
            this.EditorTabControl.TabIndex = 0;
            //
            // MacTabPage
            //
            this.MacTabPage.Controls.Add(this.MacLayoutPanel);
            this.MacTabPage.Location = new System.Drawing.Point(4, 22);
            this.MacTabPage.Name = "MacTabPage";
            this.MacTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.MacTabPage.Size = new System.Drawing.Size(730, 378);
            this.MacTabPage.TabIndex = 0;
            this.MacTabPage.Text = "MAC Address && HTTP Proxy";
            this.MacTabPage.UseVisualStyleBackColor = true;
            //
            // MacLayoutPanel
            //
            this.MacLayoutPanel.ColumnCount = 1;
            this.MacLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.MacLayoutPanel.Controls.Add(this.PresetNameGroupBox, 0, 0);
            this.MacLayoutPanel.Controls.Add(this.MacGroupBox, 0, 1);
            this.MacLayoutPanel.Controls.Add(this.ProxyGroupBox, 0, 2);
            this.MacLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MacLayoutPanel.Location = new System.Drawing.Point(3, 3);
            this.MacLayoutPanel.Name = "MacLayoutPanel";
            this.MacLayoutPanel.Padding = new System.Windows.Forms.Padding(6);
            this.MacLayoutPanel.RowCount = 3;
            this.MacLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 64F));
            this.MacLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 132F));
            this.MacLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.MacLayoutPanel.Size = new System.Drawing.Size(724, 372);
            this.MacLayoutPanel.TabIndex = 0;
            //
            // PresetNameGroupBox
            //
            this.PresetNameGroupBox.Controls.Add(this.PresetNameFlowPanel);
            this.PresetNameGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PresetNameGroupBox.Location = new System.Drawing.Point(9, 9);
            this.PresetNameGroupBox.Name = "PresetNameGroupBox";
            this.PresetNameGroupBox.Size = new System.Drawing.Size(706, 58);
            this.PresetNameGroupBox.TabIndex = 0;
            this.PresetNameGroupBox.TabStop = false;
            this.PresetNameGroupBox.Text = "Preset Name";
            //
            // PresetNameFlowPanel
            //
            this.PresetNameFlowPanel.Controls.Add(this.PresetNameLabel);
            this.PresetNameFlowPanel.Controls.Add(this._presetNameTextBox);
            this.PresetNameFlowPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PresetNameFlowPanel.FlowDirection = System.Windows.Forms.FlowDirection.LeftToRight;
            this.PresetNameFlowPanel.Location = new System.Drawing.Point(3, 16);
            this.PresetNameFlowPanel.Name = "PresetNameFlowPanel";
            this.PresetNameFlowPanel.Padding = new System.Windows.Forms.Padding(8, 10, 8, 8);
            this.PresetNameFlowPanel.Size = new System.Drawing.Size(700, 39);
            this.PresetNameFlowPanel.TabIndex = 0;
            //
            // PresetNameLabel
            //
            this.PresetNameLabel.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.PresetNameLabel.AutoSize = true;
            this.PresetNameLabel.Location = new System.Drawing.Point(11, 13);
            this.PresetNameLabel.Name = "PresetNameLabel";
            this.PresetNameLabel.Size = new System.Drawing.Size(70, 13);
            this.PresetNameLabel.TabIndex = 0;
            this.PresetNameLabel.Text = "Preset Name:";
            //
            // _presetNameTextBox
            //
            this._presetNameTextBox.Location = new System.Drawing.Point(87, 13);
            this._presetNameTextBox.Name = "_presetNameTextBox";
            this._presetNameTextBox.Size = new System.Drawing.Size(280, 20);
            this._presetNameTextBox.TabIndex = 1;
            //
            // MacGroupBox
            //
            this.MacGroupBox.Controls.Add(this.MacInnerLayoutPanel);
            this.MacGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MacGroupBox.Location = new System.Drawing.Point(9, 73);
            this.MacGroupBox.Name = "MacGroupBox";
            this.MacGroupBox.Size = new System.Drawing.Size(706, 126);
            this.MacGroupBox.TabIndex = 1;
            this.MacGroupBox.TabStop = false;
            this.MacGroupBox.Text = "MAC Address";
            //
            // MacInnerLayoutPanel
            //
            this.MacInnerLayoutPanel.ColumnCount = 1;
            this.MacInnerLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.MacInnerLayoutPanel.Controls.Add(this._includeMacCheckBox, 0, 0);
            this.MacInnerLayoutPanel.Controls.Add(this._useRandomMacRadio, 0, 1);
            this.MacInnerLayoutPanel.Controls.Add(this._useRandom02MacRadio, 0, 2);
            this.MacInnerLayoutPanel.Controls.Add(this._useOriginalMacRadio, 0, 3);
            this.MacInnerLayoutPanel.Controls.Add(this.CustomMacFlowPanel, 0, 4);
            this.MacInnerLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MacInnerLayoutPanel.Location = new System.Drawing.Point(3, 16);
            this.MacInnerLayoutPanel.Name = "MacInnerLayoutPanel";
            this.MacInnerLayoutPanel.Padding = new System.Windows.Forms.Padding(8);
            this.MacInnerLayoutPanel.RowCount = 5;
            this.MacInnerLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.MacInnerLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.MacInnerLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.MacInnerLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.MacInnerLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.MacInnerLayoutPanel.Size = new System.Drawing.Size(700, 107);
            this.MacInnerLayoutPanel.TabIndex = 0;
            //
            // _includeMacCheckBox
            //
            this._includeMacCheckBox.AutoSize = true;
            this._includeMacCheckBox.Location = new System.Drawing.Point(11, 11);
            this._includeMacCheckBox.Name = "_includeMacCheckBox";
            this._includeMacCheckBox.Size = new System.Drawing.Size(127, 17);
            this._includeMacCheckBox.TabIndex = 0;
            this._includeMacCheckBox.Text = "Include MAC Address";
            this._includeMacCheckBox.UseVisualStyleBackColor = true;
            //
            // _useRandomMacRadio
            //
            this._useRandomMacRadio.AutoSize = true;
            this._useRandomMacRadio.Location = new System.Drawing.Point(28, 35);
            this._useRandomMacRadio.Margin = new System.Windows.Forms.Padding(20, 0, 0, 0);
            this._useRandomMacRadio.Name = "_useRandomMacRadio";
            this._useRandomMacRadio.Size = new System.Drawing.Size(164, 17);
            this._useRandomMacRadio.TabIndex = 1;
            this._useRandomMacRadio.TabStop = true;
            this._useRandomMacRadio.Text = "Use Random MAC Address";
            this._useRandomMacRadio.UseVisualStyleBackColor = true;
            //
            // _useRandom02MacRadio
            //
            this._useRandom02MacRadio.AutoSize = true;
            this._useRandom02MacRadio.Location = new System.Drawing.Point(28, 59);
            this._useRandom02MacRadio.Margin = new System.Windows.Forms.Padding(20, 0, 0, 0);
            this._useRandom02MacRadio.Name = "_useRandom02MacRadio";
            this._useRandom02MacRadio.Size = new System.Drawing.Size(253, 17);
            this._useRandom02MacRadio.TabIndex = 2;
            this._useRandom02MacRadio.TabStop = true;
            this._useRandom02MacRadio.Text = "Use Random MAC Address (0x02 first octet)";
            this._useRandom02MacRadio.UseVisualStyleBackColor = true;
            //
            // _useOriginalMacRadio
            //
            this._useOriginalMacRadio.AutoSize = true;
            this._useOriginalMacRadio.Location = new System.Drawing.Point(28, 83);
            this._useOriginalMacRadio.Margin = new System.Windows.Forms.Padding(20, 0, 0, 0);
            this._useOriginalMacRadio.Name = "_useOriginalMacRadio";
            this._useOriginalMacRadio.Size = new System.Drawing.Size(156, 17);
            this._useOriginalMacRadio.TabIndex = 3;
            this._useOriginalMacRadio.TabStop = true;
            this._useOriginalMacRadio.Text = "Use Original MAC Address";
            this._useOriginalMacRadio.UseVisualStyleBackColor = true;
            //
            // CustomMacFlowPanel
            //
            this.CustomMacFlowPanel.Controls.Add(this._useCustomMacRadio);
            this.CustomMacFlowPanel.Controls.Add(this._customMacTextBox);
            this.CustomMacFlowPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CustomMacFlowPanel.FlowDirection = System.Windows.Forms.FlowDirection.LeftToRight;
            this.CustomMacFlowPanel.Location = new System.Drawing.Point(11, 107);
            this.CustomMacFlowPanel.Name = "CustomMacFlowPanel";
            this.CustomMacFlowPanel.Size = new System.Drawing.Size(686, 1);
            this.CustomMacFlowPanel.TabIndex = 4;
            //
            // _useCustomMacRadio
            //
            this._useCustomMacRadio.AutoSize = true;
            this._useCustomMacRadio.Location = new System.Drawing.Point(20, 2);
            this._useCustomMacRadio.Margin = new System.Windows.Forms.Padding(20, 2, 0, 2);
            this._useCustomMacRadio.Name = "_useCustomMacRadio";
            this._useCustomMacRadio.Size = new System.Drawing.Size(84, 17);
            this._useCustomMacRadio.TabIndex = 0;
            this._useCustomMacRadio.TabStop = true;
            this._useCustomMacRadio.Text = "Use Custom";
            this._useCustomMacRadio.UseVisualStyleBackColor = true;
            //
            // _customMacTextBox
            //
            this._customMacTextBox.Location = new System.Drawing.Point(107, 2);
            this._customMacTextBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 3);
            this._customMacTextBox.Name = "_customMacTextBox";
            this._customMacTextBox.Size = new System.Drawing.Size(220, 20);
            this._customMacTextBox.TabIndex = 1;
            //
            // ProxyGroupBox
            //
            this.ProxyGroupBox.Controls.Add(this.ProxyPlaceholderLabel);
            this.ProxyGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ProxyGroupBox.Location = new System.Drawing.Point(9, 205);
            this.ProxyGroupBox.Name = "ProxyGroupBox";
            this.ProxyGroupBox.Size = new System.Drawing.Size(706, 158);
            this.ProxyGroupBox.TabIndex = 2;
            this.ProxyGroupBox.TabStop = false;
            this.ProxyGroupBox.Text = "Internet Explorer HTTP Proxy";
            //
            // ProxyPlaceholderLabel
            //
            this.ProxyPlaceholderLabel.AutoSize = true;
            this.ProxyPlaceholderLabel.ForeColor = System.Drawing.SystemColors.GrayText;
            this.ProxyPlaceholderLabel.Location = new System.Drawing.Point(14, 28);
            this.ProxyPlaceholderLabel.Name = "ProxyPlaceholderLabel";
            this.ProxyPlaceholderLabel.Size = new System.Drawing.Size(250, 13);
            this.ProxyPlaceholderLabel.TabIndex = 0;
            this.ProxyPlaceholderLabel.Text = "Proxy settings are not supported in this release.";
            //
            // Ipv4TabPage
            //
            this.Ipv4TabPage.Controls.Add(this.Ipv4LayoutPanel);
            this.Ipv4TabPage.Location = new System.Drawing.Point(4, 22);
            this.Ipv4TabPage.Name = "Ipv4TabPage";
            this.Ipv4TabPage.Padding = new System.Windows.Forms.Padding(3);
            this.Ipv4TabPage.Size = new System.Drawing.Size(730, 378);
            this.Ipv4TabPage.TabIndex = 1;
            this.Ipv4TabPage.Text = "Internet Protocol v4";
            this.Ipv4TabPage.UseVisualStyleBackColor = true;
            //
            // Ipv4LayoutPanel
            //
            this.Ipv4LayoutPanel.ColumnCount = 2;
            this.Ipv4LayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 230F));
            this.Ipv4LayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.Ipv4LayoutPanel.Controls.Add(this.Ipv4LeftGroupBox, 0, 0);
            this.Ipv4LayoutPanel.Controls.Add(this._ipv4RightGroup, 1, 0);
            this.Ipv4LayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Ipv4LayoutPanel.Location = new System.Drawing.Point(3, 3);
            this.Ipv4LayoutPanel.Name = "Ipv4LayoutPanel";
            this.Ipv4LayoutPanel.Padding = new System.Windows.Forms.Padding(8);
            this.Ipv4LayoutPanel.Size = new System.Drawing.Size(724, 372);
            this.Ipv4LayoutPanel.TabIndex = 0;
            //
            // Ipv4LeftGroupBox
            //
            this.Ipv4LeftGroupBox.Controls.Add(this.Ipv4LeftLayoutPanel);
            this.Ipv4LeftGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Ipv4LeftGroupBox.Location = new System.Drawing.Point(11, 11);
            this.Ipv4LeftGroupBox.Name = "Ipv4LeftGroupBox";
            this.Ipv4LeftGroupBox.Size = new System.Drawing.Size(224, 350);
            this.Ipv4LeftGroupBox.TabIndex = 0;
            this.Ipv4LeftGroupBox.TabStop = false;
            this.Ipv4LeftGroupBox.Text = "Internet Protocol v4 Parameters";
            //
            // Ipv4LeftLayoutPanel
            //
            this.Ipv4LeftLayoutPanel.ColumnCount = 1;
            this.Ipv4LeftLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.Ipv4LeftLayoutPanel.Controls.Add(this._includeIpv4CheckBox, 0, 0);
            this.Ipv4LeftLayoutPanel.Controls.Add(this._dhcpIpv4CheckBox, 0, 1);
            this.Ipv4LeftLayoutPanel.Controls.Add(this._ipv4AddressCheckBox, 0, 2);
            this.Ipv4LeftLayoutPanel.Controls.Add(this._ipv4GatewayCheckBox, 0, 3);
            this.Ipv4LeftLayoutPanel.Controls.Add(this._ipv4DnsCheckBox, 0, 4);
            this.Ipv4LeftLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Ipv4LeftLayoutPanel.Location = new System.Drawing.Point(3, 16);
            this.Ipv4LeftLayoutPanel.Name = "Ipv4LeftLayoutPanel";
            this.Ipv4LeftLayoutPanel.Padding = new System.Windows.Forms.Padding(8);
            this.Ipv4LeftLayoutPanel.RowCount = 5;
            this.Ipv4LeftLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.Ipv4LeftLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.Ipv4LeftLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.Ipv4LeftLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.Ipv4LeftLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.Ipv4LeftLayoutPanel.Size = new System.Drawing.Size(218, 331);
            this.Ipv4LeftLayoutPanel.TabIndex = 0;
            //
            // _includeIpv4CheckBox
            //
            this._includeIpv4CheckBox.AutoSize = true;
            this._includeIpv4CheckBox.Location = new System.Drawing.Point(11, 11);
            this._includeIpv4CheckBox.Name = "_includeIpv4CheckBox";
            this._includeIpv4CheckBox.Size = new System.Drawing.Size(161, 17);
            this._includeIpv4CheckBox.TabIndex = 0;
            this._includeIpv4CheckBox.Text = "Include Internet Protocol v4";
            this._includeIpv4CheckBox.UseVisualStyleBackColor = true;
            //
            // _dhcpIpv4CheckBox
            //
            this._dhcpIpv4CheckBox.AutoSize = true;
            this._dhcpIpv4CheckBox.Location = new System.Drawing.Point(28, 35);
            this._dhcpIpv4CheckBox.Margin = new System.Windows.Forms.Padding(20, 0, 0, 0);
            this._dhcpIpv4CheckBox.Name = "_dhcpIpv4CheckBox";
            this._dhcpIpv4CheckBox.Size = new System.Drawing.Size(67, 17);
            this._dhcpIpv4CheckBox.TabIndex = 1;
            this._dhcpIpv4CheckBox.Text = "DHCPv4";
            this._dhcpIpv4CheckBox.UseVisualStyleBackColor = true;
            //
            // _ipv4AddressCheckBox
            //
            this._ipv4AddressCheckBox.AutoSize = true;
            this._ipv4AddressCheckBox.Location = new System.Drawing.Point(28, 59);
            this._ipv4AddressCheckBox.Margin = new System.Windows.Forms.Padding(20, 0, 0, 0);
            this._ipv4AddressCheckBox.Name = "_ipv4AddressCheckBox";
            this._ipv4AddressCheckBox.Size = new System.Drawing.Size(92, 17);
            this._ipv4AddressCheckBox.TabIndex = 2;
            this._ipv4AddressCheckBox.Text = "IPv4 Address";
            this._ipv4AddressCheckBox.UseVisualStyleBackColor = true;
            //
            // _ipv4GatewayCheckBox
            //
            this._ipv4GatewayCheckBox.AutoSize = true;
            this._ipv4GatewayCheckBox.Location = new System.Drawing.Point(28, 83);
            this._ipv4GatewayCheckBox.Margin = new System.Windows.Forms.Padding(20, 0, 0, 0);
            this._ipv4GatewayCheckBox.Name = "_ipv4GatewayCheckBox";
            this._ipv4GatewayCheckBox.Size = new System.Drawing.Size(92, 17);
            this._ipv4GatewayCheckBox.TabIndex = 3;
            this._ipv4GatewayCheckBox.Text = "IPv4 Gateway";
            this._ipv4GatewayCheckBox.UseVisualStyleBackColor = true;
            //
            // _ipv4DnsCheckBox
            //
            this._ipv4DnsCheckBox.AutoSize = true;
            this._ipv4DnsCheckBox.Location = new System.Drawing.Point(28, 107);
            this._ipv4DnsCheckBox.Margin = new System.Windows.Forms.Padding(20, 0, 0, 0);
            this._ipv4DnsCheckBox.Name = "_ipv4DnsCheckBox";
            this._ipv4DnsCheckBox.Size = new System.Drawing.Size(106, 17);
            this._ipv4DnsCheckBox.TabIndex = 4;
            this._ipv4DnsCheckBox.Text = "IPv4 DNS Server";
            this._ipv4DnsCheckBox.UseVisualStyleBackColor = true;
            //
            // _ipv4RightGroup
            //
            this._ipv4RightGroup.Controls.Add(this.Ipv4RightLayoutPanel);
            this._ipv4RightGroup.Dock = System.Windows.Forms.DockStyle.Fill;
            this._ipv4RightGroup.Location = new System.Drawing.Point(241, 11);
            this._ipv4RightGroup.Name = "_ipv4RightGroup";
            this._ipv4RightGroup.Size = new System.Drawing.Size(472, 350);
            this._ipv4RightGroup.TabIndex = 1;
            this._ipv4RightGroup.TabStop = false;
            this._ipv4RightGroup.Text = "Internet Protocol v4";
            //
            // Ipv4RightLayoutPanel
            //
            this.Ipv4RightLayoutPanel.ColumnCount = 1;
            this.Ipv4RightLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.Ipv4RightLayoutPanel.Controls.Add(this._ipv4AddressGroup, 0, 0);
            this.Ipv4RightLayoutPanel.Controls.Add(this._ipv4GatewayGroup, 0, 1);
            this.Ipv4RightLayoutPanel.Controls.Add(this._ipv4DnsGroup, 0, 2);
            this.Ipv4RightLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Ipv4RightLayoutPanel.Location = new System.Drawing.Point(3, 16);
            this.Ipv4RightLayoutPanel.Name = "Ipv4RightLayoutPanel";
            this.Ipv4RightLayoutPanel.Padding = new System.Windows.Forms.Padding(8);
            this.Ipv4RightLayoutPanel.RowCount = 3;
            this.Ipv4RightLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.3F));
            this.Ipv4RightLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.3F));
            this.Ipv4RightLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.4F));
            this.Ipv4RightLayoutPanel.Size = new System.Drawing.Size(466, 331);
            this.Ipv4RightLayoutPanel.TabIndex = 0;
            //
            // _ipv4AddressGroup
            //
            this._ipv4AddressGroup.Controls.Add(this._ipv4AddressGrid);
            this._ipv4AddressGroup.Dock = System.Windows.Forms.DockStyle.Fill;
            this._ipv4AddressGroup.Location = new System.Drawing.Point(11, 11);
            this._ipv4AddressGroup.Name = "_ipv4AddressGroup";
            this._ipv4AddressGroup.Size = new System.Drawing.Size(444, 100);
            this._ipv4AddressGroup.TabIndex = 0;
            this._ipv4AddressGroup.TabStop = false;
            //
            // _ipv4GatewayGroup
            //
            this._ipv4GatewayGroup.Controls.Add(this._ipv4GatewayGrid);
            this._ipv4GatewayGroup.Dock = System.Windows.Forms.DockStyle.Fill;
            this._ipv4GatewayGroup.Location = new System.Drawing.Point(11, 117);
            this._ipv4GatewayGroup.Name = "_ipv4GatewayGroup";
            this._ipv4GatewayGroup.Size = new System.Drawing.Size(444, 100);
            this._ipv4GatewayGroup.TabIndex = 1;
            this._ipv4GatewayGroup.TabStop = false;
            //
            // _ipv4DnsGroup
            //
            this._ipv4DnsGroup.Controls.Add(this._ipv4DnsGrid);
            this._ipv4DnsGroup.Dock = System.Windows.Forms.DockStyle.Fill;
            this._ipv4DnsGroup.Location = new System.Drawing.Point(11, 223);
            this._ipv4DnsGroup.Name = "_ipv4DnsGroup";
            this._ipv4DnsGroup.Size = new System.Drawing.Size(444, 97);
            this._ipv4DnsGroup.TabIndex = 2;
            this._ipv4DnsGroup.TabStop = false;
            //
            // Ipv6TabPage
            //
            this.Ipv6TabPage.Controls.Add(this.Ipv6PlaceholderLabel);
            this.Ipv6TabPage.Location = new System.Drawing.Point(4, 22);
            this.Ipv6TabPage.Name = "Ipv6TabPage";
            this.Ipv6TabPage.Padding = new System.Windows.Forms.Padding(3);
            this.Ipv6TabPage.Size = new System.Drawing.Size(730, 378);
            this.Ipv6TabPage.TabIndex = 2;
            this.Ipv6TabPage.Text = "Internet Protocol v6";
            this.Ipv6TabPage.UseVisualStyleBackColor = true;
            //
            // Ipv6PlaceholderLabel
            //
            this.Ipv6PlaceholderLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Ipv6PlaceholderLabel.ForeColor = System.Drawing.SystemColors.GrayText;
            this.Ipv6PlaceholderLabel.Location = new System.Drawing.Point(3, 3);
            this.Ipv6PlaceholderLabel.Name = "Ipv6PlaceholderLabel";
            this.Ipv6PlaceholderLabel.Size = new System.Drawing.Size(724, 372);
            this.Ipv6PlaceholderLabel.TabIndex = 0;
            this.Ipv6PlaceholderLabel.Text = "IPv6 preset editing UI is included for compatibility and will be enabled in a fu" +
    "ture update.";
            this.Ipv6PlaceholderLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            //
            // ButtonFlowPanel
            //
            this.ButtonFlowPanel.Controls.Add(this.CancelDialogButton);
            this.ButtonFlowPanel.Controls.Add(this.SaveButton);
            this.ButtonFlowPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ButtonFlowPanel.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.ButtonFlowPanel.Location = new System.Drawing.Point(3, 413);
            this.ButtonFlowPanel.Name = "ButtonFlowPanel";
            this.ButtonFlowPanel.Padding = new System.Windows.Forms.Padding(0, 8, 0, 0);
            this.ButtonFlowPanel.Size = new System.Drawing.Size(738, 38);
            this.ButtonFlowPanel.TabIndex = 1;
            //
            // CancelDialogButton
            //
            this.CancelDialogButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CancelDialogButton.Location = new System.Drawing.Point(649, 11);
            this.CancelDialogButton.Name = "CancelDialogButton";
            this.CancelDialogButton.Size = new System.Drawing.Size(86, 23);
            this.CancelDialogButton.TabIndex = 0;
            this.CancelDialogButton.Text = "Cancel";
            this.CancelDialogButton.UseVisualStyleBackColor = true;
            //
            // SaveButton
            //
            this.SaveButton.Location = new System.Drawing.Point(557, 11);
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.Size = new System.Drawing.Size(86, 23);
            this.SaveButton.TabIndex = 1;
            this.SaveButton.Text = "Save";
            this.SaveButton.UseVisualStyleBackColor = true;
            //
            // _ipv4AddressGrid
            //
            this._ipv4AddressGrid.AllowUserToAddRows = false;
            this._ipv4AddressGrid.AllowUserToDeleteRows = false;
            this._ipv4AddressGrid.AllowUserToResizeRows = false;
            this._ipv4AddressGrid.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this._ipv4AddressGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this._ipv4AddressGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this._ipv4AddressGrid.Location = new System.Drawing.Point(3, 16);
            this._ipv4AddressGrid.MultiSelect = false;
            this._ipv4AddressGrid.Name = "_ipv4AddressGrid";
            this._ipv4AddressGrid.RowHeadersVisible = false;
            this._ipv4AddressGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this._ipv4AddressGrid.Size = new System.Drawing.Size(438, 81);
            this._ipv4AddressGrid.TabIndex = 0;
            //
            // _ipv4GatewayGrid
            //
            this._ipv4GatewayGrid.AllowUserToAddRows = false;
            this._ipv4GatewayGrid.AllowUserToDeleteRows = false;
            this._ipv4GatewayGrid.AllowUserToResizeRows = false;
            this._ipv4GatewayGrid.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this._ipv4GatewayGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this._ipv4GatewayGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this._ipv4GatewayGrid.Location = new System.Drawing.Point(3, 16);
            this._ipv4GatewayGrid.MultiSelect = false;
            this._ipv4GatewayGrid.Name = "_ipv4GatewayGrid";
            this._ipv4GatewayGrid.RowHeadersVisible = false;
            this._ipv4GatewayGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this._ipv4GatewayGrid.Size = new System.Drawing.Size(438, 81);
            this._ipv4GatewayGrid.TabIndex = 0;
            //
            // _ipv4DnsGrid
            //
            this._ipv4DnsGrid.AllowUserToAddRows = false;
            this._ipv4DnsGrid.AllowUserToDeleteRows = false;
            this._ipv4DnsGrid.AllowUserToResizeRows = false;
            this._ipv4DnsGrid.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this._ipv4DnsGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this._ipv4DnsGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this._ipv4DnsGrid.Location = new System.Drawing.Point(3, 16);
            this._ipv4DnsGrid.MultiSelect = false;
            this._ipv4DnsGrid.Name = "_ipv4DnsGrid";
            this._ipv4DnsGrid.RowHeadersVisible = false;
            this._ipv4DnsGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this._ipv4DnsGrid.Size = new System.Drawing.Size(438, 78);
            this._ipv4DnsGrid.TabIndex = 0;
            //
            // PresetEditorDialog
            //
            this.AcceptButton = this.SaveButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.CancelDialogButton;
            this.ClientSize = new System.Drawing.Size(760, 470);
            this.Controls.Add(this.MainLayoutPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PresetEditorDialog";
            this.Padding = new System.Windows.Forms.Padding(8);
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.MainLayoutPanel.ResumeLayout(false);
            this.EditorTabControl.ResumeLayout(false);
            this.MacTabPage.ResumeLayout(false);
            this.MacLayoutPanel.ResumeLayout(false);
            this.PresetNameGroupBox.ResumeLayout(false);
            this.PresetNameFlowPanel.ResumeLayout(false);
            this.PresetNameFlowPanel.PerformLayout();
            this.MacGroupBox.ResumeLayout(false);
            this.MacInnerLayoutPanel.ResumeLayout(false);
            this.MacInnerLayoutPanel.PerformLayout();
            this.CustomMacFlowPanel.ResumeLayout(false);
            this.CustomMacFlowPanel.PerformLayout();
            this.ProxyGroupBox.ResumeLayout(false);
            this.ProxyGroupBox.PerformLayout();
            this.Ipv4TabPage.ResumeLayout(false);
            this.Ipv4LayoutPanel.ResumeLayout(false);
            this.Ipv4LeftGroupBox.ResumeLayout(false);
            this.Ipv4LeftLayoutPanel.ResumeLayout(false);
            this.Ipv4LeftLayoutPanel.PerformLayout();
            this._ipv4RightGroup.ResumeLayout(false);
            this.Ipv4RightLayoutPanel.ResumeLayout(false);
            this._ipv4AddressGroup.ResumeLayout(false);
            this._ipv4GatewayGroup.ResumeLayout(false);
            this._ipv4DnsGroup.ResumeLayout(false);
            this.Ipv6TabPage.ResumeLayout(false);
            this.ButtonFlowPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this._ipv4AddressGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._ipv4GatewayGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._ipv4DnsGrid)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel? MainLayoutPanel;
        private System.Windows.Forms.TabControl? EditorTabControl;
        private System.Windows.Forms.TabPage? MacTabPage;
        private System.Windows.Forms.TabPage? Ipv4TabPage;
        private System.Windows.Forms.TabPage? Ipv6TabPage;
        private System.Windows.Forms.TableLayoutPanel? MacLayoutPanel;
        private System.Windows.Forms.GroupBox? PresetNameGroupBox;
        private System.Windows.Forms.FlowLayoutPanel? PresetNameFlowPanel;
        private System.Windows.Forms.Label? PresetNameLabel;
        private System.Windows.Forms.GroupBox? MacGroupBox;
        private System.Windows.Forms.TableLayoutPanel? MacInnerLayoutPanel;
        private System.Windows.Forms.FlowLayoutPanel? CustomMacFlowPanel;
        private System.Windows.Forms.GroupBox? ProxyGroupBox;
        private System.Windows.Forms.Label? ProxyPlaceholderLabel;
        private System.Windows.Forms.TableLayoutPanel? Ipv4LayoutPanel;
        private System.Windows.Forms.GroupBox? Ipv4LeftGroupBox;
        private System.Windows.Forms.TableLayoutPanel? Ipv4LeftLayoutPanel;
        private System.Windows.Forms.TableLayoutPanel? Ipv4RightLayoutPanel;
        private System.Windows.Forms.Label? Ipv6PlaceholderLabel;
        private System.Windows.Forms.FlowLayoutPanel? ButtonFlowPanel;
        private System.Windows.Forms.Button? CancelDialogButton;
        private System.Windows.Forms.Button? SaveButton;
        private System.Windows.Forms.TextBox? _presetNameTextBox;
        private System.Windows.Forms.CheckBox? _includeMacCheckBox;
        private System.Windows.Forms.RadioButton? _useRandomMacRadio;
        private System.Windows.Forms.RadioButton? _useRandom02MacRadio;
        private System.Windows.Forms.RadioButton? _useOriginalMacRadio;
        private System.Windows.Forms.RadioButton? _useCustomMacRadio;
        private System.Windows.Forms.TextBox? _customMacTextBox;
        private System.Windows.Forms.CheckBox? _includeIpv4CheckBox;
        private System.Windows.Forms.CheckBox? _dhcpIpv4CheckBox;
        private System.Windows.Forms.CheckBox? _ipv4AddressCheckBox;
        private System.Windows.Forms.CheckBox? _ipv4GatewayCheckBox;
        private System.Windows.Forms.CheckBox? _ipv4DnsCheckBox;
        private System.Windows.Forms.GroupBox? _ipv4RightGroup;
        private System.Windows.Forms.GroupBox? _ipv4AddressGroup;
        private System.Windows.Forms.GroupBox? _ipv4GatewayGroup;
        private System.Windows.Forms.GroupBox? _ipv4DnsGroup;
        private System.Windows.Forms.DataGridView? _ipv4AddressGrid;
        private System.Windows.Forms.DataGridView? _ipv4GatewayGrid;
        private System.Windows.Forms.DataGridView? _ipv4DnsGrid;
    }
}
