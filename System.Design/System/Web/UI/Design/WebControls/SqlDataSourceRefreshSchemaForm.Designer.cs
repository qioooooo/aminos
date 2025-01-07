namespace System.Web.UI.Design.WebControls
{
	internal partial class SqlDataSourceRefreshSchemaForm : global::System.Web.UI.Design.Util.DesignerForm
	{
		private void InitializeComponent()
		{
			global::System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle = new global::System.Windows.Forms.DataGridViewCellStyle();
			global::System.Windows.Forms.DataGridViewComboBoxColumn dataGridViewComboBoxColumn = new global::System.Windows.Forms.DataGridViewComboBoxColumn();
			global::System.Windows.Forms.DataGridViewComboBoxColumn dataGridViewComboBoxColumn2 = new global::System.Windows.Forms.DataGridViewComboBoxColumn();
			global::System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this._parametersDataGridView = new global::System.Windows.Forms.DataGridView();
			this._okButton = new global::System.Windows.Forms.Button();
			this._commandTextBox = new global::System.Windows.Forms.TextBox();
			this._commandLabel = new global::System.Windows.Forms.Label();
			this._helpLabel = new global::System.Windows.Forms.Label();
			this._cancelButton = new global::System.Windows.Forms.Button();
			this._parametersLabel = new global::System.Windows.Forms.Label();
			base.SuspendLayout();
			this._helpLabel.Anchor = global::System.Windows.Forms.AnchorStyles.Top | global::System.Windows.Forms.AnchorStyles.Left | global::System.Windows.Forms.AnchorStyles.Right;
			this._helpLabel.Location = new global::System.Drawing.Point(12, 12);
			this._helpLabel.Name = "_helpLabel";
			this._helpLabel.Size = new global::System.Drawing.Size(576, 47);
			this._helpLabel.TabIndex = 10;
			this._commandLabel.Anchor = global::System.Windows.Forms.AnchorStyles.Top | global::System.Windows.Forms.AnchorStyles.Left | global::System.Windows.Forms.AnchorStyles.Right;
			this._commandLabel.Location = new global::System.Drawing.Point(12, 64);
			this._commandLabel.Name = "_commandLabel";
			this._commandLabel.Size = new global::System.Drawing.Size(576, 16);
			this._commandLabel.TabIndex = 20;
			this._commandTextBox.Anchor = global::System.Windows.Forms.AnchorStyles.Top | global::System.Windows.Forms.AnchorStyles.Left | global::System.Windows.Forms.AnchorStyles.Right;
			this._commandTextBox.BackColor = global::System.Drawing.SystemColors.Control;
			this._commandTextBox.Location = new global::System.Drawing.Point(12, 82);
			this._commandTextBox.Multiline = true;
			this._commandTextBox.Name = "_commandTextBox";
			this._commandTextBox.ReadOnly = true;
			this._commandTextBox.ScrollBars = global::System.Windows.Forms.ScrollBars.Vertical;
			this._commandTextBox.Size = new global::System.Drawing.Size(576, 50);
			this._commandTextBox.TabIndex = 30;
			this._parametersLabel.Anchor = global::System.Windows.Forms.AnchorStyles.Top | global::System.Windows.Forms.AnchorStyles.Left | global::System.Windows.Forms.AnchorStyles.Right;
			this._parametersLabel.Location = new global::System.Drawing.Point(13, 142);
			this._parametersLabel.Name = "_parametersLabel";
			this._parametersLabel.Size = new global::System.Drawing.Size(576, 16);
			this._parametersLabel.TabIndex = 40;
			this._parametersDataGridView.AllowUserToAddRows = false;
			this._parametersDataGridView.AllowUserToDeleteRows = false;
			this._parametersDataGridView.Anchor = global::System.Windows.Forms.AnchorStyles.Top | global::System.Windows.Forms.AnchorStyles.Bottom | global::System.Windows.Forms.AnchorStyles.Left | global::System.Windows.Forms.AnchorStyles.Right;
			this._parametersDataGridView.AutoGenerateColumns = false;
			this._parametersDataGridView.ColumnHeadersHeightSizeMode = global::System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
			dataGridViewTextBoxColumn.DataPropertyName = "Name";
			dataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle;
			dataGridViewTextBoxColumn.Name = "_parameterNameColumn";
			dataGridViewTextBoxColumn.ReadOnly = true;
			dataGridViewTextBoxColumn.ValueType = typeof(string);
			dataGridViewComboBoxColumn.DataPropertyName = "Type";
			dataGridViewComboBoxColumn.DefaultCellStyle = dataGridViewCellStyle;
			dataGridViewComboBoxColumn.Name = "_parameterTypeColumn";
			dataGridViewComboBoxColumn.ValueType = typeof(string);
			dataGridViewComboBoxColumn2.DataPropertyName = "DbType";
			dataGridViewComboBoxColumn2.DefaultCellStyle = dataGridViewCellStyle;
			dataGridViewComboBoxColumn2.Name = "_parameterDbTypeColumn";
			dataGridViewComboBoxColumn2.ValueType = typeof(string);
			dataGridViewTextBoxColumn2.DataPropertyName = "DefaultValue";
			dataGridViewTextBoxColumn2.DefaultCellStyle = dataGridViewCellStyle;
			dataGridViewTextBoxColumn2.Name = "_parameterValueColumn";
			dataGridViewTextBoxColumn2.ValueType = typeof(string);
			this._parametersDataGridView.EditMode = global::System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
			this._parametersDataGridView.Columns.Add(dataGridViewTextBoxColumn);
			this._parametersDataGridView.Columns.Add(dataGridViewComboBoxColumn);
			this._parametersDataGridView.Columns.Add(dataGridViewComboBoxColumn2);
			this._parametersDataGridView.Columns.Add(dataGridViewTextBoxColumn2);
			this._parametersDataGridView.Location = new global::System.Drawing.Point(12, 160);
			this._parametersDataGridView.MultiSelect = false;
			this._parametersDataGridView.Name = "_parametersDataGridView";
			this._parametersDataGridView.RowHeadersVisible = false;
			this._parametersDataGridView.Size = new global::System.Drawing.Size(576, 156);
			this._parametersDataGridView.TabIndex = 50;
			this._okButton.Anchor = global::System.Windows.Forms.AnchorStyles.Bottom | global::System.Windows.Forms.AnchorStyles.Right;
			this._okButton.Location = new global::System.Drawing.Point(432, 331);
			this._okButton.Name = "_okButton";
			this._okButton.TabIndex = 60;
			this._okButton.Click += new global::System.EventHandler(this.OnOkButtonClick);
			this._cancelButton.Anchor = global::System.Windows.Forms.AnchorStyles.Bottom | global::System.Windows.Forms.AnchorStyles.Right;
			this._cancelButton.DialogResult = global::System.Windows.Forms.DialogResult.Cancel;
			this._cancelButton.Location = new global::System.Drawing.Point(513, 331);
			this._cancelButton.Name = "_cancelButton";
			this._cancelButton.TabIndex = 70;
			base.AcceptButton = this._okButton;
			base.CancelButton = this._cancelButton;
			base.ClientSize = new global::System.Drawing.Size(600, 366);
			base.Controls.Add(this._parametersLabel);
			base.Controls.Add(this._parametersDataGridView);
			base.Controls.Add(this._cancelButton);
			base.Controls.Add(this._commandLabel);
			base.Controls.Add(this._helpLabel);
			base.Controls.Add(this._okButton);
			base.Controls.Add(this._commandTextBox);
			this.MinimumSize = new global::System.Drawing.Size(600, 366);
			base.Name = "SqlDataSourceRefreshSchemaForm";
			base.SizeGripStyle = global::System.Windows.Forms.SizeGripStyle.Show;
			base.InitializeForm();
			base.ResumeLayout(false);
		}

		private global::System.Windows.Forms.TextBox _commandTextBox;

		private global::System.Windows.Forms.Label _helpLabel;

		private global::System.Windows.Forms.Button _cancelButton;

		private global::System.Windows.Forms.Label _commandLabel;

		private global::System.Windows.Forms.Button _okButton;

		private global::System.Windows.Forms.DataGridView _parametersDataGridView;

		private global::System.Windows.Forms.Label _parametersLabel;
	}
}
