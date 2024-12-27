using System;
using System.Diagnostics;
using System.Threading;

namespace System.Net
{
	// Token: 0x020003A2 RID: 930
	internal class LazyAsyncResult : IAsyncResult
	{
		// Token: 0x170005A1 RID: 1441
		// (get) Token: 0x06001D14 RID: 7444 RVA: 0x0006F630 File Offset: 0x0006E630
		private static LazyAsyncResult.ThreadContext CurrentThreadContext
		{
			get
			{
				LazyAsyncResult.ThreadContext threadContext = LazyAsyncResult.t_ThreadContext;
				if (threadContext == null)
				{
					threadContext = new LazyAsyncResult.ThreadContext();
					LazyAsyncResult.t_ThreadContext = threadContext;
				}
				return threadContext;
			}
		}

		// Token: 0x06001D15 RID: 7445 RVA: 0x0006F653 File Offset: 0x0006E653
		internal LazyAsyncResult(object myObject, object myState, AsyncCallback myCallBack)
		{
			this.m_AsyncObject = myObject;
			this.m_AsyncState = myState;
			this.m_AsyncCallback = myCallBack;
			this.m_Result = DBNull.Value;
		}

		// Token: 0x06001D16 RID: 7446 RVA: 0x0006F67B File Offset: 0x0006E67B
		internal LazyAsyncResult(object myObject, object myState, AsyncCallback myCallBack, object result)
		{
			this.m_AsyncObject = myObject;
			this.m_AsyncState = myState;
			this.m_AsyncCallback = myCallBack;
			this.m_Result = result;
			this.m_IntCompleted = 1;
			if (this.m_AsyncCallback != null)
			{
				this.m_AsyncCallback(this);
			}
		}

		// Token: 0x170005A2 RID: 1442
		// (get) Token: 0x06001D17 RID: 7447 RVA: 0x0006F6BB File Offset: 0x0006E6BB
		internal object AsyncObject
		{
			get
			{
				return this.m_AsyncObject;
			}
		}

		// Token: 0x170005A3 RID: 1443
		// (get) Token: 0x06001D18 RID: 7448 RVA: 0x0006F6C3 File Offset: 0x0006E6C3
		public object AsyncState
		{
			get
			{
				return this.m_AsyncState;
			}
		}

		// Token: 0x170005A4 RID: 1444
		// (get) Token: 0x06001D19 RID: 7449 RVA: 0x0006F6CB File Offset: 0x0006E6CB
		// (set) Token: 0x06001D1A RID: 7450 RVA: 0x0006F6D3 File Offset: 0x0006E6D3
		protected AsyncCallback AsyncCallback
		{
			get
			{
				return this.m_AsyncCallback;
			}
			set
			{
				this.m_AsyncCallback = value;
			}
		}

		// Token: 0x170005A5 RID: 1445
		// (get) Token: 0x06001D1B RID: 7451 RVA: 0x0006F6DC File Offset: 0x0006E6DC
		public WaitHandle AsyncWaitHandle
		{
			get
			{
				this.m_UserEvent = true;
				if (this.m_IntCompleted == 0)
				{
					Interlocked.CompareExchange(ref this.m_IntCompleted, int.MinValue, 0);
				}
				ManualResetEvent manualResetEvent = (ManualResetEvent)this.m_Event;
				while (manualResetEvent == null)
				{
					this.LazilyCreateEvent(out manualResetEvent);
				}
				return manualResetEvent;
			}
		}

		// Token: 0x06001D1C RID: 7452 RVA: 0x0006F728 File Offset: 0x0006E728
		private bool LazilyCreateEvent(out ManualResetEvent waitHandle)
		{
			waitHandle = new ManualResetEvent(false);
			bool flag;
			try
			{
				if (Interlocked.CompareExchange(ref this.m_Event, waitHandle, null) == null)
				{
					if (this.InternalPeekCompleted)
					{
						waitHandle.Set();
					}
					flag = true;
				}
				else
				{
					waitHandle.Close();
					waitHandle = (ManualResetEvent)this.m_Event;
					flag = false;
				}
			}
			catch
			{
				this.m_Event = null;
				if (waitHandle != null)
				{
					waitHandle.Close();
				}
				throw;
			}
			return flag;
		}

