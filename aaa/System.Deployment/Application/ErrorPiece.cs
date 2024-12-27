using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace System.Deployment.Application
{
	// Token: 0x0200004D RID: 77
	internal class ErrorPiece : ModalPiece
	{
		// Token: 0x06000264 RID: 612 RVA: 0x0000F2D0 File Offset: 0x0000E2D0
		public ErrorPiece(UserInterfaceForm parentForm, string errorTitle, string errorMessage, string logFileLocation, string linkUrl, string linkUrlMessage, ManualResetEvent modalEvent)
		{
			this._errorMessage = errorMessage;
			this._logFileLocation = logFileLocation;
			this._linkUrl = linkUrl;
			this._linkUrlMessage = linkUrlMessage;
			this._modalResult = UserInterfaceModalResult.Ok;
			this._modalEvent = modalEvent;
			base.SuspendLayout();
			this.InitializeComponent();
			this.InitializeContent();
			base.ResumeLayout(false);
			parentForm.SuspendLayout();
			parentForm.SwitchUserInterfacePiece(this);
			parentForm.Text = errorTitle;
			parentForm.MinimizeBox = false;
			parentForm.MaximizeBox = false;
			parentForm.ControlBox = true;
			parentForm.ActiveControl = this.btnOk;
			parentForm.ResumeLayout(false);
			parentForm.PerformLayout();
			parentForm.Visible = true;
			if (Form.ActiveForm != parentForm)
			{
				parentForm.Activate();
			}
		}

		// Token: 0x06000265 RID: 613 RVA: 0x0000F384 File Offset: 0x0000E384
		private void InitializeComponent()
		{
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(ErrorPiece));
			this.lblMessage = new Label();
			this.pictureIcon = new PictureBox();
			this.btnOk = new Button();
			this.btnSupport = new Button();
			this.okDetailsTableLayoutPanel = new TableLayoutPanel();
			this.overarchingTableLayoutPanel = new TableLayoutPanel();
			this.errorLink = new LinkLabel();
			((ISupportInitialize)this.pictureIcon).BeginInit();
			this.okDetailsTableLayoutPanel.SuspendLayout();
			this.overarchingTableLayoutPanel.SuspendLayout();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.lblMessage, "lblMessage");
			this.lblMessage.Name = "lblMessage";
			componentResourceManager.ApplyResources(this.pictureIcon, "pictureIcon");
			this.pictureIcon.Name = "pictureIcon";
			this.pictureIcon.TabStop = false;
			componentResourceManager.ApplyResources(this.btnOk, "btnOk");
			this.btnOk.MinimumSize = new Size(75, 23);
			this.btnOk.Name = "btnOk";
			this.btnOk.Click += this.btnOk_Click;
			componentResourceManager.ApplyResources(this.btnSupport, "btnSupport");
			this.btnSupport.MinimumSize = new Size(75, 23);
			this.btnSupport.Name = "btnSupport";
			this.btnSupport.Click += this.btnSupport_Click;
			componentResourceManager.ApplyResources(this.okDetailsTableLayoutPanel, "okDetailsTableLayoutPanel");
			this.overarchingTableLayoutPanel.SetColumnSpan(this.okDetailsTableLayoutPanel, 2);
			this.okDetailsTableLayoutPanel.Controls.Add(this.btnOk, 0, 0);
			this.okDetailsTableLayoutPanel.Controls.Add(this.btnSupport, 1, 0);
			this.okDetailsTableLayoutPanel.Name = "okDetailsTableLayoutPanel";
			componentResourceManager.ApplyResources(this.overarchingTableLayoutPanel, "overarchingTableLayoutPanel");
			this.overarchingTableLayoutPanel.Controls.Add(this.pictureIcon, 0, 0);
			this.overarchingTableLayoutPanel.Controls.Add(this.okDetailsTableLayoutPanel, 0, 2);
			this.overarchingTableLayoutPanel.Controls.Add(this.lblMessage, 1, 0);
			this.overarchingTableLayoutPanel.Controls.Add(this.errorLink, 1, 1);
			this.overarchingTableLayoutPanel.MinimumSize = new Size(348, 99);
			this.overarchingTableLayoutPanel.Name = "overarchingTableLayoutPanel";
			componentResourceManager.ApplyResources(this.errorLink, "errorLink");
			this.errorLink.MinimumSize = new Size(300, 32);
			this.errorLink.Name = "errorLink";
			this.errorLink.LinkClicked += this.errorLink_LinkClicked;
			componentResourceManager.ApplyResources(this, "$this");
			base.Controls.Add(this.overarchingTableLayoutPanel);
			this.MinimumSize = new Size(373, 124);
			base.Name = "ErrorPiece";
			((ISupportInitialize)this.pictureIcon).EndInit();
			this.okDetailsTableLayoutPanel.ResumeLayout(false);
			this.okDetailsTableLayoutPanel.PerformLayout();
			this.overarchingTableLayoutPanel.ResumeLayout(false);
			this.overarchingTableLayoutPanel.PerformLayout();
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		// Token: 0x06000266 RID: 614 RVA: 0x0000F6C8 File Offset: 0x0000E6C8
		private void InitializeContent()
		{
			Bitmap bitmap = (Bitmap)Resources.GetImage("information.bmp");
			bitmap.MakeTransparent();
			this.pictureIcon.Image = bitmap;
			this.lblMessage.Text = this._errorMessage;
			if (this._linkUrl != null && this._linkUrlMessage != null)
			{
				string @string = Resources.GetString("UI_ErrorClickHereHere");
				this.errorLink.Text = this._linkUrlMessage;
				int num = this._linkUrlMessage.LastIndexOf(@string, StringComparison.Ordinal);
				this.errorLink.Links.Add(num, @string.Length, this._linkUrl);
			}
			else
			{
				this.errorLink.Text = string.Empty;
				this.errorLink.Links.Clear();
			}
			if (this._logFileLocation == null)
			{
				this.btnSupport.Enabled = false;
			}
		}

		// Token: 0x06000267 RID: 615 RVA: 0x0000F798 File Offset: 0x0000E798
		private void errorLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			this.errorLink.Links[this.errorLink.Links.IndexOf(e.Link)].Visited = true;
			if (this._linkUrl != null && UserInterface.IsValidHttpUrl(this._linkUrl))
			{
				UserInterface.LaunchUrlInBrowser(e.Link.LinkData.ToString());
			}
		}

		// Token: 0x06000268 RID: 616 RVA: 0x0000F7FB File Offset: 0x0000E7FB
		private void btnOk_Click(object sender, EventArgs e)
		{
			this._modalResult = UserInterfaceModalResult.Ok;
			this._modalEvent.Set();
			base.Enabled = false;
		}

		// Token: 0x06000269 RID: 617 RVA: 0x0000F818 File Offset: 0x0000E818
		private void btnSupport_Click(object sender, EventArgs e)
		{
			try
			{
				Process.Start("notepad.exe", this._logFileLocation);
			}
			catch (Win32Exception)
			{
			}
		}

		// Token: 0x040001D8 RID: 472
		private Label lblMessage;

		// Token: 0x040001D9 RID: 473
		private PictureBox pictureIcon;

		// Token: 0x040001DA RID: 474
		private Button btnOk;

		// Token: 0x040001DB RID: 475
		private Button btnSupport;

		// Token: 0x040001DC RID: 476
		private TableLayoutPanel okDetailsTableLayoutPanel;

		// Token: 0x040001DD RID: 477
		private TableLayoutPanel overarchingTableLayoutPanel;

		// Token: 0x040001DE RID: 478
		private LinkLabel errorLink;

		// Token: 0x040001DF RID: 479
		private string _errorMessage;

		// Token: 0x040001E0 RID: 480
		private string _logFileLocation;

		// Token: 0x040001E1 RID: 481
		private string _linkUrl;

		// Token: 0x040001E2 RID: 482
		private string _linkUrlMessage;
	}
}
