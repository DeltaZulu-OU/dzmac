#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Dzmac.Core.Presets;

namespace Dzmac.Forms
{
    internal sealed partial class PresetSelectionDialog : Form
    {
        private readonly IReadOnlyList<TpfPreset> _presets;

        public IReadOnlyList<int> SelectedIndices
            => PresetCheckedListBox.CheckedIndices.Cast<int>().ToList();

        public PresetSelectionDialog(string title, IReadOnlyList<TpfPreset> presets)
        {
            _presets = presets ?? throw new ArgumentNullException(nameof(presets));

            InitializeComponent();
            Text = title;

            PresetCheckedListBox.SelectedIndexChanged += PresetCheckedListBox_SelectedIndexChanged;

            foreach (var preset in _presets)
            {
                PresetCheckedListBox.Items.Add(preset.Name, false);
            }

            if (PresetCheckedListBox.Items.Count > 0)
            {
                PresetCheckedListBox.SelectedIndex = 0;
            }
        }

        private void SelectAllButton_Click(object sender, EventArgs e) => SetAllChecks(checkedState: true);

        private void SelectNoneButton_Click(object sender, EventArgs e) => SetAllChecks(checkedState: false);

        private void PresetCheckedListBox_SelectedIndexChanged(object sender, EventArgs e) => RefreshProperties();

        private void SetAllChecks(bool checkedState)
        {
            for (var i = 0; i < PresetCheckedListBox.Items.Count; i++)
            {
                PresetCheckedListBox.SetItemChecked(i, checkedState);
            }
        }

        private void RefreshProperties()
        {
            PropertyListView.BeginUpdate();
            PropertyListView.Items.Clear();

            if (PresetCheckedListBox.SelectedIndex >= 0 && PresetCheckedListBox.SelectedIndex < _presets.Count)
            {
                foreach (var pair in TpfPresetFormatter.ToProperties(_presets[PresetCheckedListBox.SelectedIndex]))
                {
                    PropertyListView.Items.Add(new ListViewItem(new[] { pair.Key, pair.Value }));
                }
            }

            PropertyListView.EndUpdate();
        }
    }
}
