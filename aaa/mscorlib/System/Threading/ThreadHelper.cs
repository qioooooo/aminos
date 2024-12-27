using System;

namespace System.Threading
{
	// Token: 0x02000154 RID: 340
	internal class ThreadHelper
	{
		// Token: 0x060012B6 RID: 4790 RVA: 0x000343EF File Offset: 0x000333EF
		internal ThreadHelper(Delegate start)
		{
			this._start = start;
		}

		// Token: 0x060012B7 RID: 4791 RVA: 0x000343FE File Offset: 0x000333FE
		internal void SetExecutionContextHelper(ExecutionContext ec)
		{
			this._executionContext = ec;
		}

		// Token: 0x060012B8 RID: 4792 RVA: 0x00034408 File Offset: 0x00033408
		internal static void ThreadStart_Context(object state)
		{
			ThreadHelper threadHelper = (ThreadHelper)state;
			if (threadHelper._start is ThreadStart)
			{
				((ThreadStart)threadHelper._start)();
				return;
			}
			((ParameterizedThreadStart)threadHelper._start)(threadHelper._startArg);
		}

		// Token: 0x060012B9 RID: 4793 RVA: 0x00034450 File Offset: 0x00033450
		internal void ThreadStart(object obj)
		{
			this._startArg = obj;
			if (this._executionContext != null)
			{
				ExecutionContext.Run(this._executionContext, ThreadHelper._ccb, this);
				return;
			}
			((ParameterizedThreadStart)this._start)(obj);
		}

		// Token: 0x060012BA RID: 4794 RVA: 0x00034484 File Offset: 0x00033484
		internal void ThreadStart()
		{
			if (this._executionContext != null)
			{
				ExecutionContext.Run(this._executionContext, ThreadHelper._ccb, this);
				return;
			}
			((ThreadStart)this._start)();
		}

		// Token: 0x04000646 RID: 1606
		private Delegate _start;

		// Token: 0x04000647 RID: 1607
		private object _startArg;

		// Token: 0x04000648 RID: 1608
		private ExecutionContext _executionContext;

		// Token: 0x04000649 RID: 1609
		internal static ContextCallback _ccb = new ContextCallback(ThreadHelper.ThreadStart_Context);
	}
}
