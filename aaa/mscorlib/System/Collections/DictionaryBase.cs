using System;
using System.Runtime.InteropServices;

namespace System.Collections
{
	// Token: 0x0200024B RID: 587
	[ComVisible(true)]
	[Serializable]
	public abstract class DictionaryBase : IDictionary, ICollection, IEnumerable
	{
		// Token: 0x1700034A RID: 842
		// (get) Token: 0x0600177C RID: 6012 RVA: 0x0003CC0D File Offset: 0x0003BC0D
		protected Hashtable InnerHashtable
		{
			get
			{
				if (this.hashtable == null)
				{
					this.hashtable = new Hashtable();
				}
				return this.hashtable;
			}
		}

		// Token: 0x1700034B RID: 843
		// (get) Token: 0x0600177D RID: 6013 RVA: 0x0003CC28 File Offset: 0x0003BC28
		protected IDictionary Dictionary
		{
			get
			{
				return this;
			}
		}

		// Token: 0x1700034C RID: 844
		// (get) Token: 0x0600177E RID: 6014 RVA: 0x0003CC2B File Offset: 0x0003BC2B
		public int Count
		{
			get
			{
				if (this.hashtable != null)
				{
					return this.hashtable.Count;
				}
				return 0;
			}
		}

		// Token: 0x1700034D RID: 845
		// (get) Token: 0x0600177F RID: 6015 RVA: 0x0003CC42 File Offset: 0x0003BC42
		bool IDictionary.IsReadOnly
		{
			get
			{
				return this.InnerHashtable.IsReadOnly;
			}
		}

		// Token: 0x1700034E RID: 846
		// (get) Token: 0x06001780 RID: 6016 RVA: 0x0003CC4F File Offset: 0x0003BC4F
		bool IDictionary.IsFixedSize
		{
			get
			{
				return this.InnerHashtable.IsFixedSize;
			}
		}

		// Token: 0x1700034F RID: 847
		// (get) Token: 0x06001781 RID: 6017 RVA: 0x0003CC5C File Offset: 0x0003BC5C
		bool ICollection.IsSynchronized
		{
			get
			{
				return this.InnerHashtable.IsSynchronized;
			}
		}

		// Token: 0x17000350 RID: 848
		// (get) Token: 0x06001782 RID: 6018 RVA: 0x0003CC69 File Offset: 0x0003BC69
		ICollection IDictionary.Keys
		{
			get
			{
				return this.InnerHashtable.Keys;
			}
		}

		// Token: 0x17000351 RID: 849
		// (get) Token: 0x06001783 RID: 6019 RVA: 0x0003CC76 File Offset: 0x0003BC76
		object ICollection.SyncRoot
		{
			get
			{
				return this.InnerHashtable.SyncRoot;
			}
		}

		// Token: 0x17000352 RID: 850
		// (get) Token: 0x06001784 RID: 6020 RVA: 0x0003CC83 File Offset: 0x0003BC83
		ICollection IDictionary.Values
		{
			get
			{
				return this.InnerHashtable.Values;
			}
		}

		// Token: 0x06001785 RID: 6021 RVA: 0x0003CC90 File Offset: 0x0003BC90
		public void CopyTo(Array array, int index)
		{
			this.InnerHashtable.CopyTo(array, index);
		}

		// Token: 0x17000353 RID: 851
		object IDictionary.this[object key]
		{
			get
			{
				object obj = this.InnerHashtable[key];
				this.OnGet(key, obj);
				return obj;
			}
			set
			{
				this.OnValidate(key, value);
				bool flag = true;
				object obj = this.InnerHashtable[key];
				if (obj == null)
				{
					flag = this.InnerHashtable.Contains(key);
				}
				this.OnSet(key, obj, value);
				this.InnerHashtable[key] = value;
				try
				{
					this.OnSetComplete(key, obj, value);
				}
				catch
				{
					if (flag)
					{
						this.InnerHashtable[key] = obj;
					}
					else
					{
						this.InnerHashtable.Remove(key);
					}
					throw;
				}
			}
		}

