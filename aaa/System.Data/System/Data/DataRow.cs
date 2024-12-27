using System;
using System.Collections;
using System.ComponentModel;
using System.Data.Common;
using System.Diagnostics;
using System.Threading;
using System.Xml;

namespace System.Data
{
	// Token: 0x02000081 RID: 129
	public class DataRow
	{
		// Token: 0x06000775 RID: 1909 RVA: 0x001DF920 File Offset: 0x001DED20
		protected internal DataRow(DataRowBuilder builder)
		{
			this.tempRecord = builder._record;
			this._table = builder._table;
			this._columns = this._table.Columns;
		}

		// Token: 0x170000E4 RID: 228
		// (get) Token: 0x06000776 RID: 1910 RVA: 0x001DF984 File Offset: 0x001DED84
		// (set) Token: 0x06000777 RID: 1911 RVA: 0x001DF99C File Offset: 0x001DED9C
		internal XmlBoundElement Element
		{
			get
			{
				return (XmlBoundElement)this._element;
			}
			set
			{
				this._element = value;
			}
		}

		// Token: 0x170000E5 RID: 229
		// (get) Token: 0x06000778 RID: 1912 RVA: 0x001DF9B0 File Offset: 0x001DEDB0
		// (set) Token: 0x06000779 RID: 1913 RVA: 0x001DF9D0 File Offset: 0x001DEDD0
		internal DataColumn LastChangedColumn
		{
			get
			{
				if (this._countColumnChange != 1)
				{
					return null;
				}
				return this._lastChangedColumn;
			}
			set
			{
				this._countColumnChange++;
				this._lastChangedColumn = value;
			}
		}

		// Token: 0x170000E6 RID: 230
		// (get) Token: 0x0600077A RID: 1914 RVA: 0x001DF9F4 File Offset: 0x001DEDF4
		internal bool HasPropertyChanged
		{
			get
			{
				return 0 < this._countColumnChange;
			}
		}

		// Token: 0x170000E7 RID: 231
		// (get) Token: 0x0600077B RID: 1915 RVA: 0x001DFA0C File Offset: 0x001DEE0C
		// (set) Token: 0x0600077C RID: 1916 RVA: 0x001DFA20 File Offset: 0x001DEE20
		internal int RBTreeNodeId
		{
			get
			{
				return this._rbTreeNodeId;
			}
			set
			{
				Bid.Trace("<ds.DataRow.set_RBTreeNodeId|INFO> %d#, value=%d\n", this.ObjectID, value);
				this._rbTreeNodeId = value;
			}
		}

		// Token: 0x170000E8 RID: 232
		// (get) Token: 0x0600077D RID: 1917 RVA: 0x001DFA48 File Offset: 0x001DEE48
		// (set) Token: 0x0600077E RID: 1918 RVA: 0x001DFA70 File Offset: 0x001DEE70
		public string RowError
		{
			get
			{
				if (this.error != null)
				{
					return this.error.Text;
				}
				return string.Empty;
			}
			set
			{
				Bid.Trace("<ds.DataRow.set_RowError|API> %d#, value='%ls'\n", this.ObjectID, value);
				if (this.error == null)
				{
					if (!ADP.IsEmpty(value))
					{
						this.error = new DataError(value);
					}
					this.RowErrorChanged();
					return;
				}
				if (this.error.Text != value)
				{
					this.error.Text = value;
					this.RowErrorChanged();
				}
			}
		}

		// Token: 0x0600077F RID: 1919 RVA: 0x001DFAD8 File Offset: 0x001DEED8
		private void RowErrorChanged()
		{
			if (this.oldRecord != -1)
			{
				this._table.RecordChanged(this.oldRecord);
			}
			if (this.newRecord != -1)
			{
				this._table.RecordChanged(this.newRecord);
			}
		}

		// Token: 0x170000E9 RID: 233
		// (get) Token: 0x06000780 RID: 1920 RVA: 0x001DFB1C File Offset: 0x001DEF1C
		// (set) Token: 0x06000781 RID: 1921 RVA: 0x001DFB30 File Offset: 0x001DEF30
		internal long rowID
		{
			get
			{
				return this._rowID;
			}
			set
			{
				this.ResetLastChangedColumn();
				this._rowID = value;
			}
		}

		// Token: 0x170000EA RID: 234
		// (get) Token: 0x06000782 RID: 1922 RVA: 0x001DFB4C File Offset: 0x001DEF4C
		public DataRowState RowState
		{
			get
			{
				if (this.oldRecord == this.newRecord)
				{
					if (this.oldRecord == -1)
					{
						return DataRowState.Detached;
					}
					if (0 < this._columns.ColumnsImplementingIChangeTrackingCount)
					{
						foreach (DataColumn dataColumn in this._columns.ColumnsImplementingIChangeTracking)
						{
							object obj = this[dataColumn];
							if (DBNull.Value != obj && ((IChangeTracking)obj).IsChanged)
							{
								return DataRowState.Modified;
							}
						}
					}
					return DataRowState.Unchanged;
				}
				else
				{
					if (this.oldRecord == -1)
					{
						return DataRowState.Added;
					}
					if (this.newRecord == -1)
					{
						return DataRowState.Deleted;
					}
					return DataRowState.Modified;
				}
			}
		}

		// Token: 0x170000EB RID: 235
		// (get) Token: 0x06000783 RID: 1923 RVA: 0x001DFBE0 File Offset: 0x001DEFE0
		public DataTable Table
		{
			get
			{
				return this._table;
			}
		}

		// Token: 0x170000EC RID: 236
		public object this[int columnIndex]
		{
			get
			{
				DataColumn dataColumn = this._columns[columnIndex];
				int defaultRecord = this.GetDefaultRecord();
				return dataColumn[defaultRecord];
			}
			set
			{
				DataColumn dataColumn = this._columns[columnIndex];
				this[dataColumn] = value;
			}
		}

