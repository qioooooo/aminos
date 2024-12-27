using System;

namespace System.Windows.Forms
{
	// Token: 0x020003AA RID: 938
	public class DateBoldEventArgs : EventArgs
	{
		// Token: 0x06003923 RID: 14627 RVA: 0x000D174C File Offset: 0x000D074C
		internal DateBoldEventArgs(DateTime start, int size)
		{
			this.startDate = start;
			this.size = size;
		}

		// Token: 0x17000AA9 RID: 2729
		// (get) Token: 0x06003924 RID: 14628 RVA: 0x000D1762 File Offset: 0x000D0762
		public DateTime StartDate
		{
			get
			{
				return this.startDate;
			}
		}

		// Token: 0x17000AAA RID: 2730
		// (get) Token: 0x06003925 RID: 14629 RVA: 0x000D176A File Offset: 0x000D076A
		public int Size
		{
			get
			{
				return this.size;
			}
		}

		// Token: 0x17000AAB RID: 2731
		// (get) Token: 0x06003926 RID: 14630 RVA: 0x000D1772 File Offset: 0x000D0772
		// (set) Token: 0x06003927 RID: 14631 RVA: 0x000D177A File Offset: 0x000D077A
		public int[] DaysToBold
		{
			get
			{
				return this.daysToBold;
			}
			set
			{
				this.daysToBold = value;
			}
		}

		// Token: 0x04001CA5 RID: 7333
		private readonly DateTime startDate;

		// Token: 0x04001CA6 RID: 7334
		private readonly int size;

		// Token: 0x04001CA7 RID: 7335
		private int[] daysToBold;
	}
}
