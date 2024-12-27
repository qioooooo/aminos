using System;
using System.Xml.XPath;

namespace MS.Internal.Xml.XPath
{
	// Token: 0x02000142 RID: 322
	internal class Group : AstNode
	{
		// Token: 0x06001233 RID: 4659 RVA: 0x0004FCF0 File Offset: 0x0004ECF0
		public Group(AstNode groupNode)
		{
			this.groupNode = groupNode;
		}

		// Token: 0x1700047A RID: 1146
		// (get) Token: 0x06001234 RID: 4660 RVA: 0x0004FCFF File Offset: 0x0004ECFF
		public override AstNode.AstType Type
		{
			get
			{
				return AstNode.AstType.Group;
			}
		}

		// Token: 0x1700047B RID: 1147
		// (get) Token: 0x06001235 RID: 4661 RVA: 0x0004FD02 File Offset: 0x0004ED02
		public override XPathResultType ReturnType
		{
			get
			{
				return XPathResultType.NodeSet;
			}
		}

		// Token: 0x1700047C RID: 1148
		// (get) Token: 0x06001236 RID: 4662 RVA: 0x0004FD05 File Offset: 0x0004ED05
		public AstNode GroupNode
		{
			get
			{
				return this.groupNode;
			}
		}

		// Token: 0x04000B87 RID: 2951
		private AstNode groupNode;
	}
}