		// Token: 0x06000786 RID: 1926 RVA: 0x001DFC40 File Offset: 0x001DF040
		internal void CheckForLoops(DataRelation rel)
		{
			if (this._table.fInLoadDiffgram || (this._table.DataSet != null && this._table.DataSet.fInLoadDiffgram))
			{
				return;
			}
			int count = this._table.Rows.Count;
			int num = 0;
			for (DataRow dataRow = this.GetParentRow(rel); dataRow != null; dataRow = dataRow.GetParentRow(rel))
			{
				if (dataRow == this || num > count)
				{
					throw ExceptionBuilder.NestedCircular(this._table.TableName);
				}
				num++;
			}
		}

		// Token: 0x06000787 RID: 1927 RVA: 0x001DFCC0 File Offset: 0x001DF0C0
		internal int GetNestedParentCount()
		{
			int num = 0;
			DataRelation[] nestedParentRelations = this._table.NestedParentRelations;
			foreach (DataRelation dataRelation in nestedParentRelations)
			{
				if (dataRelation != null)
				{
					if (dataRelation.ParentTable == this._table)
					{
						this.CheckForLoops(dataRelation);
					}
					DataRow parentRow = this.GetParentRow(dataRelation);
					if (parentRow != null)
					{
						num++;
					}
				}
			}
			return num;
		}

		// Token: 0x170000ED RID: 237
		public object this[string columnName]
		{
			get
			{
				DataColumn dataColumn = this.GetDataColumn(columnName);
				int defaultRecord = this.GetDefaultRecord();
				return dataColumn[defaultRecord];
			}
			set
			{
				DataColumn dataColumn = this.GetDataColumn(columnName);
				this[dataColumn] = value;
			}
		}

		// Token: 0x170000EE RID: 238
		public object this[DataColumn column]
		{
			get
			{
				this.CheckColumn(column);
				int defaultRecord = this.GetDefaultRecord();
				return column[defaultRecord];
			}
			set
			{
				this.CheckColumn(column);
				if (this.inChangingEvent)
				{
					throw ExceptionBuilder.EditInRowChanging();
				}
				if (-1L != this.rowID && column.ReadOnly)
				{
					throw ExceptionBuilder.ReadOnly(column.ColumnName);
				}
				DataColumnChangeEventArgs dataColumnChangeEventArgs = null;
				if (this._table.NeedColumnChangeEvents)
				{
					dataColumnChangeEventArgs = new DataColumnChangeEventArgs(this, column, value);
					this._table.OnColumnChanging(dataColumnChangeEventArgs);
				}
				if (column.Table != this._table)
				{
					throw ExceptionBuilder.ColumnNotInTheTable(column.ColumnName, this._table.TableName);
				}
				if (-1L != this.rowID && column.ReadOnly)
				{
					throw ExceptionBuilder.ReadOnly(column.ColumnName);
				}
				object obj = ((dataColumnChangeEventArgs != null) ? dataColumnChangeEventArgs.ProposedValue : value);
				if (obj == null)
				{
					if (column.IsValueType)
					{
						throw ExceptionBuilder.CannotSetToNull(column);
					}
					obj = DBNull.Value;
				}
				bool flag = this.BeginEditInternal();
				try
				{
					int proposedRecordNo = this.GetProposedRecordNo();
					column[proposedRecordNo] = obj;
				}
				catch (Exception ex)
				{
					if (ADP.IsCatchableOrSecurityExceptionType(ex) && flag)
					{
						this.CancelEdit();
					}
					throw;
				}
				this.LastChangedColumn = column;
				if (dataColumnChangeEventArgs != null)
				{
					this._table.OnColumnChanged(dataColumnChangeEventArgs);
				}
				if (flag)
				{
					this.EndEdit();
				}
			}
		}

		// Token: 0x170000EF RID: 239
		public object this[int columnIndex, DataRowVersion version]
		{
			get
			{
				DataColumn dataColumn = this._columns[columnIndex];
				int recordFromVersion = this.GetRecordFromVersion(version);
				return dataColumn[recordFromVersion];
			}
		}

		// Token: 0x170000F0 RID: 240
		public object this[string columnName, DataRowVersion version]
		{
			get
			{
				DataColumn dataColumn = this.GetDataColumn(columnName);
				int recordFromVersion = this.GetRecordFromVersion(version);
				return dataColumn[recordFromVersion];
			}
		}

		// Token: 0x170000F1 RID: 241
		public object this[DataColumn column, DataRowVersion version]
		{
			get
			{
				this.CheckColumn(column);
				int recordFromVersion = this.GetRecordFromVersion(version);
				return column[recordFromVersion];
			}
		}

