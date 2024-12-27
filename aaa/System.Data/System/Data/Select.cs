using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Globalization;

namespace System.Data
{
	// Token: 0x020000D9 RID: 217
	internal sealed class Select
	{
		// Token: 0x06000D08 RID: 3336 RVA: 0x001FCE04 File Offset: 0x001FC204
		public Select(DataTable table, string filterExpression, string sort, DataViewRowState recordStates)
		{
			this.table = table;
			this.IndexFields = table.ParseSortString(sort);
			this.indexDesc = Select.ConvertIndexFieldtoIndexDesc(this.IndexFields);
			if (filterExpression != null && filterExpression.Length > 0)
			{
				this.rowFilter = new DataExpression(this.table, filterExpression);
				this.expression = this.rowFilter.ExpressionNode;
			}
			this.recordStates = recordStates;
		}

		// Token: 0x06000D09 RID: 3337 RVA: 0x001FCE74 File Offset: 0x001FC274
		private bool IsSupportedOperator(int op)
		{
			return (op >= 7 && op <= 11) || op == 13 || op == 39;
		}

		// Token: 0x06000D0A RID: 3338 RVA: 0x001FCE98 File Offset: 0x001FC298
		private void AnalyzeExpression(BinaryNode expr)
		{
			if (this.linearExpression == this.expression)
			{
				return;
			}
			if (expr.op == 27)
			{
				this.linearExpression = this.expression;
				return;
			}
			if (expr.op != 26)
			{
				if (this.IsSupportedOperator(expr.op))
				{
					if (expr.left is NameNode && expr.right is ConstNode)
					{
						Select.ColumnInfo columnInfo = this.candidateColumns[((NameNode)expr.left).column.Ordinal];
						columnInfo.expr = ((columnInfo.expr == null) ? expr : new BinaryNode(this.table, 26, expr, columnInfo.expr));
						if (expr.op == 7)
						{
							columnInfo.equalsOperator = true;
						}
						this.candidatesForBinarySearch = true;
						return;
					}
					if (expr.right is NameNode && expr.left is ConstNode)
					{
						ExpressionNode left = expr.left;
						expr.left = expr.right;
						expr.right = left;
						switch (expr.op)
						{
						case 8:
							expr.op = 9;
							break;
						case 9:
							expr.op = 8;
							break;
						case 10:
							expr.op = 11;
							break;
						case 11:
							expr.op = 10;
							break;
						}
						Select.ColumnInfo columnInfo2 = this.candidateColumns[((NameNode)expr.left).column.Ordinal];
						columnInfo2.expr = ((columnInfo2.expr == null) ? expr : new BinaryNode(this.table, 26, expr, columnInfo2.expr));
						if (expr.op == 7)
						{
							columnInfo2.equalsOperator = true;
						}
						this.candidatesForBinarySearch = true;
						return;
					}
				}
				this.linearExpression = ((this.linearExpression == null) ? expr : new BinaryNode(this.table, 26, expr, this.linearExpression));
				return;
			}
			bool flag = false;
			bool flag2 = false;
			if (expr.left is BinaryNode)
			{
				this.AnalyzeExpression((BinaryNode)expr.left);
				if (this.linearExpression == this.expression)
				{
					return;
				}
				flag = true;
			}
			else
			{
				UnaryNode unaryNode = expr.left as UnaryNode;
				if (unaryNode != null)
				{
					while (unaryNode.op == 0 && unaryNode.right is UnaryNode && ((UnaryNode)unaryNode.right).op == 0)
					{
						unaryNode = (UnaryNode)unaryNode.right;
					}
					if (unaryNode.op == 0 && unaryNode.right is BinaryNode)
					{
						this.AnalyzeExpression((BinaryNode)unaryNode.right);
						if (this.linearExpression == this.expression)
						{
							return;
						}
						flag = true;
					}
				}
			}
			if (expr.right is BinaryNode)
			{
				this.AnalyzeExpression((BinaryNode)expr.right);
				if (this.linearExpression == this.expression)
				{
					return;
				}
				flag2 = true;
			}
			else
			{
				UnaryNode unaryNode2 = expr.right as UnaryNode;
				if (unaryNode2 != null)
				{
					while (unaryNode2.op == 0 && unaryNode2.right is UnaryNode && ((UnaryNode)unaryNode2.right).op == 0)
					{
						unaryNode2 = (UnaryNode)unaryNode2.right;
					}
					if (unaryNode2.op == 0 && unaryNode2.right is BinaryNode)
					{
						this.AnalyzeExpression((BinaryNode)unaryNode2.right);
						if (this.linearExpression == this.expression)
						{
							return;
						}
						flag2 = true;
					}
				}
			}
			if (flag && flag2)
			{
				return;
			}
			ExpressionNode expressionNode = (flag ? expr.right : expr.left);
			this.linearExpression = ((this.linearExpression == null) ? expressionNode : new BinaryNode(this.table, 26, expressionNode, this.linearExpression));
		}

