using System;
using System.Collections;
using System.Threading;
using System.Web.Util;

namespace System.Web
{
	// Token: 0x020000C4 RID: 196
	internal class RequestTimeoutManager
	{
		// Token: 0x060008E5 RID: 2277 RVA: 0x000287B4 File Offset: 0x000277B4
		internal RequestTimeoutManager()
		{
			this._requestCount = 0;
			this._lists = new DoubleLinkList[13];
			for (int i = 0; i < this._lists.Length; i++)
			{
				this._lists[i] = new DoubleLinkList();
			}
			this._currentList = 0;
			this._inProgressLock = 0;
			this._timer = new Timer(new TimerCallback(this.TimerCompletionCallback), null, this._timerPeriod, this._timerPeriod);
		}

		// Token: 0x060008E6 RID: 2278 RVA: 0x0002883C File Offset: 0x0002783C
		internal void Stop()
		{
			if (this._timer != null)
			{
				((IDisposable)this._timer).Dispose();
				this._timer = null;
			}
			while (this._inProgressLock != 0)
			{
				Thread.Sleep(100);
			}
			if (this._requestCount > 0)
			{
				this.CancelTimedOutRequests(DateTime.UtcNow.AddYears(1));
			}
		}

		// Token: 0x060008E7 RID: 2279 RVA: 0x00028891 File Offset: 0x00027891
		private void TimerCompletionCallback(object state)
		{
			if (this._requestCount > 0)
			{
				this.CancelTimedOutRequests(DateTime.UtcNow);
			}
		}

		// Token: 0x060008E8 RID: 2280 RVA: 0x000288A8 File Offset: 0x000278A8
		private void CancelTimedOutRequests(DateTime now)
		{
			if (Interlocked.CompareExchange(ref this._inProgressLock, 1, 0) != 0)
			{
				return;
			}
			ArrayList arrayList = new ArrayList(this._requestCount);
			for (int i = 0; i < this._lists.Length; i++)
			{
				lock (this._lists[i])
				{
					DoubleLinkListEnumerator enumerator = this._lists[i].GetEnumerator();
					while (enumerator.MoveNext())
					{
						arrayList.Add(enumerator.GetDoubleLink());
					}
				}
			}
			int count = arrayList.Count;
			for (int j = 0; j < count; j++)
			{
				((RequestTimeoutManager.RequestTimeoutEntry)arrayList[j]).TimeoutIfNeeded(now);
			}
			Interlocked.Exchange(ref this._inProgressLock, 0);
		}

		// Token: 0x060008E9 RID: 2281 RVA: 0x0002896C File Offset: 0x0002796C
		internal void Add(HttpContext context)
		{
			if (context.TimeoutLink != null)
			{
				((RequestTimeoutManager.RequestTimeoutEntry)context.TimeoutLink).IncrementCount();
				return;
			}
			RequestTimeoutManager.RequestTimeoutEntry requestTimeoutEntry = new RequestTimeoutManager.RequestTimeoutEntry(context);
			int num = this._currentList++;
			if (num >= this._lists.Length)
			{
				num = 0;
				this._currentList = 0;
			}
			requestTimeoutEntry.AddToList(this._lists[num]);
			Interlocked.Increment(ref this._requestCount);
			context.TimeoutLink = requestTimeoutEntry;
		}

		// Token: 0x060008EA RID: 2282 RVA: 0x000289E0 File Offset: 0x000279E0
		internal void Remove(HttpContext context)
		{
			RequestTimeoutManager.RequestTimeoutEntry requestTimeoutEntry = (RequestTimeoutManager.RequestTimeoutEntry)context.TimeoutLink;
			if (requestTimeoutEntry != null)
			{
				if (requestTimeoutEntry.DecrementCount() != 0)
				{
					return;
				}
				requestTimeoutEntry.RemoveFromList();
				Interlocked.Decrement(ref this._requestCount);
			}
			context.TimeoutLink = null;
		}

		// Token: 0x04001220 RID: 4640
		private int _requestCount;

		// Token: 0x04001221 RID: 4641
		private DoubleLinkList[] _lists;

		// Token: 0x04001222 RID: 4642
		private int _currentList;

		// Token: 0x04001223 RID: 4643
		private int _inProgressLock;

		// Token: 0x04001224 RID: 4644
		private readonly TimeSpan _timerPeriod = new TimeSpan(0, 0, 15);

		// Token: 0x04001225 RID: 4645
		private Timer _timer;

		// Token: 0x020000C6 RID: 198
		private class RequestTimeoutEntry : DoubleLink
		{
			// Token: 0x060008F1 RID: 2289 RVA: 0x00028AEB File Offset: 0x00027AEB
			internal RequestTimeoutEntry(HttpContext context)
			{
				this._context = context;
				this._count = 1;
			}

			// Token: 0x060008F2 RID: 2290 RVA: 0x00028B04 File Offset: 0x00027B04
			internal void AddToList(DoubleLinkList list)
			{
				lock (list)
				{
					list.InsertTail(this);
					this._list = list;
				}
			}

			// Token: 0x060008F3 RID: 2291 RVA: 0x00028B40 File Offset: 0x00027B40
			internal void RemoveFromList()
			{
				if (this._list != null)
				{
					lock (this._list)
					{
						base.Remove();
						this._list = null;
					}
				}
			}

			// Token: 0x060008F4 RID: 2292 RVA: 0x00028B88 File Offset: 0x00027B88
			internal void TimeoutIfNeeded(DateTime now)
			{
				Thread thread = this._context.MustTimeout(now);
				if (thread != null)
				{
					this.RemoveFromList();
					thread.Abort(new HttpApplication.CancelModuleException(true));
				}
			}

			// Token: 0x060008F5 RID: 2293 RVA: 0x00028BB7 File Offset: 0x00027BB7
			internal void IncrementCount()
			{
				Interlocked.Increment(ref this._count);
			}

			// Token: 0x060008F6 RID: 2294 RVA: 0x00028BC5 File Offset: 0x00027BC5
			internal int DecrementCount()
			{
				return Interlocked.Decrement(ref this._count);
			}

			// Token: 0x04001229 RID: 4649
			private HttpContext _context;

			// Token: 0x0400122A RID: 4650
			private DoubleLinkList _list;

			// Token: 0x0400122B RID: 4651
			private int _count;
		}
	}
}
