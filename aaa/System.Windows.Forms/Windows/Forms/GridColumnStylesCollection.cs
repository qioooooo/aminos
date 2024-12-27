using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing.Design;
using System.Globalization;

namespace System.Windows.Forms
{
	// Token: 0x020002D5 RID: 725
	[Editor("System.Windows.Forms.Design.DataGridColumnCollectionEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
	[ListBindable(false)]
	public class GridColumnStylesCollection : BaseCollection, IList, ICollection, IEnumerable
	{
		// Token: 0x060029FE RID: 10750 RVA: 0x0006F216 File Offset: 0x0006E216
		int IList.Add(object value)
		{
			return this.Add((DataGridColumnStyle)value);
		}

		// Token: 0x060029FF RID: 10751 RVA: 0x0006F224 File Offset: 0x0006E224
		void IList.Clear()
		{
			this.Clear();
		}

		// Token: 0x06002A00 RID: 10752 RVA: 0x0006F22C File Offset: 0x0006E22C
		bool IList.Contains(object value)
		{
			return this.items.Contains(value);
		}

		// Token: 0x06002A01 RID: 10753 RVA: 0x0006F23A File Offset: 0x0006E23A
		int IList.IndexOf(object value)
		{
			return this.items.IndexOf(value);
		}

		// Token: 0x06002A02 RID: 10754 RVA: 0x0006F248 File Offset: 0x0006E248
		void IList.Insert(int index, object value)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06002A03 RID: 10755 RVA: 0x0006F24F File Offset: 0x0006E24F
		void IList.Remove(object value)
		{
			this.Remove((DataGridColumnStyle)value);
		}

		// Token: 0x06002A04 RID: 10756 RVA: 0x0006F25D File Offset: 0x0006E25D
		void IList.RemoveAt(int index)
		{
			this.RemoveAt(index);
		}

		// Token: 0x170006E9 RID: 1769
		// (get) Token: 0x06002A05 RID: 10757 RVA: 0x0006F266 File Offset: 0x0006E266
		bool IList.IsFixedSize
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170006EA RID: 1770
		// (get) Token: 0x06002A06 RID: 10758 RVA: 0x0006F269 File Offset: 0x0006E269
		bool IList.IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170006EB RID: 1771
		object IList.this[int index]
		{
			get
			{
				return this.items[index];
			}
			set
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x06002A09 RID: 10761 RVA: 0x0006F281 File Offset: 0x0006E281
		void ICollection.CopyTo(Array array, int index)
		{
			this.items.CopyTo(array, index);
		}

		// Token: 0x170006EC RID: 1772
		// (get) Token: 0x06002A0A RID: 10762 RVA: 0x0006F290 File Offset: 0x0006E290
		int ICollection.Count
		{
			get
			{
				return this.items.Count;
			}
		}

		// Token: 0x170006ED RID: 1773
		// (get) Token: 0x06002A0B RID: 10763 RVA: 0x0006F29D File Offset: 0x0006E29D
		bool ICollection.IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170006EE RID: 1774
		// (get) Token: 0x06002A0C RID: 10764 RVA: 0x0006F2A0 File Offset: 0x0006E2A0
		object ICollection.SyncRoot
		{
			get
			{
				return this;
			}
		}

		// Token: 0x06002A0D RID: 10765 RVA: 0x0006F2A3 File Offset: 0x0006E2A3
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.items.GetEnumerator();
		}

		// Token: 0x06002A0E RID: 10766 RVA: 0x0006F2B0 File Offset: 0x0006E2B0
		internal GridColumnStylesCollection(DataGridTableStyle table)
		{
			this.owner = table;
		}

		// Token: 0x06002A0F RID: 10767 RVA: 0x0006F2CA File Offset: 0x0006E2CA
		internal GridColumnStylesCollection(DataGridTableStyle table, bool isDefault)
			: this(table)
		{
			this.isDefault = isDefault;
		}

		// Token: 0x170006EF RID: 1775
		// (get) Token: 0x06002A10 RID: 10768 RVA: 0x0006F2DA File Offset: 0x0006E2DA
		protected override ArrayList List
		{
			get
			{
				return this.items;
			}
		}

		// Token: 0x170006F0 RID: 1776
		public DataGridColumnStyle this[int index]
		{
			get
			{
				return (DataGridColumnStyle)this.items[index];
			}
		}

		// Token: 0x170006F1 RID: 1777
		public DataGridColumnStyle this[string columnName]
		{
			get
			{
				int count = this.items.Count;
				for (int i = 0; i < count; i++)
				{
					DataGridColumnStyle dataGridColumnStyle = (DataGridColumnStyle)this.items[i];
					if (string.Equals(dataGridColumnStyle.MappingName, columnName, StringComparison.OrdinalIgnoreCase))
					{
						return dataGridColumnStyle;
					}
				}
				return null;
			}
		}

		// Token: 0x06002A13 RID: 10771 RVA: 0x0006F344 File Offset: 0x0006E344
		internal DataGridColumnStyle MapColumnStyleToPropertyName(string mappingName)
		{
			int count = this.items.Count;
			for (int i = 0; i < count; i++)
			{
				DataGridColumnStyle dataGridColumnStyle = (DataGridColumnStyle)this.items[i];
				if (string.Equals(dataGridColumnStyle.MappingName, mappingName, StringComparison.OrdinalIgnoreCase))
				{
					return dataGridColumnStyle;
				}
			}
			return null;
		}

		// Token: 0x170006F2 RID: 1778
		public DataGridColumnStyle this[PropertyDescriptor propertyDesciptor]
		{
			get
			{
				int count = this.items.Count;
				for (int i = 0; i < count; i++)
				{
					DataGridColumnStyle dataGridColumnStyle = (DataGridColumnStyle)this.items[i];
					if (propertyDesciptor.Equals(dataGridColumnStyle.PropertyDescriptor))
					{
						return dataGridColumnStyle;
					}
				}
				return null;
			}
		}

		// Token: 0x170006F3 RID: 1779
		// (get) Token: 0x06002A15 RID: 10773 RVA: 0x0006F3D8 File Offset: 0x0006E3D8
		internal DataGridTableStyle DataGridTableStyle
		{
			get
			{
				return this.owner;
			}
		}

		// Token: 0x06002A16 RID: 10774 RVA: 0x0006F3E0 File Offset: 0x0006E3E0
		internal void CheckForMappingNameDuplicates(DataGridColumnStyle column)
		{
			if (string.IsNullOrEmpty(column.MappingName))
			{
				return;
			}
			for (int i = 0; i < this.items.Count; i++)
			{
				if (((DataGridColumnStyle)this.items[i]).MappingName.Equals(column.MappingName) && column != this.items[i])
				{
					throw new ArgumentException(SR.GetString("DataGridColumnStyleDuplicateMappingName"), "column");
				}
			}
		}

		// Token: 0x06002A17 RID: 10775 RVA: 0x0006F458 File Offset: 0x0006E458
		private void ColumnStyleMappingNameChanged(object sender, EventArgs pcea)
		{
			this.OnCollectionChanged(new CollectionChangeEventArgs(CollectionChangeAction.Refresh, null));
		}

		// Token: 0x06002A18 RID: 10776 RVA: 0x0006F467 File Offset: 0x0006E467
		private void ColumnStylePropDescChanged(object sender, EventArgs pcea)
		{
			this.OnCollectionChanged(new CollectionChangeEventArgs(CollectionChangeAction.Refresh, (DataGridColumnStyle)sender));
		}

		// Token: 0x06002A19 RID: 10777 RVA: 0x0006F47C File Offset: 0x0006E47C
		public virtual int Add(DataGridColumnStyle column)
		{
			if (this.isDefault)
			{
				throw new ArgumentException(SR.GetString("DataGridDefaultColumnCollectionChanged"));
			}
			this.CheckForMappingNameDuplicates(column);
			column.SetDataGridTableInColumn(this.owner, true);
			column.MappingNameChanged += this.ColumnStyleMappingNameChanged;
			column.PropertyDescriptorChanged += this.ColumnStylePropDescChanged;
			if (this.DataGridTableStyle != null && column.Width == -1)
			{
				column.width = this.DataGridTableStyle.PreferredColumnWidth;
			}
			int num = this.items.Add(column);
			this.OnCollectionChanged(new CollectionChangeEventArgs(CollectionChangeAction.Add, column));
			return num;
		}

		// Token: 0x06002A1A RID: 10778 RVA: 0x0006F518 File Offset: 0x0006E518
		public void AddRange(DataGridColumnStyle[] columns)
		{
			if (columns == null)
			{
				throw new ArgumentNullException("columns");
			}
			for (int i = 0; i < columns.Length; i++)
			{
				this.Add(columns[i]);
			}
		}

		// Token: 0x06002A1B RID: 10779 RVA: 0x0006F54B File Offset: 0x0006E54B
		internal void AddDefaultColumn(DataGridColumnStyle column)
		{
			column.SetDataGridTableInColumn(this.owner, true);
			this.items.Add(column);
		}

		// Token: 0x06002A1C RID: 10780 RVA: 0x0006F568 File Offset: 0x0006E568
		internal void ResetDefaultColumnCollection()
		{
			for (int i = 0; i < this.Count; i++)
			{
				this[i].ReleaseHostedControl();
			}
			this.items.Clear();
		}

		// Token: 0x1400012F RID: 303
		// (add) Token: 0x06002A1D RID: 10781 RVA: 0x0006F59D File Offset: 0x0006E59D
		// (remove) Token: 0x06002A1E RID: 10782 RVA: 0x0006F5B6 File Offset: 0x0006E5B6
		public event CollectionChangeEventHandler CollectionChanged
		{
			add
			{
				this.onCollectionChanged = (CollectionChangeEventHandler)Delegate.Combine(this.onCollectionChanged, value);
			}
			remove
			{
				this.onCollectionChanged = (CollectionChangeEventHandler)Delegate.Remove(this.onCollectionChanged, value);
			}
		}

		// Token: 0x06002A1F RID: 10783 RVA: 0x0006F5D0 File Offset: 0x0006E5D0
		public void Clear()
		{
			for (int i = 0; i < this.Count; i++)
			{
				this[i].ReleaseHostedControl();
			}
			this.items.Clear();
			this.OnCollectionChanged(new CollectionChangeEventArgs(CollectionChangeAction.Refresh, null));
		}

		// Token: 0x06002A20 RID: 10784 RVA: 0x0006F612 File Offset: 0x0006E612
		public bool Contains(PropertyDescriptor propertyDescriptor)
		{
			return this[propertyDescriptor] != null;
		}

		// Token: 0x06002A21 RID: 10785 RVA: 0x0006F624 File Offset: 0x0006E624
		public bool Contains(DataGridColumnStyle column)
		{
			int num = this.items.IndexOf(column);
			return num != -1;
		}

		// Token: 0x06002A22 RID: 10786 RVA: 0x0006F648 File Offset: 0x0006E648
		public bool Contains(string name)
		{
			foreach (object obj in this.items)
			{
				DataGridColumnStyle dataGridColumnStyle = (DataGridColumnStyle)obj;
				if (string.Compare(dataGridColumnStyle.MappingName, name, true, CultureInfo.InvariantCulture) == 0)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06002A23 RID: 10787 RVA: 0x0006F690 File Offset: 0x0006E690
		public int IndexOf(DataGridColumnStyle element)
		{
			int count = this.items.Count;
			for (int i = 0; i < count; i++)
			{
				DataGridColumnStyle dataGridColumnStyle = (DataGridColumnStyle)this.items[i];
				if (element == dataGridColumnStyle)
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x06002A24 RID: 10788 RVA: 0x0006F6D0 File Offset: 0x0006E6D0
		protected void OnCollectionChanged(CollectionChangeEventArgs e)
		{
			if (this.onCollectionChanged != null)
			{
				this.onCollectionChanged(this, e);
			}
			DataGrid dataGrid = this.owner.DataGrid;
			if (dataGrid != null)
			{
				dataGrid.checkHierarchy = true;
			}
		}

		// Token: 0x06002A25 RID: 10789 RVA: 0x0006F708 File Offset: 0x0006E708
		public void Remove(DataGridColumnStyle column)
		{
			if (this.isDefault)
			{
				throw new ArgumentException(SR.GetString("DataGridDefaultColumnCollectionChanged"));
			}
			int num = -1;
			int count = this.items.Count;
			for (int i = 0; i < count; i++)
			{
				if (this.items[i] == column)
				{
					num = i;
					break;
				}
			}
			if (num == -1)
			{
				throw new InvalidOperationException(SR.GetString("DataGridColumnCollectionMissing"));
			}
			this.RemoveAt(num);
		}

		// Token: 0x06002A26 RID: 10790 RVA: 0x0006F778 File Offset: 0x0006E778
		public void RemoveAt(int index)
		{
			if (this.isDefault)
			{
				throw new ArgumentException(SR.GetString("DataGridDefaultColumnCollectionChanged"));
			}
			DataGridColumnStyle dataGridColumnStyle = (DataGridColumnStyle)this.items[index];
			dataGridColumnStyle.SetDataGridTableInColumn(null, true);
			dataGridColumnStyle.MappingNameChanged -= this.ColumnStyleMappingNameChanged;
			dataGridColumnStyle.PropertyDescriptorChanged -= this.ColumnStylePropDescChanged;
			this.items.RemoveAt(index);
			this.OnCollectionChanged(new CollectionChangeEventArgs(CollectionChangeAction.Remove, dataGridColumnStyle));
		}

		// Token: 0x06002A27 RID: 10791 RVA: 0x0006F7F4 File Offset: 0x0006E7F4
		public void ResetPropertyDescriptors()
		{
			for (int i = 0; i < this.Count; i++)
			{
				this[i].PropertyDescriptor = null;
			}
		}

		// Token: 0x04001797 RID: 6039
		private CollectionChangeEventHandler onCollectionChanged;

		// Token: 0x04001798 RID: 6040
		private ArrayList items = new ArrayList();

		// Token: 0x04001799 RID: 6041
		private DataGridTableStyle owner;

		// Token: 0x0400179A RID: 6042
		private bool isDefault;
	}
}
