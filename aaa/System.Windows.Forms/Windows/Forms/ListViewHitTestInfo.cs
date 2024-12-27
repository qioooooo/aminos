using System;

namespace System.Windows.Forms
{
	// Token: 0x0200048F RID: 1167
	public class ListViewHitTestInfo
	{
		// Token: 0x060045BD RID: 17853 RVA: 0x000FD69A File Offset: 0x000FC69A
		public ListViewHitTestInfo(ListViewItem hitItem, ListViewItem.ListViewSubItem hitSubItem, ListViewHitTestLocations hitLocation)
		{
			this.item = hitItem;
			this.subItem = hitSubItem;
			this.loc = hitLocation;
		}

		// Token: 0x17000DC3 RID: 3523
		// (get) Token: 0x060045BE RID: 17854 RVA: 0x000FD6B7 File Offset: 0x000FC6B7
		public ListViewHitTestLocations Location
		{
			get
			{
				return this.loc;
			}
		}

		// Token: 0x17000DC4 RID: 3524
		// (get) Token: 0x060045BF RID: 17855 RVA: 0x000FD6BF File Offset: 0x000FC6BF
		public ListViewItem Item
		{
			get
			{
				return this.item;
			}
		}

		// Token: 0x17000DC5 RID: 3525
		// (get) Token: 0x060045C0 RID: 17856 RVA: 0x000FD6C7 File Offset: 0x000FC6C7
		public ListViewItem.ListViewSubItem SubItem
		{
			get
			{
				return this.subItem;
			}
		}

		// Token: 0x0400216A RID: 8554
		private ListViewHitTestLocations loc;

		// Token: 0x0400216B RID: 8555
		private ListViewItem item;

		// Token: 0x0400216C RID: 8556
		private ListViewItem.ListViewSubItem subItem;
	}
}