		// Token: 0x170000F2 RID: 242
		// (get) Token: 0x0600078F RID: 1935 RVA: 0x001DFF30 File Offset: 0x001DF330
		// (set) Token: 0x06000790 RID: 1936 RVA: 0x001DFF7C File Offset: 0x001DF37C
		public object[] ItemArray
		{
			get
			{
				int defaultRecord = this.GetDefaultRecord();
				object[] array = new object[this._columns.Count];
				for (int i = 0; i < array.Length; i++)
				{
					DataColumn dataColumn = this._columns[i];
					array[i] = dataColumn[defaultRecord];
				}
				return array;
			}
			set
			{
				if (value == null)
				{
					throw ExceptionBuilder.ArgumentNull("ItemArray");
				}
				if (this._columns.Count < value.Length)
				{
					throw ExceptionBuilder.ValueArrayLength();
				}
				DataColumnChangeEventArgs dataColumnChangeEventArgs = null;
				if (this._table.NeedColumnChangeEvents)
				{
					dataColumnChangeEventArgs = new DataColumnChangeEventArgs(this);
				}
				bool flag = this.BeginEditInternal();
				for (int i = 0; i < value.Length; i++)
				{
					if (value[i] != null)
					{
						DataColumn dataColumn = this._columns[i];
						if (-1L != this.rowID && dataColumn.ReadOnly)
						{
							throw ExceptionBuilder.ReadOnly(dataColumn.ColumnName);
						}
						if (dataColumnChangeEventArgs != null)
						{
							dataColumnChangeEventArgs.InitializeColumnChangeEvent(dataColumn, value[i]);
							this._table.OnColumnChanging(dataColumnChangeEventArgs);
						}
						if (dataColumn.Table != this._table)
						{
							throw ExceptionBuilder.ColumnNotInTheTable(dataColumn.ColumnName, this._table.TableName);
						}
						if (-1L != this.rowID && dataColumn.ReadOnly)
						{
							throw ExceptionBuilder.ReadOnly(dataColumn.ColumnName);
						}
						if (this.tempRecord == -1)
						{
							this.BeginEditInternal();
						}
						object obj = ((dataColumnChangeEventArgs != null) ? dataColumnChangeEventArgs.ProposedValue : value[i]);
						if (obj == null)
						{
							if (dataColumn.IsValueType)
							{
								throw ExceptionBuilder.CannotSetToNull(dataColumn);
							}
							obj = DBNull.Value;
						}
						try
						{
							int proposedRecordNo = this.GetProposedRecordNo();
							dataColumn[proposedRecordNo] = obj;
						}
						catch (Exception ex)
						{
							if (ADP.IsCatchableOrSecurityExceptionType(ex) && flag)
							{
								this.CancelEdit();
							}
							throw;
						}
						this.LastChangedColumn = dataColumn;
						if (dataColumnChangeEventArgs != null)
						{
							this._table.OnColumnChanged(dataColumnChangeEventArgs);
						}
					}
				}
				this.EndEdit();
			}
		}

		// Token: 0x06000791 RID: 1937 RVA: 0x001E010C File Offset: 0x001DF50C
		public void AcceptChanges()
		{
			IntPtr intPtr;
			Bid.ScopeEnter(out intPtr, "<ds.DataRow.AcceptChanges|API> %d#\n", this.ObjectID);
			try
			{
				this.EndEdit();
				if (this.RowState != DataRowState.Detached && this.RowState != DataRowState.Deleted && this._columns.ColumnsImplementingIChangeTrackingCount > 0)
				{
					foreach (DataColumn dataColumn in this._columns.ColumnsImplementingIChangeTracking)
					{
						object obj = this[dataColumn];
						if (DBNull.Value != obj)
						{
							IChangeTracking changeTracking = (IChangeTracking)obj;
							if (changeTracking.IsChanged)
							{
								changeTracking.AcceptChanges();
							}
						}
					}
				}
				this._table.CommitRow(this);
			}
			finally
			{
				Bid.ScopeLeave(ref intPtr);
			}
		}

		// Token: 0x06000792 RID: 1938 RVA: 0x001E01C8 File Offset: 0x001DF5C8
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public void BeginEdit()
		{
			this.BeginEditInternal();
		}

		// Token: 0x06000793 RID: 1939 RVA: 0x001E01DC File Offset: 0x001DF5DC
		private bool BeginEditInternal()
		{
			if (this.inChangingEvent)
			{
				throw ExceptionBuilder.BeginEditInRowChanging();
			}
			if (this.tempRecord != -1)
			{
				if (this.tempRecord < this._table.recordManager.LastFreeRecord)
				{
					return false;
				}
				this.tempRecord = -1;
			}
			if (this.oldRecord != -1 && this.newRecord == -1)
			{
				throw ExceptionBuilder.DeletedRowInaccessible();
			}
			this.ResetLastChangedColumn();
			this.tempRecord = this._table.NewRecord(this.newRecord);
			return true;
		}

		// Token: 0x06000794 RID: 1940 RVA: 0x001E0258 File Offset: 0x001DF658
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public void CancelEdit()
		{
			if (this.inChangingEvent)
			{
				throw ExceptionBuilder.CancelEditInRowChanging();
			}
			this._table.FreeRecord(ref this.tempRecord);
			this.ResetLastChangedColumn();
		}

		// Token: 0x06000795 RID: 1941 RVA: 0x001E028C File Offset: 0x001DF68C
		private void CheckColumn(DataColumn column)
		{
			if (column == null)
			{
				throw ExceptionBuilder.ArgumentNull("column");
			}
			if (column.Table != this._table)
			{
				throw ExceptionBuilder.ColumnNotInTheTable(column.ColumnName, this._table.TableName);
			}
		}

		// Token: 0x06000796 RID: 1942 RVA: 0x001E02CC File Offset: 0x001DF6CC
		internal void CheckInTable()
		{
			if (this.rowID == -1L)
			{
				throw ExceptionBuilder.RowNotInTheTable();
			}
		}

		// Token: 0x06000797 RID: 1943 RVA: 0x001E02EC File Offset: 0x001DF6EC
		public void Delete()
		{
			if (this.inDeletingEvent)
			{
				throw ExceptionBuilder.DeleteInRowDeleting();
			}
			if (this.newRecord == -1)
			{
				return;
			}
			this._table.DeleteRow(this);
		}

		// Token: 0x06000798 RID: 1944 RVA: 0x001E0320 File Offset: 0x001DF720
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public void EndEdit()
		{
			if (this.inChangingEvent)
			{
				throw ExceptionBuilder.EndEditInRowChanging();
			}
			if (this.newRecord == -1)
			{
				return;
			}
			if (this.tempRecord != -1)
			{
				try
				{
					this.SetNewRecord(this.tempRecord);
				}
				finally
				{
					this.ResetLastChangedColumn();
				}
			}
		}

