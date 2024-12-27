using System;
using System.Collections;
using System.Design;
using System.Globalization;

namespace System.Web.UI.Design
{
	// Token: 0x0200039B RID: 923
	public sealed class TemplateGroupCollection : IList, ICollection, IEnumerable
	{
		// Token: 0x06002222 RID: 8738 RVA: 0x000BB698 File Offset: 0x000BA698
		public TemplateGroupCollection()
		{
		}

		// Token: 0x06002223 RID: 8739 RVA: 0x000BB6A0 File Offset: 0x000BA6A0
		internal TemplateGroupCollection(TemplateGroup[] verbs)
		{
			for (int i = 0; i < verbs.Length; i++)
			{
				this.Add(verbs[i]);
			}
		}

		// Token: 0x17000645 RID: 1605
		// (get) Token: 0x06002224 RID: 8740 RVA: 0x000BB6CB File Offset: 0x000BA6CB
		public int Count
		{
			get
			{
				return this.InternalList.Count;
			}
		}

		// Token: 0x17000646 RID: 1606
		// (get) Token: 0x06002225 RID: 8741 RVA: 0x000BB6D8 File Offset: 0x000BA6D8
		private ArrayList InternalList
		{
			get
			{
				if (this._list == null)
				{
					this._list = new ArrayList();
				}
				return this._list;
			}
		}

		// Token: 0x17000647 RID: 1607
		public TemplateGroup this[int index]
		{
			get
			{
				return (TemplateGroup)this.InternalList[index];
			}
			set
			{
				this.InternalList[index] = value;
			}
		}

		// Token: 0x06002228 RID: 8744 RVA: 0x000BB715 File Offset: 0x000BA715
		public int Add(TemplateGroup group)
		{
			return this.InternalList.Add(group);
		}

		// Token: 0x06002229 RID: 8745 RVA: 0x000BB723 File Offset: 0x000BA723
		public void AddRange(TemplateGroupCollection groups)
		{
			this.InternalList.AddRange(groups);
		}

		// Token: 0x0600222A RID: 8746 RVA: 0x000BB731 File Offset: 0x000BA731
		public void Clear()
		{
			this.InternalList.Clear();
		}

		// Token: 0x0600222B RID: 8747 RVA: 0x000BB73E File Offset: 0x000BA73E
		public bool Contains(TemplateGroup group)
		{
			return this.InternalList.Contains(group);
		}

		// Token: 0x0600222C RID: 8748 RVA: 0x000BB74C File Offset: 0x000BA74C
		public void CopyTo(TemplateGroup[] array, int index)
		{
			this.InternalList.CopyTo(array, index);
		}

		// Token: 0x0600222D RID: 8749 RVA: 0x000BB75B File Offset: 0x000BA75B
		public int IndexOf(TemplateGroup group)
		{
			return this.InternalList.IndexOf(group);
		}

		// Token: 0x0600222E RID: 8750 RVA: 0x000BB769 File Offset: 0x000BA769
		public void Insert(int index, TemplateGroup group)
		{
			this.InternalList.Insert(index, group);
		}

		// Token: 0x0600222F RID: 8751 RVA: 0x000BB778 File Offset: 0x000BA778
		public void Remove(TemplateGroup group)
		{
			this.InternalList.Remove(group);
		}

		// Token: 0x06002230 RID: 8752 RVA: 0x000BB786 File Offset: 0x000BA786
		public void RemoveAt(int index)
		{
			this.InternalList.RemoveAt(index);
		}

		// Token: 0x17000648 RID: 1608
		// (get) Token: 0x06002231 RID: 8753 RVA: 0x000BB794 File Offset: 0x000BA794
		int ICollection.Count
		{
			get
			{
				return this.Count;
			}
		}

		// Token: 0x17000649 RID: 1609
		// (get) Token: 0x06002232 RID: 8754 RVA: 0x000BB79C File Offset: 0x000BA79C
		bool IList.IsFixedSize
		{
			get
			{
				return this.InternalList.IsFixedSize;
			}
		}

