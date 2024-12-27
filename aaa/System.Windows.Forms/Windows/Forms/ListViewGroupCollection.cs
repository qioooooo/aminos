using System;
using System.Collections;
using System.ComponentModel;
using System.Globalization;

namespace System.Windows.Forms
{
	// Token: 0x0200048C RID: 1164
	[ListBindable(false)]
	public class ListViewGroupCollection : IList, ICollection, IEnumerable
	{
		// Token: 0x06004581 RID: 17793 RVA: 0x000FCC63 File Offset: 0x000FBC63
		internal ListViewGroupCollection(ListView listView)
		{
			this.listView = listView;
		}

		// Token: 0x17000DB5 RID: 3509
		// (get) Token: 0x06004582 RID: 17794 RVA: 0x000FCC72 File Offset: 0x000FBC72
		public int Count
		{
			get
			{
				return this.List.Count;
			}
		}

		// Token: 0x17000DB6 RID: 3510
		// (get) Token: 0x06004583 RID: 17795 RVA: 0x000FCC7F File Offset: 0x000FBC7F
		object ICollection.SyncRoot
		{
			get
			{
				return this;
			}
		}

		// Token: 0x17000DB7 RID: 3511
		// (get) Token: 0x06004584 RID: 17796 RVA: 0x000FCC82 File Offset: 0x000FBC82
		bool ICollection.IsSynchronized
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000DB8 RID: 3512
		// (get) Token: 0x06004585 RID: 17797 RVA: 0x000FCC85 File Offset: 0x000FBC85
		bool IList.IsFixedSize
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000DB9 RID: 3513
		// (get) Token: 0x06004586 RID: 17798 RVA: 0x000FCC88 File Offset: 0x000FBC88
		bool IList.IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000DBA RID: 3514
		// (get) Token: 0x06004587 RID: 17799 RVA: 0x000FCC8B File Offset: 0x000FBC8B
		private ArrayList List
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

		// Token: 0x17000DBB RID: 3515
		public ListViewGroup this[int index]
		{
			get
			{
				return (ListViewGroup)this.List[index];
			}
			set
			{
				if (this.List.Contains(value))
				{
					return;
				}
				this.List[index] = value;
			}
		}

		// Token: 0x17000DBC RID: 3516
		public ListViewGroup this[string key]
		{
			get
			{
				if (this.list == null)
				{
					return null;
				}
				for (int i = 0; i < this.list.Count; i++)
				{
					if (string.Compare(key, this[i].Name, false, CultureInfo.CurrentCulture) == 0)
					{
						return this[i];
					}
				}
				return null;
			}
			set
			{
				int num = -1;
				if (this.list == null)
				{
					return;
				}
				for (int i = 0; i < this.list.Count; i++)
				{
					if (string.Compare(key, this[i].Name, false, CultureInfo.CurrentCulture) == 0)
					{
						num = i;
						break;
					}
				}
				if (num != -1)
				{
					this.list[num] = value;
				}
			}
		}

		// Token: 0x17000DBD RID: 3517
		object IList.this[int index]
		{
			get
			{
				return this[index];
			}
			set
			{
				if (value is ListViewGroup)
				{
					this[index] = (ListViewGroup)value;
				}
			}
		}

		// Token: 0x0600458E RID: 17806 RVA: 0x000FCDA8 File Offset: 0x000FBDA8
		public int Add(ListViewGroup group)
		{
			if (this.Contains(group))
			{
				return -1;
			}
			this.CheckListViewItems(group);
			group.ListViewInternal = this.listView;
			int num = this.List.Add(group);
			if (this.listView.IsHandleCreated)
			{
				this.listView.InsertGroupInListView(this.List.Count, group);
				this.MoveGroupItems(group);
			}
			return num;
		}

		// Token: 0x0600458F RID: 17807 RVA: 0x000FCE0C File Offset: 0x000FBE0C
		public ListViewGroup Add(string key, string headerText)
		{
			ListViewGroup listViewGroup = new ListViewGroup(key, headerText);
			this.Add(listViewGroup);
			return listViewGroup;
		}

		// Token: 0x06004590 RID: 17808 RVA: 0x000FCE2A File Offset: 0x000FBE2A
		int IList.Add(object value)
		{
			if (value is ListViewGroup)
			{
				return this.Add((ListViewGroup)value);
			}
			throw new ArgumentException("value");
		}

		// Token: 0x06004591 RID: 17809 RVA: 0x000FCE4C File Offset: 0x000FBE4C
		public void AddRange(ListViewGroup[] groups)
		{
			for (int i = 0; i < groups.Length; i++)
			{
				this.Add(groups[i]);
			}
		}

