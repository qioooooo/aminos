namespace System.Web.UI.Design.WebControls
{
	// Token: 0x020004B1 RID: 1201
	internal sealed partial class SqlDataSourceAdvancedOptionsForm : global::System.Web.UI.Design.Util.DesignerForm
	{
		// Token: 0x06002B7F RID: 11135 RVA: 0x000EFE04 File Offset: 0x000EEE04
		private void InitializeComponent()
		{
			this._helpLabel = new global::System.Windows.Forms.Label();
			this._generateCheckBox = new global::System.Windows.Forms.CheckBox();
			this._generateHelpLabel = new global::System.Windows.Forms.Label();
			this._optimisticCheckBox = new global::System.Windows.Forms.CheckBox();
			this._optimisticHelpLabel = new global::System.Windows.Forms.Label();
			this._okButton = new global::System.Windows.Forms.Button();
			this._cancelButton = new global::System.Windows.Forms.Button();
			base.SuspendLayout();
			this._helpLabel.Anchor = global::System.Windows.Forms.AnchorStyles.Top | global::System.Windows.Forms.AnchorStyles.Left | global::System.Windows.Forms.AnchorStyles.Right;
			this._helpLabel.Location = new global::System.Drawing.Point(12, 12);
			this._helpLabel.Name = "_helpLabel";
			this._helpLabel.Size = new global::System.Drawing.Size(374, 32);
			this._helpLabel.TabIndex = 10;
			this._generateCheckBox.Anchor = global::System.Windows.Forms.AnchorStyles.Top | global::System.Windows.Forms.AnchorStyles.Left | global::System.Windows.Forms.AnchorStyles.Right;
			this._generateCheckBox.Location = new global::System.Drawing.Point(12, 52);
			this._generateCheckBox.Name = "_generateCheckBox";
			this._generateCheckBox.Size = new global::System.Drawing.Size(374, 18);
			this._generateCheckBox.TabIndex = 20;
			this._generateCheckBox.CheckedChanged += new global::System.EventHandler(this.OnGenerateCheckBoxCheckedChanged);
			this._generateHelpLabel.Anchor = global::System.Windows.Forms.AnchorStyles.Top | global::System.Windows.Forms.AnchorStyles.Left | global::System.Windows.Forms.AnchorStyles.Right;
			this._generateHelpLabel.Location = new global::System.Drawing.Point(29, 73);
			this._generateHelpLabel.Name = "_generateHelpLabel";
			this._generateHelpLabel.Size = new global::System.Drawing.Size(357, 48);
			this._generateHelpLabel.TabIndex = 30;
			this._optimisticCheckBox.Anchor = global::System.Windows.Forms.AnchorStyles.Top | global::System.Windows.Forms.AnchorStyles.Left | global::System.Windows.Forms.AnchorStyles.Right;
			this._optimisticCheckBox.Location = new global::System.Drawing.Point(12, 132);
			this._optimisticCheckBox.Name = "_optimisticCheckBox";
			this._optimisticCheckBox.Size = new global::System.Drawing.Size(374, 18);
			this._optimisticCheckBox.TabIndex = 40;
			this._optimisticHelpLabel.Anchor = global::System.Windows.Forms.AnchorStyles.Top | global::System.Windows.Forms.AnchorStyles.Left | global::System.Windows.Forms.AnchorStyles.Right;
			this._optimisticHelpLabel.Location = new global::System.Drawing.Point(29, 153);
			this._optimisticHelpLabel.Name = "_optimisticHelpLabel";
			this._optimisticHelpLabel.Size = new global::System.Drawing.Size(357, 52);
			this._optimisticHelpLabel.TabIndex = 50;
			this._okButton.Anchor = global::System.Windows.Forms.AnchorStyles.Bottom | global::System.Windows.Forms.AnchorStyles.Right;
			this._okButton.Location = new global::System.Drawing.Point(230, 209);
			this._okButton.Name = "_okButton";
			this._okButton.TabIndex = 60;
			this._okButton.Click += new global::System.EventHandler(this.OnOkButtonClick);
			this._cancelButton.Anchor = global::System.Windows.Forms.AnchorStyles.Bottom | global::System.Windows.Forms.AnchorStyles.Right;
			this._cancelButton.DialogResult = global::System.Windows.Forms.DialogResult.Cancel;
			this._cancelButton.Location = new global::System.Drawing.Point(311, 209);
			this._cancelButton.Name = "_cancelButton";
			this._cancelButton.TabIndex = 70;
			this._cancelButton.Click += new global::System.EventHandler(this.OnCancelButtonClick);
			base.AcceptButton = this._okButton;
			base.CancelButton = this._cancelButton;
			base.ClientSize = new global::System.Drawing.Size(398, 244);
			base.Controls.Add(this._cancelButton);
			base.Controls.Add(this._okButton);
			base.Controls.Add(this._optimisticHelpLabel);
			base.Controls.Add(this._optimisticCheckBox);
			base.Controls.Add(this._generateHelpLabel);
			base.Controls.Add(this._generateCheckBox);
			base.Controls.Add(this._helpLabel);
			base.FormBorderStyle = global::System.Windows.Forms.FormBorderStyle.FixedDialog;
			base.Name = "SqlDataSourceAdvancedOptionsForm";
			base.SizeGripStyle = global::System.Windows.Forms.SizeGripStyle.Hide;
			base.InitializeForm();
			base.ResumeLayout(false);
		}

		// Token: 0x04001D8E RID: 7566
		private global::System.Windows.Forms.Button _okButton;

		// Token: 0x04001D8F RID: 7567
		private global::System.Windows.Forms.Label _helpLabel;

		// Token: 0x04001D90 RID: 7568
		private global::System.Windows.Forms.CheckBox _generateCheckBox;

		// Token: 0x04001D91 RID: 7569
		private global::System.Windows.Forms.Label _generateHelpLabel;

		// Token: 0x04001D92 RID: 7570
		private global::System.Windows.Forms.CheckBox _optimisticCheckBox;

		// Token: 0x04001D93 RID: 7571
		private global::System.Windows.Forms.Label _optimisticHelpLabel;

		// Token: 0x04001D94 RID: 7572
		private global::System.Windows.Forms.Button _cancelButton;
	}
}
