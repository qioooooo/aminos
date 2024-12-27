using System;

namespace System.Timers
{
	// Token: 0x02000734 RID: 1844
	public class ElapsedEventArgs : EventArgs
	{
		// Token: 0x06003838 RID: 14392 RVA: 0x000ED56C File Offset: 0x000EC56C
		internal ElapsedEventArgs(int low, int high)
		{
			long num = ((long)high << 32) | ((long)low & (long)((ulong)(-1)));
			this.signalTime = DateTime.FromFileTime(num);
		}

		// Token: 0x17000D0A RID: 3338
		// (get) Token: 0x06003839 RID: 14393 RVA: 0x000ED597 File Offset: 0x000EC597
		public DateTime SignalTime
		{
			get
			{
				return this.signalTime;
			}
		}

		// Token: 0x0400322A RID: 12842
		private DateTime signalTime;
	}
}
