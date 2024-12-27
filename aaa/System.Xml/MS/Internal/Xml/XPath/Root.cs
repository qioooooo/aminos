using System;
using System.Xml.XPath;

namespace MS.Internal.Xml.XPath
{
	// Token: 0x0200015B RID: 347
	internal class Root : AstNode
	{
		// Token: 0x17000496 RID: 1174
		// (get) Token: 0x060012E8 RID: 4840 RVA: 0x0005240E File Offset: 0x0005140E
		public override AstNode.AstType Type
		{
			get
			{
				return AstNode.AstType.Root;
			}
		}

		// Token: 0x17000497 RID: 1175
		// (get) Token: 0x060012E9 RID: 4841 RVA: 0x00052411 File Offset: 0x00051411
		public override XPathResultType ReturnType
		{
			get
			{
				return XPathResultType.NodeSet;
			}
		}
	}
}
