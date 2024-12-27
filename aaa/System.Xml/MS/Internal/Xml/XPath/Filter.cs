using System;
using System.Xml.XPath;

namespace MS.Internal.Xml.XPath
{
	// Token: 0x0200013B RID: 315
	internal class Filter : AstNode
	{
		// Token: 0x06001207 RID: 4615 RVA: 0x0004F3F0 File Offset: 0x0004E3F0
		public Filter(AstNode input, AstNode condition)
		{
			this.input = input;
			this.condition = condition;
		}

		// Token: 0x1700046E RID: 1134
		// (get) Token: 0x06001208 RID: 4616 RVA: 0x0004F406 File Offset: 0x0004E406
		public override AstNode.AstType Type
		{
			get
			{
				return AstNode.AstType.Filter;
			}
		}

		// Token: 0x1700046F RID: 1135
		// (get) Token: 0x06001209 RID: 4617 RVA: 0x0004F409 File Offset: 0x0004E409
		public override XPathResultType ReturnType
		{
			get
			{
				return XPathResultType.NodeSet;
			}
		}

		// Token: 0x17000470 RID: 1136
		// (get) Token: 0x0600120A RID: 4618 RVA: 0x0004F40C File Offset: 0x0004E40C
		public AstNode Input
		{
			get
			{
				return this.input;
			}
		}

		// Token: 0x17000471 RID: 1137
		// (get) Token: 0x0600120B RID: 4619 RVA: 0x0004F414 File Offset: 0x0004E414
		public AstNode Condition
		{
			get
			{
				return this.condition;
			}
		}

		// Token: 0x04000B5C RID: 2908
		private AstNode input;

		// Token: 0x04000B5D RID: 2909
		private AstNode condition;
	}
}
