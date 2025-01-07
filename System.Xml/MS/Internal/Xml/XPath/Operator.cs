using System;
using System.Xml.XPath;

namespace MS.Internal.Xml.XPath
{
	internal class Operator : AstNode
	{
		public Operator(Operator.Op op, AstNode opnd1, AstNode opnd2)
		{
			this.opType = op;
			this.opnd1 = opnd1;
			this.opnd2 = opnd2;
		}

		public override AstNode.AstType Type
		{
			get
			{
				return AstNode.AstType.Operator;
			}
		}

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

		public Operator.Op OperatorType
		{
			get
			{
				return this.opType;
			}
		}

		public AstNode Operand1
		{
			get
			{
				return this.opnd1;
			}
		}

		public AstNode Operand2
		{
			get
			{
				return this.opnd2;
			}
		}

		private Operator.Op opType;

		private AstNode opnd1;

		private AstNode opnd2;

		public enum Op
		{
			LT,
			GT,
			LE,
			GE,
			EQ,
			NE,
			OR,
			AND,
			PLUS,
			MINUS,
			MUL,
			MOD,
			DIV,
			UNION,
			INVALID
		}
	}
}
