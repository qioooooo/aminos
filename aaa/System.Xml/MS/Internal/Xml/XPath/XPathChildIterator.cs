using System;
using System.Xml.XPath;

namespace MS.Internal.Xml.XPath
{
	// Token: 0x02000166 RID: 358
	internal class XPathChildIterator : XPathAxisIterator
	{
		// Token: 0x06001345 RID: 4933 RVA: 0x00053628 File Offset: 0x00052628
		public XPathChildIterator(XPathNavigator nav, XPathNodeType type)
			: base(nav, type, false)
		{
		}

		// Token: 0x06001346 RID: 4934 RVA: 0x00053633 File Offset: 0x00052633
		public XPathChildIterator(XPathNavigator nav, string name, string namespaceURI)
			: base(nav, name, namespaceURI, false)
		{
		}

		// Token: 0x06001347 RID: 4935 RVA: 0x0005363F File Offset: 0x0005263F
		public XPathChildIterator(XPathChildIterator it)
			: base(it)
		{
		}

		// Token: 0x06001348 RID: 4936 RVA: 0x00053648 File Offset: 0x00052648
		public override XPathNodeIterator Clone()
		{
			return new XPathChildIterator(this);
		}

		// Token: 0x06001349 RID: 4937 RVA: 0x00053650 File Offset: 0x00052650
		public override bool MoveNext()
		{
			while (this.first ? this.nav.MoveToFirstChild() : this.nav.MoveToNext())
			{
				this.first = false;
				if (this.Matches)
				{
					this.position++;
					return true;
				}
			}
			return false;
		}
	}
}
