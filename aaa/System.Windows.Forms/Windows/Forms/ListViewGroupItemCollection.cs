using System;
using System.Collections;
using System.ComponentModel;

namespace System.Windows.Forms
{
	// Token: 0x0200048E RID: 1166
	internal class ListViewGroupItemCollection : ListView.ListViewItemCollection.IInnerList
	{
		// Token: 0x060045A9 RID: 17833 RVA: 0x000FD39E File Offset: 0x000FC39E
		public ListViewGroupItemCollection(ListViewGroup group)
		{
			this.group = group;
		}

		// Token: 0x17000DBE RID: 3518
		// (get) Token: 0x060045AA RID: 17834 RVA: 0x000FD3AD File Offset: 0x000FC3AD
		public int Count
		{
			get
			{
				return this.Items.Count;
			}
		}

		// Token: 0x17000DBF RID: 3519
		// (get) Token: 0x060045AB RID: 17835 RVA: 0x000FD3BA File Offset: 0x000FC3BA
		private ArrayList Items
		{
			get
			{
				if (this.items == null)
				{
					this.items = new ArrayList();
				}
				return this.items;
			}
		}

		// Token: 0x17000DC0 RID: 3520
		// (get) Token: 0x060045AC RID: 17836 RVA: 0x000FD3D5 File Offset: 0x000FC3D5
		public bool OwnerIsVirtualListView
		{
			get
			{
				return this.group.ListView != null && this.group.ListView.VirtualMode;
			}
		}

		// Token: 0x17000DC1 RID: 3521
		// (get) Token: 0x060045AD RID: 17837 RVA: 0x000FD3F8 File Offset: 0x000FC3F8
		public bool OwnerIsDesignMode
		{
			get
			{
				if (this.group.ListView != null)
				{
					ISite site = this.group.ListView.Site;
					return site != null && site.DesignMode;
				}
				return false;
			}
		}

		// Token: 0x17000DC2 RID: 3522
		public ListViewItem this[int index]
		{
			get
			{
				return (ListViewItem)this.Items[index];
			}
			set
			{
				if (value != this.Items[index])
				{
					this.MoveToGroup((ListViewItem)this.Items[index], null);
					this.Items[index] = value;
					this.MoveToGroup((ListViewItem)this.Items[index], this.group);
				}
			}
		}

		// Token: 0x060045B0 RID: 17840 RVA: 0x000FD4A2 File Offset: 0x000FC4A2
		public ListViewItem Add(ListViewItem value)
		{
			this.CheckListViewItem(value);
			this.MoveToGroup(value, this.group);
			this.Items.Add(value);
			return value;
		}

		// Token: 0x060045B1 RID: 17841 RVA: 0x000FD4C8 File Offset: 0x000FC4C8
		public void AddRange(ListViewItem[] items)
		{
			for (int i = 0; i < items.Length; i++)
			{
				this.CheckListViewItem(items[i]);
			}
			this.Items.AddRange(items);
			for (int j = 0; j < items.Length; j++)
			{
				this.MoveToGroup(items[j], this.group);
			}
		}

		// Token: 0x060045B2 RID: 17842 RVA: 0x000FD518 File Offset: 0x000FC518
		private void CheckListViewItem(ListViewItem item)
		{
			if (item.ListView != null && item.ListView != this.group.ListView)
			{
				throw new ArgumentException(SR.GetString("OnlyOneControl", new object[] { item.Text }), "item");
			}
		}

		// Token: 0x060045B3 RID: 17843 RVA: 0x000FD568 File Offset: 0x000FC568
		public void Clear()
		{
			for (int i = 0; i < this.Count; i++)
			{
				this.MoveToGroup(this[i], null);
			}
			this.Items.Clear();
		}

		// Token: 0x060045B4 RID: 17844 RVA: 0x000FD59F File Offset: 0x000FC59F
		public bool Contains(ListViewItem item)
		{
			return this.Items.Contains(item);
		}

		// Token: 0x060045B5 RID: 17845 RVA: 0x000FD5AD File Offset: 0x000FC5AD
		public void CopyTo(Array dest, int index)
		{
			this.Items.CopyTo(dest, index);
		}

		// Token: 0x060045B6 RID: 17846 RVA: 0x000FD5BC File Offset: 0x000FC5BC
		public IEnumerator GetEnumerator()
		{
			return this.Items.GetEnumerator();
		}

		// Token: 0x060045B7 RID: 17847 RVA: 0x000FD5C9 File Offset: 0x000FC5C9
		public int IndexOf(ListViewItem item)
		{
			return this.Items.IndexOf(item);
		}

		// Token: 0x060045B8 RID: 17848 RVA: 0x000FD5D7 File Offset: 0x000FC5D7
		public ListViewItem Insert(int index, ListViewItem item)
		{
			this.CheckListViewItem(item);
			this.MoveToGroup(item, this.group);
			this.Items.Insert(index, item);
			return item;
		}

		// Token: 0x060045B9 RID: 17849 RVA: 0x000FD5FC File Offset: 0x000FC5FC
		private void MoveToGroup(ListViewItem item, ListViewGroup newGroup)
		{
			ListViewGroup listViewGroup = item.Group;
			if (listViewGroup != newGroup)
			{
				item.group = newGroup;
				if (listViewGroup != null)
				{
					listViewGroup.Items.Remove(item);
				}
				this.UpdateNativeListViewItem(item);
			}
		}

		// Token: 0x060045BA RID: 17850 RVA: 0x000FD631 File Offset: 0x000FC631
		public void Remove(ListViewItem item)
		{
			this.Items.Remove(item);
			if (item.group == this.group)
			{
				item.group = null;
				this.UpdateNativeListViewItem(item);
			}
		}

		// Token: 0x060045BB RID: 17851 RVA: 0x000FD65B File Offset: 0x000FC65B
		public void RemoveAt(int index)
		{
			this.Remove(this[index]);
		}

		// Token: 0x060045BC RID: 17852 RVA: 0x000FD66A File Offset: 0x000FC66A
		private void UpdateNativeListViewItem(ListViewItem item)
		{
			if (item.ListView != null && item.ListView.IsHandleCreated && !item.ListView.InsertingItemsNatively)
			{
				item.UpdateStateToListView(item.Index);
			}
		}

		// Token: 0x04002168 RID: 8552
		private ListViewGroup group;

		// Token: 0x04002169 RID: 8553
		private ArrayList items;
	}
}
