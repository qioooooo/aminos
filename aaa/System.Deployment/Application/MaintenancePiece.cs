using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace System.Deployment.Application
{
	// Token: 0x02000074 RID: 116
	internal class MaintenancePiece : ModalPiece
	{
		// Token: 0x0600038B RID: 907 RVA: 0x000140D8 File Offset: 0x000130D8
		public MaintenancePiece(UserInterfaceForm parentForm, UserInterfaceInfo info, MaintenanceInfo maintenanceInfo, ManualResetEvent modalEvent)
		{
			this._modalResult = UserInterfaceModalResult.Cancel;
			this._info = info;
			this._maintenanceInfo = maintenanceInfo;
			this._modalEvent = modalEvent;
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
			parentForm.ActiveControl = this.btnCancel;
			parentForm.ResumeLayout(false);
			parentForm.PerformLayout();
			parentForm.Visible = true;
		}

		// Token: 0x0600038C RID: 908 RVA: 0x000141A4 File Offset: 0x000131A4
		private void InitializeComponent()
		{
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(MaintenancePiece));
			this.lblHeader = new Label();
			this.lblSubHeader = new Label();
			this.pictureRestore = new PictureBox();
			this.pictureRemove = new PictureBox();
			this.radioRestore = new RadioButton();
			this.radioRemove = new RadioButton();
			this.groupRule = new GroupBox();
			this.groupDivider = new GroupBox();
			this.btnOk = new Button();
			this.btnCancel = new Button();
			this.btnHelp = new Button();
			this.topTableLayoutPanel = new TableLayoutPanel();
			this.pictureDesktop = new PictureBox();
			this.okCancelHelpTableLayoutPanel = new TableLayoutPanel();
			this.contentTableLayoutPanel = new TableLayoutPanel();
			this.overarchingTableLayoutPanel = new TableLayoutPanel();
			((ISupportInitialize)this.pictureRestore).BeginInit();
			((ISupportInitialize)this.pictureRemove).BeginInit();
			this.topTableLayoutPanel.SuspendLayout();
			((ISupportInitialize)this.pictureDesktop).BeginInit();
			this.okCancelHelpTableLayoutPanel.SuspendLayout();
			this.contentTableLayoutPanel.SuspendLayout();
			this.overarchingTableLayoutPanel.SuspendLayout();
			base.SuspendLayout();
			this.lblHeader.AutoEllipsis = true;
			componentResourceManager.ApplyResources(this.lblHeader, "lblHeader");
			this.lblHeader.Margin = new Padding(10, 11, 3, 0);
			this.lblHeader.Name = "lblHeader";
			this.lblHeader.UseMnemonic = false;
			componentResourceManager.ApplyResources(this.lblSubHeader, "lblSubHeader");
			this.lblSubHeader.Margin = new Padding(29, 3, 3, 8);
			this.lblSubHeader.Name = "lblSubHeader";
			componentResourceManager.ApplyResources(this.pictureRestore, "pictureRestore");
			this.pictureRestore.Margin = new Padding(0, 0, 3, 0);
			this.pictureRestore.Name = "pictureRestore";
			this.pictureRestore.TabStop = false;
			componentResourceManager.ApplyResources(this.pictureRemove, "pictureRemove");
			this.pictureRemove.Margin = new Padding(0, 0, 3, 0);
			this.pictureRemove.Name = "pictureRemove";
			this.pictureRemove.TabStop = false;
			componentResourceManager.ApplyResources(this.radioRestore, "radioRestore");
			this.radioRestore.Margin = new Padding(3, 0, 0, 0);
			this.radioRestore.Name = "radioRestore";
			this.radioRestore.CheckedChanged += this.radioRestore_CheckedChanged;
			componentResourceManager.ApplyResources(this.radioRemove, "radioRemove");
			this.radioRemove.Margin = new Padding(3, 0, 0, 0);
			this.radioRemove.Name = "radioRemove";
			this.radioRemove.CheckedChanged += this.radioRemove_CheckedChanged;
			componentResourceManager.ApplyResources(this.groupRule, "groupRule");
			this.groupRule.Margin = new Padding(0);
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
			this.btnOk.Margin = new Padding(0, 0, 4, 0);
			this.btnOk.MinimumSize = new Size(75, 23);
			this.btnOk.Name = "btnOk";
			this.btnOk.Padding = new Padding(10, 0, 10, 0);
			this.btnOk.Click += this.btnOk_Click;
			componentResourceManager.ApplyResources(this.btnCancel, "btnCancel");
			this.btnCancel.Margin = new Padding(2, 0, 2, 0);
			this.btnCancel.MinimumSize = new Size(75, 23);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Padding = new Padding(10, 0, 10, 0);
			this.btnCancel.Click += this.btnCancel_Click;
			componentResourceManager.ApplyResources(this.btnHelp, "btnHelp");
			this.btnHelp.Margin = new Padding(4, 0, 0, 0);
			this.btnHelp.MinimumSize = new Size(75, 23);
			this.btnHelp.Name = "btnHelp";
			this.btnHelp.Padding = new Padding(10, 0, 10, 0);
			this.btnHelp.Click += this.btnHelp_Click;
			componentResourceManager.ApplyResources(this.topTableLayoutPanel, "topTableLayoutPanel");
			this.topTableLayoutPanel.BackColor = SystemColors.Window;
			this.topTableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 87.2f));
			this.topTableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 12.8f));
			this.topTableLayoutPanel.Controls.Add(this.pictureDesktop, 1, 0);
			this.topTableLayoutPanel.Controls.Add(this.lblHeader, 0, 0);
			this.topTableLayoutPanel.Controls.Add(this.lblSubHeader, 0, 1);
			this.topTableLayoutPanel.Margin = new Padding(0);
			this.topTableLayoutPanel.Name = "topTableLayoutPanel";
			this.topTableLayoutPanel.RowStyles.Add(new RowStyle());
			this.topTableLayoutPanel.RowStyles.Add(new RowStyle());
			componentResourceManager.ApplyResources(this.pictureDesktop, "pictureDesktop");
			this.pictureDesktop.Margin = new Padding(3, 0, 0, 0);
			this.pictureDesktop.Name = "pictureDesktop";
			this.topTableLayoutPanel.SetRowSpan(this.pictureDesktop, 2);
			this.pictureDesktop.TabStop = false;
			componentResourceManager.ApplyResources(this.okCancelHelpTableLayoutPanel, "okCancelHelpTableLayoutPanel");
			this.okCancelHelpTableLayoutPanel.ColumnStyles.Add(new ColumnStyle());
			this.okCancelHelpTableLayoutPanel.ColumnStyles.Add(new ColumnStyle());
			this.okCancelHelpTableLayoutPanel.ColumnStyles.Add(new ColumnStyle());
			this.okCancelHelpTableLayoutPanel.Controls.Add(this.btnOk, 0, 0);
			this.okCancelHelpTableLayoutPanel.Controls.Add(this.btnCancel, 1, 0);
			this.okCancelHelpTableLayoutPanel.Controls.Add(this.btnHelp, 2, 0);
			this.okCancelHelpTableLayoutPanel.Margin = new Padding(0, 9, 8, 8);
			this.okCancelHelpTableLayoutPanel.Name = "okCancelHelpTableLayoutPanel";
			this.okCancelHelpTableLayoutPanel.RowStyles.Add(new RowStyle());
			componentResourceManager.ApplyResources(this.contentTableLayoutPanel, "contentTableLayoutPanel");
			this.contentTableLayoutPanel.ColumnStyles.Add(new ColumnStyle());
			this.contentTableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
			this.contentTableLayoutPanel.Controls.Add(this.pictureRestore, 0, 0);
			this.contentTableLayoutPanel.Controls.Add(this.pictureRemove, 0, 1);
			this.contentTableLayoutPanel.Controls.Add(this.radioRemove, 1, 1);
			this.contentTableLayoutPanel.Controls.Add(this.radioRestore, 1, 0);
			this.contentTableLayoutPanel.Margin = new Padding(20, 22, 12, 22);
			this.contentTableLayoutPanel.Name = "contentTableLayoutPanel";
			this.contentTableLayoutPanel.RowStyles.Add(new RowStyle());
			this.contentTableLayoutPanel.RowStyles.Add(new RowStyle());
			componentResourceManager.ApplyResources(this.overarchingTableLayoutPanel, "overarchingTableLayoutPanel");
			this.overarchingTableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
			this.overarchingTableLayoutPanel.Controls.Add(this.topTableLayoutPanel, 0, 0);
			this.overarchingTableLayoutPanel.Controls.Add(this.okCancelHelpTableLayoutPanel, 0, 4);
			this.overarchingTableLayoutPanel.Controls.Add(this.contentTableLayoutPanel, 0, 2);
			this.overarchingTableLayoutPanel.Controls.Add(this.groupDivider, 0, 3);
			this.overarchingTableLayoutPanel.Controls.Add(this.groupRule, 0, 1);
			this.overarchingTableLayoutPanel.Margin = new Padding(0);
			this.overarchingTableLayoutPanel.Name = "overarchingTableLayoutPanel";
			this.overarchingTableLayoutPanel.RowStyles.Add(new RowStyle());
			this.overarchingTableLayoutPanel.RowStyles.Add(new RowStyle());
			this.overarchingTableLayoutPanel.RowStyles.Add(new RowStyle());
			this.overarchingTableLayoutPanel.RowStyles.Add(new RowStyle());
			this.overarchingTableLayoutPanel.RowStyles.Add(new RowStyle());
			componentResourceManager.ApplyResources(this, "$this");
			base.Controls.Add(this.overarchingTableLayoutPanel);
			base.Name = "MaintenancePiece";
			((ISupportInitialize)this.pictureRestore).EndInit();
			((ISupportInitialize)this.pictureRemove).EndInit();
			this.topTableLayoutPanel.ResumeLayout(false);
			this.topTableLayoutPanel.PerformLayout();
			((ISupportInitialize)this.pictureDesktop).EndInit();
			this.okCancelHelpTableLayoutPanel.ResumeLayout(false);
			this.okCancelHelpTableLayoutPanel.PerformLayout();
			this.contentTableLayoutPanel.ResumeLayout(false);
			this.contentTableLayoutPanel.PerformLayout();
			this.overarchingTableLayoutPanel.ResumeLayout(false);
			this.overarchingTableLayoutPanel.PerformLayout();
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		// Token: 0x0600038D RID: 909 RVA: 0x00014B70 File Offset: 0x00013B70
		private void InitializeContent()
		{
			this.pictureDesktop.Image = Resources.GetImage("setup.bmp");
			this.pictureRestore.Enabled = (this._maintenanceInfo.maintenanceFlags & MaintenanceFlags.RestorationPossible) != MaintenanceFlags.ClearFlag;
			Bitmap bitmap = (Bitmap)Resources.GetImage("restore.bmp");
			bitmap.MakeTransparent();
			this.pictureRestore.Image = bitmap;
			Bitmap bitmap2 = (Bitmap)Resources.GetImage("remove.bmp");
			bitmap2.MakeTransparent();
			this.pictureRemove.Image = bitmap2;
			this.lblHeader.Text = this._info.productName;
			this.radioRestore.Checked = (this._maintenanceInfo.maintenanceFlags & MaintenanceFlags.RestorationPossible) != MaintenanceFlags.ClearFlag;
			this.radioRestore.Enabled = (this._maintenanceInfo.maintenanceFlags & MaintenanceFlags.RestorationPossible) != MaintenanceFlags.ClearFlag;
			this.radioRemove.Checked = (this._maintenanceInfo.maintenanceFlags & MaintenanceFlags.RestorationPossible) == MaintenanceFlags.ClearFlag;
			this.btnHelp.Enabled = UserInterface.IsValidHttpUrl(this._info.supportUrl);
		}

		// Token: 0x0600038E RID: 910 RVA: 0x00014C7C File Offset: 0x00013C7C
		private void btnOk_Click(object sender, EventArgs e)
		{
			this._modalResult = UserInterfaceModalResult.Ok;
			this._modalEvent.Set();
			base.Enabled = false;
		}

		// Token: 0x0600038F RID: 911 RVA: 0x00014C98 File Offset: 0x00013C98
		private void btnCancel_Click(object sender, EventArgs e)
		{
			this._modalResult = UserInterfaceModalResult.Cancel;
			this._modalEvent.Set();
			base.Enabled = false;
		}

		// Token: 0x06000390 RID: 912 RVA: 0x00014CB4 File Offset: 0x00013CB4
		private void btnHelp_Click(object sender, EventArgs e)
		{
			if (UserInterface.IsValidHttpUrl(this._info.supportUrl))
			{
				UserInterface.LaunchUrlInBrowser(this._info.supportUrl);
			}
		}

		// Token: 0x06000391 RID: 913 RVA: 0x00014CD8 File Offset: 0x00013CD8
		private void radioRestore_CheckedChanged(object sender, EventArgs e)
		{
			if (this.radioRestore.Checked)
			{
				this._maintenanceInfo.maintenanceFlags |= MaintenanceFlags.RestoreSelected;
				return;
			}
			this._maintenanceInfo.maintenanceFlags &= ~MaintenanceFlags.RestoreSelected;
		}

		// Token: 0x06000392 RID: 914 RVA: 0x00014D0F File Offset: 0x00013D0F
		private void radioRemove_CheckedChanged(object sender, EventArgs e)
		{
			if (this.radioRemove.Checked)
			{
				this._maintenanceInfo.maintenanceFlags |= MaintenanceFlags.RemoveSelected;
				return;
			}
			this._maintenanceInfo.maintenanceFlags &= ~MaintenanceFlags.RemoveSelected;
		}

		// Token: 0x04000287 RID: 647
		private Label lblHeader;

		// Token: 0x04000288 RID: 648
		private Label lblSubHeader;

		// Token: 0x04000289 RID: 649
		private PictureBox pictureDesktop;

		// Token: 0x0400028A RID: 650
		private PictureBox pictureRestore;

		// Token: 0x0400028B RID: 651
		private PictureBox pictureRemove;

		// Token: 0x0400028C RID: 652
		private RadioButton radioRestore;

		// Token: 0x0400028D RID: 653
		private RadioButton radioRemove;

		// Token: 0x0400028E RID: 654
		private GroupBox groupRule;

		// Token: 0x0400028F RID: 655
		private GroupBox groupDivider;

		// Token: 0x04000290 RID: 656
		private Button btnOk;

		// Token: 0x04000291 RID: 657
		private Button btnCancel;

		// Token: 0x04000292 RID: 658
		private Button btnHelp;

		// Token: 0x04000293 RID: 659
		private TableLayoutPanel okCancelHelpTableLayoutPanel;

		// Token: 0x04000294 RID: 660
		private TableLayoutPanel contentTableLayoutPanel;

		// Token: 0x04000295 RID: 661
		private TableLayoutPanel topTableLayoutPanel;

		// Token: 0x04000296 RID: 662
		private TableLayoutPanel overarchingTableLayoutPanel;

		// Token: 0x04000297 RID: 663
		private UserInterfaceInfo _info;

		// Token: 0x04000298 RID: 664
		private MaintenanceInfo _maintenanceInfo;
	}
}
