using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;

namespace System.Data
{
	// Token: 0x020000C8 RID: 200
	internal sealed class Merger
	{
		// Token: 0x06000CC8 RID: 3272 RVA: 0x001FAFC0 File Offset: 0x001FA3C0
		internal Merger(DataSet dataSet, bool preserveChanges, MissingSchemaAction missingSchemaAction)
		{
			this.dataSet = dataSet;
			this.preserveChanges = preserveChanges;
			if (missingSchemaAction == MissingSchemaAction.AddWithKey)
			{
				this.missingSchemaAction = MissingSchemaAction.Add;
				return;
			}
			this.missingSchemaAction = missingSchemaAction;
		}

		// Token: 0x06000CC9 RID: 3273 RVA: 0x001FAFF4 File Offset: 0x001FA3F4
		internal Merger(DataTable dataTable, bool preserveChanges, MissingSchemaAction missingSchemaAction)
		{
			this.isStandAlonetable = true;
			this.dataTable = dataTable;
			this.preserveChanges = preserveChanges;
			if (missingSchemaAction == MissingSchemaAction.AddWithKey)
			{
				this.missingSchemaAction = MissingSchemaAction.Add;
				return;
			}
			this.missingSchemaAction = missingSchemaAction;
		}

		// Token: 0x06000CCA RID: 3274 RVA: 0x001FB030 File Offset: 0x001FA430
		internal void MergeDataSet(DataSet source)
		{
			if (source == this.dataSet)
			{
				return;
			}
			bool enforceConstraints = this.dataSet.EnforceConstraints;
			this.dataSet.EnforceConstraints = false;
			this._IgnoreNSforTableLookup = this.dataSet.namespaceURI != source.namespaceURI;
			List<DataColumn> list = null;
			if (MissingSchemaAction.Add == this.missingSchemaAction)
			{
				list = new List<DataColumn>();
				foreach (object obj in this.dataSet.Tables)
				{
					DataTable dataTable = (DataTable)obj;
					foreach (object obj2 in dataTable.Columns)
					{
						DataColumn dataColumn = (DataColumn)obj2;
						list.Add(dataColumn);
					}
				}
			}
			for (int i = 0; i < source.Tables.Count; i++)
			{
				this.MergeTableData(source.Tables[i]);
			}
			if (MissingSchemaAction.Ignore != this.missingSchemaAction)
			{
				this.MergeConstraints(source);
				for (int j = 0; j < source.Relations.Count; j++)
				{
					this.MergeRelation(source.Relations[j]);
				}
			}
			if (MissingSchemaAction.Add == this.missingSchemaAction)
			{
				foreach (object obj3 in source.Tables)
				{
					DataTable dataTable2 = (DataTable)obj3;
					DataTable dataTable3;
					if (this._IgnoreNSforTableLookup)
					{
						dataTable3 = this.dataSet.Tables[dataTable2.TableName];
					}
					else
					{
						dataTable3 = this.dataSet.Tables[dataTable2.TableName, dataTable2.Namespace];
					}
					foreach (object obj4 in dataTable2.Columns)
					{
						DataColumn dataColumn2 = (DataColumn)obj4;
						if (dataColumn2.Computed)
						{
							DataColumn dataColumn3 = dataTable3.Columns[dataColumn2.ColumnName];
							if (!list.Contains(dataColumn3))
							{
								dataColumn3.Expression = dataColumn2.Expression;
							}
						}
					}
				}
			}
			this.MergeExtendedProperties(source.ExtendedProperties, this.dataSet.ExtendedProperties);
			foreach (object obj5 in this.dataSet.Tables)
			{
				DataTable dataTable4 = (DataTable)obj5;
				dataTable4.EvaluateExpressions();
			}
			this.dataSet.EnforceConstraints = enforceConstraints;
		}

