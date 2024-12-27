using System;
using System.Collections;

namespace System.Runtime.Remoting.Channels.Http
{
	// Token: 0x02000025 RID: 37
	internal class AggregateDictionary : IDictionary, ICollection, IEnumerable
	{
		// Token: 0x0600010A RID: 266 RVA: 0x00005ABA File Offset: 0x00004ABA
		public AggregateDictionary(ICollection dictionaries)
		{
			this._dictionaries = dictionaries;
		}

		// Token: 0x17000034 RID: 52
		public virtual object this[object key]
		{
			get
			{
				foreach (object obj in this._dictionaries)
				{
					IDictionary dictionary = (IDictionary)obj;
					if (dictionary.Contains(key))
					{
						return dictionary[key];
					}
				}
				return null;
			}
			set
			{
				foreach (object obj in this._dictionaries)
				{
					IDictionary dictionary = (IDictionary)obj;
					if (dictionary.Contains(key))
					{
						dictionary[key] = value;
					}
				}
			}
		}

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x0600010D RID: 269 RVA: 0x00005B98 File Offset: 0x00004B98
		public virtual ICollection Keys
		{
			get
			{
				ArrayList arrayList = new ArrayList();
				foreach (object obj in this._dictionaries)
				{
					IDictionary dictionary = (IDictionary)obj;
					ICollection keys = dictionary.Keys;
					if (keys != null)
					{
						foreach (object obj2 in keys)
						{
							arrayList.Add(obj2);
						}
					}
				}
				return arrayList;
			}
		}

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x0600010E RID: 270 RVA: 0x00005C48 File Offset: 0x00004C48
		public virtual ICollection Values
		{
			get
			{
				ArrayList arrayList = new ArrayList();
				foreach (object obj in this._dictionaries)
				{
					IDictionary dictionary = (IDictionary)obj;
					ICollection values = dictionary.Values;
					if (values != null)
					{
						foreach (object obj2 in values)
						{
							arrayList.Add(obj2);
						}
					}
				}
				return arrayList;
			}
		}

		// Token: 0x0600010F RID: 271 RVA: 0x00005CF8 File Offset: 0x00004CF8
		public virtual bool Contains(object key)
		{
			foreach (object obj in this._dictionaries)
			{
				IDictionary dictionary = (IDictionary)obj;
				if (dictionary.Contains(key))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x06000110 RID: 272 RVA: 0x00005D5C File Offset: 0x00004D5C
		public virtual bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x06000111 RID: 273 RVA: 0x00005D5F File Offset: 0x00004D5F
		public virtual bool IsFixedSize
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000112 RID: 274 RVA: 0x00005D62 File Offset: 0x00004D62
		public virtual void Add(object key, object value)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06000113 RID: 275 RVA: 0x00005D69 File Offset: 0x00004D69
		public virtual void Clear()
		{
			throw new NotSupportedException();
		}

		// Token: 0x06000114 RID: 276 RVA: 0x00005D70 File Offset: 0x00004D70
		public virtual void Remove(object key)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06000115 RID: 277 RVA: 0x00005D77 File Offset: 0x00004D77
		public virtual IDictionaryEnumerator GetEnumerator()
		{
			return new DictionaryEnumeratorByKeys(this);
		}

		// Token: 0x06000116 RID: 278 RVA: 0x00005D7F File Offset: 0x00004D7F
		public virtual void CopyTo(Array array, int index)
		{
			throw new NotSupportedException();
		}

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x06000117 RID: 279 RVA: 0x00005D88 File Offset: 0x00004D88
		public virtual int Count
		{
			get
			{
				int num = 0;
				foreach (object obj in this._dictionaries)
				{
					IDictionary dictionary = (IDictionary)obj;
					num += dictionary.Count;
				}
				return num;
			}
		}

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x06000118 RID: 280 RVA: 0x00005DE8 File Offset: 0x00004DE8
		public virtual object SyncRoot
		{
			get
			{
				return this;
			}
		}

		// Token: 0x1700003B RID: 59
		// (get) Token: 0x06000119 RID: 281 RVA: 0x00005DEB File Offset: 0x00004DEB
		public virtual bool IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600011A RID: 282 RVA: 0x00005DEE File Offset: 0x00004DEE
		IEnumerator IEnumerable.GetEnumerator()
		{
			return new DictionaryEnumeratorByKeys(this);
		}

		// Token: 0x040000D6 RID: 214
		private ICollection _dictionaries;
	}
}
