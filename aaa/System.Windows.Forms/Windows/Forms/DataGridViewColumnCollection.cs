using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;

namespace System.Windows.Forms
{
	// Token: 0x0200032F RID: 815
	[ListBindable(false)]
	public class DataGridViewColumnCollection : BaseCollection, IList, ICollection, IEnumerable
	{
		// Token: 0x17000965 RID: 2405
		// (get) Token: 0x0600342A RID: 13354 RVA: 0x000B82F2 File Offset: 0x000B72F2
		bool IList.IsFixedSize
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000966 RID: 2406
		// (get) Token: 0x0600342B RID: 13355 RVA: 0x000B82F5 File Offset: 0x000B72F5
		bool IList.IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000967 RID: 2407
		object IList.this[int index]
		{
			get
			{
				return this[index];
			}
			set
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x0600342E RID: 13358 RVA: 0x000B8308 File Offset: 0x000B7308
		int IList.Add(object value)
		{
			return this.Add((DataGridViewColumn)value);
		}

		// Token: 0x0600342F RID: 13359 RVA: 0x000B8316 File Offset: 0x000B7316
		void IList.Clear()
		{
			this.Clear();
		}

		// Token: 0x06003430 RID: 13360 RVA: 0x000B831E File Offset: 0x000B731E
		bool IList.Contains(object value)
		{
			return this.items.Contains(value);
		}

		// Token: 0x06003431 RID: 13361 RVA: 0x000B832C File Offset: 0x000B732C
		int IList.IndexOf(object value)
		{
			return this.items.IndexOf(value);
		}

		// Token: 0x06003432 RID: 13362 RVA: 0x000B833A File Offset: 0x000B733A
		void IList.Insert(int index, object value)
		{
			this.Insert(index, (DataGridViewColumn)value);
		}

		// Token: 0x06003433 RID: 13363 RVA: 0x000B8349 File Offset: 0x000B7349
		void IList.Remove(object value)
		{
			this.Remove((DataGridViewColumn)value);
		}

		// Token: 0x06003434 RID: 13364 RVA: 0x000B8357 File Offset: 0x000B7357
		void IList.RemoveAt(int index)
		{
			this.RemoveAt(index);
		}

		// Token: 0x17000968 RID: 2408
		// (get) Token: 0x06003435 RID: 13365 RVA: 0x000B8360 File Offset: 0x000B7360
		int ICollection.Count
		{
			get
			{
				return this.items.Count;
			}
		}

		// Token: 0x17000969 RID: 2409
		// (get) Token: 0x06003436 RID: 13366 RVA: 0x000B836D File Offset: 0x000B736D
		bool ICollection.IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700096A RID: 2410
		// (get) Token: 0x06003437 RID: 13367 RVA: 0x000B8370 File Offset: 0x000B7370
		object ICollection.SyncRoot
		{
			get
			{
				return this;
			}
		}

		// Token: 0x06003438 RID: 13368 RVA: 0x000B8373 File Offset: 0x000B7373
		void ICollection.CopyTo(Array array, int index)
		{
			this.items.CopyTo(array, index);
		}

		// Token: 0x06003439 RID: 13369 RVA: 0x000B8382 File Offset: 0x000B7382
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.items.GetEnumerator();
		}

		// Token: 0x0600343A RID: 13370 RVA: 0x000B838F File Offset: 0x000B738F
		public DataGridViewColumnCollection(DataGridView dataGridView)
		{
			this.InvalidateCachedColumnCounts();
			this.InvalidateCachedColumnsWidths();
			this.dataGridView = dataGridView;
		}

		// Token: 0x1700096B RID: 2411
		// (get) Token: 0x0600343B RID: 13371 RVA: 0x000B83BC File Offset: 0x000B73BC
		internal static IComparer ColumnCollectionOrderComparer
		{
			get
			{
				return DataGridViewColumnCollection.columnOrderComparer;
			}
		}

		// Token: 0x1700096C RID: 2412
		// (get) Token: 0x0600343C RID: 13372 RVA: 0x000B83C3 File Offset: 0x000B73C3
		protected override ArrayList List
		{
			get
			{
				return this.items;
			}
		}

		// Token: 0x1700096D RID: 2413
		// (get) Token: 0x0600343D RID: 13373 RVA: 0x000B83CB File Offset: 0x000B73CB
		protected DataGridView DataGridView
		{
			get
			{
				return this.dataGridView;
			}
		}

		// Token: 0x1700096E RID: 2414
		public DataGridViewColumn this[int index]
		{
			get
			{
				return (DataGridViewColumn)this.items[index];
			}
		}

		// Token: 0x1700096F RID: 2415
		public DataGridViewColumn this[string columnName]
		{
			get
			{
				if (columnName == null)
				{
					throw new ArgumentNullException("columnName");
				}
				int count = this.items.Count;
				for (int i = 0; i < count; i++)
				{
					DataGridViewColumn dataGridViewColumn = (DataGridViewColumn)this.items[i];
					if (string.Equals(dataGridViewColumn.Name, columnName, StringComparison.OrdinalIgnoreCase))
					{
						return dataGridViewColumn;
					}
				}
				return null;
			}
		}

		// Token: 0x140001D3 RID: 467
		// (add) Token: 0x06003440 RID: 13376 RVA: 0x000B843F File Offset: 0x000B743F
		// (remove) Token: 0x06003441 RID: 13377 RVA: 0x000B8458 File Offset: 0x000B7458
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

