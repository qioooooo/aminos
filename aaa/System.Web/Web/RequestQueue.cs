using System;
using System.Collections;
using System.Threading;
using System.Web.Hosting;

namespace System.Web
{
	// Token: 0x020000C3 RID: 195
	internal class RequestQueue
	{
		// Token: 0x060008DA RID: 2266 RVA: 0x0002836C File Offset: 0x0002736C
		private static bool IsLocal(HttpWorkerRequest wr)
		{
			string remoteAddress = wr.GetRemoteAddress();
			return remoteAddress == "127.0.0.1" || remoteAddress == "::1" || (!string.IsNullOrEmpty(remoteAddress) && remoteAddress == wr.GetLocalAddress());
		}

		// Token: 0x060008DB RID: 2267 RVA: 0x000283B8 File Offset: 0x000273B8
		private void QueueRequest(HttpWorkerRequest wr, bool isLocal)
		{
			lock (this)
			{
				if (isLocal)
				{
					this._localQueue.Enqueue(wr);
				}
				else
				{
					this._externQueue.Enqueue(wr);
				}
				this._count++;
			}
			PerfCounters.IncrementGlobalCounter(GlobalPerfCounter.REQUESTS_QUEUED);
			PerfCounters.IncrementCounter(AppPerfCounter.REQUESTS_IN_APPLICATION_QUEUE);
			if (EtwTrace.IsTraceEnabled(4, 1))
			{
				EtwTrace.Trace(EtwTraceType.ETW_TYPE_REQ_QUEUED, wr);
			}
		}

		// Token: 0x060008DC RID: 2268 RVA: 0x00028430 File Offset: 0x00027430
		private HttpWorkerRequest DequeueRequest(bool localOnly)
		{
			HttpWorkerRequest httpWorkerRequest = null;
			while (this._count > 0)
			{
				lock (this)
				{
					if (this._localQueue.Count > 0)
					{
						httpWorkerRequest = (HttpWorkerRequest)this._localQueue.Dequeue();
						this._count--;
					}
					else if (!localOnly && this._externQueue.Count > 0)
					{
						httpWorkerRequest = (HttpWorkerRequest)this._externQueue.Dequeue();
						this._count--;
					}
				}
				if (httpWorkerRequest == null)
				{
					break;
				}
				PerfCounters.DecrementGlobalCounter(GlobalPerfCounter.REQUESTS_QUEUED);
				PerfCounters.DecrementCounter(AppPerfCounter.REQUESTS_IN_APPLICATION_QUEUE);
				if (EtwTrace.IsTraceEnabled(4, 1))
				{
					EtwTrace.Trace(EtwTraceType.ETW_TYPE_REQ_DEQUEUED, httpWorkerRequest);
				}
				if (this.CheckClientConnected(httpWorkerRequest))
				{
					break;
				}
				HttpRuntime.RejectRequestNow(httpWorkerRequest, true);
				httpWorkerRequest = null;
				PerfCounters.IncrementGlobalCounter(GlobalPerfCounter.REQUESTS_DISCONNECTED);
				PerfCounters.IncrementCounter(AppPerfCounter.APP_REQUEST_DISCONNECTED);
			}
			return httpWorkerRequest;
		}

		// Token: 0x060008DD RID: 2269 RVA: 0x00028510 File Offset: 0x00027510
		private bool CheckClientConnected(HttpWorkerRequest wr)
		{
			return !(DateTime.UtcNow - wr.GetStartTime() > this._clientConnectedTime) || wr.IsClientConnected();
		}

		// Token: 0x060008DE RID: 2270 RVA: 0x00028538 File Offset: 0x00027538
		internal RequestQueue(int minExternFreeThreads, int minLocalFreeThreads, int queueLimit, TimeSpan clientConnectedTime)
		{
			this._minExternFreeThreads = minExternFreeThreads;
			this._minLocalFreeThreads = minLocalFreeThreads;
			this._queueLimit = queueLimit;
			this._clientConnectedTime = clientConnectedTime;
			this._workItemCallback = new WaitCallback(this.WorkItemCallback);
			this._timer = new Timer(new TimerCallback(this.TimerCompletionCallback), null, this._timerPeriod, this._timerPeriod);
			this._iis6 = HostingEnvironment.IsUnderIIS6Process;
			int num;
			int num2;
			ThreadPool.GetMaxThreads(out num, out num2);
			UnsafeNativeMethods.SetMinRequestsExecutingToDetectDeadlock(num - minExternFreeThreads);
		}

