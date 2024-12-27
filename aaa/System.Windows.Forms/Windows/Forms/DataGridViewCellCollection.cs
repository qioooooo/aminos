using System;
using System.Collections;
using System.ComponentModel;

namespace System.Windows.Forms
{
	// Token: 0x02000311 RID: 785
	[ListBindable(false)]
	public class DataGridViewCellCollection : BaseCollection, IList, ICollection, IEnumerable
	{
		// Token: 0x0600330B RID: 13067 RVA: 0x000B352B File Offset: 0x000B252B
		int IList.Add(object value)
		{
			return this.Add((DataGridViewCell)value);
		}

		// Token: 0x0600330C RID: 13068 RVA: 0x000B3539 File Offset: 0x000B2539
		void IList.Clear()
		{
			this.Clear();
		}

		// Token: 0x0600330D RID: 13069 RVA: 0x000B3541 File Offset: 0x000B2541
		bool IList.Contains(object value)
		{
			return this.items.Contains(value);
		}

		// Token: 0x0600330E RID: 13070 RVA: 0x000B354F File Offset: 0x000B254F
		int IList.IndexOf(object value)
		{
			return this.items.IndexOf(value);
		}

		// Token: 0x0600330F RID: 13071 RVA: 0x000B355D File Offset: 0x000B255D
		void IList.Insert(int index, object value)
		{
			this.Insert(index, (DataGridViewCell)value);
		}

		// Token: 0x06003310 RID: 13072 RVA: 0x000B356C File Offset: 0x000B256C
		void IList.Remove(object value)
		{
			this.Remove((DataGridViewCell)value);
		}

		// Token: 0x06003311 RID: 13073 RVA: 0x000B357A File Offset: 0x000B257A
		void IList.RemoveAt(int index)
		{
			this.RemoveAt(index);
		}

		// Token: 0x170008F8 RID: 2296
		// (get) Token: 0x06003312 RID: 13074 RVA: 0x000B3583 File Offset: 0x000B2583
		bool IList.IsFixedSize
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170008F9 RID: 2297
		// (get) Token: 0x06003313 RID: 13075 RVA: 0x000B3586 File Offset: 0x000B2586
		bool IList.IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170008FA RID: 2298
		object IList.this[int index]
		{
			get
			{
				return this[index];
			}
			set
			{
				this[index] = (DataGridViewCell)value;
			}
		}

		// Token: 0x06003316 RID: 13078 RVA: 0x000B35A1 File Offset: 0x000B25A1
		void ICollection.CopyTo(Array array, int index)
		{
			this.items.CopyTo(array, index);
		}

		// Token: 0x170008FB RID: 2299
		// (get) Token: 0x06003317 RID: 13079 RVA: 0x000B35B0 File Offset: 0x000B25B0
		int ICollection.Count
		{
			get
			{
				return this.items.Count;
			}
		}

		// Token: 0x170008FC RID: 2300
		// (get) Token: 0x06003318 RID: 13080 RVA: 0x000B35BD File Offset: 0x000B25BD
		bool ICollection.IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170008FD RID: 2301
		// (get) Token: 0x06003319 RID: 13081 RVA: 0x000B35C0 File Offset: 0x000B25C0
		object ICollection.SyncRoot
		{
			get
			{
				return this;
			}
		}

		// Token: 0x0600331A RID: 13082 RVA: 0x000B35C3 File Offset: 0x000B25C3
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.items.GetEnumerator();
		}

		// Token: 0x0600331B RID: 13083 RVA: 0x000B35D0 File Offset: 0x000B25D0
		public DataGridViewCellCollection(DataGridViewRow dataGridViewRow)
		{
			this.owner = dataGridViewRow;
		}

		// Token: 0x170008FE RID: 2302
		// (get) Token: 0x0600331C RID: 13084 RVA: 0x000B35EA File Offset: 0x000B25EA
		protected override ArrayList List
		{
			get
			{
				return this.items;
			}
		}

		// Token: 0x170008FF RID: 2303
		public DataGridViewCell this[int index]
		{
			get
			{
				return (DataGridViewCell)this.items[index];
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				if (value.DataGridView != null)
				{
					throw new InvalidOperationException(SR.GetString("DataGridViewCellCollection_CellAlreadyBelongsToDataGridView"));
				}
				if (value.OwningRow != null)
				{
					throw new InvalidOperationException(SR.GetString("DataGridViewCellCollection_CellAlreadyBelongsToDataGridViewRow"));
				}
				if (this.owner.DataGridView != null)
				{
					this.owner.DataGridView.OnReplacingCell(this.owner, index);
				}
				DataGridViewCell dataGridViewCell = (DataGridViewCell)this.items[index];
				this.items[index] = value;
				value.OwningRowInternal = this.owner;
				value.StateInternal = dataGridViewCell.State;
				if (this.owner.DataGridView != null)
				{
					value.DataGridViewInternal = this.owner.DataGridView;
					value.OwningColumnInternal = this.owner.DataGridView.Columns[index];
					this.owner.DataGridView.OnReplacedCell(this.owner, index);
				}
				dataGridViewCell.DataGridViewInternal = null;
				dataGridViewCell.OwningRowInternal = null;
				dataGridViewCell.OwningColumnInternal = null;
				if (dataGridViewCell.ReadOnly)
				{
					dataGridViewCell.ReadOnlyInternal = false;
				}
				if (dataGridViewCell.Selected)
				{
					dataGridViewCell.SelectedInternal = false;
				}
			}
		}

