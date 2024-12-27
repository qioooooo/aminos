using System;
using System.Globalization;
using System.Runtime.Serialization;
using System.Text;

namespace System.Collections.Specialized
{
	// Token: 0x0200025B RID: 603
	[Serializable]
	public class NameValueCollection : NameObjectCollectionBase
	{
		// Token: 0x060014B8 RID: 5304 RVA: 0x00044DCD File Offset: 0x00043DCD
		public NameValueCollection()
		{
		}

		// Token: 0x060014B9 RID: 5305 RVA: 0x00044DD5 File Offset: 0x00043DD5
		public NameValueCollection(NameValueCollection col)
			: base((col != null) ? col.Comparer : null)
		{
			this.Add(col);
		}

		// Token: 0x060014BA RID: 5306 RVA: 0x00044DF0 File Offset: 0x00043DF0
		[Obsolete("Please use NameValueCollection(IEqualityComparer) instead.")]
		public NameValueCollection(IHashCodeProvider hashProvider, IComparer comparer)
			: base(hashProvider, comparer)
		{
		}

		// Token: 0x060014BB RID: 5307 RVA: 0x00044DFA File Offset: 0x00043DFA
		public NameValueCollection(int capacity)
			: base(capacity)
		{
		}

		// Token: 0x060014BC RID: 5308 RVA: 0x00044E03 File Offset: 0x00043E03
		public NameValueCollection(IEqualityComparer equalityComparer)
			: base(equalityComparer)
		{
		}

		// Token: 0x060014BD RID: 5309 RVA: 0x00044E0C File Offset: 0x00043E0C
		public NameValueCollection(int capacity, IEqualityComparer equalityComparer)
			: base(capacity, equalityComparer)
		{
		}

		// Token: 0x060014BE RID: 5310 RVA: 0x00044E16 File Offset: 0x00043E16
		public NameValueCollection(int capacity, NameValueCollection col)
			: base(capacity, (col != null) ? col.Comparer : null)
		{
			if (col == null)
			{
				throw new ArgumentNullException("col");
			}
			base.Comparer = col.Comparer;
			this.Add(col);
		}

		// Token: 0x060014BF RID: 5311 RVA: 0x00044E4C File Offset: 0x00043E4C
		[Obsolete("Please use NameValueCollection(Int32, IEqualityComparer) instead.")]
		public NameValueCollection(int capacity, IHashCodeProvider hashProvider, IComparer comparer)
			: base(capacity, hashProvider, comparer)
		{
		}

		// Token: 0x060014C0 RID: 5312 RVA: 0x00044E57 File Offset: 0x00043E57
		internal NameValueCollection(DBNull dummy)
			: base(dummy)
		{
		}

