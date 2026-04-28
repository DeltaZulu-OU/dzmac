using System;
using System.Windows.Forms;
using Dzmac.Core;
using Dzmac.Core.Presets;

namespace Dzmac.Forms
{
    internal sealed partial class PresetEditorDialog : Form
    {
        private readonly TpfPreset _workingCopy;

        public TpfPreset Preset => ClonePreset(_workingCopy);

        public PresetEditorDialog(string title, TpfPreset seed, bool startBlank = false)
        {
            if (seed == null)
            {
                throw new ArgumentNullException(nameof(seed));
            }

            _workingCopy = ClonePreset(seed);

            InitializeComponent();
            Icon = AppIconProvider.GetIcon();
            Text = title;

            SaveButton.Click += SaveButton_Click;

            _includeMacCheckBox.CheckedChanged += (_, __) => RefreshMacControlState();
            _useCustomMacRadio.CheckedChanged += (_, __) => RefreshMacControlState();

            _includeIpv4CheckBox.CheckedChanged += (_, __) => RefreshIpv4ControlState();
            _dhcpIpv4CheckBox.CheckedChanged += (_, __) => RefreshIpv4ControlState();
            _ipv4AddressCheckBox.CheckedChanged += (_, __) => RefreshIpv4ControlState();
            _ipv4GatewayCheckBox.CheckedChanged += (_, __) => RefreshIpv4ControlState();
            _ipv4DnsCheckBox.CheckedChanged += (_, __) => RefreshIpv4ControlState();

            _ipv4AddressGrid.ContextMenuStrip = CreateGridMenu(_ipv4AddressGrid, "IP");
            _ipv4GatewayGrid.ContextMenuStrip = CreateGridMenu(_ipv4GatewayGrid, "Gateway");
            _ipv4DnsGrid.ContextMenuStrip = CreateGridMenu(_ipv4DnsGrid, "DNS");

            HookGridEvents(_ipv4AddressGrid);
            HookGridEvents(_ipv4GatewayGrid);
            HookGridEvents(_ipv4DnsGrid);

            if (startBlank)
            {
                BindAsBlankPreset();
            }
            else
            {
                BindFromPreset(_workingCopy);
            }
        }

        private static DataGridView CreateIpv4Grid(params (string Header, float FillWeight)[] columns)
        {
            var grid = new DataGridView
            {
                Dock = DockStyle.Fill,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                AllowUserToResizeRows = false,
                MultiSelect = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                RowHeadersVisible = false,
                ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableWithoutHeaderText
            };

            foreach (var column in columns)
            {
                grid.Columns.Add(new DataGridViewTextBoxColumn
                {
                    HeaderText = column.Header,
                    AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                    FillWeight = column.FillWeight * 100F,
                    SortMode = DataGridViewColumnSortMode.NotSortable
                });
            }

            return grid;
        }

        private void HookGridEvents(DataGridView grid)
        {
            grid.CellMouseDown += (_, e) =>
            {
                if (e.RowIndex >= 0 && e.Button == MouseButtons.Right)
                {
                    grid.ClearSelection();
                    grid.Rows[e.RowIndex].Selected = true;
                    grid.CurrentCell = grid.Rows[e.RowIndex].Cells[0];
                }
            };

            grid.RowsAdded += (_, __) => RefreshIpv4GroupCaptions();
            grid.RowsRemoved += (_, __) => RefreshIpv4GroupCaptions();
            grid.CellValueChanged += (_, __) => RefreshIpv4GroupCaptions();
        }

        private ContextMenuStrip CreateGridMenu(DataGridView grid, string noun)
        {
            var menu = new ContextMenuStrip();
            var addItem = new ToolStripMenuItem($"Add {noun}");
            var removeItem = new ToolStripMenuItem($"Remove {noun}");
            var copyItem = new ToolStripMenuItem($"Copy {noun}");

            addItem.Click += (_, __) => AddGridRow(grid);
            removeItem.Click += (_, __) => RemoveSelectedGridRow(grid);
            copyItem.Click += (_, __) => CopySelectedGridRow(grid);

            menu.Items.Add(addItem);
            menu.Items.Add(removeItem);
            menu.Items.Add(new ToolStripSeparator());
            menu.Items.Add(copyItem);

            menu.Opening += (_, __) =>
            {
                var hasSelection = GetSelectedRow(grid) != null;
                addItem.Enabled = grid.Enabled && grid.Rows.Count == 0;
                removeItem.Enabled = grid.Enabled && hasSelection;
                copyItem.Enabled = grid.Enabled && hasSelection;
            };

            return menu;
        }

