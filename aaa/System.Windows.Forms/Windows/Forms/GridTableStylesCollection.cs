using System;
using System.Collections;
using System.ComponentModel;
using System.Globalization;

namespace System.Windows.Forms
{
	// Token: 0x020002E2 RID: 738
	[ListBindable(false)]
	public class GridTableStylesCollection : BaseCollection, IList, ICollection, IEnumerable
	{
		// Token: 0x06002B70 RID: 11120 RVA: 0x00074B25 File Offset: 0x00073B25
		int IList.Add(object value)
		{
			return this.Add((DataGridTableStyle)value);
		}

		// Token: 0x06002B71 RID: 11121 RVA: 0x00074B33 File Offset: 0x00073B33
		void IList.Clear()
		{
			this.Clear();
		}

		// Token: 0x06002B72 RID: 11122 RVA: 0x00074B3B File Offset: 0x00073B3B
		bool IList.Contains(object value)
		{
			return this.items.Contains(value);
		}

		// Token: 0x06002B73 RID: 11123 RVA: 0x00074B49 File Offset: 0x00073B49
		int IList.IndexOf(object value)
		{
			return this.items.IndexOf(value);
		}

		// Token: 0x06002B74 RID: 11124 RVA: 0x00074B57 File Offset: 0x00073B57
		void IList.Insert(int index, object value)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06002B75 RID: 11125 RVA: 0x00074B5E File Offset: 0x00073B5E
		void IList.Remove(object value)
		{
			this.Remove((DataGridTableStyle)value);
		}

		// Token: 0x06002B76 RID: 11126 RVA: 0x00074B6C File Offset: 0x00073B6C
		void IList.RemoveAt(int index)
		{
			this.RemoveAt(index);
		}

		// Token: 0x1700074B RID: 1867
		// (get) Token: 0x06002B77 RID: 11127 RVA: 0x00074B75 File Offset: 0x00073B75
		bool IList.IsFixedSize
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700074C RID: 1868
		// (get) Token: 0x06002B78 RID: 11128 RVA: 0x00074B78 File Offset: 0x00073B78
		bool IList.IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700074D RID: 1869
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

		// Token: 0x06002B7B RID: 11131 RVA: 0x00074B90 File Offset: 0x00073B90
		void ICollection.CopyTo(Array array, int index)
		{
			this.items.CopyTo(array, index);
		}

		// Token: 0x1700074E RID: 1870
		// (get) Token: 0x06002B7C RID: 11132 RVA: 0x00074B9F File Offset: 0x00073B9F
		int ICollection.Count
		{
			get
			{
				return this.items.Count;
			}
		}

		// Token: 0x1700074F RID: 1871
		// (get) Token: 0x06002B7D RID: 11133 RVA: 0x00074BAC File Offset: 0x00073BAC
		bool ICollection.IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000750 RID: 1872
		// (get) Token: 0x06002B7E RID: 11134 RVA: 0x00074BAF File Offset: 0x00073BAF
		object ICollection.SyncRoot
		{
			get
			{
				return this;
			}
		}

		// Token: 0x06002B7F RID: 11135 RVA: 0x00074BB2 File Offset: 0x00073BB2
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.items.GetEnumerator();
		}

		// Token: 0x06002B80 RID: 11136 RVA: 0x00074BBF File Offset: 0x00073BBF
		internal GridTableStylesCollection(DataGrid grid)
		{
			this.owner = grid;
		}

		// Token: 0x17000751 RID: 1873
		// (get) Token: 0x06002B81 RID: 11137 RVA: 0x00074BD9 File Offset: 0x00073BD9
		protected override ArrayList List
		{
			get
			{
				return this.items;
			}
		}

		// Token: 0x17000752 RID: 1874
		public DataGridTableStyle this[int index]
		{
			get
			{
				return (DataGridTableStyle)this.items[index];
			}
		}

		// Token: 0x17000753 RID: 1875
		public DataGridTableStyle this[string tableName]
		{
			get
			{
				if (tableName == null)
				{
					throw new ArgumentNullException("tableName");
				}
				int count = this.items.Count;
				for (int i = 0; i < count; i++)
				{
					DataGridTableStyle dataGridTableStyle = (DataGridTableStyle)this.items[i];
					if (string.Equals(dataGridTableStyle.MappingName, tableName, StringComparison.OrdinalIgnoreCase))
					{
						return dataGridTableStyle;
					}
				}
				return null;
			}
		}

		// Token: 0x06002B84 RID: 11140 RVA: 0x00074C4C File Offset: 0x00073C4C
		internal void CheckForMappingNameDuplicates(DataGridTableStyle table)
		{
			if (string.IsNullOrEmpty(table.MappingName))
			{
				return;
			}
			for (int i = 0; i < this.items.Count; i++)
			{
				if (((DataGridTableStyle)this.items[i]).MappingName.Equals(table.MappingName) && table != this.items[i])
				{
					throw new ArgumentException(SR.GetString("DataGridTableStyleDuplicateMappingName"), "table");
				}
			}
		}

