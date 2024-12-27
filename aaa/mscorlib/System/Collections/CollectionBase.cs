using System;
using System.Runtime.InteropServices;

namespace System.Collections
{
	// Token: 0x02000248 RID: 584
	[ComVisible(true)]
	[Serializable]
	public abstract class CollectionBase : IList, ICollection, IEnumerable
	{
		// Token: 0x0600174C RID: 5964 RVA: 0x0003C70C File Offset: 0x0003B70C
		protected CollectionBase()
		{
			this.list = new ArrayList();
		}

		// Token: 0x0600174D RID: 5965 RVA: 0x0003C71F File Offset: 0x0003B71F
		protected CollectionBase(int capacity)
		{
			this.list = new ArrayList(capacity);
		}

		// Token: 0x1700033C RID: 828
		// (get) Token: 0x0600174E RID: 5966 RVA: 0x0003C733 File Offset: 0x0003B733
		protected ArrayList InnerList
		{
			get
			{
				if (this.list == null)
				{
					this.list = new ArrayList();
				}
				return this.list;
			}
		}

		// Token: 0x1700033D RID: 829
		// (get) Token: 0x0600174F RID: 5967 RVA: 0x0003C74E File Offset: 0x0003B74E
		protected IList List
		{
			get
			{
				return this;
			}
		}

		// Token: 0x1700033E RID: 830
		// (get) Token: 0x06001750 RID: 5968 RVA: 0x0003C751 File Offset: 0x0003B751
		// (set) Token: 0x06001751 RID: 5969 RVA: 0x0003C75E File Offset: 0x0003B75E
		[ComVisible(false)]
		public int Capacity
		{
			get
			{
				return this.InnerList.Capacity;
			}
			set
			{
				this.InnerList.Capacity = value;
			}
		}

		// Token: 0x1700033F RID: 831
		// (get) Token: 0x06001752 RID: 5970 RVA: 0x0003C76C File Offset: 0x0003B76C
		public int Count
		{
			get
			{
				if (this.list != null)
				{
					return this.list.Count;
				}
				return 0;
			}
		}

		// Token: 0x06001753 RID: 5971 RVA: 0x0003C783 File Offset: 0x0003B783
		public void Clear()
		{
			this.OnClear();
			this.InnerList.Clear();
			this.OnClearComplete();
		}

		// Token: 0x06001754 RID: 5972 RVA: 0x0003C79C File Offset: 0x0003B79C
		public void RemoveAt(int index)
		{
			if (index < 0 || index >= this.InnerList.Count)
			{
				throw new ArgumentOutOfRangeException("index", Environment.GetResourceString("ArgumentOutOfRange_Index"));
			}
			object obj = this.InnerList[index];
			this.OnValidate(obj);
			this.OnRemove(index, obj);
			this.InnerList.RemoveAt(index);
			try
			{
				this.OnRemoveComplete(index, obj);
			}
			catch
			{
				this.InnerList.Insert(index, obj);
				throw;
			}
		}

		// Token: 0x17000340 RID: 832
		// (get) Token: 0x06001755 RID: 5973 RVA: 0x0003C824 File Offset: 0x0003B824
		bool IList.IsReadOnly
		{
			get
			{
				return this.InnerList.IsReadOnly;
			}
		}

		// Token: 0x17000341 RID: 833
		// (get) Token: 0x06001756 RID: 5974 RVA: 0x0003C831 File Offset: 0x0003B831
		bool IList.IsFixedSize
		{
			get
			{
				return this.InnerList.IsFixedSize;
			}
		}

		// Token: 0x17000342 RID: 834
		// (get) Token: 0x06001757 RID: 5975 RVA: 0x0003C83E File Offset: 0x0003B83E
		bool ICollection.IsSynchronized
		{
			get
			{
				return this.InnerList.IsSynchronized;
			}
		}

		// Token: 0x17000343 RID: 835
		// (get) Token: 0x06001758 RID: 5976 RVA: 0x0003C84B File Offset: 0x0003B84B
		object ICollection.SyncRoot
		{
			get
			{
				return this.InnerList.SyncRoot;
			}
		}

		// Token: 0x06001759 RID: 5977 RVA: 0x0003C858 File Offset: 0x0003B858
		void ICollection.CopyTo(Array array, int index)
		{
			this.InnerList.CopyTo(array, index);
		}