		// Token: 0x06000799 RID: 1945 RVA: 0x001E0380 File Offset: 0x001DF780
		public void SetColumnError(int columnIndex, string error)
		{
			DataColumn dataColumn = this._columns[columnIndex];
			if (dataColumn == null)
			{
				throw ExceptionBuilder.ColumnOutOfRange(columnIndex);
			}
			this.SetColumnError(dataColumn, error);
		}

		// Token: 0x0600079A RID: 1946 RVA: 0x001E03AC File Offset: 0x001DF7AC
		public void SetColumnError(string columnName, string error)
		{
			DataColumn dataColumn = this.GetDataColumn(columnName);
			this.SetColumnError(dataColumn, error);
		}

		// Token: 0x0600079B RID: 1947 RVA: 0x001E03CC File Offset: 0x001DF7CC
		public void SetColumnError(DataColumn column, string error)
		{
			this.CheckColumn(column);
			IntPtr intPtr;
			Bid.ScopeEnter(out intPtr, "<ds.DataRow.SetColumnError|API> %d#, column=%d, error='%ls'\n", this.ObjectID, column.ObjectID, error);
			try
			{
				if (this.error == null)
				{
					this.error = new DataError();
				}
				if (this.GetColumnError(column) != error)
				{
					this.error.SetColumnError(column, error);
					this.RowErrorChanged();
				}
			}
			finally
			{
				Bid.ScopeLeave(ref intPtr);
			}
		}

		// Token: 0x0600079C RID: 1948 RVA: 0x001E0454 File Offset: 0x001DF854
		public string GetColumnError(int columnIndex)
		{
			DataColumn dataColumn = this._columns[columnIndex];
			return this.GetColumnError(dataColumn);
		}

		// Token: 0x0600079D RID: 1949 RVA: 0x001E0478 File Offset: 0x001DF878
		public string GetColumnError(string columnName)
		{
			DataColumn dataColumn = this.GetDataColumn(columnName);
			return this.GetColumnError(dataColumn);
		}

		// Token: 0x0600079E RID: 1950 RVA: 0x001E0494 File Offset: 0x001DF894
		public string GetColumnError(DataColumn column)
		{
			this.CheckColumn(column);
			if (this.error == null)
			{
				this.error = new DataError();
			}
			return this.error.GetColumnError(column);
		}

		// Token: 0x0600079F RID: 1951 RVA: 0x001E04C8 File Offset: 0x001DF8C8
		public void ClearErrors()
		{
			if (this.error != null)
			{
				this.error.Clear();
				this.RowErrorChanged();
			}
		}

		// Token: 0x060007A0 RID: 1952 RVA: 0x001E04F0 File Offset: 0x001DF8F0
		internal void ClearError(DataColumn column)
		{
			if (this.error != null)
			{
				this.error.Clear(column);
				this.RowErrorChanged();
			}
		}

		// Token: 0x170000F3 RID: 243
		// (get) Token: 0x060007A1 RID: 1953 RVA: 0x001E0518 File Offset: 0x001DF918
		public bool HasErrors
		{
			get
			{
				return this.error != null && this.error.HasErrors;
			}
		}

		// Token: 0x060007A2 RID: 1954 RVA: 0x001E053C File Offset: 0x001DF93C
		public DataColumn[] GetColumnsInError()
		{
			if (this.error == null)
			{
				return DataTable.zeroColumns;
			}
			return this.error.GetColumnsInError();
		}

		// Token: 0x060007A3 RID: 1955 RVA: 0x001E0564 File Offset: 0x001DF964
		public DataRow[] GetChildRows(string relationName)
		{
			return this.GetChildRows(this._table.ChildRelations[relationName], DataRowVersion.Default);
		}

		// Token: 0x060007A4 RID: 1956 RVA: 0x001E0590 File Offset: 0x001DF990
		public DataRow[] GetChildRows(string relationName, DataRowVersion version)
		{
			return this.GetChildRows(this._table.ChildRelations[relationName], version);
		}

		// Token: 0x060007A5 RID: 1957 RVA: 0x001E05B8 File Offset: 0x001DF9B8
		public DataRow[] GetChildRows(DataRelation relation)
		{
			return this.GetChildRows(relation, DataRowVersion.Default);
		}

		// Token: 0x060007A6 RID: 1958 RVA: 0x001E05D4 File Offset: 0x001DF9D4
		public DataRow[] GetChildRows(DataRelation relation, DataRowVersion version)
		{
			if (relation == null)
			{
				return this._table.NewRowArray(0);
			}
			if (relation.DataSet != this._table.DataSet)
			{
				throw ExceptionBuilder.RowNotInTheDataSet();
			}
			if (relation.ParentKey.Table != this._table)
			{
				throw ExceptionBuilder.RelationForeignTable(relation.ParentTable.TableName, this._table.TableName);
			}
			return DataRelation.GetChildRows(relation.ParentKey, relation.ChildKey, this, version);
		}

		// Token: 0x060007A7 RID: 1959 RVA: 0x001E0650 File Offset: 0x001DFA50
		internal DataColumn GetDataColumn(string columnName)
		{
			DataColumn dataColumn = this._columns[columnName];
			if (dataColumn != null)
			{
				return dataColumn;
			}
			throw ExceptionBuilder.ColumnNotInTheTable(columnName, this._table.TableName);
		}

		// Token: 0x060007A8 RID: 1960 RVA: 0x001E0680 File Offset: 0x001DFA80
		public DataRow GetParentRow(string relationName)
		{
			return this.GetParentRow(this._table.ParentRelations[relationName], DataRowVersion.Default);
		}