		// Token: 0x06000D0B RID: 3339 RVA: 0x001FD210 File Offset: 0x001FC610
		private bool CompareSortIndexDesc(int[] id)
		{
			if (id.Length < this.indexDesc.Length)
			{
				return false;
			}
			int num = 0;
			int num2 = 0;
			while (num2 < id.Length && num < this.indexDesc.Length)
			{
				if (id[num2] == this.indexDesc[num])
				{
					num++;
				}
				else
				{
					Select.ColumnInfo columnInfo = this.candidateColumns[DataKey.ColumnOrder(id[num2])];
					if (columnInfo == null || !columnInfo.equalsOperator)
					{
						return false;
					}
				}
				num2++;
			}
			return num == this.indexDesc.Length;
		}

		// Token: 0x06000D0C RID: 3340 RVA: 0x001FD284 File Offset: 0x001FC684
		internal static int[] ConvertIndexFieldtoIndexDesc(IndexField[] fields)
		{
			int[] array = new int[fields.Length];
			for (int i = 0; i < fields.Length; i++)
			{
				array[i] = fields[i].Column.Ordinal | (fields[i].IsDescending ? int.MinValue : 0);
			}
			return array;
		}

		// Token: 0x06000D0D RID: 3341 RVA: 0x001FD2E0 File Offset: 0x001FC6E0
		private bool FindSortIndex()
		{
			this.index = null;
			this.table.indexesLock.AcquireReaderLock(-1);
			try
			{
				int count = this.table.indexes.Count;
				int count2 = this.table.Rows.Count;
				for (int i = 0; i < count; i++)
				{
					Index index = this.table.indexes[i];
					if (index.RecordStates == this.recordStates && index.IsSharable && this.CompareSortIndexDesc(index.IndexDesc))
					{
						this.index = index;
						return true;
					}
				}
			}
			finally
			{
				this.table.indexesLock.ReleaseReaderLock();
			}
			return false;
		}

		// Token: 0x06000D0E RID: 3342 RVA: 0x001FD3A8 File Offset: 0x001FC7A8
		private int CompareClosestCandidateIndexDesc(int[] id)
		{
			int num = ((id.Length < this.nCandidates) ? id.Length : this.nCandidates);
			int i;
			for (i = 0; i < num; i++)
			{
				Select.ColumnInfo columnInfo = this.candidateColumns[DataKey.ColumnOrder(id[i])];
				if (columnInfo == null || columnInfo.expr == null)
				{
					break;
				}
				if (!columnInfo.equalsOperator)
				{
					return i + 1;
				}
			}
			return i;
		}

		// Token: 0x06000D0F RID: 3343 RVA: 0x001FD400 File Offset: 0x001FC800
		private bool FindClosestCandidateIndex()
		{
			this.index = null;
			this.matchedCandidates = 0;
			bool flag = true;
			this.table.indexesLock.AcquireReaderLock(-1);
			try
			{
				int count = this.table.indexes.Count;
				int count2 = this.table.Rows.Count;
				for (int i = 0; i < count; i++)
				{
					Index index = this.table.indexes[i];
					if (index.RecordStates == this.recordStates && index.IsSharable)
					{
						int num = this.CompareClosestCandidateIndexDesc(index.IndexDesc);
						if (num > this.matchedCandidates || (num == this.matchedCandidates && !flag))
						{
							this.matchedCandidates = num;
							this.index = index;
							flag = this.CompareSortIndexDesc(index.IndexDesc);
							if (this.matchedCandidates == this.nCandidates && flag)
							{
								return true;
							}
						}
					}
				}
			}
			finally
			{
				this.table.indexesLock.ReleaseReaderLock();
			}
			return this.index != null && flag;
		}