		// Token: 0x1700064A RID: 1610
		// (get) Token: 0x06002233 RID: 8755 RVA: 0x000BB7A9 File Offset: 0x000BA7A9
		bool IList.IsReadOnly
		{
			get
			{
				return this.InternalList.IsReadOnly;
			}
		}

		// Token: 0x1700064B RID: 1611
		// (get) Token: 0x06002234 RID: 8756 RVA: 0x000BB7B6 File Offset: 0x000BA7B6
		bool ICollection.IsSynchronized
		{
			get
			{
				return this.InternalList.IsSynchronized;
			}
		}

		// Token: 0x1700064C RID: 1612
		// (get) Token: 0x06002235 RID: 8757 RVA: 0x000BB7C3 File Offset: 0x000BA7C3
		object ICollection.SyncRoot
		{
			get
			{
				return this.InternalList.SyncRoot;
			}
		}

		// Token: 0x1700064D RID: 1613
		object IList.this[int index]
		{
			get
			{
				return this[index];
			}
			set
			{
				if (!(value is TemplateGroup))
				{
					throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, SR.GetString("WrongType"), new object[] { "TemplateGroup" }), "value");
				}
				this[index] = (TemplateGroup)value;
			}
		}

		// Token: 0x06002238 RID: 8760 RVA: 0x000BB830 File Offset: 0x000BA830
		int IList.Add(object o)
		{
			if (!(o is TemplateGroup))
			{
				throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, SR.GetString("WrongType"), new object[] { "TemplateGroup" }), "o");
			}
			return this.Add((TemplateGroup)o);
		}

		// Token: 0x06002239 RID: 8761 RVA: 0x000BB880 File Offset: 0x000BA880
		void IList.Clear()
		{
			this.Clear();
		}

		// Token: 0x0600223A RID: 8762 RVA: 0x000BB888 File Offset: 0x000BA888
		bool IList.Contains(object o)
		{
			if (!(o is TemplateGroup))
			{
				throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, SR.GetString("WrongType"), new object[] { "TemplateGroup" }), "o");
			}
			return this.Contains((TemplateGroup)o);
		}

		// Token: 0x0600223B RID: 8763 RVA: 0x000BB8D8 File Offset: 0x000BA8D8
		void ICollection.CopyTo(Array array, int index)
		{
			this.InternalList.CopyTo(array, index);
		}

		// Token: 0x0600223C RID: 8764 RVA: 0x000BB8E7 File Offset: 0x000BA8E7
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.InternalList.GetEnumerator();
		}

		// Token: 0x0600223D RID: 8765 RVA: 0x000BB8F4 File Offset: 0x000BA8F4
		int IList.IndexOf(object o)
		{
			if (!(o is TemplateGroup))
			{
				throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, SR.GetString("WrongType"), new object[] { "TemplateGroup" }), "o");
			}
			return this.IndexOf((TemplateGroup)o);
		}

		// Token: 0x0600223E RID: 8766 RVA: 0x000BB944 File Offset: 0x000BA944
		void IList.Insert(int index, object o)
		{
			if (!(o is TemplateGroup))
			{
				throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, SR.GetString("WrongType"), new object[] { "TemplateGroup" }), "o");
			}
			this.Insert(index, (TemplateGroup)o);
		}

		// Token: 0x0600223F RID: 8767 RVA: 0x000BB998 File Offset: 0x000BA998
		void IList.Remove(object o)
		{
			if (!(o is TemplateGroup))
			{
				throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, SR.GetString("WrongType"), new object[] { "TemplateGroup" }), "o");
			}
			this.Remove((TemplateGroup)o);
		}

		// Token: 0x06002240 RID: 8768 RVA: 0x000BB9E8 File Offset: 0x000BA9E8
		void IList.RemoveAt(int index)
		{
			this.RemoveAt(index);
		}

		// Token: 0x04001847 RID: 6215
		private ArrayList _list;
	}
}