		// Token: 0x060007A9 RID: 1961 RVA: 0x001E06AC File Offset: 0x001DFAAC
		public DataRow GetParentRow(string relationName, DataRowVersion version)
		{
			return this.GetParentRow(this._table.ParentRelations[relationName], version);
		}

		// Token: 0x060007AA RID: 1962 RVA: 0x001E06D4 File Offset: 0x001DFAD4
		public DataRow GetParentRow(DataRelation relation)
		{
			return this.GetParentRow(relation, DataRowVersion.Default);
		}

		// Token: 0x060007AB RID: 1963 RVA: 0x001E06F0 File Offset: 0x001DFAF0
		public DataRow GetParentRow(DataRelation relation, DataRowVersion version)
		{
			if (relation == null)
			{
				return null;
			}
			if (relation.DataSet != this._table.DataSet)
			{
				throw ExceptionBuilder.RelationForeignRow();
			}
			if (relation.ChildKey.Table != this._table)
			{
				throw ExceptionBuilder.GetParentRowTableMismatch(relation.ChildTable.TableName, this._table.TableName);
			}
			return DataRelation.GetParentRow(relation.ParentKey, relation.ChildKey, this, version);
		}

		// Token: 0x060007AC RID: 1964 RVA: 0x001E0760 File Offset: 0x001DFB60
		internal DataRow GetNestedParentRow(DataRowVersion version)
		{
			DataRelation[] nestedParentRelations = this._table.NestedParentRelations;
			foreach (DataRelation dataRelation in nestedParentRelations)
			{
				if (dataRelation != null)
				{
					if (dataRelation.ParentTable == this._table)
					{
						this.CheckForLoops(dataRelation);
					}
					DataRow parentRow = this.GetParentRow(dataRelation, version);
					if (parentRow != null)
					{
						return parentRow;
					}
				}
			}
			return null;
		}

		// Token: 0x060007AD RID: 1965 RVA: 0x001E07BC File Offset: 0x001DFBBC
		public DataRow[] GetParentRows(string relationName)
		{
			return this.GetParentRows(this._table.ParentRelations[relationName], DataRowVersion.Default);
		}

		// Token: 0x060007AE RID: 1966 RVA: 0x001E07E8 File Offset: 0x001DFBE8
		public DataRow[] GetParentRows(string relationName, DataRowVersion version)
		{
			return this.GetParentRows(this._table.ParentRelations[relationName], version);
		}

		// Token: 0x060007AF RID: 1967 RVA: 0x001E0810 File Offset: 0x001DFC10
		public DataRow[] GetParentRows(DataRelation relation)
		{
			return this.GetParentRows(relation, DataRowVersion.Default);
		}

		// Token: 0x060007B0 RID: 1968 RVA: 0x001E082C File Offset: 0x001DFC2C
		public DataRow[] GetParentRows(DataRelation relation, DataRowVersion version)
		{
			if (relation == null)
			{
				return this._table.NewRowArray(0);
			}
			if (relation.DataSet != this._table.DataSet)
			{
				throw ExceptionBuilder.RowNotInTheDataSet();
			}
			if (relation.ChildKey.Table != this._table)
			{
				throw ExceptionBuilder.GetParentRowTableMismatch(relation.ChildTable.TableName, this._table.TableName);
			}
			return DataRelation.GetParentRows(relation.ParentKey, relation.ChildKey, this, version);
		}

		// Token: 0x060007B1 RID: 1969 RVA: 0x001E08A8 File Offset: 0x001DFCA8
		internal object[] GetColumnValues(DataColumn[] columns)
		{
			return this.GetColumnValues(columns, DataRowVersion.Default);
		}

		// Token: 0x060007B2 RID: 1970 RVA: 0x001E08C4 File Offset: 0x001DFCC4
		internal object[] GetColumnValues(DataColumn[] columns, DataRowVersion version)
		{
			DataKey dataKey = new DataKey(columns, false);
			return this.GetKeyValues(dataKey, version);
		}

		// Token: 0x060007B3 RID: 1971 RVA: 0x001E08E4 File Offset: 0x001DFCE4
		internal object[] GetKeyValues(DataKey key)
		{
			int defaultRecord = this.GetDefaultRecord();
			return key.GetKeyValues(defaultRecord);
		}

		// Token: 0x060007B4 RID: 1972 RVA: 0x001E0900 File Offset: 0x001DFD00
		internal object[] GetKeyValues(DataKey key, DataRowVersion version)
		{
			int recordFromVersion = this.GetRecordFromVersion(version);
			return key.GetKeyValues(recordFromVersion);
		}

		// Token: 0x060007B5 RID: 1973 RVA: 0x001E0920 File Offset: 0x001DFD20
		internal int GetCurrentRecordNo()
		{
			if (this.newRecord == -1)
			{
				throw ExceptionBuilder.NoCurrentData();
			}
			return this.newRecord;
		}

		// Token: 0x060007B6 RID: 1974 RVA: 0x001E0944 File Offset: 0x001DFD44
		internal int GetDefaultRecord()
		{
			if (this.tempRecord != -1)
			{
				return this.tempRecord;
			}
			if (this.newRecord != -1)
			{
				return this.newRecord;
			}
			if (this.oldRecord == -1)
			{
				throw ExceptionBuilder.RowRemovedFromTheTable();
			}
			throw ExceptionBuilder.DeletedRowInaccessible();
		}

		// Token: 0x060007B7 RID: 1975 RVA: 0x001E0988 File Offset: 0x001DFD88
		internal int GetOriginalRecordNo()
		{
			if (this.oldRecord == -1)
			{
				throw ExceptionBuilder.NoOriginalData();
			}
			return this.oldRecord;
		}

