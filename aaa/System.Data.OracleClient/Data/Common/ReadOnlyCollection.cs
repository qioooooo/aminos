using System;
using System.Collections;
using System.Collections.Generic;

namespace System.Data.Common
{
	// Token: 0x02000089 RID: 137
	[Serializable]
	internal sealed class ReadOnlyCollection<T> : ICollection, ICollection<T>, IEnumerable<T>, IEnumerable
	{
		// Token: 0x060007CD RID: 1997 RVA: 0x000725F0 File Offset: 0x000719F0
		internal ReadOnlyCollection(T[] items)
		{
			this._items = items;
		}

		// Token: 0x060007CE RID: 1998 RVA: 0x0007260C File Offset: 0x00071A0C
		public void CopyTo(T[] array, int arrayIndex)
		{
			Array.Copy(this._items, 0, array, arrayIndex, this._items.Length);
		}

		// Token: 0x060007CF RID: 1999 RVA: 0x00072630 File Offset: 0x00071A30
		void ICollection.CopyTo(Array array, int arrayIndex)
		{
			Array.Copy(this._items, 0, array, arrayIndex, this._items.Length);
		}

		// Token: 0x060007D0 RID: 2000 RVA: 0x00072654 File Offset: 0x00071A54
		IEnumerator<T> IEnumerable<T>.GetEnumerator()
		{
			return new ReadOnlyCollection<T>.Enumerator<T>(this._items);
		}

		// Token: 0x060007D1 RID: 2001 RVA: 0x00072674 File Offset: 0x00071A74
		public IEnumerator GetEnumerator()
		{
			return new ReadOnlyCollection<T>.Enumerator<T>(this._items);
		}

		// Token: 0x1700015B RID: 347
		// (get) Token: 0x060007D2 RID: 2002 RVA: 0x00072694 File Offset: 0x00071A94
		bool ICollection.IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700015C RID: 348
		// (get) Token: 0x060007D3 RID: 2003 RVA: 0x000726A4 File Offset: 0x00071AA4
		object ICollection.SyncRoot
		{
			get
			{
				return this._items;
			}
		}

		// Token: 0x1700015D RID: 349
		// (get) Token: 0x060007D4 RID: 2004 RVA: 0x000726B8 File Offset: 0x00071AB8
		bool ICollection<T>.IsReadOnly
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060007D5 RID: 2005 RVA: 0x000726C8 File Offset: 0x00071AC8
		void ICollection<T>.Add(T value)
		{
			throw new NotSupportedException();
		}

		// Token: 0x060007D6 RID: 2006 RVA: 0x000726DC File Offset: 0x00071ADC
		void ICollection<T>.Clear()
		{
			throw new NotSupportedException();
		}

		// Token: 0x060007D7 RID: 2007 RVA: 0x000726F0 File Offset: 0x00071AF0
		bool ICollection<T>.Contains(T value)
		{
			return Array.IndexOf<T>(this._items, value) >= 0;
		}

		// Token: 0x060007D8 RID: 2008 RVA: 0x00072710 File Offset: 0x00071B10
		bool ICollection<T>.Remove(T value)
		{
			throw new NotSupportedException();
		}

		// Token: 0x1700015E RID: 350
		// (get) Token: 0x060007D9 RID: 2009 RVA: 0x00072724 File Offset: 0x00071B24
		public int Count
		{
			get
			{
				return this._items.Length;
			}
		}

		// Token: 0x04000509 RID: 1289
		private T[] _items;

		// Token: 0x0200008A RID: 138
		[Serializable]
		internal struct Enumerator<K> : IEnumerator<K>, IDisposable, IEnumerator
		{
			// Token: 0x060007DA RID: 2010 RVA: 0x0007273C File Offset: 0x00071B3C
			internal Enumerator(K[] items)
			{
				this._items = items;
				this._index = -1;
			}

			// Token: 0x060007DB RID: 2011 RVA: 0x00072758 File Offset: 0x00071B58
			public void Dispose()
			{
			}

			// Token: 0x060007DC RID: 2012 RVA: 0x00072768 File Offset: 0x00071B68
			public bool MoveNext()
			{
				return ++this._index < this._items.Length;
			}

			// Token: 0x1700015F RID: 351
			// (get) Token: 0x060007DD RID: 2013 RVA: 0x00072790 File Offset: 0x00071B90
			public K Current
			{
				get
				{
					return this._items[this._index];
				}
			}

			// Token: 0x17000160 RID: 352
			// (get) Token: 0x060007DE RID: 2014 RVA: 0x000727B0 File Offset: 0x00071BB0
			object IEnumerator.Current
			{
				get
				{
					return this._items[this._index];
				}
			}

			// Token: 0x060007DF RID: 2015 RVA: 0x000727D4 File Offset: 0x00071BD4
			void IEnumerator.Reset()
			{
				this._index = -1;
			}

			// Token: 0x0400050A RID: 1290
			private K[] _items;

			// Token: 0x0400050B RID: 1291
			private int _index;
		}
	}
}
