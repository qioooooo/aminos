using System;

namespace System.Collections.Specialized
{
	// Token: 0x0200024F RID: 591
	[Serializable]
	public class HybridDictionary : IDictionary, ICollection, IEnumerable
	{
		// Token: 0x06001440 RID: 5184 RVA: 0x000434FC File Offset: 0x000424FC
		public HybridDictionary()
		{
		}

		// Token: 0x06001441 RID: 5185 RVA: 0x00043504 File Offset: 0x00042504
		public HybridDictionary(int initialSize)
			: this(initialSize, false)
		{
		}

		// Token: 0x06001442 RID: 5186 RVA: 0x0004350E File Offset: 0x0004250E
		public HybridDictionary(bool caseInsensitive)
		{
			this.caseInsensitive = caseInsensitive;
		}

		// Token: 0x06001443 RID: 5187 RVA: 0x0004351D File Offset: 0x0004251D
		public HybridDictionary(int initialSize, bool caseInsensitive)
		{
			this.caseInsensitive = caseInsensitive;
			if (initialSize >= 6)
			{
				if (caseInsensitive)
				{
					this.hashtable = new Hashtable(initialSize, StringComparer.OrdinalIgnoreCase);
					return;
				}
				this.hashtable = new Hashtable(initialSize);
			}
		}

		// Token: 0x17000424 RID: 1060
		public object this[object key]
		{
			get
			{
				ListDictionary listDictionary = this.list;
				if (this.hashtable != null)
				{
					return this.hashtable[key];
				}
				if (listDictionary != null)
				{
					return listDictionary[key];
				}
				if (key == null)
				{
					throw new ArgumentNullException("key", SR.GetString("ArgumentNull_Key"));
				}
				return null;
			}
			set
			{
				if (this.hashtable != null)
				{
					this.hashtable[key] = value;
					return;
				}
				if (this.list == null)
				{
					this.list = new ListDictionary(this.caseInsensitive ? StringComparer.OrdinalIgnoreCase : null);
					this.list[key] = value;
					return;
				}
				if (this.list.Count >= 8)
				{
					this.ChangeOver();
					this.hashtable[key] = value;
					return;
				}
				this.list[key] = value;
			}
		}

		// Token: 0x17000425 RID: 1061
		// (get) Token: 0x06001446 RID: 5190 RVA: 0x00043627 File Offset: 0x00042627
		private ListDictionary List
		{
			get
			{
				if (this.list == null)
				{
					this.list = new ListDictionary(this.caseInsensitive ? StringComparer.OrdinalIgnoreCase : null);
				}
				return this.list;
			}
		}

		// Token: 0x06001447 RID: 5191 RVA: 0x00043654 File Offset: 0x00042654
		private void ChangeOver()
		{
			IDictionaryEnumerator enumerator = this.list.GetEnumerator();
			Hashtable hashtable;
			if (this.caseInsensitive)
			{
				hashtable = new Hashtable(13, StringComparer.OrdinalIgnoreCase);
			}
			else
			{
				hashtable = new Hashtable(13);
			}
			while (enumerator.MoveNext())
			{
				hashtable.Add(enumerator.Key, enumerator.Value);
			}
			this.hashtable = hashtable;
			this.list = null;
		}

		// Token: 0x17000426 RID: 1062
		// (get) Token: 0x06001448 RID: 5192 RVA: 0x000436B8 File Offset: 0x000426B8
		public int Count
		{
			get
			{
				ListDictionary listDictionary = this.list;
				if (this.hashtable != null)
				{
					return this.hashtable.Count;
				}
				if (listDictionary != null)
				{
					return listDictionary.Count;
				}
				return 0;
			}
		}

		// Token: 0x17000427 RID: 1063
		// (get) Token: 0x06001449 RID: 5193 RVA: 0x000436EB File Offset: 0x000426EB
		public ICollection Keys
		{
			get
			{
				if (this.hashtable != null)
				{
					return this.hashtable.Keys;
				}
				return this.List.Keys;
			}
		}

		// Token: 0x17000428 RID: 1064
		// (get) Token: 0x0600144A RID: 5194 RVA: 0x0004370C File Offset: 0x0004270C
		public bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000429 RID: 1065
		// (get) Token: 0x0600144B RID: 5195 RVA: 0x0004370F File Offset: 0x0004270F
		public bool IsFixedSize
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700042A RID: 1066
		// (get) Token: 0x0600144C RID: 5196 RVA: 0x00043712 File Offset: 0x00042712
		public bool IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700042B RID: 1067
		// (get) Token: 0x0600144D RID: 5197 RVA: 0x00043715 File Offset: 0x00042715
		public object SyncRoot
		{
			get
			{
				return this;
			}
		}

