using System;
using System.Collections.Generic;
using System.Xml.XPath;

namespace System.Xml.Xsl.Runtime
{
	// Token: 0x02000075 RID: 117
	internal class DocumentOrderComparer : IComparer<XPathNavigator>
	{
		// Token: 0x060006E9 RID: 1769 RVA: 0x000251A0 File Offset: 0x000241A0
		public int Compare(XPathNavigator navThis, XPathNavigator navThat)
		{
			switch (navThis.ComparePosition(navThat))
			{
			case XmlNodeOrder.Before:
				return -1;
			case XmlNodeOrder.After:
				return 1;
			case XmlNodeOrder.Same:
				return 0;
			default:
				if (this.roots == null)
				{
					this.roots = new List<XPathNavigator>();
				}
				if (this.GetDocumentIndex(navThis) >= this.GetDocumentIndex(navThat))
				{
					return 1;
				}
				return -1;
			}
		}

		// Token: 0x060006EA RID: 1770 RVA: 0x000251F8 File Offset: 0x000241F8
		public int GetDocumentIndex(XPathNavigator nav)
		{
			if (this.roots == null)
			{
				this.roots = new List<XPathNavigator>();
			}
			XPathNavigator xpathNavigator = nav.Clone();
			xpathNavigator.MoveToRoot();
			for (int i = 0; i < this.roots.Count; i++)
			{
				if (xpathNavigator.IsSamePosition(this.roots[i]))
				{
					return i;
				}
			}
			this.roots.Add(xpathNavigator);
			return this.roots.Count - 1;
		}

		// Token: 0x0400045D RID: 1117
		private List<XPathNavigator> roots;
	}
}
