using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Threading;
using System.Windows.Forms;

namespace System.Deployment.Application
{
	// Token: 0x020000CF RID: 207
	internal class UpdatePiece : ModalPiece
	{
		// Token: 0x06000569 RID: 1385 RVA: 0x0001C924 File Offset: 0x0001B924
		public UpdatePiece(UserInterfaceForm parentForm, UserInterfaceInfo info, ManualResetEvent modalEvent)
		{
			this._info = info;
			this._modalEvent = modalEvent;
			this._modalResult = UserInterfaceModalResult.Cancel;
			base.SuspendLayout();
			this.InitializeComponent();
			this.InitializeContent();
			base.ResumeLayout(false);
			parentForm.SuspendLayout();
			parentForm.SwitchUserInterfacePiece(this);
			parentForm.Text = this._info.formTitle;
			parentForm.MinimizeBox = false;
			parentForm.MaximizeBox = false;
			parentForm.ControlBox = true;
			this.lblHeader.Font = new Font(this.lblHeader.Font, this.lblHeader.Font.Style | FontStyle.Bold);
			this.linkAppId.Font = new Font(this.linkAppId.Font, this.linkAppId.Font.Style | FontStyle.Bold);
			this.lblFromId.Font = new Font(this.lblFromId.Font, this.lblFromId.Font.Style | FontStyle.Bold);
			parentForm.ActiveControl = this.btnOk;
			parentForm.ResumeLayout(false);
			parentForm.PerformLayout();
			parentForm.Visible = true;
		}