		// Token: 0x1700042C RID: 1068
		// (get) Token: 0x0600144E RID: 5198 RVA: 0x00043718 File Offset: 0x00042718
		public ICollection Values
		{
			get
			{
				if (this.hashtable != null)
				{
					return this.hashtable.Values;
				}
				return this.List.Values;
			}
		}

		// Token: 0x0600144F RID: 5199 RVA: 0x0004373C File Offset: 0x0004273C
		public void Add(object key, object value)
		{
			if (this.hashtable != null)
			{
				this.hashtable.Add(key, value);
				return;
			}
			if (this.list == null)
			{
				this.list = new ListDictionary(this.caseInsensitive ? StringComparer.OrdinalIgnoreCase : null);
				this.list.Add(key, value);
				return;
			}
			if (this.list.Count + 1 >= 9)
			{
				this.ChangeOver();
				this.hashtable.Add(key, value);
				return;
			}
			this.list.Add(key, value);
		}

		// Token: 0x06001450 RID: 5200 RVA: 0x000437C4 File Offset: 0x000427C4
		public void Clear()
		{
			if (this.hashtable != null)
			{
				Hashtable hashtable = this.hashtable;
				this.hashtable = null;
				hashtable.Clear();
			}
			if (this.list != null)
			{
				ListDictionary listDictionary = this.list;
				this.list = null;
				listDictionary.Clear();
			}
		}

		// Token: 0x06001451 RID: 5201 RVA: 0x0004380C File Offset: 0x0004280C
		public bool Contains(object key)
		{
			ListDictionary listDictionary = this.list;
			if (this.hashtable != null)
			{
				return this.hashtable.Contains(key);
			}
			if (listDictionary != null)
			{
				return listDictionary.Contains(key);
			}
			if (key == null)
			{
				throw new ArgumentNullException("key", SR.GetString("ArgumentNull_Key"));
			}
			return false;
		}

		// Token: 0x06001452 RID: 5202 RVA: 0x00043859 File Offset: 0x00042859
		public void CopyTo(Array array, int index)
		{
			if (this.hashtable != null)
			{
				this.hashtable.CopyTo(array, index);
				return;
			}
			this.List.CopyTo(array, index);
		}

		// Token: 0x06001453 RID: 5203 RVA: 0x00043880 File Offset: 0x00042880
		public IDictionaryEnumerator GetEnumerator()
		{
			if (this.hashtable != null)
			{
				return this.hashtable.GetEnumerator();
			}
			if (this.list == null)
			{
				this.list = new ListDictionary(this.caseInsensitive ? StringComparer.OrdinalIgnoreCase : null);
			}
			return this.list.GetEnumerator();
		}

		// Token: 0x06001454 RID: 5204 RVA: 0x000438D0 File Offset: 0x000428D0
		IEnumerator IEnumerable.GetEnumerator()
		{
			if (this.hashtable != null)
			{
				return this.hashtable.GetEnumerator();
			}
			if (this.list == null)
			{
				this.list = new ListDictionary(this.caseInsensitive ? StringComparer.OrdinalIgnoreCase : null);
			}
			return this.list.GetEnumerator();
		}

		// Token: 0x06001455 RID: 5205 RVA: 0x00043920 File Offset: 0x00042920
		public void Remove(object key)
		{
			if (this.hashtable != null)
			{
				this.hashtable.Remove(key);
				return;
			}
			if (this.list != null)
			{
				this.list.Remove(key);
				return;
			}
			if (key == null)
			{
				throw new ArgumentNullException("key", SR.GetString("ArgumentNull_Key"));
			}
		}

		// Token: 0x0400116E RID: 4462
		private const int CutoverPoint = 9;

		// Token: 0x0400116F RID: 4463
		private const int InitialHashtableSize = 13;

		// Token: 0x04001170 RID: 4464
		private const int FixedSizeCutoverPoint = 6;

		// Token: 0x04001171 RID: 4465
		private ListDictionary list;

		// Token: 0x04001172 RID: 4466
		private Hashtable hashtable;

		// Token: 0x04001173 RID: 4467
		private bool caseInsensitive;
	}
}
