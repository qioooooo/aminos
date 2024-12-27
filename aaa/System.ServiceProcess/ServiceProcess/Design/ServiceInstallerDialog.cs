using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace System.ServiceProcess.Design
{
	// Token: 0x0200003A RID: 58
	public partial class ServiceInstallerDialog : Form
	{
		// Token: 0x06000119 RID: 281 RVA: 0x00006369 File Offset: 0x00005369
		public ServiceInstallerDialog()
		{
			this.InitializeComponent();
		}

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x0600011A RID: 282 RVA: 0x00006377 File Offset: 0x00005377
		// (set) Token: 0x0600011B RID: 283 RVA: 0x00006384 File Offset: 0x00005384
		public string Password
		{
			get
			{
				return this.passwordEdit.Text;
			}
			set
			{
				this.passwordEdit.Text = value;
			}
		}

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x0600011C RID: 284 RVA: 0x00006392 File Offset: 0x00005392
		public ServiceInstallerDialogResult Result
		{
			get
			{
				return this.result;
			}
		}

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x0600011D RID: 285 RVA: 0x0000639A File Offset: 0x0000539A
		// (set) Token: 0x0600011E RID: 286 RVA: 0x000063A7 File Offset: 0x000053A7
		public string Username
		{
			get
			{
				return this.usernameEdit.Text;
			}
			set
			{
				this.usernameEdit.Text = value;
			}
		}

		// Token: 0x0600011F RID: 287 RVA: 0x000063B5 File Offset: 0x000053B5
		[STAThread]
		public static void Main()
		{
			Application.Run(new ServiceInstallerDialog());
		}

		// Token: 0x06000121 RID: 289 RVA: 0x000069C3 File Offset: 0x000059C3
		private void cancelButton_Click(object sender, EventArgs e)
		{
			this.result = ServiceInstallerDialogResult.Canceled;
			base.DialogResult = DialogResult.Cancel;
		}

		// Token: 0x06000122 RID: 290 RVA: 0x000069D4 File Offset: 0x000059D4
		private void okButton_Click(object sender, EventArgs e)
		{
			this.result = ServiceInstallerDialogResult.OK;
			if (this.passwordEdit.Text == this.confirmPassword.Text)
			{
				base.DialogResult = DialogResult.OK;
				return;
			}
			MessageBoxOptions messageBoxOptions = (MessageBoxOptions)0;
			Control control = this;
			while (control.RightToLeft == RightToLeft.Inherit)
			{
				control = control.Parent;
			}
			if (control.RightToLeft == RightToLeft.Yes)
			{
				messageBoxOptions = MessageBoxOptions.RightAlign | MessageBoxOptions.RtlReading;
			}
			base.DialogResult = DialogResult.None;
			MessageBox.Show(Res.GetString("Label_MissmatchedPasswords"), Res.GetString("Label_SetServiceLogin"), MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, messageBoxOptions);
			this.passwordEdit.Text = string.Empty;
			this.confirmPassword.Text = string.Empty;
			this.passwordEdit.Focus();
		}

		// Token: 0x06000123 RID: 291 RVA: 0x00006A84 File Offset: 0x00005A84
		private void ServiceInstallerDialog_HelpButtonClicked(object sender, CancelEventArgs e)
		{
			e.Cancel = true;
		}

		// Token: 0x0400024E RID: 590
		private ServiceInstallerDialogResult result;
	}
}
