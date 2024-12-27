using System;
using System.Collections.Generic;

namespace System.Data
{
	// Token: 0x020001B5 RID: 437
	internal sealed class LookupNode : ExpressionNode
	{
		// Token: 0x0600191C RID: 6428 RVA: 0x0023DB1C File Offset: 0x0023CF1C
		internal LookupNode(DataTable table, string columnName, string relationName)
			: base(table)
		{
			this.relationName = relationName;
			this.columnName = columnName;
		}

		// Token: 0x0600191D RID: 6429 RVA: 0x0023DB40 File Offset: 0x0023CF40
		internal override void Bind(DataTable table, List<DataColumn> list)
		{
			base.BindTable(table);
			this.column = null;
			this.relation = null;
			if (table == null)
			{
				throw ExprException.ExpressionUnbound(this.ToString());
			}
			DataRelationCollection parentRelations = table.ParentRelations;
			if (this.relationName == null)
			{
				if (parentRelations.Count > 1)
				{
					throw ExprException.UnresolvedRelation(table.TableName, this.ToString());
				}
				this.relation = parentRelations[0];
			}
			else
			{
				this.relation = parentRelations[this.relationName];
			}
			if (this.relation == null)
			{
				throw ExprException.BindFailure(this.relationName);
			}
			DataTable parentTable = this.relation.ParentTable;
			this.column = parentTable.Columns[this.columnName];
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

		// Token: 0x0600191E RID: 6430 RVA: 0x0023DC4C File Offset: 0x0023D04C
		internal override object Eval()
		{
			throw ExprException.EvalNoContext();
		}

		// Token: 0x0600191F RID: 6431 RVA: 0x0023DC60 File Offset: 0x0023D060
		internal override object Eval(DataRow row, DataRowVersion version)
		{
			if (this.column == null || this.relation == null)
			{
				throw ExprException.ExpressionUnbound(this.ToString());
			}
			DataRow parentRow = row.GetParentRow(this.relation, version);
			if (parentRow == null)
			{
				return DBNull.Value;
			}
			return parentRow[this.column, parentRow.HasVersion(version) ? version : DataRowVersion.Current];
		}

		// Token: 0x06001920 RID: 6432 RVA: 0x0023DCC0 File Offset: 0x0023D0C0
		internal override object Eval(int[] recordNos)
		{
			throw ExprException.ComputeNotAggregate(this.ToString());
		}

		// Token: 0x06001921 RID: 6433 RVA: 0x0023DCD8 File Offset: 0x0023D0D8
		internal override bool IsConstant()
		{
			return false;
		}

		// Token: 0x06001922 RID: 6434 RVA: 0x0023DCE8 File Offset: 0x0023D0E8
		internal override bool IsTableConstant()
		{
			return false;
		}

		// Token: 0x06001923 RID: 6435 RVA: 0x0023DCF8 File Offset: 0x0023D0F8
		internal override bool HasLocalAggregate()
		{
			return false;
		}

		// Token: 0x06001924 RID: 6436 RVA: 0x0023DD08 File Offset: 0x0023D108
		internal override bool HasRemoteAggregate()
		{
			return false;
		}

		// Token: 0x06001925 RID: 6437 RVA: 0x0023DD18 File Offset: 0x0023D118
		internal override bool DependsOn(DataColumn column)
		{
			return this.column == column;
		}

		// Token: 0x06001926 RID: 6438 RVA: 0x0023DD34 File Offset: 0x0023D134
		internal override ExpressionNode Optimize()
		{
			return this;
		}

		// Token: 0x04000DD8 RID: 3544
		private readonly string relationName;

		// Token: 0x04000DD9 RID: 3545
		private readonly string columnName;

		// Token: 0x04000DDA RID: 3546
		private DataColumn column;

		// Token: 0x04000DDB RID: 3547
		private DataRelation relation;
	}
}
