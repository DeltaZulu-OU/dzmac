using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using MacChanger.Gui.DTO;

namespace MacChanger.Gui.Forms
{
    public partial class MainForm : Form
    {
        private readonly VendorManager _vm;
        private NetworkConnectionDetail _selected;
        internal List<NetworkConnection> NetworkConnections { get; set; }
        public MainForm()
        {
            _vm = new VendorManager();
            InitializeComponent();
        }

        /// <summary>
        ///     A placeholder method for events not implemented.
        /// </summary>
        private static void NotImplemented() => _ = MessageBox.Show("Not implemented.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

        private void RefreshConnections()
        {
            ConnectionsGrid.BeginUpdate();
            NetworkConnections = new List<NetworkConnection>();
            foreach (var adapter in NetworkAdapterFactory.GetNetworkAdapters(_vm))
            {
                NetworkConnections.Add(new NetworkConnection(adapter));
            }
            ConnectionsGrid.DataSource = NetworkConnections;
            ConnectionsGrid.EndUpdate();
            ConnectionsGrid.AutoResizeColumns();
        }

        private Task RefreshConnectionsBackground() => Task.Factory.StartNew(() => Invoke(new Action(RefreshConnections)));

        private void UpdateVendorList() => _vm.Refresh();

        #region EventHandlers

        private void AboutItem_Click(object sender, EventArgs e) => new AboutBox().Show(this);

        private void AssociateItem_Click(object sender, EventArgs e) => NotImplemented();

        private void checkBox1_CheckedChanged(object sender, EventArgs e) => NotImplemented();
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
                }
            }
        }

        private void DeleteItem_Click(object sender, EventArgs e) => NotImplemented();

        private void ExitItem_Click(object sender, EventArgs e) => Close();

        private void ExportPresetItem_Click(object sender, EventArgs e) => NotImplemented();

        private void ExportReportItem_Click(object sender, EventArgs e) => NotImplemented();

        private void HelpTopicsItem_Click(object sender, EventArgs e) => System.Diagnostics.Process.Start("https://github.com/zbalkan/MacChanger");

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
        }

        private void MainForm_Resize(object sender, EventArgs e) => ConnectionsGrid.AutoResizeColumns();

        private void OpenPresetItem_Click(object sender, EventArgs e) => NotImplemented();

        private void OptionsMenu_Click(object sender, EventArgs e) => NotImplemented();

        private async void RefreshItem_Click(object sender, EventArgs e)
        {
            MainStatusBar.Text = "Refreshing network interface data...";
            await RefreshConnectionsBackground();
            MainStatusBar.Text = "Ready";
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
        #endregion EventHandlers

    }
}
