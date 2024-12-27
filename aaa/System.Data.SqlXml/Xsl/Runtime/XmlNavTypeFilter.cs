using System;
using System.Xml.XPath;

namespace System.Xml.Xsl.Runtime
{
	// Token: 0x020000AD RID: 173
	internal class XmlNavTypeFilter : XmlNavigatorFilter
	{
		// Token: 0x06000808 RID: 2056 RVA: 0x0002874C File Offset: 0x0002774C
		static XmlNavTypeFilter()
		{
			XmlNavTypeFilter.TypeFilters[1] = new XmlNavTypeFilter(XPathNodeType.Element);
			XmlNavTypeFilter.TypeFilters[4] = new XmlNavTypeFilter(XPathNodeType.Text);
			XmlNavTypeFilter.TypeFilters[7] = new XmlNavTypeFilter(XPathNodeType.ProcessingInstruction);
			XmlNavTypeFilter.TypeFilters[8] = new XmlNavTypeFilter(XPathNodeType.Comment);
		}

		// Token: 0x06000809 RID: 2057 RVA: 0x00028799 File Offset: 0x00027799
		public static XmlNavigatorFilter Create(XPathNodeType nodeType)
		{
			return XmlNavTypeFilter.TypeFilters[(int)nodeType];
		}

		// Token: 0x0600080A RID: 2058 RVA: 0x000287A2 File Offset: 0x000277A2
		private XmlNavTypeFilter(XPathNodeType nodeType)
		{
			this.nodeType = nodeType;
			this.mask = XPathNavigator.GetContentKindMask(nodeType);
		}

		// Token: 0x0600080B RID: 2059 RVA: 0x000287BD File Offset: 0x000277BD
		public override bool MoveToContent(XPathNavigator navigator)
		{
			return navigator.MoveToChild(this.nodeType);
		}

		// Token: 0x0600080C RID: 2060 RVA: 0x000287CB File Offset: 0x000277CB
		public override bool MoveToNextContent(XPathNavigator navigator)
		{
			return navigator.MoveToNext(this.nodeType);
		}

		// Token: 0x0600080D RID: 2061 RVA: 0x000287D9 File Offset: 0x000277D9
		public override bool MoveToFollowingSibling(XPathNavigator navigator)
		{
			return navigator.MoveToNext(this.nodeType);
		}

		// Token: 0x0600080E RID: 2062 RVA: 0x000287E7 File Offset: 0x000277E7
		public override bool MoveToPreviousSibling(XPathNavigator navigator)
		{
			return navigator.MoveToPrevious(this.nodeType);
		}

		// Token: 0x0600080F RID: 2063 RVA: 0x000287F5 File Offset: 0x000277F5
		public override bool MoveToFollowing(XPathNavigator navigator, XPathNavigator navEnd)
		{
			return navigator.MoveToFollowing(this.nodeType, navEnd);
		}

		// Token: 0x06000810 RID: 2064 RVA: 0x00028804 File Offset: 0x00027804
		public override bool IsFiltered(XPathNavigator navigator)
		{
			return ((1 << (int)navigator.NodeType) & this.mask) == 0;
		}

		// Token: 0x0400056B RID: 1387
		private static XmlNavigatorFilter[] TypeFilters = new XmlNavigatorFilter[9];

		// Token: 0x0400056C RID: 1388
		private XPathNodeType nodeType;

		// Token: 0x0400056D RID: 1389
		private int mask;
	}
}