		// Token: 0x17000344 RID: 836
		object IList.this[int index]
		{
			get
			{
				if (index < 0 || index >= this.InnerList.Count)
				{
					throw new ArgumentOutOfRangeException("index", Environment.GetResourceString("ArgumentOutOfRange_Index"));
				}
				return this.InnerList[index];
			}
			set
			{
				if (index < 0 || index >= this.InnerList.Count)
				{
					throw new ArgumentOutOfRangeException("index", Environment.GetResourceString("ArgumentOutOfRange_Index"));
				}
				this.OnValidate(value);
				object obj = this.InnerList[index];
				this.OnSet(index, obj, value);
				this.InnerList[index] = value;
				try
				{
					this.OnSetComplete(index, obj, value);
				}
				catch
				{
					this.InnerList[index] = obj;
					throw;
				}
			}
		}

		// Token: 0x0600175C RID: 5980 RVA: 0x0003C928 File Offset: 0x0003B928
		bool IList.Contains(object value)
		{
			return this.InnerList.Contains(value);
		}

		// Token: 0x0600175D RID: 5981 RVA: 0x0003C938 File Offset: 0x0003B938
		int IList.Add(object value)
		{
			this.OnValidate(value);
			this.OnInsert(this.InnerList.Count, value);
			int num = this.InnerList.Add(value);
			try
			{
				this.OnInsertComplete(num, value);
			}
			catch
			{
				this.InnerList.RemoveAt(num);
				throw;
			}
			return num;
		}

		// Token: 0x0600175E RID: 5982 RVA: 0x0003C998 File Offset: 0x0003B998
		void IList.Remove(object value)
		{
			this.OnValidate(value);
			int num = this.InnerList.IndexOf(value);
			if (num < 0)
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_RemoveArgNotFound"));
			}
			this.OnRemove(num, value);
			this.InnerList.RemoveAt(num);
			try
			{
				this.OnRemoveComplete(num, value);
			}
			catch
			{
				this.InnerList.Insert(num, value);
				throw;
			}
		}

		// Token: 0x0600175F RID: 5983 RVA: 0x0003CA0C File Offset: 0x0003BA0C
		int IList.IndexOf(object value)
		{
			return this.InnerList.IndexOf(value);
		}

		// Token: 0x06001760 RID: 5984 RVA: 0x0003CA1C File Offset: 0x0003BA1C
		void IList.Insert(int index, object value)
		{
			if (index < 0 || index > this.InnerList.Count)
			{
				throw new ArgumentOutOfRangeException("index", Environment.GetResourceString("ArgumentOutOfRange_Index"));
			}
			this.OnValidate(value);
			this.OnInsert(index, value);
			this.InnerList.Insert(index, value);
			try
			{
				this.OnInsertComplete(index, value);
			}
			catch
			{
				this.InnerList.RemoveAt(index);
				throw;
			}
		}

		// Token: 0x06001761 RID: 5985 RVA: 0x0003CA98 File Offset: 0x0003BA98
		public IEnumerator GetEnumerator()
		{
			return this.InnerList.GetEnumerator();
		}

		// Token: 0x06001762 RID: 5986 RVA: 0x0003CAA5 File Offset: 0x0003BAA5
		protected virtual void OnSet(int index, object oldValue, object newValue)
		{
		}

		// Token: 0x06001763 RID: 5987 RVA: 0x0003CAA7 File Offset: 0x0003BAA7
		protected virtual void OnInsert(int index, object value)
		{
		}

		// Token: 0x06001764 RID: 5988 RVA: 0x0003CAA9 File Offset: 0x0003BAA9
		protected virtual void OnClear()
		{
		}

		// Token: 0x06001765 RID: 5989 RVA: 0x0003CAAB File Offset: 0x0003BAAB
		protected virtual void OnRemove(int index, object value)
		{
		}

		// Token: 0x06001766 RID: 5990 RVA: 0x0003CAAD File Offset: 0x0003BAAD
		protected virtual void OnValidate(object value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
		}

		// Token: 0x06001767 RID: 5991 RVA: 0x0003CABD File Offset: 0x0003BABD
		protected virtual void OnSetComplete(int index, object oldValue, object newValue)
		{
		}

		// Token: 0x06001768 RID: 5992 RVA: 0x0003CABF File Offset: 0x0003BABF
		protected virtual void OnInsertComplete(int index, object value)
		{
		}

		// Token: 0x06001769 RID: 5993 RVA: 0x0003CAC1 File Offset: 0x0003BAC1
		protected virtual void OnClearComplete()
		{
		}

		// Token: 0x0600176A RID: 5994 RVA: 0x0003CAC3 File Offset: 0x0003BAC3
		protected virtual void OnRemoveComplete(int index, object value)
		{
		}

		// Token: 0x04000938 RID: 2360
		private ArrayList list;
	}
}