		// Token: 0x06000CCB RID: 3275 RVA: 0x001FB364 File Offset: 0x001FA764
		internal void MergeTable(DataTable src)
		{
			bool flag = false;
			if (!this.isStandAlonetable)
			{
				if (src.DataSet == this.dataSet)
				{
					return;
				}
				flag = this.dataSet.EnforceConstraints;
				this.dataSet.EnforceConstraints = false;
			}
			else
			{
				if (src == this.dataTable)
				{
					return;
				}
				this.dataTable.SuspendEnforceConstraints = true;
			}
			if (this.dataSet != null)
			{
				if (src.DataSet == null || src.DataSet.namespaceURI != this.dataSet.namespaceURI)
				{
					this._IgnoreNSforTableLookup = true;
				}
			}
			else if (this.dataTable.DataSet == null || src.DataSet == null || src.DataSet.namespaceURI != this.dataTable.DataSet.namespaceURI)
			{
				this._IgnoreNSforTableLookup = true;
			}
			this.MergeTableData(src);
			DataTable dataTable = this.dataTable;
			if (dataTable == null && this.dataSet != null)
			{
				if (this._IgnoreNSforTableLookup)
				{
					dataTable = this.dataSet.Tables[src.TableName];
				}
				else
				{
					dataTable = this.dataSet.Tables[src.TableName, src.Namespace];
				}
			}
			if (dataTable != null)
			{
				dataTable.EvaluateExpressions();
			}
			if (!this.isStandAlonetable)
			{
				this.dataSet.EnforceConstraints = flag;
				return;
			}
			this.dataTable.SuspendEnforceConstraints = false;
			try
			{
				if (this.dataTable.EnforceConstraints)
				{
					this.dataTable.EnableConstraints();
				}
			}
			catch (ConstraintException)
			{
				if (this.dataTable.DataSet != null)
				{
					this.dataTable.DataSet.EnforceConstraints = false;
				}
				throw;
			}
		}

		// Token: 0x06000CCC RID: 3276 RVA: 0x001FB508 File Offset: 0x001FA908
		private void MergeTable(DataTable src, DataTable dst)
		{
			int count = src.Rows.Count;
			bool flag = dst.Rows.Count == 0;
			if (0 < count)
			{
				Index index = null;
				DataKey dataKey = default(DataKey);
				dst.SuspendIndexEvents();
				try
				{
					if (!flag && dst.primaryKey != null)
					{
						dataKey = this.GetSrcKey(src, dst);
						if (dataKey.HasValue)
						{
							index = dst.primaryKey.Key.GetSortIndex(DataViewRowState.Unchanged | DataViewRowState.Added | DataViewRowState.Deleted | DataViewRowState.ModifiedOriginal);
						}
					}
					foreach (object obj in src.Rows)
					{
						DataRow dataRow = (DataRow)obj;
						DataRow dataRow2 = null;
						if (index != null)
						{
							dataRow2 = dst.FindMergeTarget(dataRow, dataKey, index);
						}
						dst.MergeRow(dataRow, dataRow2, this.preserveChanges, index);
					}
				}
				finally
				{
					dst.RestoreIndexEvents(true);
				}
			}
			this.MergeExtendedProperties(src.ExtendedProperties, dst.ExtendedProperties);
		}

		// Token: 0x06000CCD RID: 3277 RVA: 0x001FB628 File Offset: 0x001FAA28
		internal void MergeRows(DataRow[] rows)
		{
			DataTable dataTable = null;
			DataTable dataTable2 = null;
			DataKey dataKey = default(DataKey);
			Index index = null;
			bool enforceConstraints = this.dataSet.EnforceConstraints;
			this.dataSet.EnforceConstraints = false;
			for (int i = 0; i < rows.Length; i++)
			{
				DataRow dataRow = rows[i];
				if (dataRow == null)
				{
					throw ExceptionBuilder.ArgumentNull("rows[" + i + "]");
				}
				if (dataRow.Table == null)
				{
					throw ExceptionBuilder.ArgumentNull("rows[" + i + "].Table");
				}
				if (dataRow.Table.DataSet != this.dataSet)
				{
					if (dataTable != dataRow.Table)
					{
						dataTable = dataRow.Table;
						dataTable2 = this.MergeSchema(dataRow.Table);
						if (dataTable2 == null)
						{
							this.dataSet.EnforceConstraints = enforceConstraints;
							return;
						}
						if (dataTable2.primaryKey != null)
						{
							dataKey = this.GetSrcKey(dataTable, dataTable2);
						}
						if (dataKey.HasValue)
						{
							if (index != null)
							{
								index.RemoveRef();
							}
							index = new Index(dataTable2, dataTable2.primaryKey.Key.GetIndexDesc(), DataViewRowState.Unchanged | DataViewRowState.Added | DataViewRowState.Deleted | DataViewRowState.ModifiedOriginal, null);
							index.AddRef();
							index.AddRef();
						}
					}
					if (dataRow.newRecord != -1 || dataRow.oldRecord != -1)
					{
						DataRow dataRow2 = null;
						if (0 < dataTable2.Rows.Count && index != null)
						{
							dataRow2 = dataTable2.FindMergeTarget(dataRow, dataKey, index);
						}
						dataRow2 = dataTable2.MergeRow(dataRow, dataRow2, this.preserveChanges, index);
						if (dataRow2.Table.dependentColumns != null && dataRow2.Table.dependentColumns.Count > 0)
						{
							dataRow2.Table.EvaluateExpressions(dataRow2, DataRowAction.Change, null);
						}
					}
				}
			}
			if (index != null)
			{
				index.RemoveRef();
			}
			this.dataSet.EnforceConstraints = enforceConstraints;
		}

