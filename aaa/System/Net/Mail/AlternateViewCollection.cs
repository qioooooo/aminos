using System;
using System.Collections.ObjectModel;

namespace System.Net.Mail
{
	// Token: 0x02000678 RID: 1656
	public sealed class AlternateViewCollection : Collection<AlternateView>, IDisposable
	{
		// Token: 0x0600332B RID: 13099 RVA: 0x000D8474 File Offset: 0x000D7474
		internal AlternateViewCollection()
		{
		}

		// Token: 0x0600332C RID: 13100 RVA: 0x000D847C File Offset: 0x000D747C
		public void Dispose()
		{
			if (this.disposed)
			{
				return;
			}
			foreach (AlternateView alternateView in this)
			{
				alternateView.Dispose();
			}
			base.Clear();
			this.disposed = true;
		}

		// Token: 0x0600332D RID: 13101 RVA: 0x000D84DC File Offset: 0x000D74DC
		protected override void RemoveItem(int index)
		{
			if (this.disposed)
			{
				throw new ObjectDisposedException(base.GetType().FullName);
			}
			base.RemoveItem(index);
		}

		// Token: 0x0600332E RID: 13102 RVA: 0x000D84FE File Offset: 0x000D74FE
		protected override void ClearItems()
		{
			if (this.disposed)
			{
				throw new ObjectDisposedException(base.GetType().FullName);
			}
			base.ClearItems();
		}

		// Token: 0x0600332F RID: 13103 RVA: 0x000D851F File Offset: 0x000D751F
		protected override void SetItem(int index, AlternateView item)
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

		// Token: 0x06003330 RID: 13104 RVA: 0x000D8550 File Offset: 0x000D7550
		protected override void InsertItem(int index, AlternateView item)
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

		// Token: 0x04002F80 RID: 12160
		private bool disposed;
	}
}
