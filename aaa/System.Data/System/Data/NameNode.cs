using System;
using System.Collections.Generic;
using System.Data.Common;

namespace System.Data
{
	// Token: 0x020001B6 RID: 438
	internal sealed class NameNode : ExpressionNode
	{
		// Token: 0x06001927 RID: 6439 RVA: 0x0023DD44 File Offset: 0x0023D144
		internal NameNode(DataTable table, char[] text, int start, int pos)
			: base(table)
		{
			this.name = NameNode.ParseName(text, start, pos);
		}

		// Token: 0x06001928 RID: 6440 RVA: 0x0023DD68 File Offset: 0x0023D168
		internal NameNode(DataTable table, string name)
			: base(table)
		{
			this.name = name;
		}

		// Token: 0x17000332 RID: 818
		// (get) Token: 0x06001929 RID: 6441 RVA: 0x0023DD84 File Offset: 0x0023D184
		internal override bool IsSqlColumn
		{
			get
			{
				return this.column.IsSqlType;
			}
		}

		// Token: 0x0600192A RID: 6442 RVA: 0x0023DD9C File Offset: 0x0023D19C
		internal override void Bind(DataTable table, List<DataColumn> list)
		{
			base.BindTable(table);
			if (table == null)
			{
				throw ExprException.UnboundName(this.name);
			}
			try
			{
				this.column = table.Columns[this.name];
			}
			catch (Exception ex)
			{
				this.found = false;
				if (!ADP.IsCatchableExceptionType(ex))
				{
					throw;
				}
				throw ExprException.UnboundName(this.name);
			}
			if (this.column == null)
			{
				throw ExprException.UnboundName(this.name);
			}
			this.name = this.column.ColumnName;
			this.found = true;
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
		}

		// Token: 0x0600192B RID: 6443 RVA: 0x0023DE78 File Offset: 0x0023D278
		internal override object Eval()
		{
			throw ExprException.EvalNoContext();
		}

		// Token: 0x0600192C RID: 6444 RVA: 0x0023DE8C File Offset: 0x0023D28C
		internal override object Eval(DataRow row, DataRowVersion version)
		{
			if (!this.found)
			{
				throw ExprException.UnboundName(this.name);
			}
			if (row != null)
			{
				return this.column[row.GetRecordFromVersion(version)];
			}
			if (this.IsTableConstant())
			{
				return this.column.DataExpression.Evaluate();
			}
			throw ExprException.UnboundName(this.name);
		}

		// Token: 0x0600192D RID: 6445 RVA: 0x0023DEE8 File Offset: 0x0023D2E8
		internal override object Eval(int[] records)
		{
			throw ExprException.ComputeNotAggregate(this.ToString());
		}

		// Token: 0x0600192E RID: 6446 RVA: 0x0023DF00 File Offset: 0x0023D300
		internal override bool IsConstant()
		{
			return false;
		}

		// Token: 0x0600192F RID: 6447 RVA: 0x0023DF10 File Offset: 0x0023D310
		internal override bool IsTableConstant()
		{
			return this.column != null && this.column.Computed && this.column.DataExpression.IsTableAggregate();
		}

		// Token: 0x06001930 RID: 6448 RVA: 0x0023DF44 File Offset: 0x0023D344
		internal override bool HasLocalAggregate()
		{
			return this.column != null && this.column.Computed && this.column.DataExpression.HasLocalAggregate();
		}

		// Token: 0x06001931 RID: 6449 RVA: 0x0023DF78 File Offset: 0x0023D378
		internal override bool HasRemoteAggregate()
		{
			return this.column != null && this.column.Computed && this.column.DataExpression.HasRemoteAggregate();
		}

		// Token: 0x06001932 RID: 6450 RVA: 0x0023DFAC File Offset: 0x0023D3AC
		internal override bool DependsOn(DataColumn column)
		{
			return this.column == column || (this.column.Computed && this.column.DataExpression.DependsOn(column));
		}

		// Token: 0x06001933 RID: 6451 RVA: 0x0023DFE4 File Offset: 0x0023D3E4
		internal override ExpressionNode Optimize()
		{
			return this;
		}

		// Token: 0x06001934 RID: 6452 RVA: 0x0023DFF4 File Offset: 0x0023D3F4
		internal static string ParseName(char[] text, int start, int pos)
		{
			char c = '\0';
			string text2 = "";
			int num = start;
			int num2 = pos;
			checked
			{
				if (text[start] == '`')
				{
					start++;
					pos--;
					c = '\\';
					text2 = "`";
				}
				else if (text[start] == '[')
				{
					start++;
					pos--;
					c = '\\';
					text2 = "]\\";
				}
			}
			if (c != '\0')
			{
				int num3 = start;
				for (int i = start; i < pos; i++)
				{
					if (text[i] == c && i + 1 < pos && text2.IndexOf(text[i + 1]) >= 0)
					{
						i++;
					}
					text[num3] = text[i];
					num3++;
				}
				pos = num3;
			}
			if (pos == start)
			{
				throw ExprException.InvalidName(new string(text, num, num2 - num));
			}
			return new string(text, start, pos - start);
		}

		// Token: 0x04000DDC RID: 3548
		internal char open;

		// Token: 0x04000DDD RID: 3549
		internal char close;

		// Token: 0x04000DDE RID: 3550
		internal string name;

		// Token: 0x04000DDF RID: 3551
		internal bool found;

		// Token: 0x04000DE0 RID: 3552
		internal bool type;

		// Token: 0x04000DE1 RID: 3553
		internal DataColumn column;
	}
}
