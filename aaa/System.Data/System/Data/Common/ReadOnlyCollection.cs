using System;
using System.Collections;
using System.Collections.Generic;

namespace System.Data.Common
{
	// Token: 0x0200012F RID: 303
	[Serializable]
	internal sealed class ReadOnlyCollection<T> : ICollection, ICollection<T>, IEnumerable<T>, IEnumerable
	{
		// Token: 0x06001400 RID: 5120 RVA: 0x00224EA8 File Offset: 0x002242A8
		internal ReadOnlyCollection(T[] items)
		{
			this._items = items;
		}

		// Token: 0x06001401 RID: 5121 RVA: 0x00224EC4 File Offset: 0x002242C4
		public void CopyTo(T[] array, int arrayIndex)
		{
			Array.Copy(this._items, 0, array, arrayIndex, this._items.Length);
		}

		// Token: 0x06001402 RID: 5122 RVA: 0x00224EE8 File Offset: 0x002242E8
		void ICollection.CopyTo(Array array, int arrayIndex)
		{
			Array.Copy(this._items, 0, array, arrayIndex, this._items.Length);
		}

		// Token: 0x06001403 RID: 5123 RVA: 0x00224F0C File Offset: 0x0022430C
		IEnumerator<T> IEnumerable<T>.GetEnumerator()
		{
			return new ReadOnlyCollection<T>.Enumerator<T>(this._items);
		}

		// Token: 0x06001404 RID: 5124 RVA: 0x00224F2C File Offset: 0x0022432C
		public IEnumerator GetEnumerator()
		{
			return new ReadOnlyCollection<T>.Enumerator<T>(this._items);
		}

		// Token: 0x170002BB RID: 699
		// (get) Token: 0x06001405 RID: 5125 RVA: 0x00224F4C File Offset: 0x0022434C
		bool ICollection.IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170002BC RID: 700
		// (get) Token: 0x06001406 RID: 5126 RVA: 0x00224F5C File Offset: 0x0022435C
		object ICollection.SyncRoot
		{
			get
			{
				return this._items;
			}
		}

		// Token: 0x170002BD RID: 701
		// (get) Token: 0x06001407 RID: 5127 RVA: 0x00224F70 File Offset: 0x00224370
		bool ICollection<T>.IsReadOnly
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06001408 RID: 5128 RVA: 0x00224F80 File Offset: 0x00224380
		void ICollection<T>.Add(T value)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06001409 RID: 5129 RVA: 0x00224F94 File Offset: 0x00224394
		void ICollection<T>.Clear()
		{
			throw new NotSupportedException();
		}

		// Token: 0x0600140A RID: 5130 RVA: 0x00224FA8 File Offset: 0x002243A8
		bool ICollection<T>.Contains(T value)
		{
			return Array.IndexOf<T>(this._items, value) >= 0;
		}

		// Token: 0x0600140B RID: 5131 RVA: 0x00224FC8 File Offset: 0x002243C8
		bool ICollection<T>.Remove(T value)
		{
			throw new NotSupportedException();
		}

		// Token: 0x170002BE RID: 702
		// (get) Token: 0x0600140C RID: 5132 RVA: 0x00224FDC File Offset: 0x002243DC
		public int Count
		{
			get
			{
				return this._items.Length;
			}
		}

		// Token: 0x04000C28 RID: 3112
		private T[] _items;

		// Token: 0x02000130 RID: 304
		[Serializable]
		internal struct Enumerator<K> : IEnumerator<K>, IDisposable, IEnumerator
		{
			// Token: 0x0600140D RID: 5133 RVA: 0x00224FF4 File Offset: 0x002243F4
			internal Enumerator(K[] items)
			{
				this._items = items;
				this._index = -1;
			}

			// Token: 0x0600140E RID: 5134 RVA: 0x00225010 File Offset: 0x00224410
			public void Dispose()
			{
			}

			// Token: 0x0600140F RID: 5135 RVA: 0x00225020 File Offset: 0x00224420
			public bool MoveNext()
			{
				return ++this._index < this._items.Length;
			}

			// Token: 0x170002BF RID: 703
			// (get) Token: 0x06001410 RID: 5136 RVA: 0x00225048 File Offset: 0x00224448
			public K Current
			{
				get
				{
					return this._items[this._index];
				}
			}

			// Token: 0x170002C0 RID: 704
			// (get) Token: 0x06001411 RID: 5137 RVA: 0x00225068 File Offset: 0x00224468
			object IEnumerator.Current
			{
				get
				{
					return this._items[this._index];
				}
			}

			// Token: 0x06001412 RID: 5138 RVA: 0x0022508C File Offset: 0x0022448C
			void IEnumerator.Reset()
			{
				this._index = -1;
			}

			// Token: 0x04000C29 RID: 3113
			private K[] _items;

			// Token: 0x04000C2A RID: 3114
			private int _index;
		}
	}
}