		// Token: 0x06000D10 RID: 3344 RVA: 0x001FD51C File Offset: 0x001FC91C
		private void InitCandidateColumns()
		{
			this.nCandidates = 0;
			this.candidateColumns = new Select.ColumnInfo[this.table.Columns.Count];
			if (this.rowFilter == null)
			{
				return;
			}
			DataColumn[] dependency = this.rowFilter.GetDependency();
			for (int i = 0; i < dependency.Length; i++)
			{
				if (dependency[i].Table == this.table)
				{
					this.candidateColumns[dependency[i].Ordinal] = new Select.ColumnInfo();
					this.nCandidates++;
				}
			}
		}

		// Token: 0x06000D11 RID: 3345 RVA: 0x001FD5A0 File Offset: 0x001FC9A0
		private void CreateIndex()
		{
			if (this.index == null)
			{
				if (this.nCandidates == 0)
				{
					this.index = new Index(this.table, this.IndexFields, this.recordStates, null);
					this.index.AddRef();
					return;
				}
				int num = this.candidateColumns.Length;
				int num2 = this.indexDesc.Length;
				bool flag = true;
				int i;
				for (i = 0; i < num; i++)
				{
					if (this.candidateColumns[i] != null && !this.candidateColumns[i].equalsOperator)
					{
						flag = false;
						break;
					}
				}
				int num3 = 0;
				for (i = 0; i < num2; i++)
				{
					Select.ColumnInfo columnInfo = this.candidateColumns[DataKey.ColumnOrder(this.indexDesc[i])];
					if (columnInfo != null)
					{
						columnInfo.flag = true;
						num3++;
					}
				}
				int num4 = num2 - num3;
				int[] array = new int[this.nCandidates + num4];
				if (flag)
				{
					num3 = 0;
					for (i = 0; i < num; i++)
					{
						if (this.candidateColumns[i] != null)
						{
							array[num3++] = i;
							this.candidateColumns[i].flag = false;
						}
					}
					for (i = 0; i < num2; i++)
					{
						Select.ColumnInfo columnInfo2 = this.candidateColumns[DataKey.ColumnOrder(this.indexDesc[i])];
						if (columnInfo2 == null || columnInfo2.flag)
						{
							array[num3++] = this.indexDesc[i];
							if (columnInfo2 != null)
							{
								columnInfo2.flag = false;
							}
						}
					}
					for (i = 0; i < this.candidateColumns.Length; i++)
					{
						if (this.candidateColumns[i] != null)
						{
							this.candidateColumns[i].flag = false;
						}
					}
					IndexField[] array2 = DataTable.zeroIndexField;
					if (0 < array.Length)
					{
						array2 = new IndexField[array.Length];
						for (int j = 0; j < array.Length; j++)
						{
							DataColumn dataColumn = this.table.Columns[DataKey.ColumnOrder(array[j])];
							bool flag2 = DataKey.SortDecending(array[j]);
							array2[j] = new IndexField(dataColumn, flag2);
						}
					}
					this.index = new Index(this.table, array, array2, this.recordStates, null);
					if (!this.IsOperatorIn(this.expression))
					{
						this.index.AddRef();
					}
					this.matchedCandidates = this.nCandidates;
					return;
				}
				for (i = 0; i < num2; i++)
				{
					array[i] = this.indexDesc[i];
					Select.ColumnInfo columnInfo3 = this.candidateColumns[DataKey.ColumnOrder(this.indexDesc[i])];
					if (columnInfo3 != null)
					{
						columnInfo3.flag = true;
					}
				}
				num3 = i;
				for (i = 0; i < num; i++)
				{
					if (this.candidateColumns[i] != null)
					{
						if (!this.candidateColumns[i].flag)
						{
							array[num3++] = i;
						}
						else
						{
							this.candidateColumns[i].flag = false;
						}
					}
				}
				IndexField[] array3 = DataTable.zeroIndexField;
				if (0 < array.Length)
				{
					array3 = new IndexField[array.Length];
					for (int k = 0; k < array.Length; k++)
					{
						DataColumn dataColumn2 = this.table.Columns[DataKey.ColumnOrder(array[k])];
						bool flag3 = DataKey.SortDecending(array[k]);
						array3[k] = new IndexField(dataColumn2, flag3);
					}
				}
				this.index = new Index(this.table, array, array3, this.recordStates, null);
				this.matchedCandidates = 0;
				if (this.linearExpression != this.expression)
				{
					int[] array4 = this.index.IndexDesc;
					while (this.matchedCandidates < num3)
					{
						Select.ColumnInfo columnInfo4 = this.candidateColumns[DataKey.ColumnOrder(array4[this.matchedCandidates])];
						if (columnInfo4 == null || columnInfo4.expr == null)
						{
							break;
						}
						this.matchedCandidates++;
						if (!columnInfo4.equalsOperator)
						{
							break;
						}
					}
				}
				for (i = 0; i < this.candidateColumns.Length; i++)
				{
					if (this.candidateColumns[i] != null)
					{
						this.candidateColumns[i].flag = false;
					}
				}
			}
		}

