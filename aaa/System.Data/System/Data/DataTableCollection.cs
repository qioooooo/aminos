using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Threading;

namespace System.Data
{
	// Token: 0x0200009D RID: 157
	[DefaultEvent("CollectionChanged")]
	[Editor("Microsoft.VSDesigner.Data.Design.TablesCollectionEditor, Microsoft.VSDesigner, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", "System.Drawing.Design.UITypeEditor, System.Drawing, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	[ListBindable(false)]
	public sealed class DataTableCollection : InternalDataCollectionBase
	{
		// Token: 0x06000A6D RID: 2669 RVA: 0x001F3498 File Offset: 0x001F2898
		internal DataTableCollection(DataSet dataSet)
		{
			Bid.Trace("<ds.DataTableCollection.DataTableCollection|INFO> %d#, dataSet=%d\n", this.ObjectID, (dataSet != null) ? dataSet.ObjectID : 0);
			this.dataSet = dataSet;
		}

		// Token: 0x17000150 RID: 336
		// (get) Token: 0x06000A6E RID: 2670 RVA: 0x001F34F0 File Offset: 0x001F28F0
		protected override ArrayList List
		{
			get
			{
				return this._list;
			}
		}

		// Token: 0x17000151 RID: 337
		// (get) Token: 0x06000A6F RID: 2671 RVA: 0x001F3504 File Offset: 0x001F2904
		internal int ObjectID
		{
			get
			{
				return this._objectID;
			}
		}

		// Token: 0x17000152 RID: 338
		public DataTable this[int index]
		{
			get
			{
				DataTable dataTable;
				try
				{
					dataTable = (DataTable)this._list[index];
				}
				catch (ArgumentOutOfRangeException)
				{
					throw ExceptionBuilder.TableOutOfRange(index);
				}
				return dataTable;
			}
		}

		// Token: 0x17000153 RID: 339
		public DataTable this[string name]
		{
			get
			{
				int num = this.InternalIndexOf(name);
				if (num == -2)
				{
					throw ExceptionBuilder.CaseInsensitiveNameConflict(name);
				}
				if (num == -3)
				{
					throw ExceptionBuilder.NamespaceNameConflict(name);
				}
				if (num >= 0)
				{
					return (DataTable)this._list[num];
				}
				return null;
			}
		}

		// Token: 0x17000154 RID: 340
		public DataTable this[string name, string tableNamespace]
		{
			get
			{
				if (tableNamespace == null)
				{
					throw ExceptionBuilder.ArgumentNull("tableNamespace");
				}
				int num = this.InternalIndexOf(name, tableNamespace);
				if (num == -2)
				{
					throw ExceptionBuilder.CaseInsensitiveNameConflict(name);
				}
				if (num >= 0)
				{
					return (DataTable)this._list[num];
				}
				return null;
			}
		}

		// Token: 0x06000A73 RID: 2675 RVA: 0x001F35EC File Offset: 0x001F29EC
		internal DataTable GetTable(string name, string ns)
		{
			for (int i = 0; i < this._list.Count; i++)
			{
				DataTable dataTable = (DataTable)this._list[i];
				if (dataTable.TableName == name && dataTable.Namespace == ns)
				{
					return dataTable;
				}
			}
			return null;
		}

		// Token: 0x06000A74 RID: 2676 RVA: 0x001F3640 File Offset: 0x001F2A40
		internal DataTable GetTableSmart(string name, string ns)
		{
			int num = 0;
			DataTable dataTable = null;
			for (int i = 0; i < this._list.Count; i++)
			{
				DataTable dataTable2 = (DataTable)this._list[i];
				if (dataTable2.TableName == name)
				{
					if (dataTable2.Namespace == ns)
					{
						return dataTable2;
					}
					num++;
					dataTable = dataTable2;
				}
			}
			if (num != 1)
			{
				return null;
			}
			return dataTable;
		}

		// Token: 0x06000A75 RID: 2677 RVA: 0x001F36A4 File Offset: 0x001F2AA4
		public void Add(DataTable table)
		{
			IntPtr intPtr;
			Bid.ScopeEnter(out intPtr, "<ds.DataTableCollection.Add|API> %d#, table=%d\n", this.ObjectID, (table != null) ? table.ObjectID : 0);
			try
			{
				this.OnCollectionChanging(new CollectionChangeEventArgs(CollectionChangeAction.Add, table));
				this.BaseAdd(table);
				this.ArrayAdd(table);
				if (table.SetLocaleValue(this.dataSet.Locale, false, false) || table.SetCaseSensitiveValue(this.dataSet.CaseSensitive, false, false))
				{
					table.ResetIndexes();
				}
				this.OnCollectionChanged(new CollectionChangeEventArgs(CollectionChangeAction.Add, table));
			}
			finally
			{
				Bid.ScopeLeave(ref intPtr);
			}
		}

		// Token: 0x06000A76 RID: 2678 RVA: 0x001F3750 File Offset: 0x001F2B50
		public void AddRange(DataTable[] tables)
		{
			IntPtr intPtr;
			Bid.ScopeEnter(out intPtr, "<ds.DataTableCollection.AddRange|API> %d#\n", this.ObjectID);
			try
			{
				if (this.dataSet.fInitInProgress)
				{
					this.delayedAddRangeTables = tables;
				}
				else if (tables != null)
				{
					foreach (DataTable dataTable in tables)
					{
						if (dataTable != null)
						{
							this.Add(dataTable);
						}
					}
				}
			}
			finally
			{
				Bid.ScopeLeave(ref intPtr);
			}
		}

		// Token: 0x06000A77 RID: 2679 RVA: 0x001F37CC File Offset: 0x001F2BCC
		public DataTable Add(string name)
		{
			DataTable dataTable = new DataTable(name);
			this.Add(dataTable);
			return dataTable;
		}

		// Token: 0x06000A78 RID: 2680 RVA: 0x001F37E8 File Offset: 0x001F2BE8
		public DataTable Add(string name, string tableNamespace)
		{
			DataTable dataTable = new DataTable(name, tableNamespace);
			this.Add(dataTable);
			return dataTable;
		}

		// Token: 0x06000A79 RID: 2681 RVA: 0x001F3808 File Offset: 0x001F2C08
		public DataTable Add()
		{
			DataTable dataTable = new DataTable();
			this.Add(dataTable);
			return dataTable;
		}

		// Token: 0x1400001C RID: 28
		// (add) Token: 0x06000A7A RID: 2682 RVA: 0x001F3824 File Offset: 0x001F2C24
		// (remove) Token: 0x06000A7B RID: 2683 RVA: 0x001F3858 File Offset: 0x001F2C58
		[ResDescription("collectionChangedEventDescr")]
		public event CollectionChangeEventHandler CollectionChanged
		{
			add
			{
				Bid.Trace("<ds.DataTableCollection.add_CollectionChanged|API> %d#\n", this.ObjectID);
				this.onCollectionChangedDelegate = (CollectionChangeEventHandler)Delegate.Combine(this.onCollectionChangedDelegate, value);
			}
			remove
			{
				Bid.Trace("<ds.DataTableCollection.remove_CollectionChanged|API> %d#\n", this.ObjectID);
				this.onCollectionChangedDelegate = (CollectionChangeEventHandler)Delegate.Remove(this.onCollectionChangedDelegate, value);
			}
		}

		// Token: 0x1400001D RID: 29
		// (add) Token: 0x06000A7C RID: 2684 RVA: 0x001F388C File Offset: 0x001F2C8C
		// (remove) Token: 0x06000A7D RID: 2685 RVA: 0x001F38C0 File Offset: 0x001F2CC0
		public event CollectionChangeEventHandler CollectionChanging
		{
			add
			{
				Bid.Trace("<ds.DataTableCollection.add_CollectionChanging|API> %d#\n", this.ObjectID);
				this.onCollectionChangingDelegate = (CollectionChangeEventHandler)Delegate.Combine(this.onCollectionChangingDelegate, value);
			}
			remove
			{
				Bid.Trace("<ds.DataTableCollection.remove_CollectionChanging|API> %d#\n", this.ObjectID);
				this.onCollectionChangingDelegate = (CollectionChangeEventHandler)Delegate.Remove(this.onCollectionChangingDelegate, value);
			}
		}

		// Token: 0x06000A7E RID: 2686 RVA: 0x001F38F4 File Offset: 0x001F2CF4
		private void ArrayAdd(DataTable table)
		{
			this._list.Add(table);
		}

		// Token: 0x06000A7F RID: 2687 RVA: 0x001F3910 File Offset: 0x001F2D10
		internal string AssignName()
		{
			string text;
			while (this.Contains(text = this.MakeName(this.defaultNameIndex)))
			{
				this.defaultNameIndex++;
			}
			return text;
		}

		// Token: 0x06000A80 RID: 2688 RVA: 0x001F3948 File Offset: 0x001F2D48
		private void BaseAdd(DataTable table)
		{
			if (table == null)
			{
				throw ExceptionBuilder.ArgumentNull("table");
			}
			if (table.DataSet == this.dataSet)
			{
				throw ExceptionBuilder.TableAlreadyInTheDataSet();
			}
			if (table.DataSet != null)
			{
				throw ExceptionBuilder.TableAlreadyInOtherDataSet();
			}
			if (table.TableName.Length == 0)
			{
				table.TableName = this.AssignName();
			}
			else
			{
				if (base.NamesEqual(table.TableName, this.dataSet.DataSetName, false, this.dataSet.Locale) != 0 && !table.fNestedInDataset)
				{
					throw ExceptionBuilder.DatasetConflictingName(this.dataSet.DataSetName);
				}
				this.RegisterName(table.TableName, table.Namespace);
			}
			table.SetDataSet(this.dataSet);
		}

		// Token: 0x06000A81 RID: 2689 RVA: 0x001F39FC File Offset: 0x001F2DFC
		private void BaseGroupSwitch(DataTable[] oldArray, int oldLength, DataTable[] newArray, int newLength)
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
				if (!flag && oldArray[i].DataSet == this.dataSet)
				{
					this.BaseRemove(oldArray[i]);
				}
			}
			for (int k = 0; k < newLength; k++)
			{
				if (newArray[k].DataSet != this.dataSet)
				{
					this.BaseAdd(newArray[k]);
					this._list.Add(newArray[k]);
				}
			}
		}

		// Token: 0x06000A82 RID: 2690 RVA: 0x001F3A90 File Offset: 0x001F2E90
		private void BaseRemove(DataTable table)
		{
			if (this.CanRemove(table, true))
			{
				this.UnregisterName(table.TableName);
				table.SetDataSet(null);
			}
			this._list.Remove(table);
			this.dataSet.OnRemovedTable(table);
		}

		// Token: 0x06000A83 RID: 2691 RVA: 0x001F3AD4 File Offset: 0x001F2ED4
		public bool CanRemove(DataTable table)
		{
			return this.CanRemove(table, false);
		}

		// Token: 0x06000A84 RID: 2692 RVA: 0x001F3AEC File Offset: 0x001F2EEC
		internal bool CanRemove(DataTable table, bool fThrowException)
		{
			IntPtr intPtr;
			Bid.ScopeEnter(out intPtr, "<ds.DataTableCollection.CanRemove|INFO> %d#, table=%d, fThrowException=%d{bool}\n", this.ObjectID, (table != null) ? table.ObjectID : 0, fThrowException);
			bool flag;
			try
			{
				if (table == null)
				{
					if (fThrowException)
					{
						throw ExceptionBuilder.ArgumentNull("table");
					}
					flag = false;
				}
				else if (table.DataSet != this.dataSet)
				{
					if (fThrowException)
					{
						throw ExceptionBuilder.TableNotInTheDataSet(table.TableName);
					}
					flag = false;
				}
				else
				{
					this.dataSet.OnRemoveTable(table);
					if (table.ChildRelations.Count != 0 || table.ParentRelations.Count != 0)
					{
						if (fThrowException)
						{
							throw ExceptionBuilder.TableInRelation();
						}
						flag = false;
					}
					else
					{
						ParentForeignKeyConstraintEnumerator parentForeignKeyConstraintEnumerator = new ParentForeignKeyConstraintEnumerator(this.dataSet, table);
						while (parentForeignKeyConstraintEnumerator.GetNext())
						{
							ForeignKeyConstraint foreignKeyConstraint = parentForeignKeyConstraintEnumerator.GetForeignKeyConstraint();
							if (foreignKeyConstraint.Table != table || foreignKeyConstraint.RelatedTable != table)
							{
								if (!fThrowException)
								{
									return false;
								}
								throw ExceptionBuilder.TableInConstraint(table, foreignKeyConstraint);
							}
						}
						ChildForeignKeyConstraintEnumerator childForeignKeyConstraintEnumerator = new ChildForeignKeyConstraintEnumerator(this.dataSet, table);
						while (childForeignKeyConstraintEnumerator.GetNext())
						{
							ForeignKeyConstraint foreignKeyConstraint2 = childForeignKeyConstraintEnumerator.GetForeignKeyConstraint();
							if (foreignKeyConstraint2.Table != table || foreignKeyConstraint2.RelatedTable != table)
							{
								if (!fThrowException)
								{
									return false;
								}
								throw ExceptionBuilder.TableInConstraint(table, foreignKeyConstraint2);
							}
						}
						flag = true;
					}
				}
			}
			finally
			{
				Bid.ScopeLeave(ref intPtr);
			}
			return flag;
		}

		// Token: 0x06000A85 RID: 2693 RVA: 0x001F3C34 File Offset: 0x001F3034
		public void Clear()
		{
			IntPtr intPtr;
			Bid.ScopeEnter(out intPtr, "<ds.DataTableCollection.Clear|API> %d#\n", this.ObjectID);
			try
			{
				int count = this._list.Count;
				DataTable[] array = new DataTable[this._list.Count];
				this._list.CopyTo(array, 0);
				this.OnCollectionChanging(InternalDataCollectionBase.RefreshEventArgs);
				if (this.dataSet.fInitInProgress && this.delayedAddRangeTables != null)
				{
					this.delayedAddRangeTables = null;
				}
				this.BaseGroupSwitch(array, count, null, 0);
				this._list.Clear();
				this.OnCollectionChanged(InternalDataCollectionBase.RefreshEventArgs);
			}
			finally
			{
				Bid.ScopeLeave(ref intPtr);
			}
		}

		// Token: 0x06000A86 RID: 2694 RVA: 0x001F3CEC File Offset: 0x001F30EC
		public bool Contains(string name)
		{
			return this.InternalIndexOf(name) >= 0;
		}

		// Token: 0x06000A87 RID: 2695 RVA: 0x001F3D08 File Offset: 0x001F3108
		public bool Contains(string name, string tableNamespace)
		{
			if (name == null)
			{
				throw ExceptionBuilder.ArgumentNull("name");
			}
			if (tableNamespace == null)
			{
				throw ExceptionBuilder.ArgumentNull("tableNamespace");
			}
			return this.InternalIndexOf(name, tableNamespace) >= 0;
		}

		// Token: 0x06000A88 RID: 2696 RVA: 0x001F3D40 File Offset: 0x001F3140
		internal bool Contains(string name, string tableNamespace, bool checkProperty, bool caseSensitive)
		{
			if (!caseSensitive)
			{
				return this.InternalIndexOf(name) >= 0;
			}
			int count = this._list.Count;
			for (int i = 0; i < count; i++)
			{
				DataTable dataTable = (DataTable)this._list[i];
				string text = (checkProperty ? dataTable.Namespace : dataTable.tableNamespace);
				if (base.NamesEqual(dataTable.TableName, name, true, this.dataSet.Locale) == 1 && text == tableNamespace)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000A89 RID: 2697 RVA: 0x001F3DC4 File Offset: 0x001F31C4
		internal bool Contains(string name, bool caseSensitive)
		{
			if (!caseSensitive)
			{
				return this.InternalIndexOf(name) >= 0;
			}
			int count = this._list.Count;
			for (int i = 0; i < count; i++)
			{
				DataTable dataTable = (DataTable)this._list[i];
				if (base.NamesEqual(dataTable.TableName, name, true, this.dataSet.Locale) == 1)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000A8A RID: 2698 RVA: 0x001F3E2C File Offset: 0x001F322C
		public void CopyTo(DataTable[] array, int index)
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
				array[index + i] = (DataTable)this._list[i];
			}
		}

		// Token: 0x06000A8B RID: 2699 RVA: 0x001F3E9C File Offset: 0x001F329C
		public int IndexOf(DataTable table)
		{
			int count = this._list.Count;
			for (int i = 0; i < count; i++)
			{
				if (table == (DataTable)this._list[i])
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x06000A8C RID: 2700 RVA: 0x001F3ED8 File Offset: 0x001F32D8
		public int IndexOf(string tableName)
		{
			int num = this.InternalIndexOf(tableName);
			if (num >= 0)
			{
				return num;
			}
			return -1;
		}

		// Token: 0x06000A8D RID: 2701 RVA: 0x001F3EF4 File Offset: 0x001F32F4
		public int IndexOf(string tableName, string tableNamespace)
		{
			return this.IndexOf(tableName, tableNamespace, true);
		}

		// Token: 0x06000A8E RID: 2702 RVA: 0x001F3F0C File Offset: 0x001F330C
		internal int IndexOf(string tableName, string tableNamespace, bool chekforNull)
		{
			if (chekforNull)
			{
				if (tableName == null)
				{
					throw ExceptionBuilder.ArgumentNull("tableName");
				}
				if (tableNamespace == null)
				{
					throw ExceptionBuilder.ArgumentNull("tableNamespace");
				}
			}
			int num = this.InternalIndexOf(tableName, tableNamespace);
			if (num >= 0)
			{
				return num;
			}
			return -1;
		}

		// Token: 0x06000A8F RID: 2703 RVA: 0x001F3F48 File Offset: 0x001F3348
		internal void ReplaceFromInference(List<DataTable> tableList)
		{
			this._list.Clear();
			this._list.AddRange(tableList);
		}

		// Token: 0x06000A90 RID: 2704 RVA: 0x001F3F6C File Offset: 0x001F336C
		internal int InternalIndexOf(string tableName)
		{
			int num = -1;
			if (tableName != null && 0 < tableName.Length)
			{
				int count = this._list.Count;
				for (int i = 0; i < count; i++)
				{
					DataTable dataTable = (DataTable)this._list[i];
					int num2 = base.NamesEqual(dataTable.TableName, tableName, false, this.dataSet.Locale);
					if (num2 == 1)
					{
						for (int j = i + 1; j < count; j++)
						{
							DataTable dataTable2 = (DataTable)this._list[j];
							if (base.NamesEqual(dataTable2.TableName, tableName, false, this.dataSet.Locale) == 1)
							{
								return -3;
							}
						}
						return i;
					}
					if (num2 == -1)
					{
						num = ((num == -1) ? i : (-2));
					}
				}
			}
			return num;
		}

		// Token: 0x06000A91 RID: 2705 RVA: 0x001F4034 File Offset: 0x001F3434
		internal int InternalIndexOf(string tableName, string tableNamespace)
		{
			int num = -1;
			if (tableName != null && 0 < tableName.Length)
			{
				int count = this._list.Count;
				for (int i = 0; i < count; i++)
				{
					DataTable dataTable = (DataTable)this._list[i];
					int num2 = base.NamesEqual(dataTable.TableName, tableName, false, this.dataSet.Locale);
					if (num2 == 1 && dataTable.Namespace == tableNamespace)
					{
						return i;
					}
					if (num2 == -1 && dataTable.Namespace == tableNamespace)
					{
						num = ((num == -1) ? i : (-2));
					}
				}
			}
			return num;
		}

		// Token: 0x06000A92 RID: 2706 RVA: 0x001F40D0 File Offset: 0x001F34D0
		internal void FinishInitCollection()
		{
			if (this.delayedAddRangeTables != null)
			{
				foreach (DataTable dataTable in this.delayedAddRangeTables)
				{
					if (dataTable != null)
					{
						this.Add(dataTable);
					}
				}
				this.delayedAddRangeTables = null;
			}
		}

		// Token: 0x06000A93 RID: 2707 RVA: 0x001F4110 File Offset: 0x001F3510
		private string MakeName(int index)
		{
			if (1 == index)
			{
				return "Table1";
			}
			return "Table" + index.ToString(CultureInfo.InvariantCulture);
		}

		// Token: 0x06000A94 RID: 2708 RVA: 0x001F4140 File Offset: 0x001F3540
		private void OnCollectionChanged(CollectionChangeEventArgs ccevent)
		{
			if (this.onCollectionChangedDelegate != null)
			{
				Bid.Trace("<ds.DataTableCollection.OnCollectionChanged|INFO> %d#\n", this.ObjectID);
				this.onCollectionChangedDelegate(this, ccevent);
			}
		}

		// Token: 0x06000A95 RID: 2709 RVA: 0x001F4174 File Offset: 0x001F3574
		private void OnCollectionChanging(CollectionChangeEventArgs ccevent)
		{
			if (this.onCollectionChangingDelegate != null)
			{
				Bid.Trace("<ds.DataTableCollection.OnCollectionChanging|INFO> %d#\n", this.ObjectID);
				this.onCollectionChangingDelegate(this, ccevent);
			}
		}

		// Token: 0x06000A96 RID: 2710 RVA: 0x001F41A8 File Offset: 0x001F35A8
		internal void RegisterName(string name, string tbNamespace)
		{
			Bid.Trace("<ds.DataTableCollection.RegisterName|INFO> %d#, name='%ls', tbNamespace='%ls'\n", this.ObjectID, name, tbNamespace);
			CultureInfo locale = this.dataSet.Locale;
			int count = this._list.Count;
			for (int i = 0; i < count; i++)
			{
				DataTable dataTable = (DataTable)this._list[i];
				if (base.NamesEqual(name, dataTable.TableName, true, locale) != 0 && tbNamespace == dataTable.Namespace)
				{
					throw ExceptionBuilder.DuplicateTableName(((DataTable)this._list[i]).TableName);
				}
			}
			if (base.NamesEqual(name, this.MakeName(this.defaultNameIndex), true, locale) != 0)
			{
				this.defaultNameIndex++;
			}
		}

		// Token: 0x06000A97 RID: 2711 RVA: 0x001F4260 File Offset: 0x001F3660
		public void Remove(DataTable table)
		{
			IntPtr intPtr;
			Bid.ScopeEnter(out intPtr, "<ds.DataTableCollection.Remove|API> %d#, table=%d\n", this.ObjectID, (table != null) ? table.ObjectID : 0);
			try
			{
				this.OnCollectionChanging(new CollectionChangeEventArgs(CollectionChangeAction.Remove, table));
				this.BaseRemove(table);
				this.OnCollectionChanged(new CollectionChangeEventArgs(CollectionChangeAction.Remove, table));
			}
			finally
			{
				Bid.ScopeLeave(ref intPtr);
			}
		}

		// Token: 0x06000A98 RID: 2712 RVA: 0x001F42D4 File Offset: 0x001F36D4
		public void RemoveAt(int index)
		{
			IntPtr intPtr;
			Bid.ScopeEnter(out intPtr, "<ds.DataTableCollection.RemoveAt|API> %d#, index=%d\n", this.ObjectID, index);
			try
			{
				DataTable dataTable = this[index];
				if (dataTable == null)
				{
					throw ExceptionBuilder.TableOutOfRange(index);
				}
				this.Remove(dataTable);
			}
			finally
			{
				Bid.ScopeLeave(ref intPtr);
			}
		}

		// Token: 0x06000A99 RID: 2713 RVA: 0x001F4334 File Offset: 0x001F3734
		public void Remove(string name)
		{
			IntPtr intPtr;
			Bid.ScopeEnter(out intPtr, "<ds.DataTableCollection.Remove|API> %d#, name='%ls'\n", this.ObjectID, name);
			try
			{
				DataTable dataTable = this[name];
				if (dataTable == null)
				{
					throw ExceptionBuilder.TableNotInTheDataSet(name);
				}
				this.Remove(dataTable);
			}
			finally
			{
				Bid.ScopeLeave(ref intPtr);
			}
		}

		// Token: 0x06000A9A RID: 2714 RVA: 0x001F4394 File Offset: 0x001F3794
		public void Remove(string name, string tableNamespace)
		{
			if (name == null)
			{
				throw ExceptionBuilder.ArgumentNull("name");
			}
			if (tableNamespace == null)
			{
				throw ExceptionBuilder.ArgumentNull("tableNamespace");
			}
			DataTable dataTable = this[name, tableNamespace];
			if (dataTable == null)
			{
				throw ExceptionBuilder.TableNotInTheDataSet(name);
			}
			this.Remove(dataTable);
		}

		// Token: 0x06000A9B RID: 2715 RVA: 0x001F43D8 File Offset: 0x001F37D8
		internal void UnregisterName(string name)
		{
			Bid.Trace("<ds.DataTableCollection.UnregisterName|INFO> %d#, name='%ls'\n", this.ObjectID, name);
			if (base.NamesEqual(name, this.MakeName(this.defaultNameIndex - 1), true, this.dataSet.Locale) != 0)
			{
				do
				{
					this.defaultNameIndex--;
				}
				while (this.defaultNameIndex > 1 && !this.Contains(this.MakeName(this.defaultNameIndex - 1)));
			}
		}

		// Token: 0x0400080E RID: 2062
		private readonly DataSet dataSet;

		// Token: 0x0400080F RID: 2063
		private readonly ArrayList _list = new ArrayList();

		// Token: 0x04000810 RID: 2064
		private int defaultNameIndex = 1;

		// Token: 0x04000811 RID: 2065
		private DataTable[] delayedAddRangeTables;

		// Token: 0x04000812 RID: 2066
		private CollectionChangeEventHandler onCollectionChangedDelegate;

		// Token: 0x04000813 RID: 2067
		private CollectionChangeEventHandler onCollectionChangingDelegate;

		// Token: 0x04000814 RID: 2068
		private static int _objectTypeCount;

		// Token: 0x04000815 RID: 2069
		private readonly int _objectID = Interlocked.Increment(ref DataTableCollection._objectTypeCount);
	}
}
