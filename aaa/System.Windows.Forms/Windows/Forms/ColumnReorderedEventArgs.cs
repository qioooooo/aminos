using System;
using System.ComponentModel;

namespace System.Windows.Forms
{
	// Token: 0x02000289 RID: 649
	public class ColumnReorderedEventArgs : CancelEventArgs
	{
		// Token: 0x060022D6 RID: 8918 RVA: 0x0004CF24 File Offset: 0x0004BF24
		public ColumnReorderedEventArgs(int oldDisplayIndex, int newDisplayIndex, ColumnHeader header)
		{
			this.oldDisplayIndex = oldDisplayIndex;
			this.newDisplayIndex = newDisplayIndex;
			this.header = header;
		}

		// Token: 0x17000547 RID: 1351
		// (get) Token: 0x060022D7 RID: 8919 RVA: 0x0004CF41 File Offset: 0x0004BF41
		public int OldDisplayIndex
		{
			get
			{
				return this.oldDisplayIndex;
			}
		}

		// Token: 0x17000548 RID: 1352
		// (get) Token: 0x060022D8 RID: 8920 RVA: 0x0004CF49 File Offset: 0x0004BF49
		public int NewDisplayIndex
		{
			get
			{
				return this.newDisplayIndex;
			}
		}

		// Token: 0x17000549 RID: 1353
		// (get) Token: 0x060022D9 RID: 8921 RVA: 0x0004CF51 File Offset: 0x0004BF51
		public ColumnHeader Header
		{
			get
			{
				return this.header;
			}
		}

		// Token: 0x04001533 RID: 5427
		private int oldDisplayIndex;

		// Token: 0x04001534 RID: 5428
		private int newDisplayIndex;

		// Token: 0x04001535 RID: 5429
		private ColumnHeader header;
	}
}
