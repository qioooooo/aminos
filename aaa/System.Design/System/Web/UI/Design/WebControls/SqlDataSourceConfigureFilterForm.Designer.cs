namespace System.Web.UI.Design.WebControls
{
	// Token: 0x020004B3 RID: 1203
	internal partial class SqlDataSourceConfigureFilterForm : global::System.Web.UI.Design.Util.DesignerForm
	{
		// Token: 0x06002B95 RID: 11157 RVA: 0x000F0AD0 File Offset: 0x000EFAD0
		private void InitializeComponent()
		{
			this._helpLabel = new global::System.Windows.Forms.Label();
			this._columnLabel = new global::System.Windows.Forms.Label();
			this._columnsComboBox = new global::System.Web.UI.Design.Util.AutoSizeComboBox();
			this._operatorsComboBox = new global::System.Web.UI.Design.Util.AutoSizeComboBox();
			this._operatorLabel = new global::System.Windows.Forms.Label();
			this._whereClausesLabel = new global::System.Windows.Forms.Label();
			this._addButton = new global::System.Windows.Forms.Button();
			this._removeButton = new global::System.Windows.Forms.Button();
			this._cancelButton = new global::System.Windows.Forms.Button();
			this._okButton = new global::System.Windows.Forms.Button();
			this._expressionLabel = new global::System.Windows.Forms.Label();
			this._propertiesGroupBox = new global::System.Windows.Forms.GroupBox();
			this._propertiesPanel = new global::System.Windows.Forms.Panel();
			this._sourceComboBox = new global::System.Web.UI.Design.Util.AutoSizeComboBox();
			this._sourceLabel = new global::System.Windows.Forms.Label();
			this._expressionTextBox = new global::System.Windows.Forms.TextBox();
			this._whereClausesListView = new global::System.Windows.Forms.ListView();
			this._expressionColumnHeader = new global::System.Windows.Forms.ColumnHeader("");
			this._valueColumnHeader = new global::System.Windows.Forms.ColumnHeader("");
			this._valueTextBox = new global::System.Windows.Forms.TextBox();
			this._valueLabel = new global::System.Windows.Forms.Label();
			this._propertiesGroupBox.SuspendLayout();
			base.SuspendLayout();
			this._helpLabel.Anchor = global::System.Windows.Forms.AnchorStyles.Top | global::System.Windows.Forms.AnchorStyles.Left | global::System.Windows.Forms.AnchorStyles.Right;
			this._helpLabel.Location = new global::System.Drawing.Point(12, 11);
			this._helpLabel.Name = "_helpLabel";
			this._helpLabel.Size = new global::System.Drawing.Size(524, 42);
			this._helpLabel.TabIndex = 10;
			this._columnLabel.Location = new global::System.Drawing.Point(12, 59);
			this._columnLabel.Name = "_columnLabel";
			this._columnLabel.Size = new global::System.Drawing.Size(172, 15);
			this._columnLabel.TabIndex = 20;
			this._columnsComboBox.DropDownStyle = global::System.Windows.Forms.ComboBoxStyle.DropDownList;
			this._columnsComboBox.Location = new global::System.Drawing.Point(12, 77);
			this._columnsComboBox.Name = "_columnsComboBox";
			this._columnsComboBox.Size = new global::System.Drawing.Size(172, 21);
			this._columnsComboBox.Sorted = true;
			this._columnsComboBox.TabIndex = 30;
			this._columnsComboBox.SelectedIndexChanged += new global::System.EventHandler(this.OnColumnsComboBoxSelectedIndexChanged);
			this._operatorLabel.Location = new global::System.Drawing.Point(12, 104);
			this._operatorLabel.Name = "_operatorLabel";
			this._operatorLabel.Size = new global::System.Drawing.Size(172, 15);
			this._operatorLabel.TabIndex = 40;
			this._operatorsComboBox.DropDownStyle = global::System.Windows.Forms.ComboBoxStyle.DropDownList;
			this._operatorsComboBox.Location = new global::System.Drawing.Point(12, 122);
			this._operatorsComboBox.Name = "_operatorsComboBox";
			this._operatorsComboBox.Size = new global::System.Drawing.Size(172, 21);
			this._operatorsComboBox.TabIndex = 50;
			this._operatorsComboBox.SelectedIndexChanged += new global::System.EventHandler(this.OnOperatorsComboBoxSelectedIndexChanged);
			this._sourceLabel.Location = new global::System.Drawing.Point(12, 148);
			this._sourceLabel.Name = "_sourceLabel";
			this._sourceLabel.Size = new global::System.Drawing.Size(172, 15);
			this._sourceLabel.TabIndex = 60;
			this._sourceComboBox.DropDownStyle = global::System.Windows.Forms.ComboBoxStyle.DropDownList;
			this._sourceComboBox.Location = new global::System.Drawing.Point(12, 166);
			this._sourceComboBox.Name = "_sourceComboBox";
			this._sourceComboBox.Size = new global::System.Drawing.Size(172, 21);
			this._sourceComboBox.TabIndex = 70;
			this._sourceComboBox.SelectedIndexChanged += new global::System.EventHandler(this.OnSourceComboBoxSelectedIndexChanged);
			this._propertiesGroupBox.Anchor = global::System.Windows.Forms.AnchorStyles.Top | global::System.Windows.Forms.AnchorStyles.Left | global::System.Windows.Forms.AnchorStyles.Right;
			this._propertiesGroupBox.Controls.Add(this._propertiesPanel);
			this._propertiesGroupBox.Location = new global::System.Drawing.Point(243, 59);
			this._propertiesGroupBox.Name = "_propertiesGroupBox";
			this._propertiesGroupBox.Size = new global::System.Drawing.Size(194, 127);
			this._propertiesGroupBox.TabIndex = 80;
			this._propertiesGroupBox.TabStop = false;
			this._propertiesPanel.Anchor = global::System.Windows.Forms.AnchorStyles.Top | global::System.Windows.Forms.AnchorStyles.Bottom | global::System.Windows.Forms.AnchorStyles.Left | global::System.Windows.Forms.AnchorStyles.Right;
			this._propertiesPanel.Location = new global::System.Drawing.Point(10, 15);
			this._propertiesPanel.Name = "_propertiesPanel";
			this._propertiesPanel.Size = new global::System.Drawing.Size(164, 100);
			this._propertiesPanel.TabIndex = 10;
			this._expressionLabel.Location = new global::System.Drawing.Point(12, 194);
			this._expressionLabel.Name = "_expressionLabel";
			this._expressionLabel.Size = new global::System.Drawing.Size(225, 15);
			this._expressionLabel.TabIndex = 90;
			this._expressionTextBox.Location = new global::System.Drawing.Point(12, 212);
			this._expressionTextBox.Name = "_expressionTextBox";
			this._expressionTextBox.ReadOnly = true;
			this._expressionTextBox.Size = new global::System.Drawing.Size(224, 20);
			this._expressionTextBox.TabIndex = 100;
			this._valueLabel.Anchor = global::System.Windows.Forms.AnchorStyles.Top | global::System.Windows.Forms.AnchorStyles.Left | global::System.Windows.Forms.AnchorStyles.Right;
			this._valueLabel.Location = new global::System.Drawing.Point(243, 194);
			this._valueLabel.Name = "_valueLabel";
			this._valueLabel.Size = new global::System.Drawing.Size(194, 15);
			this._valueLabel.TabIndex = 110;
			this._valueTextBox.Anchor = global::System.Windows.Forms.AnchorStyles.Top | global::System.Windows.Forms.AnchorStyles.Left | global::System.Windows.Forms.AnchorStyles.Right;
			this._valueTextBox.Location = new global::System.Drawing.Point(243, 212);
			this._valueTextBox.Name = "_valueTextBox";
			this._valueTextBox.ReadOnly = true;
			this._valueTextBox.Size = new global::System.Drawing.Size(194, 20);
			this._valueTextBox.TabIndex = 120;
			this._addButton.Anchor = global::System.Windows.Forms.AnchorStyles.Top | global::System.Windows.Forms.AnchorStyles.Right;
			this._addButton.Location = new global::System.Drawing.Point(443, 212);
			this._addButton.Name = "_addButton";
			this._addButton.Size = new global::System.Drawing.Size(90, 23);
			this._addButton.TabIndex = 125;
			this._addButton.Click += new global::System.EventHandler(this.OnAddButtonClick);
			this._whereClausesLabel.Anchor = global::System.Windows.Forms.AnchorStyles.Top | global::System.Windows.Forms.AnchorStyles.Left | global::System.Windows.Forms.AnchorStyles.Right;
			this._whereClausesLabel.Location = new global::System.Drawing.Point(12, 242);
			this._whereClausesLabel.Name = "_whereClausesLabel";
			this._whereClausesLabel.Size = new global::System.Drawing.Size(425, 15);
			this._whereClausesLabel.TabIndex = 130;
			this._whereClausesListView.Anchor = global::System.Windows.Forms.AnchorStyles.Top | global::System.Windows.Forms.AnchorStyles.Bottom | global::System.Windows.Forms.AnchorStyles.Left | global::System.Windows.Forms.AnchorStyles.Right;
			this._whereClausesListView.Columns.AddRange(new global::System.Windows.Forms.ColumnHeader[] { this._expressionColumnHeader, this._valueColumnHeader });
			this._whereClausesListView.FullRowSelect = true;
			this._whereClausesListView.HeaderStyle = global::System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
			this._whereClausesListView.HideSelection = false;
			this._whereClausesListView.Location = new global::System.Drawing.Point(12, 260);
			this._whereClausesListView.MultiSelect = false;
			this._whereClausesListView.Name = "_whereClausesListView";
			this._whereClausesListView.Size = new global::System.Drawing.Size(425, 78);
			this._whereClausesListView.TabIndex = 135;
			this._whereClausesListView.View = global::System.Windows.Forms.View.Details;
			this._whereClausesListView.SelectedIndexChanged += new global::System.EventHandler(this.OnWhereClausesListViewSelectedIndexChanged);
			this._expressionColumnHeader.Text = "";
			this._expressionColumnHeader.Width = 225;
			this._valueColumnHeader.Text = "";
			this._valueColumnHeader.Width = 160;
			this._removeButton.Anchor = global::System.Windows.Forms.AnchorStyles.Top | global::System.Windows.Forms.AnchorStyles.Right;
			this._removeButton.Location = new global::System.Drawing.Point(442, 260);
			this._removeButton.Name = "_removeButton";
			this._removeButton.Size = new global::System.Drawing.Size(90, 23);
			this._removeButton.TabIndex = 140;
			this._removeButton.Click += new global::System.EventHandler(this.OnRemoveButtonClick);
			this._okButton.Anchor = global::System.Windows.Forms.AnchorStyles.Bottom | global::System.Windows.Forms.AnchorStyles.Right;
			this._okButton.Location = new global::System.Drawing.Point(376, 346);
			this._okButton.Name = "_okButton";
			this._okButton.Size = new global::System.Drawing.Size(75, 23);
			this._okButton.TabIndex = 150;
			this._okButton.Click += new global::System.EventHandler(this.OnOkButtonClick);
			this._cancelButton.Anchor = global::System.Windows.Forms.AnchorStyles.Bottom | global::System.Windows.Forms.AnchorStyles.Right;
			this._cancelButton.DialogResult = global::System.Windows.Forms.DialogResult.Cancel;
			this._cancelButton.Location = new global::System.Drawing.Point(457, 346);
			this._cancelButton.Name = "_cancelButton";
			this._cancelButton.Size = new global::System.Drawing.Size(75, 23);
			this._cancelButton.TabIndex = 160;
			this._cancelButton.Click += new global::System.EventHandler(this.OnCancelButtonClick);
			base.AcceptButton = this._okButton;
			base.CancelButton = this._cancelButton;
			base.ClientSize = new global::System.Drawing.Size(544, 381);
			base.Controls.Add(this._valueTextBox);
			base.Controls.Add(this._valueLabel);
			base.Controls.Add(this._whereClausesListView);
			base.Controls.Add(this._expressionTextBox);
			base.Controls.Add(this._propertiesGroupBox);
			base.Controls.Add(this._expressionLabel);
			base.Controls.Add(this._okButton);
			base.Controls.Add(this._cancelButton);
			base.Controls.Add(this._removeButton);
			base.Controls.Add(this._addButton);
			base.Controls.Add(this._whereClausesLabel);
			base.Controls.Add(this._operatorsComboBox);
			base.Controls.Add(this._operatorLabel);
			base.Controls.Add(this._columnsComboBox);
			base.Controls.Add(this._columnLabel);
			base.Controls.Add(this._helpLabel);
			base.Controls.Add(this._sourceLabel);
			base.Controls.Add(this._sourceComboBox);
			this.MinimumSize = new global::System.Drawing.Size(552, 415);
			base.Name = "SqlDataSourceConfigureFilterForm";
			base.SizeGripStyle = global::System.Windows.Forms.SizeGripStyle.Show;
			this._propertiesGroupBox.ResumeLayout(false);
			base.InitializeForm();
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		// Token: 0x04001D95 RID: 7573
		private global::System.Windows.Forms.Label _helpLabel;

		// Token: 0x04001D96 RID: 7574
		private global::System.Windows.Forms.Label _columnLabel;

		// Token: 0x04001D97 RID: 7575
		private global::System.Web.UI.Design.Util.AutoSizeComboBox _columnsComboBox;

		// Token: 0x04001D98 RID: 7576
		private global::System.Web.UI.Design.Util.AutoSizeComboBox _operatorsComboBox;

		// Token: 0x04001D99 RID: 7577
		private global::System.Windows.Forms.Label _operatorLabel;

		// Token: 0x04001D9A RID: 7578
		private global::System.Windows.Forms.Label _expressionLabel;

		// Token: 0x04001D9B RID: 7579
		private global::System.Windows.Forms.Button _addButton;

		// Token: 0x04001D9C RID: 7580
		private global::System.Windows.Forms.GroupBox _propertiesGroupBox;

		// Token: 0x04001D9D RID: 7581
		private global::System.Windows.Forms.TextBox _expressionTextBox;

		// Token: 0x04001D9E RID: 7582
		private global::System.Windows.Forms.Label _whereClausesLabel;

		// Token: 0x04001D9F RID: 7583
		private global::System.Windows.Forms.Button _removeButton;

		// Token: 0x04001DA0 RID: 7584
		private global::System.Windows.Forms.Button _cancelButton;

		// Token: 0x04001DA1 RID: 7585
		private global::System.Windows.Forms.Button _okButton;

		// Token: 0x04001DA2 RID: 7586
		private global::System.Windows.Forms.Panel _propertiesPanel;

		// Token: 0x04001DA3 RID: 7587
		private global::System.Web.UI.Design.Util.AutoSizeComboBox _sourceComboBox;

		// Token: 0x04001DA4 RID: 7588
		private global::System.Windows.Forms.ListView _whereClausesListView;

		// Token: 0x04001DA5 RID: 7589
		private global::System.Windows.Forms.ColumnHeader _expressionColumnHeader;

		// Token: 0x04001DA6 RID: 7590
		private global::System.Windows.Forms.ColumnHeader _valueColumnHeader;

		// Token: 0x04001DA7 RID: 7591
		private global::System.Windows.Forms.TextBox _valueTextBox;

		// Token: 0x04001DA8 RID: 7592
		private global::System.Windows.Forms.Label _valueLabel;

		// Token: 0x04001DA9 RID: 7593
		private global::System.Windows.Forms.Label _sourceLabel;
	}
}