		// Token: 0x060007B8 RID: 1976 RVA: 0x001E09AC File Offset: 0x001DFDAC
		private int GetProposedRecordNo()
		{
			if (this.tempRecord == -1)
			{
				throw ExceptionBuilder.NoProposedData();
			}
			return this.tempRecord;
		}

		// Token: 0x060007B9 RID: 1977 RVA: 0x001E09D0 File Offset: 0x001DFDD0
		internal int GetRecordFromVersion(DataRowVersion version)
		{
			if (version <= DataRowVersion.Current)
			{
				if (version == DataRowVersion.Original)
				{
					return this.GetOriginalRecordNo();
				}
				if (version == DataRowVersion.Current)
				{
					return this.GetCurrentRecordNo();
				}
			}
			else
			{
				if (version == DataRowVersion.Proposed)
				{
					return this.GetProposedRecordNo();
				}
				if (version == DataRowVersion.Default)
				{
					return this.GetDefaultRecord();
				}
			}
			throw ExceptionBuilder.InvalidRowVersion();
		}

		// Token: 0x060007BA RID: 1978 RVA: 0x001E0A2C File Offset: 0x001DFE2C
		internal DataRowVersion GetDefaultRowVersion(DataViewRowState viewState)
		{
			if (this.oldRecord == this.newRecord)
			{
				if (this.oldRecord == -1)
				{
					return DataRowVersion.Default;
				}
				return DataRowVersion.Default;
			}
			else
			{
				if (this.oldRecord == -1)
				{
					return DataRowVersion.Default;
				}
				if (this.newRecord == -1)
				{
					return DataRowVersion.Original;
				}
				if ((DataViewRowState.ModifiedCurrent & viewState) != DataViewRowState.None)
				{
					return DataRowVersion.Default;
				}
				return DataRowVersion.Original;
			}
		}

		// Token: 0x060007BB RID: 1979 RVA: 0x001E0A8C File Offset: 0x001DFE8C
		internal DataViewRowState GetRecordState(int record)
		{
			if (record == -1)
			{
				return DataViewRowState.None;
			}
			if (record == this.oldRecord && record == this.newRecord)
			{
				return DataViewRowState.Unchanged;
			}
			if (record == this.oldRecord)
			{
				if (this.newRecord == -1)
				{
					return DataViewRowState.Deleted;
				}
				return DataViewRowState.ModifiedOriginal;
			}
			else
			{
				if (record != this.newRecord)
				{
					return DataViewRowState.None;
				}
				if (this.oldRecord == -1)
				{
					return DataViewRowState.Added;
				}
				return DataViewRowState.ModifiedCurrent;
			}
		}

		// Token: 0x060007BC RID: 1980 RVA: 0x001E0AE4 File Offset: 0x001DFEE4
		internal bool HasKeyChanged(DataKey key)
		{
			return this.HasKeyChanged(key, DataRowVersion.Current, DataRowVersion.Proposed);
		}

		// Token: 0x060007BD RID: 1981 RVA: 0x001E0B04 File Offset: 0x001DFF04
		internal bool HasKeyChanged(DataKey key, DataRowVersion version1, DataRowVersion version2)
		{
			return !this.HasVersion(version1) || !this.HasVersion(version2) || !key.RecordsEqual(this.GetRecordFromVersion(version1), this.GetRecordFromVersion(version2));
		}

		// Token: 0x060007BE RID: 1982 RVA: 0x001E0B40 File Offset: 0x001DFF40
		public bool HasVersion(DataRowVersion version)
		{
			if (version <= DataRowVersion.Current)
			{
				if (version == DataRowVersion.Original)
				{
					return this.oldRecord != -1;
				}
				if (version == DataRowVersion.Current)
				{
					return this.newRecord != -1;
				}
			}
			else
			{
				if (version == DataRowVersion.Proposed)
				{
					return this.tempRecord != -1;
				}
				if (version == DataRowVersion.Default)
				{
					return this.tempRecord != -1 || this.newRecord != -1;
				}
			}
			throw ExceptionBuilder.InvalidRowVersion();
		}