		// Token: 0x06003442 RID: 13378 RVA: 0x000B8474 File Offset: 0x000B7474
		internal int ActualDisplayIndexToColumnIndex(int actualDisplayIndex, DataGridViewElementStates includeFilter)
		{
			DataGridViewColumn dataGridViewColumn = this.GetFirstColumn(includeFilter);
			for (int i = 0; i < actualDisplayIndex; i++)
			{
				dataGridViewColumn = this.GetNextColumn(dataGridViewColumn, includeFilter, DataGridViewElementStates.None);
			}
			return dataGridViewColumn.Index;
		}

		// Token: 0x06003443 RID: 13379 RVA: 0x000B84A8 File Offset: 0x000B74A8
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public virtual int Add(string columnName, string headerText)
		{
			return this.Add(new DataGridViewTextBoxColumn
			{
				Name = columnName,
				HeaderText = headerText
			});
		}

		// Token: 0x06003444 RID: 13380 RVA: 0x000B84D0 File Offset: 0x000B74D0
		public virtual int Add(DataGridViewColumn dataGridViewColumn)
		{
			if (this.DataGridView.NoDimensionChangeAllowed)
			{
				throw new InvalidOperationException(SR.GetString("DataGridView_ForbiddenOperationInEventHandler"));
			}
			if (this.DataGridView.InDisplayIndexAdjustments)
			{
				throw new InvalidOperationException(SR.GetString("DataGridView_CannotAlterDisplayIndexWithinAdjustments"));
			}
			this.DataGridView.OnAddingColumn(dataGridViewColumn);
			this.InvalidateCachedColumnsOrder();
			int num = this.items.Add(dataGridViewColumn);
			dataGridViewColumn.IndexInternal = num;
			dataGridViewColumn.DataGridViewInternal = this.dataGridView;
			this.UpdateColumnCaches(dataGridViewColumn, true);
			this.DataGridView.OnAddedColumn(dataGridViewColumn);
			this.OnCollectionChanged(new CollectionChangeEventArgs(CollectionChangeAction.Add, dataGridViewColumn), false, new Point(-1, -1));
			return num;
		}

		// Token: 0x06003445 RID: 13381 RVA: 0x000B8574 File Offset: 0x000B7574
		public virtual void AddRange(params DataGridViewColumn[] dataGridViewColumns)
		{
			if (dataGridViewColumns == null)
			{
				throw new ArgumentNullException("dataGridViewColumns");
			}
			if (this.DataGridView.NoDimensionChangeAllowed)
			{
				throw new InvalidOperationException(SR.GetString("DataGridView_ForbiddenOperationInEventHandler"));
			}
			if (this.DataGridView.InDisplayIndexAdjustments)
			{
				throw new InvalidOperationException(SR.GetString("DataGridView_CannotAlterDisplayIndexWithinAdjustments"));
			}
			ArrayList arrayList = new ArrayList(dataGridViewColumns.Length);
			ArrayList arrayList2 = new ArrayList(dataGridViewColumns.Length);
			foreach (DataGridViewColumn dataGridViewColumn in dataGridViewColumns)
			{
				if (dataGridViewColumn.DisplayIndex != -1)
				{
					arrayList.Add(dataGridViewColumn);
				}
			}
			int j;
			while (arrayList.Count > 0)
			{
				int num = int.MaxValue;
				int num2 = -1;
				for (j = 0; j < arrayList.Count; j++)
				{
					DataGridViewColumn dataGridViewColumn2 = (DataGridViewColumn)arrayList[j];
					if (dataGridViewColumn2.DisplayIndex < num)
					{
						num = dataGridViewColumn2.DisplayIndex;
						num2 = j;
					}
				}
				arrayList2.Add(arrayList[num2]);
				arrayList.RemoveAt(num2);
			}
			foreach (DataGridViewColumn dataGridViewColumn3 in dataGridViewColumns)
			{
				if (dataGridViewColumn3.DisplayIndex == -1)
				{
					arrayList2.Add(dataGridViewColumn3);
				}
			}
			j = 0;
			foreach (object obj in arrayList2)
			{
				DataGridViewColumn dataGridViewColumn4 = (DataGridViewColumn)obj;
				dataGridViewColumns[j] = dataGridViewColumn4;
				j++;
			}
			this.DataGridView.OnAddingColumns(dataGridViewColumns);
			foreach (DataGridViewColumn dataGridViewColumn5 in dataGridViewColumns)
			{
				this.InvalidateCachedColumnsOrder();
				j = this.items.Add(dataGridViewColumn5);
				dataGridViewColumn5.IndexInternal = j;
				dataGridViewColumn5.DataGridViewInternal = this.dataGridView;
				this.UpdateColumnCaches(dataGridViewColumn5, true);
				this.DataGridView.OnAddedColumn(dataGridViewColumn5);
			}
			this.OnCollectionChanged(new CollectionChangeEventArgs(CollectionChangeAction.Refresh, null), false, new Point(-1, -1));
		}

