using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using BrightIdeasSoftware;
using Dzmac.Core;
using Dzmac.Core.Reporting;
using Dzmac.DTO;
using Dzmac.Properties;

namespace Dzmac.Forms
{
    internal partial class MainForm : Form
    {
        private readonly struct PerformanceSample
        {
            public double Value { get; }

            public PerformanceSample(double value)
            {
                Value = value;
            }
        }

        private const string zeroMacValue = "00-00-00-00-00-00";
        private const int performanceSampleCapacity = 120;
        private const int performanceRefreshMs = 1000;
        private const int vendorComboBatchSize = 200;
        private const int vendorComboLoadAheadThreshold = 20;
        private readonly ToolTip _connectionDetailsTooltip;
        private readonly VendorList _vm;
        private readonly AdapterAdminService _adminService;
        private readonly TextNetworkReportBuilder _networkReportBuilder;
        private readonly object _performanceBufferSync = new object();
        private readonly System.Windows.Forms.Timer _loadingProgressTimer;
        private readonly PerformanceSample[] _receivedSamples = new PerformanceSample[performanceSampleCapacity];
        private readonly PerformanceSample[] _sentSamples = new PerformanceSample[performanceSampleCapacity];
        private readonly double[] _receivedPaintBuffer = new double[performanceSampleCapacity];
        private readonly double[] _sentPaintBuffer = new double[performanceSampleCapacity];
        private int _sampleCount;
        private int _sampleWriteIndex;
        private NetworkInterface _selectedNetworkInterface;
        private CancellationTokenSource _performanceLoopCancellation;
        private int _performanceResolveVersion;
        private int _performanceUiUpdatePending;
        private string _performanceHistoryConfigId;
        private bool _isRefreshing;
        private bool _isVendorListLoading;
        private bool _isVendorListReady;
        private bool _isVendorComboLoading;
        private bool _isAppendingVendorBatch;
        private bool _isStartupInitialized;
        private bool _locallyAdministered;
        private bool _persistOriginalMacRecord = true;
        private bool _reenableOnChange;
        private CancellationTokenSource _refreshCancellation;
        private CancellationTokenSource _vendorRefreshCancellation;
        private NetworkConnection _selected;
        private IReadOnlyDictionary<string, NetworkInterface> _networkInterfacesById = new ReadOnlyDictionary<string, NetworkInterface>(new Dictionary<string, NetworkInterface>());
        private List<Vendor> _vendorComboItems;
        private int _vendorComboLoadedCount;

        public MainForm()
        {
            _vm = new VendorList();
            _adminService = new AdapterAdminService();
            _networkReportBuilder = new TextNetworkReportBuilder();
            _connectionDetailsTooltip = new ToolTip();
            InitializeComponent();
            ConfigureV1Surface();
            _loadingPanel.BringToFront();
            _loadingPanel.Visible = true;
            _loadingProgressTimer = new System.Windows.Forms.Timer { Interval = 60 };
            _loadingProgressTimer.Tick += LoadingProgressTimer_Tick;
            _loadingProgressTimer.Start();

            InfoTabs.SelectedIndexChanged += InfoTabs_SelectedIndexChanged;
            Shown += MainForm_ShownAsync;
            ConnectionsGrid.EmptyListMsg = Resources.StatusNoAdaptersLoaded;
            VendorComboBox.Enabled = false;
            VendorComboBox.DropDown += VendorComboBox_DropDown;
            VendorComboBox.SelectionChangeCommitted += VendorComboBox_SelectionChangeCommitted;
            VendorComboBox.SelectedIndexChanged += VendorComboBox_SelectedIndexChanged;
            VendorComboBox.MouseWheel += VendorComboBox_MouseWheel;
            VendorComboBox.KeyDown += VendorComboBox_KeyDown;
            PersistentAddressCheckBox.Checked = true;
            UpdateSelectionState();
        }

        internal List<NetworkConnection> NetworkConnections { get; set; }

        #region EventHandlers

        private void AboutItem_Click(object sender, EventArgs e) => new AboutBox().Show(this);

        private void AssociateItem_Click(object sender, EventArgs e) => NotImplemented();

        private void AutoStartCheckBox_CheckedChanged(object sender, EventArgs e) => _reenableOnChange = AutoStartCheckBox.Checked;

