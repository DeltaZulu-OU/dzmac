using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using MacChanger.Gui.DTO;

namespace MacChanger.Gui.Forms
{
    public partial class MainForm : Form
    {
        internal List<NetworkConnection> NetworkConnections { get; set; }

        private NetworkConnection _selected;
        private readonly VendorManager _vm;

        public MainForm()
        {
            _vm = new VendorManager();
            InitializeComponent();
        }

        private static void NotImplemented() => _ = MessageBox.Show("Not implemented.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

        private void RefreshConnections()
        {
            ConnectionsGrid.BeginUpdate();
            var adapters = NetworkAdapterFactory.GetNetworkAdapters().ToList();
            NetworkConnections = new List<NetworkConnection>();
            foreach (var adapter in adapters)
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
        private void AssociateItem_Click(object sender, EventArgs e) => NotImplemented();

        private void ConnectionsGrid_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ConnectionsGrid?.SelectedItem != null)
            {
                // The event is triggered twice: First a reset where index is 0xffffff (-1)
                // and the second is the actual value.
                if (ConnectionsGrid.SelectedItem.Index != -1)
                {
                    _selected = ConnectionsGrid.SelectedItem.RowObject as NetworkConnection;
                    ConnectionNameLabel.Text = _selected.Name;
                    DeviceLabel.Text = _selected.Adapter.ManagedAdapter.Description;
                }
            }
        }

        private void DeleteItem_Click(object sender, EventArgs e) => NotImplemented();

        private void ExitItem_Click(object sender, EventArgs e) => Close();

        private void ExportPresetItem_Click(object sender, EventArgs e) => NotImplemented();

        private void ExportReportItem_Click(object sender, EventArgs e) => NotImplemented();

        private void ImportPresetItem_Click(object sender, EventArgs e) => NotImplemented();

        private void MainForm_Load(object sender, EventArgs e) => RefreshConnectionsBackground();

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

        private void HelpTopicsItem_Click(object sender, EventArgs e) => System.Diagnostics.Process.Start("https://github.com/zbalkan/MacChanger");

        private void CliParamsHelpItem_Click(object sender, EventArgs e) => NotImplemented();

        private void CheckUpdateItem_Click(object sender, EventArgs e) => NotImplemented();

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

        private void AboutItem_Click(object sender, EventArgs e) => new AboutBox().Show(this);

        #endregion EventHandlers

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            _selected?.Dispose();
            _vm?.Dispose();
        }
    }
}
