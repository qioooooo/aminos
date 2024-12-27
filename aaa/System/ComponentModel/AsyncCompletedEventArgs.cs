using System;
using System.Reflection;
using System.Security.Permissions;

namespace System.ComponentModel
{
	// Token: 0x02000097 RID: 151
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public class AsyncCompletedEventArgs : EventArgs
	{
		// Token: 0x06000571 RID: 1393 RVA: 0x00016D97 File Offset: 0x00015D97
		public AsyncCompletedEventArgs(Exception error, bool cancelled, object userState)
		{
			this.error = error;
			this.cancelled = cancelled;
			this.userState = userState;
		}

		// Token: 0x1700010D RID: 269
		// (get) Token: 0x06000572 RID: 1394 RVA: 0x00016DB4 File Offset: 0x00015DB4
		[SRDescription("Async_AsyncEventArgs_Cancelled")]
		public bool Cancelled
		{
			get
			{
				return this.cancelled;
			}
		}

		// Token: 0x1700010E RID: 270
		// (get) Token: 0x06000573 RID: 1395 RVA: 0x00016DBC File Offset: 0x00015DBC
		[SRDescription("Async_AsyncEventArgs_Error")]
		public Exception Error
		{
			get
			{
				return this.error;
			}
		}

		// Token: 0x1700010F RID: 271
		// (get) Token: 0x06000574 RID: 1396 RVA: 0x00016DC4 File Offset: 0x00015DC4
		[SRDescription("Async_AsyncEventArgs_UserState")]
		public object UserState
		{
			get
			{
				return this.userState;
			}
		}

		// Token: 0x06000575 RID: 1397 RVA: 0x00016DCC File Offset: 0x00015DCC
		protected void RaiseExceptionIfNecessary()
		{
			if (this.Error != null)
			{
				throw new TargetInvocationException(SR.GetString("Async_ExceptionOccurred"), this.Error);
			}
			if (this.Cancelled)
			{
				throw new InvalidOperationException(SR.GetString("Async_OperationCancelled"));
			}
		}

		// Token: 0x040008CE RID: 2254
		private readonly Exception error;

		// Token: 0x040008CF RID: 2255
		private readonly bool cancelled;

		// Token: 0x040008D0 RID: 2256
		private readonly object userState;
	}
}
