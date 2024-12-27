namespace System.ServiceProcess.Design
{
	// Token: 0x0200003A RID: 58
	public partial class ServiceInstallerDialog : global::System.Windows.Forms.Form
	{
		// Token: 0x06000120 RID: 288 RVA: 0x000063C4 File Offset: 0x000053C4
		private void InitializeComponent()
		{
			global::System.ComponentModel.ComponentResourceManager componentResourceManager = new global::System.ComponentModel.ComponentResourceManager(typeof(global::System.ServiceProcess.Design.ServiceInstallerDialog));
			this.okButton = new global::System.Windows.Forms.Button();
			this.passwordEdit = new global::System.Windows.Forms.TextBox();
			this.cancelButton = new global::System.Windows.Forms.Button();
			this.confirmPassword = new global::System.Windows.Forms.TextBox();
			this.usernameEdit = new global::System.Windows.Forms.TextBox();
			this.label1 = new global::System.Windows.Forms.Label();
			this.label2 = new global::System.Windows.Forms.Label();
			this.label3 = new global::System.Windows.Forms.Label();
			this.okCancelTableLayoutPanel = new global::System.Windows.Forms.TableLayoutPanel();
			this.overarchingTableLayoutPanel = new global::System.Windows.Forms.TableLayoutPanel();
			this.okCancelTableLayoutPanel.SuspendLayout();
			this.overarchingTableLayoutPanel.SuspendLayout();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.okButton, "okButton");
			this.okButton.AutoSizeMode = global::System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.okButton.DialogResult = global::System.Windows.Forms.DialogResult.OK;
			this.okButton.Margin = new global::System.Windows.Forms.Padding(0, 0, 3, 0);
			this.okButton.MinimumSize = new global::System.Drawing.Size(75, 23);
			this.okButton.Name = "okButton";
			this.okButton.Padding = new global::System.Windows.Forms.Padding(10, 0, 10, 0);
			this.okButton.Click += new global::System.EventHandler(this.okButton_Click);
			componentResourceManager.ApplyResources(this.passwordEdit, "passwordEdit");
			this.passwordEdit.Margin = new global::System.Windows.Forms.Padding(3, 3, 0, 3);
			this.passwordEdit.Name = "passwordEdit";
			componentResourceManager.ApplyResources(this.cancelButton, "cancelButton");
			this.cancelButton.AutoSizeMode = global::System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.cancelButton.DialogResult = global::System.Windows.Forms.DialogResult.Cancel;
			this.cancelButton.Margin = new global::System.Windows.Forms.Padding(3, 0, 0, 0);
			this.cancelButton.MinimumSize = new global::System.Drawing.Size(75, 23);
			this.cancelButton.Name = "cancelButton";
			this.cancelButton.Padding = new global::System.Windows.Forms.Padding(10, 0, 10, 0);
			this.cancelButton.Click += new global::System.EventHandler(this.cancelButton_Click);
			componentResourceManager.ApplyResources(this.confirmPassword, "confirmPassword");
			this.confirmPassword.Margin = new global::System.Windows.Forms.Padding(3, 3, 0, 3);
			this.confirmPassword.Name = "confirmPassword";
			componentResourceManager.ApplyResources(this.usernameEdit, "usernameEdit");
			this.usernameEdit.Margin = new global::System.Windows.Forms.Padding(3, 0, 0, 3);
			this.usernameEdit.Name = "usernameEdit";
			componentResourceManager.ApplyResources(this.label1, "label1");
			this.label1.Margin = new global::System.Windows.Forms.Padding(0, 0, 3, 3);
			this.label1.Name = "label1";
			componentResourceManager.ApplyResources(this.label2, "label2");
			this.label2.Margin = new global::System.Windows.Forms.Padding(0, 3, 3, 3);
			this.label2.Name = "label2";
			componentResourceManager.ApplyResources(this.label3, "label3");
			this.label3.Margin = new global::System.Windows.Forms.Padding(0, 3, 3, 3);
			this.label3.Name = "label3";
			componentResourceManager.ApplyResources(this.okCancelTableLayoutPanel, "okCancelTableLayoutPanel");
			this.okCancelTableLayoutPanel.AutoSizeMode = global::System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.overarchingTableLayoutPanel.SetColumnSpan(this.okCancelTableLayoutPanel, 2);
			this.okCancelTableLayoutPanel.ColumnStyles.Add(new global::System.Windows.Forms.ColumnStyle(global::System.Windows.Forms.SizeType.Percent, 50f));
			this.okCancelTableLayoutPanel.ColumnStyles.Add(new global::System.Windows.Forms.ColumnStyle(global::System.Windows.Forms.SizeType.Percent, 50f));
			this.okCancelTableLayoutPanel.Controls.Add(this.okButton, 0, 0);
			this.okCancelTableLayoutPanel.Controls.Add(this.cancelButton, 1, 0);
			this.okCancelTableLayoutPanel.Margin = new global::System.Windows.Forms.Padding(0, 6, 0, 0);
			this.okCancelTableLayoutPanel.Name = "okCancelTableLayoutPanel";
			this.okCancelTableLayoutPanel.RowStyles.Add(new global::System.Windows.Forms.RowStyle(global::System.Windows.Forms.SizeType.Percent, 50f));
			componentResourceManager.ApplyResources(this.overarchingTableLayoutPanel, "overarchingTableLayoutPanel");
			this.overarchingTableLayoutPanel.ColumnStyles.Add(new global::System.Windows.Forms.ColumnStyle());
			this.overarchingTableLayoutPanel.ColumnStyles.Add(new global::System.Windows.Forms.ColumnStyle(global::System.Windows.Forms.SizeType.Percent, 50f));
			this.overarchingTableLayoutPanel.Controls.Add(this.label1, 0, 0);
			this.overarchingTableLayoutPanel.Controls.Add(this.okCancelTableLayoutPanel, 0, 3);
			this.overarchingTableLayoutPanel.Controls.Add(this.label2, 0, 1);
			this.overarchingTableLayoutPanel.Controls.Add(this.confirmPassword, 1, 2);
			this.overarchingTableLayoutPanel.Controls.Add(this.label3, 0, 2);
			this.overarchingTableLayoutPanel.Controls.Add(this.passwordEdit, 1, 1);
			this.overarchingTableLayoutPanel.Controls.Add(this.usernameEdit, 1, 0);
			this.overarchingTableLayoutPanel.Name = "overarchingTableLayoutPanel";
			this.overarchingTableLayoutPanel.RowStyles.Add(new global::System.Windows.Forms.RowStyle());
			this.overarchingTableLayoutPanel.RowStyles.Add(new global::System.Windows.Forms.RowStyle());
			this.overarchingTableLayoutPanel.RowStyles.Add(new global::System.Windows.Forms.RowStyle());
			this.overarchingTableLayoutPanel.RowStyles.Add(new global::System.Windows.Forms.RowStyle());
			base.AcceptButton = this.okButton;
			componentResourceManager.ApplyResources(this, "$this");
			base.AutoScaleMode = global::System.Windows.Forms.AutoScaleMode.Font;
			base.AutoScaleDimensions = new global::System.Drawing.SizeF(6f, 13f);
			base.AutoScaleMode = global::System.Windows.Forms.AutoScaleMode.Font;
			base.CancelButton = this.cancelButton;
			base.Controls.Add(this.overarchingTableLayoutPanel);
			base.FormBorderStyle = global::System.Windows.Forms.FormBorderStyle.FixedDialog;
			base.HelpButton = true;
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "ServiceInstallerDialog";
			base.ShowIcon = false;
			base.ShowInTaskbar = false;
			base.HelpButtonClicked += new global::System.ComponentModel.CancelEventHandler(this.ServiceInstallerDialog_HelpButtonClicked);
			this.okCancelTableLayoutPanel.ResumeLayout(false);
			this.okCancelTableLayoutPanel.PerformLayout();
			this.overarchingTableLayoutPanel.ResumeLayout(false);
			this.overarchingTableLayoutPanel.PerformLayout();
			base.ResumeLayout(false);
		}

		// Token: 0x04000244 RID: 580
		private global::System.Windows.Forms.Button okButton;

		// Token: 0x04000245 RID: 581
		private global::System.Windows.Forms.TextBox passwordEdit;

		// Token: 0x04000246 RID: 582
		private global::System.Windows.Forms.Button cancelButton;

		// Token: 0x04000247 RID: 583
		private global::System.Windows.Forms.TextBox confirmPassword;

		// Token: 0x04000248 RID: 584
		private global::System.Windows.Forms.TextBox usernameEdit;

		// Token: 0x04000249 RID: 585
		private global::System.Windows.Forms.Label label1;

		// Token: 0x0400024A RID: 586
		private global::System.Windows.Forms.Label label2;

		// Token: 0x0400024B RID: 587
		private global::System.Windows.Forms.Label label3;

		// Token: 0x0400024C RID: 588
		private global::System.Windows.Forms.TableLayoutPanel okCancelTableLayoutPanel;

		// Token: 0x0400024D RID: 589
		private global::System.Windows.Forms.TableLayoutPanel overarchingTableLayoutPanel;
	}
}
