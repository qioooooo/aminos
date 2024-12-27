using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Windows.Forms;

namespace System.Deployment.Application
{
	// Token: 0x020000C5 RID: 197
	internal class ProgressPiece : FormPiece, IDownloadNotification
	{
		// Token: 0x060004CE RID: 1230 RVA: 0x00018908 File Offset: 0x00017908
		public ProgressPiece(UserInterfaceForm parentForm, UserInterfaceInfo info)
		{
			this._info = info;
			base.SuspendLayout();
			this.InitializeComponent();
			this.InitializeContent();
			base.ResumeLayout(false);
			parentForm.SuspendLayout();
			parentForm.SwitchUserInterfacePiece(this);
			parentForm.Text = string.Format(CultureInfo.CurrentUICulture, Resources.GetString("UI_ProgressTitle"), new object[]
			{
				0,
				this._info.formTitle
			});
			parentForm.MinimizeBox = true;
			parentForm.MaximizeBox = false;
			parentForm.ControlBox = true;
			this.lblHeader.Font = new Font(this.lblHeader.Font, this.lblHeader.Font.Style | FontStyle.Bold);
			this.linkAppId.Font = new Font(this.linkAppId.Font, this.linkAppId.Font.Style | FontStyle.Bold);
			this.lblFromId.Font = new Font(this.lblFromId.Font, this.lblFromId.Font.Style | FontStyle.Bold);
			parentForm.ActiveControl = this.btnCancel;
			parentForm.ResumeLayout(false);
			parentForm.PerformLayout();
			parentForm.Visible = true;
			this.updateUIMethodInvoker = new MethodInvoker(this.UpdateUI);
			this.disableMethodInvoker = new MethodInvoker(this.Disable);
		}

		// Token: 0x060004CF RID: 1231 RVA: 0x00018A64 File Offset: 0x00017A64
		public void DownloadModified(object sender, DownloadEventArgs e)
		{
			if (this._userCancelling)
			{
				FileDownloader fileDownloader = (FileDownloader)sender;
				fileDownloader.Cancel();
				return;
			}
			this._downloadData = e;
			if (this._info.iconFilePath != null && this._appIconBitmap == null && e.Cookie != null && File.Exists(this._info.iconFilePath))
			{
				using (Icon icon = Icon.ExtractAssociatedIcon(this._info.iconFilePath))
				{
					this._appIconBitmap = this.TryGet32x32Bitmap(icon);
				}
			}
			base.BeginInvoke(this.updateUIMethodInvoker);
		}

		// Token: 0x060004D0 RID: 1232 RVA: 0x00018B04 File Offset: 0x00017B04
		public void DownloadCompleted(object sender, DownloadEventArgs e)
		{
			base.BeginInvoke(this.disableMethodInvoker);
		}

		// Token: 0x060004D1 RID: 1233 RVA: 0x00018B14 File Offset: 0x00017B14
		public override bool OnClosing()
		{
			bool flag = base.OnClosing();
			if (!base.Enabled)
			{
				return false;
			}
			this._userCancelling = true;
			return flag;
		}

