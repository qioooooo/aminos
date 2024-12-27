using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.XPath;

namespace System.Xml.Xsl.Runtime
{
	// Token: 0x020000B6 RID: 182
	[EditorBrowsable(EditorBrowsableState.Never)]
	public class XmlQuerySequence<T> : IList<T>, ICollection<T>, IEnumerable<T>, IList, ICollection, IEnumerable
	{
		// Token: 0x060008C8 RID: 2248 RVA: 0x0002B061 File Offset: 0x0002A061
		public static XmlQuerySequence<T> CreateOrReuse(XmlQuerySequence<T> seq)
		{
			if (seq != null)
			{
				seq.Clear();
				return seq;
			}
			return new XmlQuerySequence<T>();
		}

		// Token: 0x060008C9 RID: 2249 RVA: 0x0002B073 File Offset: 0x0002A073
		public static XmlQuerySequence<T> CreateOrReuse(XmlQuerySequence<T> seq, T item)
		{
			if (seq != null)
			{
				seq.Clear();
				seq.Add(item);
				return seq;
			}
			return new XmlQuerySequence<T>(item);
		}

		// Token: 0x060008CA RID: 2250 RVA: 0x0002B08D File Offset: 0x0002A08D
		public XmlQuerySequence()
		{
			this.items = new T[16];
		}

		// Token: 0x060008CB RID: 2251 RVA: 0x0002B0A2 File Offset: 0x0002A0A2
		public XmlQuerySequence(int capacity)
		{
			this.items = new T[capacity];
		}

		// Token: 0x060008CC RID: 2252 RVA: 0x0002B0B6 File Offset: 0x0002A0B6
		public XmlQuerySequence(T[] array, int size)
		{
			this.items = array;
			this.size = size;
		}

		// Token: 0x060008CD RID: 2253 RVA: 0x0002B0CC File Offset: 0x0002A0CC
		public XmlQuerySequence(T value)
		{
			this.items = new T[1];
			this.items[0] = value;
			this.size = 1;
		}

		// Token: 0x060008CE RID: 2254 RVA: 0x0002B0F4 File Offset: 0x0002A0F4
		IEnumerator IEnumerable.GetEnumerator()
		{
			return new IListEnumerator<T>(this);
		}

		// Token: 0x060008CF RID: 2255 RVA: 0x0002B101 File Offset: 0x0002A101
		public IEnumerator<T> GetEnumerator()
		{
			return new IListEnumerator<T>(this);
		}

		// Token: 0x1700014E RID: 334
		// (get) Token: 0x060008D0 RID: 2256 RVA: 0x0002B10E File Offset: 0x0002A10E
		public int Count
		{
			get
			{
				return this.size;
			}
		}

		// Token: 0x1700014F RID: 335
		// (get) Token: 0x060008D1 RID: 2257 RVA: 0x0002B116 File Offset: 0x0002A116
		bool ICollection.IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000150 RID: 336
		// (get) Token: 0x060008D2 RID: 2258 RVA: 0x0002B119 File Offset: 0x0002A119
		object ICollection.SyncRoot
		{
			get
			{
				return this;
			}
		}

		// Token: 0x060008D3 RID: 2259 RVA: 0x0002B11C File Offset: 0x0002A11C
		void ICollection.CopyTo(Array array, int index)
		{
			if (this.size == 0)
			{
				return;
			}
			Array.Copy(this.items, 0, array, index, this.size);
		}

		// Token: 0x17000151 RID: 337
		// (get) Token: 0x060008D4 RID: 2260 RVA: 0x0002B13B File Offset: 0x0002A13B
		bool ICollection<T>.IsReadOnly
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060008D5 RID: 2261 RVA: 0x0002B13E File Offset: 0x0002A13E
		void ICollection<T>.Add(T value)
		{
			throw new NotSupportedException();
		}

		// Token: 0x060008D6 RID: 2262 RVA: 0x0002B145 File Offset: 0x0002A145
		void ICollection<T>.Clear()
		{
			throw new NotSupportedException();
		}

		// Token: 0x060008D7 RID: 2263 RVA: 0x0002B14C File Offset: 0x0002A14C
		public bool Contains(T value)
		{
			return this.IndexOf(value) != -1;
		}

		// Token: 0x060008D8 RID: 2264 RVA: 0x0002B15C File Offset: 0x0002A15C
		public void CopyTo(T[] array, int index)
		{
			for (int i = 0; i < this.Count; i++)
			{
				array[index + i] = this[i];
			}
		}

		// Token: 0x060008D9 RID: 2265 RVA: 0x0002B18A File Offset: 0x0002A18A
		bool ICollection<T>.Remove(T value)
		{
			throw new NotSupportedException();
		}

