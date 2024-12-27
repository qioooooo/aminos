using System;
using System.Collections.ObjectModel;

namespace System.Net.Mail
{
	// Token: 0x0200067A RID: 1658
	public sealed class AttachmentCollection : Collection<Attachment>, IDisposable
	{
		// Token: 0x06003342 RID: 13122 RVA: 0x000D87FC File Offset: 0x000D77FC
		internal AttachmentCollection()
		{
		}

		// Token: 0x06003343 RID: 13123 RVA: 0x000D8804 File Offset: 0x000D7804
		public void Dispose()
		{
			if (this.disposed)
			{
				return;
			}
			foreach (Attachment attachment in this)
			{
				attachment.Dispose();
			}
			base.Clear();
			this.disposed = true;
		}

		// Token: 0x06003344 RID: 13124 RVA: 0x000D8864 File Offset: 0x000D7864
		protected override void RemoveItem(int index)
		{
			if (this.disposed)
			{
				throw new ObjectDisposedException(base.GetType().FullName);
			}
			base.RemoveItem(index);
		}

		// Token: 0x06003345 RID: 13125 RVA: 0x000D8886 File Offset: 0x000D7886
		protected override void ClearItems()
		{
			if (this.disposed)
			{
				throw new ObjectDisposedException(base.GetType().FullName);
			}
			base.ClearItems();
		}

		// Token: 0x06003346 RID: 13126 RVA: 0x000D88A7 File Offset: 0x000D78A7
		protected override void SetItem(int index, Attachment item)
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

		// Token: 0x06003347 RID: 13127 RVA: 0x000D88D8 File Offset: 0x000D78D8
		protected override void InsertItem(int index, Attachment item)
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

		// Token: 0x04002F83 RID: 12163
		private bool disposed;
	}
}
