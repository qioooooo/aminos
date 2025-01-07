namespace System.Windows.Forms.Design
{
	internal partial class FormatStringDialog : global::System.Windows.Forms.Form
	{
		private void InitializeComponent()
		{
			this.cancelButton = new global::System.Windows.Forms.Button();
			this.okButton = new global::System.Windows.Forms.Button();
			this.formatControl1 = new global::System.Windows.Forms.Design.FormatControl();
			base.SuspendLayout();
			this.formatControl1.Location = new global::System.Drawing.Point(10, 10);
			this.formatControl1.Margin = new global::System.Windows.Forms.Padding(0);
			this.formatControl1.Name = "formatControl1";
			this.formatControl1.Size = new global::System.Drawing.Size(376, 268);
			this.formatControl1.TabIndex = 0;
			this.cancelButton.Location = new global::System.Drawing.Point(299, 288);
			this.cancelButton.Name = "cancelButton";
			this.cancelButton.Size = new global::System.Drawing.Size(87, 23);
			this.cancelButton.TabIndex = 2;
			this.cancelButton.Text = global::System.Design.SR.GetString("DataGridView_Cancel");
			this.cancelButton.DialogResult = global::System.Windows.Forms.DialogResult.Cancel;
			this.cancelButton.Click += new global::System.EventHandler(this.cancelButton_Click);
			this.okButton.Location = new global::System.Drawing.Point(203, 288);
			this.okButton.Name = "okButton";
			this.okButton.Size = new global::System.Drawing.Size(87, 23);
			this.okButton.TabIndex = 1;
			this.okButton.Text = global::System.Design.SR.GetString("DataGridView_OK");
			this.okButton.DialogResult = global::System.Windows.Forms.DialogResult.OK;
			this.okButton.Click += new global::System.EventHandler(this.okButton_Click);
			base.AutoScaleMode = global::System.Windows.Forms.AutoScaleMode.Font;
			base.AutoScaleDimensions = new global::System.Drawing.SizeF(6f, 13f);
			base.ClientSize = new global::System.Drawing.Size(396, 295);
			this.AutoSize = true;
			base.HelpButton = true;
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.FormBorderStyle = global::System.Windows.Forms.FormBorderStyle.FixedDialog;
			base.StartPosition = global::System.Windows.Forms.FormStartPosition.CenterScreen;
			base.ShowInTaskbar = false;
			base.Icon = null;
			base.Name = "Form1";
			base.Controls.Add(this.okButton);
			base.Controls.Add(this.formatControl1);
			base.Controls.Add(this.cancelButton);
			base.Padding = new global::System.Windows.Forms.Padding(0);
			this.Text = global::System.Design.SR.GetString("FormatStringDialogTitle");
			base.HelpButtonClicked += new global::System.ComponentModel.CancelEventHandler(this.FormatStringDialog_HelpButtonClicked);
			base.HelpRequested += new global::System.Windows.Forms.HelpEventHandler(this.FormatStringDialog_HelpRequested);
			base.Load += new global::System.EventHandler(this.FormatStringDialog_Load);
			base.ResumeLayout(false);
		}

		private global::System.Windows.Forms.Button cancelButton;

		private global::System.Windows.Forms.Button okButton;

		private global::System.Windows.Forms.Design.FormatControl formatControl1;
	}
}
