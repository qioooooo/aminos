using System;
using System.Xml.XPath;

namespace MS.Internal.Xml.XPath
{
	// Token: 0x0200014F RID: 335
	internal class Operand : AstNode
	{
		// Token: 0x060012A3 RID: 4771 RVA: 0x0005120B File Offset: 0x0005020B
		public Operand(string val)
		{
			this.type = XPathResultType.String;
			this.val = val;
		}

		// Token: 0x060012A4 RID: 4772 RVA: 0x00051221 File Offset: 0x00050221
		public Operand(double val)
		{
			this.type = XPathResultType.Number;
			this.val = val;
		}

		// Token: 0x060012A5 RID: 4773 RVA: 0x0005123C File Offset: 0x0005023C
		public Operand(bool val)
		{
			this.type = XPathResultType.Boolean;
			this.val = val;
		}

		// Token: 0x17000486 RID: 1158
		// (get) Token: 0x060012A6 RID: 4774 RVA: 0x00051257 File Offset: 0x00050257
		public override AstNode.AstType Type
		{
			get
			{
				return AstNode.AstType.ConstantOperand;
			}
		}

		// Token: 0x17000487 RID: 1159
		// (get) Token: 0x060012A7 RID: 4775 RVA: 0x0005125A File Offset: 0x0005025A
		public override XPathResultType ReturnType
		{
			get
			{
				return this.type;
			}
		}

		// Token: 0x17000488 RID: 1160
		// (get) Token: 0x060012A8 RID: 4776 RVA: 0x00051262 File Offset: 0x00050262
		public object OperandValue
		{
			get
			{
				return this.val;
			}
		}

		// Token: 0x04000BA4 RID: 2980
		private XPathResultType type;

		// Token: 0x04000BA5 RID: 2981
		private object val;
	}
}
