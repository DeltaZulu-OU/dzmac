using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using MacChanger.Gui.DTO;
using MacChanger.Gui.Properties;

namespace MacChanger.Gui.Forms
{
    public partial class MainForm : Form
    {
        private const string zeroMacValue = "00-00-00-00-00-00";
        private readonly Label _loadingLabel;
        private readonly Panel _loadingPanel;
        private readonly ProgressBar _loadingProgressBar;
        private readonly VendorManager _vm;
        private ListView _ipv4AddressListView;
        private ListView _ipv4DnsListView;
        private ListView _ipv4GatewayListView;
        private ListView _ipv6AddressListView;
        private ListView _ipv6DnsListView;
        private ListView _ipv6GatewayListView;
        private bool _isRefreshing;
        private bool _isVendorComboBound;
        private bool _isVendorListLoading;
        private bool _isVendorListReady;
        private bool _locallyAdministered;
        private bool _reenableOnChange;
        private CancellationTokenSource _refreshCancellation;
        private NetworkConnectionDetail _selected;
        private List<Vendor> _vendors;

        public MainForm()
        {
            _vm = new VendorManager();
            InitializeComponent();
            ConfigureV1Surface();
            InitializeIpAddressPage();

            _loadingProgressBar = new ProgressBar
            {
                Style = ProgressBarStyle.Marquee,
                MarqueeAnimationSpeed = 20,
                Width = 250,
                Height = 18,
                Anchor = AnchorStyles.None
            };

            _loadingLabel = new Label
            {
                AutoSize = true,
                Anchor = AnchorStyles.None,
                Text = "Loading network adapters..."
            };

            var loadingLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 3,
                BackColor = Color.FromArgb(225, SystemColors.Control)
            };
            loadingLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
            loadingLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 45f));
            loadingLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            loadingLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            loadingLayout.Controls.Add(_loadingLabel, 0, 1);
            loadingLayout.Controls.Add(_loadingProgressBar, 0, 2);

            _loadingPanel = new Panel
            {
                Dock = DockStyle.Fill,
                Visible = false
            };
            _loadingPanel.Controls.Add(loadingLayout);

            MainTableLayoutPanel.Controls.Add(_loadingPanel, 0, 0);
            _loadingPanel.BringToFront();

            ConnectionsGrid.EmptyListMsg = "No network adapters loaded.";
            VendorComboBox.Enabled = false;
            UpdateSelectionState();
        }

        internal List<NetworkConnection> NetworkConnections { get; set; }

        #region EventHandlers

        private void AboutItem_Click(object sender, EventArgs e) => new AboutBox().Show(this);

        private void AssociateItem_Click(object sender, EventArgs e) => NotImplemented();

        private void AutoStartCheckBox_CheckedChanged(object sender, EventArgs e) => _reenableOnChange = AutoStartCheckBox.Checked;

        private void ChangeMacButton_Click(object sender, EventArgs e)
        {
            if (_selected == null)
            {
                return;
            }

            var targetMac = macTextBox.Text;

            // Ignore default value to prevend accidents
            if (targetMac.Equals(zeroMacValue))
            {
                return;
            }

            if (_selected.TryUpdateMac(new MacAddress(targetMac)))
            {
                _ = MessageBox.Show("Successfully updated MAC address", "MAC Address Change", MessageBoxButtons.OK, MessageBoxIcon.Information);
                _ = RefreshConnectionsBackground();
            }
            else
            {
                _ = MessageBox.Show("Failed to update MAC address", "MAC Address Change", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CheckUpdateItem_Click(object sender, EventArgs e) => NotImplemented();

        private void CliParamsHelpItem_Click(object sender, EventArgs e) => new CommandLineParametersHelpForm().Show(this);

        private void ConnectionsGrid_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_isRefreshing)
            {
                return;
            }

            if (ConnectionsGrid?.SelectedItem != null)
            {
                // The event is triggered twice:
                // First one is a reset event where index is 0xffffff (-1),
                // and the second is the actual value.
                if (ConnectionsGrid.SelectedItem.Index != -1)
                {
                    var row = ConnectionsGrid.SelectedItem.RowObject as NetworkConnection;
                    _selected = row?.Detail;
                    BindSelection();
                }
            }
            else
            {
                _selected = null;
                BindSelection();
            }
        }

        private void DeleteItem_Click(object sender, EventArgs e) => NotImplemented();

        private void Dhcp4EnabledItem_Click(object sender, EventArgs e)
        {
            if (_selected == null)
            {
                return;
            }

            if (DhcpEnabledItem.Checked)
            {
                if (_selected.TryDhcpDisable())
                {
                    DhcpEnabledItem.Checked = false;
                }
            }
            else
            {
                if (_selected.TryDhcpEnable())
                {
                    DhcpEnabledItem.Checked = true;
                }
            }

            _ = RefreshConnectionsBackground();
        }

        private void DhcpReleaseIpItem_Click(object sender, EventArgs e)
        {
            if (_selected == null)
            {
                return;
            }

            if (_selected.TryDhcpRelease(out var message))
            {
                MessageBox.Show(message, "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show(message, "Failure", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void DhcpRenewIpItem_Click(object sender, EventArgs e)
        {
            if (_selected == null)
            {
                return;
            }

            if (_selected.TryDhcpRenew(out var message))
            {
                MessageBox.Show(message, "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show(message, "Failure", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void ExitItem_Click(object sender, EventArgs e) => Close();

        private void ExportPresetItem_Click(object sender, EventArgs e) => NotImplemented();

        private void ExportReportItem_Click(object sender, EventArgs e) => NotImplemented();

        private void HelpTopicsItem_Click(object sender, EventArgs e) => VisitUrl("https://github.com/zbalkan/MacChanger");

        private void ImportPresetItem_Click(object sender, EventArgs e) => NotImplemented();

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            _refreshCancellation?.Cancel();
            _refreshCancellation?.Dispose();
            _selected?.Dispose();
            DisposeConnections(NetworkConnections);
            _vm?.Dispose();
        }

        private async void MainForm_LoadAsync(object sender, EventArgs e)
        {
            MakeTextboxBackgroundTransparent();
            ShowSpeedInKBytesPerSecItem.Checked = Settings.Default.ShowSpeedInKBytesPerSec;

            var refreshTask = RefreshConnectionsBackground(clearListWhileLoading: true);
            _ = LoadVendorsInBackground();
            await refreshTask;
        }

        private void MainForm_Resize(object sender, EventArgs e) => ConnectionsGrid.AutoResizeColumns();

        private void MakeTextboxBackgroundTransparent()
        {
            ConnectionValueTextbox.BackColor = Color.FromArgb(255, InformationPage.BackColor.R, InformationPage.BackColor.G, InformationPage.BackColor.B);
            DeviceValueTextbox.BackColor = Color.FromArgb(255, InformationPage.BackColor.R, InformationPage.BackColor.G, InformationPage.BackColor.B);
            HardwareIdValueTextbox.BackColor = Color.FromArgb(255, InformationPage.BackColor.R, InformationPage.BackColor.G, InformationPage.BackColor.B);
            ConfigIdValueTextbox.BackColor = Color.FromArgb(255, InformationPage.BackColor.R, InformationPage.BackColor.G, InformationPage.BackColor.B);
            Ipv4ValueTextbox.BackColor = Color.FromArgb(255, InformationPage.BackColor.R, InformationPage.BackColor.G, InformationPage.BackColor.B);
            Ipv6ValueTextbox.BackColor = Color.FromArgb(255, InformationPage.BackColor.R, InformationPage.BackColor.G, InformationPage.BackColor.B);
            OriginalMacValueTextbox.BackColor = Color.FromArgb(255, InformationPage.BackColor.R, InformationPage.BackColor.G, InformationPage.BackColor.B);
            OriginalMacVendorTextbox.BackColor = Color.FromArgb(255, InformationPage.BackColor.R, InformationPage.BackColor.G, InformationPage.BackColor.B);
            ActiveMacValueTextbox.BackColor = Color.FromArgb(255, InformationPage.BackColor.R, InformationPage.BackColor.G, InformationPage.BackColor.B);
            ActiveMacVendorTextbox.BackColor = Color.FromArgb(255, InformationPage.BackColor.R, InformationPage.BackColor.G, InformationPage.BackColor.B);
        }

        private void NetworkConnectionsItem_Click(object sender, EventArgs e) => OpenNetworkConnections();

        private void OpenPresetItem_Click(object sender, EventArgs e) => NotImplemented();

        private void PersistentAddressCheckBox_CheckedChanged(object sender, EventArgs e) => NotImplemented();

        private void RandomMacButton_Click(object sender, EventArgs e)
        {
            if (_selected == null)
            {
                return;
            }

            if (!_isVendorListReady)
            {
                _ = MessageBox.Show("Vendor list is still loading. Please try again in a moment.", "Vendor List Loading", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            Vendor randomVendor;
            try
            {
                randomVendor = _vm.GetRandom();
            }
            catch (MacChangerException ex)
            {
                _ = MessageBox.Show(ex.Message, "Vendor List Unavailable", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var randomMac = _selected.GetRandom(randomVendor.Oui);

            var matchedVendor = _vm.FindByMac(randomMac, _locallyAdministered);
            if (_isVendorComboBound)
            {
                VendorComboBox.SelectedItem = matchedVendor;
            }
            else
            {
                VendorComboBox.Text = matchedVendor?.VendorName ?? string.Empty;
            }

            if (_locallyAdministered)
            {
                randomMac.SetAsLocallyAdministered();
            }

            macTextBox.Text = randomMac.ToString(MacAddress.MacDelimiter.Colon);
        }

        private async void RefreshItem_Click(object sender, EventArgs e)
        {
            await RefreshConnectionsBackground();
        }

        private void RestoreMacButton_Click(object sender, EventArgs e)
        {
            if (_selected == null)
            {
                return;
            }

            if (_selected.Changed == "No")
            {
                return;
            }

            if (_selected.TryReset())
            {
                _ = MessageBox.Show("Successfully restored MAC address", "MAC Address Restore", MessageBoxButtons.OK, MessageBoxIcon.Information);

                if (_reenableOnChange && _selected.TryDisable())
                {
                    _ = _selected.TryEnable();
                }

                _ = RefreshConnectionsBackground();
            }
            else
            {
                _ = MessageBox.Show("Failed to restore MAC address", "MAC Address Restore", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SavePresetAsItem_Click(object sender, EventArgs e) => NotImplemented();

        private void SavePresetItem_Click(object sender, EventArgs e) => NotImplemented();

        private void ShowSpeedInKBytesPerSecItem_CheckedChanged(object sender, EventArgs e)
        {
            var showSpeedInKBytesPerSec = ShowSpeedInKBytesPerSecItem.Checked;
            Settings.Default.ShowSpeedInKBytesPerSec = showSpeedInKBytesPerSec;
            Settings.Default.Save();

            if (NetworkConnections == null || NetworkConnections.Count == 0)
            {
                return;
            }

            foreach (var networkConnection in NetworkConnections)
            {
                networkConnection.Detail.ShowSpeedInKBytesPerSec = showSpeedInKBytesPerSec;
            }

            ConnectionsGrid.BeginUpdate();
            ConnectionsGrid.RefreshObjects(NetworkConnections);
            ConnectionsGrid.EndUpdate();
        }

        private async void UpdateOuiItem_ClickAsync(object sender, EventArgs e)
        {
            var result = MessageBox.Show("This will initiate OUI download in the background.\n\nAre you sure?", "Update Vendor List (OUI) from IEEE", MessageBoxButtons.YesNo);

            if (result != DialogResult.Yes)
            {
                return;
            }

            MainStatusBar.Text = "Downloading OUI data...";
            await Task.Factory.StartNew(_vm.Refresh);
            _ = MessageBox.Show("Vendor list updated.", "Update Vendor List (OUI) from IEEE", MessageBoxButtons.OK);
            MainStatusBar.Text = "Ready";
        }

        private void WikiLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            WikiLink.LinkVisited = true;
            VisitUrl("https://en.wikipedia.org/wiki/MAC_address#IEEE_802c_local_MAC_address_usage");
        }

        private void ZeroTwoCheckBox_CheckedChanged(object sender, EventArgs e) => _locallyAdministered = ZeroTwoCheckBox.Checked;

        #endregion EventHandlers

        #region Private Methods

        private static ListView CreateDetailsListView(params string[] columns)
        {
            var listView = new ListView
            {
                Dock = DockStyle.Fill,
                View = View.Details,
                FullRowSelect = true,
                GridLines = true,
                HeaderStyle = ColumnHeaderStyle.Nonclickable
            };

            var columnWidth = columns.Length == 1 ? 600 : 280;
            foreach (var column in columns)
            {
                listView.Columns.Add(column, columnWidth);
            }

            return listView;
        }

        private static GroupBox CreateIpSectionGroup(string title, Control content)
        {
            var group = new GroupBox
            {
                Dock = DockStyle.Fill,
                Text = title,
                Padding = new Padding(6)
            };

            group.Controls.Add(content);
            return group;
        }

        private static void DisposeConnections(List<NetworkConnection> connections)
        {
            if (connections == null)
            {
                return;
            }

            foreach (var networkConnection in connections)
            {
                networkConnection.Dispose();
            }
        }

        /// <summary>
        ///     A placeholder method for events not implemented.
        /// </summary>
        private static void NotImplemented() => _ = MessageBox.Show("Not implemented.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

        private static void OpenNetworkConnections()
        {
            try
            {
                _ = Process.Start(new ProcessStartInfo
                {
                    FileName = "ncpa.cpl",
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                _ = MessageBox.Show($"Unable to open Network Connections.{Environment.NewLine}{Environment.NewLine}{ex.Message}", "Network Connections", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private static void PopulateSingleValueRows(ListView listView, IEnumerable<string> values, string fallback)
        {
            listView.BeginUpdate();
            listView.Items.Clear();

            foreach (var value in values)
            {
                listView.Items.Add(new ListViewItem(value));
            }

            if (listView.Items.Count == 0)
            {
                listView.Items.Add(new ListViewItem(fallback));
            }

            listView.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
            listView.EndUpdate();
        }

        private static void VisitUrl(string url)
        {
            try
            {
                _ = Process.Start(url);
            }
            catch
            {
                // ignore
            }
        }

        private void BindIpAddressDetails()
        {
            if (_ipv4AddressListView == null)
            {
                return;
            }

            PopulateIpv4AddressRows(_selected?.Ipv4Addresses ?? Array.Empty<AdapterIpv4Address>());
            PopulateSingleValueRows(_ipv4GatewayListView, _selected?.Ipv4Gateways ?? Array.Empty<string>(), "No IPv4 gateway");
            PopulateSingleValueRows(_ipv4DnsListView, _selected?.Ipv4DnsServers ?? Array.Empty<string>(), "No IPv4 DNS server");

            PopulateIpv6AddressRows(_selected?.Ipv6Addresses ?? Array.Empty<AdapterIpv6Address>());
            PopulateSingleValueRows(_ipv6GatewayListView, _selected?.Ipv6Gateways ?? Array.Empty<string>(), "No IPv6 gateway");
            PopulateSingleValueRows(_ipv6DnsListView, _selected?.Ipv6DnsServers ?? Array.Empty<string>(), "No IPv6 DNS server");
        }

        private void BindSelection()
        {
            var hasSelection = _selected != null;
            if (!hasSelection)
            {
                ConnectionValueTextbox.Text = "...";
                DeviceValueTextbox.Text = "...";
                HardwareIdValueTextbox.Text = "...";
                ConfigIdValueTextbox.Text = "...";
                Ipv4ValueTextbox.Text = "...";
                Ipv6ValueTextbox.Text = "...";
                OriginalMacValueTextbox.Text = "...";
                OriginalMacVendorTextbox.Text = "...";
                ActiveMacValueTextbox.Text = "...";
                ActiveMacVendorTextbox.Text = "...";
                DhcpEnabledItem.Checked = false;
                BindIpAddressDetails();
                UpdateSelectionState();
                return;
            }

            ConnectionValueTextbox.Text = _selected.Name;
            DeviceValueTextbox.Text = _selected.Device;
            HardwareIdValueTextbox.Text = _selected.HardwareId;
            ConfigIdValueTextbox.Text = _selected.ConfigId;
            Ipv4ValueTextbox.Text = _selected.IPv4Status;
            Ipv6ValueTextbox.Text = _selected.IPv6Status;
            OriginalMacValueTextbox.Text = _selected.OriginalMac;
            OriginalMacVendorTextbox.Text = _selected.OriginalVendor;
            ActiveMacValueTextbox.Text = _selected.ActiveMac;
            ActiveMacVendorTextbox.Text = _selected.ActiveVendor;
            DhcpEnabledItem.Checked = _selected.IsDhcpEnabled;
            BindIpAddressDetails();
            UpdateSelectionState();
        }

        private void ConfigureV1Surface()
        {
            // Keep v1 UI surface aligned with implemented feature set.
            ExportReportItem.Visible = false;
            toolStripSeparator1.Visible = false;
            OpenPresetItem.Visible = false;
            SavePresetItem.Visible = false;
            SavePresetAsItem.Visible = false;
            toolStripSeparator2.Visible = false;
            ImportPresetItem.Visible = false;
            ExportPresetItem.Visible = false;
            AssociateItem.Visible = false;
            toolStripSeparator7.Visible = false;
            DeleteItem.Visible = false;
            CliParamsHelpItem.Visible = true;
            toolStripSeparator6.Visible = true;
            CheckUpdateItem.Visible = false;

            if (InfoTabs.TabPages.Contains(PresetsPage))
            {
                InfoTabs.TabPages.Remove(PresetsPage);
            }

            PersistentAddressCheckBox.Visible = false;
        }

        private void InitializeIpAddressPage()
        {
            IPAddressPage.Controls.Clear();

            var rootLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 1
            };
            rootLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
            rootLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));

            var ipv4Column = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 3
            };
            ipv4Column.RowStyles.Add(new RowStyle(SizeType.Percent, 50f));
            ipv4Column.RowStyles.Add(new RowStyle(SizeType.Percent, 25f));
            ipv4Column.RowStyles.Add(new RowStyle(SizeType.Percent, 25f));

            var ipv6Column = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 3
            };
            ipv6Column.RowStyles.Add(new RowStyle(SizeType.Percent, 50f));
            ipv6Column.RowStyles.Add(new RowStyle(SizeType.Percent, 25f));
            ipv6Column.RowStyles.Add(new RowStyle(SizeType.Percent, 25f));

            _ipv4AddressListView = CreateDetailsListView("IP Address", "Subnet Mask");
            _ipv4GatewayListView = CreateDetailsListView("Gateway");
            _ipv4DnsListView = CreateDetailsListView("DNS Server");

            _ipv6AddressListView = CreateDetailsListView("Unicast IPv6 Address", "Prefix");
            _ipv6GatewayListView = CreateDetailsListView("Gateway/Next Hop");
            _ipv6DnsListView = CreateDetailsListView("DNS Server");

            ipv4Column.Controls.Add(CreateIpSectionGroup("Internet Protocol v4 (DHCPv4)", _ipv4AddressListView), 0, 0);
            ipv4Column.Controls.Add(CreateIpSectionGroup("Gateway", _ipv4GatewayListView), 0, 1);
            ipv4Column.Controls.Add(CreateIpSectionGroup("DNS Server", _ipv4DnsListView), 0, 2);

            // DHCPv6 operations are intentionally out of scope; IPv6 is read-only status.
            ipv6Column.Controls.Add(CreateIpSectionGroup("Internet Protocol v6 (Stateless)", _ipv6AddressListView), 0, 0);
            ipv6Column.Controls.Add(CreateIpSectionGroup("Gateway/Next Hop", _ipv6GatewayListView), 0, 1);
            ipv6Column.Controls.Add(CreateIpSectionGroup("DNS Server", _ipv6DnsListView), 0, 2);

            rootLayout.Controls.Add(ipv4Column, 0, 0);
            rootLayout.Controls.Add(ipv6Column, 1, 0);

            IPAddressPage.Controls.Add(rootLayout);
        }

        private async Task LoadVendorsInBackground()
        {
            if (_isVendorListLoading || _isVendorListReady)
            {
                return;
            }

            _isVendorListLoading = true;
            if (!IsDisposed)
            {
                MainStatusBar.Text = "Loading vendor list...";
            }

            try
            {
                var vendors = await Task.Run(() => _vm.GetVendorList().ToList());
                if (IsDisposed)
                {
                    return;
                }

                BeginInvoke(new Action(() =>
                {
                    if (IsDisposed)
                    {
                        return;
                    }

                    _vendors = vendors;
                    VendorComboBox.Enabled = true;
                    _isVendorListReady = true;
                    TryBindVendorsWhenReady();
                    UpdateSelectionState();

                    if (MainStatusBar.Text == "Loading vendor list...")
                    {
                        MainStatusBar.Text = "Ready";
                    }
                }));
            }
            catch (Exception ex)
            {
                if (!IsDisposed)
                {
                    BeginInvoke(new Action(() =>
                    {
                        if (IsDisposed)
                        {
                            return;
                        }

                        MainStatusBar.Text = "Vendor list failed to load.";
                        _ = MessageBox.Show(ex.Message, "Vendor List Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }));
                }
            }
            finally
            {
                _isVendorListLoading = false;
            }
        }

        private void PopulateIpv4AddressRows(IEnumerable<AdapterIpv4Address> addresses)
        {
            _ipv4AddressListView.BeginUpdate();
            _ipv4AddressListView.Items.Clear();

            foreach (var entry in addresses)
            {
                _ipv4AddressListView.Items.Add(new ListViewItem(new[] { entry.Address, entry.SubnetMask }));
            }

            if (_ipv4AddressListView.Items.Count == 0)
            {
                _ipv4AddressListView.Items.Add(new ListViewItem(new[] { "No IPv4 address", string.Empty }));
            }

            _ipv4AddressListView.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
            _ipv4AddressListView.EndUpdate();
        }

        private void PopulateIpv6AddressRows(IEnumerable<AdapterIpv6Address> addresses)
        {
            _ipv6AddressListView.BeginUpdate();
            _ipv6AddressListView.Items.Clear();

            foreach (var entry in addresses)
            {
                _ipv6AddressListView.Items.Add(new ListViewItem(new[] { entry.Address, entry.PrefixLength.ToString() }));
            }

            if (_ipv6AddressListView.Items.Count == 0)
            {
                _ipv6AddressListView.Items.Add(new ListViewItem(new[] { "No IPv6 address", string.Empty }));
            }

            _ipv6AddressListView.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
            _ipv6AddressListView.EndUpdate();
        }

        private async Task RefreshConnectionsBackground(bool clearListWhileLoading = false)
        {
            if (_isRefreshing)
            {
                return;
            }

            _refreshCancellation?.Cancel();
            _refreshCancellation?.Dispose();
            _refreshCancellation = new CancellationTokenSource();
            var cancellationToken = _refreshCancellation.Token;

            SetLoadingState(true, clearListWhileLoading);

            try
            {
                var updatedConnections = await Task.Run(() =>
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    var adapters = NetworkAdapterFactory.GetNetworkAdapters(_vm).ToList();
                    cancellationToken.ThrowIfCancellationRequested();

                    return adapters.Select(adapter => new NetworkConnection(adapter, ShowSpeedInKBytesPerSecItem.Checked)).ToList();
                }, cancellationToken);

                if (cancellationToken.IsCancellationRequested || IsDisposed)
                {
                    DisposeConnections(updatedConnections);
                    return;
                }

                BeginInvoke(new Action(() =>
                {
                    if (IsDisposed)
                    {
                        DisposeConnections(updatedConnections);
                        return;
                    }

                    ConnectionsGrid.BeginUpdate();
                    DisposeConnections(NetworkConnections);
                    NetworkConnections = updatedConnections;
                    ConnectionsGrid.DataSource = NetworkConnections;
                    ConnectionsGrid.EndUpdate();
                    ConnectionsGrid.AutoResizeColumns();

                    ConnectionsGrid.SelectedItem = null;
                    _selected = null;
                    BindSelection();
                    MainStatusBar.Text = $"Loaded {NetworkConnections.Count} adapters.";
                }));
            }
            catch (OperationCanceledException)
            {
                if (!IsDisposed)
                {
                    MainStatusBar.Text = "Adapter loading cancelled.";
                }
            }
            catch (Exception ex)
            {
                if (!IsDisposed)
                {
                    MainStatusBar.Text = "Failed to load network adapters.";
                    _ = MessageBox.Show(ex.Message, "Adapter Discovery Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            finally
            {
                if (!IsDisposed)
                {
                    BeginInvoke(new Action(() =>
                    {
                        if (!IsDisposed)
                        {
                            SetLoadingState(false, false);
                            if (MainStatusBar.Text == "Loading network adapters...")
                            {
                                MainStatusBar.Text = "Ready";
                            }
                        }
                    }));
                }
            }
        }

        private void SetLoadingState(bool isLoading, bool clearListWhileLoading)
        {
            _isRefreshing = isLoading;
            RefreshItem.Enabled = !isLoading;
            ConnectionsGrid.Enabled = !isLoading;

            if (isLoading)
            {
                MainStatusBar.Text = "Loading network adapters...";
                _loadingLabel.Text = "Loading network adapters...";
                _loadingPanel.Visible = false;

                if (clearListWhileLoading)
                {
                    ConnectionsGrid.EmptyListMsg = "Loading network adapters...";
                    ConnectionsGrid.DataSource = Array.Empty<NetworkConnection>();
                    _selected = null;
                    BindSelection();
                }
            }
            else
            {
                _loadingPanel.Visible = false;
                ConnectionsGrid.EmptyListMsg = "No network adapters loaded.";
            }

            UpdateSelectionState();
        }

        private void TryBindVendorsWhenReady()
        {
            if (_isVendorComboBound || !_isVendorListReady || _vendors == null)
            {
                return;
            }

            var currentText = VendorComboBox.Text;
            VendorComboBox.BeginUpdate();
            try
            {
                VendorComboBox.DataSource = _vendors;
                VendorComboBox.SelectedItem = null;
                if (!string.IsNullOrWhiteSpace(currentText))
                {
                    VendorComboBox.Text = currentText;
                }
            }
            finally
            {
                VendorComboBox.EndUpdate();
            }

            _isVendorComboBound = true;
        }

        private void UpdateSelectionState()
        {
            var hasSelection = _selected != null;
            var enableActions = hasSelection && !_isRefreshing;

            ChangeMacButton.Enabled = enableActions;
            RestoreMacButton.Enabled = enableActions;
            RandomMacButton.Enabled = enableActions && _isVendorListReady;
            DhcpEnabledItem.Enabled = enableActions;
            DhcpRenewIpItem.Enabled = enableActions && _selected != null && _selected.IsDhcpEnabled;
            DhcpReleaseIpItem.Enabled = enableActions && _selected != null && _selected.IsDhcpEnabled;
            DeleteItem.Enabled = enableActions;
        }

        #endregion Private Methods
    }
}
