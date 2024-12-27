using System;
using System.Collections;

namespace System.Text.RegularExpressions
{
	// Token: 0x02000021 RID: 33
	[Serializable]
	public class GroupCollection : ICollection, IEnumerable
	{
		// Token: 0x06000154 RID: 340 RVA: 0x0000B2EC File Offset: 0x0000A2EC
		internal GroupCollection(Match match, Hashtable caps)
		{
			this._match = match;
			this._captureMap = caps;
		}

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x06000155 RID: 341 RVA: 0x0000B302 File Offset: 0x0000A302
		public object SyncRoot
		{
			get
			{
				return this._match;
			}
		}

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x06000156 RID: 342 RVA: 0x0000B30A File Offset: 0x0000A30A
		public bool IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x06000157 RID: 343 RVA: 0x0000B30D File Offset: 0x0000A30D
		public bool IsReadOnly
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x06000158 RID: 344 RVA: 0x0000B310 File Offset: 0x0000A310
		public int Count
		{
			get
			{
				return this._match._matchcount.Length;
			}
		}

		// Token: 0x17000033 RID: 51
		public Group this[int groupnum]
		{
			get
			{
				return this.GetGroup(groupnum);
			}
		}

		// Token: 0x17000034 RID: 52
		public Group this[string groupname]
		{
			get
			{
				if (this._match._regex == null)
				{
					return Group._emptygroup;
				}
				return this.GetGroup(this._match._regex.GroupNumberFromName(groupname));
			}
		}

		// Token: 0x0600015B RID: 347 RVA: 0x0000B354 File Offset: 0x0000A354
		internal Group GetGroup(int groupnum)
		{
			if (this._captureMap != null)
			{
				object obj = this._captureMap[groupnum];
				if (obj == null)
				{
					return Group._emptygroup;
				}
				return this.GetGroupImpl((int)obj);
			}
			else
			{
				if (groupnum >= this._match._matchcount.Length || groupnum < 0)
				{
					return Group._emptygroup;
				}
				return this.GetGroupImpl(groupnum);
			}
		}

		// Token: 0x0600015C RID: 348 RVA: 0x0000B3B4 File Offset: 0x0000A3B4
		internal Group GetGroupImpl(int groupnum)
		{
			if (groupnum == 0)
			{
				return this._match;
			}
			if (this._groups == null)
			{
				this._groups = new Group[this._match._matchcount.Length - 1];
				for (int i = 0; i < this._groups.Length; i++)
				{
					this._groups[i] = new Group(this._match._text, this._match._matches[i + 1], this._match._matchcount[i + 1]);
				}
			}
			return this._groups[groupnum - 1];
		}

		// Token: 0x0600015D RID: 349 RVA: 0x0000B440 File Offset: 0x0000A440
		public void CopyTo(Array array, int arrayIndex)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			int num = arrayIndex;
			for (int i = 0; i < this.Count; i++)
			{
				array.SetValue(this[i], num);
				num++;
			}
		}

		// Token: 0x0600015E RID: 350 RVA: 0x0000B480 File Offset: 0x0000A480
		public IEnumerator GetEnumerator()
		{
			return new GroupEnumerator(this);
		}

		// Token: 0x0400072D RID: 1837
		internal Match _match;

		// Token: 0x0400072E RID: 1838
		internal Hashtable _captureMap;

		// Token: 0x0400072F RID: 1839
		internal Group[] _groups;
	}
}
