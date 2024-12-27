using System;
using System.Xml.XPath;

namespace System.Xml.Xsl.Runtime
{
	// Token: 0x020000AE RID: 174
	internal class XmlNavAttrFilter : XmlNavigatorFilter
	{
		// Token: 0x06000811 RID: 2065 RVA: 0x0002881B File Offset: 0x0002781B
		public static XmlNavigatorFilter Create()
		{
			return XmlNavAttrFilter.Singleton;
		}

		// Token: 0x06000812 RID: 2066 RVA: 0x00028822 File Offset: 0x00027822
		private XmlNavAttrFilter()
		{
		}

		// Token: 0x06000813 RID: 2067 RVA: 0x0002882A File Offset: 0x0002782A
		public override bool MoveToContent(XPathNavigator navigator)
		{
			return navigator.MoveToFirstChild();
		}

		// Token: 0x06000814 RID: 2068 RVA: 0x00028832 File Offset: 0x00027832
		public override bool MoveToNextContent(XPathNavigator navigator)
		{
			return navigator.MoveToNext();
		}

		// Token: 0x06000815 RID: 2069 RVA: 0x0002883A File Offset: 0x0002783A
		public override bool MoveToFollowingSibling(XPathNavigator navigator)
		{
			return navigator.MoveToNext();
		}

		// Token: 0x06000816 RID: 2070 RVA: 0x00028842 File Offset: 0x00027842
		public override bool MoveToPreviousSibling(XPathNavigator navigator)
		{
			return navigator.MoveToPrevious();
		}

		// Token: 0x06000817 RID: 2071 RVA: 0x0002884A File Offset: 0x0002784A
		public override bool MoveToFollowing(XPathNavigator navigator, XPathNavigator navEnd)
		{
			return navigator.MoveToFollowing(XPathNodeType.All, navEnd);
		}

		// Token: 0x06000818 RID: 2072 RVA: 0x00028855 File Offset: 0x00027855
		public override bool IsFiltered(XPathNavigator navigator)
		{
			return navigator.NodeType == XPathNodeType.Attribute;
		}

		// Token: 0x0400056E RID: 1390
		private static XmlNavigatorFilter Singleton = new XmlNavAttrFilter();
	}
}