		// Token: 0x06004592 RID: 17810 RVA: 0x000FCE74 File Offset: 0x000FBE74
		public void AddRange(ListViewGroupCollection groups)
		{
			for (int i = 0; i < groups.Count; i++)
			{
				this.Add(groups[i]);
			}
		}

		// Token: 0x06004593 RID: 17811 RVA: 0x000FCEA0 File Offset: 0x000FBEA0
		private void CheckListViewItems(ListViewGroup group)
		{
			for (int i = 0; i < group.Items.Count; i++)
			{
				ListViewItem listViewItem = group.Items[i];
				if (listViewItem.ListView != null && listViewItem.ListView != this.listView)
				{
					throw new ArgumentException(SR.GetString("OnlyOneControl", new object[] { listViewItem.Text }));
				}
			}
		}

		// Token: 0x06004594 RID: 17812 RVA: 0x000FCF08 File Offset: 0x000FBF08
		public void Clear()
		{
			if (this.listView.IsHandleCreated)
			{
				for (int i = 0; i < this.Count; i++)
				{
					this.listView.RemoveGroupFromListView(this[i]);
				}
			}
			for (int j = 0; j < this.Count; j++)
			{
				this[j].ListViewInternal = null;
			}
			this.List.Clear();
			this.listView.UpdateGroupView();
		}

		// Token: 0x06004595 RID: 17813 RVA: 0x000FCF79 File Offset: 0x000FBF79
		public bool Contains(ListViewGroup value)
		{
			return this.List.Contains(value);
		}

		// Token: 0x06004596 RID: 17814 RVA: 0x000FCF87 File Offset: 0x000FBF87
		bool IList.Contains(object value)
		{
			return value is ListViewGroup && this.Contains((ListViewGroup)value);
		}

		// Token: 0x06004597 RID: 17815 RVA: 0x000FCF9F File Offset: 0x000FBF9F
		public void CopyTo(Array array, int index)
		{
			this.List.CopyTo(array, index);
		}

		// Token: 0x06004598 RID: 17816 RVA: 0x000FCFAE File Offset: 0x000FBFAE
		public IEnumerator GetEnumerator()
		{
			return this.List.GetEnumerator();
		}

		// Token: 0x06004599 RID: 17817 RVA: 0x000FCFBB File Offset: 0x000FBFBB
		public int IndexOf(ListViewGroup value)
		{
			return this.List.IndexOf(value);
		}

		// Token: 0x0600459A RID: 17818 RVA: 0x000FCFC9 File Offset: 0x000FBFC9
		int IList.IndexOf(object value)
		{
			if (value is ListViewGroup)
			{
				return this.IndexOf((ListViewGroup)value);
			}
			return -1;
		}

		// Token: 0x0600459B RID: 17819 RVA: 0x000FCFE4 File Offset: 0x000FBFE4
		public void Insert(int index, ListViewGroup group)
		{
			if (this.Contains(group))
			{
				return;
			}
			group.ListViewInternal = this.listView;
			this.List.Insert(index, group);
			if (this.listView.IsHandleCreated)
			{
				this.listView.InsertGroupInListView(index, group);
				this.MoveGroupItems(group);
			}
		}

		// Token: 0x0600459C RID: 17820 RVA: 0x000FD035 File Offset: 0x000FC035
		void IList.Insert(int index, object value)
		{
			if (value is ListViewGroup)
			{
				this.Insert(index, (ListViewGroup)value);
			}
		}

		// Token: 0x0600459D RID: 17821 RVA: 0x000FD04C File Offset: 0x000FC04C
		private void MoveGroupItems(ListViewGroup group)
		{
			foreach (object obj in group.Items)
			{
				ListViewItem listViewItem = (ListViewItem)obj;
				if (listViewItem.ListView == this.listView)
				{
					listViewItem.UpdateStateToListView(listViewItem.Index);
				}
			}
		}

		// Token: 0x0600459E RID: 17822 RVA: 0x000FD0B8 File Offset: 0x000FC0B8
		public void Remove(ListViewGroup group)
		{
			group.ListViewInternal = null;
			this.List.Remove(group);
			if (this.listView.IsHandleCreated)
			{
				this.listView.RemoveGroupFromListView(group);
			}
		}

		// Token: 0x0600459F RID: 17823 RVA: 0x000FD0E6 File Offset: 0x000FC0E6
		void IList.Remove(object value)
		{
			if (value is ListViewGroup)
			{
				this.Remove((ListViewGroup)value);
			}
		}

		// Token: 0x060045A0 RID: 17824 RVA: 0x000FD0FC File Offset: 0x000FC0FC
		public void RemoveAt(int index)
		{
			this.Remove(this[index]);
		}

		// Token: 0x04002166 RID: 8550
		private ListView listView;

		// Token: 0x04002167 RID: 8551
		private ArrayList list;
	}
}
