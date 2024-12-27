using System;
using System.Globalization;
using System.Security;

namespace System.Windows.Forms
{
	// Token: 0x020004AE RID: 1198
	internal class MdiWindowListStrip : MenuStrip
	{
		// Token: 0x060047F8 RID: 18424 RVA: 0x0010568D File Offset: 0x0010468D
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				this.mdiParent = null;
			}
			base.Dispose(disposing);
		}

		// Token: 0x17000E59 RID: 3673
		// (get) Token: 0x060047F9 RID: 18425 RVA: 0x001056A0 File Offset: 0x001046A0
		internal ToolStripMenuItem MergeItem
		{
			get
			{
				if (this.mergeItem == null)
				{
					this.mergeItem = new ToolStripMenuItem();
					this.mergeItem.MergeAction = MergeAction.MatchOnly;
				}
				if (this.mergeItem.Owner == null)
				{
					this.Items.Add(this.mergeItem);
				}
				return this.mergeItem;
			}
		}

		// Token: 0x17000E5A RID: 3674
		// (get) Token: 0x060047FA RID: 18426 RVA: 0x001056F1 File Offset: 0x001046F1
		// (set) Token: 0x060047FB RID: 18427 RVA: 0x001056F9 File Offset: 0x001046F9
		internal MenuStrip MergedMenu
		{
			get
			{
				return this.mergedMenu;
			}
			set
			{
				this.mergedMenu = value;
			}
		}

		// Token: 0x060047FC RID: 18428 RVA: 0x00105704 File Offset: 0x00104704
		public void PopulateItems(Form mdiParent, ToolStripMenuItem mdiMergeItem, bool includeSeparator)
		{
			this.mdiParent = mdiParent;
			base.SuspendLayout();
			this.MergeItem.DropDown.SuspendLayout();
			try
			{
				ToolStripMenuItem toolStripMenuItem = this.MergeItem;
				toolStripMenuItem.DropDownItems.Clear();
				toolStripMenuItem.Text = mdiMergeItem.Text;
				Form[] mdiChildren = mdiParent.MdiChildren;
				if (mdiChildren != null && mdiChildren.Length != 0)
				{
					if (includeSeparator)
					{
						ToolStripSeparator toolStripSeparator = new ToolStripSeparator();
						toolStripSeparator.MergeAction = MergeAction.Append;
						toolStripSeparator.MergeIndex = -1;
						toolStripMenuItem.DropDownItems.Add(toolStripSeparator);
					}
					Form activeMdiChild = mdiParent.ActiveMdiChild;
					int num = 0;
					int num2 = 1;
					int num3 = 0;
					bool flag = false;
					for (int i = 0; i < mdiChildren.Length; i++)
					{
						if (mdiChildren[i].Visible && mdiChildren[i].CloseReason == CloseReason.None)
						{
							num++;
							if ((flag && num3 < 9) || (!flag && num3 < 8) || mdiChildren[i].Equals(activeMdiChild))
							{
								string text = WindowsFormsUtils.EscapeTextWithAmpersands(mdiParent.MdiChildren[i].Text);
								text = ((text == null) ? string.Empty : text);
								ToolStripMenuItem toolStripMenuItem2 = new ToolStripMenuItem(mdiParent.MdiChildren[i]);
								toolStripMenuItem2.Text = string.Format(CultureInfo.CurrentCulture, "&{0} {1}", new object[] { num2, text });
								toolStripMenuItem2.MergeAction = MergeAction.Append;
								toolStripMenuItem2.MergeIndex = num2;
								toolStripMenuItem2.Click += this.OnWindowListItemClick;
								if (mdiChildren[i].Equals(activeMdiChild))
								{
									toolStripMenuItem2.Checked = true;
									flag = true;
								}
								num2++;
								num3++;
								toolStripMenuItem.DropDownItems.Add(toolStripMenuItem2);
							}
						}
					}
					if (num > 9)
					{
						ToolStripMenuItem toolStripMenuItem3 = new ToolStripMenuItem();
						toolStripMenuItem3.Text = SR.GetString("MDIMenuMoreWindows");
						toolStripMenuItem3.Click += this.OnMoreWindowsMenuItemClick;
						toolStripMenuItem3.MergeAction = MergeAction.Append;
						toolStripMenuItem.DropDownItems.Add(toolStripMenuItem3);
					}
				}
			}
			finally
			{
				base.ResumeLayout(false);
				this.MergeItem.DropDown.ResumeLayout(false);
			}
		}

		// Token: 0x060047FD RID: 18429 RVA: 0x0010592C File Offset: 0x0010492C
		private void OnMoreWindowsMenuItemClick(object sender, EventArgs e)
		{
			Form[] mdiChildren = this.mdiParent.MdiChildren;
			if (mdiChildren != null)
			{
				IntSecurity.AllWindows.Assert();
				try
				{
					using (MdiWindowDialog mdiWindowDialog = new MdiWindowDialog())
					{
						mdiWindowDialog.SetItems(this.mdiParent.ActiveMdiChild, mdiChildren);
						DialogResult dialogResult = mdiWindowDialog.ShowDialog();
						if (dialogResult == DialogResult.OK)
						{
							mdiWindowDialog.ActiveChildForm.Activate();
							if (mdiWindowDialog.ActiveChildForm.ActiveControl != null && !mdiWindowDialog.ActiveChildForm.ActiveControl.Focused)
							{
								mdiWindowDialog.ActiveChildForm.ActiveControl.Focus();
							}
						}
					}
				}
				finally
				{
					CodeAccessPermission.RevertAssert();
				}
			}
		}

		// Token: 0x060047FE RID: 18430 RVA: 0x001059E0 File Offset: 0x001049E0
		private void OnWindowListItemClick(object sender, EventArgs e)
		{
			ToolStripMenuItem toolStripMenuItem = sender as ToolStripMenuItem;
			if (toolStripMenuItem != null)
			{
				Form mdiForm = toolStripMenuItem.MdiForm;
				if (mdiForm != null)
				{
					IntSecurity.ModifyFocus.Assert();
					try
					{
						mdiForm.Activate();
						if (mdiForm.ActiveControl != null && !mdiForm.ActiveControl.Focused)
						{
							mdiForm.ActiveControl.Focus();
						}
					}
					finally
					{
						CodeAccessPermission.RevertAssert();
					}
				}
			}
		}

		// Token: 0x040021FB RID: 8699
		private Form mdiParent;

		// Token: 0x040021FC RID: 8700
		private ToolStripMenuItem mergeItem;

		// Token: 0x040021FD RID: 8701
		private MenuStrip mergedMenu;
	}
}