		// Token: 0x060004D2 RID: 1234 RVA: 0x00018B3C File Offset: 0x00017B3C
		private void InitializeComponent()
		{
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(ProgressPiece));
			this.topTextTableLayoutPanel = new TableLayoutPanel();
			this.pictureDesktop = new PictureBox();
			this.lblSubHeader = new Label();
			this.lblHeader = new Label();
			this.pictureAppIcon = new PictureBox();
			this.lblApplication = new Label();
			this.linkAppId = new LinkLabel();
			this.lblFrom = new Label();
			this.lblFromId = new Label();
			this.progress = new ProgressBar();
			this.lblProgressText = new Label();
			this.groupRule = new GroupBox();
			this.groupDivider = new GroupBox();
			this.btnCancel = new Button();
			this.overarchingTableLayoutPanel = new TableLayoutPanel();
			this.contentTableLayoutPanel = new TableLayoutPanel();
			this.topTextTableLayoutPanel.SuspendLayout();
			((ISupportInitialize)this.pictureDesktop).BeginInit();
			((ISupportInitialize)this.pictureAppIcon).BeginInit();
			this.overarchingTableLayoutPanel.SuspendLayout();
			this.contentTableLayoutPanel.SuspendLayout();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.topTextTableLayoutPanel, "topTextTableLayoutPanel");
			this.topTextTableLayoutPanel.BackColor = SystemColors.Window;
			this.topTextTableLayoutPanel.Controls.Add(this.pictureDesktop, 1, 0);
			this.topTextTableLayoutPanel.Controls.Add(this.lblSubHeader, 0, 1);
			this.topTextTableLayoutPanel.Controls.Add(this.lblHeader, 0, 0);
			this.topTextTableLayoutPanel.MinimumSize = new Size(498, 61);
			this.topTextTableLayoutPanel.Name = "topTextTableLayoutPanel";
			componentResourceManager.ApplyResources(this.pictureDesktop, "pictureDesktop");
			this.pictureDesktop.MinimumSize = new Size(61, 61);
			this.pictureDesktop.Name = "pictureDesktop";
			this.topTextTableLayoutPanel.SetRowSpan(this.pictureDesktop, 2);
			this.pictureDesktop.TabStop = false;
			componentResourceManager.ApplyResources(this.lblSubHeader, "lblSubHeader");
			this.lblSubHeader.Name = "lblSubHeader";
			componentResourceManager.ApplyResources(this.lblHeader, "lblHeader");
			this.lblHeader.AutoEllipsis = true;
			this.lblHeader.Name = "lblHeader";
			this.lblHeader.UseMnemonic = false;
			componentResourceManager.ApplyResources(this.pictureAppIcon, "pictureAppIcon");
			this.pictureAppIcon.Name = "pictureAppIcon";
			this.pictureAppIcon.TabStop = false;
			componentResourceManager.ApplyResources(this.lblApplication, "lblApplication");
			this.lblApplication.Name = "lblApplication";
			componentResourceManager.ApplyResources(this.linkAppId, "linkAppId");
			this.linkAppId.AutoEllipsis = true;
			this.linkAppId.Name = "linkAppId";
			this.linkAppId.UseMnemonic = false;
			this.linkAppId.LinkClicked += this.linkAppId_LinkClicked;
			componentResourceManager.ApplyResources(this.lblFrom, "lblFrom");
			this.lblFrom.Name = "lblFrom";
			componentResourceManager.ApplyResources(this.lblFromId, "lblFromId");
			this.lblFromId.AutoEllipsis = true;
			this.lblFromId.MinimumSize = new Size(384, 32);
			this.lblFromId.Name = "lblFromId";
			this.lblFromId.UseMnemonic = false;
			componentResourceManager.ApplyResources(this.progress, "progress");
			this.contentTableLayoutPanel.SetColumnSpan(this.progress, 2);
			this.progress.Name = "progress";
			this.progress.TabStop = false;
			componentResourceManager.ApplyResources(this.lblProgressText, "lblProgressText");
			this.contentTableLayoutPanel.SetColumnSpan(this.lblProgressText, 2);
			this.lblProgressText.Name = "lblProgressText";
			componentResourceManager.ApplyResources(this.groupRule, "groupRule");
			this.groupRule.BackColor = SystemColors.ControlDark;
			this.groupRule.FlatStyle = FlatStyle.Flat;
			this.groupRule.Name = "groupRule";
			this.groupRule.TabStop = false;
			componentResourceManager.ApplyResources(this.groupDivider, "groupDivider");
			this.groupDivider.BackColor = SystemColors.ControlDark;
			this.groupDivider.FlatStyle = FlatStyle.Flat;
			this.groupDivider.Name = "groupDivider";
			this.groupDivider.TabStop = false;
			componentResourceManager.ApplyResources(this.btnCancel, "btnCancel");
			this.btnCancel.MinimumSize = new Size(75, 23);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Click += this.btnCancel_Click;
			componentResourceManager.ApplyResources(this.overarchingTableLayoutPanel, "overarchingTableLayoutPanel");
			this.overarchingTableLayoutPanel.Controls.Add(this.contentTableLayoutPanel, 0, 2);
			this.overarchingTableLayoutPanel.Controls.Add(this.topTextTableLayoutPanel, 0, 0);
			this.overarchingTableLayoutPanel.Controls.Add(this.groupRule, 0, 1);
			this.overarchingTableLayoutPanel.Controls.Add(this.btnCancel, 0, 4);
			this.overarchingTableLayoutPanel.Controls.Add(this.groupDivider, 0, 3);
			this.overarchingTableLayoutPanel.MinimumSize = new Size(498, 240);
			this.overarchingTableLayoutPanel.Name = "overarchingTableLayoutPanel";
			componentResourceManager.ApplyResources(this.contentTableLayoutPanel, "contentTableLayoutPanel");
			this.contentTableLayoutPanel.Controls.Add(this.pictureAppIcon, 0, 0);
			this.contentTableLayoutPanel.Controls.Add(this.lblApplication, 1, 0);
			this.contentTableLayoutPanel.Controls.Add(this.lblFrom, 1, 1);
			this.contentTableLayoutPanel.Controls.Add(this.lblProgressText, 1, 3);
			this.contentTableLayoutPanel.Controls.Add(this.linkAppId, 2, 0);
			this.contentTableLayoutPanel.Controls.Add(this.progress, 1, 2);
			this.contentTableLayoutPanel.Controls.Add(this.lblFromId, 2, 1);
			this.contentTableLayoutPanel.MinimumSize = new Size(466, 123);
			this.contentTableLayoutPanel.Name = "contentTableLayoutPanel";
			componentResourceManager.ApplyResources(this, "$this");
			base.Controls.Add(this.overarchingTableLayoutPanel);
			this.MinimumSize = new Size(498, 240);
			base.Name = "ProgressPiece";
			this.topTextTableLayoutPanel.ResumeLayout(false);
			this.topTextTableLayoutPanel.PerformLayout();
			((ISupportInitialize)this.pictureDesktop).EndInit();
			((ISupportInitialize)this.pictureAppIcon).EndInit();
			this.overarchingTableLayoutPanel.ResumeLayout(false);
			this.overarchingTableLayoutPanel.PerformLayout();
			this.contentTableLayoutPanel.ResumeLayout(false);
			this.contentTableLayoutPanel.PerformLayout();
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		// Token: 0x060004D3 RID: 1235 RVA: 0x0001921C File Offset: 0x0001821C
		private void InitializeContent()
		{
			this.pictureDesktop.Image = Resources.GetImage("setup.bmp");
			this.lblHeader.Text = this._info.formTitle;
			using (Icon icon = Resources.GetIcon("defaultappicon.ico"))
			{
				this.pictureAppIcon.Image = this.TryGet32x32Bitmap(icon);
			}
			this.linkAppId.Text = this._info.productName;
			this.linkAppId.Links.Clear();
			if (UserInterface.IsValidHttpUrl(this._info.supportUrl))
			{
				this.linkAppId.Links.Add(0, this._info.productName.Length, this._info.supportUrl);
			}
			this.lblFromId.Text = this._info.sourceSite;
		}

