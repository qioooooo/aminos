using System;
using System.Threading;

namespace System.Web.SessionState
{
	// Token: 0x02000376 RID: 886
	internal class QueuedRequestCounter
	{
		// Token: 0x06002B08 RID: 11016 RVA: 0x000BED05 File Offset: 0x000BDD05
		public int Increment()
		{
			return Interlocked.Increment(ref this._count);
		}

		// Token: 0x06002B09 RID: 11017 RVA: 0x000BED12 File Offset: 0x000BDD12
		public int Decrement()
		{
			return Interlocked.Decrement(ref this._count);
		}

		// Token: 0x17000934 RID: 2356
		// (get) Token: 0x06002B0A RID: 11018 RVA: 0x000BED1F File Offset: 0x000BDD1F
		public int Count
		{
			get
			{
				return Interlocked.CompareExchange(ref this._count, 0, 0);
			}
		}

		// Token: 0x04001FBE RID: 8126
		private int _count;
	}
}
