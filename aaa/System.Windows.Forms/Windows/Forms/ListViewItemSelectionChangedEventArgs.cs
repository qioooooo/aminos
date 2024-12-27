using System;

namespace System.Windows.Forms
{
	// Token: 0x02000499 RID: 1177
	public class ListViewItemSelectionChangedEventArgs : EventArgs
	{
		// Token: 0x06004672 RID: 18034 RVA: 0x000FFD36 File Offset: 0x000FED36
		public ListViewItemSelectionChangedEventArgs(ListViewItem item, int itemIndex, bool isSelected)
		{
			this.item = item;
			this.itemIndex = itemIndex;
			this.isSelected = isSelected;
		}

		// Token: 0x17000DFB RID: 3579
		// (get) Token: 0x06004673 RID: 18035 RVA: 0x000FFD53 File Offset: 0x000FED53
		public bool IsSelected
		{
			get
			{
				return this.isSelected;
			}
		}

		// Token: 0x17000DFC RID: 3580
		// (get) Token: 0x06004674 RID: 18036 RVA: 0x000FFD5B File Offset: 0x000FED5B
		public ListViewItem Item
		{
			get
			{
				return this.item;
			}
		}

		// Token: 0x17000DFD RID: 3581
		// (get) Token: 0x06004675 RID: 18037 RVA: 0x000FFD63 File Offset: 0x000FED63
		public int ItemIndex
		{
			get
			{
				return this.itemIndex;
			}
		}

		// Token: 0x04002199 RID: 8601
		private ListViewItem item;

		// Token: 0x0400219A RID: 8602
		private int itemIndex;

		// Token: 0x0400219B RID: 8603
		private bool isSelected;
	}
}
