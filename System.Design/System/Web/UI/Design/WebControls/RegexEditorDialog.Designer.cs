﻿namespace System.Web.UI.Design.WebControls
{
	[global::System.ComponentModel.ToolboxItem(false)]
	[global::System.Security.Permissions.SecurityPermission(global::System.Security.Permissions.SecurityAction.Demand, Flags = global::System.Security.Permissions.SecurityPermissionFlag.UnmanagedCode)]
	public partial class RegexEditorDialog : global::System.Windows.Forms.Form
	{
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		private void InitializeComponent()
		{
			this.components = new global::System.ComponentModel.Container();
			this.lblTestResult = new global::System.Windows.Forms.Label();
			this.lstStandardExpressions = new global::System.Windows.Forms.ListBox();
			this.lblStandardExpressions = new global::System.Windows.Forms.Label();
			this.cmdTestValidate = new global::System.Windows.Forms.Button();
			this.txtExpression = new global::System.Windows.Forms.TextBox();
			this.lblInput = new global::System.Windows.Forms.Label();
			this.grpExpression = new global::System.Windows.Forms.GroupBox();
			this.txtSampleInput = new global::System.Windows.Forms.TextBox();
			this.cmdCancel = new global::System.Windows.Forms.Button();
			this.lblExpression = new global::System.Windows.Forms.Label();
			this.cmdOK = new global::System.Windows.Forms.Button();
			global::System.Drawing.Font dialogFont = global::System.Web.UI.Design.Util.UIServiceHelper.GetDialogFont(this.site);
			if (dialogFont != null)
			{
				this.Font = dialogFont;
			}
			string @string = global::System.Design.SR.GetString("RTL");
			if (!string.Equals(@string, "RTL_False", global::System.StringComparison.Ordinal))
			{
				this.RightToLeft = global::System.Windows.Forms.RightToLeft.Yes;
				this.RightToLeftLayout = true;
			}
			base.MinimizeBox = false;
			base.MaximizeBox = false;
			base.ShowInTaskbar = false;
			base.StartPosition = global::System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = global::System.Design.SR.GetString("RegexEditor_Title");
			base.ImeMode = global::System.Windows.Forms.ImeMode.Disable;
			base.AcceptButton = this.cmdOK;
			base.CancelButton = this.cmdCancel;
			base.Icon = null;
			base.FormBorderStyle = global::System.Windows.Forms.FormBorderStyle.FixedDialog;
			base.ClientSize = new global::System.Drawing.Size(348, 215);
			base.Activated += new global::System.EventHandler(this.RegexTypeEditor_Activated);
			base.HelpRequested += new global::System.Windows.Forms.HelpEventHandler(this.Form_HelpRequested);
			base.HelpButton = true;
			base.HelpButtonClicked += new global::System.ComponentModel.CancelEventHandler(this.HelpButton_Click);
			this.lstStandardExpressions.Location = new global::System.Drawing.Point(12, 30);
			this.lstStandardExpressions.Size = new global::System.Drawing.Size(324, 84);
			this.lstStandardExpressions.TabIndex = 1;
			this.lstStandardExpressions.SelectedIndexChanged += new global::System.EventHandler(this.lstStandardExpressions_SelectedIndexChanged);
			this.lstStandardExpressions.Sorted = true;
			this.lstStandardExpressions.IntegralHeight = true;
			this.lstStandardExpressions.Items.AddRange(this.CannedExpressions);
			this.lblStandardExpressions.Location = new global::System.Drawing.Point(12, 12);
			this.lblStandardExpressions.Text = global::System.Design.SR.GetString("RegexEditor_StdExp");
			this.lblStandardExpressions.Size = new global::System.Drawing.Size(328, 16);
			this.lblStandardExpressions.TabIndex = 0;
			this.txtExpression.Location = new global::System.Drawing.Point(12, 140);
			this.txtExpression.TabIndex = 3;
			this.txtExpression.Size = new global::System.Drawing.Size(324, 20);
			this.txtExpression.TextChanged += new global::System.EventHandler(this.txtExpression_TextChanged);
			this.lblExpression.Location = new global::System.Drawing.Point(12, 122);
			this.lblExpression.Text = global::System.Design.SR.GetString("RegexEditor_ValidationExpression");
			this.lblExpression.Size = new global::System.Drawing.Size(328, 16);
			this.lblExpression.TabIndex = 2;
			this.cmdOK.Location = new global::System.Drawing.Point(180, 180);
			this.cmdOK.DialogResult = global::System.Windows.Forms.DialogResult.OK;
			this.cmdOK.Size = new global::System.Drawing.Size(75, 23);
			this.cmdOK.TabIndex = 9;
			this.cmdOK.Text = global::System.Design.SR.GetString("OK");
			this.cmdOK.FlatStyle = global::System.Windows.Forms.FlatStyle.System;
			this.cmdOK.Click += new global::System.EventHandler(this.cmdOK_Click);
			this.cmdCancel.Location = new global::System.Drawing.Point(261, 180);
			this.cmdCancel.DialogResult = global::System.Windows.Forms.DialogResult.Cancel;
			this.cmdCancel.Size = new global::System.Drawing.Size(75, 23);
			this.cmdCancel.TabIndex = 10;
			this.cmdCancel.FlatStyle = global::System.Windows.Forms.FlatStyle.System;
			this.cmdCancel.Text = global::System.Design.SR.GetString("Cancel");
			this.grpExpression.Location = new global::System.Drawing.Point(8, 280);
			this.grpExpression.ImeMode = global::System.Windows.Forms.ImeMode.Disable;
			this.grpExpression.TabIndex = 4;
			this.grpExpression.TabStop = false;
			this.grpExpression.Text = global::System.Design.SR.GetString("RegexEditor_TestExpression");
			this.grpExpression.Size = new global::System.Drawing.Size(328, 80);
			this.grpExpression.Visible = false;
			this.txtSampleInput.Location = new global::System.Drawing.Point(88, 24);
			this.txtSampleInput.TabIndex = 6;
			this.txtSampleInput.Size = new global::System.Drawing.Size(160, 20);
			this.grpExpression.Controls.Add(this.lblTestResult);
			this.grpExpression.Controls.Add(this.txtSampleInput);
			this.grpExpression.Controls.Add(this.cmdTestValidate);
			this.grpExpression.Controls.Add(this.lblInput);
			this.cmdTestValidate.Location = new global::System.Drawing.Point(256, 24);
			this.cmdTestValidate.Size = new global::System.Drawing.Size(56, 20);
			this.cmdTestValidate.TabIndex = 7;
			this.cmdTestValidate.Text = global::System.Design.SR.GetString("RegexEditor_Validate");
			this.cmdTestValidate.FlatStyle = global::System.Windows.Forms.FlatStyle.System;
			this.cmdTestValidate.Click += new global::System.EventHandler(this.cmdTestValidate_Click);
			this.lblInput.Location = new global::System.Drawing.Point(8, 28);
			this.lblInput.Text = global::System.Design.SR.GetString("RegexEditor_SampleInput");
			this.lblInput.Size = new global::System.Drawing.Size(80, 16);
			this.lblInput.TabIndex = 5;
			this.lblTestResult.Location = new global::System.Drawing.Point(8, 56);
			this.lblTestResult.Size = new global::System.Drawing.Size(312, 16);
			this.lblTestResult.TabIndex = 8;
			base.Controls.Add(this.txtExpression);
			base.Controls.Add(this.lstStandardExpressions);
			base.Controls.Add(this.lblStandardExpressions);
			base.Controls.Add(this.lblExpression);
			base.Controls.Add(this.grpExpression);
			base.Controls.Add(this.cmdCancel);
			base.Controls.Add(this.cmdOK);
		}

		private global::System.ComponentModel.Container components;

		private global::System.Windows.Forms.TextBox txtExpression;

		private global::System.Windows.Forms.ListBox lstStandardExpressions;

		private global::System.Windows.Forms.Label lblStandardExpressions;

		private global::System.Windows.Forms.Label lblTestResult;

		private global::System.Windows.Forms.TextBox txtSampleInput;

		private global::System.Windows.Forms.Button cmdTestValidate;

		private global::System.Windows.Forms.Label lblInput;

		private global::System.Windows.Forms.Label lblExpression;

		private global::System.Windows.Forms.GroupBox grpExpression;

		private global::System.Windows.Forms.Button cmdCancel;

		private global::System.Windows.Forms.Button cmdOK;

		private global::System.ComponentModel.ISite site;
	}
}