using System;
using System.Threading;
using Microsoft.Win32.SafeHandles;

namespace System.DirectoryServices.Protocols
{
	// Token: 0x0200006C RID: 108
	internal class LdapAsyncResult : IAsyncResult
	{
		// Token: 0x06000238 RID: 568 RVA: 0x0000A61E File Offset: 0x0000961E
		public LdapAsyncResult(AsyncCallback callbackRoutine, object state, bool partialResults)
		{
			this.stateObject = state;
			this.callback = callbackRoutine;
			this.manualResetEvent = new ManualResetEvent(false);
			this.partialResults = partialResults;
		}

		// Token: 0x1700009A RID: 154
		// (get) Token: 0x06000239 RID: 569 RVA: 0x0000A647 File Offset: 0x00009647
		object IAsyncResult.AsyncState
		{
			get
			{
				return this.stateObject;
			}
		}

		// Token: 0x1700009B RID: 155
		// (get) Token: 0x0600023A RID: 570 RVA: 0x0000A64F File Offset: 0x0000964F
		WaitHandle IAsyncResult.AsyncWaitHandle
		{
			get
			{
				if (this.asyncWaitHandle == null)
				{
					this.asyncWaitHandle = new LdapAsyncResult.LdapAsyncWaitHandle(this.manualResetEvent.SafeWaitHandle);
				}
				return this.asyncWaitHandle;
			}
		}

		// Token: 0x1700009C RID: 156
		// (get) Token: 0x0600023B RID: 571 RVA: 0x0000A675 File Offset: 0x00009675
		bool IAsyncResult.CompletedSynchronously
		{
			get
			{
				return this.completedSynchronously;
			}
		}

		// Token: 0x1700009D RID: 157
		// (get) Token: 0x0600023C RID: 572 RVA: 0x0000A67D File Offset: 0x0000967D
		bool IAsyncResult.IsCompleted
		{
			get
			{
				return this.completed;
			}
		}

		// Token: 0x0600023D RID: 573 RVA: 0x0000A685 File Offset: 0x00009685
		public override int GetHashCode()
		{
			return this.manualResetEvent.GetHashCode();
		}

		// Token: 0x0600023E RID: 574 RVA: 0x0000A692 File Offset: 0x00009692
		public override bool Equals(object o)
		{
			return o is LdapAsyncResult && o != null && this == (LdapAsyncResult)o;
		}

		// Token: 0x0400021D RID: 541
		private LdapAsyncResult.LdapAsyncWaitHandle asyncWaitHandle;

		// Token: 0x0400021E RID: 542
		internal AsyncCallback callback;

		// Token: 0x0400021F RID: 543
		internal bool completed;

		// Token: 0x04000220 RID: 544
		private bool completedSynchronously;

		// Token: 0x04000221 RID: 545
		internal ManualResetEvent manualResetEvent;

		// Token: 0x04000222 RID: 546
		private object stateObject;

		// Token: 0x04000223 RID: 547
		internal LdapRequestState resultObject;

		// Token: 0x04000224 RID: 548
		internal bool partialResults;

		// Token: 0x0200006D RID: 109
		internal sealed class LdapAsyncWaitHandle : WaitHandle
		{
			// Token: 0x0600023F RID: 575 RVA: 0x0000A6AA File Offset: 0x000096AA
			public LdapAsyncWaitHandle(SafeWaitHandle handle)
			{
				base.SafeWaitHandle = handle;
			}

			// Token: 0x06000240 RID: 576 RVA: 0x0000A6BC File Offset: 0x000096BC
			~LdapAsyncWaitHandle()
			{
				base.SafeWaitHandle = null;
			}
		}
	}
}
