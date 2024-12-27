using System;
using System.Security.Permissions;
using System.Threading;

namespace System.Web.UI
{
	// Token: 0x0200043F RID: 1087
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class PageAsyncTask
	{
		// Token: 0x060033D5 RID: 13269 RVA: 0x000E1443 File Offset: 0x000E0443
		public PageAsyncTask(BeginEventHandler beginHandler, EndEventHandler endHandler, EndEventHandler timeoutHandler, object state)
			: this(beginHandler, endHandler, timeoutHandler, state, false)
		{
		}

		// Token: 0x060033D6 RID: 13270 RVA: 0x000E1454 File Offset: 0x000E0454
		public PageAsyncTask(BeginEventHandler beginHandler, EndEventHandler endHandler, EndEventHandler timeoutHandler, object state, bool executeInParallel)
		{
			if (beginHandler == null)
			{
				throw new ArgumentNullException("beginHandler");
			}
			if (endHandler == null)
			{
				throw new ArgumentNullException("endHandler");
			}
			this._beginHandler = beginHandler;
			this._endHandler = endHandler;
			this._timeoutHandler = timeoutHandler;
			this._state = state;
			this._executeInParallel = executeInParallel;
		}

		// Token: 0x17000B7E RID: 2942
		// (get) Token: 0x060033D7 RID: 13271 RVA: 0x000E14A8 File Offset: 0x000E04A8
		public BeginEventHandler BeginHandler
		{
			get
			{
				return this._beginHandler;
			}
		}

		// Token: 0x17000B7F RID: 2943
		// (get) Token: 0x060033D8 RID: 13272 RVA: 0x000E14B0 File Offset: 0x000E04B0
		public EndEventHandler EndHandler
		{
			get
			{
				return this._endHandler;
			}
		}

		// Token: 0x17000B80 RID: 2944
		// (get) Token: 0x060033D9 RID: 13273 RVA: 0x000E14B8 File Offset: 0x000E04B8
		public EndEventHandler TimeoutHandler
		{
			get
			{
				return this._timeoutHandler;
			}
		}

		// Token: 0x17000B81 RID: 2945
		// (get) Token: 0x060033DA RID: 13274 RVA: 0x000E14C0 File Offset: 0x000E04C0
		public object State
		{
			get
			{
				return this._state;
			}
		}

		// Token: 0x17000B82 RID: 2946
		// (get) Token: 0x060033DB RID: 13275 RVA: 0x000E14C8 File Offset: 0x000E04C8
		public bool ExecuteInParallel
		{
			get
			{
				return this._executeInParallel;
			}
		}

		// Token: 0x17000B83 RID: 2947
		// (get) Token: 0x060033DC RID: 13276 RVA: 0x000E14D0 File Offset: 0x000E04D0
		internal bool Started
		{
			get
			{
				return this._started;
			}
		}

		// Token: 0x17000B84 RID: 2948
		// (get) Token: 0x060033DD RID: 13277 RVA: 0x000E14D8 File Offset: 0x000E04D8
		internal bool CompletedSynchronously
		{
			get
			{
				return this._completedSynchronously;
			}
		}

		// Token: 0x17000B85 RID: 2949
		// (get) Token: 0x060033DE RID: 13278 RVA: 0x000E14E0 File Offset: 0x000E04E0
		internal bool Completed
		{
			get
			{
				return this._completed;
			}
		}

		// Token: 0x17000B86 RID: 2950
		// (get) Token: 0x060033DF RID: 13279 RVA: 0x000E14E8 File Offset: 0x000E04E8
		internal IAsyncResult AsyncResult
		{
			get
			{
				return this._asyncResult;
			}
		}

		// Token: 0x17000B87 RID: 2951
		// (get) Token: 0x060033E0 RID: 13280 RVA: 0x000E14F0 File Offset: 0x000E04F0
		internal Exception Error
		{
			get
			{
				return this._error;
			}
		}

		// Token: 0x060033E1 RID: 13281 RVA: 0x000E14F8 File Offset: 0x000E04F8
		internal void Start(PageAsyncTaskManager manager, object source, EventArgs args)
		{
			this._taskManager = manager;
			this._completionCallback = new AsyncCallback(this.OnAsyncTaskCompletion);
			this._started = true;
			try
			{
				IAsyncResult asyncResult = this._beginHandler(source, args, this._completionCallback, this._state);
				if (asyncResult == null)
				{
					throw new InvalidOperationException(SR.GetString("Async_null_asyncresult"));
				}
				if (this._asyncResult == null)
				{
					this._asyncResult = asyncResult;
				}
			}
			catch (Exception ex)
			{
				this._error = ex;
				this._completed = true;
				this._completedSynchronously = true;
				this._taskManager.TaskCompleted(true);
			}
		}

		// Token: 0x060033E2 RID: 13282 RVA: 0x000E1598 File Offset: 0x000E0598
		private void OnAsyncTaskCompletion(IAsyncResult ar)
		{
			if (this._asyncResult == null)
			{
				this._asyncResult = ar;
			}
			this.CompleteTask(false);
		}

		// Token: 0x060033E3 RID: 13283 RVA: 0x000E15B0 File Offset: 0x000E05B0
		internal void ForceTimeout(bool syncCaller)
		{
			this.CompleteTask(true, syncCaller);
		}

		// Token: 0x060033E4 RID: 13284 RVA: 0x000E15BA File Offset: 0x000E05BA
		private void CompleteTask(bool timedOut)
		{
			this.CompleteTask(timedOut, false);
		}

		// Token: 0x060033E5 RID: 13285 RVA: 0x000E15C4 File Offset: 0x000E05C4
		private void CompleteTask(bool timedOut, bool syncTimeoutCaller)
		{
			if (Interlocked.Exchange(ref this._completionMethodLock, 1) != 0)
			{
				return;
			}
			bool flag = false;
			bool flag2;
			if (timedOut)
			{
				flag2 = !syncTimeoutCaller;
			}
			else
			{
				this._completedSynchronously = this._asyncResult.CompletedSynchronously;
				flag2 = !this._completedSynchronously;
			}
			HttpApplication application = this._taskManager.Application;
			try
			{
				if (flag2)
				{
					lock (application)
					{
						HttpApplication.ThreadContext threadContext = null;
						try
						{
							threadContext = application.OnThreadEnter();
							if (timedOut)
							{
								if (this._timeoutHandler != null)
								{
									this._timeoutHandler(this._asyncResult);
								}
							}
							else
							{
								this._endHandler(this._asyncResult);
							}
						}
						finally
						{
							if (threadContext != null)
							{
								threadContext.Leave();
							}
						}
						goto IL_00CB;
					}
				}
				if (timedOut)
				{
					if (this._timeoutHandler != null)
					{
						this._timeoutHandler(this._asyncResult);
					}
				}
				else
				{
					this._endHandler(this._asyncResult);
				}
				IL_00CB:;
			}
			catch (ThreadAbortException ex)
			{
				this._error = ex;
				HttpApplication.CancelModuleException ex2 = ex.ExceptionState as HttpApplication.CancelModuleException;
				if (ex2 != null && !ex2.Timeout)
				{
					lock (application)
					{
						if (!application.IsRequestCompleted)
						{
							flag = true;
							application.CompleteRequest();
						}
					}
					this._error = null;
				}
				Thread.ResetAbort();
			}
			catch (Exception ex3)
			{
				this._error = ex3;
			}
			this._completed = true;
			this._taskManager.TaskCompleted(this._completedSynchronously);
			if (flag)
			{
				this._taskManager.CompleteAllTasksNow(false);
			}
		}

		// Token: 0x04002467 RID: 9319
		private BeginEventHandler _beginHandler;

		// Token: 0x04002468 RID: 9320
		private EndEventHandler _endHandler;

		// Token: 0x04002469 RID: 9321
		private EndEventHandler _timeoutHandler;

		// Token: 0x0400246A RID: 9322
		private object _state;

		// Token: 0x0400246B RID: 9323
		private bool _executeInParallel;

		// Token: 0x0400246C RID: 9324
		private PageAsyncTaskManager _taskManager;

		// Token: 0x0400246D RID: 9325
		private int _completionMethodLock;

		// Token: 0x0400246E RID: 9326
		private bool _started;

		// Token: 0x0400246F RID: 9327
		private bool _completed;

		// Token: 0x04002470 RID: 9328
		private bool _completedSynchronously;

		// Token: 0x04002471 RID: 9329
		private AsyncCallback _completionCallback;

		// Token: 0x04002472 RID: 9330
		private IAsyncResult _asyncResult;

		// Token: 0x04002473 RID: 9331
		private Exception _error;
	}
}
