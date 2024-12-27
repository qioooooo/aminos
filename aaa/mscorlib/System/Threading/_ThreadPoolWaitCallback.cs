using System;

namespace System.Threading
{
	// Token: 0x0200015F RID: 351
	internal class _ThreadPoolWaitCallback
	{
		// Token: 0x06001372 RID: 4978 RVA: 0x0003553C File Offset: 0x0003453C
		internal static void WaitCallback_Context(object state)
		{
			_ThreadPoolWaitCallback threadPoolWaitCallback = (_ThreadPoolWaitCallback)state;
			threadPoolWaitCallback._waitCallback(threadPoolWaitCallback._state);
		}

		// Token: 0x06001373 RID: 4979 RVA: 0x00035561 File Offset: 0x00034561
		internal _ThreadPoolWaitCallback(WaitCallback waitCallback, object state, bool compressStack, ref StackCrawlMark stackMark)
		{
			this._waitCallback = waitCallback;
			this._state = state;
			if (compressStack && !ExecutionContext.IsFlowSuppressed())
			{
				this._executionContext = ExecutionContext.Capture(ref stackMark);
				ExecutionContext.ClearSyncContext(this._executionContext);
			}
		}

		// Token: 0x06001374 RID: 4980 RVA: 0x0003559C File Offset: 0x0003459C
		internal static void PerformWaitCallback(object state)
		{
			int tickCount = Environment.TickCount;
			for (;;)
			{
				_ThreadPoolWaitCallback threadPoolWaitCallback = ThreadPoolGlobals.tpQueue.DeQueue();
				if (threadPoolWaitCallback == null)
				{
					break;
				}
				ThreadPool.CompleteThreadPoolRequest(ThreadPoolGlobals.tpQueue.GetQueueCount());
				_ThreadPoolWaitCallback.PerformWaitCallbackInternal(threadPoolWaitCallback);
				int tickCount2 = Environment.TickCount;
				int num = tickCount2 - tickCount;
				if ((long)num > (long)((ulong)ThreadPoolGlobals.tpQuantum) && ThreadPool.ShouldReturnToVm())
				{
					return;
				}
			}
		}

		// Token: 0x06001375 RID: 4981 RVA: 0x000355F4 File Offset: 0x000345F4
		internal static void PerformWaitCallbackInternal(_ThreadPoolWaitCallback tpWaitCallBack)
		{
			if (tpWaitCallBack._executionContext == null)
			{
				WaitCallback waitCallback = tpWaitCallBack._waitCallback;
				waitCallback(tpWaitCallBack._state);
				return;
			}
			ExecutionContext.Run(tpWaitCallBack._executionContext, _ThreadPoolWaitCallback._ccb, tpWaitCallBack);
		}

		// Token: 0x0400066D RID: 1645
		private WaitCallback _waitCallback;

		// Token: 0x0400066E RID: 1646
		private ExecutionContext _executionContext;

		// Token: 0x0400066F RID: 1647
		private object _state;

		// Token: 0x04000670 RID: 1648
		protected internal _ThreadPoolWaitCallback _next;

		// Token: 0x04000671 RID: 1649
		internal static ContextCallback _ccb = new ContextCallback(_ThreadPoolWaitCallback.WaitCallback_Context);
	}
}