		// Token: 0x0600056A RID: 1386 RVA: 0x0001CA40 File Offset: 0x0001BA40
		private void InitializeComponent()
		{
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(UpdatePiece));
			this.descriptionTableLayoutPanel = new TableLayoutPanel();
			this.pictureDesktop = new PictureBox();
			this.lblSubHeader = new Label();
			this.lblHeader = new Label();
			this.lblApplication = new Label();
			this.linkAppId = new LinkLabel();
			this.lblFrom = new Label();
			this.lblFromId = new Label();
			this.groupRule = new GroupBox();
			this.groupDivider = new GroupBox();
			this.btnOk = new Button();
			this.btnSkip = new Button();
			this.contentTableLayoutPanel = new TableLayoutPanel();
			this.okSkipTableLayoutPanel = new TableLayoutPanel();
			this.overarchingTableLayoutPanel = new TableLayoutPanel();
			this.descriptionTableLayoutPanel.SuspendLayout();
			((ISupportInitialize)this.pictureDesktop).BeginInit();
			this.contentTableLayoutPanel.SuspendLayout();
			this.okSkipTableLayoutPanel.SuspendLayout();
			this.overarchingTableLayoutPanel.SuspendLayout();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.descriptionTableLayoutPanel, "descriptionTableLayoutPanel");
			this.descriptionTableLayoutPanel.BackColor = SystemColors.Window;
			this.descriptionTableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 400f));
			this.descriptionTableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 60f));
			this.descriptionTableLayoutPanel.Controls.Add(this.pictureDesktop, 1, 0);
			this.descriptionTableLayoutPanel.Controls.Add(this.lblSubHeader, 0, 1);
			this.descriptionTableLayoutPanel.Controls.Add(this.lblHeader, 0, 0);
			this.descriptionTableLayoutPanel.Margin = new Padding(0);
			this.descriptionTableLayoutPanel.Name = "descriptionTableLayoutPanel";
			this.descriptionTableLayoutPanel.RowStyles.Add(new RowStyle());
			this.descriptionTableLayoutPanel.RowStyles.Add(new RowStyle());
			componentResourceManager.ApplyResources(this.pictureDesktop, "pictureDesktop");
			this.pictureDesktop.Margin = new Padding(3, 0, 0, 0);
			this.pictureDesktop.Name = "pictureDesktop";
			this.descriptionTableLayoutPanel.SetRowSpan(this.pictureDesktop, 2);
			this.pictureDesktop.TabStop = false;
			componentResourceManager.ApplyResources(this.lblSubHeader, "lblSubHeader");
			this.lblSubHeader.Margin = new Padding(29, 3, 3, 8);
			this.lblSubHeader.Name = "lblSubHeader";
			componentResourceManager.ApplyResources(this.lblHeader, "lblHeader");
			this.lblHeader.Margin = new Padding(10, 11, 3, 0);
			this.lblHeader.Name = "lblHeader";
			componentResourceManager.ApplyResources(this.lblApplication, "lblApplication");
			this.lblApplication.Margin = new Padding(0, 0, 3, 3);
			this.lblApplication.Name = "lblApplication";
			componentResourceManager.ApplyResources(this.linkAppId, "linkAppId");
			this.linkAppId.AutoEllipsis = true;
			this.linkAppId.Margin = new Padding(3, 0, 0, 3);
			this.linkAppId.Name = "linkAppId";
			this.linkAppId.TabStop = true;
			this.linkAppId.UseMnemonic = false;
			this.linkAppId.LinkClicked += this.linkAppId_LinkClicked;
			componentResourceManager.ApplyResources(this.lblFrom, "lblFrom");
			this.lblFrom.Margin = new Padding(0, 3, 3, 0);
			this.lblFrom.Name = "lblFrom";
			componentResourceManager.ApplyResources(this.lblFromId, "lblFromId");
			this.lblFromId.AutoEllipsis = true;
			this.lblFromId.Margin = new Padding(3, 3, 0, 0);
			this.lblFromId.Name = "lblFromId";
			this.lblFromId.UseMnemonic = false;
			componentResourceManager.ApplyResources(this.groupRule, "groupRule");
			this.groupRule.Margin = new Padding(0, 0, 0, 3);
			this.groupRule.BackColor = SystemColors.ControlDark;
			this.groupRule.FlatStyle = FlatStyle.Flat;
			this.groupRule.Name = "groupRule";
			this.groupRule.TabStop = false;
			componentResourceManager.ApplyResources(this.groupDivider, "groupDivider");
			this.groupDivider.Margin = new Padding(0, 3, 0, 3);
			this.groupDivider.BackColor = SystemColors.ControlDark;
			this.groupDivider.FlatStyle = FlatStyle.Flat;
			this.groupDivider.Name = "groupDivider";
			this.groupDivider.TabStop = false;
			componentResourceManager.ApplyResources(this.btnOk, "btnOk");
			this.btnOk.Margin = new Padding(0, 0, 3, 0);
			this.btnOk.MinimumSize = new Size(75, 23);
			this.btnOk.Name = "btnOk";
			this.btnOk.Padding = new Padding(10, 0, 10, 0);
			this.btnOk.Click += this.btnOk_Click;
			componentResourceManager.ApplyResources(this.btnSkip, "btnSkip");
			this.btnSkip.Margin = new Padding(3, 0, 0, 0);
			this.btnSkip.MinimumSize = new Size(75, 23);
			this.btnSkip.Name = "btnSkip";
			this.btnSkip.Padding = new Padding(10, 0, 10, 0);
			this.btnSkip.Click += this.btnSkip_Click;
			componentResourceManager.ApplyResources(this.contentTableLayoutPanel, "contentTableLayoutPanel");
			this.contentTableLayoutPanel.ColumnStyles.Add(new ColumnStyle());
			this.contentTableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
			this.contentTableLayoutPanel.Controls.Add(this.lblApplication, 0, 0);
			this.contentTableLayoutPanel.Controls.Add(this.lblFrom, 0, 1);
			this.contentTableLayoutPanel.Controls.Add(this.linkAppId, 1, 0);
			this.contentTableLayoutPanel.Controls.Add(this.lblFromId, 1, 1);
			this.contentTableLayoutPanel.Margin = new Padding(20, 15, 12, 18);
			this.contentTableLayoutPanel.Name = "contentTableLayoutPanel";
			this.contentTableLayoutPanel.RowStyles.Add(new RowStyle());
			this.contentTableLayoutPanel.RowStyles.Add(new RowStyle());
			componentResourceManager.ApplyResources(this.okSkipTableLayoutPanel, "okSkipTableLayoutPanel");
			this.okSkipTableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
			this.okSkipTableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
			this.okSkipTableLayoutPanel.Controls.Add(this.btnOk, 0, 0);
			this.okSkipTableLayoutPanel.Controls.Add(this.btnSkip, 1, 0);
			this.okSkipTableLayoutPanel.Margin = new Padding(0, 7, 8, 6);
			this.okSkipTableLayoutPanel.Name = "okSkipTableLayoutPanel";
			this.okSkipTableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 50f));
			componentResourceManager.ApplyResources(this.overarchingTableLayoutPanel, "overarchingTableLayoutPanel");
			this.overarchingTableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
			this.overarchingTableLayoutPanel.Controls.Add(this.descriptionTableLayoutPanel, 0, 0);
			this.overarchingTableLayoutPanel.Controls.Add(this.okSkipTableLayoutPanel, 0, 4);
			this.overarchingTableLayoutPanel.Controls.Add(this.contentTableLayoutPanel, 0, 2);
			this.overarchingTableLayoutPanel.Controls.Add(this.groupRule, 0, 1);
			this.overarchingTableLayoutPanel.Controls.Add(this.groupDivider, 0, 3);
			this.overarchingTableLayoutPanel.Margin = new Padding(0);
			this.overarchingTableLayoutPanel.Name = "overarchingTableLayoutPanel";
			this.overarchingTableLayoutPanel.RowStyles.Add(new RowStyle());
			this.overarchingTableLayoutPanel.RowStyles.Add(new RowStyle());
			this.overarchingTableLayoutPanel.RowStyles.Add(new RowStyle());
			this.overarchingTableLayoutPanel.RowStyles.Add(new RowStyle());
			this.overarchingTableLayoutPanel.RowStyles.Add(new RowStyle());
			componentResourceManager.ApplyResources(this, "$this");
			base.Controls.Add(this.overarchingTableLayoutPanel);
			base.Name = "UpdatePiece";
			this.descriptionTableLayoutPanel.ResumeLayout(false);
			this.descriptionTableLayoutPanel.PerformLayout();
			((ISupportInitialize)this.pictureDesktop).EndInit();
			this.contentTableLayoutPanel.ResumeLayout(false);
			this.contentTableLayoutPanel.PerformLayout();
			this.okSkipTableLayoutPanel.ResumeLayout(false);
			this.okSkipTableLayoutPanel.PerformLayout();
			this.overarchingTableLayoutPanel.ResumeLayout(false);
			this.overarchingTableLayoutPanel.PerformLayout();
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		// Token: 0x0600056B RID: 1387 RVA: 0x0001D33C File Offset: 0x0001C33C
		private void InitializeContent()
		{
			this.pictureDesktop.Image = Resources.GetImage("setup.bmp");
			this.lblSubHeader.Text = string.Format(CultureInfo.CurrentUICulture, Resources.GetString("UI_UpdateSubHeader"), new object[] { UserInterface.LimitDisplayTextLength(this._info.productName) });
			this.linkAppId.Text = this._info.productName;
			this.linkAppId.Links.Clear();
			if (UserInterface.IsValidHttpUrl(this._info.supportUrl))
			{
				this.linkAppId.Links.Add(0, this._info.productName.Length, this._info.supportUrl);
			}
			this.lblFromId.Text = this._info.sourceSite;
		}

		// Token: 0x0600056C RID: 1388 RVA: 0x0001D413 File Offset: 0x0001C413
		private void linkAppId_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			this.linkAppId.LinkVisited = true;
			UserInterface.LaunchUrlInBrowser(e.Link.LinkData.ToString());
		}

		// Token: 0x0600056D RID: 1389 RVA: 0x0001D436 File Offset: 0x0001C436
		private void btnOk_Click(object sender, EventArgs e)
		{
			this._modalResult = UserInterfaceModalResult.Ok;
			this._modalEvent.Set();
			base.Enabled = false;
		}

		// Token: 0x0600056E RID: 1390 RVA: 0x0001D452 File Offset: 0x0001C452
		private void btnSkip_Click(object sender, EventArgs e)
		{
			this._modalResult = UserInterfaceModalResult.Skip;
			this._modalEvent.Set();
			base.Enabled = false;
		}

		// Token: 0x0400047A RID: 1146
		private Label lblHeader;

		// Token: 0x0400047B RID: 1147
		private Label lblSubHeader;

		// Token: 0x0400047C RID: 1148
		private PictureBox pictureDesktop;

		// Token: 0x0400047D RID: 1149
		private Label lblApplication;

		// Token: 0x0400047E RID: 1150
		private LinkLabel linkAppId;

		// Token: 0x0400047F RID: 1151
		private Label lblFrom;

		// Token: 0x04000480 RID: 1152
		private Label lblFromId;

		// Token: 0x04000481 RID: 1153
		private GroupBox groupRule;

		// Token: 0x04000482 RID: 1154
		private GroupBox groupDivider;

		// Token: 0x04000483 RID: 1155
		private Button btnOk;

		// Token: 0x04000484 RID: 1156
		private Button btnSkip;

		// Token: 0x04000485 RID: 1157
		private TableLayoutPanel contentTableLayoutPanel;

		// Token: 0x04000486 RID: 1158
		private TableLayoutPanel descriptionTableLayoutPanel;

		// Token: 0x04000487 RID: 1159
		private TableLayoutPanel okSkipTableLayoutPanel;

		// Token: 0x04000488 RID: 1160
		private TableLayoutPanel overarchingTableLayoutPanel;

		// Token: 0x04000489 RID: 1161
		private UserInterfaceInfo _info;
	}
}
