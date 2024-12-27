using System;

namespace System.Windows.Forms
{
	// Token: 0x02000378 RID: 888
	internal class DataGridViewIntLinkedListElement
	{
		// Token: 0x06003662 RID: 13922 RVA: 0x000C2584 File Offset: 0x000C1584
		public DataGridViewIntLinkedListElement(int integer)
		{
			this.integer = integer;
		}

		// Token: 0x170009EE RID: 2542
		// (get) Token: 0x06003663 RID: 13923 RVA: 0x000C2593 File Offset: 0x000C1593
		// (set) Token: 0x06003664 RID: 13924 RVA: 0x000C259B File Offset: 0x000C159B
		public int Int
		{
			get
			{
				return this.integer;
			}
			set
			{
				this.integer = value;
			}
		}

		// Token: 0x170009EF RID: 2543
		// (get) Token: 0x06003665 RID: 13925 RVA: 0x000C25A4 File Offset: 0x000C15A4
		// (set) Token: 0x06003666 RID: 13926 RVA: 0x000C25AC File Offset: 0x000C15AC
		public DataGridViewIntLinkedListElement Next
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

		// Token: 0x04001BD7 RID: 7127
		private int integer;

		// Token: 0x04001BD8 RID: 7128
		private DataGridViewIntLinkedListElement next;
	}
}
