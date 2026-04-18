using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net.NetworkInformation;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Dzmac.Gui.DTO;
using Dzmac.Gui.Properties;
using Dzmac.Gui.Core;
using BrightIdeasSoftware;

namespace Dzmac.Gui.Forms
{
    public partial class MainForm : Form
    {
        private readonly struct PerformanceSample
        {
            public DateTime Timestamp { get; }
            public double Value { get; }

            public PerformanceSample(DateTime timestamp, double value)
            {
                Timestamp = timestamp;
                Value = value;
            }
        }

        private const string zeroMacValue = "00-00-00-00-00-00";
        private const int performanceSampleCapacity = 120;
        private const int performanceRefreshMs = 1000;
        private readonly Label _loadingLabel;
        private readonly Panel _loadingPanel;
        private readonly ProgressBar _loadingProgressBar;
        private readonly ToolTip _connectionDetailsTooltip;
        private readonly VendorManager _vm;
        private readonly IAdapterAdminService _adminService;
        private readonly object _performanceBufferSync = new object();
        private readonly PerformanceSample[] _receivedSamples = new PerformanceSample[performanceSampleCapacity];
        private readonly PerformanceSample[] _sentSamples = new PerformanceSample[performanceSampleCapacity];
        private int _sampleCount;
        private int _sampleWriteIndex;
        private Label _performanceReceivedLabel;
        private Label _performanceReceivedSpeedLabel;
        private Label _performanceSentLabel;
        private Label _performanceSentSpeedLabel;
        private Panel _performanceGraphPanel;
        private NetworkInterface _selectedNetworkInterface;
        private CancellationTokenSource _performanceLoopCancellation;
        private int _performanceResolveVersion;
        private int _performanceUiUpdatePending;
        private bool _isRefreshing;
        private bool _isVendorComboBound;
        private bool _isVendorListLoading;
        private bool _isVendorListReady;
        private bool _isStartupInitialized;
        private bool _locallyAdministered;
        private bool _reenableOnChange;
        private CancellationTokenSource _refreshCancellation;
        private CancellationTokenSource _vendorRefreshCancellation;
        private NetworkConnectionDetail _selected;
        private IReadOnlyDictionary<string, NetworkInterface> _networkInterfacesById = new ReadOnlyDictionary<string, NetworkInterface>(new Dictionary<string, NetworkInterface>());
        private List<Vendor> _vendors;

        public MainForm()
        {
            _vm = new VendorManager();
            _adminService = new AdapterAdminService();
            _connectionDetailsTooltip = new ToolTip();
            InitializeComponent();
            ConfigureV1Surface();

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

            InfoTabs.SelectedIndexChanged += InfoTabs_SelectedIndexChanged;
            Shown += MainForm_ShownAsync;
            ConnectionsGrid.EmptyListMsg = "No network adapters loaded.";
            VendorComboBox.Enabled = false;
            InitializePerformancePanel();
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

            if (_adminService.SetRegistryMac(_selected.Adapter, new MacAddress(targetMac)).IsSuccess)
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
                if (_adminService.SetDhcpEnabled(_selected.Adapter, false).IsSuccess)
                {
                    DhcpEnabledItem.Checked = false;
                }
            }
            else
            {
                if (_adminService.SetDhcpEnabled(_selected.Adapter, true).IsSuccess)
                {
                    DhcpEnabledItem.Checked = true;
                }
            }

            _ = RefreshConnectionsBackground();
        }

        private async void DhcpReleaseIpItem_Click(object sender, EventArgs e)
        {
            if (_selected == null)
            {
                return;
            }

            var selectedDetail = _selected;
            await RunDhcpActionAsync(
                selectedDetail,
                actionName: "release IP",
                inProgressText: "Releasing the IP for",
                successText: "Released the IP for",
                failureText: "Failed to release the IP for",
                action: () => _adminService.ReleaseDhcpLease(selectedDetail.Adapter));
        }

        private async void DhcpRenewIpItem_Click(object sender, EventArgs e)
        {
            if (_selected == null)
            {
                return;
            }

            var selectedDetail = _selected;
            await RunDhcpActionAsync(
                selectedDetail,
                actionName: "renew IP",
                inProgressText: "Renewing the IP for",
                successText: "Renewed the IP for",
                failureText: "Failed to renew the IP for",
                action: () => _adminService.RenewDhcpLease(selectedDetail.Adapter));
        }

        private void ExitItem_Click(object sender, EventArgs e) => Close();

