using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace System.Deployment.Application
{
	// Token: 0x020000C3 RID: 195
	internal class PlatformPiece : ModalPiece
	{
		// Token: 0x060004C5 RID: 1221 RVA: 0x000183E0 File Offset: 0x000173E0
		public PlatformPiece(UserInterfaceForm parentForm, string platformDetectionErrorMsg, Uri supportUrl, ManualResetEvent modalEvent)
		{
			this._errorMessage = platformDetectionErrorMsg;
			this._supportUrl = supportUrl;
			this._modalResult = UserInterfaceModalResult.Ok;
			this._modalEvent = modalEvent;
			base.SuspendLayout();
			this.InitializeComponent();
			this.InitializeContent();
			base.ResumeLayout(false);
			parentForm.SuspendLayout();
			parentForm.SwitchUserInterfacePiece(this);
			parentForm.Text = Resources.GetString("UI_PlatformDetectionFailedTitle");
			parentForm.MinimizeBox = false;
			parentForm.MaximizeBox = false;
			parentForm.ControlBox = true;
			parentForm.ActiveControl = this.btnOk;
			parentForm.ResumeLayout(false);
			parentForm.PerformLayout();
			parentForm.Visible = true;
		}

		// Token: 0x060004C6 RID: 1222 RVA: 0x0001847C File Offset: 0x0001747C
		private void InitializeComponent()
		{
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(PlatformPiece));
			this.lblMessage = new Label();
			this.pictureIcon = new PictureBox();
			this.btnOk = new Button();
			this.linkSupport = new LinkLabel();
			this.overarchingTableLayoutPanel = new TableLayoutPanel();
			((ISupportInitialize)this.pictureIcon).BeginInit();
			this.overarchingTableLayoutPanel.SuspendLayout();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.lblMessage, "lblMessage");
			this.lblMessage.Name = "lblMessage";
			componentResourceManager.ApplyResources(this.pictureIcon, "pictureIcon");
			this.pictureIcon.Name = "pictureIcon";
			this.pictureIcon.TabStop = false;
			componentResourceManager.ApplyResources(this.btnOk, "btnOk");
			this.overarchingTableLayoutPanel.SetColumnSpan(this.btnOk, 2);
			this.btnOk.MinimumSize = new Size(75, 23);
			this.btnOk.Name = "btnOk";
			this.btnOk.Click += this.btnOk_Click;
			componentResourceManager.ApplyResources(this.linkSupport, "linkSupport");
			this.linkSupport.Name = "linkSupport";
			this.linkSupport.TabStop = true;
			this.linkSupport.LinkClicked += this.linkSupport_LinkClicked;
			componentResourceManager.ApplyResources(this.overarchingTableLayoutPanel, "overarchingTableLayoutPanel");
			this.overarchingTableLayoutPanel.Controls.Add(this.pictureIcon, 0, 0);
			this.overarchingTableLayoutPanel.Controls.Add(this.btnOk, 0, 2);
			this.overarchingTableLayoutPanel.Controls.Add(this.linkSupport, 1, 1);
			this.overarchingTableLayoutPanel.Controls.Add(this.lblMessage, 1, 0);
			this.overarchingTableLayoutPanel.MinimumSize = new Size(349, 88);
			this.overarchingTableLayoutPanel.Name = "overarchingTableLayoutPanel";
			componentResourceManager.ApplyResources(this, "$this");
			base.Controls.Add(this.overarchingTableLayoutPanel);
			this.MinimumSize = new Size(373, 112);
			base.Name = "PlatformPiece";
			((ISupportInitialize)this.pictureIcon).EndInit();
			this.overarchingTableLayoutPanel.ResumeLayout(false);
			this.overarchingTableLayoutPanel.PerformLayout();
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		// Token: 0x060004C7 RID: 1223 RVA: 0x000186E0 File Offset: 0x000176E0
		private void InitializeContent()
		{
			Bitmap bitmap = (Bitmap)Resources.GetImage("information.bmp");
			bitmap.MakeTransparent();
			this.pictureIcon.Image = bitmap;
			this.linkSupport.Links.Clear();
			if (this._supportUrl == null)
			{
				this.linkSupport.Text = Resources.GetString("UI_PlatformContactAdmin");
			}
			else
			{
				string @string = Resources.GetString("UI_PlatformClickHere");
				string string2 = Resources.GetString("UI_PlatformClickHereHere");
				int num = @string.LastIndexOf(string2, StringComparison.Ordinal);
				this.linkSupport.Text = @string;
				this.linkSupport.Links.Add(num, string2.Length, this._supportUrl.AbsoluteUri);
			}
			this.lblMessage.Text = this._errorMessage;
		}

		// Token: 0x060004C8 RID: 1224 RVA: 0x000187A3 File Offset: 0x000177A3
		private void btnOk_Click(object sender, EventArgs e)
		{
			this._modalResult = UserInterfaceModalResult.Ok;
			this._modalEvent.Set();
			base.Enabled = false;
		}

		// Token: 0x060004C9 RID: 1225 RVA: 0x000187C0 File Offset: 0x000177C0
		private void linkSupport_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			this.linkSupport.Links[this.linkSupport.Links.IndexOf(e.Link)].Visited = true;
			if (this._supportUrl != null && UserInterface.IsValidHttpUrl(this._supportUrl.AbsoluteUri))
			{
				UserInterface.LaunchUrlInBrowser(e.Link.LinkData.ToString());
			}
		}

		// Token: 0x04000435 RID: 1077
		private Label lblMessage;

		// Token: 0x04000436 RID: 1078
		private PictureBox pictureIcon;

		// Token: 0x04000437 RID: 1079
		private LinkLabel linkSupport;

		// Token: 0x04000438 RID: 1080
		private Button btnOk;

		// Token: 0x04000439 RID: 1081
		private TableLayoutPanel overarchingTableLayoutPanel;

		// Token: 0x0400043A RID: 1082
		private string _errorMessage;

		// Token: 0x0400043B RID: 1083
		private Uri _supportUrl;
	}
}