        private static void AddGridRow(DataGridView grid)
        {
            if (grid.Rows.Count > 0)
            {
                return;
            }

            var values = new object[grid.Columns.Count];
            grid.Rows.Add(values);
            grid.ClearSelection();
            grid.Rows[0].Selected = true;
            grid.CurrentCell = grid.Rows[0].Cells[0];
            grid.BeginEdit(true);
        }

        private static void RemoveSelectedGridRow(DataGridView grid)
        {
            var row = GetSelectedRow(grid);
            if (row == null)
            {
                return;
            }

            grid.Rows.Remove(row);
        }

        private static void CopySelectedGridRow(DataGridView grid)
        {
            var row = GetSelectedRow(grid);
            if (row == null)
            {
                return;
            }

            var parts = new string[grid.Columns.Count];
            for (var i = 0; i < grid.Columns.Count; i++)
            {
                parts[i] = row.Cells[i].Value?.ToString() ?? string.Empty;
            }

            Clipboard.SetText(string.Join("\t", parts));
        }

        private static DataGridViewRow GetSelectedRow(DataGridView grid)
        {
            if (grid.SelectedRows.Count > 0)
            {
                return grid.SelectedRows[0];
            }

            if (grid.CurrentRow != null && !grid.CurrentRow.IsNewRow)
            {
                return grid.CurrentRow;
            }

            return null;
        }

        private void BindFromPreset(TpfPreset preset)
        {
            _presetNameTextBox.Text = preset.Name;
            _includeMacCheckBox.Checked = true;

            switch (preset.MacMode)
            {
                case TpfMacMode.Random:
                    _useRandomMacRadio.Checked = true;
                    break;
                case TpfMacMode.RandomWith02:
                    _useRandom02MacRadio.Checked = true;
                    break;
                case TpfMacMode.Custom:
                    _useCustomMacRadio.Checked = true;
                    _customMacTextBox.Text = preset.CustomMac ?? string.Empty;
                    break;
                default:
                    _useOriginalMacRadio.Checked = true;
                    break;
            }

            var ipv4 = preset.Ipv4;
            _includeIpv4CheckBox.Checked = ipv4?.Enabled == true;
            _dhcpIpv4CheckBox.Checked = ipv4 != null && ipv4.Enabled && !ipv4.IsStatic;
            _ipv4AddressCheckBox.Checked = ipv4 != null && ipv4.Enabled && ipv4.IsStatic;
            _ipv4GatewayCheckBox.Checked = ipv4?.GatewayEnabled == true;
            _ipv4DnsCheckBox.Checked = ipv4?.DnsEnabled == true;

            SetSingleRow(_ipv4AddressGrid, ipv4?.Address, ipv4?.SubnetMask);
            SetSingleRow(
                _ipv4GatewayGrid,
                ipv4?.DefaultGateway,
                string.IsNullOrWhiteSpace(ipv4?.DefaultGateway) ? string.Empty : (ipv4?.GatewayMetric ?? 0).ToString());
            SetSingleRow(_ipv4DnsGrid, ipv4?.PrimaryDnsServer);

            RefreshMacControlState();
            RefreshIpv4ControlState();
        }

        private void BindAsBlankPreset()
        {
            _presetNameTextBox.Text = string.Empty;
            _includeMacCheckBox.Checked = false;
            _useRandomMacRadio.Checked = false;
            _useRandom02MacRadio.Checked = false;
            _useOriginalMacRadio.Checked = false;
            _useCustomMacRadio.Checked = false;
            _customMacTextBox.Text = string.Empty;

            _includeIpv4CheckBox.Checked = false;
            _dhcpIpv4CheckBox.Checked = false;
            _ipv4AddressCheckBox.Checked = false;
            _ipv4GatewayCheckBox.Checked = false;
            _ipv4DnsCheckBox.Checked = false;

            _ipv4AddressGrid.Rows.Clear();
            _ipv4GatewayGrid.Rows.Clear();
            _ipv4DnsGrid.Rows.Clear();

            RefreshMacControlState();
            RefreshIpv4ControlState();
        }

