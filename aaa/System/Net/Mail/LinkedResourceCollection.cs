using System;
using System.Collections.ObjectModel;

namespace System.Net.Mail
{
	// Token: 0x02000698 RID: 1688
	public sealed class LinkedResourceCollection : Collection<LinkedResource>, IDisposable
	{
		// Token: 0x0600340F RID: 13327 RVA: 0x000DB85C File Offset: 0x000DA85C
		internal LinkedResourceCollection()
		{
		}

		// Token: 0x06003410 RID: 13328 RVA: 0x000DB864 File Offset: 0x000DA864
		public void Dispose()
		{
			if (this.disposed)
			{
				return;
			}
			foreach (LinkedResource linkedResource in this)
			{
				linkedResource.Dispose();
			}
			base.Clear();
			this.disposed = true;
		}

		// Token: 0x06003411 RID: 13329 RVA: 0x000DB8C4 File Offset: 0x000DA8C4
		protected override void RemoveItem(int index)
		{
			if (this.disposed)
			{
				throw new ObjectDisposedException(base.GetType().FullName);
			}
			base.RemoveItem(index);
		}

		// Token: 0x06003412 RID: 13330 RVA: 0x000DB8E6 File Offset: 0x000DA8E6
		protected override void ClearItems()
		{
			if (this.disposed)
			{
				throw new ObjectDisposedException(base.GetType().FullName);
			}
			base.ClearItems();
		}

		// Token: 0x06003413 RID: 13331 RVA: 0x000DB907 File Offset: 0x000DA907
		protected override void SetItem(int index, LinkedResource item)
		{
			if (this.disposed)
			{
				throw new ObjectDisposedException(base.GetType().FullName);
			}
			if (item == null)
			{
				throw new ArgumentNullException("item");
			}
			base.SetItem(index, item);
		}

		// Token: 0x06003414 RID: 13332 RVA: 0x000DB938 File Offset: 0x000DA938
		protected override void InsertItem(int index, LinkedResource item)
		{
			if (this.disposed)
			{
				throw new ObjectDisposedException(base.GetType().FullName);
			}
			if (item == null)
			{
				throw new ArgumentNullException("item");
			}
			base.InsertItem(index, item);
		}

		// Token: 0x04002FF5 RID: 12277
		private bool disposed;
	}
}
