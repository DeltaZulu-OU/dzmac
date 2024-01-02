using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MacChanger.Gui
{
    public partial class MainForm : Form
    {
        internal List<NetworkConnection> NetworkConnections { get; set; }

        private NetworkConnection selected;

        public MainForm()
        {
            InitializeComponent();
        }

        private static void NotImplemented() => _ = MessageBox.Show("Not implemented.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

        private void AssociateItem_Click(object sender, EventArgs e) => NotImplemented();

        private void ConnectionsGrid_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ConnectionsGrid?.SelectedItem != null)
            {
                // The event is triggered twice: First a reset where index is 0xffffff (-1)
                // and the second is the actual value.
                if (ConnectionsGrid.SelectedItem.Index != -1)
                {
                    selected = ConnectionsGrid.SelectedItem.RowObject as NetworkConnection;
                    ConnectionNameLabel.Text = selected.Name;
                    DeviceLabel.Text = selected.Adapter.ManagedAdapter.Description;
                }
            }
        }

        private void DeleteItem_Click(object sender, EventArgs e) => NotImplemented();

        private void ExitItem_Click(object sender, EventArgs e) => Close();

        private void ExportPresetItem_Click(object sender, EventArgs e) => NotImplemented();

        private void ExportReportItem_Click(object sender, EventArgs e) => NotImplemented();

        private void HelpMenu_Click(object sender, EventArgs e) => NotImplemented();

        private void ImportPresetItem_Click(object sender, EventArgs e) => NotImplemented();

        private void MainForm_Load(object sender, EventArgs e) => RefreshConnectionsBackground();

        private void MainForm_Resize(object sender, EventArgs e) => ConnectionsGrid.AutoResizeColumns();

        private NetworkConnection Map(Adapter adapter) => new NetworkConnection
        {
            Enabled = true,
            Name = adapter.ManagedAdapter.Name,
            Changed = "No",
            MacAddress = adapter.Mac,
            LinkStatus = adapter.ManagedAdapter.OperationalStatus.ToString(),
            Speed = ReadableSpeed(adapter.ManagedAdapter.Speed.ToString()),
            Adapter = adapter
        };

        private void OpenPresetItem_Click(object sender, EventArgs e) => NotImplemented();

        private void OptionsMenu_Click(object sender, EventArgs e) => NotImplemented();

        private string ReadableSpeed(string speed)
        {
            var s = long.Parse(speed);

            if (s > 1000000000)
            {
                float v = s / 1000000000;
                return $"{v:F2} Gbps";
            }
            else if (s > 1000000)
            {
                float v = s / 1000000;
                return $"{v:F2} Mbps";
            }
            else if (s > 1000)
            {
                float v = s / 1000;
                return $"{v:F2} Kbps";
            }
            else if (s == -1)
            {
                return "0 bps";
            }

            return speed;
        }

        private void RefreshConnections()
        {
            ConnectionsGrid.BeginUpdate();
            var adapters = AdapterFactory.GetAdapters().ToList();
            NetworkConnections = new List<NetworkConnection>();
            foreach (var adapter in adapters)
            {
                NetworkConnections.Add(Map(adapter));
            }
            ConnectionsGrid.DataSource = NetworkConnections;
            ConnectionsGrid.EndUpdate();
            ConnectionsGrid.AutoResizeColumns();
        }

        private void RefreshConnectionsBackground() => _ = Task.Factory.StartNew(() => ConnectionsGrid.BeginInvoke(new Action(RefreshConnections))).ConfigureAwait(false);
        private void RefreshItem_Click(object sender, EventArgs e) => RefreshConnectionsBackground();
        private void SavePresetAsItem_Click(object sender, EventArgs e) => NotImplemented();
        private void SavePresetItem_Click(object sender, EventArgs e) => NotImplemented();
    }
}
