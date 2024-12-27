using System;
using System.ComponentModel;
using System.Security.Permissions;

namespace System.Windows.Forms
{
	// Token: 0x020004B0 RID: 1200
	[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	internal sealed partial class MdiWindowDialog : Form
	{
		// Token: 0x060047FF RID: 18431 RVA: 0x00105A4C File Offset: 0x00104A4C
		public MdiWindowDialog()
		{
			this.InitializeComponent();
		}

		// Token: 0x17000E5B RID: 3675
		// (get) Token: 0x06004800 RID: 18432 RVA: 0x00105A5A File Offset: 0x00104A5A
		public Form ActiveChildForm
		{
			get
			{
				return this.active;
			}
		}

		// Token: 0x06004801 RID: 18433 RVA: 0x00105A64 File Offset: 0x00104A64
		public void SetItems(Form active, Form[] all)
		{
			int num = 0;
			for (int i = 0; i < all.Length; i++)
			{
				if (all[i].Visible)
				{
					int num2 = this.itemList.Items.Add(new MdiWindowDialog.ListItem(all[i]));
					if (all[i].Equals(active))
					{
						num = num2;
					}
				}
			}
			this.active = active;
			this.itemList.SelectedIndex = num;
		}

		// Token: 0x06004802 RID: 18434 RVA: 0x00105AC4 File Offset: 0x00104AC4
		private void ItemList_doubleClick(object source, EventArgs e)
		{
			this.okButton.PerformClick();
		}

		// Token: 0x06004803 RID: 18435 RVA: 0x00105AD4 File Offset: 0x00104AD4
		private void ItemList_selectedIndexChanged(object source, EventArgs e)
		{
			MdiWindowDialog.ListItem listItem = (MdiWindowDialog.ListItem)this.itemList.SelectedItem;
			if (listItem != null)
			{
				this.active = listItem.form;
			}
		}

		// Token: 0x04002207 RID: 8711
		private Form active;

		// Token: 0x020004B1 RID: 1201
		private class ListItem
		{
			// Token: 0x06004805 RID: 18437 RVA: 0x00105D83 File Offset: 0x00104D83
			public ListItem(Form f)
			{
				this.form = f;
			}

			// Token: 0x06004806 RID: 18438 RVA: 0x00105D92 File Offset: 0x00104D92
			public override string ToString()
			{
				return this.form.Text;
			}

			// Token: 0x04002208 RID: 8712
			public Form form;
		}
	}
}
