using System;
using System.Xml.XPath;

namespace MS.Internal.Xml.XPath
{
	// Token: 0x02000121 RID: 289
	internal sealed class AbsoluteQuery : ContextQuery
	{
		// Token: 0x0600114B RID: 4427 RVA: 0x0004D7C2 File Offset: 0x0004C7C2
		public AbsoluteQuery()
		{
		}

		// Token: 0x0600114C RID: 4428 RVA: 0x0004D7CA File Offset: 0x0004C7CA
		private AbsoluteQuery(AbsoluteQuery other)
			: base(other)
		{
		}

		// Token: 0x0600114D RID: 4429 RVA: 0x0004D7D3 File Offset: 0x0004C7D3
		public override object Evaluate(XPathNodeIterator context)
		{
			this.contextNode = context.Current.Clone();
			this.contextNode.MoveToRoot();
			this.count = 0;
			return this;
		}

		// Token: 0x0600114E RID: 4430 RVA: 0x0004D7F9 File Offset: 0x0004C7F9
		public override XPathNavigator MatchNode(XPathNavigator context)
		{
			if (context != null && context.NodeType == XPathNodeType.Root)
			{
				return context;
			}
			return null;
		}

		// Token: 0x0600114F RID: 4431 RVA: 0x0004D809 File Offset: 0x0004C809
		public override XPathNodeIterator Clone()
		{
			return new AbsoluteQuery(this);
		}
	}
}
