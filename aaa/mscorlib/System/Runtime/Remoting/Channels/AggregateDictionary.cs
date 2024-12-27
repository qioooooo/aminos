using System;
using System.Collections;

namespace System.Runtime.Remoting.Channels
{
	// Token: 0x020006DF RID: 1759
	internal class AggregateDictionary : IDictionary, ICollection, IEnumerable
	{
		// Token: 0x06003F31 RID: 16177 RVA: 0x000D85A7 File Offset: 0x000D75A7
		public AggregateDictionary(ICollection dictionaries)
		{
			this._dictionaries = dictionaries;
		}

		// Token: 0x17000ABC RID: 2748
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

		// Token: 0x17000ABD RID: 2749
		// (get) Token: 0x06003F34 RID: 16180 RVA: 0x000D8684 File Offset: 0x000D7684
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

		// Token: 0x17000ABE RID: 2750
		// (get) Token: 0x06003F35 RID: 16181 RVA: 0x000D8734 File Offset: 0x000D7734
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

		// Token: 0x06003F36 RID: 16182 RVA: 0x000D87E4 File Offset: 0x000D77E4
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

		// Token: 0x17000ABF RID: 2751
		// (get) Token: 0x06003F37 RID: 16183 RVA: 0x000D8848 File Offset: 0x000D7848
		public virtual bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000AC0 RID: 2752
		// (get) Token: 0x06003F38 RID: 16184 RVA: 0x000D884B File Offset: 0x000D784B
		public virtual bool IsFixedSize
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06003F39 RID: 16185 RVA: 0x000D884E File Offset: 0x000D784E
		public virtual void Add(object key, object value)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06003F3A RID: 16186 RVA: 0x000D8855 File Offset: 0x000D7855
		public virtual void Clear()
		{
			throw new NotSupportedException();
		}

		// Token: 0x06003F3B RID: 16187 RVA: 0x000D885C File Offset: 0x000D785C
		public virtual void Remove(object key)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06003F3C RID: 16188 RVA: 0x000D8863 File Offset: 0x000D7863
		public virtual IDictionaryEnumerator GetEnumerator()
		{
			return new DictionaryEnumeratorByKeys(this);
		}

		// Token: 0x06003F3D RID: 16189 RVA: 0x000D886B File Offset: 0x000D786B
		public virtual void CopyTo(Array array, int index)
		{
			throw new NotSupportedException();
		}

		// Token: 0x17000AC1 RID: 2753
		// (get) Token: 0x06003F3E RID: 16190 RVA: 0x000D8874 File Offset: 0x000D7874
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

		// Token: 0x17000AC2 RID: 2754
		// (get) Token: 0x06003F3F RID: 16191 RVA: 0x000D88D4 File Offset: 0x000D78D4
		public virtual object SyncRoot
		{
			get
			{
				return this;
			}
		}

		// Token: 0x17000AC3 RID: 2755
		// (get) Token: 0x06003F40 RID: 16192 RVA: 0x000D88D7 File Offset: 0x000D78D7
		public virtual bool IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06003F41 RID: 16193 RVA: 0x000D88DA File Offset: 0x000D78DA
		IEnumerator IEnumerable.GetEnumerator()
		{
			return new DictionaryEnumeratorByKeys(this);
		}

		// Token: 0x04001FD1 RID: 8145
		private ICollection _dictionaries;
	}
}