		// Token: 0x060007BF RID: 1983 RVA: 0x001E0BC0 File Offset: 0x001DFFC0
		internal bool HasChanges()
		{
			if (!this.HasVersion(DataRowVersion.Original) || !this.HasVersion(DataRowVersion.Current))
			{
				return true;
			}
			foreach (object obj in this.Table.Columns)
			{
				DataColumn dataColumn = (DataColumn)obj;
				if (dataColumn.Compare(this.oldRecord, this.newRecord) != 0)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060007C0 RID: 1984 RVA: 0x001E0C5C File Offset: 0x001E005C
		internal bool HaveValuesChanged(DataColumn[] columns)
		{
			return this.HaveValuesChanged(columns, DataRowVersion.Current, DataRowVersion.Proposed);
		}

		// Token: 0x060007C1 RID: 1985 RVA: 0x001E0C7C File Offset: 0x001E007C
		internal bool HaveValuesChanged(DataColumn[] columns, DataRowVersion version1, DataRowVersion version2)
		{
			for (int i = 0; i < columns.Length; i++)
			{
				this.CheckColumn(columns[i]);
			}
			DataKey dataKey = new DataKey(columns, false);
			return this.HasKeyChanged(dataKey, version1, version2);
		}

		// Token: 0x060007C2 RID: 1986 RVA: 0x001E0CB4 File Offset: 0x001E00B4
		public bool IsNull(int columnIndex)
		{
			DataColumn dataColumn = this._columns[columnIndex];
			int defaultRecord = this.GetDefaultRecord();
			return dataColumn.IsNull(defaultRecord);
		}

		// Token: 0x060007C3 RID: 1987 RVA: 0x001E0CDC File Offset: 0x001E00DC
		public bool IsNull(string columnName)
		{
			DataColumn dataColumn = this.GetDataColumn(columnName);
			int defaultRecord = this.GetDefaultRecord();
			return dataColumn.IsNull(defaultRecord);
		}

		// Token: 0x060007C4 RID: 1988 RVA: 0x001E0D00 File Offset: 0x001E0100
		public bool IsNull(DataColumn column)
		{
			this.CheckColumn(column);
			int defaultRecord = this.GetDefaultRecord();
			return column.IsNull(defaultRecord);
		}

		// Token: 0x060007C5 RID: 1989 RVA: 0x001E0D24 File Offset: 0x001E0124
		public bool IsNull(DataColumn column, DataRowVersion version)
		{
			this.CheckColumn(column);
			int recordFromVersion = this.GetRecordFromVersion(version);
			return column.IsNull(recordFromVersion);
		}

		// Token: 0x060007C6 RID: 1990 RVA: 0x001E0D48 File Offset: 0x001E0148
		public void RejectChanges()
		{
			IntPtr intPtr;
			Bid.ScopeEnter(out intPtr, "<ds.DataRow.RejectChanges|API> %d#\n", this.ObjectID);
			try
			{
				if (this.RowState != DataRowState.Detached)
				{
					if (this._columns.ColumnsImplementingIChangeTrackingCount != this._columns.ColumnsImplementingIRevertibleChangeTrackingCount)
					{
						foreach (DataColumn dataColumn in this._columns.ColumnsImplementingIChangeTracking)
						{
							if (!dataColumn.ImplementsIRevertibleChangeTracking)
							{
								object obj;
								if (this.RowState != DataRowState.Deleted)
								{
									obj = this[dataColumn];
								}
								else
								{
									obj = this[dataColumn, DataRowVersion.Original];
								}
								if (DBNull.Value != obj && ((IChangeTracking)obj).IsChanged)
								{
									throw ExceptionBuilder.UDTImplementsIChangeTrackingButnotIRevertible(dataColumn.DataType.AssemblyQualifiedName);
								}
							}
						}
					}
					foreach (DataColumn dataColumn2 in this._columns.ColumnsImplementingIChangeTracking)
					{
						object obj2;
						if (this.RowState != DataRowState.Deleted)
						{
							obj2 = this[dataColumn2];
						}
						else
						{
							obj2 = this[dataColumn2, DataRowVersion.Original];
						}
						if (DBNull.Value != obj2)
						{
							IChangeTracking changeTracking = (IChangeTracking)obj2;
							if (changeTracking.IsChanged)
							{
								((IRevertibleChangeTracking)obj2).RejectChanges();
							}
						}
					}
				}
				this._table.RollbackRow(this);
			}
			finally
			{
				Bid.ScopeLeave(ref intPtr);
			}
		}

		// Token: 0x060007C7 RID: 1991 RVA: 0x001E0EA0 File Offset: 0x001E02A0
		internal void ResetLastChangedColumn()
		{
			this._lastChangedColumn = null;
			this._countColumnChange = 0;
		}

		// Token: 0x060007C8 RID: 1992 RVA: 0x001E0EBC File Offset: 0x001E02BC
		internal void SetKeyValues(DataKey key, object[] keyValues)
		{
			bool flag = true;
			bool flag2 = this.tempRecord == -1;
			for (int i = 0; i < keyValues.Length; i++)
			{
				object obj = this[key.ColumnsReference[i]];
				if (!obj.Equals(keyValues[i]))
				{
					if (flag2 && flag)
					{
						flag = false;
						this.BeginEditInternal();
					}
					this[key.ColumnsReference[i]] = keyValues[i];
				}
			}
			if (!flag)
			{
				this.EndEdit();
			}
		}

		// Token: 0x060007C9 RID: 1993 RVA: 0x001E0F28 File Offset: 0x001E0328
		internal void SetNewRecord(int record)
		{
			this._table.SetNewRecord(this, record, DataRowAction.Change, false, true);
		}

		// Token: 0x060007CA RID: 1994 RVA: 0x001E0F48 File Offset: 0x001E0348
		protected void SetNull(DataColumn column)
		{
			this[column] = DBNull.Value;
		}

		// Token: 0x060007CB RID: 1995 RVA: 0x001E0F64 File Offset: 0x001E0364
		internal void SetNestedParentRow(DataRow parentRow, bool setNonNested)
		{
			if (parentRow == null)
			{
				this.SetParentRowToDBNull();
				return;
			}
			foreach (object obj in this._table.ParentRelations)
			{
				DataRelation dataRelation = (DataRelation)obj;
				if ((dataRelation.Nested || setNonNested) && dataRelation.ParentKey.Table == parentRow._table)
				{
					object[] keyValues = parentRow.GetKeyValues(dataRelation.ParentKey);
					this.SetKeyValues(dataRelation.ChildKey, keyValues);
					if (dataRelation.Nested)
					{
						if (parentRow._table == this._table)
						{
							this.CheckForLoops(dataRelation);
						}
						else
						{
							this.GetParentRow(dataRelation);
						}
					}
				}
			}
		}

		// Token: 0x060007CC RID: 1996 RVA: 0x001E1034 File Offset: 0x001E0434
		public void SetParentRow(DataRow parentRow)
		{
			this.SetNestedParentRow(parentRow, true);
		}

		// Token: 0x060007CD RID: 1997 RVA: 0x001E104C File Offset: 0x001E044C
		public void SetParentRow(DataRow parentRow, DataRelation relation)
		{
			if (relation == null)
			{
				this.SetParentRow(parentRow);
				return;
			}
			if (parentRow == null)
			{
				this.SetParentRowToDBNull(relation);
				return;
			}
			if (this._table.DataSet != parentRow._table.DataSet)
			{
				throw ExceptionBuilder.ParentRowNotInTheDataSet();
			}
			if (relation.ChildKey.Table != this._table)
			{
				throw ExceptionBuilder.SetParentRowTableMismatch(relation.ChildKey.Table.TableName, this._table.TableName);
			}
			if (relation.ParentKey.Table != parentRow._table)
			{
				throw ExceptionBuilder.SetParentRowTableMismatch(relation.ParentKey.Table.TableName, parentRow._table.TableName);
			}
			object[] keyValues = parentRow.GetKeyValues(relation.ParentKey);
			this.SetKeyValues(relation.ChildKey, keyValues);
		}

		// Token: 0x060007CE RID: 1998 RVA: 0x001E111C File Offset: 0x001E051C
		internal void SetParentRowToDBNull()
		{
			foreach (object obj in this._table.ParentRelations)
			{
				DataRelation dataRelation = (DataRelation)obj;
				this.SetParentRowToDBNull(dataRelation);
			}
		}

		// Token: 0x060007CF RID: 1999 RVA: 0x001E1188 File Offset: 0x001E0588
		internal void SetParentRowToDBNull(DataRelation relation)
		{
			if (relation.ChildKey.Table != this._table)
			{
				throw ExceptionBuilder.SetParentRowTableMismatch(relation.ChildKey.Table.TableName, this._table.TableName);
			}
			object[] array = new object[] { DBNull.Value };
			this.SetKeyValues(relation.ChildKey, array);
		}

		// Token: 0x060007D0 RID: 2000 RVA: 0x001E11EC File Offset: 0x001E05EC
		public void SetAdded()
		{
			if (this.RowState == DataRowState.Unchanged)
			{
				this._table.SetOldRecord(this, -1);
				return;
			}
			throw ExceptionBuilder.SetAddedAndModifiedCalledOnnonUnchanged();
		}

		// Token: 0x060007D1 RID: 2001 RVA: 0x001E1218 File Offset: 0x001E0618
		public void SetModified()
		{
			if (this.RowState != DataRowState.Unchanged)
			{
				throw ExceptionBuilder.SetAddedAndModifiedCalledOnnonUnchanged();
			}
			this.tempRecord = this._table.NewRecord(this.newRecord);
			if (this.tempRecord != -1)
			{
				this.SetNewRecord(this.tempRecord);
				return;
			}
		}

		// Token: 0x060007D2 RID: 2002 RVA: 0x001E1264 File Offset: 0x001E0664
		internal int CopyValuesIntoStore(ArrayList storeList, ArrayList nullbitList, int storeIndex)
		{
			int num = 0;
			if (this.oldRecord != -1)
			{
				for (int i = 0; i < this._columns.Count; i++)
				{
					this._columns[i].CopyValueIntoStore(this.oldRecord, storeList[i], (BitArray)nullbitList[i], storeIndex);
				}
				num++;
				storeIndex++;
			}
			DataRowState rowState = this.RowState;
			if (DataRowState.Added == rowState || DataRowState.Modified == rowState)
			{
				for (int j = 0; j < this._columns.Count; j++)
				{
					this._columns[j].CopyValueIntoStore(this.newRecord, storeList[j], (BitArray)nullbitList[j], storeIndex);
				}
				num++;
				storeIndex++;
			}
			if (-1 != this.tempRecord)
			{
				for (int k = 0; k < this._columns.Count; k++)
				{
					this._columns[k].CopyValueIntoStore(this.tempRecord, storeList[k], (BitArray)nullbitList[k], storeIndex);
				}
				num++;
				storeIndex++;
			}
			return num;
		}

		// Token: 0x060007D3 RID: 2003 RVA: 0x001E1378 File Offset: 0x001E0778
		[Conditional("DEBUG")]
		private void VerifyValueFromStorage(DataColumn column, DataRowVersion version, object valueFromStorage)
		{
			if (column.DataExpression != null && !this.inChangingEvent && this.tempRecord == -1 && version == DataRowVersion.Original && this.oldRecord == this.newRecord)
			{
				version = DataRowVersion.Current;
			}
		}

		// Token: 0x04000734 RID: 1844
		private readonly DataTable _table;

		// Token: 0x04000735 RID: 1845
		private readonly DataColumnCollection _columns;

		// Token: 0x04000736 RID: 1846
		internal int oldRecord = -1;

		// Token: 0x04000737 RID: 1847
		internal int newRecord = -1;

		// Token: 0x04000738 RID: 1848
		internal int tempRecord;

		// Token: 0x04000739 RID: 1849
		internal long _rowID = -1L;

		// Token: 0x0400073A RID: 1850
		internal DataRowAction _action;

		// Token: 0x0400073B RID: 1851
		internal bool inChangingEvent;

		// Token: 0x0400073C RID: 1852
		internal bool inDeletingEvent;

		// Token: 0x0400073D RID: 1853
		internal bool inCascade;

		// Token: 0x0400073E RID: 1854
		private DataColumn _lastChangedColumn;

		// Token: 0x0400073F RID: 1855
		private int _countColumnChange;

		// Token: 0x04000740 RID: 1856
		private DataError error;

		// Token: 0x04000741 RID: 1857
		private object _element;

		// Token: 0x04000742 RID: 1858
		private int _rbTreeNodeId;

		// Token: 0x04000743 RID: 1859
		private static int _objectTypeCount;

		// Token: 0x04000744 RID: 1860
		internal readonly int ObjectID = Interlocked.Increment(ref DataRow._objectTypeCount);
	}
}