		// Token: 0x06000D12 RID: 3346 RVA: 0x001FD950 File Offset: 0x001FCD50
		private bool IsOperatorIn(ExpressionNode enode)
		{
			BinaryNode binaryNode = enode as BinaryNode;
			return binaryNode != null && (5 == binaryNode.op || this.IsOperatorIn(binaryNode.right) || this.IsOperatorIn(binaryNode.left));
		}

		// Token: 0x06000D13 RID: 3347 RVA: 0x001FD990 File Offset: 0x001FCD90
		private void BuildLinearExpression()
		{
			int[] array = this.index.IndexDesc;
			int num = array.Length;
			for (int i = 0; i < this.matchedCandidates; i++)
			{
				Select.ColumnInfo columnInfo = this.candidateColumns[DataKey.ColumnOrder(array[i])];
				columnInfo.flag = true;
			}
			int num2 = this.candidateColumns.Length;
			for (int i = 0; i < num2; i++)
			{
				if (this.candidateColumns[i] != null)
				{
					if (!this.candidateColumns[i].flag)
					{
						if (this.candidateColumns[i].expr != null)
						{
							this.linearExpression = ((this.linearExpression == null) ? this.candidateColumns[i].expr : new BinaryNode(this.table, 26, this.candidateColumns[i].expr, this.linearExpression));
						}
					}
					else
					{
						this.candidateColumns[i].flag = false;
					}
				}
			}
		}

		// Token: 0x06000D14 RID: 3348 RVA: 0x001FDA64 File Offset: 0x001FCE64
		public DataRow[] SelectRows()
		{
			bool flag = true;
			this.InitCandidateColumns();
			if (this.expression is BinaryNode)
			{
				this.AnalyzeExpression((BinaryNode)this.expression);
				if (!this.candidatesForBinarySearch)
				{
					this.linearExpression = this.expression;
				}
				if (this.linearExpression == this.expression)
				{
					for (int i = 0; i < this.candidateColumns.Length; i++)
					{
						if (this.candidateColumns[i] != null)
						{
							this.candidateColumns[i].equalsOperator = false;
							this.candidateColumns[i].expr = null;
						}
					}
				}
				else
				{
					flag = !this.FindClosestCandidateIndex();
				}
			}
			else
			{
				this.linearExpression = this.expression;
			}
			if (this.index == null && (this.indexDesc.Length > 0 || this.linearExpression == this.expression))
			{
				flag = !this.FindSortIndex();
			}
			if (this.index == null)
			{
				this.CreateIndex();
				flag = false;
			}
			if (this.index.RecordCount == 0)
			{
				return this.table.NewRowArray(0);
			}
			Range binaryFilteredRecords;
			if (this.matchedCandidates == 0)
			{
				binaryFilteredRecords = new Range(0, this.index.RecordCount - 1);
				this.linearExpression = this.expression;
				return this.GetLinearFilteredRows(binaryFilteredRecords);
			}
			binaryFilteredRecords = this.GetBinaryFilteredRecords();
			if (binaryFilteredRecords.Count == 0)
			{
				return this.table.NewRowArray(0);
			}
			if (this.matchedCandidates < this.nCandidates)
			{
				this.BuildLinearExpression();
			}
			if (!flag)
			{
				return this.GetLinearFilteredRows(binaryFilteredRecords);
			}
			this.records = this.GetLinearFilteredRecords(binaryFilteredRecords);
			this.recordCount = this.records.Length;
			if (this.recordCount == 0)
			{
				return this.table.NewRowArray(0);
			}
			this.Sort(0, this.recordCount - 1);
			return this.GetRows();
		}