		// Token: 0x06001D1D RID: 7453 RVA: 0x0006F7A0 File Offset: 0x0006E7A0
		[Conditional("DEBUG")]
		protected void DebugProtectState(bool protect)
		{
		}

		// Token: 0x170005A6 RID: 1446
		// (get) Token: 0x06001D1E RID: 7454 RVA: 0x0006F7A4 File Offset: 0x0006E7A4
		public bool CompletedSynchronously
		{
			get
			{
				int num = this.m_IntCompleted;
				if (num == 0)
				{
					num = Interlocked.CompareExchange(ref this.m_IntCompleted, int.MinValue, 0);
				}
				return num > 0;
			}
		}

		// Token: 0x170005A7 RID: 1447
		// (get) Token: 0x06001D1F RID: 7455 RVA: 0x0006F7D4 File Offset: 0x0006E7D4
		public bool IsCompleted
		{
			get
			{
				int num = this.m_IntCompleted;
				if (num == 0)
				{
					num = Interlocked.CompareExchange(ref this.m_IntCompleted, int.MinValue, 0);
				}
				return (num & int.MaxValue) != 0;
			}
		}

		// Token: 0x170005A8 RID: 1448
		// (get) Token: 0x06001D20 RID: 7456 RVA: 0x0006F80A File Offset: 0x0006E80A
		internal bool InternalPeekCompleted
		{
			get
			{
				return (this.m_IntCompleted & int.MaxValue) != 0;
			}
		}

		// Token: 0x170005A9 RID: 1449
		// (get) Token: 0x06001D21 RID: 7457 RVA: 0x0006F81E File Offset: 0x0006E81E
		// (set) Token: 0x06001D22 RID: 7458 RVA: 0x0006F835 File Offset: 0x0006E835
		internal object Result
		{
			get
			{
				if (this.m_Result != DBNull.Value)
				{
					return this.m_Result;
				}
				return null;
			}
			set
			{
				this.m_Result = value;
			}
		}

		// Token: 0x170005AA RID: 1450
		// (get) Token: 0x06001D23 RID: 7459 RVA: 0x0006F83E File Offset: 0x0006E83E
		// (set) Token: 0x06001D24 RID: 7460 RVA: 0x0006F846 File Offset: 0x0006E846
		internal bool EndCalled
		{
			get
			{
				return this.m_EndCalled;
			}
			set
			{
				this.m_EndCalled = value;
			}
		}

		// Token: 0x170005AB RID: 1451
		// (get) Token: 0x06001D25 RID: 7461 RVA: 0x0006F84F File Offset: 0x0006E84F
		// (set) Token: 0x06001D26 RID: 7462 RVA: 0x0006F857 File Offset: 0x0006E857
		internal int ErrorCode
		{
			get
			{
				return this.m_ErrorCode;
			}
			set
			{
				this.m_ErrorCode = value;
			}
		}

		// Token: 0x06001D27 RID: 7463 RVA: 0x0006F860 File Offset: 0x0006E860
		protected void ProtectedInvokeCallback(object result, IntPtr userToken)
		{
			if (result == DBNull.Value)
			{
				throw new ArgumentNullException("result");
			}
			if ((this.m_IntCompleted & 2147483647) == 0 && (Interlocked.Increment(ref this.m_IntCompleted) & 2147483647) == 1)
			{
				if (this.m_Result == DBNull.Value)
				{
					this.m_Result = result;
				}
				ManualResetEvent manualResetEvent = (ManualResetEvent)this.m_Event;
				if (manualResetEvent != null)
				{
					try
					{
						manualResetEvent.Set();
					}
					catch (ObjectDisposedException)
					{
					}
				}
				this.Complete(userToken);
			}
		}

		// Token: 0x06001D28 RID: 7464 RVA: 0x0006F8E8 File Offset: 0x0006E8E8
		internal void InvokeCallback(object result)
		{
			this.ProtectedInvokeCallback(result, IntPtr.Zero);
		}

		// Token: 0x06001D29 RID: 7465 RVA: 0x0006F8F6 File Offset: 0x0006E8F6
		internal void InvokeCallback()
		{
			this.ProtectedInvokeCallback(null, IntPtr.Zero);
		}