		// Token: 0x06000CCE RID: 3278 RVA: 0x001FB7E0 File Offset: 0x001FABE0
		private DataTable MergeSchema(DataTable table)
		{
			DataTable dataTable = null;
			if (!this.isStandAlonetable)
			{
				if (this.dataSet.Tables.Contains(table.TableName, true))
				{
					if (this._IgnoreNSforTableLookup)
					{
						dataTable = this.dataSet.Tables[table.TableName];
					}
					else
					{
						dataTable = this.dataSet.Tables[table.TableName, table.Namespace];
					}
				}
			}
			else
			{
				dataTable = this.dataTable;
			}
			if (dataTable == null)
			{
				if (MissingSchemaAction.Add == this.missingSchemaAction)
				{
					dataTable = table.Clone(table.DataSet);
					this.dataSet.Tables.Add(dataTable);
				}
				else if (MissingSchemaAction.Error == this.missingSchemaAction)
				{
					throw ExceptionBuilder.MergeMissingDefinition(table.TableName);
				}
			}
			else
			{
				if (MissingSchemaAction.Ignore != this.missingSchemaAction)
				{
					int count = dataTable.Columns.Count;
					for (int i = 0; i < table.Columns.Count; i++)
					{
						DataColumn dataColumn = table.Columns[i];
						DataColumn dataColumn2 = (dataTable.Columns.Contains(dataColumn.ColumnName, true) ? dataTable.Columns[dataColumn.ColumnName] : null);
						if (dataColumn2 == null)
						{
							if (MissingSchemaAction.Add == this.missingSchemaAction)
							{
								dataColumn2 = dataColumn.Clone();
								dataTable.Columns.Add(dataColumn2);
							}
							else
							{
								if (this.isStandAlonetable)
								{
									throw ExceptionBuilder.MergeFailed(Res.GetString("DataMerge_MissingColumnDefinition", new object[] { table.TableName, dataColumn.ColumnName }));
								}
								this.dataSet.RaiseMergeFailed(dataTable, Res.GetString("DataMerge_MissingColumnDefinition", new object[] { table.TableName, dataColumn.ColumnName }), this.missingSchemaAction);
							}
						}
						else
						{
							if (dataColumn2.DataType != dataColumn.DataType || (dataColumn2.DataType == typeof(DateTime) && dataColumn2.DateTimeMode != dataColumn.DateTimeMode && (dataColumn2.DateTimeMode & dataColumn.DateTimeMode) != DataSetDateTime.Unspecified))
							{
								if (this.isStandAlonetable)
								{
									throw ExceptionBuilder.MergeFailed(Res.GetString("DataMerge_DataTypeMismatch", new object[] { dataColumn.ColumnName }));
								}
								this.dataSet.RaiseMergeFailed(dataTable, Res.GetString("DataMerge_DataTypeMismatch", new object[] { dataColumn.ColumnName }), MissingSchemaAction.Error);
							}
							this.MergeExtendedProperties(dataColumn.ExtendedProperties, dataColumn2.ExtendedProperties);
						}
					}
					if (this.isStandAlonetable)
					{
						for (int j = count; j < dataTable.Columns.Count; j++)
						{
							dataTable.Columns[j].Expression = table.Columns[dataTable.Columns[j].ColumnName].Expression;
						}
					}
					DataColumn[] primaryKey = dataTable.PrimaryKey;
					DataColumn[] primaryKey2 = table.PrimaryKey;
					if (primaryKey.Length != primaryKey2.Length)
					{
						if (primaryKey.Length == 0)
						{
							DataColumn[] array = new DataColumn[primaryKey2.Length];
							for (int k = 0; k < primaryKey2.Length; k++)
							{
								array[k] = dataTable.Columns[primaryKey2[k].ColumnName];
							}
							dataTable.PrimaryKey = array;
						}
						else if (primaryKey2.Length != 0)
						{
							this.dataSet.RaiseMergeFailed(dataTable, Res.GetString("DataMerge_PrimaryKeyMismatch"), this.missingSchemaAction);
						}
					}
					else
					{
						for (int l = 0; l < primaryKey.Length; l++)
						{
							if (string.Compare(primaryKey[l].ColumnName, primaryKey2[l].ColumnName, false, dataTable.Locale) != 0)
							{
								this.dataSet.RaiseMergeFailed(table, Res.GetString("DataMerge_PrimaryKeyColumnsMismatch", new object[]
								{
									primaryKey[l].ColumnName,
									primaryKey2[l].ColumnName
								}), this.missingSchemaAction);
							}
						}
					}
				}
				this.MergeExtendedProperties(table.ExtendedProperties, dataTable.ExtendedProperties);
			}
			return dataTable;
		}

