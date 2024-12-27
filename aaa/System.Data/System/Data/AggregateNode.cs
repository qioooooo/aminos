using System;
using System.Collections.Generic;

namespace System.Data
{
	// Token: 0x020001A2 RID: 418
	internal sealed class AggregateNode : ExpressionNode
	{
		// Token: 0x06001864 RID: 6244 RVA: 0x00236E80 File Offset: 0x00236280
		internal AggregateNode(DataTable table, FunctionId aggregateType, string columnName)
			: this(table, aggregateType, columnName, true, null)
		{
		}

		// Token: 0x06001865 RID: 6245 RVA: 0x00236E98 File Offset: 0x00236298
		internal AggregateNode(DataTable table, FunctionId aggregateType, string columnName, string relationName)
			: this(table, aggregateType, columnName, false, relationName)
		{
		}

		// Token: 0x06001866 RID: 6246 RVA: 0x00236EB4 File Offset: 0x002362B4
		internal AggregateNode(DataTable table, FunctionId aggregateType, string columnName, bool local, string relationName)
			: base(table)
		{
			this.aggregate = (Aggregate)aggregateType;
			if (aggregateType == FunctionId.Sum)
			{
				this.type = AggregateType.Sum;
			}
			else if (aggregateType == FunctionId.Avg)
			{
				this.type = AggregateType.Mean;
			}
			else if (aggregateType == FunctionId.Min)
			{
				this.type = AggregateType.Min;
			}
			else if (aggregateType == FunctionId.Max)
			{
				this.type = AggregateType.Max;
			}
			else if (aggregateType == FunctionId.Count)
			{
				this.type = AggregateType.Count;
			}
			else if (aggregateType == FunctionId.Var)
			{
				this.type = AggregateType.Var;
			}
			else
			{
				if (aggregateType != FunctionId.StDev)
				{
					throw ExprException.UndefinedFunction(Function.FunctionName[(int)aggregateType]);
				}
				this.type = AggregateType.StDev;
			}
			this.local = local;
			this.relationName = relationName;
			this.columnName = columnName;
		}

		// Token: 0x06001867 RID: 6247 RVA: 0x00236F58 File Offset: 0x00236358
		internal override void Bind(DataTable table, List<DataColumn> list)
		{
			base.BindTable(table);
			if (table == null)
			{
				throw ExprException.AggregateUnbound(this.ToString());
			}
			if (this.local)
			{
				this.relation = null;
			}
			else
			{
				DataRelationCollection childRelations = table.ChildRelations;
				if (this.relationName == null)
				{
					if (childRelations.Count > 1)
					{
						throw ExprException.UnresolvedRelation(table.TableName, this.ToString());
					}
					if (childRelations.Count != 1)
					{
						throw ExprException.AggregateUnbound(this.ToString());
					}
					this.relation = childRelations[0];
				}
				else
				{
					this.relation = childRelations[this.relationName];
				}
			}
			this.childTable = ((this.relation == null) ? table : this.relation.ChildTable);
			this.column = this.childTable.Columns[this.columnName];
			if (this.column == null)
			{
				throw ExprException.UnboundName(this.columnName);
			}
			int i;
			for (i = 0; i < list.Count; i++)
			{
				DataColumn dataColumn = list[i];
				if (this.column == dataColumn)
				{
					break;
				}
			}
			if (i >= list.Count)
			{
				list.Add(this.column);
			}
			AggregateNode.Bind(this.relation, list);
		}

		// Token: 0x06001868 RID: 6248 RVA: 0x0023707C File Offset: 0x0023647C
		internal static void Bind(DataRelation relation, List<DataColumn> list)
		{
			if (relation != null)
			{
				foreach (DataColumn dataColumn in relation.ChildColumnsReference)
				{
					if (!list.Contains(dataColumn))
					{
						list.Add(dataColumn);
					}
				}
				foreach (DataColumn dataColumn2 in relation.ParentColumnsReference)
				{
					if (!list.Contains(dataColumn2))
					{
						list.Add(dataColumn2);
					}
				}
			}
		}