		// Token: 0x06001D2A RID: 7466 RVA: 0x0006F904 File Offset: 0x0006E904
		protected virtual void Complete(IntPtr userToken)
		{
			bool flag = false;
			LazyAsyncResult.ThreadContext currentThreadContext = LazyAsyncResult.CurrentThreadContext;
			try
			{
				currentThreadContext.m_NestedIOCount++;
				if (this.m_AsyncCallback != null)
				{
					if (currentThreadContext.m_NestedIOCount >= 50)
					{
						ThreadPool.QueueUserWorkItem(new WaitCallback(this.WorkerThreadComplete));
						flag = true;
					}
					else
					{
						this.m_AsyncCallback(this);
					}
				}
			}
			finally
			{
				currentThreadContext.m_NestedIOCount--;
				if (!flag)
				{
					this.Cleanup();
				}
			}
		}

		// Token: 0x06001D2B RID: 7467 RVA: 0x0006F988 File Offset: 0x0006E988
		private void WorkerThreadComplete(object state)
		{
			try
			{
				this.m_AsyncCallback(this);
			}
			finally
			{
				this.Cleanup();
			}
		}

		// Token: 0x06001D2C RID: 7468 RVA: 0x0006F9BC File Offset: 0x0006E9BC
		protected virtual void Cleanup()
		{
		}

		// Token: 0x06001D2D RID: 7469 RVA: 0x0006F9BE File Offset: 0x0006E9BE
		internal object InternalWaitForCompletion()
		{
			return this.WaitForCompletion(true);
		}

		// Token: 0x06001D2E RID: 7470 RVA: 0x0006F9C8 File Offset: 0x0006E9C8
		private object WaitForCompletion(bool snap)
		{
			ManualResetEvent manualResetEvent = null;
			bool flag = false;
			if (!(snap ? this.IsCompleted : this.InternalPeekCompleted))
			{
				manualResetEvent = (ManualResetEvent)this.m_Event;
				if (manualResetEvent == null)
				{
					flag = this.LazilyCreateEvent(out manualResetEvent);
				}
			}
			if (manualResetEvent == null)
			{
				goto IL_0077;
			}
			try
			{
				try
				{
					manualResetEvent.WaitOne(-1, false);
				}
				catch (ObjectDisposedException)
				{
				}
				goto IL_0077;
			}
			finally
			{
				if (flag && !this.m_UserEvent)
				{
					ManualResetEvent manualResetEvent2 = (ManualResetEvent)this.m_Event;
					this.m_Event = null;
					if (!this.m_UserEvent)
					{
						manualResetEvent2.Close();
					}
				}
			}
			IL_0071:
			Thread.SpinWait(1);
			IL_0077:
			if (this.m_Result != DBNull.Value)
			{
				return this.m_Result;
			}
			goto IL_0071;
		}

		// Token: 0x06001D2F RID: 7471 RVA: 0x0006FA7C File Offset: 0x0006EA7C
		internal void InternalCleanup()
		{
			if ((this.m_IntCompleted & 2147483647) == 0 && (Interlocked.Increment(ref this.m_IntCompleted) & 2147483647) == 1)
			{
				this.m_Result = null;
				this.Cleanup();
			}
		}

		// Token: 0x04001D5A RID: 7514
		private const int c_HighBit = -2147483648;

		// Token: 0x04001D5B RID: 7515
		private const int c_ForceAsyncCount = 50;

		// Token: 0x04001D5C RID: 7516
		[ThreadStatic]
		private static LazyAsyncResult.ThreadContext t_ThreadContext;

		// Token: 0x04001D5D RID: 7517
		private object m_AsyncObject;

		// Token: 0x04001D5E RID: 7518
		private object m_AsyncState;

		// Token: 0x04001D5F RID: 7519
		private AsyncCallback m_AsyncCallback;

		// Token: 0x04001D60 RID: 7520
		private object m_Result;

		// Token: 0x04001D61 RID: 7521
		private int m_ErrorCode;

		// Token: 0x04001D62 RID: 7522
		private int m_IntCompleted;

		// Token: 0x04001D63 RID: 7523
		private bool m_EndCalled;

		// Token: 0x04001D64 RID: 7524
		private bool m_UserEvent;

		// Token: 0x04001D65 RID: 7525
		private object m_Event;

		// Token: 0x020003A3 RID: 931
		private class ThreadContext
		{
			// Token: 0x04001D66 RID: 7526
			internal int m_NestedIOCount;
		}
	}
}
