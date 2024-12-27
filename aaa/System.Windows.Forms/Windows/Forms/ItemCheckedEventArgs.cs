using System;

namespace System.Windows.Forms
{
	// Token: 0x02000454 RID: 1108
	public class ItemCheckedEventArgs : EventArgs
	{
		// Token: 0x060041A6 RID: 16806 RVA: 0x000EB1A8 File Offset: 0x000EA1A8
		public ItemCheckedEventArgs(ListViewItem item)
		{
			this.lvi = item;
		}

		// Token: 0x17000CBC RID: 3260
		// (get) Token: 0x060041A7 RID: 16807 RVA: 0x000EB1B7 File Offset: 0x000EA1B7
		public ListViewItem Item
		{
			get
			{
				return this.lvi;
			}
		}

		// Token: 0x04001FA8 RID: 8104
		private ListViewItem lvi;
	}
}