        private static void SetSingleRow(DataGridView grid, params string[] values)
        {
            grid.Rows.Clear();

            var hasValue = false;
            for (var i = 0; i < values.Length; i++)
            {
                if (!string.IsNullOrWhiteSpace(values[i]))
                {
                    hasValue = true;
                    break;
                }
            }

            if (!hasValue)
            {
                return;
            }

            var rowValues = new object[grid.Columns.Count];
            for (var i = 0; i < grid.Columns.Count && i < values.Length; i++)
            {
                rowValues[i] = values[i] ?? string.Empty;
            }

            grid.Rows.Add(rowValues);
        }

        private void RefreshMacControlState()
        {
            var macEnabled = _includeMacCheckBox.Checked;
            _useRandomMacRadio.Enabled = macEnabled;
            _useRandom02MacRadio.Enabled = macEnabled;
            _useOriginalMacRadio.Enabled = macEnabled;
            _useCustomMacRadio.Enabled = macEnabled;
            _customMacTextBox.Enabled = macEnabled && _useCustomMacRadio.Checked;
        }

        private void RefreshIpv4ControlState()
        {
            var include = _includeIpv4CheckBox.Checked;
            _dhcpIpv4CheckBox.Enabled = include;

            var useDhcp = include && _dhcpIpv4CheckBox.Checked;
            _ipv4AddressCheckBox.Enabled = include && !useDhcp;
            _ipv4GatewayCheckBox.Enabled = include && !useDhcp;
            _ipv4DnsCheckBox.Enabled = include;

            var useStaticAddress = include && !useDhcp && _ipv4AddressCheckBox.Checked;
            _ipv4AddressGroup.Enabled = useStaticAddress;
            _ipv4AddressGrid.Enabled = useStaticAddress;

            var useGateway = include && !useDhcp && _ipv4GatewayCheckBox.Checked;
            _ipv4GatewayGroup.Enabled = useGateway;
            _ipv4GatewayGrid.Enabled = useGateway;

            var useDns = include && _ipv4DnsCheckBox.Checked;
            _ipv4DnsGroup.Enabled = useDns;
            _ipv4DnsGrid.Enabled = useDns;

            _ipv4RightGroup.Text = useDhcp
                ? "Internet Protocol v4 [DHCPv4]"
                : "Internet Protocol v4 [Static IP]";

            RefreshIpv4GroupCaptions();
        }

