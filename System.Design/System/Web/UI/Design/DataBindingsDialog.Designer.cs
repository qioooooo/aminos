namespace System.Web.UI.Design
{
	internal sealed partial class DataBindingsDialog : global::System.Web.UI.Design.Util.DesignerForm
	{
		private void InitializeComponent()
		{
			this._instructionLabel = new global::System.Windows.Forms.Label();
			this._bindablePropsLabels = new global::System.Windows.Forms.Label();
			this._bindablePropsTree = new global::System.Windows.Forms.TreeView();
			this._allPropsCheckBox = new global::System.Windows.Forms.CheckBox();
			this._bindingLabel = new global::System.Windows.Forms.Label();
			this._fieldBindingRadio = new global::System.Windows.Forms.RadioButton();
			this._fieldLabel = new global::System.Windows.Forms.Label();
			this._fieldCombo = new global::System.Windows.Forms.ComboBox();
			this._formatLabel = new global::System.Windows.Forms.Label();
			this._formatCombo = new global::System.Windows.Forms.ComboBox();
			this._sampleLabel = new global::System.Windows.Forms.Label();
			this._sampleTextBox = new global::System.Windows.Forms.TextBox();
			this._exprBindingRadio = new global::System.Windows.Forms.RadioButton();
			this._exprTextBox = new global::System.Windows.Forms.TextBox();
			this._okButton = new global::System.Windows.Forms.Button();
			this._cancelButton = new global::System.Windows.Forms.Button();
			this._refreshSchemaLink = new global::System.Windows.Forms.LinkLabel();
			this._exprLabel = new global::System.Windows.Forms.Label();
			this._twoWayBindingCheckBox = new global::System.Windows.Forms.CheckBox();
			this._fieldBindingPanel = new global::System.Windows.Forms.Panel();
			this._customBindingPanel = new global::System.Windows.Forms.Panel();
			this._bindingOptionsPanel = new global::System.Windows.Forms.Panel();
			base.SuspendLayout();
			this._instructionLabel.FlatStyle = global::System.Windows.Forms.FlatStyle.System;
			this._instructionLabel.Location = new global::System.Drawing.Point(12, 12);
			this._instructionLabel.Name = "_instructionLabel";
			this._instructionLabel.Size = new global::System.Drawing.Size(508, 30);
			this._instructionLabel.TabIndex = 0;
			this._bindablePropsLabels.FlatStyle = global::System.Windows.Forms.FlatStyle.System;
			this._bindablePropsLabels.Location = new global::System.Drawing.Point(12, 52);
			this._bindablePropsLabels.Name = "_bindablePropsLabels";
			this._bindablePropsLabels.Size = new global::System.Drawing.Size(184, 16);
			this._bindablePropsLabels.TabIndex = 1;
			this._bindablePropsTree.HideSelection = false;
			this._bindablePropsTree.ImageIndex = -1;
			this._bindablePropsTree.Location = new global::System.Drawing.Point(12, 72);
			this._bindablePropsTree.Name = "_bindablePropsTree";
			this._bindablePropsTree.SelectedImageIndex = -1;
			this._bindablePropsTree.ShowLines = false;
			this._bindablePropsTree.ShowPlusMinus = false;
			this._bindablePropsTree.ShowRootLines = false;
			this._bindablePropsTree.Size = new global::System.Drawing.Size(184, 112);
			this._bindablePropsTree.TabIndex = 2;
			this._bindablePropsTree.Sorted = true;
			this._bindablePropsTree.AfterSelect += new global::System.Windows.Forms.TreeViewEventHandler(this.OnBindablePropsTreeAfterSelect);
			this._allPropsCheckBox.FlatStyle = global::System.Windows.Forms.FlatStyle.System;
			this._allPropsCheckBox.Location = new global::System.Drawing.Point(12, 190);
			this._allPropsCheckBox.Name = "_allPropsCheckBox";
			this._allPropsCheckBox.Size = new global::System.Drawing.Size(184, 40);
			this._allPropsCheckBox.TabIndex = 3;
			this._allPropsCheckBox.Visible = true;
			this._allPropsCheckBox.CheckedChanged += new global::System.EventHandler(this.OnShowAllCheckedChanged);
			this._allPropsCheckBox.TextAlign = global::System.Drawing.ContentAlignment.TopLeft;
			this._allPropsCheckBox.CheckAlign = global::System.Drawing.ContentAlignment.TopLeft;
			this._bindingLabel.Location = new global::System.Drawing.Point(210, 52);
			this._bindingLabel.Name = "_bindingGroupLabel";
			this._bindingLabel.Size = new global::System.Drawing.Size(306, 16);
			this._bindingLabel.TabIndex = 4;
			this._fieldLabel.FlatStyle = global::System.Windows.Forms.FlatStyle.System;
			this._fieldLabel.Location = new global::System.Drawing.Point(0, 4);
			this._fieldLabel.Name = "_fieldLabel";
			this._fieldLabel.Size = new global::System.Drawing.Size(104, 16);
			this._fieldLabel.TabIndex = 100;
			this._fieldCombo.DropDownStyle = global::System.Windows.Forms.ComboBoxStyle.DropDownList;
			this._fieldCombo.Location = new global::System.Drawing.Point(118, 0);
			this._fieldCombo.Name = "_fieldCombo";
			this._fieldCombo.Size = new global::System.Drawing.Size(164, 21);
			this._fieldCombo.TabIndex = 101;
			this._fieldCombo.SelectedIndexChanged += new global::System.EventHandler(this.OnFieldComboSelectedIndexChanged);
			this._formatLabel.FlatStyle = global::System.Windows.Forms.FlatStyle.System;
			this._formatLabel.Location = new global::System.Drawing.Point(0, 32);
			this._formatLabel.Name = "_formatLabel";
			this._formatLabel.Size = new global::System.Drawing.Size(114, 16);
			this._formatLabel.TabIndex = 102;
			this._formatCombo.Location = new global::System.Drawing.Point(118, 28);
			this._formatCombo.Name = "_formatCombo";
			this._formatCombo.Size = new global::System.Drawing.Size(164, 21);
			this._formatCombo.TabIndex = 103;
			this._formatCombo.LostFocus += new global::System.EventHandler(this.OnFormatComboLostFocus);
			this._formatCombo.TextChanged += new global::System.EventHandler(this.OnFormatComboTextChanged);
			this._formatCombo.SelectedIndexChanged += new global::System.EventHandler(this.OnFormatComboSelectedIndexChanged);
			this._sampleLabel.FlatStyle = global::System.Windows.Forms.FlatStyle.System;
			this._sampleLabel.Location = new global::System.Drawing.Point(0, 60);
			this._sampleLabel.Name = "_sampleLabel";
			this._sampleLabel.Size = new global::System.Drawing.Size(114, 16);
			this._sampleLabel.TabIndex = 104;
			this._sampleTextBox.Location = new global::System.Drawing.Point(118, 56);
			this._sampleTextBox.Name = "_sampleTextBox";
			this._sampleTextBox.ReadOnly = true;
			this._sampleTextBox.Size = new global::System.Drawing.Size(164, 20);
			this._sampleTextBox.TabIndex = 105;
			this._exprTextBox.Location = new global::System.Drawing.Point(0, 18);
			this._exprTextBox.Name = "_exprTextBox";
			this._exprTextBox.Size = new global::System.Drawing.Size(282, 20);
			this._exprTextBox.TabIndex = 201;
			this._exprTextBox.TextChanged += new global::System.EventHandler(this.OnExprTextBoxTextChanged);
			this._okButton.FlatStyle = global::System.Windows.Forms.FlatStyle.System;
			this._okButton.Location = new global::System.Drawing.Point(360, 279);
			this._okButton.Name = "_okButton";
			this._okButton.TabIndex = 7;
			this._okButton.Click += new global::System.EventHandler(this.OnOKButtonClick);
			this._cancelButton.DialogResult = global::System.Windows.Forms.DialogResult.Cancel;
			this._cancelButton.FlatStyle = global::System.Windows.Forms.FlatStyle.System;
			this._cancelButton.Location = new global::System.Drawing.Point(441, 279);
			this._cancelButton.Name = "_cancelButton";
			this._cancelButton.TabIndex = 8;
			this._refreshSchemaLink.Visible = false;
			this._refreshSchemaLink.Location = new global::System.Drawing.Point(12, 283);
			this._refreshSchemaLink.Name = "_refreshSchemaLink";
			this._refreshSchemaLink.Size = new global::System.Drawing.Size(197, 16);
			this._refreshSchemaLink.TabIndex = 6;
			this._refreshSchemaLink.TabStop = true;
			this._refreshSchemaLink.LinkClicked += new global::System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.OnRefreshSchemaLinkLinkClicked);
			this._exprLabel.Location = new global::System.Drawing.Point(0, 0);
			this._exprLabel.Name = "_exprLabel";
			this._exprLabel.Size = new global::System.Drawing.Size(290, 16);
			this._exprLabel.TabIndex = 200;
			this._twoWayBindingCheckBox.FlatStyle = global::System.Windows.Forms.FlatStyle.System;
			this._twoWayBindingCheckBox.Location = new global::System.Drawing.Point(118, 83);
			this._twoWayBindingCheckBox.Name = "_twoWayBindingCheckBox";
			this._twoWayBindingCheckBox.Size = new global::System.Drawing.Size(168, 30);
			this._twoWayBindingCheckBox.TabIndex = 106;
			this._twoWayBindingCheckBox.Enabled = true;
			this._twoWayBindingCheckBox.CheckedChanged += new global::System.EventHandler(this.OnTwoWayBindingChecked);
			this._twoWayBindingCheckBox.TextAlign = global::System.Drawing.ContentAlignment.TopLeft;
			this._twoWayBindingCheckBox.CheckAlign = global::System.Drawing.ContentAlignment.TopLeft;
			this._fieldBindingRadio.Checked = true;
			this._fieldBindingRadio.FlatStyle = global::System.Windows.Forms.FlatStyle.System;
			this._fieldBindingRadio.Location = new global::System.Drawing.Point(0, 0);
			this._fieldBindingRadio.Name = "_fieldBindingRadio";
			this._fieldBindingRadio.Size = new global::System.Drawing.Size(302, 18);
			this._fieldBindingRadio.TabIndex = 0;
			this._fieldBindingRadio.TabStop = true;
			this._fieldBindingRadio.CheckedChanged += new global::System.EventHandler(this.OnFieldBindingRadioCheckedChanged);
			this._exprBindingRadio.Location = new global::System.Drawing.Point(0, 127);
			this._exprBindingRadio.Name = "_exprBindingRadio";
			this._exprBindingRadio.Size = new global::System.Drawing.Size(302, 18);
			this._exprBindingRadio.TabIndex = 2;
			this._exprBindingRadio.CheckedChanged += new global::System.EventHandler(this.OnExprBindingRadioCheckedChanged);
			this._fieldBindingPanel.TabIndex = 1;
			this._fieldBindingPanel.Name = "_fieldBindingPanel";
			this._fieldBindingPanel.Location = new global::System.Drawing.Point(16, 20);
			this._fieldBindingPanel.Size = new global::System.Drawing.Size(286, 105);
			this._fieldBindingPanel.Controls.Add(this._fieldLabel);
			this._fieldBindingPanel.Controls.Add(this._fieldCombo);
			this._fieldBindingPanel.Controls.Add(this._formatLabel);
			this._fieldBindingPanel.Controls.Add(this._formatCombo);
			this._fieldBindingPanel.Controls.Add(this._sampleLabel);
			this._fieldBindingPanel.Controls.Add(this._sampleTextBox);
			this._fieldBindingPanel.Controls.Add(this._twoWayBindingCheckBox);
			this._customBindingPanel.TabIndex = 3;
			this._customBindingPanel.Name = "_customBindingPanel";
			this._customBindingPanel.Location = new global::System.Drawing.Point(16, 148);
			this._customBindingPanel.Size = new global::System.Drawing.Size(286, 54);
			this._customBindingPanel.Controls.Add(this._exprLabel);
			this._customBindingPanel.Controls.Add(this._exprTextBox);
			this._bindingOptionsPanel.TabIndex = 5;
			this._bindingOptionsPanel.Name = "_bindingOptionsPanel";
			this._bindingOptionsPanel.Location = new global::System.Drawing.Point(214, 76);
			this._bindingOptionsPanel.Size = new global::System.Drawing.Size(302, 200);
			this._bindingOptionsPanel.Controls.Add(this._fieldBindingRadio);
			this._bindingOptionsPanel.Controls.Add(this._exprBindingRadio);
			this._bindingOptionsPanel.Controls.Add(this._fieldBindingPanel);
			this._bindingOptionsPanel.Controls.Add(this._customBindingPanel);
			base.AcceptButton = this._okButton;
			base.CancelButton = this._cancelButton;
			base.ClientSize = new global::System.Drawing.Size(524, 314);
			base.Controls.AddRange(new global::System.Windows.Forms.Control[] { this._refreshSchemaLink, this._cancelButton, this._okButton, this._bindingLabel, this._allPropsCheckBox, this._bindablePropsTree, this._bindablePropsLabels, this._instructionLabel, this._bindingOptionsPanel });
			base.FormBorderStyle = global::System.Windows.Forms.FormBorderStyle.FixedDialog;
			base.Name = "DataBindingsDialog";
			base.InitializeForm();
			base.ResumeLayout(false);
		}

		private global::System.Windows.Forms.Label _instructionLabel;

		private global::System.Windows.Forms.Label _bindablePropsLabels;

		private global::System.Windows.Forms.TreeView _bindablePropsTree;

		private global::System.Windows.Forms.CheckBox _allPropsCheckBox;

		private global::System.Windows.Forms.Label _bindingLabel;

		private global::System.Windows.Forms.RadioButton _fieldBindingRadio;

		private global::System.Windows.Forms.Label _fieldLabel;

		private global::System.Windows.Forms.ComboBox _fieldCombo;

		private global::System.Windows.Forms.Label _formatLabel;

		private global::System.Windows.Forms.Label _sampleLabel;

		private global::System.Windows.Forms.TextBox _sampleTextBox;

		private global::System.Windows.Forms.RadioButton _exprBindingRadio;

		private global::System.Windows.Forms.TextBox _exprTextBox;

		private global::System.Windows.Forms.Button _okButton;

		private global::System.Windows.Forms.Button _cancelButton;

		private global::System.Windows.Forms.ComboBox _formatCombo;

		private global::System.Windows.Forms.Label _exprLabel;

		private global::System.Windows.Forms.LinkLabel _refreshSchemaLink;

		private global::System.Windows.Forms.CheckBox _twoWayBindingCheckBox;

		private global::System.Windows.Forms.Panel _fieldBindingPanel;

		private global::System.Windows.Forms.Panel _customBindingPanel;

		private global::System.Windows.Forms.Panel _bindingOptionsPanel;
	}
}
