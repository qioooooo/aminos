using System;
using System.Collections;
using System.Threading;

namespace System.Web.UI
{
	// Token: 0x02000440 RID: 1088
	internal class PageAsyncTaskManager
	{
		// Token: 0x060033E6 RID: 13286 RVA: 0x000E1768 File Offset: 0x000E0768
		internal PageAsyncTaskManager(Page page)
		{
			this._page = page;
			this._app = page.Context.ApplicationInstance;
			this._tasks = new ArrayList();
			this._resumeTasksCallback = new WaitCallback(this.ResumeTasksThreadpoolThread);
		}

		// Token: 0x17000B88 RID: 2952
		// (get) Token: 0x060033E7 RID: 13287 RVA: 0x000E17A5 File Offset: 0x000E07A5
		internal HttpApplication Application
		{
			get
			{
				return this._app;
			}
		}

		// Token: 0x060033E8 RID: 13288 RVA: 0x000E17AD File Offset: 0x000E07AD
		internal void AddTask(PageAsyncTask task)
		{
			this._tasks.Add(task);
		}

		// Token: 0x17000B89 RID: 2953
		// (get) Token: 0x060033E9 RID: 13289 RVA: 0x000E17BC File Offset: 0x000E07BC
		internal bool AnyTasksRemain
		{
			get
			{
				for (int i = 0; i < this._tasks.Count; i++)
				{
					PageAsyncTask pageAsyncTask = (PageAsyncTask)this._tasks[i];
					if (!pageAsyncTask.Started)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x17000B8A RID: 2954
		// (get) Token: 0x060033EA RID: 13290 RVA: 0x000E17FC File Offset: 0x000E07FC
		internal bool FailedToStartTasks
		{
			get
			{
				return this._failedToStart;
			}
		}

		// Token: 0x17000B8B RID: 2955
		// (get) Token: 0x060033EB RID: 13291 RVA: 0x000E1804 File Offset: 0x000E0804
		internal bool TaskExecutionInProgress
		{
			get
			{
				return this._inProgress;
			}
		}

		// Token: 0x17000B8C RID: 2956
		// (get) Token: 0x060033EC RID: 13292 RVA: 0x000E1810 File Offset: 0x000E0810
		private Exception AnyTaskError
		{
			get
			{
				for (int i = 0; i < this._tasks.Count; i++)
				{
					PageAsyncTask pageAsyncTask = (PageAsyncTask)this._tasks[i];
					if (pageAsyncTask.Error != null)
					{
						return pageAsyncTask.Error;
					}
				}
				return null;
			}
		}

		// Token: 0x17000B8D RID: 2957
		// (get) Token: 0x060033ED RID: 13293 RVA: 0x000E1855 File Offset: 0x000E0855
		private bool TimeoutEndReached
		{
			get
			{
				if (!this._timeoutEndReached && DateTime.UtcNow >= this._timeoutEnd)
				{
					this._timeoutEndReached = true;
				}
				return this._timeoutEndReached;
			}
		}

		// Token: 0x060033EE RID: 13294 RVA: 0x000E1884 File Offset: 0x000E0884
		private void WaitForAllStartedTasks(bool syncCaller, bool forceTimeout)
		{
			for (int i = 0; i < this._tasks.Count; i++)
			{
				PageAsyncTask pageAsyncTask = (PageAsyncTask)this._tasks[i];
				if (pageAsyncTask.Started && !pageAsyncTask.Completed)
				{
					if (!forceTimeout && !this.TimeoutEndReached)
					{
						DateTime utcNow = DateTime.UtcNow;
						if (utcNow < this._timeoutEnd)
						{
							WaitHandle asyncWaitHandle = pageAsyncTask.AsyncResult.AsyncWaitHandle;
							if (asyncWaitHandle != null)
							{
								bool flag = asyncWaitHandle.WaitOne(this._timeoutEnd - utcNow, false);
								if (flag && pageAsyncTask.Completed)
								{
									goto IL_00AA;
								}
							}
						}
					}
					bool flag2 = false;
					while (!pageAsyncTask.Completed)
					{
						if (forceTimeout || (!flag2 && this.TimeoutEndReached))
						{
							pageAsyncTask.ForceTimeout(syncCaller);
							flag2 = true;
						}
						else
						{
							Thread.Sleep(50);
						}
					}
				}
				IL_00AA:;
			}
		}

		// Token: 0x060033EF RID: 13295 RVA: 0x000E1950 File Offset: 0x000E0950
		internal void RegisterHandlersForPagePreRenderCompleteAsync()
		{
			this._page.AddOnPreRenderCompleteAsync(new BeginEventHandler(this.BeginExecuteAsyncTasks), new EndEventHandler(this.EndExecuteAsyncTasks));
		}

		// Token: 0x060033F0 RID: 13296 RVA: 0x000E1975 File Offset: 0x000E0975
		private IAsyncResult BeginExecuteAsyncTasks(object sender, EventArgs e, AsyncCallback cb, object extraData)
		{
			return this.ExecuteTasks(cb, extraData);
		}

		// Token: 0x060033F1 RID: 13297 RVA: 0x000E1980 File Offset: 0x000E0980
		private void EndExecuteAsyncTasks(IAsyncResult ar)
		{
			this._asyncResult.End();
		}

		// Token: 0x060033F2 RID: 13298 RVA: 0x000E1990 File Offset: 0x000E0990
		internal HttpAsyncResult ExecuteTasks(AsyncCallback callback, object extraData)
		{
			this._failedToStart = false;
			this._timeoutEnd = DateTime.UtcNow + this._page.AsyncTimeout;
			this._timeoutEndReached = false;
			this._tasksStarted = 0;
			this._tasksCompleted = 0;
			this._asyncResult = new HttpAsyncResult(callback, extraData);
			bool flag = callback == null;
			if (flag)
			{
				try
				{
				}
				finally
				{
					try
					{
						Monitor.Exit(this._app);
						Monitor.Enter(this._app);
					}
					catch (SynchronizationLockException)
					{
						this._failedToStart = true;
						throw new InvalidOperationException(SR.GetString("Async_tasks_wrong_thread"));
					}
				}
			}
			this._inProgress = true;
			try
			{
				this.ResumeTasks(flag, true);
			}
			finally
			{
				if (flag)
				{
					this._inProgress = false;
				}
			}
			return this._asyncResult;
		}

		// Token: 0x060033F3 RID: 13299 RVA: 0x000E1A6C File Offset: 0x000E0A6C
		private void ResumeTasks(bool waitUntilDone, bool onCallerThread)
		{
			Interlocked.Increment(ref this._tasksStarted);
			try
			{
				if (onCallerThread)
				{
					this.ResumeTasksPossiblyUnderLock(waitUntilDone);
				}
				else
				{
					lock (this._app)
					{
						HttpApplication.ThreadContext threadContext = null;
						try
						{
							threadContext = this._app.OnThreadEnter();
							this.ResumeTasksPossiblyUnderLock(waitUntilDone);
						}
						finally
						{
							if (threadContext != null)
							{
								threadContext.Leave();
							}
						}
					}
				}
			}
			finally
			{
				this.TaskCompleted(onCallerThread);
			}
		}

		// Token: 0x060033F4 RID: 13300 RVA: 0x000E1AFC File Offset: 0x000E0AFC
		private void ResumeTasksPossiblyUnderLock(bool waitUntilDone)
		{
			while (this.AnyTasksRemain)
			{
				bool flag = false;
				bool flag2 = false;
				bool flag3 = false;
				for (int i = 0; i < this._tasks.Count; i++)
				{
					PageAsyncTask pageAsyncTask = (PageAsyncTask)this._tasks[i];
					if (!pageAsyncTask.Started && (!flag3 || pageAsyncTask.ExecuteInParallel))
					{
						flag = true;
						Interlocked.Increment(ref this._tasksStarted);
						pageAsyncTask.Start(this, this._page, EventArgs.Empty);
						if (!pageAsyncTask.CompletedSynchronously)
						{
							flag2 = true;
							if (!pageAsyncTask.ExecuteInParallel)
							{
								break;
							}
							flag3 = true;
						}
					}
				}
				if (!flag)
				{
					return;
				}
				if (!this.TimeoutEndReached && flag2 && !waitUntilDone)
				{
					this.StartTimerIfNeeeded();
					return;
				}
				bool flag4 = true;
				try
				{
					try
					{
					}
					finally
					{
						Monitor.Exit(this._app);
						flag4 = false;
					}
					this.WaitForAllStartedTasks(true, false);
				}
				finally
				{
					if (!flag4)
					{
						Monitor.Enter(this._app);
					}
				}
			}
		}

		// Token: 0x060033F5 RID: 13301 RVA: 0x000E1BF8 File Offset: 0x000E0BF8
		private void ResumeTasksThreadpoolThread(object data)
		{
			this.ResumeTasks(false, false);
		}

		// Token: 0x060033F6 RID: 13302 RVA: 0x000E1C04 File Offset: 0x000E0C04
		internal void TaskCompleted(bool onCallerThread)
		{
			int num = Interlocked.Increment(ref this._tasksCompleted);
			if (num < this._tasksStarted)
			{
				return;
			}
			if (!this.AnyTasksRemain)
			{
				this._inProgress = false;
				this._asyncResult.Complete(onCallerThread, null, this.AnyTaskError);
				return;
			}
			if (Thread.CurrentThread.IsThreadPoolThread)
			{
				this.ResumeTasks(false, onCallerThread);
				return;
			}
			ThreadPool.QueueUserWorkItem(this._resumeTasksCallback);
		}

		// Token: 0x060033F7 RID: 13303 RVA: 0x000E1C70 File Offset: 0x000E0C70
		private void StartTimerIfNeeeded()
		{
			if (this._timeoutTimer != null)
			{
				return;
			}
			DateTime utcNow = DateTime.UtcNow;
			if (utcNow >= this._timeoutEnd)
			{
				return;
			}
			double totalMilliseconds = (this._timeoutEnd - utcNow).TotalMilliseconds;
			if (totalMilliseconds >= 2147483647.0)
			{
				return;
			}
			this._timeoutTimer = new Timer(new TimerCallback(this.TimeoutTimerCallback), null, (int)totalMilliseconds, -1);
		}

		// Token: 0x060033F8 RID: 13304 RVA: 0x000E1CD8 File Offset: 0x000E0CD8
		internal void DisposeTimer()
		{
			Timer timeoutTimer = this._timeoutTimer;
			if (timeoutTimer != null && Interlocked.CompareExchange<Timer>(ref this._timeoutTimer, null, timeoutTimer) == timeoutTimer)
			{
				timeoutTimer.Dispose();
			}
		}

		// Token: 0x060033F9 RID: 13305 RVA: 0x000E1D05 File Offset: 0x000E0D05
		private void TimeoutTimerCallback(object state)
		{
			this.DisposeTimer();
			this.WaitForAllStartedTasks(false, false);
		}

		// Token: 0x060033FA RID: 13306 RVA: 0x000E1D15 File Offset: 0x000E0D15
		internal void CompleteAllTasksNow(bool syncCaller)
		{
			this.WaitForAllStartedTasks(syncCaller, true);
		}

		// Token: 0x04002474 RID: 9332
		private Page _page;

		// Token: 0x04002475 RID: 9333
		private HttpApplication _app;

		// Token: 0x04002476 RID: 9334
		private HttpAsyncResult _asyncResult;

		// Token: 0x04002477 RID: 9335
		private bool _failedToStart;

		// Token: 0x04002478 RID: 9336
		private ArrayList _tasks;

		// Token: 0x04002479 RID: 9337
		private DateTime _timeoutEnd;

		// Token: 0x0400247A RID: 9338
		private volatile bool _timeoutEndReached;

		// Token: 0x0400247B RID: 9339
		private volatile bool _inProgress;

		// Token: 0x0400247C RID: 9340
		private int _tasksStarted;

		// Token: 0x0400247D RID: 9341
		private int _tasksCompleted;

		// Token: 0x0400247E RID: 9342
		private WaitCallback _resumeTasksCallback;

		// Token: 0x0400247F RID: 9343
		private Timer _timeoutTimer;
	}
}