        private void RefreshIpv4GroupCaptions()
        {
            _ipv4AddressGroup.Text = $"IP Address ({_ipv4AddressGrid.Rows.Count})";
            _ipv4GatewayGroup.Text = $"Gateway ({_ipv4GatewayGrid.Rows.Count})";
            _ipv4DnsGroup.Text = $"DNS Server ({_ipv4DnsGrid.Rows.Count})";
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            var name = (_presetNameTextBox.Text ?? string.Empty).Trim();
            if (name.Length == 0)
            {
                MessageBox.Show("Preset name is required.", "Preset", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                _presetNameTextBox.Focus();
                return;
            }

            if (_includeMacCheckBox.Checked && _useCustomMacRadio.Checked)
            {
                var normalizedMac = (_customMacTextBox.Text ?? string.Empty).Replace("-", string.Empty).Replace(":", string.Empty).Trim();
                if (!MacAddress.IsValidMac(normalizedMac))
                {
                    MessageBox.Show("Custom MAC address is invalid.", "Preset", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    _customMacTextBox.Focus();
                    return;
                }

                _workingCopy.CustomMac = normalizedMac;
            }
            else
            {
                _workingCopy.CustomMac = string.Empty;
            }

            _workingCopy.Name = name;
            _workingCopy.MacMode = ResolveMacMode();
            var ipv4Settings = BuildIpv4Settings();
            if (!TryValidateIpv4Settings(ipv4Settings, out var ipv4Error))
            {
                MessageBox.Show(ipv4Error, "Preset", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            _workingCopy.Ipv4 = ipv4Settings;

            DialogResult = DialogResult.OK;
            Close();
        }

        private TpfMacMode ResolveMacMode()
        {
            if (!_includeMacCheckBox.Checked)
            {
                return TpfMacMode.Original;
            }

            if (_useRandomMacRadio.Checked)
            {
                return TpfMacMode.Random;
            }

            if (_useRandom02MacRadio.Checked)
            {
                return TpfMacMode.RandomWith02;
            }

            if (_useCustomMacRadio.Checked)
            {
                return TpfMacMode.Custom;
            }

            return TpfMacMode.Original;
        }

        private TpfIpv4Settings BuildIpv4Settings()
        {
            if (!_includeIpv4CheckBox.Checked)
            {
                return new TpfIpv4Settings { Enabled = false };
            }

            var useDhcp = _dhcpIpv4CheckBox.Checked;
            var useStaticAddress = !useDhcp && _ipv4AddressCheckBox.Checked;
            var metric = 0;
            _ = int.TryParse(GetGridValue(_ipv4GatewayGrid, 0, 1), out metric);

            return new TpfIpv4Settings
            {
                Enabled = true,
                IsStatic = useStaticAddress,
                Address = GetGridValue(_ipv4AddressGrid, 0, 0),
                SubnetMask = GetGridValue(_ipv4AddressGrid, 0, 1),
                GatewayEnabled = !useDhcp && _ipv4GatewayCheckBox.Checked,
                DefaultGateway = GetGridValue(_ipv4GatewayGrid, 0, 0),
                GatewayMetric = metric,
                DnsEnabled = _ipv4DnsCheckBox.Checked,
                PrimaryDnsServer = GetGridValue(_ipv4DnsGrid, 0, 0)
            };
        }

        private static bool TryValidateIpv4Settings(TpfIpv4Settings ipv4, out string error)
        {
            error = string.Empty;
            if (ipv4 == null || !ipv4.Enabled)
            {
                return true;
            }

            if (ipv4.IsStatic && !IpAddressValidator.TryValidateIpv4Address(ipv4.Address, out _))
            {
                error = "IPv4 address is invalid.";
                return false;
            }

            if (ipv4.IsStatic && !IpAddressValidator.TryValidateIpv4SubnetMask(ipv4.SubnetMask, out _))
            {
                error = "IPv4 subnet mask is invalid.";
                return false;
            }

            if (ipv4.IsStatic && ipv4.GatewayEnabled && !string.IsNullOrWhiteSpace(ipv4.DefaultGateway)
                && !IpAddressValidator.TryValidateIpv4Address(ipv4.DefaultGateway, out _))
            {
                error = "IPv4 gateway is invalid.";
                return false;
            }

            if (ipv4.DnsEnabled && !string.IsNullOrWhiteSpace(ipv4.PrimaryDnsServer)
                && !IpAddressValidator.TryValidateIpv4Address(ipv4.PrimaryDnsServer, out _))
            {
                error = "IPv4 DNS server is invalid.";
                return false;
            }

            if (ipv4.DnsEnabled && string.IsNullOrWhiteSpace(ipv4.PrimaryDnsServer))
            {
                error = "IPv4 DNS server is required when DNS is enabled.";
                return false;
            }

            return true;
        }

        private static string GetGridValue(DataGridView grid, int row, int col)
        {
            if (grid.Rows.Count <= row || grid.Columns.Count <= col)
            {
                return string.Empty;
            }

            return grid.Rows[row].Cells[col].Value?.ToString()?.Trim() ?? string.Empty;
        }

        private static TpfPreset ClonePreset(TpfPreset preset)
        {
            return new TpfPreset
            {
                Name = preset.Name,
                MacMode = preset.MacMode,
                CustomMac = preset.CustomMac,
                Ipv4 = preset.Ipv4 == null
                    ? null
                    : new TpfIpv4Settings
                    {
                        Enabled = preset.Ipv4.Enabled,
                        IsStatic = preset.Ipv4.IsStatic,
                        Address = preset.Ipv4.Address,
                        SubnetMask = preset.Ipv4.SubnetMask,
                        GatewayEnabled = preset.Ipv4.GatewayEnabled,
                        DefaultGateway = preset.Ipv4.DefaultGateway,
                        GatewayMetric = preset.Ipv4.GatewayMetric,
                        DnsEnabled = preset.Ipv4.DnsEnabled,
                        PrimaryDnsServer = preset.Ipv4.PrimaryDnsServer
                    }
            };
        }
    }
}