		// Token: 0x06000CCF RID: 3279 RVA: 0x001FBBBC File Offset: 0x001FAFBC
		private void MergeTableData(DataTable src)
		{
			DataTable dataTable = this.MergeSchema(src);
			if (dataTable == null)
			{
				return;
			}
			dataTable.MergingData = true;
			try
			{
				this.MergeTable(src, dataTable);
			}
			finally
			{
				dataTable.MergingData = false;
			}
		}

		// Token: 0x06000CD0 RID: 3280 RVA: 0x001FBC0C File Offset: 0x001FB00C
		private void MergeConstraints(DataSet source)
		{
			for (int i = 0; i < source.Tables.Count; i++)
			{
				this.MergeConstraints(source.Tables[i]);
			}
		}

		// Token: 0x06000CD1 RID: 3281 RVA: 0x001FBC44 File Offset: 0x001FB044
		private void MergeConstraints(DataTable table)
		{
			for (int i = 0; i < table.Constraints.Count; i++)
			{
				Constraint constraint = table.Constraints[i];
				Constraint constraint2 = constraint.Clone(this.dataSet, this._IgnoreNSforTableLookup);
				if (constraint2 == null)
				{
					this.dataSet.RaiseMergeFailed(table, Res.GetString("DataMerge_MissingConstraint", new object[]
					{
						constraint.GetType().FullName,
						constraint.ConstraintName
					}), this.missingSchemaAction);
				}
				else
				{
					Constraint constraint3 = constraint2.Table.Constraints.FindConstraint(constraint2);
					if (constraint3 == null)
					{
						if (MissingSchemaAction.Add == this.missingSchemaAction)
						{
							try
							{
								constraint2.Table.Constraints.Add(constraint2);
								goto IL_011F;
							}
							catch (DuplicateNameException)
							{
								constraint2.ConstraintName = "";
								constraint2.Table.Constraints.Add(constraint2);
								goto IL_011F;
							}
						}
						if (MissingSchemaAction.Error == this.missingSchemaAction)
						{
							this.dataSet.RaiseMergeFailed(table, Res.GetString("DataMerge_MissingConstraint", new object[]
							{
								constraint.GetType().FullName,
								constraint.ConstraintName
							}), this.missingSchemaAction);
						}
					}
					else
					{
						this.MergeExtendedProperties(constraint.ExtendedProperties, constraint3.ExtendedProperties);
					}
				}
				IL_011F:;
			}
		}

