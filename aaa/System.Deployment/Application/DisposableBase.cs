using System;

namespace System.Deployment.Application
{
	// Token: 0x02000048 RID: 72
	internal abstract class DisposableBase : IDisposable
	{
		// Token: 0x06000245 RID: 581 RVA: 0x0000E550 File Offset: 0x0000D550
		public DisposableBase()
		{
		}

		// Token: 0x06000246 RID: 582 RVA: 0x0000E558 File Offset: 0x0000D558
		~DisposableBase()
		{
			this.Dispose(false);
		}

		// Token: 0x06000247 RID: 583 RVA: 0x0000E588 File Offset: 0x0000D588
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06000248 RID: 584 RVA: 0x0000E597 File Offset: 0x0000D597
		private void Dispose(bool disposing)
		{
			if (!this._disposed)
			{
				if (disposing)
				{
					this.DisposeManagedResources();
				}
				this.DisposeUnmanagedResources();
			}
			this._disposed = true;
		}

		// Token: 0x06000249 RID: 585 RVA: 0x0000E5B7 File Offset: 0x0000D5B7
		protected virtual void DisposeManagedResources()
		{
		}

		// Token: 0x0600024A RID: 586 RVA: 0x0000E5B9 File Offset: 0x0000D5B9
		protected virtual void DisposeUnmanagedResources()
		{
		}

		// Token: 0x040001D2 RID: 466
		private bool _disposed;
	}
}