		// Token: 0x06002B85 RID: 11141 RVA: 0x00074CC4 File Offset: 0x00073CC4
		public virtual int Add(DataGridTableStyle table)
		{
			if (this.owner != null && this.owner.MinimumRowHeaderWidth() > table.RowHeaderWidth)
			{
				table.RowHeaderWidth = this.owner.MinimumRowHeaderWidth();
			}
			if (table.DataGrid != this.owner && table.DataGrid != null)
			{
				throw new ArgumentException(SR.GetString("DataGridTableStyleCollectionAddedParentedTableStyle"), "table");
			}
			table.DataGrid = this.owner;
			this.CheckForMappingNameDuplicates(table);
			table.MappingNameChanged += this.TableStyleMappingNameChanged;
			int num = this.items.Add(table);
			this.OnCollectionChanged(new CollectionChangeEventArgs(CollectionChangeAction.Add, table));
			return num;
		}

		// Token: 0x06002B86 RID: 11142 RVA: 0x00074D68 File Offset: 0x00073D68
		private void TableStyleMappingNameChanged(object sender, EventArgs pcea)
		{
			this.OnCollectionChanged(new CollectionChangeEventArgs(CollectionChangeAction.Refresh, null));
		}

		// Token: 0x06002B87 RID: 11143 RVA: 0x00074D78 File Offset: 0x00073D78
		public virtual void AddRange(DataGridTableStyle[] tables)
		{
			if (tables == null)
			{
				throw new ArgumentNullException("tables");
			}
			foreach (DataGridTableStyle dataGridTableStyle in tables)
			{
				dataGridTableStyle.DataGrid = this.owner;
				dataGridTableStyle.MappingNameChanged += this.TableStyleMappingNameChanged;
				this.items.Add(dataGridTableStyle);
			}
			this.OnCollectionChanged(new CollectionChangeEventArgs(CollectionChangeAction.Refresh, null));
		}

		// Token: 0x14000144 RID: 324
		// (add) Token: 0x06002B88 RID: 11144 RVA: 0x00074DDF File Offset: 0x00073DDF
		// (remove) Token: 0x06002B89 RID: 11145 RVA: 0x00074DF8 File Offset: 0x00073DF8
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

		// Token: 0x06002B8A RID: 11146 RVA: 0x00074E14 File Offset: 0x00073E14
		public void Clear()
		{
			for (int i = 0; i < this.items.Count; i++)
			{
				DataGridTableStyle dataGridTableStyle = (DataGridTableStyle)this.items[i];
				dataGridTableStyle.MappingNameChanged -= this.TableStyleMappingNameChanged;
			}
			this.items.Clear();
			this.OnCollectionChanged(new CollectionChangeEventArgs(CollectionChangeAction.Refresh, null));
		}

		// Token: 0x06002B8B RID: 11147 RVA: 0x00074E74 File Offset: 0x00073E74
		public bool Contains(DataGridTableStyle table)
		{
			int num = this.items.IndexOf(table);
			return num != -1;
		}

		// Token: 0x06002B8C RID: 11148 RVA: 0x00074E98 File Offset: 0x00073E98
		public bool Contains(string name)
		{
			int count = this.items.Count;
			for (int i = 0; i < count; i++)
			{
				DataGridTableStyle dataGridTableStyle = (DataGridTableStyle)this.items[i];
				if (string.Compare(dataGridTableStyle.MappingName, name, true, CultureInfo.InvariantCulture) == 0)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06002B8D RID: 11149 RVA: 0x00074EE8 File Offset: 0x00073EE8
		protected void OnCollectionChanged(CollectionChangeEventArgs e)
		{
			if (this.onCollectionChanged != null)
			{
				this.onCollectionChanged(this, e);
			}
			DataGrid dataGrid = this.owner;
			if (dataGrid != null)
			{
				dataGrid.checkHierarchy = true;
			}
		}

		// Token: 0x06002B8E RID: 11150 RVA: 0x00074F1C File Offset: 0x00073F1C
		public void Remove(DataGridTableStyle table)
		{
			int num = -1;
			int count = this.items.Count;
			for (int i = 0; i < count; i++)
			{
				if (this.items[i] == table)
				{
					num = i;
					break;
				}
			}
			if (num == -1)
			{
				throw new ArgumentException(SR.GetString("DataGridTableCollectionMissingTable"), "table");
			}
			this.RemoveAt(num);
		}

		// Token: 0x06002B8F RID: 11151 RVA: 0x00074F78 File Offset: 0x00073F78
		public void RemoveAt(int index)
		{
			DataGridTableStyle dataGridTableStyle = (DataGridTableStyle)this.items[index];
			dataGridTableStyle.MappingNameChanged -= this.TableStyleMappingNameChanged;
			this.items.RemoveAt(index);
			this.OnCollectionChanged(new CollectionChangeEventArgs(CollectionChangeAction.Remove, dataGridTableStyle));
		}

		// Token: 0x04001807 RID: 6151
		private CollectionChangeEventHandler onCollectionChanged;

		// Token: 0x04001808 RID: 6152
		private ArrayList items = new ArrayList();

		// Token: 0x04001809 RID: 6153
		private DataGrid owner;
	}
}
