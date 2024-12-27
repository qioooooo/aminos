using System;
using System.Threading;

namespace System.Runtime.Remoting.Channels
{
	// Token: 0x02000007 RID: 7
	internal class BasicAsyncResult : IAsyncResult
	{
		// Token: 0x06000014 RID: 20 RVA: 0x00002426 File Offset: 0x00001426
		internal BasicAsyncResult(AsyncCallback callback, object state)
		{
			this._asyncCallback = callback;
			this._asyncState = state;
		}

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x06000015 RID: 21 RVA: 0x0000243C File Offset: 0x0000143C
		public object AsyncState
		{
			get
			{
				return this._asyncState;
			}
		}

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000016 RID: 22 RVA: 0x00002444 File Offset: 0x00001444
		public WaitHandle AsyncWaitHandle
		{
			get
			{
				bool bIsComplete = this._bIsComplete;
				if (this._manualResetEvent == null)
				{
					lock (this)
					{
						if (this._manualResetEvent == null)
						{
							this._manualResetEvent = new ManualResetEvent(bIsComplete);
						}
					}
				}
				if (!bIsComplete && this._bIsComplete)
				{
					this._manualResetEvent.Set();
				}
				return this._manualResetEvent;
			}
		}

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000017 RID: 23 RVA: 0x000024B4 File Offset: 0x000014B4
		public bool CompletedSynchronously
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000018 RID: 24 RVA: 0x000024B7 File Offset: 0x000014B7
		public bool IsCompleted
		{
			get
			{
				return this._bIsComplete;
			}
		}

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x06000019 RID: 25 RVA: 0x000024BF File Offset: 0x000014BF
		internal Exception Exception
		{
			get
			{
				return this._exception;
			}
		}

		// Token: 0x0600001A RID: 26 RVA: 0x000024C8 File Offset: 0x000014C8
		internal void SetComplete(object returnValue, Exception exception)
		{
			this._returnValue = returnValue;
			this._exception = exception;
			this.CleanupOnComplete();
			this._bIsComplete = true;
			try
			{
				if (this._manualResetEvent != null)
				{
					this._manualResetEvent.Set();
				}
			}
			catch (Exception ex)
			{
				if (this._exception == null)
				{
					this._exception = ex;
				}
			}
			catch
			{
				if (this._exception == null)
				{
					this._exception = new Exception(CoreChannel.GetResourceString("Remoting_nonClsCompliantException"));
				}
			}
			if (this._asyncCallback != null)
			{
				this._asyncCallback(this);
			}
		}

		// Token: 0x0600001B RID: 27 RVA: 0x00002568 File Offset: 0x00001568
		internal virtual void CleanupOnComplete()
		{
		}

		// Token: 0x0400003A RID: 58
		private AsyncCallback _asyncCallback;

		// Token: 0x0400003B RID: 59
		private object _asyncState;

		// Token: 0x0400003C RID: 60
		private object _returnValue;

		// Token: 0x0400003D RID: 61
		private Exception _exception;

		// Token: 0x0400003E RID: 62
		private bool _bIsComplete;

		// Token: 0x0400003F RID: 63
		private ManualResetEvent _manualResetEvent;
	}
}