        private void ExportPresetItem_Click(object sender, EventArgs e) => NotImplemented();

        private void ExportReportItem_Click(object sender, EventArgs e)
        {
            if (NetworkConnections == null || NetworkConnections.Count == 0)
            {
                _ = MessageBox.Show("No network adapter information is available to export.", "Export Text Report", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using (var dialog = new SaveFileDialog())
            {
                dialog.Title = "Export Text Report";
                dialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
                dialog.DefaultExt = "txt";
                dialog.FileName = $"DZMAC-Text-Report-{DateTime.Now:yyyyMMdd-HHmmss}.txt";

                if (dialog.ShowDialog(this) != DialogResult.OK)
                {
                    return;
                }

                try
                {
                    var report = BuildTextReport();
                    System.IO.File.WriteAllText(dialog.FileName, report, Encoding.UTF8);
                    MainStatusBar.Text = $"Report exported: {dialog.FileName}";
                }
                catch (Exception ex)
                {
                    MainStatusBar.Text = "Failed to export text report.";
                    _ = MessageBox.Show(ex.Message, "Export Text Report", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void HelpTopicsItem_Click(object sender, EventArgs e) => VisitUrl("https://github.com/zbalkan/DZMAC/wiki/Help");

        private void ImportPresetItem_Click(object sender, EventArgs e) => NotImplemented();

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            _refreshCancellation?.Cancel();
            _refreshCancellation?.Dispose();
            _vendorRefreshCancellation?.Cancel();
            _vendorRefreshCancellation?.Dispose();
            _performanceLoopCancellation?.Cancel();
            _performanceLoopCancellation?.Dispose();
            _selected?.Dispose();
            DisposeConnections(NetworkConnections);
            _connectionDetailsTooltip?.Dispose();
            _vm?.Dispose();
        }

        private void MainForm_LoadAsync(object sender, EventArgs e)
        {
            ShowSpeedInKBytesPerSecItem.Checked = Settings.Default.ShowSpeedInKBytesPerSec;
            ShowAllAdaptersItem.Checked = Settings.Default.ShowAllAdapters;
        }

        private void MainForm_ShownAsync(object sender, EventArgs e)
        {
            if (_isStartupInitialized)
            {
                return;
            }

            _isStartupInitialized = true;
            Application.Idle += MainForm_InitializeStartupOnIdleAsync;
        }

        private async void MainForm_InitializeStartupOnIdleAsync(object sender, EventArgs e)
        {
            Application.Idle -= MainForm_InitializeStartupOnIdleAsync;
            await InitializeStartupAsync();
        }

        private async Task InitializeStartupAsync()
        {
            try
            {
                var interfaceCacheTask = InitializeNetworkInterfaceCacheAsync();
                var refreshTask = RefreshConnectionsBackground(clearListWhileLoading: true);
                _ = LoadVendorsInBackground();
                await Task.WhenAll(interfaceCacheTask, refreshTask);
            }
            catch (Exception ex)
            {
                if (!IsDisposed)
                {
                    MainStatusBar.Text = "Startup initialization failed.";
                    _ = MessageBox.Show(ex.Message, "Initialization Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private async Task RunDhcpActionAsync(NetworkConnectionDetail selectedDetail, string actionName, string inProgressText, string successText, string failureText, Func<AdapterAdminResult> action)
        {
            var selectedAdapterName = selectedDetail.Name;
            MainStatusBar.Text = $"{inProgressText} {selectedAdapterName}...";

            var operationResult = await Task.Run(() => action());
            var operationMessage = operationResult.Message;

            if (operationResult.IsSuccess)
            {
                MainStatusBar.Text = $"{successText} {selectedAdapterName}.";
                MessageBox.Show(operationMessage, "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            MainStatusBar.Text = $"{failureText} {selectedAdapterName}.";
            var failureDetail = $"{operationMessage} (Code: {operationResult.Code})";
            MessageBox.Show(failureDetail, "Failure", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            Diagnostics.Warning("dhcp_action_failed", $"Could not {actionName} for '{selectedAdapterName}'. {operationMessage}", ("operationCode", operationResult.Code.ToString()));
        }

        private void MainForm_Resize(object sender, EventArgs e) => ConnectionsGrid.AutoResizeColumns();

        private void NetworkConnectionsItem_Click(object sender, EventArgs e) => OpenNetworkConnections();

        private void SetConnectionDetailValue(Label label, string value)
        {
            var displayValue = string.IsNullOrWhiteSpace(value) ? "..." : value;
            label.Text = displayValue;
            _connectionDetailsTooltip.SetToolTip(label, displayValue == "..." ? string.Empty : displayValue);
        }

        private void InfoTabs_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_selected == null || !IsPerformanceTabVisible())
            {
                StopPerformanceMonitoring();
                return;
            }

            _ = RestartPerformanceMonitoringAsync(_selected.ConfigId);
        }

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
            catch (DZMACException ex)
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

        private async void RefreshItem_Click(object sender, EventArgs e) => await RefreshConnectionsBackground();

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

            if (_adminService.ResetRegistryMac(_selected.Adapter).IsSuccess)
            {
                _ = MessageBox.Show("Successfully restored MAC address", "MAC Address Restore", MessageBoxButtons.OK, MessageBoxIcon.Information);

                if (_reenableOnChange && _adminService.SetAdapterEnabled(_selected.Adapter, false).IsSuccess)
                {
                    _ = _adminService.SetAdapterEnabled(_selected.Adapter, true);
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

        private async void ShowAllAdaptersItem_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.ShowAllAdapters = ShowAllAdaptersItem.Checked;
            Settings.Default.Save();
            await RefreshConnectionsBackground();
        }

        private async void UpdateOuiItem_ClickAsync(object sender, EventArgs e)
        {
            var result = MessageBox.Show("This will initiate OUI download in the background.\n\nAre you sure?", "Update Vendor List (OUI) from IEEE", MessageBoxButtons.YesNo);

            if (result != DialogResult.Yes)
            {
                return;
            }

            MainStatusBar.Text = "Downloading OUI data...";
            _vendorRefreshCancellation?.Cancel();
            _vendorRefreshCancellation?.Dispose();
            _vendorRefreshCancellation = new CancellationTokenSource();

            try
            {
                await _vm.RefreshAsync(_vendorRefreshCancellation.Token);
                _ = MessageBox.Show("Vendor list updated.", "Update Vendor List (OUI) from IEEE", MessageBoxButtons.OK);
                MainStatusBar.Text = "Ready";
            }
            catch (OperationCanceledException)
            {
                MainStatusBar.Text = "Vendor list update canceled.";
            }
            catch (Exception ex)
            {
                MainStatusBar.Text = "Vendor list update failed.";
                _ = MessageBox.Show(ex.Message, "Update Vendor List (OUI) from IEEE", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void WikiLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            WikiLink.LinkVisited = true;
            VisitUrl("https://github.com/zbalkan/DZMAC/wiki/Help#why-does-setting-the-first-octet-to-02-help-with-some-wi-fi-mac-changes");
        }

        private void ZeroTwoCheckBox_CheckedChanged(object sender, EventArgs e) => _locallyAdministered = ZeroTwoCheckBox.Checked;

        #endregion EventHandlers

        #region Private Methods

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
            if (Ipv4AddressListView == null)
            {
                return;
            }

            PopulateIpv4AddressRows(_selected?.Ipv4Addresses ?? Array.Empty<AdapterIpv4Address>());
            PopulateSingleValueRows(Ipv4GatewayListView, _selected?.Ipv4Gateways ?? Array.Empty<string>(), "No IPv4 gateway");
            PopulateSingleValueRows(Ipv4DnsListView, _selected?.Ipv4DnsServers ?? Array.Empty<string>(), "No IPv4 DNS server");

            PopulateIpv6AddressRows(_selected?.Ipv6Addresses ?? Array.Empty<AdapterIpv6Address>());
            PopulateSingleValueRows(Ipv6GatewayListView, _selected?.Ipv6Gateways ?? Array.Empty<string>(), "No IPv6 gateway");
            PopulateSingleValueRows(Ipv6DnsListView, _selected?.Ipv6DnsServers ?? Array.Empty<string>(), "No IPv6 DNS server");
        }

        private void BindSelection()
        {
            var hasSelection = _selected != null;
            if (!hasSelection)
            {
                SetConnectionDetailValue(ConnectionValueTextbox, null);
                SetConnectionDetailValue(DeviceValueTextbox, null);
                SetConnectionDetailValue(HardwareIdValueTextbox, null);
                SetConnectionDetailValue(ConfigIdValueTextbox, null);
                SetConnectionDetailValue(Ipv4ValueTextbox, null);
                SetConnectionDetailValue(Ipv6ValueTextbox, null);
                SetConnectionDetailValue(OriginalMacValueTextbox, null);
                SetConnectionDetailValue(OriginalMacVendorTextbox, null);
                SetConnectionDetailValue(ActiveMacValueTextbox, null);
                SetConnectionDetailValue(ActiveMacVendorTextbox, null);
                DhcpEnabledItem.Checked = false;
                BindIpAddressDetails();
                StopPerformanceMonitoring();
                UpdateSelectionState();
                return;
            }

            SetConnectionDetailValue(ConnectionValueTextbox, _selected.Name);
            SetConnectionDetailValue(DeviceValueTextbox, _selected.Device);
            SetConnectionDetailValue(HardwareIdValueTextbox, _selected.HardwareId);
            SetConnectionDetailValue(ConfigIdValueTextbox, _selected.ConfigId);
            SetConnectionDetailValue(Ipv4ValueTextbox, _selected.IPv4Status);
            SetConnectionDetailValue(Ipv6ValueTextbox, _selected.IPv6Status);
            SetConnectionDetailValue(OriginalMacValueTextbox, _selected.OriginalMac);
            SetConnectionDetailValue(OriginalMacVendorTextbox, _selected.OriginalVendor);
            SetConnectionDetailValue(ActiveMacValueTextbox, _selected.ActiveMac);
            SetConnectionDetailValue(ActiveMacVendorTextbox, _selected.ActiveVendor);
            DhcpEnabledItem.Checked = _selected.IsDhcpEnabled;
            BindIpAddressDetails();
            if (IsPerformanceTabVisible())
            {
                _ = RestartPerformanceMonitoringAsync(_selected.ConfigId);
            }
            UpdateSelectionState();
        }

        private void ConfigureV1Surface()
        {
            // Keep v1 UI surface aligned with implemented feature set.
            ExportReportItem.Visible = true;
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

        private string BuildTextReport()
        {
            var report = new StringBuilder();
            var version = Application.ProductVersion;
            report.AppendLine($"DZMAC MAC Address Changer");
            report.AppendLine("===================================================");
            report.AppendLine();
            report.AppendLine($"Date: {DateTime.Now:dddd, MMMM d, yyyy  HH:mm:ss}");
            report.AppendLine();
            report.AppendLine("Text Report");
            report.AppendLine("===========");
            report.AppendLine();

            for (var index = 0; index < NetworkConnections.Count; index++)
            {
                var detail = NetworkConnections[index].Detail;
                report.AppendLine($"Interface #{index + 1}");
                report.AppendLine("=============");
                AppendReportField(report, "Connection Name", detail.Name);
                AppendReportField(report, "Device Name", detail.Device);
                AppendReportField(report, "Device Manufacturer", detail.DeviceManufacturer);
                AppendReportField(report, "Hardware ID", detail.HardwareId);
                AppendReportField(report, "Configuration ID", detail.ConfigId);
                AppendReportField(report, "Active MAC Address", detail.ActiveMac);
                AppendReportField(report, "Active MAC Address Vendor", detail.ActiveVendor);
                AppendReportField(report, "Link Speed", detail.Speed.ToLowerInvariant());
                AppendReportField(report, "Link Status", FormatReportLinkStatus(detail));
                AppendReportField(report, "TCP/IPv4", (detail.IPv4Status == "Enabled").ToString());
                AppendReportField(report, "TCP/IPv6", (detail.IPv6Status == "Enabled").ToString());
                AppendReportField(report, "DHCPv4 Enabled", detail.IsDhcpEnabled.ToString());
                AppendIpv4AddressFields(report, detail);
                AppendMultiValueField(report, "IPv4 Default Gateway", detail.Ipv4Gateways, includeMetricPlaceholder: true);
                AppendMultiValueField(report, "IPv4 DNS Server", detail.Ipv4DnsServers, includeMetricPlaceholder: false);
                AppendReportField(report, "DHCPv6 Enabled", bool.FalseString);
                report.AppendLine();
            }

            return report.ToString();
        }

        private static void AppendIpv4AddressFields(StringBuilder report, NetworkConnectionDetail detail)
        {
            if (detail.Ipv4Addresses == null || detail.Ipv4Addresses.Count == 0)
            {
                return;
            }

            foreach (var address in detail.Ipv4Addresses)
            {
                var subnetMask = string.IsNullOrWhiteSpace(address.SubnetMask) ? "0.0.0.0" : address.SubnetMask;
                AppendReportField(report, "IPv4 Address", $"{address.Address} ({subnetMask})");
            }
        }

        private static void AppendMultiValueField(StringBuilder report, string label, IReadOnlyList<string> values, bool includeMetricPlaceholder)
        {
            if (values == null || values.Count == 0)
            {
                return;
            }

            foreach (var value in values.Where(v => !string.IsNullOrWhiteSpace(v)))
            {
                var formattedValue = includeMetricPlaceholder ? $"{value} (0)" : value;
                AppendReportField(report, label, formattedValue);
            }
        }

        private static void AppendReportField(StringBuilder report, string label, string value)
        {
            var safeValue = string.IsNullOrWhiteSpace(value) ? "N/A" : value;
            report.AppendLine($"{label,-40}{safeValue}");
        }

        private static string FormatReportLinkStatus(NetworkConnectionDetail detail)
        {
            var connectionState = detail.Enabled ? "Up" : "Down";
            var operationalState = detail.Enabled && !string.Equals(detail.Speed, "0 bps", StringComparison.OrdinalIgnoreCase)
                ? "Operational"
                : "Non Operational";
            return $"{connectionState}, {operationalState}";
        }

        private async Task InitializeNetworkInterfaceCacheAsync()
        {
            try
            {
                var map = await Task.Run(() => NetworkInterface
                        .GetAllNetworkInterfaces()
                        .GroupBy(nic => nic.Id)
                        .ToDictionary(group => group.Key, group => group.First(), StringComparer.OrdinalIgnoreCase));

                _networkInterfacesById = new ReadOnlyDictionary<string, NetworkInterface>(map);
            }
            catch
            {
                _networkInterfacesById = new ReadOnlyDictionary<string, NetworkInterface>(new Dictionary<string, NetworkInterface>());
            }
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

                    _vendors = vendors.OrderBy(v => v.Oui, StringComparer.OrdinalIgnoreCase).ThenBy(v => v.VendorName, StringComparer.Ordinal).ToList();
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
            Ipv4AddressListView.BeginUpdate();
            Ipv4AddressListView.Items.Clear();

            foreach (var entry in addresses)
            {
                Ipv4AddressListView.Items.Add(new ListViewItem(new[] { entry.Address, entry.SubnetMask }));
            }

            if (Ipv4AddressListView.Items.Count == 0)
            {
                Ipv4AddressListView.Items.Add(new ListViewItem(new[] { "No IPv4 address", string.Empty }));
            }

            Ipv4AddressListView.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
            Ipv4AddressListView.EndUpdate();
        }

        private void PopulateIpv6AddressRows(IEnumerable<AdapterIpv6Address> addresses)
        {
            Ipv6AddressListView.BeginUpdate();
            Ipv6AddressListView.Items.Clear();

            foreach (var entry in addresses)
            {
                Ipv6AddressListView.Items.Add(new ListViewItem(new[] { entry.Address, entry.PrefixLength.ToString() }));
            }

            if (Ipv6AddressListView.Items.Count == 0)
            {
                Ipv6AddressListView.Items.Add(new ListViewItem(new[] { "No IPv6 address", string.Empty }));
            }

            Ipv6AddressListView.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
            Ipv6AddressListView.EndUpdate();
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
            var selectedConfigId = _selected?.ConfigId;

            SetLoadingState(true, clearListWhileLoading);

            try
            {
                var updatedConnections = await Task.Run(() =>
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    var adapters = NetworkAdapterFactory
                        .GetNetworkAdapters(_vm)
                        .Where(adapter => ShowAllAdaptersItem.Checked || adapter.IsPhysicalAdapter)
                        .OrderByDescending(adapter => adapter.Enabled)
                        .ThenBy(adapter => adapter.Name, StringComparer.CurrentCultureIgnoreCase)
                        .ToList();
                    cancellationToken.ThrowIfCancellationRequested();

                    return adapters.Select(adapter => new NetworkConnection(adapter, ShowSpeedInKBytesPerSecItem.Checked, _adminService)).ToList();
                }, cancellationToken);

                if (cancellationToken.IsCancellationRequested || IsDisposed)
                {
                    DisposeConnections(updatedConnections);
                    return;
                }

                RunOnUiThread(() =>
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
                    RestoreSelection(selectedConfigId);
                    BindSelection();
                    MainStatusBar.Text = $"Loaded {NetworkConnections.Count} adapters.";
                });
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
                    RunOnUiThread(() =>
                    {
                        if (!IsDisposed)
                        {
                            SetLoadingState(false, false);
                            if (MainStatusBar.Text == "Loading network adapters...")
                            {
                                MainStatusBar.Text = "Ready";
                            }
                        }
                    });
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
                _loadingPanel.Visible = true;

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

        private void RestoreSelection(string selectedConfigId)
        {
            if (string.IsNullOrWhiteSpace(selectedConfigId) || NetworkConnections == null || NetworkConnections.Count == 0)
            {
                ConnectionsGrid.SelectedItem = ConnectionsGrid.GetItem(0);
                _selected = ConnectionsGrid.SelectedItem?.RowObject is NetworkConnection first ? first.Detail : null;
                return;
            }

            foreach (OLVListItem item in ConnectionsGrid.Items)
            {
                if (!(item.RowObject is NetworkConnection row))
                {
                    continue;
                }

                if (string.Equals(row.Detail.ConfigId, selectedConfigId, StringComparison.OrdinalIgnoreCase))
                {
                    ConnectionsGrid.SelectedItem = item;
                    _selected = row.Detail;
                    return;
                }
            }

            ConnectionsGrid.SelectedItem = ConnectionsGrid.GetItem(0);
            _selected = ConnectionsGrid.SelectedItem?.RowObject is NetworkConnection fallback ? fallback.Detail : null;
        }

        private void RunOnUiThread(Action action)
        {
            if (IsDisposed)
            {
                return;
            }

            if (InvokeRequired)
            {
                BeginInvoke(action);
                return;
            }

            action();
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

        private static string FormatBitsPerSecond(long bitsPerSecond)
        {
            if (bitsPerSecond >= 1000000000)
            {
                return $"{bitsPerSecond / 1000000000f:F2} Gbps";
            }

            if (bitsPerSecond >= 1000000)
            {
                return $"{bitsPerSecond / 1000000f:F2} Mbps";
            }

            if (bitsPerSecond >= 1000)
            {
                return $"{bitsPerSecond / 1000f:F2} Kbps";
            }

            return $"{bitsPerSecond} bps";
        }

        private static string FormatDataSize(long bytes)
        {
            string[] units = { "B", "KB", "MB", "GB", "TB" };
            double value = bytes;
            var unitIndex = 0;

            while (value >= 1024d && unitIndex < units.Length - 1)
            {
                value /= 1024d;
                unitIndex++;
            }

            return $"{value:F2} {units[unitIndex]}";
        }

        private bool IsPerformanceTabVisible() => InfoTabs.SelectedTab == InformationPage;

        private void DrawSampleSeries(Graphics graphics, double[] samples, double maxSample, Color color)
        {
            if (samples.Length < 2 || _performanceGraphPanel.Width < 2 || _performanceGraphPanel.Height < 2)
            {
                return;
            }

            var step = (_performanceGraphPanel.Width - 1f) / (performanceSampleCapacity - 1f);

            using (var pen = new Pen(color, 1f))
            {
                for (var i = 1; i < samples.Length; i++)
                {
                    var x1 = (i - 1) * step;
                    var x2 = i * step;
                    var y1 = _performanceGraphPanel.Height - 1f - (float)(samples[i - 1] / maxSample * (_performanceGraphPanel.Height - 1f));
                    var y2 = _performanceGraphPanel.Height - 1f - (float)(samples[i] / maxSample * (_performanceGraphPanel.Height - 1f));
                    graphics.DrawLine(pen, x1, y1, x2, y2);
                }
            }
        }

        private void InitializePerformancePanel()
        {
            var panel = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 5,
                BackColor = Color.White,
                Padding = new Padding(6)
            };
            panel.RowStyles.Add(new RowStyle(SizeType.Percent, 78f));
            panel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            panel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            panel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            panel.RowStyles.Add(new RowStyle(SizeType.AutoSize));

            _performanceGraphPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White
            };
            _performanceGraphPanel.Paint += PerformanceGraphPanel_Paint;

            _performanceReceivedLabel = new Label
            {
                Dock = DockStyle.Top,
                ForeColor = Color.FromArgb(192, 0, 0),
                BackColor = Color.White,
                AutoSize = false,
                Font = new Font("Consolas", 8.25f),
                Height = 16,
                Text = "Received: 0 B"
            };

            _performanceReceivedSpeedLabel = new Label
            {
                Dock = DockStyle.Top,
                ForeColor = Color.FromArgb(192, 0, 0),
                BackColor = Color.White,
                AutoSize = false,
                Font = new Font("Consolas", 8.25f),
                Height = 16,
                Text = "-Speed  : 0 bps (Peak 0 bps)"
            };

            _performanceSentLabel = new Label
            {
                Dock = DockStyle.Top,
                ForeColor = Color.Green,
                BackColor = Color.White,
                AutoSize = false,
                Font = new Font("Consolas", 8.25f),
                Height = 16,
                Text = "Sent    : 0 B"
            };

            _performanceSentSpeedLabel = new Label
            {
                Dock = DockStyle.Top,
                ForeColor = Color.Green,
                BackColor = Color.White,
                AutoSize = false,
                Font = new Font("Consolas", 8.25f),
                Height = 16,
                Text = "-Speed  : 0 bps (Peak 0 bps)"
            };

            panel.Controls.Add(_performanceGraphPanel, 0, 0);
            panel.Controls.Add(_performanceReceivedLabel, 0, 1);
            panel.Controls.Add(_performanceReceivedSpeedLabel, 0, 2);
            panel.Controls.Add(_performanceSentLabel, 0, 3);
            panel.Controls.Add(_performanceSentSpeedLabel, 0, 4);
            PerformanceCounterGroup.Controls.Add(panel);
            PerformanceCounterGroup.Text = string.Empty;
        }

        private void PerformanceGraphPanel_Paint(object sender, PaintEventArgs e)
        {
            var receivedValues = new double[performanceSampleCapacity];
            var sentValues = new double[performanceSampleCapacity];

            lock (_performanceBufferSync)
            {
                var start = (_sampleWriteIndex - _sampleCount + performanceSampleCapacity) % performanceSampleCapacity;
                for (var i = 0; i < _sampleCount; i++)
                {
                    var sourceIndex = (start + i) % performanceSampleCapacity;
                    var targetIndex = performanceSampleCapacity - _sampleCount + i;
                    receivedValues[targetIndex] = _receivedSamples[sourceIndex].Value;
                    sentValues[targetIndex] = _sentSamples[sourceIndex].Value;
                }
            }

            e.Graphics.Clear(Color.White);
            var maxSample = Math.Max(1d, Math.Max(receivedValues.Max(), sentValues.Max()));
            DrawSampleSeries(e.Graphics, receivedValues, maxSample, Color.Red);
            DrawSampleSeries(e.Graphics, sentValues, maxSample, Color.Green);
        }

        private async Task RunPerformanceSamplingLoopAsync(NetworkInterface networkInterface, CancellationToken cancellationToken)
        {
            long? previousReceivedBytes = null;
            long? previousSentBytes = null;
            long peakReceivedBitsPerSecond = 0;
            long peakSentBitsPerSecond = 0;

            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    var stats = networkInterface.GetIPv4Statistics();
                    var receivedBytes = stats.BytesReceived;
                    var sentBytes = stats.BytesSent;

                    if (!previousReceivedBytes.HasValue || !previousSentBytes.HasValue)
                    {
                        previousReceivedBytes = receivedBytes;
                        previousSentBytes = sentBytes;
                        await Task.Delay(performanceRefreshMs, cancellationToken).ConfigureAwait(false);
                        continue;
                    }

                    var receivedDelta = Math.Max(0L, receivedBytes - previousReceivedBytes.Value);
                    var sentDelta = Math.Max(0L, sentBytes - previousSentBytes.Value);

                    previousReceivedBytes = receivedBytes;
                    previousSentBytes = sentBytes;

                    var receivedBitsPerSecond = receivedDelta * 8L * 1000 / performanceRefreshMs;
                    var sentBitsPerSecond = sentDelta * 8L * 1000 / performanceRefreshMs;

                    peakReceivedBitsPerSecond = Math.Max(peakReceivedBitsPerSecond, receivedBitsPerSecond);
                    peakSentBitsPerSecond = Math.Max(peakSentBitsPerSecond, sentBitsPerSecond);

                    lock (_performanceBufferSync)
                    {
                        _receivedSamples[_sampleWriteIndex] = new PerformanceSample(DateTime.UtcNow, receivedBitsPerSecond);
                        _sentSamples[_sampleWriteIndex] = new PerformanceSample(DateTime.UtcNow, sentBitsPerSecond);
                        _sampleWriteIndex = (_sampleWriteIndex + 1) % performanceSampleCapacity;
                        _sampleCount = Math.Min(_sampleCount + 1, performanceSampleCapacity);
                    }

                    if (IsDisposed || !IsHandleCreated)
                    {
                        return;
                    }

                    if (Interlocked.CompareExchange(ref _performanceUiUpdatePending, 1, 0) == 0)
                    {
                        BeginInvoke(new Action(() =>
                        {
                            try
                            {
                                if (IsDisposed || cancellationToken.IsCancellationRequested || _selectedNetworkInterface == null || _selectedNetworkInterface.Id != networkInterface.Id || !IsPerformanceTabVisible())
                                {
                                    return;
                                }

                                _performanceReceivedLabel.Text = $"Received: {FormatDataSize(receivedBytes)}";
                                _performanceReceivedSpeedLabel.Text = $"-Speed  : {FormatBitsPerSecond(receivedBitsPerSecond)} (Peak {FormatBitsPerSecond(peakReceivedBitsPerSecond)})";
                                _performanceSentLabel.Text = $"Sent    : {FormatDataSize(sentBytes)}";
                                _performanceSentSpeedLabel.Text = $"-Speed  : {FormatBitsPerSecond(sentBitsPerSecond)} (Peak {FormatBitsPerSecond(peakSentBitsPerSecond)})";
                                _performanceGraphPanel.Invalidate();
                            }
                            finally
                            {
                                Interlocked.Exchange(ref _performanceUiUpdatePending, 0);
                            }
                        }));
                    }

                    await Task.Delay(performanceRefreshMs, cancellationToken).ConfigureAwait(false);
                }
                catch (OperationCanceledException)
                {
                    return;
                }
                catch
                {
                    if (!IsDisposed && IsHandleCreated)
                    {
                        BeginInvoke(new Action(StopPerformanceMonitoring));
                    }

                    return;
                }
            }
        }

        private Task RestartPerformanceMonitoringAsync(string networkConfigId)
        {
            StopPerformanceMonitoring();
            var resolveVersion = Interlocked.Increment(ref _performanceResolveVersion);

            _performanceReceivedLabel.Text = "Received: resolving adapter...";
            _performanceReceivedSpeedLabel.Text = "-Speed  : waiting for sample...";
            _performanceSentLabel.Text = "Sent    : resolving adapter...";
            _performanceSentSpeedLabel.Text = "-Speed  : waiting for sample...";
            ResetPerformanceBuffers();
            _performanceGraphPanel.Invalidate();

            _networkInterfacesById.TryGetValue(networkConfigId, out var resolvedInterface);

            if (IsDisposed || resolveVersion != _performanceResolveVersion || !IsPerformanceTabVisible() || _selected == null || _selected.ConfigId != networkConfigId)
            {
                return Task.CompletedTask;
            }

            _selectedNetworkInterface = resolvedInterface;
            if (resolvedInterface == null)
            {
                _performanceReceivedLabel.Text = "Received: unavailable";
                _performanceReceivedSpeedLabel.Text = "-Speed  : unavailable";
                _performanceSentLabel.Text = "Sent    : unavailable";
                _performanceSentSpeedLabel.Text = "-Speed  : unavailable";
                return Task.CompletedTask;
            }

            _performanceReceivedLabel.Text = "Received: waiting for sample...";
            _performanceReceivedSpeedLabel.Text = "-Speed  : waiting for sample...";
            _performanceSentLabel.Text = "Sent    : waiting for sample...";
            _performanceSentSpeedLabel.Text = "-Speed  : waiting for sample...";

            _performanceLoopCancellation = new CancellationTokenSource();
            _ = RunPerformanceSamplingLoopAsync(_selectedNetworkInterface, _performanceLoopCancellation.Token);
            return Task.CompletedTask;
        }

        private void StopPerformanceMonitoring()
        {
            Interlocked.Increment(ref _performanceResolveVersion);
            _performanceLoopCancellation?.Cancel();
            _performanceLoopCancellation?.Dispose();
            _performanceLoopCancellation = null;
            Interlocked.Exchange(ref _performanceUiUpdatePending, 0);
            _selectedNetworkInterface = null;
            ResetPerformanceBuffers();
            _performanceReceivedLabel.Text = "Received: 0 B";
            _performanceReceivedSpeedLabel.Text = "-Speed  : 0 bps (Peak 0 bps)";
            _performanceSentLabel.Text = "Sent    : 0 B";
            _performanceSentSpeedLabel.Text = "-Speed  : 0 bps (Peak 0 bps)";
            _performanceGraphPanel.Invalidate();
        }

        private void ResetPerformanceBuffers()
        {
            lock (_performanceBufferSync)
            {
                Array.Clear(_receivedSamples, 0, _receivedSamples.Length);
                Array.Clear(_sentSamples, 0, _sentSamples.Length);
                _sampleWriteIndex = 0;
                _sampleCount = 0;
            }
        }

        #endregion Private Methods
    }
}
