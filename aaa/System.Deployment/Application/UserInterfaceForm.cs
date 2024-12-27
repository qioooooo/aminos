using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace System.Deployment.Application
{
	// Token: 0x020000D2 RID: 210
	internal partial class UserInterfaceForm : Form
	{
		// Token: 0x0600058C RID: 1420 RVA: 0x0001DC31 File Offset: 0x0001CC31
		public UserInterfaceForm(ManualResetEvent readyEvent, SplashInfo splashInfo)
		{
			this.onLoadEvent = readyEvent;
			this.splashPieceInfo = splashInfo;
			base.SuspendLayout();
			this.InitializeComponent();
			this.InitializeContent();
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		// Token: 0x0600058D RID: 1421 RVA: 0x0001DC66 File Offset: 0x0001CC66
		public ProgressPiece ConstructProgressPiece(UserInterfaceInfo info)
		{
			return new ProgressPiece(this, info);
		}

		// Token: 0x0600058E RID: 1422 RVA: 0x0001DC6F File Offset: 0x0001CC6F
		public UpdatePiece ConstructUpdatePiece(UserInterfaceInfo info, ManualResetEvent modalEvent)
		{
			return new UpdatePiece(this, info, modalEvent);
		}

		// Token: 0x0600058F RID: 1423 RVA: 0x0001DC79 File Offset: 0x0001CC79
		public ErrorPiece ConstructErrorPiece(string title, string message, string logFileLocation, string linkUrl, string linkUrlMessage, ManualResetEvent modalEvent)
		{
			return new ErrorPiece(this, title, message, logFileLocation, linkUrl, linkUrlMessage, modalEvent);
		}

		// Token: 0x06000590 RID: 1424 RVA: 0x0001DC8A File Offset: 0x0001CC8A
		public PlatformPiece ConstructPlatformPiece(string platformDetectionErrorMsg, Uri supportUrl, ManualResetEvent modalEvent)
		{
			return new PlatformPiece(this, platformDetectionErrorMsg, supportUrl, modalEvent);
		}

		// Token: 0x06000591 RID: 1425 RVA: 0x0001DC95 File Offset: 0x0001CC95
		public MaintenancePiece ConstructMaintenancePiece(UserInterfaceInfo info, MaintenanceInfo maintenanceInfo, ManualResetEvent modalEvent)
		{
			return new MaintenancePiece(this, info, maintenanceInfo, modalEvent);
		}

		// Token: 0x06000592 RID: 1426 RVA: 0x0001DCA0 File Offset: 0x0001CCA0
		public void ShowSimpleMessageBox(string message, string caption)
		{
			MessageBoxOptions messageBoxOptions = (MessageBoxOptions)0;
			if (this.IsRightToLeft(this))
			{
				messageBoxOptions |= MessageBoxOptions.RightAlign | MessageBoxOptions.RtlReading;
			}
			MessageBox.Show(this, message, caption, MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1, messageBoxOptions);
		}

		// Token: 0x06000593 RID: 1427 RVA: 0x0001DCD0 File Offset: 0x0001CCD0
		public void SwitchUserInterfacePiece(FormPiece piece)
		{
			FormPiece formPiece = this.currentPiece;
			this.currentPiece = piece;
			this.currentPiece.Dock = DockStyle.Fill;
			base.SuspendLayout();
			base.Controls.Add(this.currentPiece);
			if (formPiece != null)
			{
				base.Controls.Remove(formPiece);
				formPiece.Dispose();
			}
			base.ClientSize = this.currentPiece.ClientSize;
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		// Token: 0x06000594 RID: 1428 RVA: 0x0001DD43 File Offset: 0x0001CD43
		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
			this.onLoadEvent.Set();
		}

		// Token: 0x06000595 RID: 1429 RVA: 0x0001DD58 File Offset: 0x0001CD58
		protected override void OnVisibleChanged(EventArgs e)
		{
			base.OnVisibleChanged(e);
			if (base.Visible && Form.ActiveForm != this)
			{
				base.Activate();
			}
		}

		// Token: 0x06000596 RID: 1430 RVA: 0x0001DD78 File Offset: 0x0001CD78
		protected override void OnClosing(CancelEventArgs e)
		{
			base.OnClosing(e);
			if (!this.currentPiece.OnClosing())
			{
				e.Cancel = true;
				return;
			}
			e.Cancel = true;
			base.Hide();
		}

		// Token: 0x06000597 RID: 1431 RVA: 0x0001DDB0 File Offset: 0x0001CDB0
		protected override void SetVisibleCore(bool value)
		{
			if (this.splashPieceInfo.initializedAsWait)
			{
				base.SetVisibleCore(false);
				return;
			}
			base.SetVisibleCore(value);
		}

		// Token: 0x0600059A RID: 1434 RVA: 0x0001DE6B File Offset: 0x0001CE6B
		private void InitializeContent()
		{
			base.Icon = Resources.GetIcon("form.ico");
			this.Font = SystemFonts.MessageBoxFont;
			this.currentPiece = new SplashPiece(this, this.splashPieceInfo);
			base.Controls.Add(this.currentPiece);
		}

		// Token: 0x0600059B RID: 1435 RVA: 0x0001DEAB File Offset: 0x0001CEAB
		private bool IsRightToLeft(Control control)
		{
			return control.RightToLeft == RightToLeft.Yes || (control.RightToLeft != RightToLeft.No && (control.RightToLeft == RightToLeft.Inherit && control.Parent != null) && this.IsRightToLeft(control.Parent));
		}

		// Token: 0x04000495 RID: 1173
		private SplashInfo splashPieceInfo;

		// Token: 0x04000496 RID: 1174
		private ManualResetEvent onLoadEvent;

		// Token: 0x020000D3 RID: 211
		// (Invoke) Token: 0x0600059D RID: 1437
		public delegate ProgressPiece ConstructProgressPieceDelegate(UserInterfaceInfo info);

		// Token: 0x020000D4 RID: 212
		// (Invoke) Token: 0x060005A1 RID: 1441
		public delegate UpdatePiece ConstructUpdatePieceDelegate(UserInterfaceInfo info, ManualResetEvent modalEvent);

		// Token: 0x020000D5 RID: 213
		// (Invoke) Token: 0x060005A5 RID: 1445
		public delegate ErrorPiece ConstructErrorPieceDelegate(string title, string message, string logFileLocation, string linkUrl, string linkUrlMessage, ManualResetEvent modalEvent);

		// Token: 0x020000D6 RID: 214
		// (Invoke) Token: 0x060005A9 RID: 1449
		public delegate PlatformPiece ConstructPlatformPieceDelegate(string platformDetectionErrorMsg, Uri supportUrl, ManualResetEvent modalEvent);

		// Token: 0x020000D7 RID: 215
		// (Invoke) Token: 0x060005AD RID: 1453
		public delegate MaintenancePiece ConstructMaintenancePieceDelegate(UserInterfaceInfo info, MaintenanceInfo maintenanceInfo, ManualResetEvent modalEvent);

		// Token: 0x020000D8 RID: 216
		// (Invoke) Token: 0x060005B1 RID: 1457
		public delegate void ShowSimpleMessageBoxDelegate(string messsage, string caption);
	}
}
