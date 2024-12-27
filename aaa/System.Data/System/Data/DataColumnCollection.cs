using System;
using System.Collections;
using System.ComponentModel;
using System.Data.Common;
using System.Globalization;

namespace System.Data
{
	// Token: 0x0200006B RID: 107
	[Editor("Microsoft.VSDesigner.Data.Design.ColumnsCollectionEditor, Microsoft.VSDesigner, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", "System.Drawing.Design.UITypeEditor, System.Drawing, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	[DefaultEvent("CollectionChanged")]
	public sealed class DataColumnCollection : InternalDataCollectionBase
	{
		// Token: 0x0600054D RID: 1357 RVA: 0x001D88CC File Offset: 0x001D7CCC
		internal DataColumnCollection(DataTable table)
		{
			this.table = table;
			this.columnFromName = new Hashtable();
		}

		// Token: 0x170000B3 RID: 179
		// (get) Token: 0x0600054E RID: 1358 RVA: 0x001D8910 File Offset: 0x001D7D10
		protected override ArrayList List
		{
			get
			{
				return this._list;
			}
		}

		// Token: 0x170000B4 RID: 180
		// (get) Token: 0x0600054F RID: 1359 RVA: 0x001D8924 File Offset: 0x001D7D24
		internal DataColumn[] ColumnsImplementingIChangeTracking
		{
			get
			{
				return this.columnsImplementingIChangeTracking;
			}
		}

		// Token: 0x170000B5 RID: 181
		// (get) Token: 0x06000550 RID: 1360 RVA: 0x001D8938 File Offset: 0x001D7D38
		internal int ColumnsImplementingIChangeTrackingCount
		{
			get
			{
				return this.nColumnsImplementingIChangeTracking;
			}
		}

		// Token: 0x170000B6 RID: 182
		// (get) Token: 0x06000551 RID: 1361 RVA: 0x001D894C File Offset: 0x001D7D4C
		internal int ColumnsImplementingIRevertibleChangeTrackingCount
		{
			get
			{
				return this.nColumnsImplementingIRevertibleChangeTracking;
			}
		}

		// Token: 0x170000B7 RID: 183
		public DataColumn this[int index]
		{
			get
			{
				DataColumn dataColumn;
				try
				{
					dataColumn = (DataColumn)this._list[index];
				}
				catch (ArgumentOutOfRangeException)
				{
					throw ExceptionBuilder.ColumnOutOfRange(index);
				}
				return dataColumn;
			}
		}

		// Token: 0x170000B8 RID: 184
		public DataColumn this[string name]
		{
			get
			{
				if (name == null)
				{
					throw ExceptionBuilder.ArgumentNull("name");
				}
				DataColumn dataColumn = this.columnFromName[name] as DataColumn;
				if (dataColumn == null)
				{
					int num = this.IndexOfCaseInsensitive(name);
					if (0 <= num)
					{
						dataColumn = (DataColumn)this._list[num];
					}
					else if (-2 == num)
					{
						throw ExceptionBuilder.CaseInsensitiveNameConflict(name);
					}
				}
				return dataColumn;
			}
		}

		// Token: 0x170000B9 RID: 185
		internal DataColumn this[string name, string ns]
		{
			get
			{
				DataColumn dataColumn = this.columnFromName[name] as DataColumn;
				if (dataColumn != null && dataColumn.Namespace == ns)
				{
					return dataColumn;
				}
				return null;
			}
		}

		// Token: 0x06000555 RID: 1365 RVA: 0x001D8A3C File Offset: 0x001D7E3C
		public void Add(DataColumn column)
		{
			this.AddAt(-1, column);
		}

		// Token: 0x06000556 RID: 1366 RVA: 0x001D8A54 File Offset: 0x001D7E54
		internal void AddAt(int index, DataColumn column)
		{
			if (column != null && column.ColumnMapping == MappingType.SimpleContent)
			{
				if (this.table.XmlText != null && this.table.XmlText != column)
				{
					throw ExceptionBuilder.CannotAddColumn3();
				}
				if (this.table.ElementColumnCount > 0)
				{
					throw ExceptionBuilder.CannotAddColumn4(column.ColumnName);
				}
				this.OnCollectionChanging(new CollectionChangeEventArgs(CollectionChangeAction.Add, column));
				this.BaseAdd(column);
				if (index != -1)
				{
					this.ArrayAdd(index, column);
				}
				else
				{
					this.ArrayAdd(column);
				}
				this.table.XmlText = column;
			}
			else
			{
				this.OnCollectionChanging(new CollectionChangeEventArgs(CollectionChangeAction.Add, column));
				this.BaseAdd(column);
				if (index != -1)
				{
					this.ArrayAdd(index, column);
				}
				else
				{
					this.ArrayAdd(column);
				}
				if (column.ColumnMapping == MappingType.Element)
				{
					this.table.ElementColumnCount++;
				}
			}
			if (!this.table.fInitInProgress && column != null && column.Computed)
			{
				column.Expression = column.Expression;
			}
			this.OnCollectionChanged(new CollectionChangeEventArgs(CollectionChangeAction.Add, column));
		}

		// Token: 0x06000557 RID: 1367 RVA: 0x001D8B58 File Offset: 0x001D7F58
		public void AddRange(DataColumn[] columns)
		{
			if (this.table.fInitInProgress)
			{
				this.delayedAddRangeColumns = columns;
				return;
			}
			if (columns != null)
			{
				foreach (DataColumn dataColumn in columns)
				{
					if (dataColumn != null)
					{
						this.Add(dataColumn);
					}
				}
			}
		}

		// Token: 0x06000558 RID: 1368 RVA: 0x001D8B9C File Offset: 0x001D7F9C
		public DataColumn Add(string columnName, Type type, string expression)
		{
			DataColumn dataColumn = new DataColumn(columnName, type, expression);
			this.Add(dataColumn);
			return dataColumn;
		}

		// Token: 0x06000559 RID: 1369 RVA: 0x001D8BBC File Offset: 0x001D7FBC
		public DataColumn Add(string columnName, Type type)
		{
			DataColumn dataColumn = new DataColumn(columnName, type);
			this.Add(dataColumn);
			return dataColumn;
		}

		// Token: 0x0600055A RID: 1370 RVA: 0x001D8BDC File Offset: 0x001D7FDC
		public DataColumn Add(string columnName)
		{
			DataColumn dataColumn = new DataColumn(columnName);
			this.Add(dataColumn);
			return dataColumn;
		}

		// Token: 0x0600055B RID: 1371 RVA: 0x001D8BF8 File Offset: 0x001D7FF8
		public DataColumn Add()
		{
			DataColumn dataColumn = new DataColumn();
			this.Add(dataColumn);
			return dataColumn;
		}

		// Token: 0x14000004 RID: 4
		// (add) Token: 0x0600055C RID: 1372 RVA: 0x001D8C14 File Offset: 0x001D8014
		// (remove) Token: 0x0600055D RID: 1373 RVA: 0x001D8C38 File Offset: 0x001D8038
		[ResDescription("collectionChangedEventDescr")]
		public event CollectionChangeEventHandler CollectionChanged
		{
			add
			{
				this.onCollectionChangedDelegate = (CollectionChangeEventHandler)Delegate.Combine(this.onCollectionChangedDelegate, value);
			}
			remove
			{
				this.onCollectionChangedDelegate = (CollectionChangeEventHandler)Delegate.Remove(this.onCollectionChangedDelegate, value);
			}
		}

		// Token: 0x14000005 RID: 5
		// (add) Token: 0x0600055E RID: 1374 RVA: 0x001D8C5C File Offset: 0x001D805C
		// (remove) Token: 0x0600055F RID: 1375 RVA: 0x001D8C80 File Offset: 0x001D8080
		internal event CollectionChangeEventHandler CollectionChanging
		{
			add
			{
				this.onCollectionChangingDelegate = (CollectionChangeEventHandler)Delegate.Combine(this.onCollectionChangingDelegate, value);
			}
			remove
			{
				this.onCollectionChangingDelegate = (CollectionChangeEventHandler)Delegate.Remove(this.onCollectionChangingDelegate, value);
			}
		}

		// Token: 0x14000006 RID: 6
		// (add) Token: 0x06000560 RID: 1376 RVA: 0x001D8CA4 File Offset: 0x001D80A4
		// (remove) Token: 0x06000561 RID: 1377 RVA: 0x001D8CC8 File Offset: 0x001D80C8
		internal event CollectionChangeEventHandler ColumnPropertyChanged
		{
			add
			{
				this.onColumnPropertyChangedDelegate = (CollectionChangeEventHandler)Delegate.Combine(this.onColumnPropertyChangedDelegate, value);
			}
			remove
			{
				this.onColumnPropertyChangedDelegate = (CollectionChangeEventHandler)Delegate.Remove(this.onColumnPropertyChangedDelegate, value);
			}
		}

		// Token: 0x06000562 RID: 1378 RVA: 0x001D8CEC File Offset: 0x001D80EC
		private void ArrayAdd(DataColumn column)
		{
			this._list.Add(column);
			column.SetOrdinalInternal(this._list.Count - 1);
			this.CheckIChangeTracking(column);
		}

		// Token: 0x06000563 RID: 1379 RVA: 0x001D8D20 File Offset: 0x001D8120
		private void ArrayAdd(int index, DataColumn column)
		{
			this._list.Insert(index, column);
			this.CheckIChangeTracking(column);
		}

		// Token: 0x06000564 RID: 1380 RVA: 0x001D8D44 File Offset: 0x001D8144
		private void ArrayRemove(DataColumn column)
		{
			column.SetOrdinalInternal(-1);
			this._list.Remove(column);
			int count = this._list.Count;
			for (int i = 0; i < count; i++)
			{
				((DataColumn)this._list[i]).SetOrdinalInternal(i);
			}
			if (column.ImplementsIChangeTracking)
			{
				this.RemoveColumnsImplementingIChangeTrackingList(column);
			}
		}

		// Token: 0x06000565 RID: 1381 RVA: 0x001D8DA4 File Offset: 0x001D81A4
		internal string AssignName()
		{
			string text = this.MakeName(this.defaultNameIndex++);
			while (this.columnFromName[text] != null)
			{
				text = this.MakeName(this.defaultNameIndex++);
			}
			return text;
		}

		// Token: 0x06000566 RID: 1382 RVA: 0x001D8DF4 File Offset: 0x001D81F4
		private void BaseAdd(DataColumn column)
		{
			if (column == null)
			{
				throw ExceptionBuilder.ArgumentNull("column");
			}
			if (column.table == this.table)
			{
				throw ExceptionBuilder.CannotAddColumn1(column.ColumnName);
			}
			if (column.table != null)
			{
				throw ExceptionBuilder.CannotAddColumn2(column.ColumnName);
			}
			if (column.ColumnName.Length == 0)
			{
				column.ColumnName = this.AssignName();
			}
			this.RegisterColumnName(column.ColumnName, column, null);
			try
			{
				column.SetTable(this.table);
				if (!this.table.fInitInProgress && column.Computed && column.DataExpression.DependsOn(column))
				{
					throw ExceptionBuilder.ExpressionCircular();
				}
				if (0 < this.table.RecordCapacity)
				{
					column.SetCapacity(this.table.RecordCapacity);
				}
				for (int i = 0; i < this.table.RecordCapacity; i++)
				{
					column.InitializeRecord(i);
				}
				if (this.table.DataSet != null)
				{
					column.OnSetDataSet();
				}
			}
			catch (Exception ex)
			{
				if (ADP.IsCatchableOrSecurityExceptionType(ex))
				{
					this.UnregisterName(column.ColumnName);
				}
				throw;
			}
		}

		// Token: 0x06000567 RID: 1383 RVA: 0x001D8F20 File Offset: 0x001D8320
		private void BaseGroupSwitch(DataColumn[] oldArray, int oldLength, DataColumn[] newArray, int newLength)
		{
			int num = 0;
			for (int i = 0; i < oldLength; i++)
			{
				bool flag = false;
				for (int j = num; j < newLength; j++)
				{
					if (oldArray[i] == newArray[j])
					{
						if (num == j)
						{
							num++;
						}
						flag = true;
						break;
					}
				}
				if (!flag && oldArray[i].Table == this.table)
				{
					this.BaseRemove(oldArray[i]);
					this._list.Remove(oldArray[i]);
					oldArray[i].SetOrdinalInternal(-1);
				}
			}
			for (int k = 0; k < newLength; k++)
			{
				if (newArray[k].Table != this.table)
				{
					this.BaseAdd(newArray[k]);
					this._list.Add(newArray[k]);
				}
				newArray[k].SetOrdinalInternal(k);
			}
		}

		// Token: 0x06000568 RID: 1384 RVA: 0x001D8FD4 File Offset: 0x001D83D4
		private void BaseRemove(DataColumn column)
		{
			if (this.CanRemove(column, true))
			{
				if (column.errors > 0)
				{
					for (int i = 0; i < this.table.Rows.Count; i++)
					{
						this.table.Rows[i].ClearError(column);
					}
				}
				this.UnregisterName(column.ColumnName);
				column.SetTable(null);
			}
		}

		// Token: 0x06000569 RID: 1385 RVA: 0x001D903C File Offset: 0x001D843C
		public bool CanRemove(DataColumn column)
		{
			return this.CanRemove(column, false);
		}

		// Token: 0x0600056A RID: 1386 RVA: 0x001D9054 File Offset: 0x001D8454
		internal bool CanRemove(DataColumn column, bool fThrowException)
		{
			if (column == null)
			{
				if (!fThrowException)
				{
					return false;
				}
				throw ExceptionBuilder.ArgumentNull("column");
			}
			else if (column.table != this.table)
			{
				if (!fThrowException)
				{
					return false;
				}
				throw ExceptionBuilder.CannotRemoveColumn();
			}
			else
			{
				this.table.OnRemoveColumnInternal(column);
				if (this.table.primaryKey == null || !this.table.primaryKey.Key.ContainsColumn(column))
				{
					int i = 0;
					while (i < this.table.ParentRelations.Count)
					{
						if (this.table.ParentRelations[i].ChildKey.ContainsColumn(column))
						{
							if (!fThrowException)
							{
								return false;
							}
							throw ExceptionBuilder.CannotRemoveChildKey(this.table.ParentRelations[i].RelationName);
						}
						else
						{
							i++;
						}
					}
					int j = 0;
					while (j < this.table.ChildRelations.Count)
					{
						if (this.table.ChildRelations[j].ParentKey.ContainsColumn(column))
						{
							if (!fThrowException)
							{
								return false;
							}
							throw ExceptionBuilder.CannotRemoveChildKey(this.table.ChildRelations[j].RelationName);
						}
						else
						{
							j++;
						}
					}
					int k = 0;
					while (k < this.table.Constraints.Count)
					{
						if (this.table.Constraints[k].ContainsColumn(column))
						{
							if (!fThrowException)
							{
								return false;
							}
							throw ExceptionBuilder.CannotRemoveConstraint(this.table.Constraints[k].ConstraintName, this.table.Constraints[k].Table.TableName);
						}
						else
						{
							k++;
						}
					}
					if (this.table.DataSet != null)
					{
						ParentForeignKeyConstraintEnumerator parentForeignKeyConstraintEnumerator = new ParentForeignKeyConstraintEnumerator(this.table.DataSet, this.table);
						while (parentForeignKeyConstraintEnumerator.GetNext())
						{
							Constraint constraint = parentForeignKeyConstraintEnumerator.GetConstraint();
							if (((ForeignKeyConstraint)constraint).ParentKey.ContainsColumn(column))
							{
								if (!fThrowException)
								{
									return false;
								}
								throw ExceptionBuilder.CannotRemoveConstraint(constraint.ConstraintName, constraint.Table.TableName);
							}
						}
					}
					if (column.dependentColumns != null)
					{
						for (int l = 0; l < column.dependentColumns.Count; l++)
						{
							DataColumn dataColumn = column.dependentColumns[l];
							if ((!this.fInClear || (dataColumn.Table != this.table && dataColumn.Table != null)) && dataColumn.Table != null)
							{
								DataExpression dataExpression = dataColumn.DataExpression;
								if (dataExpression != null && dataExpression.DependsOn(column))
								{
									if (!fThrowException)
									{
										return false;
									}
									throw ExceptionBuilder.CannotRemoveExpression(dataColumn.ColumnName, dataColumn.Expression);
								}
							}
						}
					}
					foreach (Index index in this.table.LiveIndexes)
					{
					}
					return true;
				}
				if (!fThrowException)
				{
					return false;
				}
				throw ExceptionBuilder.CannotRemovePrimaryKey();
			}
		}

		// Token: 0x0600056B RID: 1387 RVA: 0x001D9340 File Offset: 0x001D8740
		private void CheckIChangeTracking(DataColumn column)
		{
			if (column.ImplementsIRevertibleChangeTracking)
			{
				this.nColumnsImplementingIRevertibleChangeTracking++;
				this.nColumnsImplementingIChangeTracking++;
				this.AddColumnsImplementingIChangeTrackingList(column);
				return;
			}
			if (column.ImplementsIChangeTracking)
			{
				this.nColumnsImplementingIChangeTracking++;
				this.AddColumnsImplementingIChangeTrackingList(column);
			}
		}

		// Token: 0x0600056C RID: 1388 RVA: 0x001D9398 File Offset: 0x001D8798
		public void Clear()
		{
			int count = this._list.Count;
			DataColumn[] array = new DataColumn[this._list.Count];
			this._list.CopyTo(array, 0);
			this.OnCollectionChanging(InternalDataCollectionBase.RefreshEventArgs);
			if (this.table.fInitInProgress && this.delayedAddRangeColumns != null)
			{
				this.delayedAddRangeColumns = null;
			}
			try
			{
				this.fInClear = true;
				this.BaseGroupSwitch(array, count, null, 0);
				this.fInClear = false;
			}
			catch (Exception ex)
			{
				if (ADP.IsCatchableOrSecurityExceptionType(ex))
				{
					this.fInClear = false;
					this.BaseGroupSwitch(null, 0, array, count);
					this._list.Clear();
					for (int i = 0; i < count; i++)
					{
						this._list.Add(array[i]);
					}
				}
				throw;
			}
			this._list.Clear();
			this.table.ElementColumnCount = 0;
			this.OnCollectionChanged(InternalDataCollectionBase.RefreshEventArgs);
		}

		// Token: 0x0600056D RID: 1389 RVA: 0x001D9494 File Offset: 0x001D8894
		public bool Contains(string name)
		{
			DataColumn dataColumn = this.columnFromName[name] as DataColumn;
			return dataColumn != null || this.IndexOfCaseInsensitive(name) >= 0;
		}

		// Token: 0x0600056E RID: 1390 RVA: 0x001D94C8 File Offset: 0x001D88C8
		internal bool Contains(string name, bool caseSensitive)
		{
			DataColumn dataColumn = this.columnFromName[name] as DataColumn;
			return dataColumn != null || (!caseSensitive && this.IndexOfCaseInsensitive(name) >= 0);
		}

		// Token: 0x0600056F RID: 1391 RVA: 0x001D9500 File Offset: 0x001D8900
		public void CopyTo(DataColumn[] array, int index)
		{
			if (array == null)
			{
				throw ExceptionBuilder.ArgumentNull("array");
			}
			if (index < 0)
			{
				throw ExceptionBuilder.ArgumentOutOfRange("index");
			}
			if (array.Length - index < this._list.Count)
			{
				throw ExceptionBuilder.InvalidOffsetLength();
			}
			for (int i = 0; i < this._list.Count; i++)
			{
				array[index + i] = (DataColumn)this._list[i];
			}
		}

		// Token: 0x06000570 RID: 1392 RVA: 0x001D9570 File Offset: 0x001D8970
		public int IndexOf(DataColumn column)
		{
			int count = this._list.Count;
			for (int i = 0; i < count; i++)
			{
				if (column == (DataColumn)this._list[i])
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x06000571 RID: 1393 RVA: 0x001D95AC File Offset: 0x001D89AC
		public int IndexOf(string columnName)
		{
			if (columnName != null && 0 < columnName.Length)
			{
				int count = this.Count;
				DataColumn dataColumn = this.columnFromName[columnName] as DataColumn;
				if (dataColumn != null)
				{
					for (int i = 0; i < count; i++)
					{
						if (dataColumn == this._list[i])
						{
							return i;
						}
					}
				}
				else
				{
					int num = this.IndexOfCaseInsensitive(columnName);
					if (num >= 0)
					{
						return num;
					}
					return -1;
				}
			}
			return -1;
		}

		// Token: 0x06000572 RID: 1394 RVA: 0x001D9614 File Offset: 0x001D8A14
		internal int IndexOfCaseInsensitive(string name)
		{
			int specialHashCode = this.table.GetSpecialHashCode(name);
			int num = -1;
			for (int i = 0; i < this.Count; i++)
			{
				DataColumn dataColumn = (DataColumn)this._list[i];
				if ((specialHashCode == 0 || dataColumn._hashCode == 0 || dataColumn._hashCode == specialHashCode) && base.NamesEqual(dataColumn.ColumnName, name, false, this.table.Locale) != 0)
				{
					if (num != -1)
					{
						return -2;
					}
					num = i;
				}
			}
			return num;
		}

		// Token: 0x06000573 RID: 1395 RVA: 0x001D9690 File Offset: 0x001D8A90
		internal void FinishInitCollection()
		{
			if (this.delayedAddRangeColumns != null)
			{
				foreach (DataColumn dataColumn in this.delayedAddRangeColumns)
				{
					if (dataColumn != null)
					{
						this.Add(dataColumn);
					}
				}
				foreach (DataColumn dataColumn2 in this.delayedAddRangeColumns)
				{
					if (dataColumn2 != null)
					{
						dataColumn2.FinishInitInProgress();
					}
				}
				this.delayedAddRangeColumns = null;
			}
		}

		// Token: 0x06000574 RID: 1396 RVA: 0x001D96F8 File Offset: 0x001D8AF8
		private string MakeName(int index)
		{
			if (1 == index)
			{
				return "Column1";
			}
			return "Column" + index.ToString(CultureInfo.InvariantCulture);
		}

		// Token: 0x06000575 RID: 1397 RVA: 0x001D9728 File Offset: 0x001D8B28
		internal void MoveTo(DataColumn column, int newPosition)
		{
			if (0 > newPosition || newPosition > this.Count - 1)
			{
				throw ExceptionBuilder.InvalidOrdinal("ordinal", newPosition);
			}
			if (column.ImplementsIChangeTracking)
			{
				this.RemoveColumnsImplementingIChangeTrackingList(column);
			}
			this._list.Remove(column);
			this._list.Insert(newPosition, column);
			int count = this._list.Count;
			for (int i = 0; i < count; i++)
			{
				((DataColumn)this._list[i]).SetOrdinalInternal(i);
			}
			this.CheckIChangeTracking(column);
			this.OnCollectionChanged(new CollectionChangeEventArgs(CollectionChangeAction.Refresh, column));
		}

		// Token: 0x06000576 RID: 1398 RVA: 0x001D97BC File Offset: 0x001D8BBC
		private void OnCollectionChanged(CollectionChangeEventArgs ccevent)
		{
			this.table.UpdatePropertyDescriptorCollectionCache();
			if (ccevent != null && !this.table.SchemaLoading && !this.table.fInitInProgress)
			{
				DataColumn dataColumn = (DataColumn)ccevent.Element;
			}
			if (this.onCollectionChangedDelegate != null)
			{
				this.onCollectionChangedDelegate(this, ccevent);
			}
		}

		// Token: 0x06000577 RID: 1399 RVA: 0x001D9814 File Offset: 0x001D8C14
		private void OnCollectionChanging(CollectionChangeEventArgs ccevent)
		{
			if (this.onCollectionChangingDelegate != null)
			{
				this.onCollectionChangingDelegate(this, ccevent);
			}
		}

		// Token: 0x06000578 RID: 1400 RVA: 0x001D9838 File Offset: 0x001D8C38
		internal void OnColumnPropertyChanged(CollectionChangeEventArgs ccevent)
		{
			this.table.UpdatePropertyDescriptorCollectionCache();
			if (this.onColumnPropertyChangedDelegate != null)
			{
				this.onColumnPropertyChangedDelegate(this, ccevent);
			}
		}

		// Token: 0x06000579 RID: 1401 RVA: 0x001D9868 File Offset: 0x001D8C68
		internal void RegisterColumnName(string name, DataColumn column, DataTable table)
		{
			object obj = this.columnFromName[name];
			if (obj != null)
			{
				if (!(obj is DataColumn))
				{
					throw ExceptionBuilder.CannotAddDuplicate2(name);
				}
				if (column != null)
				{
					throw ExceptionBuilder.CannotAddDuplicate(name);
				}
				throw ExceptionBuilder.CannotAddDuplicate3(name);
			}
			else
			{
				if (table != null && base.NamesEqual(name, this.MakeName(this.defaultNameIndex), true, this.table.Locale) != 0)
				{
					do
					{
						this.defaultNameIndex++;
					}
					while (this.Contains(this.MakeName(this.defaultNameIndex)));
				}
				if (column != null)
				{
					column._hashCode = this.table.GetSpecialHashCode(name);
					this.columnFromName.Add(name, column);
					return;
				}
				this.columnFromName.Add(name, table);
				return;
			}
		}

		// Token: 0x0600057A RID: 1402 RVA: 0x001D991C File Offset: 0x001D8D1C
		internal bool CanRegisterName(string name)
		{
			return null == this.columnFromName[name];
		}

		// Token: 0x0600057B RID: 1403 RVA: 0x001D9938 File Offset: 0x001D8D38
		public void Remove(DataColumn column)
		{
			this.OnCollectionChanging(new CollectionChangeEventArgs(CollectionChangeAction.Remove, column));
			this.BaseRemove(column);
			this.ArrayRemove(column);
			this.OnCollectionChanged(new CollectionChangeEventArgs(CollectionChangeAction.Remove, column));
			if (column.ColumnMapping == MappingType.Element)
			{
				this.table.ElementColumnCount--;
			}
		}

		// Token: 0x0600057C RID: 1404 RVA: 0x001D998C File Offset: 0x001D8D8C
		public void RemoveAt(int index)
		{
			DataColumn dataColumn = this[index];
			if (dataColumn == null)
			{
				throw ExceptionBuilder.ColumnOutOfRange(index);
			}
			this.Remove(dataColumn);
		}

		// Token: 0x0600057D RID: 1405 RVA: 0x001D99B4 File Offset: 0x001D8DB4
		public void Remove(string name)
		{
			DataColumn dataColumn = this[name];
			if (dataColumn == null)
			{
				throw ExceptionBuilder.ColumnNotInTheTable(name, this.table.TableName);
			}
			this.Remove(dataColumn);
		}

		// Token: 0x0600057E RID: 1406 RVA: 0x001D99E8 File Offset: 0x001D8DE8
		internal void UnregisterName(string name)
		{
			object obj = this.columnFromName[name];
			if (obj != null)
			{
				this.columnFromName.Remove(name);
			}
			if (base.NamesEqual(name, this.MakeName(this.defaultNameIndex - 1), true, this.table.Locale) != 0)
			{
				do
				{
					this.defaultNameIndex--;
				}
				while (this.defaultNameIndex > 1 && !this.Contains(this.MakeName(this.defaultNameIndex - 1)));
			}
		}

		// Token: 0x0600057F RID: 1407 RVA: 0x001D9A64 File Offset: 0x001D8E64
		private void AddColumnsImplementingIChangeTrackingList(DataColumn dataColumn)
		{
			DataColumn[] array = this.columnsImplementingIChangeTracking;
			DataColumn[] array2 = new DataColumn[array.Length + 1];
			array.CopyTo(array2, 0);
			array2[array.Length] = dataColumn;
			this.columnsImplementingIChangeTracking = array2;
		}

		// Token: 0x06000580 RID: 1408 RVA: 0x001D9A98 File Offset: 0x001D8E98
		private void RemoveColumnsImplementingIChangeTrackingList(DataColumn dataColumn)
		{
			DataColumn[] array = this.columnsImplementingIChangeTracking;
			DataColumn[] array2 = new DataColumn[array.Length - 1];
			int i = 0;
			int num = 0;
			while (i < array.Length)
			{
				if (array[i] != dataColumn)
				{
					array2[num++] = array[i];
				}
				i++;
			}
			this.columnsImplementingIChangeTracking = array2;
		}

		// Token: 0x040006FC RID: 1788
		private readonly DataTable table;

		// Token: 0x040006FD RID: 1789
		private readonly ArrayList _list = new ArrayList();

		// Token: 0x040006FE RID: 1790
		private int defaultNameIndex = 1;

		// Token: 0x040006FF RID: 1791
		private DataColumn[] delayedAddRangeColumns;

		// Token: 0x04000700 RID: 1792
		private readonly Hashtable columnFromName;

		// Token: 0x04000701 RID: 1793
		private CollectionChangeEventHandler onCollectionChangedDelegate;

		// Token: 0x04000702 RID: 1794
		private CollectionChangeEventHandler onCollectionChangingDelegate;

		// Token: 0x04000703 RID: 1795
		private CollectionChangeEventHandler onColumnPropertyChangedDelegate;

		// Token: 0x04000704 RID: 1796
		private bool fInClear;

		// Token: 0x04000705 RID: 1797
		private DataColumn[] columnsImplementingIChangeTracking = DataTable.zeroColumns;

		// Token: 0x04000706 RID: 1798
		private int nColumnsImplementingIChangeTracking;

		// Token: 0x04000707 RID: 1799
		private int nColumnsImplementingIRevertibleChangeTracking;
	}
}