		// Token: 0x06003446 RID: 13382 RVA: 0x000B8774 File Offset: 0x000B7774
		public virtual void Clear()
		{
			if (this.Count > 0)
			{
				if (this.DataGridView.NoDimensionChangeAllowed)
				{
					throw new InvalidOperationException(SR.GetString("DataGridView_ForbiddenOperationInEventHandler"));
				}
				if (this.DataGridView.InDisplayIndexAdjustments)
				{
					throw new InvalidOperationException(SR.GetString("DataGridView_CannotAlterDisplayIndexWithinAdjustments"));
				}
				for (int i = 0; i < this.Count; i++)
				{
					DataGridViewColumn dataGridViewColumn = this[i];
					dataGridViewColumn.DataGridViewInternal = null;
					if (dataGridViewColumn.HasHeaderCell)
					{
						dataGridViewColumn.HeaderCell.DataGridViewInternal = null;
					}
				}
				DataGridViewColumn[] array = new DataGridViewColumn[this.items.Count];
				this.CopyTo(array, 0);
				this.DataGridView.OnClearingColumns();
				this.InvalidateCachedColumnsOrder();
				this.items.Clear();
				this.InvalidateCachedColumnCounts();
				this.InvalidateCachedColumnsWidths();
				foreach (DataGridViewColumn dataGridViewColumn2 in array)
				{
					this.DataGridView.OnColumnRemoved(dataGridViewColumn2);
					this.DataGridView.OnColumnHidden(dataGridViewColumn2);
				}
				this.OnCollectionChanged(new CollectionChangeEventArgs(CollectionChangeAction.Refresh, null), false, new Point(-1, -1));
			}
		}

		// Token: 0x06003447 RID: 13383 RVA: 0x000B8888 File Offset: 0x000B7888
		internal int ColumnIndexToActualDisplayIndex(int columnIndex, DataGridViewElementStates includeFilter)
		{
			DataGridViewColumn dataGridViewColumn = this.GetFirstColumn(includeFilter);
			int num = 0;
			while (dataGridViewColumn != null && dataGridViewColumn.Index != columnIndex)
			{
				dataGridViewColumn = this.GetNextColumn(dataGridViewColumn, includeFilter, DataGridViewElementStates.None);
				num++;
			}
			return num;
		}

		// Token: 0x06003448 RID: 13384 RVA: 0x000B88BC File Offset: 0x000B78BC
		public virtual bool Contains(DataGridViewColumn dataGridViewColumn)
		{
			return this.items.IndexOf(dataGridViewColumn) != -1;
		}