		// Token: 0x060014C1 RID: 5313 RVA: 0x00044E60 File Offset: 0x00043E60
		protected NameValueCollection(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

		// Token: 0x060014C2 RID: 5314 RVA: 0x00044E6A File Offset: 0x00043E6A
		protected void InvalidateCachedArrays()
		{
			this._all = null;
			this._allKeys = null;
		}

		// Token: 0x060014C3 RID: 5315 RVA: 0x00044E7C File Offset: 0x00043E7C
		private static string GetAsOneString(ArrayList list)
		{
			int num = ((list != null) ? list.Count : 0);
			if (num == 1)
			{
				return (string)list[0];
			}
			if (num > 1)
			{
				StringBuilder stringBuilder = new StringBuilder((string)list[0]);
				for (int i = 1; i < num; i++)
				{
					stringBuilder.Append(',');
					stringBuilder.Append((string)list[i]);
				}
				return stringBuilder.ToString();
			}
			return null;
		}

		// Token: 0x060014C4 RID: 5316 RVA: 0x00044EF0 File Offset: 0x00043EF0
		private static string[] GetAsStringArray(ArrayList list)
		{
			int num = ((list != null) ? list.Count : 0);
			if (num == 0)
			{
				return null;
			}
			string[] array = new string[num];
			list.CopyTo(0, array, 0, num);
			return array;
		}

		// Token: 0x060014C5 RID: 5317 RVA: 0x00044F24 File Offset: 0x00043F24
		public void Add(NameValueCollection c)
		{
			if (c == null)
			{
				throw new ArgumentNullException("c");
			}
			this.InvalidateCachedArrays();
			int count = c.Count;
			for (int i = 0; i < count; i++)
			{
				string key = c.GetKey(i);
				string[] values = c.GetValues(i);
				if (values != null)
				{
					for (int j = 0; j < values.Length; j++)
					{
						this.Add(key, values[j]);
					}
				}
				else
				{
					this.Add(key, null);
				}
			}
		}

		// Token: 0x060014C6 RID: 5318 RVA: 0x00044F92 File Offset: 0x00043F92
		public virtual void Clear()
		{
			if (base.IsReadOnly)
			{
				throw new NotSupportedException(SR.GetString("CollectionReadOnly"));
			}
			this.InvalidateCachedArrays();
			base.BaseClear();
		}

		// Token: 0x060014C7 RID: 5319 RVA: 0x00044FB8 File Offset: 0x00043FB8
		public void CopyTo(Array dest, int index)
		{
			if (dest == null)
			{
				throw new ArgumentNullException("dest");
			}
			if (dest.Rank != 1)
			{
				throw new ArgumentException(SR.GetString("Arg_MultiRank"));
			}
			if (index < 0)
			{
				throw new ArgumentOutOfRangeException("index", SR.GetString("IndexOutOfRange", new object[] { index.ToString(CultureInfo.CurrentCulture) }));
			}
			if (dest.Length - index < this.Count)
			{
				throw new ArgumentException(SR.GetString("Arg_InsufficientSpace"));
			}
			int count = this.Count;
			if (this._all == null)
			{
				this._all = new string[count];
				for (int i = 0; i < count; i++)
				{
					this._all[i] = this.Get(i);
					dest.SetValue(this._all[i], i + index);
				}
				return;
			}
			for (int j = 0; j < count; j++)
			{
				dest.SetValue(this._all[j], j + index);
			}
		}

		// Token: 0x060014C8 RID: 5320 RVA: 0x000450A1 File Offset: 0x000440A1
		public bool HasKeys()
		{
			return this.InternalHasKeys();
		}

		// Token: 0x060014C9 RID: 5321 RVA: 0x000450A9 File Offset: 0x000440A9
		internal virtual bool InternalHasKeys()
		{
			return base.BaseHasKeys();
		}

		// Token: 0x060014CA RID: 5322 RVA: 0x000450B4 File Offset: 0x000440B4
		public virtual void Add(string name, string value)
		{
			if (base.IsReadOnly)
			{
				throw new NotSupportedException(SR.GetString("CollectionReadOnly"));
			}
			this.InvalidateCachedArrays();
			ArrayList arrayList = (ArrayList)base.BaseGet(name);
			if (arrayList == null)
			{
				arrayList = new ArrayList(1);
				if (value != null)
				{
					arrayList.Add(value);
				}
				base.BaseAdd(name, arrayList);
				return;
			}
			if (value != null)
			{
				arrayList.Add(value);
			}
		}

		// Token: 0x060014CB RID: 5323 RVA: 0x00045118 File Offset: 0x00044118
		public virtual string Get(string name)
		{
			ArrayList arrayList = (ArrayList)base.BaseGet(name);
			return NameValueCollection.GetAsOneString(arrayList);
		}

		// Token: 0x060014CC RID: 5324 RVA: 0x00045138 File Offset: 0x00044138
		public virtual string[] GetValues(string name)
		{
			ArrayList arrayList = (ArrayList)base.BaseGet(name);
			return NameValueCollection.GetAsStringArray(arrayList);
		}

		// Token: 0x060014CD RID: 5325 RVA: 0x00045158 File Offset: 0x00044158
		public virtual void Set(string name, string value)
		{
			if (base.IsReadOnly)
			{
				throw new NotSupportedException(SR.GetString("CollectionReadOnly"));
			}
			this.InvalidateCachedArrays();
			base.BaseSet(name, new ArrayList(1) { value });
		}

		// Token: 0x060014CE RID: 5326 RVA: 0x0004519A File Offset: 0x0004419A
		public virtual void Remove(string name)
		{
			this.InvalidateCachedArrays();
			base.BaseRemove(name);
		}

		// Token: 0x1700044D RID: 1101
		public string this[string name]
		{
			get
			{
				return this.Get(name);
			}
			set
			{
				this.Set(name, value);
			}
		}

		// Token: 0x060014D1 RID: 5329 RVA: 0x000451BC File Offset: 0x000441BC
		public virtual string Get(int index)
		{
			ArrayList arrayList = (ArrayList)base.BaseGet(index);
			return NameValueCollection.GetAsOneString(arrayList);
		}

		// Token: 0x060014D2 RID: 5330 RVA: 0x000451DC File Offset: 0x000441DC
		public virtual string[] GetValues(int index)
		{
			ArrayList arrayList = (ArrayList)base.BaseGet(index);
			return NameValueCollection.GetAsStringArray(arrayList);
		}

		// Token: 0x060014D3 RID: 5331 RVA: 0x000451FC File Offset: 0x000441FC
		public virtual string GetKey(int index)
		{
			return base.BaseGetKey(index);
		}

		// Token: 0x1700044E RID: 1102
		public string this[int index]
		{
			get
			{
				return this.Get(index);
			}
		}

		// Token: 0x1700044F RID: 1103
		// (get) Token: 0x060014D5 RID: 5333 RVA: 0x0004520E File Offset: 0x0004420E
		public virtual string[] AllKeys
		{
			get
			{
				if (this._allKeys == null)
				{
					this._allKeys = base.BaseGetAllKeys();
				}
				return this._allKeys;
			}
		}

		// Token: 0x040011A3 RID: 4515
		private string[] _all;

		// Token: 0x040011A4 RID: 4516
		private string[] _allKeys;
	}
}