		// Token: 0x17000152 RID: 338
		// (get) Token: 0x060008DA RID: 2266 RVA: 0x0002B191 File Offset: 0x0002A191
		bool IList.IsFixedSize
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000153 RID: 339
		// (get) Token: 0x060008DB RID: 2267 RVA: 0x0002B194 File Offset: 0x0002A194
		bool IList.IsReadOnly
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000154 RID: 340
		object IList.this[int index]
		{
			get
			{
				if (index >= this.size)
				{
					throw new ArgumentOutOfRangeException();
				}
				return this.items[index];
			}
			set
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x060008DE RID: 2270 RVA: 0x0002B1C0 File Offset: 0x0002A1C0
		int IList.Add(object value)
		{
			throw new NotSupportedException();
		}

		// Token: 0x060008DF RID: 2271 RVA: 0x0002B1C7 File Offset: 0x0002A1C7
		void IList.Clear()
		{
			throw new NotSupportedException();
		}

		// Token: 0x060008E0 RID: 2272 RVA: 0x0002B1CE File Offset: 0x0002A1CE
		bool IList.Contains(object value)
		{
			return this.Contains((T)((object)value));
		}

		// Token: 0x060008E1 RID: 2273 RVA: 0x0002B1DC File Offset: 0x0002A1DC
		int IList.IndexOf(object value)
		{
			return this.IndexOf((T)((object)value));
		}

		// Token: 0x060008E2 RID: 2274 RVA: 0x0002B1EA File Offset: 0x0002A1EA
		void IList.Insert(int index, object value)
		{
			throw new NotSupportedException();
		}

		// Token: 0x060008E3 RID: 2275 RVA: 0x0002B1F1 File Offset: 0x0002A1F1
		void IList.Remove(object value)
		{
			throw new NotSupportedException();
		}

		// Token: 0x060008E4 RID: 2276 RVA: 0x0002B1F8 File Offset: 0x0002A1F8
		void IList.RemoveAt(int index)
		{
			throw new NotSupportedException();
		}

		// Token: 0x17000155 RID: 341
		public T this[int index]
		{
			get
			{
				if (index >= this.size)
				{
					throw new ArgumentOutOfRangeException();
				}
				return this.items[index];
			}
			set
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x060008E7 RID: 2279 RVA: 0x0002B224 File Offset: 0x0002A224
		public int IndexOf(T value)
		{
			int num = Array.IndexOf<T>(this.items, value);
			if (num >= this.size)
			{
				return -1;
			}
			return num;
		}

		// Token: 0x060008E8 RID: 2280 RVA: 0x0002B24A File Offset: 0x0002A24A
		void IList<T>.Insert(int index, T value)
		{
			throw new NotSupportedException();
		}

		// Token: 0x060008E9 RID: 2281 RVA: 0x0002B251 File Offset: 0x0002A251
		void IList<T>.RemoveAt(int index)
		{
			throw new NotSupportedException();
		}

		// Token: 0x060008EA RID: 2282 RVA: 0x0002B258 File Offset: 0x0002A258
		public void Clear()
		{
			this.size = 0;
			this.OnItemsChanged();
		}

		// Token: 0x060008EB RID: 2283 RVA: 0x0002B268 File Offset: 0x0002A268
		public void Add(T value)
		{
			this.EnsureCache();
			this.items[this.size++] = value;
			this.OnItemsChanged();
		}

		// Token: 0x060008EC RID: 2284 RVA: 0x0002B29E File Offset: 0x0002A29E
		public void SortByKeys(Array keys)
		{
			if (this.size <= 1)
			{
				return;
			}
			Array.Sort(keys, this.items, 0, this.size);
			this.OnItemsChanged();
		}

		// Token: 0x060008ED RID: 2285 RVA: 0x0002B2C4 File Offset: 0x0002A2C4
		private void EnsureCache()
		{
			if (this.size >= this.items.Length)
			{
				T[] array = new T[this.size * 2];
				this.CopyTo(array, 0);
				this.items = array;
			}
		}

		// Token: 0x060008EE RID: 2286 RVA: 0x0002B2FE File Offset: 0x0002A2FE
		protected virtual void OnItemsChanged()
		{
		}

		// Token: 0x040005A5 RID: 1445
		private const int DefaultCacheSize = 16;

		// Token: 0x040005A6 RID: 1446
		public static readonly XmlQuerySequence<T> Empty = new XmlQuerySequence<T>();

		// Token: 0x040005A7 RID: 1447
		private static readonly Type XPathItemType = typeof(XPathItem);

		// Token: 0x040005A8 RID: 1448
		private T[] items;

		// Token: 0x040005A9 RID: 1449
		private int size;
	}
}