		// Token: 0x06000D15 RID: 3349 RVA: 0x001FDC18 File Offset: 0x001FD018
		public DataRow[] GetRows()
		{
			DataRow[] array = this.table.NewRowArray(this.recordCount);
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = this.table.recordManager[this.records[i]];
			}
			return array;
		}

		// Token: 0x06000D16 RID: 3350 RVA: 0x001FDC64 File Offset: 0x001FD064
		private bool AcceptRecord(int record)
		{
			DataRow dataRow = this.table.recordManager[record];
			if (dataRow == null)
			{
				return true;
			}
			DataRowVersion dataRowVersion = DataRowVersion.Default;
			if (dataRow.oldRecord == record)
			{
				dataRowVersion = DataRowVersion.Original;
			}
			else if (dataRow.newRecord == record)
			{
				dataRowVersion = DataRowVersion.Current;
			}
			else if (dataRow.tempRecord == record)
			{
				dataRowVersion = DataRowVersion.Proposed;
			}
			object obj = this.linearExpression.Eval(dataRow, dataRowVersion);
			bool flag;
			try
			{
				flag = DataExpression.ToBoolean(obj);
			}
			catch (Exception ex)
			{
				if (!ADP.IsCatchableExceptionType(ex))
				{
					throw;
				}
				throw ExprException.FilterConvertion(this.rowFilter.Expression);
			}
			return flag;
		}

		// Token: 0x06000D17 RID: 3351 RVA: 0x001FDD14 File Offset: 0x001FD114
		private int Eval(BinaryNode expr, DataRow row, DataRowVersion version)
		{
			if (expr.op != 26)
			{
				long num = 0L;
				object obj = expr.left.Eval(row, version);
				if (expr.op != 13 && expr.op != 39)
				{
					object obj2 = expr.right.Eval(row, version);
					bool flag = expr.left is ConstNode;
					bool flag2 = expr.right is ConstNode;
					if (obj == DBNull.Value || (expr.left.IsSqlColumn && DataStorage.IsObjectSqlNull(obj)))
					{
						return -1;
					}
					if (obj2 == DBNull.Value || (expr.right.IsSqlColumn && DataStorage.IsObjectSqlNull(obj2)))
					{
						return 1;
					}
					StorageType storageType = DataStorage.GetStorageType(obj.GetType());
					if (StorageType.Char == storageType)
					{
						if (flag2 || !expr.right.IsSqlColumn)
						{
							obj2 = Convert.ToChar(obj2, this.table.FormatProvider);
						}
						else
						{
							obj2 = SqlConvert.ChangeType2(obj2, StorageType.Char, typeof(char), this.table.FormatProvider);
						}
					}
					StorageType storageType2 = DataStorage.GetStorageType(obj2.GetType());
					StorageType storageType3;
					if (expr.left.IsSqlColumn || expr.right.IsSqlColumn)
					{
						storageType3 = expr.ResultSqlType(storageType, storageType2, flag, flag2, expr.op);
					}
					else
					{
						storageType3 = expr.ResultType(storageType, storageType2, flag, flag2, expr.op);
					}
					if (storageType3 == StorageType.Empty)
					{
						expr.SetTypeMismatchError(expr.op, obj.GetType(), obj2.GetType());
					}
					NameNode nameNode;
					CompareInfo compareInfo = (((flag && !flag2 && storageType == StorageType.String && storageType2 == StorageType.Guid && (nameNode = expr.right as NameNode) != null && nameNode.column.DataType == typeof(Guid)) || (flag2 && !flag && storageType2 == StorageType.String && storageType == StorageType.Guid && (nameNode = expr.left as NameNode) != null && nameNode.column.DataType == typeof(Guid))) ? CultureInfo.InvariantCulture.CompareInfo : null);
					num = (long)expr.BinaryCompare(obj, obj2, storageType3, expr.op, compareInfo);
				}
				int op = expr.op;
				switch (op)
				{
				case 7:
					num = ((num == 0L) ? 0L : ((num < 0L) ? (-1L) : 1L));
					break;
				case 8:
					num = ((num > 0L) ? 0L : (-1L));
					break;
				case 9:
					num = ((num < 0L) ? 0L : 1L);
					break;
				case 10:
					num = ((num >= 0L) ? 0L : (-1L));
					break;
				case 11:
					num = ((num <= 0L) ? 0L : 1L);
					break;
				case 12:
					break;
				case 13:
					num = ((obj == DBNull.Value) ? 0L : (-1L));
					break;
				default:
					if (op == 39)
					{
						num = ((obj != DBNull.Value) ? 0L : 1L);
					}
					break;
				}
				return (int)num;
			}
			int num2 = this.Eval((BinaryNode)expr.left, row, version);
			if (num2 != 0)
			{
				return num2;
			}
			int num3 = this.Eval((BinaryNode)expr.right, row, version);
			if (num3 != 0)
			{
				return num3;
			}
			return 0;
		}

