using System;
using System.Threading;

namespace System.Web
{
	// Token: 0x02000052 RID: 82
	internal class HttpAsyncResult : IAsyncResult
	{
		// Token: 0x06000292 RID: 658 RVA: 0x0000CDA0 File Offset: 0x0000BDA0
		internal HttpAsyncResult(AsyncCallback cb, object state)
		{
			this._callback = cb;
			this._asyncState = state;
			this._status = RequestNotificationStatus.Continue;
		}

		// Token: 0x06000293 RID: 659 RVA: 0x0000CDC0 File Offset: 0x0000BDC0
		internal HttpAsyncResult(AsyncCallback cb, object state, bool completed, object result, Exception error)
		{
			this._callback = cb;
			this._asyncState = state;
			this._completed = completed;
			this._completedSynchronously = completed;
			this._result = result;
			this._error = error;
			this._status = RequestNotificationStatus.Continue;
			if (this._completed && this._callback != null)
			{
				this._callback(this);
			}
		}

		// Token: 0x06000294 RID: 660 RVA: 0x0000CE22 File Offset: 0x0000BE22
		internal void SetComplete()
		{
			this._completed = true;
		}

		// Token: 0x06000295 RID: 661 RVA: 0x0000CE2B File Offset: 0x0000BE2B
		internal void Complete(bool synchronous, object result, Exception error, RequestNotificationStatus status)
		{
			this._completed = true;
			this._completedSynchronously = synchronous;
			this._result = result;
			this._error = error;
			this._status = status;
			if (this._callback != null)
			{
				this._callback(this);
			}
		}

		// Token: 0x06000296 RID: 662 RVA: 0x0000CE65 File Offset: 0x0000BE65
		internal void Complete(bool synchronous, object result, Exception error)
		{
			this.Complete(synchronous, result, error, RequestNotificationStatus.Continue);
		}

		// Token: 0x06000297 RID: 663 RVA: 0x0000CE71 File Offset: 0x0000BE71
		internal object End()
		{
			if (this._error != null)
			{
				throw new HttpException(null, this._error);
			}
			return this._result;
		}

		// Token: 0x170000B9 RID: 185
		// (get) Token: 0x06000298 RID: 664 RVA: 0x0000CE8E File Offset: 0x0000BE8E
		internal Exception Error
		{
			get
			{
				return this._error;
			}
		}

		// Token: 0x170000BA RID: 186
		// (get) Token: 0x06000299 RID: 665 RVA: 0x0000CE96 File Offset: 0x0000BE96
		internal RequestNotificationStatus Status
		{
			get
			{
				return this._status;
			}
		}

		// Token: 0x170000BB RID: 187
		// (get) Token: 0x0600029A RID: 666 RVA: 0x0000CE9E File Offset: 0x0000BE9E
		public bool IsCompleted
		{
			get
			{
				return this._completed;
			}
		}

		// Token: 0x170000BC RID: 188
		// (get) Token: 0x0600029B RID: 667 RVA: 0x0000CEA6 File Offset: 0x0000BEA6
		public bool CompletedSynchronously
		{
			get
			{
				return this._completedSynchronously;
			}
		}

		// Token: 0x170000BD RID: 189
		// (get) Token: 0x0600029C RID: 668 RVA: 0x0000CEAE File Offset: 0x0000BEAE
		public object AsyncState
		{
			get
			{
				return this._asyncState;
			}
		}

		// Token: 0x170000BE RID: 190
		// (get) Token: 0x0600029D RID: 669 RVA: 0x0000CEB6 File Offset: 0x0000BEB6
		public WaitHandle AsyncWaitHandle
		{
			get
			{
				return null;
			}
		}

		// Token: 0x04000E6C RID: 3692
		private AsyncCallback _callback;

		// Token: 0x04000E6D RID: 3693
		private object _asyncState;

		// Token: 0x04000E6E RID: 3694
		private bool _completed;

		// Token: 0x04000E6F RID: 3695
		private bool _completedSynchronously;

		// Token: 0x04000E70 RID: 3696
		private object _result;

		// Token: 0x04000E71 RID: 3697
		private Exception _error;

		// Token: 0x04000E72 RID: 3698
		private RequestNotificationStatus _status;
	}
}
