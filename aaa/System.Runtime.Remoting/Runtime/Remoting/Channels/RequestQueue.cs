using System;
using System.Collections;
using System.Threading;

namespace System.Runtime.Remoting.Channels
{
	// Token: 0x02000013 RID: 19
	internal class RequestQueue
	{
		// Token: 0x06000071 RID: 113 RVA: 0x00003DAC File Offset: 0x00002DAC
		private static bool IsLocal(SocketHandler sh)
		{
			return sh.IsLocal();
		}

		// Token: 0x06000072 RID: 114 RVA: 0x00003DB4 File Offset: 0x00002DB4
		private void QueueRequest(SocketHandler sh, bool isLocal)
		{
			lock (this)
			{
				if (isLocal)
				{
					this._localQueue.Enqueue(sh);
				}
				else
				{
					this._externQueue.Enqueue(sh);
				}
				this._count++;
			}
		}

		// Token: 0x06000073 RID: 115 RVA: 0x00003E10 File Offset: 0x00002E10
		private SocketHandler DequeueRequest(bool localOnly)
		{
			object obj = null;
			if (this._count > 0)
			{
				lock (this)
				{
					if (this._localQueue.Count > 0)
					{
						obj = this._localQueue.Dequeue();
						this._count--;
					}
					else if (!localOnly && this._externQueue.Count > 0)
					{
						obj = this._externQueue.Dequeue();
						this._count--;
					}
				}
			}
			return (SocketHandler)obj;
		}

		// Token: 0x06000074 RID: 116 RVA: 0x00003EA4 File Offset: 0x00002EA4
		internal RequestQueue(int minExternFreeThreads, int minLocalFreeThreads, int queueLimit)
		{
			this._minExternFreeThreads = minExternFreeThreads;
			this._minLocalFreeThreads = minLocalFreeThreads;
			this._queueLimit = queueLimit;
			this._workItemCallback = new WaitCallback(this.WorkItemCallback);
		}

		// Token: 0x06000075 RID: 117 RVA: 0x00003EF4 File Offset: 0x00002EF4
		internal void ProcessNextRequest(SocketHandler sh)
		{
			sh = this.GetRequestToExecute(sh);
			if (sh != null)
			{
				sh.ProcessRequestNow();
			}
		}

		// Token: 0x06000076 RID: 118 RVA: 0x00003F08 File Offset: 0x00002F08
		internal SocketHandler GetRequestToExecute(SocketHandler sh)
		{
			int num;
			int num2;
			ThreadPool.GetAvailableThreads(out num, out num2);
			int num3 = ((num2 > num) ? num : num2);
			if (num3 >= this._minExternFreeThreads && this._count == 0)
			{
				return sh;
			}
			bool flag = RequestQueue.IsLocal(sh);
			if (flag && num3 >= this._minLocalFreeThreads && this._count == 0)
			{
				return sh;
			}
			if (this._count >= this._queueLimit)
			{
				sh.RejectRequestNowSinceServerIsBusy();
				return null;
			}
			this.QueueRequest(sh, flag);
			if (num3 >= this._minExternFreeThreads)
			{
				sh = this.DequeueRequest(false);
			}
			else if (num3 >= this._minLocalFreeThreads)
			{
				sh = this.DequeueRequest(true);
			}
			else
			{
				sh = null;
			}
			if (sh == null)
			{
				this.ScheduleMoreWorkIfNeeded();
			}
			return sh;
		}

		// Token: 0x06000077 RID: 119 RVA: 0x00003FAA File Offset: 0x00002FAA
		internal void ScheduleMoreWorkIfNeeded()
		{
			if (this._count == 0)
			{
				return;
			}
			if (this._workItemCount >= 2)
			{
				return;
			}
			Interlocked.Increment(ref this._workItemCount);
			ThreadPool.UnsafeQueueUserWorkItem(this._workItemCallback, null);
		}

		// Token: 0x06000078 RID: 120 RVA: 0x00003FD8 File Offset: 0x00002FD8
		private void WorkItemCallback(object state)
		{
			Interlocked.Decrement(ref this._workItemCount);
			if (this._count == 0)
			{
				return;
			}
			int num;
			int num2;
			ThreadPool.GetAvailableThreads(out num, out num2);
			bool flag = false;
			if (num >= this._minLocalFreeThreads)
			{
				SocketHandler socketHandler = this.DequeueRequest(num < this._minExternFreeThreads);
				if (socketHandler != null)
				{
					socketHandler.ProcessRequestNow();
					flag = true;
				}
			}
			if (!flag)
			{
				Thread.Sleep(250);
				this.ScheduleMoreWorkIfNeeded();
			}
		}

		// Token: 0x0400007B RID: 123
		private const int _workItemLimit = 2;

		// Token: 0x0400007C RID: 124
		private int _minExternFreeThreads;

		// Token: 0x0400007D RID: 125
		private int _minLocalFreeThreads;

		// Token: 0x0400007E RID: 126
		private int _queueLimit;

		// Token: 0x0400007F RID: 127
		private Queue _localQueue = new Queue();

		// Token: 0x04000080 RID: 128
		private Queue _externQueue = new Queue();

		// Token: 0x04000081 RID: 129
		private int _count;

		// Token: 0x04000082 RID: 130
		private WaitCallback _workItemCallback;

		// Token: 0x04000083 RID: 131
		private int _workItemCount;
	}
}