		// Token: 0x060004D4 RID: 1236 RVA: 0x00019308 File Offset: 0x00018308
		private void linkAppId_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			this.linkAppId.LinkVisited = true;
			UserInterface.LaunchUrlInBrowser(e.Link.LinkData.ToString());
		}

		// Token: 0x060004D5 RID: 1237 RVA: 0x0001932B File Offset: 0x0001832B
		private void btnCancel_Click(object sender, EventArgs e)
		{
			this._userCancelling = true;
			this.Disable();
		}

		// Token: 0x060004D6 RID: 1238 RVA: 0x0001933A File Offset: 0x0001833A
		private void Disable()
		{
			this.lblProgressText.Text = Resources.GetString("UI_ProgressDone");
			base.Enabled = false;
		}

		// Token: 0x060004D7 RID: 1239 RVA: 0x00019358 File Offset: 0x00018358
		private Bitmap TryGet32x32Bitmap(Icon icon)
		{
			Bitmap bitmap2;
			using (Icon icon2 = new Icon(icon, 32, 32))
			{
				Bitmap bitmap = icon2.ToBitmap();
				bitmap.MakeTransparent();
				bitmap2 = bitmap;
			}
			return bitmap2;
		}

		// Token: 0x060004D8 RID: 1240 RVA: 0x0001939C File Offset: 0x0001839C
		private void UpdateUI()
		{
			if (base.IsDisposed)
			{
				return;
			}
			base.SuspendLayout();
			this.lblProgressText.Text = ProgressPiece.FormatProgressText(this._downloadData.BytesCompleted, this._downloadData.BytesTotal);
			this.progress.Minimum = 0;
			long bytesTotal = this._downloadData.BytesTotal;
			int num2;
			int num3;
			if (bytesTotal > 2147483647L)
			{
				float num = (float)bytesTotal / 2.1474836E+09f;
				num2 = (int)((float)this._downloadData.BytesCompleted / num);
				num3 = int.MaxValue;
			}
			else
			{
				num2 = (int)this._downloadData.BytesCompleted;
				num3 = (int)bytesTotal;
			}
			this.progress.Maximum = num3;
			this.progress.Value = num2;
			Form form = base.FindForm();
			form.Text = string.Format(CultureInfo.CurrentUICulture, Resources.GetString("UI_ProgressTitle"), new object[]
			{
				this._downloadData.Progress,
				this._info.formTitle
			});
			if (!this._appIconShown && this._appIconBitmap != null)
			{
				this.pictureAppIcon.Image = this._appIconBitmap;
				this._appIconShown = true;
			}
			base.ResumeLayout(false);
		}

		// Token: 0x060004D9 RID: 1241 RVA: 0x000194D4 File Offset: 0x000184D4
		private static string FormatProgressText(long completed, long total)
		{
			return string.Format(CultureInfo.CurrentUICulture, Resources.GetString("UI_ProgressText"), new object[]
			{
				ProgressPiece.FormatBytes(completed),
				ProgressPiece.FormatBytes(total)
			});
		}

		// Token: 0x060004DA RID: 1242 RVA: 0x00019510 File Offset: 0x00018510
		private static string FormatBytes(long bytes)
		{
			int num = Array.BinarySearch<long>(ProgressPiece._bytesFormatRanges, bytes);
			num = ((num >= 0) ? (num + 1) : (~num));
			return string.Format(CultureInfo.CurrentUICulture, Resources.GetString(ProgressPiece._bytesFormatStrings[num]), new object[] { (num == 0) ? ((float)bytes) : ((float)bytes / (float)ProgressPiece._bytesFormatRanges[(num - 1) / 3 * 3]) });
		}

		// Token: 0x0400043C RID: 1084
		private Label lblHeader;

		// Token: 0x0400043D RID: 1085
		private Label lblSubHeader;

		// Token: 0x0400043E RID: 1086
		private PictureBox pictureDesktop;

		// Token: 0x0400043F RID: 1087
		private PictureBox pictureAppIcon;

		// Token: 0x04000440 RID: 1088
		private Label lblApplication;

		// Token: 0x04000441 RID: 1089
		private LinkLabel linkAppId;

		// Token: 0x04000442 RID: 1090
		private Label lblFrom;

		// Token: 0x04000443 RID: 1091
		private Label lblFromId;

		// Token: 0x04000444 RID: 1092
		private ProgressBar progress;

		// Token: 0x04000445 RID: 1093
		private Label lblProgressText;

		// Token: 0x04000446 RID: 1094
		private GroupBox groupRule;

		// Token: 0x04000447 RID: 1095
		private GroupBox groupDivider;

		// Token: 0x04000448 RID: 1096
		private Button btnCancel;

		// Token: 0x04000449 RID: 1097
		private TableLayoutPanel topTextTableLayoutPanel;

		// Token: 0x0400044A RID: 1098
		private TableLayoutPanel overarchingTableLayoutPanel;

		// Token: 0x0400044B RID: 1099
		private TableLayoutPanel contentTableLayoutPanel;

		// Token: 0x0400044C RID: 1100
		private UserInterfaceInfo _info;

		// Token: 0x0400044D RID: 1101
		private bool _userCancelling;

		// Token: 0x0400044E RID: 1102
		private DownloadEventArgs _downloadData;

		// Token: 0x0400044F RID: 1103
		private Bitmap _appIconBitmap;

		// Token: 0x04000450 RID: 1104
		private bool _appIconShown;

		// Token: 0x04000451 RID: 1105
		private MethodInvoker disableMethodInvoker;

		// Token: 0x04000452 RID: 1106
		private MethodInvoker updateUIMethodInvoker;

		// Token: 0x04000453 RID: 1107
		private static long[] _bytesFormatRanges = new long[] { 1024L, 10240L, 102400L, 1048576L, 10485760L, 104857600L, 1073741824L, 10737418240L, 107374182400L };

		// Token: 0x04000454 RID: 1108
		private static string[] _bytesFormatStrings = new string[] { "UI_ProgressBytesInBytes", "UI_ProgressBytesIn1KB", "UI_ProgressBytesIn10KB", "UI_ProgressBytesIn100KB", "UI_ProgressBytesIn1MB", "UI_ProgressBytesIn10MB", "UI_ProgressBytesIn100MB", "UI_ProgressBytesIn1GB", "UI_ProgressBytesIn10GB", "UI_ProgressBytesIn100GB" };
	}
}
