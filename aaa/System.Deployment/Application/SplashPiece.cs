using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace System.Deployment.Application
{
	// Token: 0x020000C8 RID: 200
	internal class SplashPiece : FormPiece
	{
		// Token: 0x06000502 RID: 1282 RVA: 0x0001AC78 File Offset: 0x00019C78
		public SplashPiece(UserInterfaceForm parentForm, SplashInfo info)
		{
			this.info = info;
			base.SuspendLayout();
			this.InitializeComponent();
			this.InitializeContent();
			base.ResumeLayout(false);
			parentForm.SuspendLayout();
			parentForm.Text = Resources.GetString("UI_SplashTitle");
			parentForm.MinimizeBox = false;
			parentForm.MaximizeBox = false;
			parentForm.ControlBox = true;
			parentForm.ResumeLayout(false);
			this.splashTimer = new Timer();
			this.splashTimer.Tick += this.SplashTimer_Tick;
			if (info.initializedAsWait)
			{
				this.splashTimer.Interval = 2500;
				this.splashTimer.Tag = null;
				this.splashTimer.Enabled = true;
				return;
			}
			this.ShowSplash(parentForm);
		}

		// Token: 0x06000503 RID: 1283 RVA: 0x0001AD38 File Offset: 0x00019D38
		public override bool OnClosing()
		{
			bool flag = base.OnClosing();
			this.info.cancelled = true;
			this.End();
			return flag;
		}

		// Token: 0x06000504 RID: 1284 RVA: 0x0001AD5F File Offset: 0x00019D5F
		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
			if (disposing)
			{
				this.End();
			}
		}

		// Token: 0x06000505 RID: 1285 RVA: 0x0001AD74 File Offset: 0x00019D74
		private void InitializeComponent()
		{
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(SplashPiece));
			this.pictureWait = new PictureBox();
			this.lblNote = new Label();
			this.overarchingTableLayoutPanel = new TableLayoutPanel();
			((ISupportInitialize)this.pictureWait).BeginInit();
			this.overarchingTableLayoutPanel.SuspendLayout();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.pictureWait, "pictureWait");
			this.pictureWait.Name = "pictureWait";
			this.pictureWait.TabStop = false;
			componentResourceManager.ApplyResources(this.lblNote, "lblNote");
			this.lblNote.Name = "lblNote";
			componentResourceManager.ApplyResources(this.overarchingTableLayoutPanel, "overarchingTableLayoutPanel");
			this.overarchingTableLayoutPanel.Controls.Add(this.pictureWait, 0, 0);
			this.overarchingTableLayoutPanel.Controls.Add(this.lblNote, 0, 1);
			this.overarchingTableLayoutPanel.Name = "overarchingTableLayoutPanel";
			componentResourceManager.ApplyResources(this, "$this");
			base.Controls.Add(this.overarchingTableLayoutPanel);
			base.Name = "SplashPiece";
			((ISupportInitialize)this.pictureWait).EndInit();
			this.overarchingTableLayoutPanel.ResumeLayout(false);
			this.overarchingTableLayoutPanel.PerformLayout();
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		// Token: 0x06000506 RID: 1286 RVA: 0x0001AEC4 File Offset: 0x00019EC4
		private void InitializeContent()
		{
			this.pictureWait.Image = Resources.GetImage("splash.gif");
		}

		// Token: 0x06000507 RID: 1287 RVA: 0x0001AEDB File Offset: 0x00019EDB
		private void End()
		{
			this.info.initializedAsWait = false;
			this.splashTimer.Tag = this;
			this.splashTimer.Dispose();
			this.info.pieceReady.Set();
		}

		// Token: 0x06000508 RID: 1288 RVA: 0x0001AF14 File Offset: 0x00019F14
		private void ShowSplash(Form parentForm)
		{
			this.info.initializedAsWait = false;
			parentForm.Visible = true;
			this.splashTimer.Interval = 1000;
			this.splashTimer.Tag = this;
			this.splashTimer.Enabled = true;
			this.info.pieceReady.Reset();
		}

		// Token: 0x06000509 RID: 1289 RVA: 0x0001AF70 File Offset: 0x00019F70
		private void SplashTimer_Tick(object sender, EventArgs e)
		{
			if (!this.splashTimer.Enabled)
			{
				return;
			}
			this.splashTimer.Enabled = false;
			if (this.splashTimer.Tag != null)
			{
				this.info.pieceReady.Set();
				return;
			}
			this.ShowSplash(base.FindForm());
		}

		// Token: 0x0400045F RID: 1119
		private const int initialDelay = 2500;

		// Token: 0x04000460 RID: 1120
		private const int showDelay = 1000;

		// Token: 0x04000461 RID: 1121
		private PictureBox pictureWait;

		// Token: 0x04000462 RID: 1122
		private Label lblNote;

		// Token: 0x04000463 RID: 1123
		private Timer splashTimer;

		// Token: 0x04000464 RID: 1124
		private TableLayoutPanel overarchingTableLayoutPanel;

		// Token: 0x04000465 RID: 1125
		private SplashInfo info;
	}
}
