#nullable enable

namespace Dzmac.Forms
{
    internal sealed partial class PresetSelectionDialog
    {
        private System.ComponentModel.IContainer? components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components is not null))
            {
                components.Dispose();
            }

            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.RootLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.ContentSplitContainer = new System.Windows.Forms.SplitContainer();
            this.PresetCheckedListBox = new System.Windows.Forms.CheckedListBox();
            this.PropertyListView = new System.Windows.Forms.ListView();
            this.PropertyColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ValueColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.FooterPanel = new System.Windows.Forms.Panel();
            this.LeftButtonFlowPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.SelectAllButton = new System.Windows.Forms.Button();
            this.SelectNoneButton = new System.Windows.Forms.Button();
            this.RightButtonFlowPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.OkButton = new System.Windows.Forms.Button();
            this.CancelDialogButton = new System.Windows.Forms.Button();
            this.RootLayoutPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ContentSplitContainer)).BeginInit();
            this.ContentSplitContainer.Panel1.SuspendLayout();
            this.ContentSplitContainer.Panel2.SuspendLayout();
            this.ContentSplitContainer.SuspendLayout();
            this.FooterPanel.SuspendLayout();
            this.LeftButtonFlowPanel.SuspendLayout();
            this.RightButtonFlowPanel.SuspendLayout();
            this.SuspendLayout();
            //
            // RootLayoutPanel
            //
            this.RootLayoutPanel.ColumnCount = 1;
            this.RootLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.RootLayoutPanel.Controls.Add(this.ContentSplitContainer, 0, 0);
            this.RootLayoutPanel.Controls.Add(this.FooterPanel, 0, 1);
            this.RootLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RootLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.RootLayoutPanel.Name = "RootLayoutPanel";
            this.RootLayoutPanel.RowCount = 2;
            this.RootLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.RootLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.RootLayoutPanel.Size = new System.Drawing.Size(744, 381);
            this.RootLayoutPanel.TabIndex = 0;
            //
            // ContentSplitContainer
            //
            this.ContentSplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ContentSplitContainer.Location = new System.Drawing.Point(3, 3);
            this.ContentSplitContainer.Name = "ContentSplitContainer";
            //
            // ContentSplitContainer.Panel1
            //
            this.ContentSplitContainer.Panel1.Controls.Add(this.PresetCheckedListBox);
            //
            // ContentSplitContainer.Panel2
            //
            this.ContentSplitContainer.Panel2.Controls.Add(this.PropertyListView);
            this.ContentSplitContainer.Size = new System.Drawing.Size(738, 335);
            this.ContentSplitContainer.SplitterDistance = 280;
            this.ContentSplitContainer.TabIndex = 0;
            //
            // PresetCheckedListBox
            //
            this.PresetCheckedListBox.CheckOnClick = true;
            this.PresetCheckedListBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PresetCheckedListBox.FormattingEnabled = true;
            this.PresetCheckedListBox.Location = new System.Drawing.Point(0, 0);
            this.PresetCheckedListBox.Name = "PresetCheckedListBox";
            this.PresetCheckedListBox.Size = new System.Drawing.Size(280, 335);
            this.PresetCheckedListBox.TabIndex = 0;
            //
            // PropertyListView
            //
            this.PropertyListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.PropertyColumnHeader,
            this.ValueColumnHeader});
            this.PropertyListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PropertyListView.FullRowSelect = true;
            this.PropertyListView.GridLines = true;
            this.PropertyListView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.PropertyListView.HideSelection = false;
            this.PropertyListView.Location = new System.Drawing.Point(0, 0);
            this.PropertyListView.Name = "PropertyListView";
            this.PropertyListView.Size = new System.Drawing.Size(454, 335);
            this.PropertyListView.TabIndex = 0;
            this.PropertyListView.UseCompatibleStateImageBehavior = false;
            this.PropertyListView.View = System.Windows.Forms.View.Details;
            //
            // PropertyColumnHeader
            //
            this.PropertyColumnHeader.Text = "Property";
            this.PropertyColumnHeader.Width = 180;
            //
            // ValueColumnHeader
            //
            this.ValueColumnHeader.Text = "Value";
            this.ValueColumnHeader.Width = 260;
            //
            // FooterPanel
            //
            this.FooterPanel.Controls.Add(this.LeftButtonFlowPanel);
            this.FooterPanel.Controls.Add(this.RightButtonFlowPanel);
            this.FooterPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.FooterPanel.Location = new System.Drawing.Point(3, 344);
            this.FooterPanel.Name = "FooterPanel";
            this.FooterPanel.Size = new System.Drawing.Size(738, 34);
            this.FooterPanel.TabIndex = 1;
            //
            // LeftButtonFlowPanel
            //
            this.LeftButtonFlowPanel.AutoSize = true;
            this.LeftButtonFlowPanel.Controls.Add(this.SelectAllButton);
            this.LeftButtonFlowPanel.Controls.Add(this.SelectNoneButton);
            this.LeftButtonFlowPanel.Dock = System.Windows.Forms.DockStyle.Left;
            this.LeftButtonFlowPanel.FlowDirection = System.Windows.Forms.FlowDirection.LeftToRight;
            this.LeftButtonFlowPanel.Location = new System.Drawing.Point(0, 0);
            this.LeftButtonFlowPanel.Name = "LeftButtonFlowPanel";
            this.LeftButtonFlowPanel.Padding = new System.Windows.Forms.Padding(8, 4, 8, 8);
            this.LeftButtonFlowPanel.Size = new System.Drawing.Size(181, 34);
            this.LeftButtonFlowPanel.TabIndex = 0;
            //
            // SelectAllButton
            //
            this.SelectAllButton.AutoSize = true;
            this.SelectAllButton.Location = new System.Drawing.Point(11, 7);
            this.SelectAllButton.Name = "SelectAllButton";
            this.SelectAllButton.Size = new System.Drawing.Size(67, 23);
            this.SelectAllButton.TabIndex = 0;
            this.SelectAllButton.Text = "Select All";
            this.SelectAllButton.UseVisualStyleBackColor = true;
            this.SelectAllButton.Click += new System.EventHandler(this.SelectAllButton_Click);
            //
            // SelectNoneButton
            //
            this.SelectNoneButton.AutoSize = true;
            this.SelectNoneButton.Location = new System.Drawing.Point(84, 7);
            this.SelectNoneButton.Name = "SelectNoneButton";
            this.SelectNoneButton.Size = new System.Drawing.Size(78, 23);
            this.SelectNoneButton.TabIndex = 1;
            this.SelectNoneButton.Text = "Select None";
            this.SelectNoneButton.UseVisualStyleBackColor = true;
            this.SelectNoneButton.Click += new System.EventHandler(this.SelectNoneButton_Click);
            //
            // RightButtonFlowPanel
            //
            this.RightButtonFlowPanel.AutoSize = true;
            this.RightButtonFlowPanel.Controls.Add(this.OkButton);
            this.RightButtonFlowPanel.Controls.Add(this.CancelDialogButton);
            this.RightButtonFlowPanel.Dock = System.Windows.Forms.DockStyle.Right;
            this.RightButtonFlowPanel.FlowDirection = System.Windows.Forms.FlowDirection.LeftToRight;
            this.RightButtonFlowPanel.Location = new System.Drawing.Point(582, 0);
            this.RightButtonFlowPanel.Name = "RightButtonFlowPanel";
            this.RightButtonFlowPanel.Padding = new System.Windows.Forms.Padding(8, 4, 8, 8);
            this.RightButtonFlowPanel.Size = new System.Drawing.Size(156, 34);
            this.RightButtonFlowPanel.TabIndex = 1;
            //
            // OkButton
            //
            this.OkButton.AutoSize = true;
            this.OkButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.OkButton.Location = new System.Drawing.Point(11, 7);
            this.OkButton.Name = "OkButton";
            this.OkButton.Size = new System.Drawing.Size(40, 23);
            this.OkButton.TabIndex = 0;
            this.OkButton.Text = "OK";
            this.OkButton.UseVisualStyleBackColor = true;
            //
            // CancelDialogButton
            //
            this.CancelDialogButton.AutoSize = true;
            this.CancelDialogButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CancelDialogButton.Location = new System.Drawing.Point(57, 7);
            this.CancelDialogButton.Name = "CancelDialogButton";
            this.CancelDialogButton.Size = new System.Drawing.Size(56, 23);
            this.CancelDialogButton.TabIndex = 1;
            this.CancelDialogButton.Text = "Cancel";
            this.CancelDialogButton.UseVisualStyleBackColor = true;
            //
            // PresetSelectionDialog
            //
            this.AcceptButton = this.OkButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.CancelDialogButton;
            this.ClientSize = new System.Drawing.Size(744, 381);
            this.Controls.Add(this.RootLayoutPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PresetSelectionDialog";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.ContentSplitContainer.Panel1.ResumeLayout(false);
            this.ContentSplitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ContentSplitContainer)).EndInit();
            this.ContentSplitContainer.ResumeLayout(false);
            this.RootLayoutPanel.ResumeLayout(false);
            this.FooterPanel.ResumeLayout(false);
            this.FooterPanel.PerformLayout();
            this.LeftButtonFlowPanel.ResumeLayout(false);
            this.LeftButtonFlowPanel.PerformLayout();
            this.RightButtonFlowPanel.ResumeLayout(false);
            this.RightButtonFlowPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel? RootLayoutPanel;
        private System.Windows.Forms.SplitContainer? ContentSplitContainer;
        private System.Windows.Forms.CheckedListBox? PresetCheckedListBox;
        private System.Windows.Forms.ListView? PropertyListView;
        private System.Windows.Forms.Panel? FooterPanel;
        private System.Windows.Forms.FlowLayoutPanel? LeftButtonFlowPanel;
        private System.Windows.Forms.FlowLayoutPanel? RightButtonFlowPanel;
        private System.Windows.Forms.Button? SelectAllButton;
        private System.Windows.Forms.Button? SelectNoneButton;
        private System.Windows.Forms.Button? OkButton;
        private System.Windows.Forms.Button? CancelDialogButton;
        private System.Windows.Forms.ColumnHeader? PropertyColumnHeader;
        private System.Windows.Forms.ColumnHeader? ValueColumnHeader;
    }
}
