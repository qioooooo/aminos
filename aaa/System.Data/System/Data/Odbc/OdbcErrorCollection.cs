using System;
using System.Collections;

namespace System.Data.Odbc
{
	// Token: 0x020001EC RID: 492
	[Serializable]
	public sealed class OdbcErrorCollection : ICollection, IEnumerable
	{
		// Token: 0x06001B88 RID: 7048 RVA: 0x002482F8 File Offset: 0x002476F8
		internal OdbcErrorCollection()
		{
		}

		// Token: 0x170003AD RID: 941
		// (get) Token: 0x06001B89 RID: 7049 RVA: 0x00248318 File Offset: 0x00247718
		object ICollection.SyncRoot
		{
			get
			{
				return this;
			}
		}

		// Token: 0x170003AE RID: 942
		// (get) Token: 0x06001B8A RID: 7050 RVA: 0x00248328 File Offset: 0x00247728
		bool ICollection.IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170003AF RID: 943
		// (get) Token: 0x06001B8B RID: 7051 RVA: 0x00248338 File Offset: 0x00247738
		public int Count
		{
			get
			{
				return this._items.Count;
			}
		}

		// Token: 0x170003B0 RID: 944
		public OdbcError this[int i]
		{
			get
			{
				return (OdbcError)this._items[i];
			}
		}

		// Token: 0x06001B8D RID: 7053 RVA: 0x00248370 File Offset: 0x00247770
		internal void Add(OdbcError error)
		{
			this._items.Add(error);
		}

		// Token: 0x06001B8E RID: 7054 RVA: 0x0024838C File Offset: 0x0024778C
		public void CopyTo(Array array, int i)
		{
			this._items.CopyTo(array, i);
		}

		// Token: 0x06001B8F RID: 7055 RVA: 0x002483A8 File Offset: 0x002477A8
		public void CopyTo(OdbcError[] array, int i)
		{
			this._items.CopyTo(array, i);
		}

		// Token: 0x06001B90 RID: 7056 RVA: 0x002483C4 File Offset: 0x002477C4
		public IEnumerator GetEnumerator()
		{
			return this._items.GetEnumerator();
		}

		// Token: 0x06001B91 RID: 7057 RVA: 0x002483DC File Offset: 0x002477DC
		internal void SetSource(string Source)
		{
			foreach (object obj in this._items)
			{
				((OdbcError)obj).SetSource(Source);
			}
		}

		// Token: 0x0400100B RID: 4107
		private ArrayList _items = new ArrayList();
	}
}
