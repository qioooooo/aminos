using System;
using System.Collections.Generic;

namespace System.Data
{
	// Token: 0x020001B9 RID: 441
	internal sealed class ZeroOpNode : ExpressionNode
	{
		// Token: 0x06001948 RID: 6472 RVA: 0x0023E6C4 File Offset: 0x0023DAC4
		internal ZeroOpNode(int op)
			: base(null)
		{
			this.op = op;
		}

		// Token: 0x06001949 RID: 6473 RVA: 0x0023E6E0 File Offset: 0x0023DAE0
		internal override void Bind(DataTable table, List<DataColumn> list)
		{
		}

		// Token: 0x0600194A RID: 6474 RVA: 0x0023E6F0 File Offset: 0x0023DAF0
		internal override object Eval()
		{
			switch (this.op)
			{
			case 32:
				return DBNull.Value;
			case 33:
				return true;
			case 34:
				return false;
			default:
				return DBNull.Value;
			}
		}

		// Token: 0x0600194B RID: 6475 RVA: 0x0023E734 File Offset: 0x0023DB34
		internal override object Eval(DataRow row, DataRowVersion version)
		{
			return this.Eval();
		}

		// Token: 0x0600194C RID: 6476 RVA: 0x0023E748 File Offset: 0x0023DB48
		internal override object Eval(int[] recordNos)
		{
			return this.Eval();
		}

		// Token: 0x0600194D RID: 6477 RVA: 0x0023E75C File Offset: 0x0023DB5C
		internal override bool IsConstant()
		{
			return true;
		}

		// Token: 0x0600194E RID: 6478 RVA: 0x0023E76C File Offset: 0x0023DB6C
		internal override bool IsTableConstant()
		{
			return true;
		}

		// Token: 0x0600194F RID: 6479 RVA: 0x0023E77C File Offset: 0x0023DB7C
		internal override bool HasLocalAggregate()
		{
			return false;
		}

		// Token: 0x06001950 RID: 6480 RVA: 0x0023E78C File Offset: 0x0023DB8C
		internal override bool HasRemoteAggregate()
		{
			return false;
		}

		// Token: 0x06001951 RID: 6481 RVA: 0x0023E79C File Offset: 0x0023DB9C
		internal override ExpressionNode Optimize()
		{
			return this;
		}

		// Token: 0x04000E25 RID: 3621
		internal const int zop_True = 1;

		// Token: 0x04000E26 RID: 3622
		internal const int zop_False = 0;

		// Token: 0x04000E27 RID: 3623
		internal const int zop_Null = -1;

		// Token: 0x04000E28 RID: 3624
		internal readonly int op;
	}
}