		// Token: 0x06000D18 RID: 3352 RVA: 0x001FDFFC File Offset: 0x001FD3FC
		private int Evaluate(int record)
		{
			DataRow dataRow = this.table.recordManager[record];
			if (dataRow == null)
			{
				return 0;
			}
			DataRowVersion dataRowVersion = DataRowVersion.Default;
			if (dataRow.oldRecord == record)
			{
				dataRowVersion = DataRowVersion.Original;
			}
			else if (dataRow.newRecord == record)
			{
				dataRowVersion = DataRowVersion.Current;
			}
			else if (dataRow.tempRecord == record)
			{
				dataRowVersion = DataRowVersion.Proposed;
			}
			int[] array = this.index.IndexDesc;
			int i = 0;
			while (i < this.matchedCandidates)
			{
				int num = this.Eval(this.candidateColumns[DataKey.ColumnOrder(array[i])].expr, dataRow, dataRowVersion);
				if (num != 0)
				{
					if (!DataKey.SortDecending(array[i]))
					{
						return num;
					}
					return -num;
				}
				else
				{
					i++;
				}
			}
			return 0;
		}

		// Token: 0x06000D19 RID: 3353 RVA: 0x001FE0A8 File Offset: 0x001FD4A8
		private int FindFirstMatchingRecord()
		{
			int num = -1;
			int i = 0;
			int num2 = this.index.RecordCount - 1;
			while (i <= num2)
			{
				int num3 = i + num2 >> 1;
				int record = this.index.GetRecord(num3);
				int num4 = this.Evaluate(record);
				if (num4 == 0)
				{
					num = num3;
				}
				if (num4 < 0)
				{
					i = num3 + 1;
				}
				else
				{
					num2 = num3 - 1;
				}
			}
			return num;
		}

		// Token: 0x06000D1A RID: 3354 RVA: 0x001FE104 File Offset: 0x001FD504
		private int FindLastMatchingRecord(int lo)
		{
			int num = -1;
			int num2 = this.index.RecordCount - 1;
			while (lo <= num2)
			{
				int num3 = lo + num2 >> 1;
				int record = this.index.GetRecord(num3);
				int num4 = this.Evaluate(record);
				if (num4 == 0)
				{
					num = num3;
				}
				if (num4 <= 0)
				{
					lo = num3 + 1;
				}
				else
				{
					num2 = num3 - 1;
				}
			}
			return num;
		}

		// Token: 0x06000D1B RID: 3355 RVA: 0x001FE15C File Offset: 0x001FD55C
		private Range GetBinaryFilteredRecords()
		{
			if (this.matchedCandidates == 0)
			{
				return new Range(0, this.index.RecordCount - 1);
			}
			int num = this.FindFirstMatchingRecord();
			if (num == -1)
			{
				return default(Range);
			}
			int num2 = this.FindLastMatchingRecord(num);
			return new Range(num, num2);
		}

		// Token: 0x06000D1C RID: 3356 RVA: 0x001FE1AC File Offset: 0x001FD5AC
		private int[] GetLinearFilteredRecords(Range range)
		{
			if (this.linearExpression == null)
			{
				int[] array = new int[range.Count];
				RBTree<int>.RBTreeEnumerator enumerator = this.index.GetEnumerator(range.Min);
				int num = 0;
				while (num < range.Count && enumerator.MoveNext())
				{
					array[num] = enumerator.Current;
					num++;
				}
				return array;
			}
			List<int> list = new List<int>();
			RBTree<int>.RBTreeEnumerator enumerator2 = this.index.GetEnumerator(range.Min);
			int num2 = 0;
			while (num2 < range.Count && enumerator2.MoveNext())
			{
				if (this.AcceptRecord(enumerator2.Current))
				{
					list.Add(enumerator2.Current);
				}
				num2++;
			}
			return list.ToArray();
		}