		// Token: 0x06000CD2 RID: 3282 RVA: 0x001FBDA4 File Offset: 0x001FB1A4
		private void MergeRelation(DataRelation relation)
		{
			DataRelation dataRelation = null;
			int num = this.dataSet.Relations.InternalIndexOf(relation.RelationName);
			if (num < 0)
			{
				if (MissingSchemaAction.Add == this.missingSchemaAction)
				{
					DataTable dataTable;
					if (this._IgnoreNSforTableLookup)
					{
						dataTable = this.dataSet.Tables[relation.ParentTable.TableName];
					}
					else
					{
						dataTable = this.dataSet.Tables[relation.ParentTable.TableName, relation.ParentTable.Namespace];
					}
					DataTable dataTable2;
					if (this._IgnoreNSforTableLookup)
					{
						dataTable2 = this.dataSet.Tables[relation.ChildTable.TableName];
					}
					else
					{
						dataTable2 = this.dataSet.Tables[relation.ChildTable.TableName, relation.ChildTable.Namespace];
					}
					DataColumn[] array = new DataColumn[relation.ParentKey.ColumnsReference.Length];
					DataColumn[] array2 = new DataColumn[relation.ParentKey.ColumnsReference.Length];
					for (int i = 0; i < relation.ParentKey.ColumnsReference.Length; i++)
					{
						array[i] = dataTable.Columns[relation.ParentKey.ColumnsReference[i].ColumnName];
						array2[i] = dataTable2.Columns[relation.ChildKey.ColumnsReference[i].ColumnName];
					}
					try
					{
						dataRelation = new DataRelation(relation.RelationName, array, array2, relation.createConstraints);
						dataRelation.Nested = relation.Nested;
						this.dataSet.Relations.Add(dataRelation);
						goto IL_034C;
					}
					catch (Exception ex)
					{
						if (!ADP.IsCatchableExceptionType(ex))
						{
							throw;
						}
						ExceptionBuilder.TraceExceptionForCapture(ex);
						this.dataSet.RaiseMergeFailed(null, ex.Message, this.missingSchemaAction);
						goto IL_034C;
					}
				}
				throw ExceptionBuilder.MergeMissingDefinition(relation.RelationName);
			}
			dataRelation = this.dataSet.Relations[num];
			if (relation.ParentKey.ColumnsReference.Length != dataRelation.ParentKey.ColumnsReference.Length)
			{
				this.dataSet.RaiseMergeFailed(null, Res.GetString("DataMerge_MissingDefinition", new object[] { relation.RelationName }), this.missingSchemaAction);
			}
			for (int j = 0; j < relation.ParentKey.ColumnsReference.Length; j++)
			{
				DataColumn dataColumn = dataRelation.ParentKey.ColumnsReference[j];
				DataColumn dataColumn2 = relation.ParentKey.ColumnsReference[j];
				if (string.Compare(dataColumn.ColumnName, dataColumn2.ColumnName, false, dataColumn.Table.Locale) != 0)
				{
					this.dataSet.RaiseMergeFailed(null, Res.GetString("DataMerge_ReltionKeyColumnsMismatch", new object[] { relation.RelationName }), this.missingSchemaAction);
				}
				dataColumn = dataRelation.ChildKey.ColumnsReference[j];
				dataColumn2 = relation.ChildKey.ColumnsReference[j];
				if (string.Compare(dataColumn.ColumnName, dataColumn2.ColumnName, false, dataColumn.Table.Locale) != 0)
				{
					this.dataSet.RaiseMergeFailed(null, Res.GetString("DataMerge_ReltionKeyColumnsMismatch", new object[] { relation.RelationName }), this.missingSchemaAction);
				}
			}
			IL_034C:
			this.MergeExtendedProperties(relation.ExtendedProperties, dataRelation.ExtendedProperties);
		}

		// Token: 0x06000CD3 RID: 3283 RVA: 0x001FC12C File Offset: 0x001FB52C
		private void MergeExtendedProperties(PropertyCollection src, PropertyCollection dst)
		{
			if (MissingSchemaAction.Ignore == this.missingSchemaAction)
			{
				return;
			}
			IDictionaryEnumerator enumerator = src.GetEnumerator();
			while (enumerator.MoveNext())
			{
				if (!this.preserveChanges || dst[enumerator.Key] == null)
				{
					dst[enumerator.Key] = enumerator.Value;
				}
			}
		}

		// Token: 0x06000CD4 RID: 3284 RVA: 0x001FC17C File Offset: 0x001FB57C
		private DataKey GetSrcKey(DataTable src, DataTable dst)
		{
			if (src.primaryKey != null)
			{
				return src.primaryKey.Key;
			}
			DataKey dataKey = default(DataKey);
			if (dst.primaryKey != null)
			{
				DataColumn[] columnsReference = dst.primaryKey.Key.ColumnsReference;
				DataColumn[] array = new DataColumn[columnsReference.Length];
				for (int i = 0; i < columnsReference.Length; i++)
				{
					array[i] = src.Columns[columnsReference[i].ColumnName];
				}
				dataKey = new DataKey(array, false);
			}
			return dataKey;
		}

		// Token: 0x040008AD RID: 2221
		private DataSet dataSet;

		// Token: 0x040008AE RID: 2222
		private DataTable dataTable;

		// Token: 0x040008AF RID: 2223
		private bool preserveChanges;

		// Token: 0x040008B0 RID: 2224
		private MissingSchemaAction missingSchemaAction;

		// Token: 0x040008B1 RID: 2225
		private bool isStandAlonetable;

		// Token: 0x040008B2 RID: 2226
		private bool _IgnoreNSforTableLookup;
	}
}