		// Token: 0x06001788 RID: 6024 RVA: 0x0003CD4C File Offset: 0x0003BD4C
		bool IDictionary.Contains(object key)
		{
			return this.InnerHashtable.Contains(key);
		}

		// Token: 0x06001789 RID: 6025 RVA: 0x0003CD5C File Offset: 0x0003BD5C
		void IDictionary.Add(object key, object value)
		{
			this.OnValidate(key, value);
			this.OnInsert(key, value);
			this.InnerHashtable.Add(key, value);
			try
			{
				this.OnInsertComplete(key, value);
			}
			catch
			{
				this.InnerHashtable.Remove(key);
				throw;
			}
		}

		// Token: 0x0600178A RID: 6026 RVA: 0x0003CDB0 File Offset: 0x0003BDB0
		public void Clear()
		{
			this.OnClear();
			this.InnerHashtable.Clear();
			this.OnClearComplete();
		}

		// Token: 0x0600178B RID: 6027 RVA: 0x0003CDCC File Offset: 0x0003BDCC
		void IDictionary.Remove(object key)
		{
			if (this.InnerHashtable.Contains(key))
			{
				object obj = this.InnerHashtable[key];
				this.OnValidate(key, obj);
				this.OnRemove(key, obj);
				this.InnerHashtable.Remove(key);
				try
				{
					this.OnRemoveComplete(key, obj);
				}
				catch
				{
					this.InnerHashtable.Add(key, obj);
					throw;
				}
			}
		}

		// Token: 0x0600178C RID: 6028 RVA: 0x0003CE3C File Offset: 0x0003BE3C
		public IDictionaryEnumerator GetEnumerator()
		{
			return this.InnerHashtable.GetEnumerator();
		}

		// Token: 0x0600178D RID: 6029 RVA: 0x0003CE49 File Offset: 0x0003BE49
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.InnerHashtable.GetEnumerator();
		}

		// Token: 0x0600178E RID: 6030 RVA: 0x0003CE56 File Offset: 0x0003BE56
		protected virtual object OnGet(object key, object currentValue)
		{
			return currentValue;
		}

		// Token: 0x0600178F RID: 6031 RVA: 0x0003CE59 File Offset: 0x0003BE59
		protected virtual void OnSet(object key, object oldValue, object newValue)
		{
		}

		// Token: 0x06001790 RID: 6032 RVA: 0x0003CE5B File Offset: 0x0003BE5B
		protected virtual void OnInsert(object key, object value)
		{
		}

		// Token: 0x06001791 RID: 6033 RVA: 0x0003CE5D File Offset: 0x0003BE5D
		protected virtual void OnClear()
		{
		}

		// Token: 0x06001792 RID: 6034 RVA: 0x0003CE5F File Offset: 0x0003BE5F
		protected virtual void OnRemove(object key, object value)
		{
		}

		// Token: 0x06001793 RID: 6035 RVA: 0x0003CE61 File Offset: 0x0003BE61
		protected virtual void OnValidate(object key, object value)
		{
		}

		// Token: 0x06001794 RID: 6036 RVA: 0x0003CE63 File Offset: 0x0003BE63
		protected virtual void OnSetComplete(object key, object oldValue, object newValue)
		{
		}

		// Token: 0x06001795 RID: 6037 RVA: 0x0003CE65 File Offset: 0x0003BE65
		protected virtual void OnInsertComplete(object key, object value)
		{
		}

		// Token: 0x06001796 RID: 6038 RVA: 0x0003CE67 File Offset: 0x0003BE67
		protected virtual void OnClearComplete()
		{
		}

		// Token: 0x06001797 RID: 6039 RVA: 0x0003CE69 File Offset: 0x0003BE69
		protected virtual void OnRemoveComplete(object key, object value)
		{
		}

		// Token: 0x0400093D RID: 2365
		private Hashtable hashtable;
	}
}
