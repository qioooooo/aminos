using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Common;
using System.Globalization;
using System.Text;
using System.Threading;

namespace System.Data
{
	// Token: 0x020000A6 RID: 166
	[Editor("Microsoft.VSDesigner.Data.Design.DataSourceEditor, Microsoft.VSDesigner, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", "System.Drawing.Design.UITypeEditor, System.Drawing, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	[DefaultProperty("Table")]
	[DefaultEvent("PositionChanged")]
	[Designer("Microsoft.VSDesigner.Data.VS.DataViewDesigner, Microsoft.VSDesigner, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	public class DataView : MarshalByValueComponent, IBindingListView, IBindingList, IList, ICollection, IEnumerable, ITypedList, ISupportInitializeNotification, ISupportInitialize
	{
		// Token: 0x06000B1E RID: 2846 RVA: 0x001F6194 File Offset: 0x001F5594
		internal DataView(DataTable table, bool locked)
		{
			GC.SuppressFinalize(this);
			Bid.Trace("<ds.DataView.DataView|INFO> %d#, table=%d, locked=%d{bool}\n", this.ObjectID, (table != null) ? table.ObjectID : 0, locked);
			this.dvListener = new DataViewListener(this);
			this.locked = locked;
			this.table = table;
			this.dvListener.RegisterMetaDataEvents(this.table);
		}

		// Token: 0x06000B1F RID: 2847 RVA: 0x001F625C File Offset: 0x001F565C
		public DataView()
			: this(null)
		{
			this.SetIndex2("", DataViewRowState.CurrentRows, null, true);
		}

		// Token: 0x06000B20 RID: 2848 RVA: 0x001F6280 File Offset: 0x001F5680
		public DataView(DataTable table)
			: this(table, false)
		{
			this.SetIndex2("", DataViewRowState.CurrentRows, null, true);
		}

		// Token: 0x06000B21 RID: 2849 RVA: 0x001F62A4 File Offset: 0x001F56A4
		public DataView(DataTable table, string RowFilter, string Sort, DataViewRowState RowState)
		{
			GC.SuppressFinalize(this);
			Bid.Trace("<ds.DataView.DataView|API> %d#, table=%d, RowFilter='%ls', Sort='%ls', RowState=%d{ds.DataViewRowState}\n", this.ObjectID, (table != null) ? table.ObjectID : 0, RowFilter, Sort, (int)RowState);
			if (table == null)
			{
				throw ExceptionBuilder.CanNotUse();
			}
			this.dvListener = new DataViewListener(this);
			this.locked = false;
			this.table = table;
			this.dvListener.RegisterMetaDataEvents(this.table);
			if ((RowState & ~(DataViewRowState.Unchanged | DataViewRowState.Added | DataViewRowState.Deleted | DataViewRowState.ModifiedCurrent | DataViewRowState.ModifiedOriginal)) != DataViewRowState.None)
			{
				throw ExceptionBuilder.RecordStateRange();
			}
			if ((RowState & DataViewRowState.ModifiedOriginal) != DataViewRowState.None && (RowState & DataViewRowState.ModifiedCurrent) != DataViewRowState.None)
			{
				throw ExceptionBuilder.SetRowStateFilter();
			}
			if (Sort == null)
			{
				Sort = "";
			}
			if (RowFilter == null)
			{
				RowFilter = "";
			}
			DataExpression dataExpression = new DataExpression(table, RowFilter);
			this.SetIndex(Sort, RowState, dataExpression);
		}

		// Token: 0x06000B22 RID: 2850 RVA: 0x001F63C0 File Offset: 0x001F57C0
		internal DataView(DataTable table, Predicate<DataRow> predicate, Comparison<DataRow> comparison, DataViewRowState RowState)
		{
			GC.SuppressFinalize(this);
			Bid.Trace("<ds.DataView.DataView|API> %d#, table=%d, RowState=%d{ds.DataViewRowState}\n", this.ObjectID, (table != null) ? table.ObjectID : 0, (int)RowState);
			if (table == null)
			{
				throw ExceptionBuilder.CanNotUse();
			}
			this.dvListener = new DataViewListener(this);
			this.locked = false;
			this.table = table;
			this.dvListener.RegisterMetaDataEvents(this.table);
			if ((RowState & ~(DataViewRowState.Unchanged | DataViewRowState.Added | DataViewRowState.Deleted | DataViewRowState.ModifiedCurrent | DataViewRowState.ModifiedOriginal)) != DataViewRowState.None)
			{
				throw ExceptionBuilder.RecordStateRange();
			}
			if ((RowState & DataViewRowState.ModifiedOriginal) != DataViewRowState.None && (RowState & DataViewRowState.ModifiedCurrent) != DataViewRowState.None)
			{
				throw ExceptionBuilder.SetRowStateFilter();
			}
			this._comparison = comparison;
			this.SetIndex2("", RowState, (predicate != null) ? new DataView.RowPredicateFilter(predicate) : null, true);
		}

		// Token: 0x1700016F RID: 367
		// (get) Token: 0x06000B23 RID: 2851 RVA: 0x001F64D4 File Offset: 0x001F58D4
		// (set) Token: 0x06000B24 RID: 2852 RVA: 0x001F64E8 File Offset: 0x001F58E8
		[ResCategory("DataCategory_Data")]
		[DefaultValue(true)]
		[ResDescription("DataViewAllowDeleteDescr")]
		public bool AllowDelete
		{
			get
			{
				return this.allowDelete;
			}
			set
			{
				if (this.allowDelete != value)
				{
					this.allowDelete = value;
					this.OnListChanged(DataView.ResetEventArgs);
				}
			}
		}

		// Token: 0x17000170 RID: 368
		// (get) Token: 0x06000B25 RID: 2853 RVA: 0x001F6510 File Offset: 0x001F5910
		// (set) Token: 0x06000B26 RID: 2854 RVA: 0x001F6524 File Offset: 0x001F5924
		[ResCategory("DataCategory_Data")]
		[ResDescription("DataViewApplyDefaultSortDescr")]
		[RefreshProperties(RefreshProperties.All)]
		[DefaultValue(false)]
		public bool ApplyDefaultSort
		{
			get
			{
				return this.applyDefaultSort;
			}
			set
			{
				Bid.Trace("<ds.DataView.set_ApplyDefaultSort|API> %d#, %d{bool}\n", this.ObjectID, value);
				if (this.applyDefaultSort != value)
				{
					this._comparison = null;
					this.applyDefaultSort = value;
					this.UpdateIndex(true);
					this.OnListChanged(DataView.ResetEventArgs);
				}
			}
		}

		// Token: 0x17000171 RID: 369
		// (get) Token: 0x06000B27 RID: 2855 RVA: 0x001F656C File Offset: 0x001F596C
		// (set) Token: 0x06000B28 RID: 2856 RVA: 0x001F6580 File Offset: 0x001F5980
		[ResDescription("DataViewAllowEditDescr")]
		[ResCategory("DataCategory_Data")]
		[DefaultValue(true)]
		public bool AllowEdit
		{
			get
			{
				return this.allowEdit;
			}
			set
			{
				if (this.allowEdit != value)
				{
					this.allowEdit = value;
					this.OnListChanged(DataView.ResetEventArgs);
				}
			}
		}

		// Token: 0x17000172 RID: 370
		// (get) Token: 0x06000B29 RID: 2857 RVA: 0x001F65A8 File Offset: 0x001F59A8
		// (set) Token: 0x06000B2A RID: 2858 RVA: 0x001F65BC File Offset: 0x001F59BC
		[ResCategory("DataCategory_Data")]
		[DefaultValue(true)]
		[ResDescription("DataViewAllowNewDescr")]
		public bool AllowNew
		{
			get
			{
				return this.allowNew;
			}
			set
			{
				if (this.allowNew != value)
				{
					this.allowNew = value;
					this.OnListChanged(DataView.ResetEventArgs);
				}
			}
		}

		// Token: 0x17000173 RID: 371
		// (get) Token: 0x06000B2B RID: 2859 RVA: 0x001F65E4 File Offset: 0x001F59E4
		[Browsable(false)]
		[ResDescription("DataViewCountDescr")]
		public int Count
		{
			get
			{
				return this.rowViewCache.Count;
			}
		}

		// Token: 0x17000174 RID: 372
		// (get) Token: 0x06000B2C RID: 2860 RVA: 0x001F65FC File Offset: 0x001F59FC
		private int CountFromIndex
		{
			get
			{
				return ((this.index != null) ? this.index.RecordCount : 0) + ((this.addNewRow != null) ? 1 : 0);
			}
		}

		// Token: 0x17000175 RID: 373
		// (get) Token: 0x06000B2D RID: 2861 RVA: 0x001F662C File Offset: 0x001F5A2C
		[ResDescription("DataViewDataViewManagerDescr")]
		[Browsable(false)]
		public DataViewManager DataViewManager
		{
			get
			{
				return this.dataViewManager;
			}
		}

		// Token: 0x17000176 RID: 374
		// (get) Token: 0x06000B2E RID: 2862 RVA: 0x001F6640 File Offset: 0x001F5A40
		[Browsable(false)]
		public bool IsInitialized
		{
			get
			{
				return !this.fInitInProgress;
			}
		}

		// Token: 0x17000177 RID: 375
		// (get) Token: 0x06000B2F RID: 2863 RVA: 0x001F6658 File Offset: 0x001F5A58
		[ResDescription("DataViewIsOpenDescr")]
		[Browsable(false)]
		protected bool IsOpen
		{
			get
			{
				return this.open;
			}
		}

		// Token: 0x17000178 RID: 376
		// (get) Token: 0x06000B30 RID: 2864 RVA: 0x001F666C File Offset: 0x001F5A6C
		bool ICollection.IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000179 RID: 377
		// (get) Token: 0x06000B31 RID: 2865 RVA: 0x001F667C File Offset: 0x001F5A7C
		// (set) Token: 0x06000B32 RID: 2866 RVA: 0x001F66A4 File Offset: 0x001F5AA4
		[ResDescription("DataViewRowFilterDescr")]
		[ResCategory("DataCategory_Data")]
		[DefaultValue("")]
		public virtual string RowFilter
		{
			get
			{
				DataExpression dataExpression = this.rowFilter as DataExpression;
				if (dataExpression != null)
				{
					return dataExpression.Expression;
				}
				return "";
			}
			set
			{
				if (value == null)
				{
					value = "";
				}
				Bid.Trace("<ds.DataView.set_RowFilter|API> %d#, '%ls'\n", this.ObjectID, value);
				if (this.fInitInProgress)
				{
					this.delayedRowFilter = value;
					return;
				}
				CultureInfo cultureInfo = ((this.table != null) ? this.table.Locale : CultureInfo.CurrentCulture);
				if (this.rowFilter == null || string.Compare(this.RowFilter, value, false, cultureInfo) != 0)
				{
					DataExpression dataExpression = new DataExpression(this.table, value);
					this.SetIndex(this.sort, this.recordStates, dataExpression);
				}
			}
		}

		// Token: 0x1700017A RID: 378
		// (get) Token: 0x06000B33 RID: 2867 RVA: 0x001F6730 File Offset: 0x001F5B30
		// (set) Token: 0x06000B34 RID: 2868 RVA: 0x001F6754 File Offset: 0x001F5B54
		internal Predicate<DataRow> RowPredicate
		{
			get
			{
				DataView.RowPredicateFilter rowPredicateFilter = this.GetFilter() as DataView.RowPredicateFilter;
				if (rowPredicateFilter == null)
				{
					return null;
				}
				return rowPredicateFilter.PredicateFilter;
			}
			set
			{
				if (!object.ReferenceEquals(this.RowPredicate, value))
				{
					this.SetIndex(this.Sort, this.RowStateFilter, (value != null) ? new DataView.RowPredicateFilter(value) : null);
				}
			}
		}

		// Token: 0x1700017B RID: 379
		// (get) Token: 0x06000B35 RID: 2869 RVA: 0x001F6790 File Offset: 0x001F5B90
		// (set) Token: 0x06000B36 RID: 2870 RVA: 0x001F67A4 File Offset: 0x001F5BA4
		[DefaultValue(DataViewRowState.CurrentRows)]
		[ResCategory("DataCategory_Data")]
		[ResDescription("DataViewRowStateFilterDescr")]
		public DataViewRowState RowStateFilter
		{
			get
			{
				return this.recordStates;
			}
			set
			{
				Bid.Trace("<ds.DataView.set_RowStateFilter|API> %d#, %d{ds.DataViewRowState}\n", this.ObjectID, (int)value);
				if (this.fInitInProgress)
				{
					this.delayedRecordStates = value;
					return;
				}
				if ((value & ~(DataViewRowState.Unchanged | DataViewRowState.Added | DataViewRowState.Deleted | DataViewRowState.ModifiedCurrent | DataViewRowState.ModifiedOriginal)) != DataViewRowState.None)
				{
					throw ExceptionBuilder.RecordStateRange();
				}
				if ((value & DataViewRowState.ModifiedOriginal) != DataViewRowState.None && (value & DataViewRowState.ModifiedCurrent) != DataViewRowState.None)
				{
					throw ExceptionBuilder.SetRowStateFilter();
				}
				if (this.recordStates != value)
				{
					this.SetIndex(this.sort, value, this.rowFilter);
				}
			}
		}

		// Token: 0x1700017C RID: 380
		// (get) Token: 0x06000B37 RID: 2871 RVA: 0x001F680C File Offset: 0x001F5C0C
		// (set) Token: 0x06000B38 RID: 2872 RVA: 0x001F6864 File Offset: 0x001F5C64
		[ResDescription("DataViewSortDescr")]
		[ResCategory("DataCategory_Data")]
		[DefaultValue("")]
		public string Sort
		{
			get
			{
				if (this.sort.Length == 0 && this.applyDefaultSort && this.table != null && this.table._primaryIndex.Length > 0)
				{
					return this.table.FormatSortString(this.table._primaryIndex);
				}
				return this.sort;
			}
			set
			{
				if (value == null)
				{
					value = "";
				}
				Bid.Trace("<ds.DataView.set_Sort|API> %d#, '%ls'\n", this.ObjectID, value);
				if (this.fInitInProgress)
				{
					this.delayedSort = value;
					return;
				}
				CultureInfo cultureInfo = ((this.table != null) ? this.table.Locale : CultureInfo.CurrentCulture);
				if (string.Compare(this.sort, value, false, cultureInfo) != 0 || this._comparison != null)
				{
					this.CheckSort(value);
					this._comparison = null;
					this.SetIndex(value, this.recordStates, this.rowFilter);
				}
			}
		}

		// Token: 0x1700017D RID: 381
		// (get) Token: 0x06000B39 RID: 2873 RVA: 0x001F68F0 File Offset: 0x001F5CF0
		// (set) Token: 0x06000B3A RID: 2874 RVA: 0x001F6904 File Offset: 0x001F5D04
		internal Comparison<DataRow> SortComparison
		{
			get
			{
				return this._comparison;
			}
			set
			{
				Bid.Trace("<ds.DataView.set_SortComparison|API> %d#\n", this.ObjectID);
				if (!object.ReferenceEquals(this._comparison, value))
				{
					this._comparison = value;
					this.SetIndex("", this.recordStates, this.rowFilter);
				}
			}
		}

		// Token: 0x06000B3B RID: 2875 RVA: 0x001F6950 File Offset: 0x001F5D50
		private void ResetSort()
		{
			this.sort = "";
			this.SetIndex(this.sort, this.recordStates, this.rowFilter);
		}

		// Token: 0x06000B3C RID: 2876 RVA: 0x001F6980 File Offset: 0x001F5D80
		private bool ShouldSerializeSort()
		{
			return this.sort != null;
		}

		// Token: 0x1700017E RID: 382
		// (get) Token: 0x06000B3D RID: 2877 RVA: 0x001F699C File Offset: 0x001F5D9C
		object ICollection.SyncRoot
		{
			get
			{
				return this;
			}
		}

		// Token: 0x1700017F RID: 383
		// (get) Token: 0x06000B3E RID: 2878 RVA: 0x001F69AC File Offset: 0x001F5DAC
		// (set) Token: 0x06000B3F RID: 2879 RVA: 0x001F69C0 File Offset: 0x001F5DC0
		[RefreshProperties(RefreshProperties.All)]
		[ResDescription("DataViewTableDescr")]
		[TypeConverter(typeof(DataTableTypeConverter))]
		[ResCategory("DataCategory_Data")]
		[DefaultValue(null)]
		public DataTable Table
		{
			get
			{
				return this.table;
			}
			set
			{
				Bid.Trace("<ds.DataView.set_Table|API> %d#, %d\n", this.ObjectID, (value != null) ? value.ObjectID : 0);
				if (this.fInitInProgress && value != null)
				{
					this.delayedTable = value;
					return;
				}
				if (this.locked)
				{
					throw ExceptionBuilder.SetTable();
				}
				if (this.dataViewManager != null)
				{
					throw ExceptionBuilder.CanNotSetTable();
				}
				if (value != null && value.TableName.Length == 0)
				{
					throw ExceptionBuilder.CanNotBindTable();
				}
				if (this.table != value)
				{
					this.dvListener.UnregisterMetaDataEvents();
					this.table = value;
					if (this.table != null)
					{
						this.dvListener.RegisterMetaDataEvents(this.table);
					}
					this.SetIndex2("", DataViewRowState.CurrentRows, null, false);
					if (this.table != null)
					{
						this.OnListChanged(new ListChangedEventArgs(ListChangedType.PropertyDescriptorChanged, new DataTablePropertyDescriptor(this.table)));
					}
					this.OnListChanged(DataView.ResetEventArgs);
				}
			}
		}

		// Token: 0x17000180 RID: 384
		object IList.this[int recordIndex]
		{
			get
			{
				return this[recordIndex];
			}
			set
			{
				throw ExceptionBuilder.SetIListObject();
			}
		}

		// Token: 0x17000181 RID: 385
		public DataRowView this[int recordIndex]
		{
			get
			{
				return this.GetRowView(this.GetRow(recordIndex));
			}
		}

		// Token: 0x06000B43 RID: 2883 RVA: 0x001F6AE0 File Offset: 0x001F5EE0
		public virtual DataRowView AddNew()
		{
			IntPtr intPtr;
			Bid.ScopeEnter(out intPtr, "<ds.DataView.AddNew|API> %d#\n", this.ObjectID);
			DataRowView dataRowView2;
			try
			{
				this.CheckOpen();
				if (!this.AllowNew)
				{
					throw ExceptionBuilder.AddNewNotAllowNull();
				}
				if (this.addNewRow != null)
				{
					this.rowViewCache[this.addNewRow].EndEdit();
				}
				this.addNewRow = this.table.NewRow();
				DataRowView dataRowView = new DataRowView(this, this.addNewRow);
				this.rowViewCache.Add(this.addNewRow, dataRowView);
				this.OnListChanged(new ListChangedEventArgs(ListChangedType.ItemAdded, this.IndexOf(dataRowView)));
				dataRowView2 = dataRowView;
			}
			finally
			{
				Bid.ScopeLeave(ref intPtr);
			}
			return dataRowView2;
		}

		// Token: 0x06000B44 RID: 2884 RVA: 0x001F6BA0 File Offset: 0x001F5FA0
		public void BeginInit()
		{
			this.fInitInProgress = true;
		}

		// Token: 0x06000B45 RID: 2885 RVA: 0x001F6BB4 File Offset: 0x001F5FB4
		public void EndInit()
		{
			if (this.delayedTable != null && this.delayedTable.fInitInProgress)
			{
				this.delayedTable.delayedViews.Add(this);
				return;
			}
			this.fInitInProgress = false;
			this.fEndInitInProgress = true;
			if (this.delayedTable != null)
			{
				this.Table = this.delayedTable;
				this.delayedTable = null;
			}
			if (this.delayedSort != null)
			{
				this.Sort = this.delayedSort;
				this.delayedSort = null;
			}
			if (this.delayedRowFilter != null)
			{
				this.RowFilter = this.delayedRowFilter;
				this.delayedRowFilter = null;
			}
			if (this.delayedRecordStates != (DataViewRowState)(-1))
			{
				this.RowStateFilter = this.delayedRecordStates;
				this.delayedRecordStates = (DataViewRowState)(-1);
			}
			this.fEndInitInProgress = false;
			this.SetIndex(this.Sort, this.RowStateFilter, this.rowFilter);
			this.OnInitialized();
		}

		// Token: 0x06000B46 RID: 2886 RVA: 0x001F6C88 File Offset: 0x001F6088
		private void CheckOpen()
		{
			if (!this.IsOpen)
			{
				throw ExceptionBuilder.NotOpen();
			}
		}

		// Token: 0x06000B47 RID: 2887 RVA: 0x001F6CA4 File Offset: 0x001F60A4
		private void CheckSort(string sort)
		{
			if (this.table == null)
			{
				throw ExceptionBuilder.CanNotUse();
			}
			if (sort.Length == 0)
			{
				return;
			}
			this.table.ParseSortString(sort);
		}

		// Token: 0x06000B48 RID: 2888 RVA: 0x001F6CD8 File Offset: 0x001F60D8
		protected void Close()
		{
			this.shouldOpen = false;
			this.UpdateIndex();
			this.dvListener.UnregisterMetaDataEvents();
		}

		// Token: 0x06000B49 RID: 2889 RVA: 0x001F6D00 File Offset: 0x001F6100
		public void CopyTo(Array array, int index)
		{
			checked
			{
				if (this.index != null)
				{
					RBTree<int>.RBTreeEnumerator enumerator = this.index.GetEnumerator(0);
					while (enumerator.MoveNext())
					{
						int num = enumerator.Current;
						array.SetValue(this.GetRowView(num), index);
						index++;
					}
				}
				if (this.addNewRow != null)
				{
					array.SetValue(this.rowViewCache[this.addNewRow], index);
				}
			}
		}

		// Token: 0x06000B4A RID: 2890 RVA: 0x001F6D68 File Offset: 0x001F6168
		private void CopyTo(DataRowView[] array, int index)
		{
			checked
			{
				if (this.index != null)
				{
					RBTree<int>.RBTreeEnumerator enumerator = this.index.GetEnumerator(0);
					while (enumerator.MoveNext())
					{
						int num = enumerator.Current;
						array[index] = this.GetRowView(num);
						index++;
					}
				}
				if (this.addNewRow != null)
				{
					array[index] = this.rowViewCache[this.addNewRow];
				}
			}
		}

		// Token: 0x06000B4B RID: 2891 RVA: 0x001F6DC8 File Offset: 0x001F61C8
		public void Delete(int index)
		{
			this.Delete(this.GetRow(index));
		}

		// Token: 0x06000B4C RID: 2892 RVA: 0x001F6DE4 File Offset: 0x001F61E4
		internal void Delete(DataRow row)
		{
			if (row != null)
			{
				IntPtr intPtr;
				Bid.ScopeEnter(out intPtr, "<ds.DataView.Delete|API> %d#, row=%d#", this.ObjectID, row.ObjectID);
				try
				{
					this.CheckOpen();
					if (row == this.addNewRow)
					{
						this.FinishAddNew(false);
					}
					else
					{
						if (!this.AllowDelete)
						{
							throw ExceptionBuilder.CanNotDelete();
						}
						row.Delete();
					}
				}
				finally
				{
					Bid.ScopeLeave(ref intPtr);
				}
			}
		}

		// Token: 0x06000B4D RID: 2893 RVA: 0x001F6E60 File Offset: 0x001F6260
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				this.Close();
			}
			base.Dispose(disposing);
		}

		// Token: 0x06000B4E RID: 2894 RVA: 0x001F6E80 File Offset: 0x001F6280
		public int Find(object key)
		{
			return this.FindByKey(key);
		}

		// Token: 0x06000B4F RID: 2895 RVA: 0x001F6E94 File Offset: 0x001F6294
		internal virtual int FindByKey(object key)
		{
			return this.index.FindRecordByKey(key);
		}

		// Token: 0x06000B50 RID: 2896 RVA: 0x001F6EB0 File Offset: 0x001F62B0
		public int Find(object[] key)
		{
			return this.FindByKey(key);
		}

		// Token: 0x06000B51 RID: 2897 RVA: 0x001F6EC4 File Offset: 0x001F62C4
		internal virtual int FindByKey(object[] key)
		{
			return this.index.FindRecordByKey(key);
		}

		// Token: 0x06000B52 RID: 2898 RVA: 0x001F6EE0 File Offset: 0x001F62E0
		public DataRowView[] FindRows(object key)
		{
			return this.FindRowsByKey(new object[] { key });
		}

		// Token: 0x06000B53 RID: 2899 RVA: 0x001F6F00 File Offset: 0x001F6300
		public DataRowView[] FindRows(object[] key)
		{
			return this.FindRowsByKey(key);
		}

		// Token: 0x06000B54 RID: 2900 RVA: 0x001F6F14 File Offset: 0x001F6314
		internal virtual DataRowView[] FindRowsByKey(object[] key)
		{
			IntPtr intPtr;
			Bid.ScopeEnter(out intPtr, "<ds.DataView.FindRows|API> %d#\n", this.ObjectID);
			DataRowView[] dataRowViewFromRange;
			try
			{
				Range range = this.index.FindRecords(key);
				dataRowViewFromRange = this.GetDataRowViewFromRange(range);
			}
			finally
			{
				Bid.ScopeLeave(ref intPtr);
			}
			return dataRowViewFromRange;
		}

		// Token: 0x06000B55 RID: 2901 RVA: 0x001F6F70 File Offset: 0x001F6370
		internal Range FindRecords<TKey, TRow>(Index.ComparisonBySelector<TKey, TRow> comparison, TKey key) where TRow : DataRow
		{
			return this.index.FindRecords<TKey, TRow>(comparison, key);
		}

		// Token: 0x06000B56 RID: 2902 RVA: 0x001F6F8C File Offset: 0x001F638C
		internal DataRowView[] GetDataRowViewFromRange(Range range)
		{
			if (range.IsNull)
			{
				return new DataRowView[0];
			}
			DataRowView[] array = new DataRowView[range.Count];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = this[i + range.Min];
			}
			return array;
		}

		// Token: 0x06000B57 RID: 2903 RVA: 0x001F6FD8 File Offset: 0x001F63D8
		internal void FinishAddNew(bool success)
		{
			Bid.Trace("<ds.DataView.FinishAddNew|INFO> %d#, success=%d{bool}\n", this.ObjectID, success);
			DataRow dataRow = this.addNewRow;
			if (success)
			{
				if (DataRowState.Detached == dataRow.RowState)
				{
					this.table.Rows.Add(dataRow);
				}
				else
				{
					dataRow.EndEdit();
				}
			}
			if (dataRow == this.addNewRow)
			{
				this.rowViewCache.Remove(this.addNewRow);
				this.addNewRow = null;
				if (!success)
				{
					dataRow.CancelEdit();
				}
				this.OnListChanged(new ListChangedEventArgs(ListChangedType.ItemDeleted, this.Count));
			}
		}

		// Token: 0x06000B58 RID: 2904 RVA: 0x001F7060 File Offset: 0x001F6460
		public IEnumerator GetEnumerator()
		{
			DataRowView[] array = new DataRowView[this.Count];
			this.CopyTo(array, 0);
			return array.GetEnumerator();
		}

		// Token: 0x17000182 RID: 386
		// (get) Token: 0x06000B59 RID: 2905 RVA: 0x001F7088 File Offset: 0x001F6488
		bool IList.IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000183 RID: 387
		// (get) Token: 0x06000B5A RID: 2906 RVA: 0x001F7098 File Offset: 0x001F6498
		bool IList.IsFixedSize
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06000B5B RID: 2907 RVA: 0x001F70A8 File Offset: 0x001F64A8
		int IList.Add(object value)
		{
			if (value == null)
			{
				this.AddNew();
				return this.Count - 1;
			}
			throw ExceptionBuilder.AddExternalObject();
		}

		// Token: 0x06000B5C RID: 2908 RVA: 0x001F70D0 File Offset: 0x001F64D0
		void IList.Clear()
		{
			throw ExceptionBuilder.CanNotClear();
		}

		// Token: 0x06000B5D RID: 2909 RVA: 0x001F70E4 File Offset: 0x001F64E4
		bool IList.Contains(object value)
		{
			return 0 <= this.IndexOf(value as DataRowView);
		}

		// Token: 0x06000B5E RID: 2910 RVA: 0x001F7104 File Offset: 0x001F6504
		int IList.IndexOf(object value)
		{
			return this.IndexOf(value as DataRowView);
		}

		// Token: 0x06000B5F RID: 2911 RVA: 0x001F7120 File Offset: 0x001F6520
		internal int IndexOf(DataRowView rowview)
		{
			if (rowview != null)
			{
				if (object.ReferenceEquals(this.addNewRow, rowview.Row))
				{
					return this.Count - 1;
				}
				DataRowView dataRowView;
				if (this.index != null && DataRowState.Detached != rowview.Row.RowState && this.rowViewCache.TryGetValue(rowview.Row, out dataRowView) && dataRowView == rowview)
				{
					return this.IndexOfDataRowView(rowview);
				}
			}
			return -1;
		}

		// Token: 0x06000B60 RID: 2912 RVA: 0x001F7184 File Offset: 0x001F6584
		private int IndexOfDataRowView(DataRowView rowview)
		{
			return this.index.GetIndex(rowview.GetRecord());
		}

		// Token: 0x06000B61 RID: 2913 RVA: 0x001F71A4 File Offset: 0x001F65A4
		void IList.Insert(int index, object value)
		{
			throw ExceptionBuilder.InsertExternalObject();
		}

		// Token: 0x06000B62 RID: 2914 RVA: 0x001F71B8 File Offset: 0x001F65B8
		void IList.Remove(object value)
		{
			int num = this.IndexOf(value as DataRowView);
			if (0 <= num)
			{
				((IList)this).RemoveAt(num);
				return;
			}
			throw ExceptionBuilder.RemoveExternalObject();
		}

		// Token: 0x06000B63 RID: 2915 RVA: 0x001F71E4 File Offset: 0x001F65E4
		void IList.RemoveAt(int index)
		{
			this.Delete(index);
		}

		// Token: 0x06000B64 RID: 2916 RVA: 0x001F71F8 File Offset: 0x001F65F8
		internal Index GetFindIndex(string column, bool keepIndex)
		{
			if (this.findIndexes == null)
			{
				this.findIndexes = new Dictionary<string, Index>();
			}
			Index index;
			if (this.findIndexes.TryGetValue(column, out index))
			{
				if (!keepIndex)
				{
					this.findIndexes.Remove(column);
					index.RemoveRef();
					if (index.RefCount == 1)
					{
						index.RemoveRef();
					}
				}
			}
			else if (keepIndex)
			{
				index = this.table.GetIndex(column, this.recordStates, this.GetFilter());
				this.findIndexes[column] = index;
				index.AddRef();
			}
			return index;
		}

		// Token: 0x17000184 RID: 388
		// (get) Token: 0x06000B65 RID: 2917 RVA: 0x001F7284 File Offset: 0x001F6684
		bool IBindingList.AllowNew
		{
			get
			{
				return this.AllowNew;
			}
		}

		// Token: 0x06000B66 RID: 2918 RVA: 0x001F7298 File Offset: 0x001F6698
		object IBindingList.AddNew()
		{
			return this.AddNew();
		}

		// Token: 0x17000185 RID: 389
		// (get) Token: 0x06000B67 RID: 2919 RVA: 0x001F72AC File Offset: 0x001F66AC
		bool IBindingList.AllowEdit
		{
			get
			{
				return this.AllowEdit;
			}
		}

		// Token: 0x17000186 RID: 390
		// (get) Token: 0x06000B68 RID: 2920 RVA: 0x001F72C0 File Offset: 0x001F66C0
		bool IBindingList.AllowRemove
		{
			get
			{
				return this.AllowDelete;
			}
		}

		// Token: 0x17000187 RID: 391
		// (get) Token: 0x06000B69 RID: 2921 RVA: 0x001F72D4 File Offset: 0x001F66D4
		bool IBindingList.SupportsChangeNotification
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000188 RID: 392
		// (get) Token: 0x06000B6A RID: 2922 RVA: 0x001F72E4 File Offset: 0x001F66E4
		bool IBindingList.SupportsSearching
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000189 RID: 393
		// (get) Token: 0x06000B6B RID: 2923 RVA: 0x001F72F4 File Offset: 0x001F66F4
		bool IBindingList.SupportsSorting
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700018A RID: 394
		// (get) Token: 0x06000B6C RID: 2924 RVA: 0x001F7304 File Offset: 0x001F6704
		bool IBindingList.IsSorted
		{
			get
			{
				return this.Sort.Length != 0;
			}
		}

		// Token: 0x1700018B RID: 395
		// (get) Token: 0x06000B6D RID: 2925 RVA: 0x001F7324 File Offset: 0x001F6724
		PropertyDescriptor IBindingList.SortProperty
		{
			get
			{
				return this.GetSortProperty();
			}
		}

		// Token: 0x06000B6E RID: 2926 RVA: 0x001F7338 File Offset: 0x001F6738
		internal PropertyDescriptor GetSortProperty()
		{
			if (this.table != null && this.index != null && this.index.IndexFields.Length == 1)
			{
				return new DataColumnPropertyDescriptor(this.index.IndexFields[0].Column);
			}
			return null;
		}

		// Token: 0x1700018C RID: 396
		// (get) Token: 0x06000B6F RID: 2927 RVA: 0x001F7388 File Offset: 0x001F6788
		ListSortDirection IBindingList.SortDirection
		{
			get
			{
				if (this.index.IndexFields.Length == 1 && this.index.IndexFields[0].IsDescending)
				{
					return ListSortDirection.Descending;
				}
				return ListSortDirection.Ascending;
			}
		}

		// Token: 0x1400001E RID: 30
		// (add) Token: 0x06000B70 RID: 2928 RVA: 0x001F73C8 File Offset: 0x001F67C8
		// (remove) Token: 0x06000B71 RID: 2929 RVA: 0x001F73FC File Offset: 0x001F67FC
		[ResCategory("DataCategory_Data")]
		[ResDescription("DataViewListChangedDescr")]
		public event ListChangedEventHandler ListChanged
		{
			add
			{
				Bid.Trace("<ds.DataView.add_ListChanged|API> %d#\n", this.ObjectID);
				this.onListChanged = (ListChangedEventHandler)Delegate.Combine(this.onListChanged, value);
			}
			remove
			{
				Bid.Trace("<ds.DataView.remove_ListChanged|API> %d#\n", this.ObjectID);
				this.onListChanged = (ListChangedEventHandler)Delegate.Remove(this.onListChanged, value);
			}
		}

		// Token: 0x1400001F RID: 31
		// (add) Token: 0x06000B72 RID: 2930 RVA: 0x001F7430 File Offset: 0x001F6830
		// (remove) Token: 0x06000B73 RID: 2931 RVA: 0x001F7454 File Offset: 0x001F6854
		[ResCategory("DataCategory_Action")]
		[ResDescription("DataSetInitializedDescr")]
		public event EventHandler Initialized
		{
			add
			{
				this.onInitialized = (EventHandler)Delegate.Combine(this.onInitialized, value);
			}
			remove
			{
				this.onInitialized = (EventHandler)Delegate.Remove(this.onInitialized, value);
			}
		}

		// Token: 0x06000B74 RID: 2932 RVA: 0x001F7478 File Offset: 0x001F6878
		void IBindingList.AddIndex(PropertyDescriptor property)
		{
			this.GetFindIndex(property.Name, true);
		}

		// Token: 0x06000B75 RID: 2933 RVA: 0x001F7494 File Offset: 0x001F6894
		void IBindingList.ApplySort(PropertyDescriptor property, ListSortDirection direction)
		{
			this.Sort = this.CreateSortString(property, direction);
		}

		// Token: 0x06000B76 RID: 2934 RVA: 0x001F74B0 File Offset: 0x001F68B0
		int IBindingList.Find(PropertyDescriptor property, object key)
		{
			if (property != null)
			{
				bool flag = false;
				Index index = null;
				try
				{
					if (this.findIndexes == null || !this.findIndexes.TryGetValue(property.Name, out index))
					{
						flag = true;
						index = this.table.GetIndex(property.Name, this.recordStates, this.GetFilter());
						index.AddRef();
					}
					Range range = index.FindRecords(key);
					if (!range.IsNull)
					{
						return this.index.GetIndex(index.GetRecord(range.Min));
					}
				}
				finally
				{
					if (flag && index != null)
					{
						index.RemoveRef();
						if (index.RefCount == 1)
						{
							index.RemoveRef();
						}
					}
				}
				return -1;
			}
			return -1;
		}

		// Token: 0x06000B77 RID: 2935 RVA: 0x001F7578 File Offset: 0x001F6978
		void IBindingList.RemoveIndex(PropertyDescriptor property)
		{
			this.GetFindIndex(property.Name, false);
		}

		// Token: 0x06000B78 RID: 2936 RVA: 0x001F7594 File Offset: 0x001F6994
		void IBindingList.RemoveSort()
		{
			Bid.Trace("<ds.DataView.RemoveSort|API> %d#\n", this.ObjectID);
			this.Sort = string.Empty;
		}

		// Token: 0x06000B79 RID: 2937 RVA: 0x001F75BC File Offset: 0x001F69BC
		void IBindingListView.ApplySort(ListSortDescriptionCollection sorts)
		{
			if (sorts == null)
			{
				throw ExceptionBuilder.ArgumentNull("sorts");
			}
			StringBuilder stringBuilder = new StringBuilder();
			bool flag = false;
			foreach (object obj in ((IEnumerable)sorts))
			{
				ListSortDescription listSortDescription = (ListSortDescription)obj;
				if (listSortDescription == null)
				{
					throw ExceptionBuilder.ArgumentContainsNull("sorts");
				}
				PropertyDescriptor propertyDescriptor = listSortDescription.PropertyDescriptor;
				if (propertyDescriptor == null)
				{
					throw ExceptionBuilder.ArgumentNull("PropertyDescriptor");
				}
				if (!this.table.Columns.Contains(propertyDescriptor.Name))
				{
					throw ExceptionBuilder.ColumnToSortIsOutOfRange(propertyDescriptor.Name);
				}
				ListSortDirection sortDirection = listSortDescription.SortDirection;
				if (flag)
				{
					stringBuilder.Append(',');
				}
				stringBuilder.Append(this.CreateSortString(propertyDescriptor, sortDirection));
				if (!flag)
				{
					flag = true;
				}
			}
			this.Sort = stringBuilder.ToString();
		}

		// Token: 0x06000B7A RID: 2938 RVA: 0x001F76B4 File Offset: 0x001F6AB4
		private string CreateSortString(PropertyDescriptor property, ListSortDirection direction)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append('[');
			stringBuilder.Append(property.Name);
			stringBuilder.Append(']');
			if (ListSortDirection.Descending == direction)
			{
				stringBuilder.Append(" DESC");
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06000B7B RID: 2939 RVA: 0x001F76FC File Offset: 0x001F6AFC
		void IBindingListView.RemoveFilter()
		{
			Bid.Trace("<ds.DataView.RemoveFilter|API> %d#\n", this.ObjectID);
			this.RowFilter = "";
		}

		// Token: 0x1700018D RID: 397
		// (get) Token: 0x06000B7C RID: 2940 RVA: 0x001F7724 File Offset: 0x001F6B24
		// (set) Token: 0x06000B7D RID: 2941 RVA: 0x001F7738 File Offset: 0x001F6B38
		string IBindingListView.Filter
		{
			get
			{
				return this.RowFilter;
			}
			set
			{
				this.RowFilter = value;
			}
		}

		// Token: 0x1700018E RID: 398
		// (get) Token: 0x06000B7E RID: 2942 RVA: 0x001F774C File Offset: 0x001F6B4C
		ListSortDescriptionCollection IBindingListView.SortDescriptions
		{
			get
			{
				return this.GetSortDescriptions();
			}
		}

		// Token: 0x06000B7F RID: 2943 RVA: 0x001F7760 File Offset: 0x001F6B60
		internal ListSortDescriptionCollection GetSortDescriptions()
		{
			ListSortDescription[] array = new ListSortDescription[0];
			if (this.table != null && this.index != null && this.index.IndexFields.Length > 0)
			{
				array = new ListSortDescription[this.index.IndexFields.Length];
				for (int i = 0; i < this.index.IndexFields.Length; i++)
				{
					DataColumnPropertyDescriptor dataColumnPropertyDescriptor = new DataColumnPropertyDescriptor(this.index.IndexFields[i].Column);
					if (this.index.IndexFields[i].IsDescending)
					{
						array[i] = new ListSortDescription(dataColumnPropertyDescriptor, ListSortDirection.Descending);
					}
					else
					{
						array[i] = new ListSortDescription(dataColumnPropertyDescriptor, ListSortDirection.Ascending);
					}
				}
			}
			return new ListSortDescriptionCollection(array);
		}

		// Token: 0x1700018F RID: 399
		// (get) Token: 0x06000B80 RID: 2944 RVA: 0x001F7824 File Offset: 0x001F6C24
		bool IBindingListView.SupportsAdvancedSorting
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000190 RID: 400
		// (get) Token: 0x06000B81 RID: 2945 RVA: 0x001F7834 File Offset: 0x001F6C34
		bool IBindingListView.SupportsFiltering
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000B82 RID: 2946 RVA: 0x001F7844 File Offset: 0x001F6C44
		string ITypedList.GetListName(PropertyDescriptor[] listAccessors)
		{
			if (this.table != null)
			{
				if (listAccessors == null || listAccessors.Length == 0)
				{
					return this.table.TableName;
				}
				DataSet dataSet = this.table.DataSet;
				if (dataSet != null)
				{
					DataTable dataTable = dataSet.FindTable(this.table, listAccessors, 0);
					if (dataTable != null)
					{
						return dataTable.TableName;
					}
				}
			}
			return string.Empty;
		}

		// Token: 0x06000B83 RID: 2947 RVA: 0x001F789C File Offset: 0x001F6C9C
		PropertyDescriptorCollection ITypedList.GetItemProperties(PropertyDescriptor[] listAccessors)
		{
			if (this.table != null)
			{
				if (listAccessors == null || listAccessors.Length == 0)
				{
					return this.table.GetPropertyDescriptorCollection(null);
				}
				DataSet dataSet = this.table.DataSet;
				if (dataSet == null)
				{
					return new PropertyDescriptorCollection(null);
				}
				DataTable dataTable = dataSet.FindTable(this.table, listAccessors, 0);
				if (dataTable != null)
				{
					return dataTable.GetPropertyDescriptorCollection(null);
				}
			}
			return new PropertyDescriptorCollection(null);
		}

		// Token: 0x06000B84 RID: 2948 RVA: 0x001F78FC File Offset: 0x001F6CFC
		internal virtual IFilter GetFilter()
		{
			return this.rowFilter;
		}

		// Token: 0x06000B85 RID: 2949 RVA: 0x001F7910 File Offset: 0x001F6D10
		private int GetRecord(int recordIndex)
		{
			if (this.Count <= recordIndex)
			{
				throw ExceptionBuilder.RowOutOfRange(recordIndex);
			}
			if (recordIndex == this.index.RecordCount)
			{
				return this.addNewRow.GetDefaultRecord();
			}
			return this.index.GetRecord(recordIndex);
		}

		// Token: 0x06000B86 RID: 2950 RVA: 0x001F7954 File Offset: 0x001F6D54
		internal DataRow GetRow(int index)
		{
			int count = this.Count;
			if (count <= index)
			{
				throw ExceptionBuilder.GetElementIndex(index);
			}
			if (index == count - 1 && this.addNewRow != null)
			{
				return this.addNewRow;
			}
			return this.table.recordManager[this.GetRecord(index)];
		}

		// Token: 0x06000B87 RID: 2951 RVA: 0x001F79A0 File Offset: 0x001F6DA0
		private DataRowView GetRowView(int record)
		{
			return this.GetRowView(this.table.recordManager[record]);
		}

		// Token: 0x06000B88 RID: 2952 RVA: 0x001F79C4 File Offset: 0x001F6DC4
		private DataRowView GetRowView(DataRow dr)
		{
			return this.rowViewCache[dr];
		}

		// Token: 0x06000B89 RID: 2953 RVA: 0x001F79E0 File Offset: 0x001F6DE0
		protected virtual void IndexListChanged(object sender, ListChangedEventArgs e)
		{
			if (e.ListChangedType != ListChangedType.Reset)
			{
				this.OnListChanged(e);
			}
			if (this.addNewRow != null && this.index.RecordCount == 0)
			{
				this.FinishAddNew(false);
			}
			if (e.ListChangedType == ListChangedType.Reset)
			{
				this.OnListChanged(e);
			}
		}

		// Token: 0x06000B8A RID: 2954 RVA: 0x001F7A28 File Offset: 0x001F6E28
		internal void IndexListChangedInternal(ListChangedEventArgs e)
		{
			this.rowViewBuffer.Clear();
			if (ListChangedType.ItemAdded == e.ListChangedType && this.addNewMoved != null && this.addNewMoved.NewIndex != this.addNewMoved.OldIndex)
			{
				ListChangedEventArgs listChangedEventArgs = this.addNewMoved;
				this.addNewMoved = null;
				this.IndexListChanged(this, listChangedEventArgs);
			}
			this.IndexListChanged(this, e);
		}

		// Token: 0x06000B8B RID: 2955 RVA: 0x001F7A88 File Offset: 0x001F6E88
		internal void MaintainDataView(ListChangedType changedType, DataRow row, bool trackAddRemove)
		{
			DataRowView dataRowView = null;
			switch (changedType)
			{
			case ListChangedType.Reset:
				this.ResetRowViewCache();
				break;
			case ListChangedType.ItemAdded:
				if (trackAddRemove && this.rowViewBuffer.TryGetValue(row, out dataRowView))
				{
					this.rowViewBuffer.Remove(row);
				}
				if (row == this.addNewRow)
				{
					int num = this.IndexOfDataRowView(this.rowViewCache[this.addNewRow]);
					this.addNewRow = null;
					this.addNewMoved = new ListChangedEventArgs(ListChangedType.ItemMoved, num, this.Count - 1);
					return;
				}
				if (!this.rowViewCache.ContainsKey(row))
				{
					this.rowViewCache.Add(row, dataRowView ?? new DataRowView(this, row));
					return;
				}
				break;
			case ListChangedType.ItemDeleted:
				if (trackAddRemove)
				{
					this.rowViewCache.TryGetValue(row, out dataRowView);
					if (dataRowView != null)
					{
						this.rowViewBuffer.Add(row, dataRowView);
					}
				}
				if (!this.rowViewCache.Remove(row))
				{
					return;
				}
				break;
			case ListChangedType.ItemMoved:
			case ListChangedType.ItemChanged:
			case ListChangedType.PropertyDescriptorAdded:
			case ListChangedType.PropertyDescriptorDeleted:
			case ListChangedType.PropertyDescriptorChanged:
				break;
			default:
				return;
			}
		}

		// Token: 0x06000B8C RID: 2956 RVA: 0x001F7B7C File Offset: 0x001F6F7C
		protected virtual void OnListChanged(ListChangedEventArgs e)
		{
			Bid.Trace("<ds.DataView.OnListChanged|INFO> %d#, ListChangedType=%d{ListChangedType}\n", this.ObjectID, (int)e.ListChangedType);
			try
			{
				DataColumn dataColumn = null;
				string text = null;
				switch (e.ListChangedType)
				{
				case ListChangedType.ItemMoved:
				case ListChangedType.ItemChanged:
					if (0 <= e.NewIndex)
					{
						DataRow row = this.GetRow(e.NewIndex);
						if (row.HasPropertyChanged)
						{
							dataColumn = row.LastChangedColumn;
							text = ((dataColumn != null) ? dataColumn.ColumnName : string.Empty);
						}
						row.ResetLastChangedColumn();
					}
					break;
				}
				if (this.onListChanged != null)
				{
					if (dataColumn != null && e.NewIndex == e.OldIndex)
					{
						ListChangedEventArgs listChangedEventArgs = new ListChangedEventArgs(e.ListChangedType, e.NewIndex, new DataColumnPropertyDescriptor(dataColumn));
						this.onListChanged(this, listChangedEventArgs);
					}
					else
					{
						this.onListChanged(this, e);
					}
				}
				if (text != null)
				{
					this[e.NewIndex].RaisePropertyChangedEvent(text);
				}
			}
			catch (Exception ex)
			{
				if (!ADP.IsCatchableExceptionType(ex))
				{
					throw;
				}
				ExceptionBuilder.TraceExceptionWithoutRethrow(ex);
			}
		}

		// Token: 0x06000B8D RID: 2957 RVA: 0x001F7CA8 File Offset: 0x001F70A8
		private void OnInitialized()
		{
			if (this.onInitialized != null)
			{
				this.onInitialized(this, EventArgs.Empty);
			}
		}

		// Token: 0x06000B8E RID: 2958 RVA: 0x001F7CD0 File Offset: 0x001F70D0
		protected void Open()
		{
			this.shouldOpen = true;
			this.UpdateIndex();
			this.dvListener.RegisterMetaDataEvents(this.table);
		}

		// Token: 0x06000B8F RID: 2959 RVA: 0x001F7CFC File Offset: 0x001F70FC
		protected void Reset()
		{
			if (this.IsOpen)
			{
				this.index.Reset();
			}
		}

		// Token: 0x06000B90 RID: 2960 RVA: 0x001F7D1C File Offset: 0x001F711C
		internal void ResetRowViewCache()
		{
			Dictionary<DataRow, DataRowView> dictionary = new Dictionary<DataRow, DataRowView>(this.CountFromIndex, DataView.DataRowReferenceComparer.Default);
			if (this.index != null)
			{
				RBTree<int>.RBTreeEnumerator enumerator = this.index.GetEnumerator(0);
				while (enumerator.MoveNext())
				{
					int num = enumerator.Current;
					DataRow dataRow = this.table.recordManager[num];
					DataRowView dataRowView;
					if (!this.rowViewCache.TryGetValue(dataRow, out dataRowView))
					{
						dataRowView = new DataRowView(this, dataRow);
					}
					dictionary.Add(dataRow, dataRowView);
				}
			}
			if (this.addNewRow != null)
			{
				DataRowView dataRowView;
				this.rowViewCache.TryGetValue(this.addNewRow, out dataRowView);
				dictionary.Add(this.addNewRow, dataRowView);
			}
			this.rowViewCache = dictionary;
		}

		// Token: 0x06000B91 RID: 2961 RVA: 0x001F7DC4 File Offset: 0x001F71C4
		internal void SetDataViewManager(DataViewManager dataViewManager)
		{
			if (this.table == null)
			{
				throw ExceptionBuilder.CanNotUse();
			}
			if (this.dataViewManager != dataViewManager)
			{
				if (dataViewManager != null)
				{
					dataViewManager.nViews--;
				}
				this.dataViewManager = dataViewManager;
				if (dataViewManager != null)
				{
					dataViewManager.nViews++;
					DataViewSetting dataViewSetting = dataViewManager.DataViewSettings[this.table];
					try
					{
						this.applyDefaultSort = dataViewSetting.ApplyDefaultSort;
						DataExpression dataExpression = new DataExpression(this.table, dataViewSetting.RowFilter);
						this.SetIndex(dataViewSetting.Sort, dataViewSetting.RowStateFilter, dataExpression);
					}
					catch (Exception ex)
					{
						if (!ADP.IsCatchableExceptionType(ex))
						{
							throw;
						}
						ExceptionBuilder.TraceExceptionWithoutRethrow(ex);
					}
					this.locked = true;
					return;
				}
				this.SetIndex("", DataViewRowState.CurrentRows, null);
			}
		}

		// Token: 0x06000B92 RID: 2962 RVA: 0x001F7EA0 File Offset: 0x001F72A0
		internal virtual void SetIndex(string newSort, DataViewRowState newRowStates, IFilter newRowFilter)
		{
			this.SetIndex2(newSort, newRowStates, newRowFilter, true);
		}

		// Token: 0x06000B93 RID: 2963 RVA: 0x001F7EB8 File Offset: 0x001F72B8
		internal void SetIndex2(string newSort, DataViewRowState newRowStates, IFilter newRowFilter, bool fireEvent)
		{
			Bid.Trace("<ds.DataView.SetIndex|INFO> %d#, newSort='%ls', newRowStates=%d{ds.DataViewRowState}\n", this.ObjectID, newSort, (int)newRowStates);
			this.sort = newSort;
			this.recordStates = newRowStates;
			this.rowFilter = newRowFilter;
			if (this.fEndInitInProgress)
			{
				return;
			}
			if (fireEvent)
			{
				this.UpdateIndex(true);
			}
			else
			{
				this.UpdateIndex(true, false);
			}
			if (this.findIndexes != null)
			{
				Dictionary<string, Index> dictionary = this.findIndexes;
				this.findIndexes = null;
				foreach (KeyValuePair<string, Index> keyValuePair in dictionary)
				{
					keyValuePair.Value.RemoveRef();
				}
			}
		}

		// Token: 0x06000B94 RID: 2964 RVA: 0x001F7F74 File Offset: 0x001F7374
		protected void UpdateIndex()
		{
			this.UpdateIndex(false);
		}

		// Token: 0x06000B95 RID: 2965 RVA: 0x001F7F88 File Offset: 0x001F7388
		protected virtual void UpdateIndex(bool force)
		{
			this.UpdateIndex(force, true);
		}

		// Token: 0x06000B96 RID: 2966 RVA: 0x001F7FA0 File Offset: 0x001F73A0
		internal void UpdateIndex(bool force, bool fireEvent)
		{
			IntPtr intPtr;
			Bid.ScopeEnter(out intPtr, "<ds.DataView.UpdateIndex|INFO> %d#, force=%d{bool}\n", this.ObjectID, force);
			try
			{
				if (this.open != this.shouldOpen || force)
				{
					this.open = this.shouldOpen;
					Index index = null;
					if (this.open && this.table != null)
					{
						if (this.SortComparison != null)
						{
							index = new Index(this.table, this.SortComparison, this.recordStates, this.GetFilter());
							index.AddRef();
						}
						else
						{
							index = this.table.GetIndex(this.Sort, this.recordStates, this.GetFilter());
						}
					}
					if (this.index != index)
					{
						if (this.index == null)
						{
							DataTable dataTable = index.Table;
						}
						else
						{
							DataTable dataTable2 = this.index.Table;
						}
						if (this.index != null)
						{
							this.dvListener.UnregisterListChangedEvent();
						}
						this.index = index;
						if (this.index != null)
						{
							this.dvListener.RegisterListChangedEvent(this.index);
						}
						this.ResetRowViewCache();
						if (fireEvent)
						{
							this.OnListChanged(DataView.ResetEventArgs);
						}
					}
				}
			}
			finally
			{
				Bid.ScopeLeave(ref intPtr);
			}
		}

		// Token: 0x06000B97 RID: 2967 RVA: 0x001F80D4 File Offset: 0x001F74D4
		internal void ChildRelationCollectionChanged(object sender, CollectionChangeEventArgs e)
		{
			DataRelationPropertyDescriptor dataRelationPropertyDescriptor = null;
			this.OnListChanged((e.Action == CollectionChangeAction.Add) ? new ListChangedEventArgs(ListChangedType.PropertyDescriptorAdded, new DataRelationPropertyDescriptor((DataRelation)e.Element)) : ((e.Action == CollectionChangeAction.Refresh) ? new ListChangedEventArgs(ListChangedType.PropertyDescriptorChanged, dataRelationPropertyDescriptor) : ((e.Action == CollectionChangeAction.Remove) ? new ListChangedEventArgs(ListChangedType.PropertyDescriptorDeleted, new DataRelationPropertyDescriptor((DataRelation)e.Element)) : null)));
		}

		// Token: 0x06000B98 RID: 2968 RVA: 0x001F8140 File Offset: 0x001F7540
		internal void ParentRelationCollectionChanged(object sender, CollectionChangeEventArgs e)
		{
			DataRelationPropertyDescriptor dataRelationPropertyDescriptor = null;
			this.OnListChanged((e.Action == CollectionChangeAction.Add) ? new ListChangedEventArgs(ListChangedType.PropertyDescriptorAdded, new DataRelationPropertyDescriptor((DataRelation)e.Element)) : ((e.Action == CollectionChangeAction.Refresh) ? new ListChangedEventArgs(ListChangedType.PropertyDescriptorChanged, dataRelationPropertyDescriptor) : ((e.Action == CollectionChangeAction.Remove) ? new ListChangedEventArgs(ListChangedType.PropertyDescriptorDeleted, new DataRelationPropertyDescriptor((DataRelation)e.Element)) : null)));
		}

		// Token: 0x06000B99 RID: 2969 RVA: 0x001F81AC File Offset: 0x001F75AC
		protected virtual void ColumnCollectionChanged(object sender, CollectionChangeEventArgs e)
		{
			DataColumnPropertyDescriptor dataColumnPropertyDescriptor = null;
			this.OnListChanged((e.Action == CollectionChangeAction.Add) ? new ListChangedEventArgs(ListChangedType.PropertyDescriptorAdded, new DataColumnPropertyDescriptor((DataColumn)e.Element)) : ((e.Action == CollectionChangeAction.Refresh) ? new ListChangedEventArgs(ListChangedType.PropertyDescriptorChanged, dataColumnPropertyDescriptor) : ((e.Action == CollectionChangeAction.Remove) ? new ListChangedEventArgs(ListChangedType.PropertyDescriptorDeleted, new DataColumnPropertyDescriptor((DataColumn)e.Element)) : null)));
		}

		// Token: 0x06000B9A RID: 2970 RVA: 0x001F8218 File Offset: 0x001F7618
		internal void ColumnCollectionChangedInternal(object sender, CollectionChangeEventArgs e)
		{
			this.ColumnCollectionChanged(sender, e);
		}

		// Token: 0x06000B9B RID: 2971 RVA: 0x001F8230 File Offset: 0x001F7630
		public DataTable ToTable()
		{
			return this.ToTable(null, false, new string[0]);
		}

		// Token: 0x06000B9C RID: 2972 RVA: 0x001F824C File Offset: 0x001F764C
		public DataTable ToTable(string tableName)
		{
			return this.ToTable(tableName, false, new string[0]);
		}

		// Token: 0x06000B9D RID: 2973 RVA: 0x001F8268 File Offset: 0x001F7668
		public DataTable ToTable(bool distinct, params string[] columnNames)
		{
			return this.ToTable(null, distinct, columnNames);
		}

		// Token: 0x06000B9E RID: 2974 RVA: 0x001F8280 File Offset: 0x001F7680
		public DataTable ToTable(string tableName, bool distinct, params string[] columnNames)
		{
			Bid.Trace("<ds.DataView.ToTable|API> %d#, TableName='%ls', distinct=%d{bool}\n", this.ObjectID, tableName, distinct);
			if (columnNames == null)
			{
				throw ExceptionBuilder.ArgumentNull("columnNames");
			}
			DataTable dataTable = new DataTable();
			dataTable.Locale = this.table.Locale;
			dataTable.CaseSensitive = this.table.CaseSensitive;
			dataTable.TableName = ((tableName != null) ? tableName : this.table.TableName);
			dataTable.Namespace = this.table.Namespace;
			dataTable.Prefix = this.table.Prefix;
			if (columnNames.Length == 0)
			{
				columnNames = new string[this.Table.Columns.Count];
				for (int i = 0; i < columnNames.Length; i++)
				{
					columnNames[i] = this.Table.Columns[i].ColumnName;
				}
			}
			int[] array = new int[columnNames.Length];
			List<object[]> list = new List<object[]>();
			for (int j = 0; j < columnNames.Length; j++)
			{
				DataColumn dataColumn = this.Table.Columns[columnNames[j]];
				if (dataColumn == null)
				{
					throw ExceptionBuilder.ColumnNotInTheUnderlyingTable(columnNames[j], this.Table.TableName);
				}
				dataTable.Columns.Add(dataColumn.Clone());
				array[j] = this.Table.Columns.IndexOf(dataColumn);
			}
			foreach (object obj in this)
			{
				DataRowView dataRowView = (DataRowView)obj;
				object[] array2 = new object[columnNames.Length];
				for (int k = 0; k < array.Length; k++)
				{
					array2[k] = dataRowView[array[k]];
				}
				if (!distinct || !this.RowExist(list, array2))
				{
					dataTable.Rows.Add(array2);
					list.Add(array2);
				}
			}
			return dataTable;
		}

		// Token: 0x06000B9F RID: 2975 RVA: 0x001F8468 File Offset: 0x001F7868
		private bool RowExist(List<object[]> arraylist, object[] objectArray)
		{
			for (int i = 0; i < arraylist.Count; i++)
			{
				object[] array = arraylist[i];
				bool flag = true;
				for (int j = 0; j < objectArray.Length; j++)
				{
					flag &= array[j].Equals(objectArray[j]);
				}
				if (flag)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000BA0 RID: 2976 RVA: 0x001F84B4 File Offset: 0x001F78B4
		public virtual bool Equals(DataView view)
		{
			return view != null && this.Table == view.Table && this.Count == view.Count && string.Compare(this.RowFilter, view.RowFilter, StringComparison.OrdinalIgnoreCase) == 0 && string.Compare(this.Sort, view.Sort, StringComparison.OrdinalIgnoreCase) == 0 && object.ReferenceEquals(this.SortComparison, view.SortComparison) && object.ReferenceEquals(this.RowPredicate, view.RowPredicate) && this.RowStateFilter == view.RowStateFilter && this.DataViewManager == view.DataViewManager && this.AllowDelete == view.AllowDelete && this.AllowNew == view.AllowNew && this.AllowEdit == view.AllowEdit;
		}

		// Token: 0x17000191 RID: 401
		// (get) Token: 0x06000BA1 RID: 2977 RVA: 0x001F8584 File Offset: 0x001F7984
		internal int ObjectID
		{
			get
			{
				return this._objectID;
			}
		}

		// Token: 0x0400082A RID: 2090
		private DataViewManager dataViewManager;

		// Token: 0x0400082B RID: 2091
		private DataTable table;

		// Token: 0x0400082C RID: 2092
		private bool locked;

		// Token: 0x0400082D RID: 2093
		private Index index;

		// Token: 0x0400082E RID: 2094
		private Dictionary<string, Index> findIndexes;

		// Token: 0x0400082F RID: 2095
		private string sort = "";

		// Token: 0x04000830 RID: 2096
		private Comparison<DataRow> _comparison;

		// Token: 0x04000831 RID: 2097
		private IFilter rowFilter;

		// Token: 0x04000832 RID: 2098
		private DataViewRowState recordStates = DataViewRowState.CurrentRows;

		// Token: 0x04000833 RID: 2099
		private bool shouldOpen = true;

		// Token: 0x04000834 RID: 2100
		private bool open;

		// Token: 0x04000835 RID: 2101
		private bool allowNew = true;

		// Token: 0x04000836 RID: 2102
		private bool allowEdit = true;

		// Token: 0x04000837 RID: 2103
		private bool allowDelete = true;

		// Token: 0x04000838 RID: 2104
		private bool applyDefaultSort;

		// Token: 0x04000839 RID: 2105
		internal DataRow addNewRow;

		// Token: 0x0400083A RID: 2106
		private ListChangedEventArgs addNewMoved;

		// Token: 0x0400083B RID: 2107
		private ListChangedEventHandler onListChanged;

		// Token: 0x0400083C RID: 2108
		private EventHandler onInitialized;

		// Token: 0x0400083D RID: 2109
		internal static ListChangedEventArgs ResetEventArgs = new ListChangedEventArgs(ListChangedType.Reset, -1);

		// Token: 0x0400083E RID: 2110
		private DataTable delayedTable;

		// Token: 0x0400083F RID: 2111
		private string delayedRowFilter;

		// Token: 0x04000840 RID: 2112
		private string delayedSort;

		// Token: 0x04000841 RID: 2113
		private DataViewRowState delayedRecordStates = (DataViewRowState)(-1);

		// Token: 0x04000842 RID: 2114
		private bool fInitInProgress;

		// Token: 0x04000843 RID: 2115
		private bool fEndInitInProgress;

		// Token: 0x04000844 RID: 2116
		private Dictionary<DataRow, DataRowView> rowViewCache = new Dictionary<DataRow, DataRowView>(DataView.DataRowReferenceComparer.Default);

		// Token: 0x04000845 RID: 2117
		private readonly Dictionary<DataRow, DataRowView> rowViewBuffer = new Dictionary<DataRow, DataRowView>(DataView.DataRowReferenceComparer.Default);

		// Token: 0x04000846 RID: 2118
		private DataViewListener dvListener;

		// Token: 0x04000847 RID: 2119
		private static int _objectTypeCount;

		// Token: 0x04000848 RID: 2120
		private readonly int _objectID = Interlocked.Increment(ref DataView._objectTypeCount);

		// Token: 0x020000A7 RID: 167
		private sealed class DataRowReferenceComparer : IEqualityComparer<DataRow>
		{
			// Token: 0x06000BA3 RID: 2979 RVA: 0x001F85B4 File Offset: 0x001F79B4
			private DataRowReferenceComparer()
			{
			}

			// Token: 0x06000BA4 RID: 2980 RVA: 0x001F85C8 File Offset: 0x001F79C8
			public bool Equals(DataRow x, DataRow y)
			{
				return x == y;
			}

			// Token: 0x06000BA5 RID: 2981 RVA: 0x001F85DC File Offset: 0x001F79DC
			public int GetHashCode(DataRow obj)
			{
				return obj.ObjectID;
			}

			// Token: 0x04000849 RID: 2121
			internal static readonly DataView.DataRowReferenceComparer Default = new DataView.DataRowReferenceComparer();
		}

		// Token: 0x020000A9 RID: 169
		private sealed class RowPredicateFilter : IFilter
		{
			// Token: 0x06000BA8 RID: 2984 RVA: 0x001F8608 File Offset: 0x001F7A08
			internal RowPredicateFilter(Predicate<DataRow> predicate)
			{
				this.PredicateFilter = predicate;
			}

			// Token: 0x06000BA9 RID: 2985 RVA: 0x001F8624 File Offset: 0x001F7A24
			bool IFilter.Invoke(DataRow row, DataRowVersion version)
			{
				return this.PredicateFilter(row);
			}

			// Token: 0x0400084A RID: 2122
			internal readonly Predicate<DataRow> PredicateFilter;
		}
	}
}
