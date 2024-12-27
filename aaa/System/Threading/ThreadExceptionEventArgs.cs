using System;

namespace System.Threading
{
	// Token: 0x02000227 RID: 551
	public class ThreadExceptionEventArgs : EventArgs
	{
		// Token: 0x06001283 RID: 4739 RVA: 0x0003E543 File Offset: 0x0003D543
		public ThreadExceptionEventArgs(Exception t)
		{
			this.exception = t;
		}

		// Token: 0x170003B2 RID: 946
		// (get) Token: 0x06001284 RID: 4740 RVA: 0x0003E552 File Offset: 0x0003D552
		public Exception Exception
		{
			get
			{
				return this.exception;
			}
		}

		// Token: 0x040010C4 RID: 4292
		private Exception exception;
	}
}
