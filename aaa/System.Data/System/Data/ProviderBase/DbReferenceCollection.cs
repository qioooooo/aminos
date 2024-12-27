using System;
using System.Collections;

namespace System.Data.ProviderBase
{
	// Token: 0x020001FC RID: 508
	internal abstract class DbReferenceCollection
	{
		// Token: 0x06001C66 RID: 7270 RVA: 0x0024D164 File Offset: 0x0024C564
		protected DbReferenceCollection()
		{
			this._items = new DbReferenceCollection.CollectionEntry[5];
		}

		// Token: 0x06001C67 RID: 7271
		public abstract void Add(object value, int tag);

		// Token: 0x06001C68 RID: 7272 RVA: 0x0024D184 File Offset: 0x0024C584
		protected void AddItem(object value, int tag)
		{
			DbReferenceCollection.CollectionEntry[] items = this._items;
			for (int i = 0; i < items.Length; i++)
			{
				if (!items[i].HasTarget)
				{
					items[i].Target = value;
					items[i].Tag = tag;
					return;
				}
			}
			int num = ((5 == items.Length) ? 15 : (items.Length + 15));
			DbReferenceCollection.CollectionEntry[] array = new DbReferenceCollection.CollectionEntry[num];
			for (int j = 0; j < items.Length; j++)
			{
				array[j] = items[j];
			}
			array[items.Length].Target = value;
			array[items.Length].Tag = tag;
			this._items = array;
		}

		// Token: 0x06001C69 RID: 7273 RVA: 0x0024D234 File Offset: 0x0024C634
		internal IEnumerable Filter(int tag)
		{
			return new DbReferenceCollection.DbFilteredReferenceCollection(this._items, tag);
		}

		// Token: 0x06001C6A RID: 7274 RVA: 0x0024D254 File Offset: 0x0024C654
		public void Notify(int message)
		{
			DbReferenceCollection.CollectionEntry[] items = this._items;
			int num = 0;
			while (num < items.Length && items[num].InUse)
			{
				object target = items[num].Target;
				if (target != null && !this.NotifyItem(message, items[num].Tag, target))
				{
					items[num].Tag = 0;
					items[num].Target = null;
				}
				num++;
			}
		}

		// Token: 0x06001C6B RID: 7275
		protected abstract bool NotifyItem(int message, int tag, object value);

		// Token: 0x06001C6C RID: 7276 RVA: 0x0024D2C4 File Offset: 0x0024C6C4
		public void Purge()
		{
			DbReferenceCollection.CollectionEntry[] items = this._items;
			if (100 < items.Length)
			{
				this._items = new DbReferenceCollection.CollectionEntry[5];
			}
		}

		// Token: 0x06001C6D RID: 7277
		public abstract void Remove(object value);

		// Token: 0x06001C6E RID: 7278 RVA: 0x0024D2EC File Offset: 0x0024C6EC
		protected void RemoveItem(object value)
		{
			DbReferenceCollection.CollectionEntry[] items = this._items;
			int num = 0;
			while (num < items.Length && items[num].InUse)
			{
				if (value == items[num].Target)
				{
					items[num].Tag = 0;
					items[num].Target = null;
					return;
				}
				num++;
			}
		}

		// Token: 0x0400105D RID: 4189
		private DbReferenceCollection.CollectionEntry[] _items;

		// Token: 0x020001FD RID: 509
		private struct CollectionEntry
		{
			// Token: 0x170003D3 RID: 979
			// (get) Token: 0x06001C6F RID: 7279 RVA: 0x0024D348 File Offset: 0x0024C748
			public bool HasTarget
			{
				get
				{
					return this._tag != 0 && this._weak != null && this._weak.IsAlive;
				}
			}

			// Token: 0x170003D4 RID: 980
			// (get) Token: 0x06001C70 RID: 7280 RVA: 0x0024D374 File Offset: 0x0024C774
			public bool InUse
			{
				get
				{
					return null != this._weak;
				}
			}

			// Token: 0x170003D5 RID: 981
			// (get) Token: 0x06001C71 RID: 7281 RVA: 0x0024D390 File Offset: 0x0024C790
			// (set) Token: 0x06001C72 RID: 7282 RVA: 0x0024D3A4 File Offset: 0x0024C7A4
			public int Tag
			{
				get
				{
					return this._tag;
				}
				set
				{
					this._tag = value;
				}
			}

			// Token: 0x170003D6 RID: 982
			// (get) Token: 0x06001C73 RID: 7283 RVA: 0x0024D3B8 File Offset: 0x0024C7B8
			// (set) Token: 0x06001C74 RID: 7284 RVA: 0x0024D3DC File Offset: 0x0024C7DC
			public object Target
			{
				get
				{
					if (this._tag != 0)
					{
						return this._weak.Target;
					}
					return null;
				}
				set
				{
					if (this._weak == null)
					{
						this._weak = new WeakReference(value, false);
						return;
					}
					this._weak.Target = value;
				}
			}

			// Token: 0x0400105E RID: 4190
			private int _tag;

			// Token: 0x0400105F RID: 4191
			private WeakReference _weak;
		}

		// Token: 0x020001FE RID: 510
		private struct DbFilteredReferenceCollection : IEnumerable
		{
			// Token: 0x06001C75 RID: 7285 RVA: 0x0024D40C File Offset: 0x0024C80C
			internal DbFilteredReferenceCollection(DbReferenceCollection.CollectionEntry[] items, int filterTag)
			{
				this._items = items;
				this._filterTag = filterTag;
			}

			// Token: 0x06001C76 RID: 7286 RVA: 0x0024D428 File Offset: 0x0024C828
			IEnumerator IEnumerable.GetEnumerator()
			{
				return new DbReferenceCollection.DbFilteredReferenceCollection.DbFilteredReferenceCollectionedEnumerator(this._items, this._filterTag);
			}

			// Token: 0x04001060 RID: 4192
			private readonly DbReferenceCollection.CollectionEntry[] _items;

			// Token: 0x04001061 RID: 4193
			private readonly int _filterTag;

			// Token: 0x020001FF RID: 511
			private struct DbFilteredReferenceCollectionedEnumerator : IEnumerator
			{
				// Token: 0x06001C77 RID: 7287 RVA: 0x0024D44C File Offset: 0x0024C84C
				internal DbFilteredReferenceCollectionedEnumerator(DbReferenceCollection.CollectionEntry[] items, int filterTag)
				{
					this._items = items;
					this._filterTag = filterTag;
					this._current = -1;
				}

				// Token: 0x170003D7 RID: 983
				// (get) Token: 0x06001C78 RID: 7288 RVA: 0x0024D470 File Offset: 0x0024C870
				object IEnumerator.Current
				{
					get
					{
						return this._items[this._current].Target;
					}
				}

				// Token: 0x06001C79 RID: 7289 RVA: 0x0024D494 File Offset: 0x0024C894
				bool IEnumerator.MoveNext()
				{
					while (++this._current < this._items.Length && this._items[this._current].InUse)
					{
						if (this._items[this._current].Tag == this._filterTag)
						{
							return true;
						}
					}
					return false;
				}

				// Token: 0x06001C7A RID: 7290 RVA: 0x0024D4F8 File Offset: 0x0024C8F8
				void IEnumerator.Reset()
				{
					this._current = -1;
				}

				// Token: 0x04001062 RID: 4194
				private readonly DbReferenceCollection.CollectionEntry[] _items;

				// Token: 0x04001063 RID: 4195
				private readonly int _filterTag;

				// Token: 0x04001064 RID: 4196
				private int _current;
			}
		}
	}
}