		// Token: 0x17000900 RID: 2304
		public DataGridViewCell this[string columnName]
		{
			get
			{
				DataGridViewColumn dataGridViewColumn = null;
				if (this.owner.DataGridView != null)
				{
					dataGridViewColumn = this.owner.DataGridView.Columns[columnName];
				}
				if (dataGridViewColumn == null)
				{
					throw new ArgumentException(SR.GetString("DataGridViewColumnCollection_ColumnNotFound", new object[] { columnName }), "columnName");
				}
				return (DataGridViewCell)this.items[dataGridViewColumn.Index];
			}
			set
			{
				DataGridViewColumn dataGridViewColumn = null;
				if (this.owner.DataGridView != null)
				{
					dataGridViewColumn = this.owner.DataGridView.Columns[columnName];
				}
				if (dataGridViewColumn == null)
				{
					throw new ArgumentException(SR.GetString("DataGridViewColumnCollection_ColumnNotFound", new object[] { columnName }), "columnName");
				}
				this[dataGridViewColumn.Index] = value;
			}
		}

		// Token: 0x140001D2 RID: 466
		// (add) Token: 0x06003321 RID: 13089 RVA: 0x000B3808 File Offset: 0x000B2808
		// (remove) Token: 0x06003322 RID: 13090 RVA: 0x000B3821 File Offset: 0x000B2821
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

		// Token: 0x06003323 RID: 13091 RVA: 0x000B383A File Offset: 0x000B283A
		public virtual int Add(DataGridViewCell dataGridViewCell)
		{
			if (this.owner.DataGridView != null)
			{
				throw new InvalidOperationException(SR.GetString("DataGridViewCellCollection_OwningRowAlreadyBelongsToDataGridView"));
			}
			if (dataGridViewCell.OwningRow != null)
			{
				throw new InvalidOperationException(SR.GetString("DataGridViewCellCollection_CellAlreadyBelongsToDataGridViewRow"));
			}
			return this.AddInternal(dataGridViewCell);
		}

		// Token: 0x06003324 RID: 13092 RVA: 0x000B3878 File Offset: 0x000B2878
		internal int AddInternal(DataGridViewCell dataGridViewCell)
		{
			int num = this.items.Add(dataGridViewCell);
			dataGridViewCell.OwningRowInternal = this.owner;
			DataGridView dataGridView = this.owner.DataGridView;
			if (dataGridView != null && dataGridView.Columns.Count > num)
			{
				dataGridViewCell.OwningColumnInternal = dataGridView.Columns[num];
			}
			this.OnCollectionChanged(new CollectionChangeEventArgs(CollectionChangeAction.Add, dataGridViewCell));
			return num;
		}

		// Token: 0x06003325 RID: 13093 RVA: 0x000B38DC File Offset: 0x000B28DC
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public virtual void AddRange(params DataGridViewCell[] dataGridViewCells)
		{
			if (dataGridViewCells == null)
			{
				throw new ArgumentNullException("dataGridViewCells");
			}
			if (this.owner.DataGridView != null)
			{
				throw new InvalidOperationException(SR.GetString("DataGridViewCellCollection_OwningRowAlreadyBelongsToDataGridView"));
			}
			foreach (DataGridViewCell dataGridViewCell in dataGridViewCells)
			{
				if (dataGridViewCell == null)
				{
					throw new InvalidOperationException(SR.GetString("DataGridViewCellCollection_AtLeastOneCellIsNull"));
				}
				if (dataGridViewCell.OwningRow != null)
				{
					throw new InvalidOperationException(SR.GetString("DataGridViewCellCollection_CellAlreadyBelongsToDataGridViewRow"));
				}
			}
			int num = dataGridViewCells.Length;
			for (int j = 0; j < num - 1; j++)
			{
				for (int k = j + 1; k < num; k++)
				{
					if (dataGridViewCells[j] == dataGridViewCells[k])
					{
						throw new InvalidOperationException(SR.GetString("DataGridViewCellCollection_CannotAddIdenticalCells"));
					}
				}
			}
			this.items.AddRange(dataGridViewCells);
			foreach (DataGridViewCell dataGridViewCell2 in dataGridViewCells)
			{
				dataGridViewCell2.OwningRowInternal = this.owner;
			}
			this.OnCollectionChanged(new CollectionChangeEventArgs(CollectionChangeAction.Refresh, null));
		}

		// Token: 0x06003326 RID: 13094 RVA: 0x000B39D8 File Offset: 0x000B29D8
		public virtual void Clear()
		{
			if (this.owner.DataGridView != null)
			{
				throw new InvalidOperationException(SR.GetString("DataGridViewCellCollection_OwningRowAlreadyBelongsToDataGridView"));
			}
			foreach (object obj in this.items)
			{
				DataGridViewCell dataGridViewCell = (DataGridViewCell)obj;
				dataGridViewCell.OwningRowInternal = null;
			}
			this.items.Clear();
			this.OnCollectionChanged(new CollectionChangeEventArgs(CollectionChangeAction.Refresh, null));
		}

