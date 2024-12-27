using System;

namespace System.Threading
{
	// Token: 0x0200016A RID: 362
	internal class _TimerCallback
	{
		// Token: 0x060013B3 RID: 5043 RVA: 0x00035B98 File Offset: 0x00034B98
		internal static void TimerCallback_Context(object state)
		{
			_TimerCallback timerCallback = (_TimerCallback)state;
			timerCallback._timerCallback(timerCallback._state);
		}

		// Token: 0x060013B4 RID: 5044 RVA: 0x00035BBD File Offset: 0x00034BBD
		internal _TimerCallback(TimerCallback timerCallback, object state, ref StackCrawlMark stackMark)
		{
			this._timerCallback = timerCallback;
			this._state = state;
			if (!ExecutionContext.IsFlowSuppressed())
			{
				this._executionContext = ExecutionContext.Capture(ref stackMark);
				ExecutionContext.ClearSyncContext(this._executionContext);
			}
		}

		// Token: 0x060013B5 RID: 5045 RVA: 0x00035BF4 File Offset: 0x00034BF4
		internal static void PerformTimerCallback(object state)
		{
			_TimerCallback timerCallback = (_TimerCallback)state;
			if (timerCallback._executionContext == null)
			{
				TimerCallback timerCallback2 = timerCallback._timerCallback;
				timerCallback2(timerCallback._state);
				return;
			}
			ExecutionContext.Run(timerCallback._executionContext.CreateCopy(), _TimerCallback._ccb, timerCallback);
		}

		// Token: 0x04000689 RID: 1673
		private TimerCallback _timerCallback;

		// Token: 0x0400068A RID: 1674
		private ExecutionContext _executionContext;

		// Token: 0x0400068B RID: 1675
		private object _state;

		// Token: 0x0400068C RID: 1676
		internal static ContextCallback _ccb = new ContextCallback(_TimerCallback.TimerCallback_Context);
	}
}