        private async void ChangeMacButton_Click(object sender, EventArgs e)
        {
            if (_selected == null)
            {
                return;
            }

            var targetMac = macTextBox.Text.Replace("-", string.Empty).Replace(":", string.Empty);

            // Ignore default value to prevend accidents
            if (targetMac.Equals(zeroMacValue))
            {
                return;
            }

            if (!MacAddress.IsValidMac(targetMac))
            {
                _ = MessageBox.Show(Resources.MacAddressInvalid, Resources.MacAddressChange_Title, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var target = new MacAddress(targetMac);
            var progress = new Progress<string>(status => MainStatusBar.Text = status);
            ChangeMacButton.Enabled = false;
            RestoreMacButton.Enabled = false;

            try
            {
                var updateResult = await Task.Run(() => MacRotationService.TryRotateMac(_selected.Adapter, target, _persistOriginalMacRecord, progress));
                if (updateResult.Success)
                {
                    _ = MessageBox.Show(Resources.MacAddressUpdateSuccess, Resources.MacAddressChange_Title, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    _ = RefreshConnectionsBackground();
                }
                else
                {
                    _ = MessageBox.Show(updateResult.Message, Resources.MacAddressChange_Title, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            finally
            {
                UpdateSelectionState();
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
                    _selected = row;
                    BindSelection();
                }
            }
            else
            {
                _selected = null;
                BindSelection();
            }
        }

        private void DeleteItem_Click(object sender, EventArgs e)
        {
            if (_selected == null)
            {
                return;
            }

            var selectedAdapterName = _selected.Name;
            Diagnostics.Info("adapter_registry_delete_requested", ("adapter", selectedAdapterName));
            var confirmResult = MessageBox.Show(
                $"Are you sure you want to delete '{selectedAdapterName}' from the registry?\n\nThis can make Windows reinstall the adapter after a reboot or hardware re-scan.",
                Resources.ConfirmRegistryDelete_Title,
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (confirmResult != DialogResult.Yes)
            {
                MainStatusBar.Text = $"Registry delete canceled for {selectedAdapterName}.";
                Diagnostics.Info("adapter_registry_delete_cancelled", ("adapter", selectedAdapterName));
                return;
            }

            MainStatusBar.Text = $"Deleting {selectedAdapterName} from registry...";
            var result = _adminService.DeleteAdapterFromRegistry(_selected.Adapter);
            if (result.IsSuccess)
            {
                MainStatusBar.Text = $"Deleted {selectedAdapterName} from registry.";
                Diagnostics.Info("adapter_registry_delete_succeeded", ("adapter", selectedAdapterName));
                _ = MessageBox.Show(Resources.AdapterRegistryDeleteSuccess, Resources.DeleteAdapter_Title, MessageBoxButtons.OK, MessageBoxIcon.Information);
                _ = RefreshConnectionsBackground();
                return;
            }

            MainStatusBar.Text = $"Failed to delete {selectedAdapterName} from registry.";
            Diagnostics.Warning("adapter_registry_delete_failed", result.Message, ("adapter", selectedAdapterName), ("operationCode", result.Code.ToString()));
            _ = MessageBox.Show(
                $"Failed to delete network adapter from registry.\n\n{result.Message}",
                Resources.DeleteAdapterFailed_Title,
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }

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
                _ = MessageBox.Show(Resources.NoAdaptersToExport, Resources.ExportTextReport_Title, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using var dialog = new SaveFileDialog();
            dialog.Title = Resources.ExportTextReport_Title;
            dialog.Filter = Resources.ExportFileFilter;
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
                MainStatusBar.Text = Resources.ExportFailed;
                _ = MessageBox.Show(ex.Message, Resources.ExportTextReport_Title, MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
            _loadingProgressTimer?.Stop();
            _loadingProgressTimer?.Dispose();
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
                    MainStatusBar.Text = Resources.StartupFailed;
                    _ = MessageBox.Show(ex.Message, Resources.InitializationError_Title, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private async Task RunDhcpActionAsync(NetworkConnection selectedDetail, string actionName, string inProgressText, string successText, string failureText, Func<AdapterAdminResult> action)
        {
            var selectedAdapterName = selectedDetail.Name;
            MainStatusBar.Text = $"{inProgressText} {selectedAdapterName}...";

            var operationResult = await Task.Run(() => action());
            var operationMessage = operationResult.Message;

            if (operationResult.IsSuccess)
            {
                MainStatusBar.Text = $"{successText} {selectedAdapterName}.";
                MessageBox.Show(operationMessage, Resources.Success_Title, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            MainStatusBar.Text = $"{failureText} {selectedAdapterName}.";
            var failureDetail = $"{operationMessage} (Code: {operationResult.Code})";
            MessageBox.Show(failureDetail, Resources.Failure_Title, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            Diagnostics.Warning("dhcp_action_failed", $"Could not {actionName} for '{selectedAdapterName}'. {operationMessage}", ("operationCode", operationResult.Code.ToString()));
        }

        private void MainForm_Resize(object sender, EventArgs e) => ConnectionsGrid.AutoResizeColumns();

        private void LoadingProgressTimer_Tick(object sender, EventArgs e)
        {
            if (_loadingProgressBar == null || IsDisposed)
            {
                return;
            }

            var nextValue = _loadingProgressBar.Value + 3;
            _loadingProgressBar.Value = nextValue > _loadingProgressBar.Maximum ? _loadingProgressBar.Minimum : nextValue;
        }

        private void NetworkConnectionsItem_Click(object sender, EventArgs e) => OpenNetworkConnections();

        private void SetConnectionDetailValue(Label label, string value)
        {
            var displayValue = string.IsNullOrWhiteSpace(value) ? "..." : value;
            label.Text = displayValue;
            _connectionDetailsTooltip.SetToolTip(label, displayValue == "..." ? string.Empty : displayValue);
        }

        private void InfoTabs_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_selected == null)
            {
                StopPerformanceMonitoring();
                return;
            }

            if (!IsPerformanceTabVisible())
            {
                StopPerformanceMonitoring(resetPerformanceState: false);
                return;
            }

            _ = RestartPerformanceMonitoringAsync(_selected.ConfigId);
        }

        private void OpenPresetItem_Click(object sender, EventArgs e) => NotImplemented();

        private void PersistentAddressCheckBox_CheckedChanged(object sender, EventArgs e) => _persistOriginalMacRecord = PersistentAddressCheckBox.Checked;

        private void RandomMacButton_Click(object sender, EventArgs e)
        {
            if (_selected == null)
            {
                return;
            }

            if (!_isVendorListReady)
            {
                _ = MessageBox.Show(Resources.VendorListStillLoading, Resources.VendorListLoading_Title, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            Vendor randomVendor;
            try
            {
                randomVendor = _vm.GetRandom();
            }
            catch (DZMACException ex)
            {
                _ = MessageBox.Show(ex.Message, Resources.VendorListUnavailable_Title, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var randomMac = _selected.GetRandom(randomVendor.Oui);

            var matchedVendor = _vm.FindByMac(randomMac, _locallyAdministered);
            var vendorDisplayName = matchedVendor?.VendorName ?? randomVendor.VendorName;
            VendorComboBox.SelectedItem = null;
            VendorComboBox.Text = vendorDisplayName;

            if (_locallyAdministered)
            {
                randomMac = randomMac.AsLocallyAdministered();
            }

            macTextBox.Text = randomMac.ToString(MacAddress.MacDelimiter.Colon);
        }

        private async void RefreshItem_Click(object sender, EventArgs e) => await RefreshConnectionsBackground();

        private async void VendorComboBox_DropDown(object sender, EventArgs e) => await EnsureVendorComboDataSourceAsync();

        private void VendorComboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (!(VendorComboBox.SelectedItem is Vendor selectedVendor))
            {
                return;
            }

            var selectedVendorName = selectedVendor.VendorName;
            BeginInvoke(new Action(() =>
            {
                if (IsDisposed)
                {
                    return;
                }

                ResetVendorComboData(selectedVendorName);
            }));
        }

        private async void VendorComboBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down || e.KeyCode == Keys.PageDown || e.KeyCode == Keys.End)
            {
                await TryLoadNextVendorBatchIfNeededAsync();
            }
        }

        private async void VendorComboBox_MouseWheel(object sender, MouseEventArgs e) => await TryLoadNextVendorBatchIfNeededAsync();

        private async void VendorComboBox_SelectedIndexChanged(object sender, EventArgs e) => await TryLoadNextVendorBatchIfNeededAsync();

        private void RestoreMacButton_Click(object sender, EventArgs e)
        {
            if (_selected == null)
            {
                return;
            }

            if (!_selected.Adapter.Changed)
            {
                return;
            }

            if (_adminService.ResetRegistryMac(_selected.Adapter).IsSuccess)
            {
                _ = MessageBox.Show(Resources.MacAddressRestoreSuccess, Resources.MacAddressRestore_Title, MessageBoxButtons.OK, MessageBoxIcon.Information);

                if (_reenableOnChange && _adminService.SetAdapterEnabled(_selected.Adapter, false).IsSuccess)
                {
                    _ = _adminService.SetAdapterEnabled(_selected.Adapter, true);
                }

                _ = RefreshConnectionsBackground();
            }
            else
            {
                _ = MessageBox.Show(Resources.MacAddressRestoreFailed, Resources.MacAddressRestore_Title, MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                networkConnection.ShowSpeedInKBytesPerSec = showSpeedInKBytesPerSec;
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

        private async void ToggleAdapterEnabledItem_Click(object sender, EventArgs e)
        {
            if (!(ConnectionsGrid?.SelectedObject is NetworkConnection selectedConnection))
            {
                return;
            }

            await ToggleAdapterEnabledAsync(selectedConnection);
        }

        private async void UpdateOuiItem_ClickAsync(object sender, EventArgs e)
        {
            var result = MessageBox.Show(Resources.OuiDownloadConfirmMessage, Resources.UpdateOui_Title, MessageBoxButtons.YesNo);

            if (result != DialogResult.Yes)
            {
                return;
            }

            MainStatusBar.Text = Resources.StatusDownloadingOui;
            _vendorRefreshCancellation?.Cancel();
            _vendorRefreshCancellation?.Dispose();
            _vendorRefreshCancellation = new CancellationTokenSource();

            try
            {
                await _vm.RefreshAsync(_vendorRefreshCancellation.Token);
                _ = MessageBox.Show(Resources.VendorListUpdated, Resources.UpdateOui_Title, MessageBoxButtons.OK);
                ResetVendorComboData();
                MainStatusBar.Text = Resources.StatusReady;
            }
            catch (OperationCanceledException)
            {
                MainStatusBar.Text = Resources.StatusVendorListUpdateCancelled;
            }
            catch (Exception ex)
            {
                MainStatusBar.Text = Resources.StatusVendorListUpdateFailed;
                _ = MessageBox.Show(ex.Message, Resources.UpdateOui_Title, MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

        private static void DisposeConnections(List<NetworkConnection> connections, ISet<NetworkConnection> excludedConnections)
        {
            if (connections == null)
            {
                return;
            }

            foreach (var networkConnection in connections)
            {
                if (excludedConnections != null && excludedConnections.Contains(networkConnection))
                {
                    continue;
                }

                networkConnection.Dispose();
            }
        }

        /// <summary>
        ///     A placeholder method for events not implemented.
        /// </summary>
        private static void NotImplemented() => _ = MessageBox.Show(Resources.NotImplemented, Resources.Information_Title, MessageBoxButtons.OK, MessageBoxIcon.Information);

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
                _ = MessageBox.Show($"Unable to open Network Connections.{Environment.NewLine}{Environment.NewLine}{ex.Message}", Resources.NetworkConnections_Title, MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
            PopulateSingleValueRows(Ipv4GatewayListView, _selected?.Ipv4Gateways ?? Array.Empty<string>(), Resources.FallbackNoIpv4Gateway);
            PopulateSingleValueRows(Ipv4DnsListView, _selected?.Ipv4DnsServers ?? Array.Empty<string>(), Resources.FallbackNoIpv4DnsServer);

            PopulateIpv6AddressRows(_selected?.Ipv6Addresses ?? Array.Empty<AdapterIpv6Address>());
            PopulateSingleValueRows(Ipv6GatewayListView, _selected?.Ipv6Gateways ?? Array.Empty<string>(), Resources.FallbackNoIpv6Gateway);
            PopulateSingleValueRows(Ipv6DnsListView, _selected?.Ipv6DnsServers ?? Array.Empty<string>(), Resources.FallbackNoIpv6DnsServer);
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
            CliParamsHelpItem.Visible = true;
            toolStripSeparator6.Visible = true;
            CheckUpdateItem.Visible = false;

            if (InfoTabs.TabPages.Contains(PresetsPage))
            {
                InfoTabs.TabPages.Remove(PresetsPage);
            }

            PersistentAddressCheckBox.Visible = true;
        }

        private string BuildTextReport()
        {
            var entries = NetworkConnections.Select(connection => ToReportEntry(connection)).ToList();
            return _networkReportBuilder.BuildReport(entries, DateTime.Now, Application.ProductVersion);
        }

        private static NetworkReportEntry ToReportEntry(NetworkConnection connection)
        {
            if (connection == null)
            {
                throw new ArgumentNullException(nameof(connection));
            }

            var ipv4Addresses = new List<NetworkReportIpv4Address>();
            if (connection.Ipv4Addresses != null)
            {
                foreach (var address in connection.Ipv4Addresses)
                {
                    ipv4Addresses.Add(new NetworkReportIpv4Address
                    {
                        Address = address.Address,
                        SubnetMask = address.SubnetMask
                    });
                }
            }

            return new NetworkReportEntry
            {
                Name = connection.Name,
                Device = connection.Device,
                DeviceManufacturer = connection.DeviceManufacturer,
                HardwareId = connection.HardwareId,
                ConfigId = connection.ConfigId,
                ActiveMac = connection.ActiveMac,
                ActiveVendor = connection.ActiveVendor,
                Speed = connection.Speed,
                Enabled = connection.Enabled,
                IPv4Status = connection.IPv4Status,
                IPv6Status = connection.IPv6Status,
                IsDhcpEnabled = connection.IsDhcpEnabled,
                Ipv4Addresses = ipv4Addresses,
                Ipv4Gateways = connection.Ipv4Gateways ?? Array.Empty<string>(),
                Ipv4DnsServers = connection.Ipv4DnsServers ?? Array.Empty<string>()
            };
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
                MainStatusBar.Text = Resources.StatusLoadingVendorList;
            }

            try
            {
                await Task.Run(() =>
                {
                    _ = _vm.Count;
                });
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

                    VendorComboBox.Enabled = true;
                    _isVendorListReady = true;
                    UpdateSelectionState();

                    if (MainStatusBar.Text == Resources.StatusLoadingVendorList)
                    {
                        MainStatusBar.Text = Resources.StatusReady;
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

                        MainStatusBar.Text = Resources.StatusVendorListFailed;
                        _ = MessageBox.Show(ex.Message, Resources.VendorListFailed_Title, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }));
                }
            }
            finally
            {
                _isVendorListLoading = false;
            }
        }

        private async Task EnsureVendorComboDataSourceAsync()
        {
            if (!_isVendorListReady || _isVendorComboLoading || IsDisposed)
            {
                return;
            }

            _isVendorComboLoading = true;
            var existingStatus = MainStatusBar.Text;
            MainStatusBar.Text = Resources.StatusPreparingVendorList;

            try
            {
                _vendorComboItems ??= await Task.Run(() => _vm
                        .OrderBy(v => v.Oui, StringComparer.OrdinalIgnoreCase)
                        .ThenBy(v => v.VendorName, StringComparer.Ordinal)
                        .ToList());

                if (IsDisposed)
                {
                    return;
                }

                if (VendorComboBox.Items.Count == 0)
                {
                    VendorComboBox.BeginUpdate();
                    try
                    {
                        VendorComboBox.Items.Clear();
                        VendorComboBox.Items.AddRange(_vendorComboItems.Cast<object>().ToArray());
                    }
                    finally
                    {
                        VendorComboBox.EndUpdate();
                    }
                }
            }
            catch (Exception ex)
            {
                if (!IsDisposed)
                {
                    _ = MessageBox.Show(ex.Message, Resources.VendorListFailed_Title, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            finally
            {
                if (!IsDisposed && MainStatusBar.Text == Resources.StatusPreparingVendorList)
                {
                    MainStatusBar.Text = string.IsNullOrWhiteSpace(existingStatus) ? Resources.StatusReady : existingStatus;
                }

                _isVendorComboLoading = false;
            }
        }

        private async Task AppendNextVendorBatchAsync()
        {
            if (_isAppendingVendorBatch || _vendorComboItems == null || _vendorComboLoadedCount >= _vendorComboItems.Count || IsDisposed)
            {
                return;
            }

            _isAppendingVendorBatch = true;
            try
            {
                var startIndex = _vendorComboLoadedCount;
                var batch = await Task.Run(() => _vendorComboItems
                    .Skip(startIndex)
                    .Take(vendorComboBatchSize)
                    .ToArray());
                if (batch.Length == 0)
                {
                    return;
                }

                VendorComboBox.BeginUpdate();
                try
                {
                    VendorComboBox.Items.AddRange(batch.Cast<object>().ToArray());
                }
                finally
                {
                    VendorComboBox.EndUpdate();
                }

                _vendorComboLoadedCount += batch.Length;
                await Task.Yield();
            }
            finally
            {
                _isAppendingVendorBatch = false;
            }
        }

        private async Task TryLoadNextVendorBatchIfNeededAsync()
        {
            if (!VendorComboBox.DroppedDown || _vendorComboItems == null || _vendorComboLoadedCount >= _vendorComboItems.Count)
            {
                return;
            }

            var selectedIndex = VendorComboBox.SelectedIndex;
            if (selectedIndex < 0)
            {
                return;
            }

            var thresholdIndex = VendorComboBox.Items.Count - vendorComboLoadAheadThreshold;
            if (selectedIndex >= thresholdIndex)
            {
                await AppendNextVendorBatchAsync();
            }
        }

        private void ResetVendorComboData(string preserveText = null)
        {
            _vendorComboItems = null;
            _vendorComboLoadedCount = 0;
            _isAppendingVendorBatch = false;
            VendorComboBox.BeginUpdate();
            try
            {
                VendorComboBox.DataSource = null;
                VendorComboBox.Items.Clear();
                VendorComboBox.SelectedItem = null;
                if (!string.IsNullOrWhiteSpace(preserveText))
                {
                    VendorComboBox.Text = preserveText;
                }
            }
            finally
            {
                VendorComboBox.EndUpdate();
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
                Ipv4AddressListView.Items.Add(new ListViewItem(new[] { Resources.FallbackNoIpv4Address, string.Empty }));
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
                Ipv6AddressListView.Items.Add(new ListViewItem(new[] { Resources.FallbackNoIpv6Address, string.Empty }));
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
                var physicalOnly = !ShowAllAdaptersItem.Checked;
                var updatedConnections = await Task.Run(() =>
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    var adapters = NetworkAdapterFactory
                        .GetNetworkAdapters(_vm, physicalOnly)
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

                    var retainedDisabledConnections = GetRetainedDisabledConnections(updatedConnections, physicalOnly);
                    if (retainedDisabledConnections.Count > 0)
                    {
                        updatedConnections.AddRange(retainedDisabledConnections);
                        updatedConnections = updatedConnections
                            .OrderByDescending(connection => connection.Enabled)
                            .ThenBy(connection => connection.Name, StringComparer.CurrentCultureIgnoreCase)
                            .ToList();
                    }

                    ConnectionsGrid.BeginUpdate();
                    DisposeConnections(NetworkConnections, new HashSet<NetworkConnection>(retainedDisabledConnections));
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
                    MainStatusBar.Text = Resources.StatusAdapterLoadCancelled;
                }
            }
            catch (Exception ex)
            {
                if (!IsDisposed)
                {
                    MainStatusBar.Text = Resources.StatusFailedLoadAdapters;
                    _ = MessageBox.Show(ex.Message, Resources.AdapterDiscoveryFailed_Title, MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                _loadingPanel.Visible = true;
                _loadingProgressBar.Value = _loadingProgressBar.Minimum;
                if (_loadingProgressTimer != null && !_loadingProgressTimer.Enabled)
                {
                    _loadingProgressTimer.Start();
                }

                if (clearListWhileLoading)
                {
                    ConnectionsGrid.EmptyListMsg = Resources.StatusLoadingAdapters;
                    ConnectionsGrid.DataSource = Array.Empty<NetworkConnection>();
                    _selected = null;
                    BindSelection();
                }
            }
            else
            {
                _loadingPanel.Visible = false;
                if (_loadingProgressTimer != null && _loadingProgressTimer.Enabled)
                {
                    _loadingProgressTimer.Stop();
                }
                _loadingProgressBar.Value = _loadingProgressBar.Minimum;
                ConnectionsGrid.EmptyListMsg = Resources.StatusNoAdaptersLoaded;
            }

            UpdateSelectionState();
        }

        private void RestoreSelection(string selectedConfigId)
        {
            if (string.IsNullOrWhiteSpace(selectedConfigId) || NetworkConnections == null || NetworkConnections.Count == 0)
            {
                ConnectionsGrid.SelectedItem = ConnectionsGrid.GetItem(0);
                _selected = ConnectionsGrid.SelectedItem?.RowObject as NetworkConnection;
                return;
            }

            foreach (OLVListItem item in ConnectionsGrid.Items)
            {
                if (!(item.RowObject is NetworkConnection row))
                {
                    continue;
                }

                if (string.Equals(row.ConfigId, selectedConfigId, StringComparison.OrdinalIgnoreCase))
                {
                    ConnectionsGrid.SelectedItem = item;
                    _selected = row;
                    return;
                }
            }

            ConnectionsGrid.SelectedItem = ConnectionsGrid.GetItem(0);
            _selected = ConnectionsGrid.SelectedItem?.RowObject as NetworkConnection;
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
            ToggleAdapterEnabledItem.Enabled = enableActions;
            ToggleAdapterEnabledItem.Text = !hasSelection || !(ConnectionsGrid?.SelectedObject is NetworkConnection selectedConnection) || selectedConnection.Enabled ? Resources.ToggleDisableAdapter : Resources.ToggleEnableAdapter;
        }

        private List<NetworkConnection> GetRetainedDisabledConnections(IReadOnlyCollection<NetworkConnection> refreshedConnections, bool physicalOnly)
        {
            if (NetworkConnections == null || NetworkConnections.Count == 0)
            {
                return new List<NetworkConnection>();
            }

            var refreshedConfigIds = new HashSet<string>(
                refreshedConnections.Select(connection => connection.ConfigId),
                StringComparer.OrdinalIgnoreCase);

            var retainedConnections = NetworkConnections
                .Where(connection => !connection.Enabled
                                  && !refreshedConfigIds.Contains(connection.ConfigId)
                                  && (!physicalOnly || connection.Adapter.IsPhysicalAdapter))
                .ToList();

            if (retainedConnections.Count > 0)
            {
                Diagnostics.Info(
                    "adapter_discovery_retained_disabled",
                    ("retainedCount", retainedConnections.Count),
                    ("retainedAdapters", string.Join(", ", retainedConnections.Select(connection => $"{connection.Name}[{connection.ConfigId}]"))));
            }

            return retainedConnections;
        }

        private async Task ToggleAdapterEnabledAsync(NetworkConnection connection)
        {
            if (_isRefreshing || connection == null)
            {
                return;
            }

            var shouldEnable = !connection.Enabled;
            var selectedAdapterName = connection.Name;
            var targetState = shouldEnable ? "enable" : "disable";
            Diagnostics.Info("adapter_toggle_requested", ("adapter", selectedAdapterName), ("targetState", targetState));

            var approvalResult = MessageBox.Show(
                $"Are you sure you want to {targetState} '{selectedAdapterName}'?",
                Resources.ConfirmAdapterStateChange_Title,
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (approvalResult != DialogResult.Yes)
            {
                MainStatusBar.Text = $"Adapter {targetState} canceled for {selectedAdapterName}.";
                Diagnostics.Info("adapter_toggle_cancelled", ("adapter", selectedAdapterName), ("targetState", targetState));
                return;
            }

            MainStatusBar.Text = $"{(shouldEnable ? "Enabling" : "Disabling")} {selectedAdapterName}...";

            var operationResult = await Task.Run(() => connection.TrySetEnabled(shouldEnable));
            if (operationResult.IsSuccess)
            {
                MainStatusBar.Text = $"{(shouldEnable ? "Enabled" : "Disabled")} {selectedAdapterName}.";
                Diagnostics.Info("adapter_toggle_succeeded", ("adapter", selectedAdapterName), ("targetState", targetState));
                _selected = connection;
                UpdateSelectionState();
                ConnectionsGrid.RefreshObject(connection);
                _ = RefreshConnectionsBackground();
                return;
            }

            MainStatusBar.Text = $"Failed to {targetState} {selectedAdapterName}.";
            Diagnostics.Warning(
                "adapter_toggle_failed",
                operationResult.Message,
                ("adapter", selectedAdapterName),
                ("targetState", targetState),
                ("operationCode", operationResult.Code.ToString()));
            _ = MessageBox.Show(
                $"{operationResult.Message} (Code: {operationResult.Code})",
                Resources.AdapterStateChangeFailed_Title,
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
            ConnectionsGrid.RefreshObject(connection);
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

            using var pen = new Pen(color, 1f);
            for (var i = 1; i < samples.Length; i++)
            {
                var x1 = (i - 1) * step;
                var x2 = i * step;
                var y1 = _performanceGraphPanel.Height - 1f - (float)(samples[i - 1] / maxSample * (_performanceGraphPanel.Height - 1f));
                var y2 = _performanceGraphPanel.Height - 1f - (float)(samples[i] / maxSample * (_performanceGraphPanel.Height - 1f));
                graphics.DrawLine(pen, x1, y1, x2, y2);
            }
        }

        private void PerformanceGraphPanel_Paint(object sender, PaintEventArgs e)
        {
            var maxSample = 1d;

            lock (_performanceBufferSync)
            {
                var startFillIndex = performanceSampleCapacity - _sampleCount;
                if (startFillIndex > 0)
                {
                    Array.Clear(_receivedPaintBuffer, 0, startFillIndex);
                    Array.Clear(_sentPaintBuffer, 0, startFillIndex);
                }

                var start = (_sampleWriteIndex - _sampleCount + performanceSampleCapacity) % performanceSampleCapacity;
                for (var i = 0; i < _sampleCount; i++)
                {
                    var sourceIndex = (start + i) % performanceSampleCapacity;
                    var targetIndex = performanceSampleCapacity - _sampleCount + i;
                    var receivedValue = _receivedSamples[sourceIndex].Value;
                    var sentValue = _sentSamples[sourceIndex].Value;
                    _receivedPaintBuffer[targetIndex] = receivedValue;
                    _sentPaintBuffer[targetIndex] = sentValue;

                    if (receivedValue > maxSample)
                    {
                        maxSample = receivedValue;
                    }

                    if (sentValue > maxSample)
                    {
                        maxSample = sentValue;
                    }
                }
            }

            e.Graphics.Clear(Color.White);
            DrawSampleSeries(e.Graphics, _receivedPaintBuffer, maxSample, Color.Red);
            DrawSampleSeries(e.Graphics, _sentPaintBuffer, maxSample, Color.Green);
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
                        _receivedSamples[_sampleWriteIndex] = new PerformanceSample(receivedBitsPerSecond);
                        _sentSamples[_sampleWriteIndex] = new PerformanceSample(sentBitsPerSecond);
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
                        BeginInvoke(new Action(() => StopPerformanceMonitoring()));
                    }

                    return;
                }
            }
        }

        private Task RestartPerformanceMonitoringAsync(string networkConfigId)
        {
            var shouldResetPerformanceState = !string.Equals(_performanceHistoryConfigId, networkConfigId, StringComparison.OrdinalIgnoreCase);
            StopPerformanceMonitoring(resetPerformanceState: shouldResetPerformanceState);
            var resolveVersion = Interlocked.Increment(ref _performanceResolveVersion);

            if (shouldResetPerformanceState)
            {
                _performanceReceivedLabel.Text = Resources.PerfReceivedResolving;
                _performanceReceivedSpeedLabel.Text = Resources.PerfSpeedWaiting;
                _performanceSentLabel.Text = Resources.PerfSentResolving;
                _performanceSentSpeedLabel.Text = Resources.PerfSpeedWaiting;
                ResetPerformanceBuffers();
                _performanceGraphPanel.Invalidate();
            }

            _networkInterfacesById.TryGetValue(networkConfigId, out var resolvedInterface);

            if (IsDisposed || resolveVersion != _performanceResolveVersion || !IsPerformanceTabVisible() || _selected == null || _selected.ConfigId != networkConfigId)
            {
                return Task.CompletedTask;
            }

            _selectedNetworkInterface = resolvedInterface;
            if (resolvedInterface == null)
            {
                _performanceHistoryConfigId = null;
                _performanceReceivedLabel.Text = Resources.PerfReceivedUnavailable;
                _performanceReceivedSpeedLabel.Text = Resources.PerfSpeedUnavailable;
                _performanceSentLabel.Text = Resources.PerfSentUnavailable;
                _performanceSentSpeedLabel.Text = Resources.PerfSpeedUnavailable;
                return Task.CompletedTask;
            }

            _performanceReceivedLabel.Text = Resources.PerfReceivedWaiting;
            _performanceReceivedSpeedLabel.Text = Resources.PerfSpeedWaiting;
            _performanceSentLabel.Text = Resources.PerfSentWaiting;
            _performanceSentSpeedLabel.Text = Resources.PerfSpeedWaiting;

            _performanceLoopCancellation = new CancellationTokenSource();
            _performanceHistoryConfigId = networkConfigId;
            _ = RunPerformanceSamplingLoopAsync(_selectedNetworkInterface, _performanceLoopCancellation.Token);
            return Task.CompletedTask;
        }

        private void StopPerformanceMonitoring(bool resetPerformanceState = true)
        {
            Interlocked.Increment(ref _performanceResolveVersion);
            _performanceLoopCancellation?.Cancel();
            _performanceLoopCancellation?.Dispose();
            _performanceLoopCancellation = null;
            Interlocked.Exchange(ref _performanceUiUpdatePending, 0);
            _selectedNetworkInterface = null;
            if (resetPerformanceState)
            {
                _performanceHistoryConfigId = null;
                ResetPerformanceBuffers();
                _performanceReceivedLabel.Text = Resources.PerfReceivedZero;
                _performanceReceivedSpeedLabel.Text = Resources.PerfSpeedZero;
                _performanceSentLabel.Text = Resources.PerfSentZero;
                _performanceSentSpeedLabel.Text = Resources.PerfSpeedZero;
                _performanceGraphPanel.Invalidate();
            }
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
