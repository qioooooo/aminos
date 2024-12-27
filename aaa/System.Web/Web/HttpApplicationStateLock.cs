using System;
using System.Web.Util;

namespace System.Web
{
	// Token: 0x02000051 RID: 81
	internal class HttpApplicationStateLock : ReadWriteObjectLock
	{
		// Token: 0x0600028C RID: 652 RVA: 0x0000CCA4 File Offset: 0x0000BCA4
		internal HttpApplicationStateLock()
		{
		}

		// Token: 0x0600028D RID: 653 RVA: 0x0000CCAC File Offset: 0x0000BCAC
		internal override void AcquireRead()
		{
			int currentThreadId = SafeNativeMethods.GetCurrentThreadId();
			if (this._threadId != currentThreadId)
			{
				base.AcquireRead();
			}
		}

		// Token: 0x0600028E RID: 654 RVA: 0x0000CCD0 File Offset: 0x0000BCD0
		internal override void ReleaseRead()
		{
			int currentThreadId = SafeNativeMethods.GetCurrentThreadId();
			if (this._threadId != currentThreadId)
			{
				base.ReleaseRead();
			}
		}

		// Token: 0x0600028F RID: 655 RVA: 0x0000CCF4 File Offset: 0x0000BCF4
		internal override void AcquireWrite()
		{
			int currentThreadId = SafeNativeMethods.GetCurrentThreadId();
			if (this._threadId == currentThreadId)
			{
				this._recursionCount++;
				return;
			}
			base.AcquireWrite();
			this._threadId = currentThreadId;
			this._recursionCount = 1;
		}

		// Token: 0x06000290 RID: 656 RVA: 0x0000CD34 File Offset: 0x0000BD34
		internal override void ReleaseWrite()
		{
			int currentThreadId = SafeNativeMethods.GetCurrentThreadId();
			if (this._threadId == currentThreadId && --this._recursionCount == 0)
			{
				this._threadId = 0;
				base.ReleaseWrite();
			}
		}

		// Token: 0x06000291 RID: 657 RVA: 0x0000CD70 File Offset: 0x0000BD70
		internal void EnsureReleaseWrite()
		{
			int currentThreadId = SafeNativeMethods.GetCurrentThreadId();
			if (this._threadId == currentThreadId)
			{
				this._threadId = 0;
				this._recursionCount = 0;
				base.ReleaseWrite();
			}
		}

		// Token: 0x04000E6A RID: 3690
		private int _recursionCount;

		// Token: 0x04000E6B RID: 3691
		private int _threadId;
	}
}