		// Token: 0x06000D1D RID: 3357 RVA: 0x001FE260 File Offset: 0x001FD660
		private DataRow[] GetLinearFilteredRows(Range range)
		{
			if (this.linearExpression == null)
			{
				return this.index.GetRows(range);
			}
			List<DataRow> list = new List<DataRow>();
			RBTree<int>.RBTreeEnumerator enumerator = this.index.GetEnumerator(range.Min);
			int num = 0;
			while (num < range.Count && enumerator.MoveNext())
			{
				if (this.AcceptRecord(enumerator.Current))
				{
					list.Add(this.table.recordManager[enumerator.Current]);
				}
				num++;
			}
			DataRow[] array = this.table.NewRowArray(list.Count);
			list.CopyTo(array);
			return array;
		}

		// Token: 0x06000D1E RID: 3358 RVA: 0x001FE2FC File Offset: 0x001FD6FC
		private int CompareRecords(int record1, int record2)
		{
			int num = this.indexDesc.Length;
			for (int i = 0; i < num; i++)
			{
				int num2 = this.indexDesc[i];
				int num3 = this.table.Columns[DataKey.ColumnOrder(num2)].Compare(record1, record2);
				if (num3 != 0)
				{
					if (DataKey.SortDecending(num2))
					{
						num3 = -num3;
					}
					return num3;
				}
			}
			long num4 = ((this.table.recordManager[record1] == null) ? 0L : this.table.recordManager[record1].rowID);
			long num5 = ((this.table.recordManager[record2] == null) ? 0L : this.table.recordManager[record2].rowID);
			int num6 = ((num4 < num5) ? (-1) : ((num5 < num4) ? 1 : 0));
			if (num6 == 0 && record1 != record2 && this.table.recordManager[record1] != null && this.table.recordManager[record2] != null)
			{
				num4 = (long)this.table.recordManager[record1].GetRecordState(record1);
				num5 = (long)this.table.recordManager[record2].GetRecordState(record2);
				num6 = ((num4 < num5) ? (-1) : ((num5 < num4) ? 1 : 0));
			}
			return num6;
		}

		// Token: 0x06000D1F RID: 3359 RVA: 0x001FE43C File Offset: 0x001FD83C
		private void Sort(int left, int right)
		{
			int num;
			do
			{
				num = left;
				int num2 = right;
				int num3 = this.records[num + num2 >> 1];
				for (;;)
				{
					if (this.CompareRecords(this.records[num], num3) >= 0)
					{
						while (this.CompareRecords(this.records[num2], num3) > 0)
						{
							num2--;
						}
						if (num <= num2)
						{
							int num4 = this.records[num];
							this.records[num] = this.records[num2];
							this.records[num2] = num4;
							num++;
							num2--;
						}
						if (num > num2)
						{
							break;
						}
					}
					else
					{
						num++;
					}
				}
				if (left < num2)
				{
					this.Sort(left, num2);
				}
				left = num;
			}
			while (num < right);
		}

		// Token: 0x040008EB RID: 2283
		private readonly DataTable table;

		// Token: 0x040008EC RID: 2284
		private readonly int[] indexDesc;

		// Token: 0x040008ED RID: 2285
		private readonly IndexField[] IndexFields;

		// Token: 0x040008EE RID: 2286
		private DataViewRowState recordStates;

		// Token: 0x040008EF RID: 2287
		private DataExpression rowFilter;

		// Token: 0x040008F0 RID: 2288
		private ExpressionNode expression;

		// Token: 0x040008F1 RID: 2289
		private Index index;

		// Token: 0x040008F2 RID: 2290
		private int[] records;

		// Token: 0x040008F3 RID: 2291
		private int recordCount;

		// Token: 0x040008F4 RID: 2292
		private ExpressionNode linearExpression;

		// Token: 0x040008F5 RID: 2293
		private bool candidatesForBinarySearch;

		// Token: 0x040008F6 RID: 2294
		private Select.ColumnInfo[] candidateColumns;

		// Token: 0x040008F7 RID: 2295
		private int nCandidates;

		// Token: 0x040008F8 RID: 2296
		private int matchedCandidates;

		// Token: 0x020000DA RID: 218
		private sealed class ColumnInfo
		{
			// Token: 0x040008F9 RID: 2297
			public bool flag;

			// Token: 0x040008FA RID: 2298
			public bool equalsOperator;

			// Token: 0x040008FB RID: 2299
			public BinaryNode expr;
		}
	}
}