		// Token: 0x060008DF RID: 2271 RVA: 0x000285E0 File Offset: 0x000275E0
		internal HttpWorkerRequest GetRequestToExecute(HttpWorkerRequest wr)
		{
			int num;
			int num2;
			ThreadPool.GetAvailableThreads(out num, out num2);
			int num3;
			if (this._iis6)
			{
				num3 = num;
			}
			else
			{
				num3 = ((num2 > num) ? num : num2);
			}
			if (num3 >= this._minExternFreeThreads && this._count == 0)
			{
				return wr;
			}
			bool flag = RequestQueue.IsLocal(wr);
			if (flag && num3 >= this._minLocalFreeThreads && this._count == 0)
			{
				return wr;
			}
			if (this._count >= this._queueLimit)
			{
				HttpRuntime.RejectRequestNow(wr, false);
				return null;
			}
			this.QueueRequest(wr, flag);
			if (num3 >= this._minExternFreeThreads)
			{
				wr = this.DequeueRequest(false);
			}
			else if (num3 >= this._minLocalFreeThreads)
			{
				wr = this.DequeueRequest(true);
			}
			else
			{
				wr = null;
				this.ScheduleMoreWorkIfNeeded();
			}
			return wr;
		}

		// Token: 0x060008E0 RID: 2272 RVA: 0x0002868C File Offset: 0x0002768C
		internal void ScheduleMoreWorkIfNeeded()
		{
			if (this._draining)
			{
				return;
			}
			if (this._count == 0)
			{
				return;
			}
			if (this._workItemCount >= 2)
			{
				return;
			}
			int num;
			int num2;
			ThreadPool.GetAvailableThreads(out num, out num2);
			if (num < this._minLocalFreeThreads)
			{
				return;
			}
			Interlocked.Increment(ref this._workItemCount);
			ThreadPool.QueueUserWorkItem(this._workItemCallback);
		}

		// Token: 0x170002D1 RID: 721
		// (get) Token: 0x060008E1 RID: 2273 RVA: 0x000286E0 File Offset: 0x000276E0
		internal bool IsEmpty
		{
			get
			{
				return this._count == 0;
			}
		}

		// Token: 0x060008E2 RID: 2274 RVA: 0x000286EC File Offset: 0x000276EC
		private void WorkItemCallback(object state)
		{
			Interlocked.Decrement(ref this._workItemCount);
			if (this._draining)
			{
				return;
			}
			if (this._count == 0)
			{
				return;
			}
			int num;
			int num2;
			ThreadPool.GetAvailableThreads(out num, out num2);
			if (num < this._minLocalFreeThreads)
			{
				return;
			}
			HttpWorkerRequest httpWorkerRequest = this.DequeueRequest(num < this._minExternFreeThreads);
			if (httpWorkerRequest == null)
			{
				return;
			}
			this.ScheduleMoreWorkIfNeeded();
			HttpRuntime.ProcessRequestNow(httpWorkerRequest);
		}

		// Token: 0x060008E3 RID: 2275 RVA: 0x0002874A File Offset: 0x0002774A
		private void TimerCompletionCallback(object state)
		{
			this.ScheduleMoreWorkIfNeeded();
		}

		// Token: 0x060008E4 RID: 2276 RVA: 0x00028754 File Offset: 0x00027754
		internal void Drain()
		{
			this._draining = true;
			if (this._timer != null)
			{
				((IDisposable)this._timer).Dispose();
				this._timer = null;
			}
			while (this._workItemCount > 0)
			{
				Thread.Sleep(100);
			}
			if (this._count == 0)
			{
				return;
			}
			for (;;)
			{
				HttpWorkerRequest httpWorkerRequest = this.DequeueRequest(false);
				if (httpWorkerRequest == null)
				{
					break;
				}
				HttpRuntime.RejectRequestNow(httpWorkerRequest, false);
			}
		}

		// Token: 0x04001212 RID: 4626
		private const int _workItemLimit = 2;

		// Token: 0x04001213 RID: 4627
		private int _minExternFreeThreads;

		// Token: 0x04001214 RID: 4628
		private int _minLocalFreeThreads;

		// Token: 0x04001215 RID: 4629
		private int _queueLimit;

		// Token: 0x04001216 RID: 4630
		private TimeSpan _clientConnectedTime;

		// Token: 0x04001217 RID: 4631
		private bool _iis6;

		// Token: 0x04001218 RID: 4632
		private Queue _localQueue = new Queue();

		// Token: 0x04001219 RID: 4633
		private Queue _externQueue = new Queue();

		// Token: 0x0400121A RID: 4634
		private int _count;

		// Token: 0x0400121B RID: 4635
		private WaitCallback _workItemCallback;

		// Token: 0x0400121C RID: 4636
		private int _workItemCount;

		// Token: 0x0400121D RID: 4637
		private bool _draining;

		// Token: 0x0400121E RID: 4638
		private readonly TimeSpan _timerPeriod = new TimeSpan(0, 0, 10);

		// Token: 0x0400121F RID: 4639
		private Timer _timer;
	}
}
