namespace System.Windows.Forms.Design
{
	internal partial class BindingFormattingDialog : global::System.Windows.Forms.Form
	{
		private void InitializeComponent()
		{
			global::System.ComponentModel.ComponentResourceManager componentResourceManager = new global::System.ComponentModel.ComponentResourceManager(typeof(global::System.Windows.Forms.Design.BindingFormattingDialog));
			this.explanationLabel = new global::System.Windows.Forms.Label();
			this.mainTableLayoutPanel = new global::System.Windows.Forms.TableLayoutPanel();
			this.propertiesTreeView = new global::System.Windows.Forms.TreeView();
			this.propertyLabel = new global::System.Windows.Forms.Label();
			this.dataSourcePicker = new global::System.Windows.Forms.Design.BindingFormattingWindowsFormsEditorService();
			this.bindingLabel = new global::System.Windows.Forms.Label();
			this.updateModeLabel = new global::System.Windows.Forms.Label();
			this.bindingUpdateDropDown = new global::System.Windows.Forms.ComboBox();
			this.formatControl1 = new global::System.Windows.Forms.Design.FormatControl();
			this.okCancelTableLayoutPanel = new global::System.Windows.Forms.TableLayoutPanel();
			this.okButton = new global::System.Windows.Forms.Button();
			this.cancelButton = new global::System.Windows.Forms.Button();
			this.mainTableLayoutPanel.SuspendLayout();
			this.okCancelTableLayoutPanel.SuspendLayout();
			base.ShowIcon = false;
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.explanationLabel, "explanationLabel");
			this.mainTableLayoutPanel.SetColumnSpan(this.explanationLabel, 3);
			this.explanationLabel.Name = "explanationLabel";
			componentResourceManager.ApplyResources(this.mainTableLayoutPanel, "mainTableLayoutPanel");
			this.mainTableLayoutPanel.ColumnStyles.Add(new global::System.Windows.Forms.ColumnStyle(global::System.Windows.Forms.SizeType.Percent, 100f));
			this.mainTableLayoutPanel.ColumnStyles.Add(new global::System.Windows.Forms.ColumnStyle());
			this.mainTableLayoutPanel.ColumnStyles.Add(new global::System.Windows.Forms.ColumnStyle());
			this.mainTableLayoutPanel.Controls.Add(this.okCancelTableLayoutPanel, 2, 4);
			this.mainTableLayoutPanel.Controls.Add(this.formatControl1, 1, 3);
			this.mainTableLayoutPanel.Controls.Add(this.bindingUpdateDropDown, 2, 2);
			this.mainTableLayoutPanel.Controls.Add(this.propertiesTreeView, 0, 2);
			this.mainTableLayoutPanel.Controls.Add(this.updateModeLabel, 2, 1);
			this.mainTableLayoutPanel.Controls.Add(this.dataSourcePicker, 1, 2);
			this.mainTableLayoutPanel.Controls.Add(this.explanationLabel, 0, 0);
			this.mainTableLayoutPanel.Controls.Add(this.bindingLabel, 1, 1);
			this.mainTableLayoutPanel.Controls.Add(this.propertyLabel, 0, 1);
			this.mainTableLayoutPanel.MinimumSize = new global::System.Drawing.Size(542, 283);
			this.mainTableLayoutPanel.Name = "mainTableLayoutPanel";
			this.mainTableLayoutPanel.RowStyles.Add(new global::System.Windows.Forms.RowStyle());
			this.mainTableLayoutPanel.RowStyles.Add(new global::System.Windows.Forms.RowStyle());
			this.mainTableLayoutPanel.RowStyles.Add(new global::System.Windows.Forms.RowStyle());
			this.mainTableLayoutPanel.RowStyles.Add(new global::System.Windows.Forms.RowStyle(global::System.Windows.Forms.SizeType.Percent, 100f));
			this.mainTableLayoutPanel.RowStyles.Add(new global::System.Windows.Forms.RowStyle());
			componentResourceManager.ApplyResources(this.propertiesTreeView, "propertiesTreeView");
			this.propertiesTreeView.Name = "propertiesTreeView";
			this.propertiesTreeView.HideSelection = false;
			this.propertiesTreeView.TreeViewNodeSorter = new global::System.Windows.Forms.Design.BindingFormattingDialog.TreeNodeComparer();
			this.mainTableLayoutPanel.SetRowSpan(this.propertiesTreeView, 2);
			this.propertiesTreeView.BeforeSelect += new global::System.Windows.Forms.TreeViewCancelEventHandler(this.propertiesTreeView_BeforeSelect);
			this.propertiesTreeView.AfterSelect += new global::System.Windows.Forms.TreeViewEventHandler(this.propertiesTreeView_AfterSelect);
			componentResourceManager.ApplyResources(this.propertyLabel, "propertyLabel");
			this.propertyLabel.Name = "propertyLabel";
			componentResourceManager.ApplyResources(this.dataSourcePicker, "dataSourcePicker");
			this.dataSourcePicker.Name = "dataSourcePicker";
			this.dataSourcePicker.PropertyValueChanged += new global::System.EventHandler(this.dataSourcePicker_PropertyValueChanged);
			componentResourceManager.ApplyResources(this.bindingLabel, "bindingLabel");
			this.bindingLabel.Name = "bindingLabel";
			componentResourceManager.ApplyResources(this.updateModeLabel, "updateModeLabel");
			this.updateModeLabel.Name = "updateModeLabel";
			this.bindingUpdateDropDown.FormattingEnabled = true;
			componentResourceManager.ApplyResources(this.bindingUpdateDropDown, "bindingUpdateDropDown");
			this.bindingUpdateDropDown.DropDownStyle = global::System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.bindingUpdateDropDown.Name = "bindingUpdateDropDown";
			this.bindingUpdateDropDown.Items.AddRange(new object[]
			{
				global::System.Windows.Forms.DataSourceUpdateMode.Never,
				global::System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged,
				global::System.Windows.Forms.DataSourceUpdateMode.OnValidation
			});
			this.bindingUpdateDropDown.SelectedIndexChanged += new global::System.EventHandler(this.bindingUpdateDropDown_SelectedIndexChanged);
			this.mainTableLayoutPanel.SetColumnSpan(this.formatControl1, 2);
			componentResourceManager.ApplyResources(this.formatControl1, "formatControl1");
			this.formatControl1.MinimumSize = new global::System.Drawing.Size(390, 237);
			this.formatControl1.Name = "formatControl1";
			this.formatControl1.NullValueTextBoxEnabled = true;
			componentResourceManager.ApplyResources(this.okCancelTableLayoutPanel, "okCancelTableLayoutPanel");
			this.okCancelTableLayoutPanel.AutoSizeMode = global::System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.okCancelTableLayoutPanel.ColumnStyles.Add(new global::System.Windows.Forms.ColumnStyle(global::System.Windows.Forms.SizeType.Percent, 50f));
			this.okCancelTableLayoutPanel.ColumnStyles.Add(new global::System.Windows.Forms.ColumnStyle(global::System.Windows.Forms.SizeType.Percent, 50f));
			this.okCancelTableLayoutPanel.Controls.Add(this.cancelButton, 1, 0);
			this.okCancelTableLayoutPanel.Controls.Add(this.okButton, 0, 0);
			this.okCancelTableLayoutPanel.Name = "okCancelTableLayoutPanel";
			this.okCancelTableLayoutPanel.RowStyles.Add(new global::System.Windows.Forms.RowStyle(global::System.Windows.Forms.SizeType.Absolute, 29f));
			componentResourceManager.ApplyResources(this.okButton, "okButton");
			this.okButton.Name = "okButton";
			this.okButton.DialogResult = global::System.Windows.Forms.DialogResult.OK;
			this.okButton.Click += new global::System.EventHandler(this.okButton_Click);
			componentResourceManager.ApplyResources(this.cancelButton, "cancelButton");
			this.cancelButton.Name = "cancelButton";
			this.cancelButton.DialogResult = global::System.Windows.Forms.DialogResult.Cancel;
			this.cancelButton.Click += new global::System.EventHandler(this.cancelButton_Click);
			componentResourceManager.ApplyResources(this, "$this");
			base.AutoScaleMode = global::System.Windows.Forms.AutoScaleMode.Font;
			base.StartPosition = global::System.Windows.Forms.FormStartPosition.CenterParent;
			base.CancelButton = this.cancelButton;
			base.AcceptButton = this.okButton;
			base.Controls.Add(this.mainTableLayoutPanel);
			base.FormBorderStyle = global::System.Windows.Forms.FormBorderStyle.Sizable;
			base.Name = "BindingFormattingDialog";
			this.mainTableLayoutPanel.ResumeLayout(false);
			this.mainTableLayoutPanel.PerformLayout();
			this.okCancelTableLayoutPanel.ResumeLayout(false);
			base.HelpButton = true;
			base.ShowInTaskbar = false;
			base.MinimizeBox = false;
			base.MaximizeBox = false;
			base.Load += new global::System.EventHandler(this.BindingFormattingDialog_Load);
			base.Closing += new global::System.ComponentModel.CancelEventHandler(this.BindingFormattingDialog_Closing);
			base.HelpButtonClicked += new global::System.ComponentModel.CancelEventHandler(this.BindingFormattingDialog_HelpButtonClicked);
			base.HelpRequested += new global::System.Windows.Forms.HelpEventHandler(this.BindingFormattingDialog_HelpRequested);
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		private global::System.Windows.Forms.Design.BindingFormattingWindowsFormsEditorService dataSourcePicker;

		private global::System.Windows.Forms.Label explanationLabel;

		private global::System.Windows.Forms.TableLayoutPanel mainTableLayoutPanel;

		private global::System.Windows.Forms.Label propertyLabel;

		private global::System.Windows.Forms.TreeView propertiesTreeView;

		private global::System.Windows.Forms.Label bindingLabel;

		private global::System.Windows.Forms.ComboBox bindingUpdateDropDown;

		private global::System.Windows.Forms.Label updateModeLabel;

		private global::System.Windows.Forms.Design.FormatControl formatControl1;

		private global::System.Windows.Forms.TableLayoutPanel okCancelTableLayoutPanel;

		private global::System.Windows.Forms.Button okButton;

		private global::System.Windows.Forms.Button cancelButton;
	}
}
