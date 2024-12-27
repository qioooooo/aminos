using System;

namespace System.Windows.Forms
{
	// Token: 0x02000319 RID: 793
	internal class DataGridViewCellLinkedListElement
	{
		// Token: 0x06003353 RID: 13139 RVA: 0x000B40EE File Offset: 0x000B30EE
		public DataGridViewCellLinkedListElement(DataGridViewCell dataGridViewCell)
		{
			this.dataGridViewCell = dataGridViewCell;
		}

		// Token: 0x1700090D RID: 2317
		// (get) Token: 0x06003354 RID: 13140 RVA: 0x000B40FD File Offset: 0x000B30FD
		public DataGridViewCell DataGridViewCell
		{
			get
			{
				return this.dataGridViewCell;
			}
		}

		// Token: 0x1700090E RID: 2318
		// (get) Token: 0x06003355 RID: 13141 RVA: 0x000B4105 File Offset: 0x000B3105
		// (set) Token: 0x06003356 RID: 13142 RVA: 0x000B410D File Offset: 0x000B310D
		public DataGridViewCellLinkedListElement Next
		{
			get
			{
				return this.next;
			}
			set
			{
				this.next = value;
			}
		}

		// Token: 0x04001AB6 RID: 6838
		private DataGridViewCell dataGridViewCell;

		// Token: 0x04001AB7 RID: 6839
		private DataGridViewCellLinkedListElement next;
	}
}
