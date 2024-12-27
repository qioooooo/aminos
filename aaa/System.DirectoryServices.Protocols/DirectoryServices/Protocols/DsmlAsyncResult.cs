using System;
using System.Threading;
using Microsoft.Win32.SafeHandles;

namespace System.DirectoryServices.Protocols
{
	// Token: 0x02000058 RID: 88
	internal class DsmlAsyncResult : IAsyncResult
	{
		// Token: 0x060001A7 RID: 423 RVA: 0x00007AC2 File Offset: 0x00006AC2
		public DsmlAsyncResult(AsyncCallback callbackRoutine, object state)
		{
			this.stateObject = state;
			this.callback = callbackRoutine;
			this.manualResetEvent = new ManualResetEvent(false);
		}

		// Token: 0x17000070 RID: 112
		// (get) Token: 0x060001A8 RID: 424 RVA: 0x00007AE4 File Offset: 0x00006AE4
		object IAsyncResult.AsyncState
		{
			get
			{
				return this.stateObject;
			}
		}

		// Token: 0x17000071 RID: 113
		// (get) Token: 0x060001A9 RID: 425 RVA: 0x00007AEC File Offset: 0x00006AEC
		WaitHandle IAsyncResult.AsyncWaitHandle
		{
			get
			{
				if (this.asyncWaitHandle == null)
				{
					this.asyncWaitHandle = new DsmlAsyncResult.DsmlAsyncWaitHandle(this.manualResetEvent.SafeWaitHandle);
				}
				return this.asyncWaitHandle;
			}
		}

		// Token: 0x17000072 RID: 114
		// (get) Token: 0x060001AA RID: 426 RVA: 0x00007B12 File Offset: 0x00006B12
		bool IAsyncResult.CompletedSynchronously
		{
			get
			{
				return this.completedSynchronously;
			}
		}

		// Token: 0x17000073 RID: 115
		// (get) Token: 0x060001AB RID: 427 RVA: 0x00007B1A File Offset: 0x00006B1A
		bool IAsyncResult.IsCompleted
		{
			get
			{
				return this.completed;
			}
		}

		// Token: 0x060001AC RID: 428 RVA: 0x00007B22 File Offset: 0x00006B22
		public override int GetHashCode()
		{
			return this.manualResetEvent.GetHashCode();
		}

		// Token: 0x060001AD RID: 429 RVA: 0x00007B2F File Offset: 0x00006B2F
		public override bool Equals(object o)
		{
			return o is DsmlAsyncResult && o != null && this == (DsmlAsyncResult)o;
		}

		// Token: 0x04000198 RID: 408
		private DsmlAsyncResult.DsmlAsyncWaitHandle asyncWaitHandle;

		// Token: 0x04000199 RID: 409
		internal AsyncCallback callback;

		// Token: 0x0400019A RID: 410
		internal bool completed;

		// Token: 0x0400019B RID: 411
		private bool completedSynchronously;

		// Token: 0x0400019C RID: 412
		internal ManualResetEvent manualResetEvent;

		// Token: 0x0400019D RID: 413
		private object stateObject;

		// Token: 0x0400019E RID: 414
		internal RequestState resultObject;

		// Token: 0x0400019F RID: 415
		internal bool hasValidRequest;

		// Token: 0x02000059 RID: 89
		internal sealed class DsmlAsyncWaitHandle : WaitHandle
		{
			// Token: 0x060001AE RID: 430 RVA: 0x00007B47 File Offset: 0x00006B47
			public DsmlAsyncWaitHandle(SafeWaitHandle handle)
			{
				base.SafeWaitHandle = handle;
			}

			// Token: 0x060001AF RID: 431 RVA: 0x00007B58 File Offset: 0x00006B58
			~DsmlAsyncWaitHandle()
			{
				base.SafeWaitHandle = null;
			}
		}
	}
}
