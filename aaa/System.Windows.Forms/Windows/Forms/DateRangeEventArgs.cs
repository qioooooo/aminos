using System;

namespace System.Windows.Forms
{
	// Token: 0x020003AC RID: 940
	public class DateRangeEventArgs : EventArgs
	{
		// Token: 0x0600392C RID: 14636 RVA: 0x000D1783 File Offset: 0x000D0783
		public DateRangeEventArgs(DateTime start, DateTime end)
		{
			this.start = start;
			this.end = end;
		}

		// Token: 0x17000AAC RID: 2732
		// (get) Token: 0x0600392D RID: 14637 RVA: 0x000D1799 File Offset: 0x000D0799
		public DateTime Start
		{
			get
			{
				return this.start;
			}
		}

		// Token: 0x17000AAD RID: 2733
		// (get) Token: 0x0600392E RID: 14638 RVA: 0x000D17A1 File Offset: 0x000D07A1
		public DateTime End
		{
			get
			{
				return this.end;
			}
		}

		// Token: 0x04001CA8 RID: 7336
		private readonly DateTime start;

		// Token: 0x04001CA9 RID: 7337
		private readonly DateTime end;
	}
}
