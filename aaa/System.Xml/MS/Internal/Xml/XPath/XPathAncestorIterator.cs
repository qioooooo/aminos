using System;
using System.Xml.XPath;

namespace MS.Internal.Xml.XPath
{
	// Token: 0x02000163 RID: 355
	internal class XPathAncestorIterator : XPathAxisIterator
	{
		// Token: 0x0600132D RID: 4909 RVA: 0x000532F1 File Offset: 0x000522F1
		public XPathAncestorIterator(XPathNavigator nav, XPathNodeType type, bool matchSelf)
			: base(nav, type, matchSelf)
		{
		}

		// Token: 0x0600132E RID: 4910 RVA: 0x000532FC File Offset: 0x000522FC
		public XPathAncestorIterator(XPathNavigator nav, string name, string namespaceURI, bool matchSelf)
			: base(nav, name, namespaceURI, matchSelf)
		{
		}

		// Token: 0x0600132F RID: 4911 RVA: 0x00053309 File Offset: 0x00052309
		public XPathAncestorIterator(XPathAncestorIterator other)
			: base(other)
		{
		}

		// Token: 0x06001330 RID: 4912 RVA: 0x00053314 File Offset: 0x00052314
		public override bool MoveNext()
		{
			if (this.first)
			{
				this.first = false;
				if (this.matchSelf && this.Matches)
				{
					this.position = 1;
					return true;
				}
			}
			while (this.nav.MoveToParent())
			{
				if (this.Matches)
				{
					this.position++;
					return true;
				}
			}
			return false;
		}

		// Token: 0x06001331 RID: 4913 RVA: 0x0005336F File Offset: 0x0005236F
		public override XPathNodeIterator Clone()
		{
			return new XPathAncestorIterator(this);
		}
	}
}
