namespace System.Web.UI.Design.Util
{
	// Token: 0x020003CD RID: 973
	internal abstract partial class TaskForm : global::System.Web.UI.Design.Util.TaskFormBase
	{
		// Token: 0x060023B9 RID: 9145 RVA: 0x000BF8D4 File Offset: 0x000BE8D4
		private void InitializeComponent()
		{
			this._dialogButtonsTableLayoutPanel = new global::System.Windows.Forms.TableLayoutPanel();
			this._okButton = new global::System.Windows.Forms.Button();
			this._cancelButton = new global::System.Windows.Forms.Button();
			this._dummyLabel1 = new global::System.Windows.Forms.Label();
			this._dialogButtonsTableLayoutPanel.SuspendLayout();
			base.SuspendLayout();
			this._dialogButtonsTableLayoutPanel.Anchor = global::System.Windows.Forms.AnchorStyles.Bottom | global::System.Windows.Forms.AnchorStyles.Right;
			this._dialogButtonsTableLayoutPanel.AutoSize = true;
			this._dialogButtonsTableLayoutPanel.ColumnCount = 3;
			this._dialogButtonsTableLayoutPanel.ColumnStyles.Add(new global::System.Windows.Forms.ColumnStyle(global::System.Windows.Forms.SizeType.Percent, 50f));
			this._dialogButtonsTableLayoutPanel.ColumnStyles.Add(new global::System.Windows.Forms.ColumnStyle(global::System.Windows.Forms.SizeType.Absolute, 6f));
			this._dialogButtonsTableLayoutPanel.ColumnStyles.Add(new global::System.Windows.Forms.ColumnStyle(global::System.Windows.Forms.SizeType.Percent, 50f));
			this._dialogButtonsTableLayoutPanel.Controls.Add(this._okButton);
			this._dialogButtonsTableLayoutPanel.Controls.Add(this._dummyLabel1);
			this._dialogButtonsTableLayoutPanel.Controls.Add(this._cancelButton);
			this._dialogButtonsTableLayoutPanel.Location = new global::System.Drawing.Point(404, 381);
			this._dialogButtonsTableLayoutPanel.Name = "_dialogButtonsTableLayoutPanel";
			this._dialogButtonsTableLayoutPanel.RowStyles.Add(new global::System.Windows.Forms.RowStyle());
			this._dialogButtonsTableLayoutPanel.Size = new global::System.Drawing.Size(156, 23);
			this._dialogButtonsTableLayoutPanel.TabIndex = 100;
			this._okButton.Anchor = global::System.Windows.Forms.AnchorStyles.Left | global::System.Windows.Forms.AnchorStyles.Right;
			this._okButton.AutoSize = true;
			this._okButton.DialogResult = global::System.Windows.Forms.DialogResult.OK;
			this._okButton.Enabled = false;
			this._okButton.Location = new global::System.Drawing.Point(0, 0);
			this._okButton.Margin = new global::System.Windows.Forms.Padding(0);
			this._okButton.MinimumSize = new global::System.Drawing.Size(75, 23);
			this._okButton.Name = "_okButton";
			this._okButton.TabIndex = 50;
			this._okButton.Click += new global::System.EventHandler(this.OnOKButtonClick);
			this._dummyLabel1.Location = new global::System.Drawing.Point(75, 0);
			this._dummyLabel1.Margin = new global::System.Windows.Forms.Padding(0);
			this._dummyLabel1.Name = "_dummyLabel1";
			this._dummyLabel1.Size = new global::System.Drawing.Size(6, 0);
			this._dummyLabel1.TabIndex = 20;
			this._cancelButton.Anchor = global::System.Windows.Forms.AnchorStyles.Left | global::System.Windows.Forms.AnchorStyles.Right;
			this._cancelButton.AutoSize = true;
			this._cancelButton.DialogResult = global::System.Windows.Forms.DialogResult.Cancel;
			this._cancelButton.Location = new global::System.Drawing.Point(81, 0);
			this._cancelButton.Margin = new global::System.Windows.Forms.Padding(0);
			this._cancelButton.MinimumSize = new global::System.Drawing.Size(75, 23);
			this._cancelButton.Name = "_cancelButton";
			this._cancelButton.TabIndex = 70;
			this._cancelButton.Click += new global::System.EventHandler(this.OnCancelButtonClick);
			base.AcceptButton = this._okButton;
			base.FormBorderStyle = global::System.Windows.Forms.FormBorderStyle.FixedDialog;
			base.CancelButton = this._cancelButton;
			base.Controls.Add(this._dialogButtonsTableLayoutPanel);
			this._dialogButtonsTableLayoutPanel.ResumeLayout(false);
			this._dialogButtonsTableLayoutPanel.PerformLayout();
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		// Token: 0x040018AE RID: 6318
		private global::System.Windows.Forms.Button _okButton;

		// Token: 0x040018AF RID: 6319
		private global::System.Windows.Forms.Label _dummyLabel1;

		// Token: 0x040018B0 RID: 6320
		private global::System.Windows.Forms.Button _cancelButton;

		// Token: 0x040018B1 RID: 6321
		private global::System.Windows.Forms.TableLayoutPanel _dialogButtonsTableLayoutPanel;
	}
}