		// Token: 0x06003449 RID: 13385 RVA: 0x000B88D0 File Offset: 0x000B78D0
		public virtual bool Contains(string columnName)
		{
			if (columnName == null)
			{
				throw new ArgumentNullException("columnName");
			}
			int count = this.items.Count;
			for (int i = 0; i < count; i++)
			{
				DataGridViewColumn dataGridViewColumn = (DataGridViewColumn)this.items[i];
				if (string.Compare(dataGridViewColumn.Name, columnName, true, CultureInfo.InvariantCulture) == 0)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600344A RID: 13386 RVA: 0x000B892C File Offset: 0x000B792C
		public void CopyTo(DataGridViewColumn[] array, int index)
		{
			this.items.CopyTo(array, index);
		}

		// Token: 0x0600344B RID: 13387 RVA: 0x000B893C File Offset: 0x000B793C
		internal bool DisplayInOrder(int columnIndex1, int columnIndex2)
		{
			int displayIndex = ((DataGridViewColumn)this.items[columnIndex1]).DisplayIndex;
			int displayIndex2 = ((DataGridViewColumn)this.items[columnIndex2]).DisplayIndex;
			return displayIndex < displayIndex2;
		}

		// Token: 0x0600344C RID: 13388 RVA: 0x000B897C File Offset: 0x000B797C
		internal DataGridViewColumn GetColumnAtDisplayIndex(int displayIndex)
		{
			if (displayIndex < 0 || displayIndex >= this.items.Count)
			{
				return null;
			}
			DataGridViewColumn dataGridViewColumn = (DataGridViewColumn)this.items[displayIndex];
			if (dataGridViewColumn.DisplayIndex == displayIndex)
			{
				return dataGridViewColumn;
			}
			for (int i = 0; i < this.items.Count; i++)
			{
				dataGridViewColumn = (DataGridViewColumn)this.items[i];
				if (dataGridViewColumn.DisplayIndex == displayIndex)
				{
					return dataGridViewColumn;
				}
			}
			return null;
		}

		// Token: 0x0600344D RID: 13389 RVA: 0x000B89F0 File Offset: 0x000B79F0
		public int GetColumnCount(DataGridViewElementStates includeFilter)
		{
			if ((includeFilter & ~(DataGridViewElementStates.Displayed | DataGridViewElementStates.Frozen | DataGridViewElementStates.ReadOnly | DataGridViewElementStates.Resizable | DataGridViewElementStates.Selected | DataGridViewElementStates.Visible)) != DataGridViewElementStates.None)
			{
				throw new ArgumentException(SR.GetString("DataGridView_InvalidDataGridViewElementStateCombination", new object[] { "includeFilter" }));
			}
			if (includeFilter != DataGridViewElementStates.Visible)
			{
				if (includeFilter == (DataGridViewElementStates.Selected | DataGridViewElementStates.Visible))
				{
					if (this.columnCountsVisibleSelected != -1)
					{
						return this.columnCountsVisibleSelected;
					}
				}
			}
			else if (this.columnCountsVisible != -1)
			{
				return this.columnCountsVisible;
			}
			int num = 0;
			if ((includeFilter & DataGridViewElementStates.Resizable) == DataGridViewElementStates.None)
			{
				for (int i = 0; i < this.items.Count; i++)
				{
					if (((DataGridViewColumn)this.items[i]).StateIncludes(includeFilter))
					{
						num++;
					}
				}
				if (includeFilter != DataGridViewElementStates.Visible)
				{
					if (includeFilter == (DataGridViewElementStates.Selected | DataGridViewElementStates.Visible))
					{
						this.columnCountsVisibleSelected = num;
					}
				}
				else
				{
					this.columnCountsVisible = num;
				}
			}
			else
			{
				DataGridViewElementStates dataGridViewElementStates = includeFilter & ~DataGridViewElementStates.Resizable;
				for (int j = 0; j < this.items.Count; j++)
				{
					if (((DataGridViewColumn)this.items[j]).StateIncludes(dataGridViewElementStates) && ((DataGridViewColumn)this.items[j]).Resizable == DataGridViewTriState.True)
					{
						num++;
					}
				}
			}
			return num;
		}

		// Token: 0x0600344E RID: 13390 RVA: 0x000B8B08 File Offset: 0x000B7B08
		internal int GetColumnCount(DataGridViewElementStates includeFilter, int fromColumnIndex, int toColumnIndex)
		{
			int num = 0;
			DataGridViewColumn dataGridViewColumn = (DataGridViewColumn)this.items[fromColumnIndex];
			while (dataGridViewColumn != (DataGridViewColumn)this.items[toColumnIndex])
			{
				dataGridViewColumn = this.GetNextColumn(dataGridViewColumn, includeFilter, DataGridViewElementStates.None);
				if (dataGridViewColumn.StateIncludes(includeFilter))
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x0600344F RID: 13391 RVA: 0x000B8B58 File Offset: 0x000B7B58
		private int GetColumnSortedIndex(DataGridViewColumn dataGridViewColumn)
		{
			if (this.lastAccessedSortedIndex != -1 && this.itemsSorted[this.lastAccessedSortedIndex] == dataGridViewColumn)
			{
				return this.lastAccessedSortedIndex;
			}
			for (int i = 0; i < this.itemsSorted.Count; i++)
			{
				if (dataGridViewColumn.Index == ((DataGridViewColumn)this.itemsSorted[i]).Index)
				{
					this.lastAccessedSortedIndex = i;
					return i;
				}
			}
			return -1;
		}

		// Token: 0x06003450 RID: 13392 RVA: 0x000B8BC8 File Offset: 0x000B7BC8
		internal float GetColumnsFillWeight(DataGridViewElementStates includeFilter)
		{
			float num = 0f;
			for (int i = 0; i < this.items.Count; i++)
			{
				if (((DataGridViewColumn)this.items[i]).StateIncludes(includeFilter))
				{
					num += ((DataGridViewColumn)this.items[i]).FillWeight;
				}
			}
			return num;
		}

		// Token: 0x06003451 RID: 13393 RVA: 0x000B8C24 File Offset: 0x000B7C24
		public int GetColumnsWidth(DataGridViewElementStates includeFilter)
		{
			if ((includeFilter & ~(DataGridViewElementStates.Displayed | DataGridViewElementStates.Frozen | DataGridViewElementStates.ReadOnly | DataGridViewElementStates.Resizable | DataGridViewElementStates.Selected | DataGridViewElementStates.Visible)) != DataGridViewElementStates.None)
			{
				throw new ArgumentException(SR.GetString("DataGridView_InvalidDataGridViewElementStateCombination", new object[] { "includeFilter" }));
			}
			switch (includeFilter)
			{
			case DataGridViewElementStates.Visible:
				if (this.columnsWidthVisible != -1)
				{
					return this.columnsWidthVisible;
				}
				break;
			case DataGridViewElementStates.Frozen | DataGridViewElementStates.Visible:
				if (this.columnsWidthVisibleFrozen != -1)
				{
					return this.columnsWidthVisibleFrozen;
				}
				break;
			}
			int num = 0;
			for (int i = 0; i < this.items.Count; i++)
			{
				if (((DataGridViewColumn)this.items[i]).StateIncludes(includeFilter))
				{
					num += ((DataGridViewColumn)this.items[i]).Thickness;
				}
			}
			switch (includeFilter)
			{
			case DataGridViewElementStates.Visible:
				this.columnsWidthVisible = num;
				break;
			case DataGridViewElementStates.Frozen | DataGridViewElementStates.Visible:
				this.columnsWidthVisibleFrozen = num;
				break;
			}
			return num;
		}

		// Token: 0x06003452 RID: 13394 RVA: 0x000B8D08 File Offset: 0x000B7D08
		public DataGridViewColumn GetFirstColumn(DataGridViewElementStates includeFilter)
		{
			if ((includeFilter & ~(DataGridViewElementStates.Displayed | DataGridViewElementStates.Frozen | DataGridViewElementStates.ReadOnly | DataGridViewElementStates.Resizable | DataGridViewElementStates.Selected | DataGridViewElementStates.Visible)) != DataGridViewElementStates.None)
			{
				throw new ArgumentException(SR.GetString("DataGridView_InvalidDataGridViewElementStateCombination", new object[] { "includeFilter" }));
			}
			if (this.itemsSorted == null)
			{
				this.UpdateColumnOrderCache();
			}
			for (int i = 0; i < this.itemsSorted.Count; i++)
			{
				DataGridViewColumn dataGridViewColumn = (DataGridViewColumn)this.itemsSorted[i];
				if (dataGridViewColumn.StateIncludes(includeFilter))
				{
					this.lastAccessedSortedIndex = i;
					return dataGridViewColumn;
				}
			}
			return null;
		}

		// Token: 0x06003453 RID: 13395 RVA: 0x000B8D84 File Offset: 0x000B7D84
		public DataGridViewColumn GetFirstColumn(DataGridViewElementStates includeFilter, DataGridViewElementStates excludeFilter)
		{
			if (excludeFilter == DataGridViewElementStates.None)
			{
				return this.GetFirstColumn(includeFilter);
			}
			if ((includeFilter & ~(DataGridViewElementStates.Displayed | DataGridViewElementStates.Frozen | DataGridViewElementStates.ReadOnly | DataGridViewElementStates.Resizable | DataGridViewElementStates.Selected | DataGridViewElementStates.Visible)) != DataGridViewElementStates.None)
			{
				throw new ArgumentException(SR.GetString("DataGridView_InvalidDataGridViewElementStateCombination", new object[] { "includeFilter" }));
			}
			if ((excludeFilter & ~(DataGridViewElementStates.Displayed | DataGridViewElementStates.Frozen | DataGridViewElementStates.ReadOnly | DataGridViewElementStates.Resizable | DataGridViewElementStates.Selected | DataGridViewElementStates.Visible)) != DataGridViewElementStates.None)
			{
				throw new ArgumentException(SR.GetString("DataGridView_InvalidDataGridViewElementStateCombination", new object[] { "excludeFilter" }));
			}
			if (this.itemsSorted == null)
			{
				this.UpdateColumnOrderCache();
			}
			for (int i = 0; i < this.itemsSorted.Count; i++)
			{
				DataGridViewColumn dataGridViewColumn = (DataGridViewColumn)this.itemsSorted[i];
				if (dataGridViewColumn.StateIncludes(includeFilter) && dataGridViewColumn.StateExcludes(excludeFilter))
				{
					this.lastAccessedSortedIndex = i;
					return dataGridViewColumn;
				}
			}
			return null;
		}

		// Token: 0x06003454 RID: 13396 RVA: 0x000B8E3C File Offset: 0x000B7E3C
		public DataGridViewColumn GetLastColumn(DataGridViewElementStates includeFilter, DataGridViewElementStates excludeFilter)
		{
			if ((includeFilter & ~(DataGridViewElementStates.Displayed | DataGridViewElementStates.Frozen | DataGridViewElementStates.ReadOnly | DataGridViewElementStates.Resizable | DataGridViewElementStates.Selected | DataGridViewElementStates.Visible)) != DataGridViewElementStates.None)
			{
				throw new ArgumentException(SR.GetString("DataGridView_InvalidDataGridViewElementStateCombination", new object[] { "includeFilter" }));
			}
			if ((excludeFilter & ~(DataGridViewElementStates.Displayed | DataGridViewElementStates.Frozen | DataGridViewElementStates.ReadOnly | DataGridViewElementStates.Resizable | DataGridViewElementStates.Selected | DataGridViewElementStates.Visible)) != DataGridViewElementStates.None)
			{
				throw new ArgumentException(SR.GetString("DataGridView_InvalidDataGridViewElementStateCombination", new object[] { "excludeFilter" }));
			}
			if (this.itemsSorted == null)
			{
				this.UpdateColumnOrderCache();
			}
			for (int i = this.itemsSorted.Count - 1; i >= 0; i--)
			{
				DataGridViewColumn dataGridViewColumn = (DataGridViewColumn)this.itemsSorted[i];
				if (dataGridViewColumn.StateIncludes(includeFilter) && dataGridViewColumn.StateExcludes(excludeFilter))
				{
					this.lastAccessedSortedIndex = i;
					return dataGridViewColumn;
				}
			}
			return null;
		}

		// Token: 0x06003455 RID: 13397 RVA: 0x000B8EEC File Offset: 0x000B7EEC
		public DataGridViewColumn GetNextColumn(DataGridViewColumn dataGridViewColumnStart, DataGridViewElementStates includeFilter, DataGridViewElementStates excludeFilter)
		{
			if (dataGridViewColumnStart == null)
			{
				throw new ArgumentNullException("dataGridViewColumnStart");
			}
			if ((includeFilter & ~(DataGridViewElementStates.Displayed | DataGridViewElementStates.Frozen | DataGridViewElementStates.ReadOnly | DataGridViewElementStates.Resizable | DataGridViewElementStates.Selected | DataGridViewElementStates.Visible)) != DataGridViewElementStates.None)
			{
				throw new ArgumentException(SR.GetString("DataGridView_InvalidDataGridViewElementStateCombination", new object[] { "includeFilter" }));
			}
			if ((excludeFilter & ~(DataGridViewElementStates.Displayed | DataGridViewElementStates.Frozen | DataGridViewElementStates.ReadOnly | DataGridViewElementStates.Resizable | DataGridViewElementStates.Selected | DataGridViewElementStates.Visible)) != DataGridViewElementStates.None)
			{
				throw new ArgumentException(SR.GetString("DataGridView_InvalidDataGridViewElementStateCombination", new object[] { "excludeFilter" }));
			}
			if (this.itemsSorted == null)
			{
				this.UpdateColumnOrderCache();
			}
			int i = this.GetColumnSortedIndex(dataGridViewColumnStart);
			if (i != -1)
			{
				for (i++; i < this.itemsSorted.Count; i++)
				{
					DataGridViewColumn dataGridViewColumn = (DataGridViewColumn)this.itemsSorted[i];
					if (dataGridViewColumn.StateIncludes(includeFilter) && dataGridViewColumn.StateExcludes(excludeFilter))
					{
						this.lastAccessedSortedIndex = i;
						return dataGridViewColumn;
					}
				}
				return null;
			}
			bool flag = false;
			int num = int.MaxValue;
			int num2 = int.MaxValue;
			for (i = 0; i < this.items.Count; i++)
			{
				DataGridViewColumn dataGridViewColumn2 = (DataGridViewColumn)this.items[i];
				if (dataGridViewColumn2.StateIncludes(includeFilter) && dataGridViewColumn2.StateExcludes(excludeFilter) && (dataGridViewColumn2.DisplayIndex > dataGridViewColumnStart.DisplayIndex || (dataGridViewColumn2.DisplayIndex == dataGridViewColumnStart.DisplayIndex && dataGridViewColumn2.Index > dataGridViewColumnStart.Index)) && (dataGridViewColumn2.DisplayIndex < num2 || (dataGridViewColumn2.DisplayIndex == num2 && dataGridViewColumn2.Index < num)))
				{
					num = i;
					num2 = dataGridViewColumn2.DisplayIndex;
					flag = true;
				}
			}
			if (!flag)
			{
				return null;
			}
			return (DataGridViewColumn)this.items[num];
		}

		// Token: 0x06003456 RID: 13398 RVA: 0x000B9080 File Offset: 0x000B8080
		public DataGridViewColumn GetPreviousColumn(DataGridViewColumn dataGridViewColumnStart, DataGridViewElementStates includeFilter, DataGridViewElementStates excludeFilter)
		{
			if (dataGridViewColumnStart == null)
			{
				throw new ArgumentNullException("dataGridViewColumnStart");
			}
			if ((includeFilter & ~(DataGridViewElementStates.Displayed | DataGridViewElementStates.Frozen | DataGridViewElementStates.ReadOnly | DataGridViewElementStates.Resizable | DataGridViewElementStates.Selected | DataGridViewElementStates.Visible)) != DataGridViewElementStates.None)
			{
				throw new ArgumentException(SR.GetString("DataGridView_InvalidDataGridViewElementStateCombination", new object[] { "includeFilter" }));
			}
			if ((excludeFilter & ~(DataGridViewElementStates.Displayed | DataGridViewElementStates.Frozen | DataGridViewElementStates.ReadOnly | DataGridViewElementStates.Resizable | DataGridViewElementStates.Selected | DataGridViewElementStates.Visible)) != DataGridViewElementStates.None)
			{
				throw new ArgumentException(SR.GetString("DataGridView_InvalidDataGridViewElementStateCombination", new object[] { "excludeFilter" }));
			}
			if (this.itemsSorted == null)
			{
				this.UpdateColumnOrderCache();
			}
			int i = this.GetColumnSortedIndex(dataGridViewColumnStart);
			if (i != -1)
			{
				for (i--; i >= 0; i--)
				{
					DataGridViewColumn dataGridViewColumn = (DataGridViewColumn)this.itemsSorted[i];
					if (dataGridViewColumn.StateIncludes(includeFilter) && dataGridViewColumn.StateExcludes(excludeFilter))
					{
						this.lastAccessedSortedIndex = i;
						return dataGridViewColumn;
					}
				}
				return null;
			}
			bool flag = false;
			int num = -1;
			int num2 = -1;
			for (i = 0; i < this.items.Count; i++)
			{
				DataGridViewColumn dataGridViewColumn2 = (DataGridViewColumn)this.items[i];
				if (dataGridViewColumn2.StateIncludes(includeFilter) && dataGridViewColumn2.StateExcludes(excludeFilter) && (dataGridViewColumn2.DisplayIndex < dataGridViewColumnStart.DisplayIndex || (dataGridViewColumn2.DisplayIndex == dataGridViewColumnStart.DisplayIndex && dataGridViewColumn2.Index < dataGridViewColumnStart.Index)) && (dataGridViewColumn2.DisplayIndex > num2 || (dataGridViewColumn2.DisplayIndex == num2 && dataGridViewColumn2.Index > num)))
				{
					num = i;
					num2 = dataGridViewColumn2.DisplayIndex;
					flag = true;
				}
			}
			if (!flag)
			{
				return null;
			}
			return (DataGridViewColumn)this.items[num];
		}

		// Token: 0x06003457 RID: 13399 RVA: 0x000B9201 File Offset: 0x000B8201
		public int IndexOf(DataGridViewColumn dataGridViewColumn)
		{
			return this.items.IndexOf(dataGridViewColumn);
		}

		// Token: 0x06003458 RID: 13400 RVA: 0x000B9210 File Offset: 0x000B8210
		public virtual void Insert(int columnIndex, DataGridViewColumn dataGridViewColumn)
		{
			if (this.DataGridView.NoDimensionChangeAllowed)
			{
				throw new InvalidOperationException(SR.GetString("DataGridView_ForbiddenOperationInEventHandler"));
			}
			if (this.DataGridView.InDisplayIndexAdjustments)
			{
				throw new InvalidOperationException(SR.GetString("DataGridView_CannotAlterDisplayIndexWithinAdjustments"));
			}
			if (dataGridViewColumn == null)
			{
				throw new ArgumentNullException("dataGridViewColumn");
			}
			int displayIndex = dataGridViewColumn.DisplayIndex;
			if (displayIndex == -1)
			{
				dataGridViewColumn.DisplayIndex = columnIndex;
			}
			Point point;
			try
			{
				this.DataGridView.OnInsertingColumn(columnIndex, dataGridViewColumn, out point);
			}
			finally
			{
				dataGridViewColumn.DisplayIndexInternal = displayIndex;
			}
			this.InvalidateCachedColumnsOrder();
			this.items.Insert(columnIndex, dataGridViewColumn);
			dataGridViewColumn.IndexInternal = columnIndex;
			dataGridViewColumn.DataGridViewInternal = this.dataGridView;
			this.UpdateColumnCaches(dataGridViewColumn, true);
			this.DataGridView.OnInsertedColumn_PreNotification(dataGridViewColumn);
			this.OnCollectionChanged(new CollectionChangeEventArgs(CollectionChangeAction.Add, dataGridViewColumn), true, point);
		}

		// Token: 0x06003459 RID: 13401 RVA: 0x000B92EC File Offset: 0x000B82EC
		internal void InvalidateCachedColumnCount(DataGridViewElementStates includeFilter)
		{
			if (includeFilter == DataGridViewElementStates.Visible)
			{
				this.InvalidateCachedColumnCounts();
				return;
			}
			if (includeFilter == DataGridViewElementStates.Selected)
			{
				this.columnCountsVisibleSelected = -1;
			}
		}

		// Token: 0x0600345A RID: 13402 RVA: 0x000B9308 File Offset: 0x000B8308
		internal void InvalidateCachedColumnCounts()
		{
			this.columnCountsVisible = (this.columnCountsVisibleSelected = -1);
		}

		// Token: 0x0600345B RID: 13403 RVA: 0x000B9325 File Offset: 0x000B8325
		internal void InvalidateCachedColumnsOrder()
		{
			this.itemsSorted = null;
		}

		// Token: 0x0600345C RID: 13404 RVA: 0x000B932E File Offset: 0x000B832E
		internal void InvalidateCachedColumnsWidth(DataGridViewElementStates includeFilter)
		{
			if (includeFilter == DataGridViewElementStates.Visible)
			{
				this.InvalidateCachedColumnsWidths();
				return;
			}
			if (includeFilter == DataGridViewElementStates.Frozen)
			{
				this.columnsWidthVisibleFrozen = -1;
			}
		}

		// Token: 0x0600345D RID: 13405 RVA: 0x000B9348 File Offset: 0x000B8348
		internal void InvalidateCachedColumnsWidths()
		{
			this.columnsWidthVisible = (this.columnsWidthVisibleFrozen = -1);
		}

		// Token: 0x0600345E RID: 13406 RVA: 0x000B9365 File Offset: 0x000B8365
		protected virtual void OnCollectionChanged(CollectionChangeEventArgs e)
		{
			if (this.onCollectionChanged != null)
			{
				this.onCollectionChanged(this, e);
			}
		}

		// Token: 0x0600345F RID: 13407 RVA: 0x000B937C File Offset: 0x000B837C
		private void OnCollectionChanged(CollectionChangeEventArgs ccea, bool changeIsInsertion, Point newCurrentCell)
		{
			this.OnCollectionChanged_PreNotification(ccea);
			this.OnCollectionChanged(ccea);
			this.OnCollectionChanged_PostNotification(ccea, changeIsInsertion, newCurrentCell);
		}

		// Token: 0x06003460 RID: 13408 RVA: 0x000B9395 File Offset: 0x000B8395
		private void OnCollectionChanged_PreNotification(CollectionChangeEventArgs ccea)
		{
			this.DataGridView.OnColumnCollectionChanged_PreNotification(ccea);
		}

		// Token: 0x06003461 RID: 13409 RVA: 0x000B93A4 File Offset: 0x000B83A4
		private void OnCollectionChanged_PostNotification(CollectionChangeEventArgs ccea, bool changeIsInsertion, Point newCurrentCell)
		{
			DataGridViewColumn dataGridViewColumn = (DataGridViewColumn)ccea.Element;
			if (ccea.Action == CollectionChangeAction.Add && changeIsInsertion)
			{
				this.DataGridView.OnInsertedColumn_PostNotification(newCurrentCell);
			}
			else if (ccea.Action == CollectionChangeAction.Remove)
			{
				this.DataGridView.OnRemovedColumn_PostNotification(dataGridViewColumn, newCurrentCell);
			}
			this.DataGridView.OnColumnCollectionChanged_PostNotification(dataGridViewColumn);
		}

		// Token: 0x06003462 RID: 13410 RVA: 0x000B93FC File Offset: 0x000B83FC
		public virtual void Remove(DataGridViewColumn dataGridViewColumn)
		{
			if (dataGridViewColumn == null)
			{
				throw new ArgumentNullException("dataGridViewColumn");
			}
			if (dataGridViewColumn.DataGridView != this.DataGridView)
			{
				throw new ArgumentException(SR.GetString("DataGridView_ColumnDoesNotBelongToDataGridView"), "dataGridViewColumn");
			}
			int count = this.items.Count;
			for (int i = 0; i < count; i++)
			{
				if (this.items[i] == dataGridViewColumn)
				{
					this.RemoveAt(i);
					return;
				}
			}
		}

		// Token: 0x06003463 RID: 13411 RVA: 0x000B946C File Offset: 0x000B846C
		public virtual void Remove(string columnName)
		{
			if (columnName == null)
			{
				throw new ArgumentNullException("columnName");
			}
			int count = this.items.Count;
			for (int i = 0; i < count; i++)
			{
				DataGridViewColumn dataGridViewColumn = (DataGridViewColumn)this.items[i];
				if (string.Compare(dataGridViewColumn.Name, columnName, true, CultureInfo.InvariantCulture) == 0)
				{
					this.RemoveAt(i);
					return;
				}
			}
			throw new ArgumentException(SR.GetString("DataGridViewColumnCollection_ColumnNotFound", new object[] { columnName }), "columnName");
		}

		// Token: 0x06003464 RID: 13412 RVA: 0x000B94F0 File Offset: 0x000B84F0
		public virtual void RemoveAt(int index)
		{
			if (index < 0 || index >= this.Count)
			{
				throw new ArgumentOutOfRangeException("index", SR.GetString("InvalidArgument", new object[]
				{
					"index",
					index.ToString(CultureInfo.CurrentCulture)
				}));
			}
			if (this.DataGridView.NoDimensionChangeAllowed)
			{
				throw new InvalidOperationException(SR.GetString("DataGridView_ForbiddenOperationInEventHandler"));
			}
			if (this.DataGridView.InDisplayIndexAdjustments)
			{
				throw new InvalidOperationException(SR.GetString("DataGridView_CannotAlterDisplayIndexWithinAdjustments"));
			}
			this.RemoveAtInternal(index, false);
		}

		// Token: 0x06003465 RID: 13413 RVA: 0x000B9580 File Offset: 0x000B8580
		internal void RemoveAtInternal(int index, bool force)
		{
			DataGridViewColumn dataGridViewColumn = (DataGridViewColumn)this.items[index];
			Point point;
			this.DataGridView.OnRemovingColumn(dataGridViewColumn, out point, force);
			this.InvalidateCachedColumnsOrder();
			this.items.RemoveAt(index);
			dataGridViewColumn.DataGridViewInternal = null;
			this.UpdateColumnCaches(dataGridViewColumn, false);
			this.DataGridView.OnRemovedColumn_PreNotification(dataGridViewColumn);
			this.OnCollectionChanged(new CollectionChangeEventArgs(CollectionChangeAction.Remove, dataGridViewColumn), false, point);
		}

		// Token: 0x06003466 RID: 13414 RVA: 0x000B95EC File Offset: 0x000B85EC
		private void UpdateColumnCaches(DataGridViewColumn dataGridViewColumn, bool adding)
		{
			if (this.columnCountsVisible != -1 || this.columnCountsVisibleSelected != -1 || this.columnsWidthVisible != -1 || this.columnsWidthVisibleFrozen != -1)
			{
				DataGridViewElementStates state = dataGridViewColumn.State;
				if ((state & DataGridViewElementStates.Visible) != DataGridViewElementStates.None)
				{
					int num = (adding ? 1 : (-1));
					int num2 = 0;
					if (this.columnsWidthVisible != -1 || (this.columnsWidthVisibleFrozen != -1 && (state & (DataGridViewElementStates.Frozen | DataGridViewElementStates.Visible)) == (DataGridViewElementStates.Frozen | DataGridViewElementStates.Visible)))
					{
						num2 = (adding ? dataGridViewColumn.Width : (-dataGridViewColumn.Width));
					}
					if (this.columnCountsVisible != -1)
					{
						this.columnCountsVisible += num;
					}
					if (this.columnsWidthVisible != -1)
					{
						this.columnsWidthVisible += num2;
					}
					if ((state & (DataGridViewElementStates.Frozen | DataGridViewElementStates.Visible)) == (DataGridViewElementStates.Frozen | DataGridViewElementStates.Visible) && this.columnsWidthVisibleFrozen != -1)
					{
						this.columnsWidthVisibleFrozen += num2;
					}
					if ((state & (DataGridViewElementStates.Selected | DataGridViewElementStates.Visible)) == (DataGridViewElementStates.Selected | DataGridViewElementStates.Visible) && this.columnCountsVisibleSelected != -1)
					{
						this.columnCountsVisibleSelected += num;
					}
				}
			}
		}

		// Token: 0x06003467 RID: 13415 RVA: 0x000B96D3 File Offset: 0x000B86D3
		private void UpdateColumnOrderCache()
		{
			this.itemsSorted = (ArrayList)this.items.Clone();
			this.itemsSorted.Sort(DataGridViewColumnCollection.columnOrderComparer);
			this.lastAccessedSortedIndex = -1;
		}

		// Token: 0x04001B1C RID: 6940
		private CollectionChangeEventHandler onCollectionChanged;

		// Token: 0x04001B1D RID: 6941
		private ArrayList items = new ArrayList();

		// Token: 0x04001B1E RID: 6942
		private ArrayList itemsSorted;

		// Token: 0x04001B1F RID: 6943
		private int lastAccessedSortedIndex = -1;

		// Token: 0x04001B20 RID: 6944
		private int columnCountsVisible;

		// Token: 0x04001B21 RID: 6945
		private int columnCountsVisibleSelected;

		// Token: 0x04001B22 RID: 6946
		private int columnsWidthVisible;

		// Token: 0x04001B23 RID: 6947
		private int columnsWidthVisibleFrozen;

		// Token: 0x04001B24 RID: 6948
		private static DataGridViewColumnCollection.ColumnOrderComparer columnOrderComparer = new DataGridViewColumnCollection.ColumnOrderComparer();

		// Token: 0x04001B25 RID: 6949
		private DataGridView dataGridView;

		// Token: 0x02000330 RID: 816
		private class ColumnOrderComparer : IComparer
		{
			// Token: 0x0600346A RID: 13418 RVA: 0x000B9718 File Offset: 0x000B8718
			public int Compare(object x, object y)
			{
				DataGridViewColumn dataGridViewColumn = x as DataGridViewColumn;
				DataGridViewColumn dataGridViewColumn2 = y as DataGridViewColumn;
				return dataGridViewColumn.DisplayIndex - dataGridViewColumn2.DisplayIndex;
			}
		}
	}
}
