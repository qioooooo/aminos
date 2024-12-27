using System;

namespace System.Threading
{
	// Token: 0x02000160 RID: 352
	internal class _ThreadPoolWaitOrTimerCallback
	{
		// Token: 0x06001377 RID: 4983 RVA: 0x00035641 File Offset: 0x00034641
		internal _ThreadPoolWaitOrTimerCallback(WaitOrTimerCallback waitOrTimerCallback, object state, bool compressStack, ref StackCrawlMark stackMark)
		{
			this._waitOrTimerCallback = waitOrTimerCallback;
			this._state = state;
			if (compressStack && !ExecutionContext.IsFlowSuppressed())
			{
				this._executionContext = ExecutionContext.Capture(ref stackMark);
				ExecutionContext.ClearSyncContext(this._executionContext);
			}
		}

		// Token: 0x06001378 RID: 4984 RVA: 0x00035679 File Offset: 0x00034679
		private static void WaitOrTimerCallback_Context_t(object state)
		{
			_ThreadPoolWaitOrTimerCallback.WaitOrTimerCallback_Context(state, true);
		}

		// Token: 0x06001379 RID: 4985 RVA: 0x00035682 File Offset: 0x00034682
		private static void WaitOrTimerCallback_Context_f(object state)
		{
			_ThreadPoolWaitOrTimerCallback.WaitOrTimerCallback_Context(state, false);
		}

		// Token: 0x0600137A RID: 4986 RVA: 0x0003568C File Offset: 0x0003468C
		private static void WaitOrTimerCallback_Context(object state, bool timedOut)
		{
			_ThreadPoolWaitOrTimerCallback threadPoolWaitOrTimerCallback = (_ThreadPoolWaitOrTimerCallback)state;
			threadPoolWaitOrTimerCallback._waitOrTimerCallback(threadPoolWaitOrTimerCallback._state, timedOut);
		}

		// Token: 0x0600137B RID: 4987 RVA: 0x000356B4 File Offset: 0x000346B4
		internal static void PerformWaitOrTimerCallback(object state, bool timedOut)
		{
			_ThreadPoolWaitOrTimerCallback threadPoolWaitOrTimerCallback = (_ThreadPoolWaitOrTimerCallback)state;
			if (threadPoolWaitOrTimerCallback._executionContext == null)
			{
				WaitOrTimerCallback waitOrTimerCallback = threadPoolWaitOrTimerCallback._waitOrTimerCallback;
				waitOrTimerCallback(threadPoolWaitOrTimerCallback._state, timedOut);
				return;
			}
			if (timedOut)
			{
				ExecutionContext.Run(threadPoolWaitOrTimerCallback._executionContext.CreateCopy(), _ThreadPoolWaitOrTimerCallback._ccbt, threadPoolWaitOrTimerCallback);
				return;
			}
			ExecutionContext.Run(threadPoolWaitOrTimerCallback._executionContext.CreateCopy(), _ThreadPoolWaitOrTimerCallback._ccbf, threadPoolWaitOrTimerCallback);
		}

		// Token: 0x04000672 RID: 1650
		private WaitOrTimerCallback _waitOrTimerCallback;

		// Token: 0x04000673 RID: 1651
		private ExecutionContext _executionContext;

		// Token: 0x04000674 RID: 1652
		private object _state;

		// Token: 0x04000675 RID: 1653
		private static ContextCallback _ccbt = new ContextCallback(_ThreadPoolWaitOrTimerCallback.WaitOrTimerCallback_Context_t);

		// Token: 0x04000676 RID: 1654
		private static ContextCallback _ccbf = new ContextCallback(_ThreadPoolWaitOrTimerCallback.WaitOrTimerCallback_Context_f);
	}
}
