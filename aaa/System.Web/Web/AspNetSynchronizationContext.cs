using System;
using System.Threading;

namespace System.Web
{
	// Token: 0x0200000E RID: 14
	internal class AspNetSynchronizationContext : SynchronizationContext
	{
		// Token: 0x06000013 RID: 19 RVA: 0x000023CA File Offset: 0x000013CA
		internal AspNetSynchronizationContext(HttpApplication app)
		{
			this._application = app;
		}

		// Token: 0x06000014 RID: 20 RVA: 0x000023DC File Offset: 0x000013DC
		private void CallCallback(SendOrPostCallback callback, object state)
		{
			if (this._syncCaller)
			{
				this.CallCallbackPossiblyUnderLock(callback, state);
				return;
			}
			lock (this._application)
			{
				this.CallCallbackPossiblyUnderLock(callback, state);
			}
		}

		// Token: 0x06000015 RID: 21 RVA: 0x00002428 File Offset: 0x00001428
		private void CallCallbackPossiblyUnderLock(SendOrPostCallback callback, object state)
		{
			HttpApplication.ThreadContext threadContext = null;
			try
			{
				threadContext = this._application.OnThreadEnter();
				try
				{
					callback(state);
				}
				catch (Exception ex)
				{
					this._error = ex;
				}
			}
			finally
			{
				if (threadContext != null)
				{
					threadContext.Leave();
				}
			}
		}

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x06000016 RID: 22 RVA: 0x00002480 File Offset: 0x00001480
		internal int PendingOperationsCount
		{
			get
			{
				return this._pendingCount;
			}
		}

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x06000017 RID: 23 RVA: 0x00002488 File Offset: 0x00001488
		internal Exception Error
		{
			get
			{
				return this._error;
			}
		}

		// Token: 0x06000018 RID: 24 RVA: 0x00002490 File Offset: 0x00001490
		internal void ClearError()
		{
			this._error = null;
		}

		// Token: 0x06000019 RID: 25 RVA: 0x00002499 File Offset: 0x00001499
		internal void SetLastCompletionWorkItem(WaitCallback callback)
		{
			this._lastCompletionWorkItemCallback = callback;
		}

		// Token: 0x0600001A RID: 26 RVA: 0x000024A2 File Offset: 0x000014A2
		public override void Send(SendOrPostCallback callback, object state)
		{
			this.CallCallback(callback, state);
		}

		// Token: 0x0600001B RID: 27 RVA: 0x000024AC File Offset: 0x000014AC
		public override void Post(SendOrPostCallback callback, object state)
		{
			this.CallCallback(callback, state);
		}

		// Token: 0x0600001C RID: 28 RVA: 0x000024B8 File Offset: 0x000014B8
		public override SynchronizationContext CreateCopy()
		{
			return new AspNetSynchronizationContext(this._application)
			{
				_disabled = this._disabled,
				_syncCaller = this._syncCaller
			};
		}

		// Token: 0x0600001D RID: 29 RVA: 0x000024EA File Offset: 0x000014EA
		public override void OperationStarted()
		{
			if (this._invalidOperationEncountered || (this._disabled && this._pendingCount == 0))
			{
				this._invalidOperationEncountered = true;
				throw new InvalidOperationException(SR.GetString("Async_operation_disabled"));
			}
			Interlocked.Increment(ref this._pendingCount);
		}

		// Token: 0x0600001E RID: 30 RVA: 0x00002528 File Offset: 0x00001528
		public override void OperationCompleted()
		{
			if (this._invalidOperationEncountered || (this._disabled && this._pendingCount == 0))
			{
				return;
			}
			bool flag = Interlocked.Decrement(ref this._pendingCount) == 0;
			if (flag && this._lastCompletionWorkItemCallback != null)
			{
				WaitCallback lastCompletionWorkItemCallback = this._lastCompletionWorkItemCallback;
				this._lastCompletionWorkItemCallback = null;
				ThreadPool.QueueUserWorkItem(lastCompletionWorkItemCallback);
			}
		}

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x0600001F RID: 31 RVA: 0x0000257D File Offset: 0x0000157D
		internal bool Enabled
		{
			get
			{
				return !this._disabled;
			}
		}

		// Token: 0x06000020 RID: 32 RVA: 0x00002588 File Offset: 0x00001588
		internal void Enable()
		{
			this._disabled = false;
		}

		// Token: 0x06000021 RID: 33 RVA: 0x00002591 File Offset: 0x00001591
		internal void Disable()
		{
			this._disabled = true;
		}

		// Token: 0x06000022 RID: 34 RVA: 0x0000259A File Offset: 0x0000159A
		internal void SetSyncCaller()
		{
			this._syncCaller = true;
		}

		// Token: 0x06000023 RID: 35 RVA: 0x000025A3 File Offset: 0x000015A3
		internal void ResetSyncCaller()
		{
			this._syncCaller = false;
		}

		// Token: 0x04000CF3 RID: 3315
		private HttpApplication _application;

		// Token: 0x04000CF4 RID: 3316
		private bool _disabled;

		// Token: 0x04000CF5 RID: 3317
		private bool _syncCaller;

		// Token: 0x04000CF6 RID: 3318
		private bool _invalidOperationEncountered;

		// Token: 0x04000CF7 RID: 3319
		private int _pendingCount;

		// Token: 0x04000CF8 RID: 3320
		private Exception _error;

		// Token: 0x04000CF9 RID: 3321
		private WaitCallback _lastCompletionWorkItemCallback;
	}
}
