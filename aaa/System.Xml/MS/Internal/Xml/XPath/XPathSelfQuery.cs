using System;
using System.Xml.XPath;

namespace MS.Internal.Xml.XPath
{
	// Token: 0x0200016F RID: 367
	internal sealed class XPathSelfQuery : BaseAxisQuery
	{
		// Token: 0x060013A4 RID: 5028 RVA: 0x000554E8 File Offset: 0x000544E8
		public XPathSelfQuery(Query qyInput, string Name, string Prefix, XPathNodeType Type)
			: base(qyInput, Name, Prefix, Type)
		{
		}

		// Token: 0x060013A5 RID: 5029 RVA: 0x000554F5 File Offset: 0x000544F5
		private XPathSelfQuery(XPathSelfQuery other)
			: base(other)
		{
		}

		// Token: 0x060013A6 RID: 5030 RVA: 0x00055500 File Offset: 0x00054500
		public override XPathNavigator Advance()
		{
			while ((this.currentNode = this.qyInput.Advance()) != null)
			{
				if (this.matches(this.currentNode))
				{
					this.position = 1;
					return this.currentNode;
				}
			}
			return null;
		}

		// Token: 0x060013A7 RID: 5031 RVA: 0x00055542 File Offset: 0x00054542
		public override XPathNodeIterator Clone()
		{
			return new XPathSelfQuery(this);
		}
	}
}
