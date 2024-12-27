using System;
using System.Runtime.CompilerServices;

namespace System.Threading
{
	// Token: 0x0200015A RID: 346
	internal sealed class ThreadPoolRequestQueue
	{
		// Token: 0x06001358 RID: 4952 RVA: 0x000351A0 File Offset: 0x000341A0
		public ThreadPoolRequestQueue()
		{
			this.tpSync = new object();
		}

		// Token: 0x06001359 RID: 4953 RVA: 0x000351B4 File Offset: 0x000341B4
		public uint EnQueue(_ThreadPoolWaitCallback tpcallBack)
		{
			uint num = 0U;
			bool flag = false;
			bool flag2 = false;
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
			}
			finally
			{
				RuntimeHelpers.PrepareConstrainedRegions();
				try
				{
					Monitor.Enter(this.tpSync);
					flag = true;
				}
				catch (Exception)
				{
				}
				if (flag)
				{
					if (this.tpCount == 0U)
					{
						flag2 = ThreadPool.SetAppDomainRequestActive();
					}
					this.tpCount += 1U;
					num = this.tpCount;
					if (this.tpHead == null)
					{
						this.tpHead = tpcallBack;
						this.tpTail = tpcallBack;
					}
					else
					{
						this.tpTail._next = tpcallBack;
						this.tpTail = tpcallBack;
					}
					Monitor.Exit(this.tpSync);
					if (flag2)
					{
						ThreadPool.SetNativeTpEvent();
					}
				}
			}
			return num;
		}

		// Token: 0x0600135A RID: 4954 RVA: 0x00035268 File Offset: 0x00034268
		public _ThreadPoolWaitCallback DeQueue()
		{
			bool flag = false;
			_ThreadPoolWaitCallback threadPoolWaitCallback = null;
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
			}
			finally
			{
				RuntimeHelpers.PrepareConstrainedRegions();
				try
				{
					Monitor.Enter(this.tpSync);
					flag = true;
				}
				catch (Exception)
				{
				}
				if (flag)
				{
					_ThreadPoolWaitCallback threadPoolWaitCallback2 = this.tpHead;
					if (threadPoolWaitCallback2 != null)
					{
						threadPoolWaitCallback = threadPoolWaitCallback2;
						this.tpHead = threadPoolWaitCallback2._next;
						this.tpCount -= 1U;
						if (this.tpCount == 0U)
						{
							this.tpTail = null;
							ThreadPool.ClearAppDomainRequestActive();
						}
					}
					Monitor.Exit(this.tpSync);
				}
			}
			return threadPoolWaitCallback;
		}

		// Token: 0x0600135B RID: 4955 RVA: 0x00035300 File Offset: 0x00034300
		public uint GetQueueCount()
		{
			return this.tpCount;
		}

		// Token: 0x04000663 RID: 1635
		private _ThreadPoolWaitCallback tpHead;

		// Token: 0x04000664 RID: 1636
		private _ThreadPoolWaitCallback tpTail;

		// Token: 0x04000665 RID: 1637
		private object tpSync;

		// Token: 0x04000666 RID: 1638
		private uint tpCount;
	}
}