		// Token: 0x06001869 RID: 6249 RVA: 0x002370E4 File Offset: 0x002364E4
		internal override object Eval()
		{
			return this.Eval(null, DataRowVersion.Default);
		}

		// Token: 0x0600186A RID: 6250 RVA: 0x00237100 File Offset: 0x00236500
		internal override object Eval(DataRow row, DataRowVersion version)
		{
			if (this.childTable == null)
			{
				throw ExprException.AggregateUnbound(this.ToString());
			}
			DataRow[] array;
			if (this.local)
			{
				array = new DataRow[this.childTable.Rows.Count];
				this.childTable.Rows.CopyTo(array, 0);
			}
			else
			{
				if (row == null)
				{
					throw ExprException.EvalNoContext();
				}
				if (this.relation == null)
				{
					throw ExprException.AggregateUnbound(this.ToString());
				}
				array = row.GetChildRows(this.relation, version);
			}
			if (version == DataRowVersion.Proposed)
			{
				version = DataRowVersion.Default;
			}
			List<int> list = new List<int>();
			int i = 0;
			while (i < array.Length)
			{
				if (array[i].RowState == DataRowState.Deleted)
				{
					if (DataRowAction.Rollback == array[i]._action)
					{
						version = DataRowVersion.Original;
						goto IL_00BF;
					}
				}
				else if (DataRowAction.Rollback != array[i]._action || array[i].RowState != DataRowState.Added)
				{
					goto IL_00BF;
				}
				IL_00E1:
				i++;
				continue;
				IL_00BF:
				if (version != DataRowVersion.Original || array[i].oldRecord != -1)
				{
					list.Add(array[i].GetRecordFromVersion(version));
					goto IL_00E1;
				}
				goto IL_00E1;
			}
			int[] array2 = list.ToArray();
			return this.column.GetAggregateValue(array2, this.type);
		}

		// Token: 0x0600186B RID: 6251 RVA: 0x00237214 File Offset: 0x00236614
		internal override object Eval(int[] records)
		{
			if (this.childTable == null)
			{
				throw ExprException.AggregateUnbound(this.ToString());
			}
			if (!this.local)
			{
				throw ExprException.ComputeNotAggregate(this.ToString());
			}
			return this.column.GetAggregateValue(records, this.type);
		}

		// Token: 0x0600186C RID: 6252 RVA: 0x0023725C File Offset: 0x0023665C
		internal override bool IsConstant()
		{
			return false;
		}

		// Token: 0x0600186D RID: 6253 RVA: 0x0023726C File Offset: 0x0023666C
		internal override bool IsTableConstant()
		{
			return this.local;
		}

		// Token: 0x0600186E RID: 6254 RVA: 0x00237280 File Offset: 0x00236680
		internal override bool HasLocalAggregate()
		{
			return this.local;
		}

		// Token: 0x0600186F RID: 6255 RVA: 0x00237294 File Offset: 0x00236694
		internal override bool HasRemoteAggregate()
		{
			return !this.local;
		}

		// Token: 0x06001870 RID: 6256 RVA: 0x002372AC File Offset: 0x002366AC
		internal override bool DependsOn(DataColumn column)
		{
			return this.column == column || (this.column.Computed && this.column.DataExpression.DependsOn(column));
		}

		// Token: 0x06001871 RID: 6257 RVA: 0x002372E4 File Offset: 0x002366E4
		internal override ExpressionNode Optimize()
		{
			return this;
		}

		// Token: 0x04000D1D RID: 3357
		private readonly AggregateType type;

		// Token: 0x04000D1E RID: 3358
		private readonly Aggregate aggregate;

		// Token: 0x04000D1F RID: 3359
		private readonly bool local;

		// Token: 0x04000D20 RID: 3360
		private readonly string relationName;

		// Token: 0x04000D21 RID: 3361
		private readonly string columnName;

		// Token: 0x04000D22 RID: 3362
		private DataTable childTable;

		// Token: 0x04000D23 RID: 3363
		private DataColumn column;

		// Token: 0x04000D24 RID: 3364
		private DataRelation relation;
	}
}
