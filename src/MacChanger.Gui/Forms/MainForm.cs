using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using MacChanger.Gui.DTO;

namespace MacChanger.Gui.Forms
{
    public partial class MainForm : Form
    {
        internal List<NetworkConnection> NetworkConnections { get; set; }
        private const string zeroMacValue = "00-00-00-00-00-00";
        private readonly VendorManager _vm;
        private bool _locallyAdministered;
        private bool _reenableOnChange;
        private NetworkConnectionDetail _selected;
        public MainForm()
        {
            _vm = new VendorManager();
            InitializeComponent();
        }

        #region EventHandlers

        private void AboutItem_Click(object sender, EventArgs e) => new AboutBox().Show(this);

        private void AssociateItem_Click(object sender, EventArgs e) => NotImplemented();

        private void AutoStartCheckBox_CheckedChanged(object sender, EventArgs e) => _reenableOnChange = AutoStartCheckBox.Checked;

        private void ChangeMacButton_Click(object sender, EventArgs e)
        {
            var targetMac = macTextBox.Text;

            // Ignore default value to prevend accidents
            if (targetMac.Equals(zeroMacValue)) return;

            if (_selected.TryUpdateMac(new MacAddress(targetMac)))
            {
                _ = MessageBox.Show("Successfully updated MAC address", "MAC Address Change", MessageBoxButtons.OK, MessageBoxIcon.Information);
                RefreshConnectionsBackground();
            }
            else
            {
                _ = MessageBox.Show("Failed to update MAC address", "MAC Address Change", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CheckUpdateItem_Click(object sender, EventArgs e) => NotImplemented();

        private void CliParamsHelpItem_Click(object sender, EventArgs e) => NotImplemented();

        private void ConnectionsGrid_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ConnectionsGrid?.SelectedItem != null)
            {
                // The event is triggered twice: First c reset where index is 0xffffff (-1)
                // and the second is the actual value.
                if (ConnectionsGrid.SelectedItem.Index != -1)
                {
                    var row = ConnectionsGrid.SelectedItem.RowObject as NetworkConnection;
                    _selected = row.Detail;
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
                    Dhcp4EnabledItem.Checked = _selected.IsDhcpEnabled;
                }
            }
        }

        private void DeleteItem_Click(object sender, EventArgs e) => NotImplemented();

        private void Dhcp4EnabledItem_Click(object sender, EventArgs e)
        {
            if (Dhcp4EnabledItem.Checked)
            {
                if (_selected.TryDisableDhcp())
                {
                    Dhcp4EnabledItem.Checked = false;
                }
            }
            else
            {
                if (_selected.TryEnableDhcp())
                {
                    Dhcp4EnabledItem.Checked = true;
                }
            }
            RefreshConnectionsBackground();
        }

        private void ExitItem_Click(object sender, EventArgs e) => Close();

        private void ExportPresetItem_Click(object sender, EventArgs e) => NotImplemented();

        private void ExportReportItem_Click(object sender, EventArgs e) => NotImplemented();

        private void HelpTopicsItem_Click(object sender, EventArgs e) => VisitUrl("https://github.com/zbalkan/MacChanger");

        private void ImportPresetItem_Click(object sender, EventArgs e) => NotImplemented();

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            _selected?.Dispose();
            _vm?.Dispose();
        }

        private void MainForm_Load(object sender, EventArgs e)
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

            RefreshConnectionsBackground();

            // Fill in vendor combobox
            VendorComboBox.DataSource = _vm.GetVendorList().ToList();
            VendorComboBox.SelectedItem = null;
        }
        private void MainForm_Resize(object sender, EventArgs e) => ConnectionsGrid.AutoResizeColumns();

        private void OpenPresetItem_Click(object sender, EventArgs e) => NotImplemented();

        private void OptionsMenu_Click(object sender, EventArgs e) => NotImplemented();

        private void PersistentAddressCheckBox_CheckedChanged(object sender, EventArgs e) => NotImplemented();

        private void RandomMacButton_Click(object sender, EventArgs e)
        {
            var randomVendor = _vm.GetRandom();
            var randomMac = _selected.GetRandom(randomVendor.Oui);

            var vendor = _vm.FindByMac(randomMac, _locallyAdministered);
            VendorComboBox.SelectedItem = vendor;

            if (_locallyAdministered)
            {
                randomMac.SetAsLocallyAdministered();
            }
            macTextBox.Text = randomMac.ToString(MacAddress.MacDelimiter.Colon);
        }

        private async void RefreshItem_Click(object sender, EventArgs e)
        {
            MainStatusBar.Text = "Refreshing network interface data...";
            await RefreshConnectionsBackground();
            MainStatusBar.Text = "Ready";
        }

        private void RestoreMacButton_Click(object sender, EventArgs e)
        {
            if (_selected.Changed == "No") { return; }
            if (_selected.TryReset())
            {
                _ = MessageBox.Show("Successfully restored MAC address", "MAC Address Restore", MessageBoxButtons.OK, MessageBoxIcon.Information);

                if (_reenableOnChange && _selected.TryDisable())
                {
                    _ = _selected.TryEnable();
                }
                RefreshConnectionsBackground();
            }
            else
            {
                _ = MessageBox.Show("Failed to restore MAC address", "MAC Address Restore", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SavePresetAsItem_Click(object sender, EventArgs e) => NotImplemented();

        private void SavePresetItem_Click(object sender, EventArgs e) => NotImplemented();
        private async void UpdateOuiItem_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Vendor list update will take some time and the UI will be frozen in the meantime.\n\nAre you sure?", "Update Vendor List (OUI) from IEEE", MessageBoxButtons.YesNo);

            if (result != DialogResult.Yes)
            {
                return;
            }

            MainStatusBar.Text = "Downloading OUI data...";
            await Task.Factory.StartNew(() => BeginInvoke(new Action(UpdateVendorList)));
            MessageBox.Show("Vendor list updated.", "Update Vendor List (OUI) from IEEE", MessageBoxButtons.OK);
            MainStatusBar.Text = "Ready";
        }
        private void WikiLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // Specify that the link was visited.
            WikiLink.LinkVisited = true;

            // Navigate to a URL.
            VisitUrl("https://en.wikipedia.org/wiki/MAC_address#IEEE_802c_local_MAC_address_usage");
        }

        private void ZeroTwoCheckBox_CheckedChanged(object sender, EventArgs e) => _locallyAdministered = ZeroTwoCheckBox.Checked;
        #endregion EventHandlers

        #region Private Methods
        /// <summary>
        ///     A placeholder method for events not implemented.
        /// </summary>
        private static void NotImplemented() => _ = MessageBox.Show("Not implemented.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

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

        private void RefreshConnections()
        {
            ConnectionsGrid.BeginUpdate();
            NetworkConnections = NetworkAdapterFactory.GetNetworkAdapters(_vm).Select(adapter => new NetworkConnection(adapter)).ToList();
            ConnectionsGrid.DataSource = NetworkConnections;
            ConnectionsGrid.EndUpdate();
            ConnectionsGrid.AutoResizeColumns();
        }

        private Task RefreshConnectionsBackground() => Task.Factory.StartNew(() => Invoke(new Action(RefreshConnections)));

        private void UpdateVendorList() => _vm.Refresh();
        #endregion Private Methods
    }
}