		// Token: 0x06003327 RID: 13095 RVA: 0x000B3A68 File Offset: 0x000B2A68
		public void CopyTo(DataGridViewCell[] array, int index)
		{
			this.items.CopyTo(array, index);
		}

		// Token: 0x06003328 RID: 13096 RVA: 0x000B3A78 File Offset: 0x000B2A78
		public virtual bool Contains(DataGridViewCell dataGridViewCell)
		{
			int num = this.items.IndexOf(dataGridViewCell);
			return num != -1;
		}

		// Token: 0x06003329 RID: 13097 RVA: 0x000B3A99 File Offset: 0x000B2A99
		public int IndexOf(DataGridViewCell dataGridViewCell)
		{
			return this.items.IndexOf(dataGridViewCell);
		}

		// Token: 0x0600332A RID: 13098 RVA: 0x000B3AA8 File Offset: 0x000B2AA8
		public virtual void Insert(int index, DataGridViewCell dataGridViewCell)
		{
			if (this.owner.DataGridView != null)
			{
				throw new InvalidOperationException(SR.GetString("DataGridViewCellCollection_OwningRowAlreadyBelongsToDataGridView"));
			}
			if (dataGridViewCell.OwningRow != null)
			{
				throw new InvalidOperationException(SR.GetString("DataGridViewCellCollection_CellAlreadyBelongsToDataGridViewRow"));
			}
			this.items.Insert(index, dataGridViewCell);
			dataGridViewCell.OwningRowInternal = this.owner;
			this.OnCollectionChanged(new CollectionChangeEventArgs(CollectionChangeAction.Add, dataGridViewCell));
		}

		// Token: 0x0600332B RID: 13099 RVA: 0x000B3B10 File Offset: 0x000B2B10
		internal void InsertInternal(int index, DataGridViewCell dataGridViewCell)
		{
			this.items.Insert(index, dataGridViewCell);
			dataGridViewCell.OwningRowInternal = this.owner;
			DataGridView dataGridView = this.owner.DataGridView;
			if (dataGridView != null && dataGridView.Columns.Count > index)
			{
				dataGridViewCell.OwningColumnInternal = dataGridView.Columns[index];
			}
			this.OnCollectionChanged(new CollectionChangeEventArgs(CollectionChangeAction.Add, dataGridViewCell));
		}

		// Token: 0x0600332C RID: 13100 RVA: 0x000B3B72 File Offset: 0x000B2B72
		protected void OnCollectionChanged(CollectionChangeEventArgs e)
		{
			if (this.onCollectionChanged != null)
			{
				this.onCollectionChanged(this, e);
			}
		}

		// Token: 0x0600332D RID: 13101 RVA: 0x000B3B8C File Offset: 0x000B2B8C
		public virtual void Remove(DataGridViewCell cell)
		{
			if (this.owner.DataGridView != null)
			{
				throw new InvalidOperationException(SR.GetString("DataGridViewCellCollection_OwningRowAlreadyBelongsToDataGridView"));
			}
			int num = -1;
			int count = this.items.Count;
			for (int i = 0; i < count; i++)
			{
				if (this.items[i] == cell)
				{
					num = i;
					break;
				}
			}
			if (num == -1)
			{
				throw new ArgumentException(SR.GetString("DataGridViewCellCollection_CellNotFound"));
			}
			this.RemoveAt(num);
		}

		// Token: 0x0600332E RID: 13102 RVA: 0x000B3BFE File Offset: 0x000B2BFE
		public virtual void RemoveAt(int index)
		{
			if (this.owner.DataGridView != null)
			{
				throw new InvalidOperationException(SR.GetString("DataGridViewCellCollection_OwningRowAlreadyBelongsToDataGridView"));
			}
			this.RemoveAtInternal(index);
		}

		// Token: 0x0600332F RID: 13103 RVA: 0x000B3C24 File Offset: 0x000B2C24
		internal void RemoveAtInternal(int index)
		{
			DataGridViewCell dataGridViewCell = (DataGridViewCell)this.items[index];
			this.items.RemoveAt(index);
			dataGridViewCell.DataGridViewInternal = null;
			dataGridViewCell.OwningRowInternal = null;
			if (dataGridViewCell.ReadOnly)
			{
				dataGridViewCell.ReadOnlyInternal = false;
			}
			if (dataGridViewCell.Selected)
			{
				dataGridViewCell.SelectedInternal = false;
			}
			this.OnCollectionChanged(new CollectionChangeEventArgs(CollectionChangeAction.Remove, dataGridViewCell));
		}

		// Token: 0x04001AA4 RID: 6820
		private CollectionChangeEventHandler onCollectionChanged;

		// Token: 0x04001AA5 RID: 6821
		private ArrayList items = new ArrayList();

		// Token: 0x04001AA6 RID: 6822
		private DataGridViewRow owner;
	}
}
