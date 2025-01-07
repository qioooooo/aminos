namespace System.Web.UI.Design.WebControls
{
	internal partial class SqlDataSourceConfigureSortForm : global::System.Web.UI.Design.Util.DesignerForm
	{
		private void InitializeComponent()
		{
			this._helpLabel = new global::System.Windows.Forms.Label();
			this._previewLabel = new global::System.Windows.Forms.Label();
			this._previewTextBox = new global::System.Windows.Forms.TextBox();
			this._sortAscendingRadioButton1 = new global::System.Windows.Forms.RadioButton();
			this._sortDescendingRadioButton1 = new global::System.Windows.Forms.RadioButton();
			this._sortDirectionPanel1 = new global::System.Windows.Forms.Panel();
			this._fieldComboBox1 = new global::System.Web.UI.Design.Util.AutoSizeComboBox();
			this._okButton = new global::System.Windows.Forms.Button();
			this._cancelButton = new global::System.Windows.Forms.Button();
			this._sortDescendingRadioButton2 = new global::System.Windows.Forms.RadioButton();
			this._sortAscendingRadioButton2 = new global::System.Windows.Forms.RadioButton();
			this._fieldComboBox2 = new global::System.Web.UI.Design.Util.AutoSizeComboBox();
			this._sortDirectionPanel2 = new global::System.Windows.Forms.Panel();
			this._sortDescendingRadioButton3 = new global::System.Windows.Forms.RadioButton();
			this._sortAscendingRadioButton3 = new global::System.Windows.Forms.RadioButton();
			this._fieldComboBox3 = new global::System.Web.UI.Design.Util.AutoSizeComboBox();
			this._sortDirectionPanel3 = new global::System.Windows.Forms.Panel();
			this._sortByGroupBox1 = new global::System.Windows.Forms.GroupBox();
			this._sortByGroupBox2 = new global::System.Windows.Forms.GroupBox();
			this._sortByGroupBox3 = new global::System.Windows.Forms.GroupBox();
			this._sortDirectionPanel1.SuspendLayout();
			this._sortDirectionPanel2.SuspendLayout();
			this._sortDirectionPanel3.SuspendLayout();
			this._sortByGroupBox1.SuspendLayout();
			this._sortByGroupBox2.SuspendLayout();
			this._sortByGroupBox3.SuspendLayout();
			base.SuspendLayout();
			this._helpLabel.Anchor = global::System.Windows.Forms.AnchorStyles.Top | global::System.Windows.Forms.AnchorStyles.Left | global::System.Windows.Forms.AnchorStyles.Right;
			this._helpLabel.Location = new global::System.Drawing.Point(12, 12);
			this._helpLabel.Name = "_helpLabel";
			this._helpLabel.Size = new global::System.Drawing.Size(382, 16);
			this._helpLabel.TabIndex = 10;
			this._sortAscendingRadioButton1.Anchor = global::System.Windows.Forms.AnchorStyles.Top | global::System.Windows.Forms.AnchorStyles.Left | global::System.Windows.Forms.AnchorStyles.Right;
			this._sortAscendingRadioButton1.Location = new global::System.Drawing.Point(0, 0);
			this._sortAscendingRadioButton1.Name = "_sortAscendingRadioButton1";
			this._sortAscendingRadioButton1.Size = new global::System.Drawing.Size(200, 18);
			this._sortAscendingRadioButton1.TabIndex = 10;
			this._sortAscendingRadioButton1.CheckedChanged += new global::System.EventHandler(this.OnSortAscendingRadioButton1CheckedChanged);
			this._sortDescendingRadioButton1.Anchor = global::System.Windows.Forms.AnchorStyles.Top | global::System.Windows.Forms.AnchorStyles.Left | global::System.Windows.Forms.AnchorStyles.Right;
			this._sortDescendingRadioButton1.Location = new global::System.Drawing.Point(0, 18);
			this._sortDescendingRadioButton1.Name = "_sortDescendingRadioButton1";
			this._sortDescendingRadioButton1.Size = new global::System.Drawing.Size(200, 18);
			this._sortDescendingRadioButton1.TabIndex = 20;
			this._sortDirectionPanel1.Controls.Add(this._sortDescendingRadioButton1);
			this._sortDirectionPanel1.Controls.Add(this._sortAscendingRadioButton1);
			this._sortDirectionPanel1.Location = new global::System.Drawing.Point(169, 12);
			this._sortDirectionPanel1.Name = "_sortDirectionPanel1";
			this._sortDirectionPanel1.Size = new global::System.Drawing.Size(200, 38);
			this._sortDirectionPanel1.TabIndex = 20;
			this._fieldComboBox1.DropDownStyle = global::System.Windows.Forms.ComboBoxStyle.DropDownList;
			this._fieldComboBox1.Location = new global::System.Drawing.Point(9, 20);
			this._fieldComboBox1.Name = "_fieldComboBox1";
			this._fieldComboBox1.Size = new global::System.Drawing.Size(153, 21);
			this._fieldComboBox1.Sorted = true;
			this._fieldComboBox1.TabIndex = 10;
			this._fieldComboBox1.SelectedIndexChanged += new global::System.EventHandler(this.OnFieldComboBox1SelectedIndexChanged);
			this._sortDescendingRadioButton2.Anchor = global::System.Windows.Forms.AnchorStyles.Top | global::System.Windows.Forms.AnchorStyles.Left | global::System.Windows.Forms.AnchorStyles.Right;
			this._sortDescendingRadioButton2.Location = new global::System.Drawing.Point(0, 18);
			this._sortDescendingRadioButton2.Name = "_sortDescendingRadioButton2";
			this._sortDescendingRadioButton2.Size = new global::System.Drawing.Size(200, 18);
			this._sortDescendingRadioButton2.TabIndex = 20;
			this._sortAscendingRadioButton2.Anchor = global::System.Windows.Forms.AnchorStyles.Top | global::System.Windows.Forms.AnchorStyles.Left | global::System.Windows.Forms.AnchorStyles.Right;
			this._sortAscendingRadioButton2.Location = new global::System.Drawing.Point(0, 0);
			this._sortAscendingRadioButton2.Name = "_sortAscendingRadioButton2";
			this._sortAscendingRadioButton2.Size = new global::System.Drawing.Size(200, 18);
			this._sortAscendingRadioButton2.TabIndex = 10;
			this._sortAscendingRadioButton2.CheckedChanged += new global::System.EventHandler(this.OnSortAscendingRadioButton2CheckedChanged);
			this._fieldComboBox2.DropDownStyle = global::System.Windows.Forms.ComboBoxStyle.DropDownList;
			this._fieldComboBox2.Location = new global::System.Drawing.Point(9, 20);
			this._fieldComboBox2.Name = "_fieldComboBox2";
			this._fieldComboBox2.Size = new global::System.Drawing.Size(153, 21);
			this._fieldComboBox2.Sorted = true;
			this._fieldComboBox2.TabIndex = 10;
			this._fieldComboBox2.SelectedIndexChanged += new global::System.EventHandler(this.OnFieldComboBox2SelectedIndexChanged);
			this._sortDirectionPanel2.Controls.Add(this._sortDescendingRadioButton2);
			this._sortDirectionPanel2.Controls.Add(this._sortAscendingRadioButton2);
			this._sortDirectionPanel2.Location = new global::System.Drawing.Point(169, 12);
			this._sortDirectionPanel2.Name = "_sortDirectionPanel2";
			this._sortDirectionPanel2.Size = new global::System.Drawing.Size(200, 38);
			this._sortDirectionPanel2.TabIndex = 20;
			this._sortDescendingRadioButton3.Anchor = global::System.Windows.Forms.AnchorStyles.Top | global::System.Windows.Forms.AnchorStyles.Left | global::System.Windows.Forms.AnchorStyles.Right;
			this._sortDescendingRadioButton3.Location = new global::System.Drawing.Point(0, 18);
			this._sortDescendingRadioButton3.Name = "_sortDescendingRadioButton3";
			this._sortDescendingRadioButton3.Size = new global::System.Drawing.Size(200, 18);
			this._sortDescendingRadioButton3.TabIndex = 20;
			this._sortAscendingRadioButton3.Anchor = global::System.Windows.Forms.AnchorStyles.Top | global::System.Windows.Forms.AnchorStyles.Left | global::System.Windows.Forms.AnchorStyles.Right;
			this._sortAscendingRadioButton3.Location = new global::System.Drawing.Point(0, 0);
			this._sortAscendingRadioButton3.Name = "_sortAscendingRadioButton3";
			this._sortAscendingRadioButton3.Size = new global::System.Drawing.Size(200, 18);
			this._sortAscendingRadioButton3.TabIndex = 10;
			this._sortAscendingRadioButton3.CheckedChanged += new global::System.EventHandler(this.OnSortAscendingRadioButton3CheckedChanged);
			this._fieldComboBox3.DropDownStyle = global::System.Windows.Forms.ComboBoxStyle.DropDownList;
			this._fieldComboBox3.Location = new global::System.Drawing.Point(9, 20);
			this._fieldComboBox3.Name = "_fieldComboBox3";
			this._fieldComboBox3.Size = new global::System.Drawing.Size(153, 21);
			this._fieldComboBox3.Sorted = true;
			this._fieldComboBox3.TabIndex = 10;
			this._fieldComboBox3.SelectedIndexChanged += new global::System.EventHandler(this.OnFieldComboBox3SelectedIndexChanged);
			this._sortDirectionPanel3.Controls.Add(this._sortDescendingRadioButton3);
			this._sortDirectionPanel3.Controls.Add(this._sortAscendingRadioButton3);
			this._sortDirectionPanel3.Location = new global::System.Drawing.Point(169, 12);
			this._sortDirectionPanel3.Name = "_sortDirectionPanel3";
			this._sortDirectionPanel3.Size = new global::System.Drawing.Size(200, 38);
			this._sortDirectionPanel3.TabIndex = 20;
			this._sortByGroupBox1.Anchor = global::System.Windows.Forms.AnchorStyles.Top | global::System.Windows.Forms.AnchorStyles.Left | global::System.Windows.Forms.AnchorStyles.Right;
			this._sortByGroupBox1.Controls.Add(this._fieldComboBox1);
			this._sortByGroupBox1.Controls.Add(this._sortDirectionPanel1);
			this._sortByGroupBox1.Location = new global::System.Drawing.Point(12, 33);
			this._sortByGroupBox1.Name = "_sortByGroupBox1";
			this._sortByGroupBox1.Size = new global::System.Drawing.Size(384, 56);
			this._sortByGroupBox1.TabIndex = 20;
			this._sortByGroupBox1.TabStop = false;
			this._sortByGroupBox2.Anchor = global::System.Windows.Forms.AnchorStyles.Top | global::System.Windows.Forms.AnchorStyles.Left | global::System.Windows.Forms.AnchorStyles.Right;
			this._sortByGroupBox2.Controls.Add(this._fieldComboBox2);
			this._sortByGroupBox2.Controls.Add(this._sortDirectionPanel2);
			this._sortByGroupBox2.Location = new global::System.Drawing.Point(12, 95);
			this._sortByGroupBox2.Name = "_sortByGroupBox2";
			this._sortByGroupBox2.Size = new global::System.Drawing.Size(384, 56);
			this._sortByGroupBox2.TabIndex = 30;
			this._sortByGroupBox2.TabStop = false;
			this._sortByGroupBox3.Anchor = global::System.Windows.Forms.AnchorStyles.Top | global::System.Windows.Forms.AnchorStyles.Left | global::System.Windows.Forms.AnchorStyles.Right;
			this._sortByGroupBox3.Controls.Add(this._fieldComboBox3);
			this._sortByGroupBox3.Controls.Add(this._sortDirectionPanel3);
			this._sortByGroupBox3.Location = new global::System.Drawing.Point(12, 157);
			this._sortByGroupBox3.Name = "_sortByGroupBox3";
			this._sortByGroupBox3.Size = new global::System.Drawing.Size(384, 56);
			this._sortByGroupBox3.TabIndex = 40;
			this._sortByGroupBox3.TabStop = false;
			this._previewLabel.Anchor = global::System.Windows.Forms.AnchorStyles.Top | global::System.Windows.Forms.AnchorStyles.Left | global::System.Windows.Forms.AnchorStyles.Right;
			this._previewLabel.Location = new global::System.Drawing.Point(12, 219);
			this._previewLabel.Name = "_previewLabel";
			this._previewLabel.Size = new global::System.Drawing.Size(384, 13);
			this._previewLabel.TabIndex = 50;
			this._previewTextBox.Anchor = global::System.Windows.Forms.AnchorStyles.Top | global::System.Windows.Forms.AnchorStyles.Bottom | global::System.Windows.Forms.AnchorStyles.Left | global::System.Windows.Forms.AnchorStyles.Right;
			this._previewTextBox.BackColor = global::System.Drawing.SystemColors.Control;
			this._previewTextBox.Location = new global::System.Drawing.Point(12, 237);
			this._previewTextBox.Multiline = true;
			this._previewTextBox.Name = "_previewTextBox";
			this._previewTextBox.ReadOnly = true;
			this._previewTextBox.ScrollBars = global::System.Windows.Forms.ScrollBars.Vertical;
			this._previewTextBox.Size = new global::System.Drawing.Size(384, 72);
			this._previewTextBox.TabIndex = 60;
			this._previewTextBox.Text = "";
			this._okButton.Anchor = global::System.Windows.Forms.AnchorStyles.Bottom | global::System.Windows.Forms.AnchorStyles.Right;
			this._okButton.Location = new global::System.Drawing.Point(240, 321);
			this._okButton.Name = "_okButton";
			this._okButton.TabIndex = 70;
			this._okButton.Click += new global::System.EventHandler(this.OnOkButtonClick);
			this._cancelButton.Anchor = global::System.Windows.Forms.AnchorStyles.Bottom | global::System.Windows.Forms.AnchorStyles.Right;
			this._cancelButton.DialogResult = global::System.Windows.Forms.DialogResult.Cancel;
			this._cancelButton.Location = new global::System.Drawing.Point(321, 321);
			this._cancelButton.Name = "_cancelButton";
			this._cancelButton.TabIndex = 80;
			this._cancelButton.Click += new global::System.EventHandler(this.OnCancelButtonClick);
			base.AcceptButton = this._okButton;
			base.CancelButton = this._cancelButton;
			base.ClientSize = new global::System.Drawing.Size(408, 356);
			base.Controls.Add(this._sortByGroupBox2);
			base.Controls.Add(this._sortByGroupBox3);
			base.Controls.Add(this._sortByGroupBox1);
			base.Controls.Add(this._cancelButton);
			base.Controls.Add(this._okButton);
			base.Controls.Add(this._previewTextBox);
			base.Controls.Add(this._previewLabel);
			base.Controls.Add(this._helpLabel);
			this.MinimumSize = new global::System.Drawing.Size(416, 390);
			base.Name = "SqlDataSourceConfigureSortForm";
			base.SizeGripStyle = global::System.Windows.Forms.SizeGripStyle.Show;
			this._sortDirectionPanel1.ResumeLayout(false);
			this._sortDirectionPanel2.ResumeLayout(false);
			this._sortDirectionPanel3.ResumeLayout(false);
			this._sortByGroupBox1.ResumeLayout(false);
			this._sortByGroupBox2.ResumeLayout(false);
			this._sortByGroupBox3.ResumeLayout(false);
			base.InitializeForm();
			base.ResumeLayout(false);
		}

		private global::System.Windows.Forms.Label _helpLabel;

		private global::System.Windows.Forms.TextBox _previewTextBox;

		private global::System.Windows.Forms.Label _previewLabel;

		private global::System.Windows.Forms.Button _okButton;

		private global::System.Windows.Forms.Button _cancelButton;

		private global::System.Web.UI.Design.Util.AutoSizeComboBox _fieldComboBox1;

		private global::System.Web.UI.Design.Util.AutoSizeComboBox _fieldComboBox2;

		private global::System.Web.UI.Design.Util.AutoSizeComboBox _fieldComboBox3;

		private global::System.Windows.Forms.RadioButton _sortAscendingRadioButton1;

		private global::System.Windows.Forms.RadioButton _sortDescendingRadioButton1;

		private global::System.Windows.Forms.Panel _sortDirectionPanel1;

		private global::System.Windows.Forms.RadioButton _sortDescendingRadioButton2;

		private global::System.Windows.Forms.RadioButton _sortAscendingRadioButton2;

		private global::System.Windows.Forms.Panel _sortDirectionPanel2;

		private global::System.Windows.Forms.RadioButton _sortDescendingRadioButton3;

		private global::System.Windows.Forms.RadioButton _sortAscendingRadioButton3;

		private global::System.Windows.Forms.Panel _sortDirectionPanel3;

		private global::System.Windows.Forms.GroupBox _sortByGroupBox1;

		private global::System.Windows.Forms.GroupBox _sortByGroupBox2;

		private global::System.Windows.Forms.GroupBox _sortByGroupBox3;
	}
}
