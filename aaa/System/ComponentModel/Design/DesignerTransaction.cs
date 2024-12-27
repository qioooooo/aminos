using System;
using System.Security.Permissions;

namespace System.ComponentModel.Design
{
	// Token: 0x0200016A RID: 362
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
	public abstract class DesignerTransaction : IDisposable
	{
		// Token: 0x06000BBA RID: 3002 RVA: 0x00028903 File Offset: 0x00027903
		protected DesignerTransaction()
			: this("")
		{
		}

		// Token: 0x06000BBB RID: 3003 RVA: 0x00028910 File Offset: 0x00027910
		protected DesignerTransaction(string description)
		{
			this.desc = description;
		}

		// Token: 0x1700025A RID: 602
		// (get) Token: 0x06000BBC RID: 3004 RVA: 0x0002891F File Offset: 0x0002791F
		public bool Canceled
		{
			get
			{
				return this.canceled;
			}
		}

		// Token: 0x1700025B RID: 603
		// (get) Token: 0x06000BBD RID: 3005 RVA: 0x00028927 File Offset: 0x00027927
		public bool Committed
		{
			get
			{
				return this.committed;
			}
		}

		// Token: 0x1700025C RID: 604
		// (get) Token: 0x06000BBE RID: 3006 RVA: 0x0002892F File Offset: 0x0002792F
		public string Description
		{
			get
			{
				return this.desc;
			}
		}

		// Token: 0x06000BBF RID: 3007 RVA: 0x00028937 File Offset: 0x00027937
		public void Cancel()
		{
			if (!this.canceled && !this.committed)
			{
				this.canceled = true;
				GC.SuppressFinalize(this);
				this.suppressedFinalization = true;
				this.OnCancel();
			}
		}

		// Token: 0x06000BC0 RID: 3008 RVA: 0x00028963 File Offset: 0x00027963
		public void Commit()
		{
			if (!this.committed && !this.canceled)
			{
				this.committed = true;
				GC.SuppressFinalize(this);
				this.suppressedFinalization = true;
				this.OnCommit();
			}
		}

		// Token: 0x06000BC1 RID: 3009
		protected abstract void OnCancel();

		// Token: 0x06000BC2 RID: 3010
		protected abstract void OnCommit();

		// Token: 0x06000BC3 RID: 3011 RVA: 0x00028990 File Offset: 0x00027990
		~DesignerTransaction()
		{
			this.Dispose(false);
		}

		// Token: 0x06000BC4 RID: 3012 RVA: 0x000289C0 File Offset: 0x000279C0
		void IDisposable.Dispose()
		{
			this.Dispose(true);
			if (!this.suppressedFinalization)
			{
				GC.SuppressFinalize(this);
			}
		}

		// Token: 0x06000BC5 RID: 3013 RVA: 0x000289D7 File Offset: 0x000279D7
		protected virtual void Dispose(bool disposing)
		{
			this.Cancel();
		}

		// Token: 0x04000AB8 RID: 2744
		private bool committed;

		// Token: 0x04000AB9 RID: 2745
		private bool canceled;

		// Token: 0x04000ABA RID: 2746
		private bool suppressedFinalization;

		// Token: 0x04000ABB RID: 2747
		private string desc;
	}
}
