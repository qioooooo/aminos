using System;
using System.Xml.XPath;

namespace MS.Internal.Xml.XPath
{
	// Token: 0x02000151 RID: 337
	internal class Operator : AstNode
	{
		// Token: 0x060012AE RID: 4782 RVA: 0x000512C6 File Offset: 0x000502C6
		public Operator(Operator.Op op, AstNode opnd1, AstNode opnd2)
		{
			this.opType = op;
			this.opnd1 = opnd1;
			this.opnd2 = opnd2;
		}

		// Token: 0x1700048A RID: 1162
		// (get) Token: 0x060012AF RID: 4783 RVA: 0x000512E3 File Offset: 0x000502E3
		public override AstNode.AstType Type
		{
			get
			{
				return AstNode.AstType.Operator;
			}
		}

		// Token: 0x1700048B RID: 1163
		// (get) Token: 0x060012B0 RID: 4784 RVA: 0x000512E6 File Offset: 0x000502E6
		public override XPathResultType ReturnType
		{
			get
			{
				if (this.opType < Operator.Op.PLUS)
				{
					return XPathResultType.Boolean;
				}
				if (this.opType < Operator.Op.UNION)
				{
					return XPathResultType.Number;
				}
				return XPathResultType.NodeSet;
			}
		}

		// Token: 0x1700048C RID: 1164
		// (get) Token: 0x060012B1 RID: 4785 RVA: 0x00051300 File Offset: 0x00050300
		public Operator.Op OperatorType
		{
			get
			{
				return this.opType;
			}
		}

		// Token: 0x1700048D RID: 1165
		// (get) Token: 0x060012B2 RID: 4786 RVA: 0x00051308 File Offset: 0x00050308
		public AstNode Operand1
		{
			get
			{
				return this.opnd1;
			}
		}

		// Token: 0x1700048E RID: 1166
		// (get) Token: 0x060012B3 RID: 4787 RVA: 0x00051310 File Offset: 0x00050310
		public AstNode Operand2
		{
			get
			{
				return this.opnd2;
			}
		}

		// Token: 0x04000BA7 RID: 2983
		private Operator.Op opType;

		// Token: 0x04000BA8 RID: 2984
		private AstNode opnd1;

		// Token: 0x04000BA9 RID: 2985
		private AstNode opnd2;

		// Token: 0x02000152 RID: 338
		public enum Op
		{
			// Token: 0x04000BAB RID: 2987
			LT,
			// Token: 0x04000BAC RID: 2988
			GT,
			// Token: 0x04000BAD RID: 2989
			LE,
			// Token: 0x04000BAE RID: 2990
			GE,
			// Token: 0x04000BAF RID: 2991
			EQ,
			// Token: 0x04000BB0 RID: 2992
			NE,
			// Token: 0x04000BB1 RID: 2993
			OR,
			// Token: 0x04000BB2 RID: 2994
			AND,
			// Token: 0x04000BB3 RID: 2995
			PLUS,
			// Token: 0x04000BB4 RID: 2996
			MINUS,
			// Token: 0x04000BB5 RID: 2997
			MUL,
			// Token: 0x04000BB6 RID: 2998
			MOD,
			// Token: 0x04000BB7 RID: 2999
			DIV,
			// Token: 0x04000BB8 RID: 3000
			UNION,
			// Token: 0x04000BB9 RID: 3001
			INVALID
		}
	}
}
