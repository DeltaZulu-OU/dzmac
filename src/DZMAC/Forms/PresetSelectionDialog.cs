#nullable enable

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Dzmac.Core.Presets;

namespace Dzmac.Forms
{
    internal sealed class PresetSelectionDialog : Form
    {
        private readonly CheckedListBox _presetList;
        private readonly ListView _propertyList;

        public IReadOnlyList<int> SelectedIndices
            => _presetList.CheckedIndices.Cast<int>().ToList();

        public PresetSelectionDialog(string title, IReadOnlyList<TpfPreset> presets)
        {
            if (presets == null)
            {
                throw new ArgumentNullException(nameof(presets));
            }

            Text = title;
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MinimizeBox = false;
            MaximizeBox = false;
            Width = 760;
            Height = 420;

            var root = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 2
            };
            root.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            root.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            Controls.Add(root);

            var content = new SplitContainer
            {
                Dock = DockStyle.Fill,
                SplitterDistance = 280,
                IsSplitterFixed = false
            };
            root.Controls.Add(content, 0, 0);

            _presetList = new CheckedListBox
            {
                Dock = DockStyle.Fill,
                CheckOnClick = true
            };
            _presetList.SelectedIndexChanged += (_, __) => RefreshProperties(presets);
            content.Panel1.Controls.Add(_presetList);

            foreach (var preset in presets)
            {
                _presetList.Items.Add(preset.Name, false);
            }

            _propertyList = new ListView
            {
                Dock = DockStyle.Fill,
                View = View.Details,
                FullRowSelect = true,
                GridLines = true,
                HeaderStyle = ColumnHeaderStyle.Nonclickable
            };
            _propertyList.Columns.Add("Property", 180);
            _propertyList.Columns.Add("Value", 260);
            content.Panel2.Controls.Add(_propertyList);

            var leftButtons = new FlowLayoutPanel
            {
                Dock = DockStyle.Left,
                AutoSize = true,
                FlowDirection = FlowDirection.LeftToRight,
                Padding = new Padding(8, 4, 8, 8)
            };

            var selectAll = new Button { Text = "Select All", AutoSize = true };
            selectAll.Click += (_, __) => SetAllChecks(checkedState: true);
            leftButtons.Controls.Add(selectAll);

            var selectNone = new Button { Text = "Select None", AutoSize = true };
            selectNone.Click += (_, __) => SetAllChecks(checkedState: false);
            leftButtons.Controls.Add(selectNone);

            var rightButtons = new FlowLayoutPanel
            {
                Dock = DockStyle.Right,
                AutoSize = true,
                FlowDirection = FlowDirection.LeftToRight,
                Padding = new Padding(8, 4, 8, 8)
            };

            var ok = new Button
            {
                Text = "OK",
                AutoSize = true,
                DialogResult = DialogResult.OK
            };
            rightButtons.Controls.Add(ok);

            var cancel = new Button
            {
                Text = "Cancel",
                AutoSize = true,
                DialogResult = DialogResult.Cancel
            };
            rightButtons.Controls.Add(cancel);

            var footer = new Panel { Dock = DockStyle.Fill, Height = 40 };
            footer.Controls.Add(leftButtons);
            footer.Controls.Add(rightButtons);
            root.Controls.Add(footer, 0, 1);

            AcceptButton = ok;
            CancelButton = cancel;

            if (_presetList.Items.Count > 0)
            {
                _presetList.SelectedIndex = 0;
            }
        }

        private void SetAllChecks(bool checkedState)
        {
            for (var i = 0; i < _presetList.Items.Count; i++)
            {
                _presetList.SetItemChecked(i, checkedState);
            }
        }

        private void RefreshProperties(IReadOnlyList<TpfPreset> presets)
        {
            _propertyList.BeginUpdate();
            _propertyList.Items.Clear();

            if (_presetList.SelectedIndex >= 0 && _presetList.SelectedIndex < presets.Count)
            {
                foreach (var pair in TpfPresetFormatter.ToProperties(presets[_presetList.SelectedIndex]))
                {
                    _propertyList.Items.Add(new ListViewItem(new[] { pair.Key, pair.Value }));
                }
            }

            _propertyList.EndUpdate();
        }
    }
}
