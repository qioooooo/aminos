using System;
using System.Xml.XPath;

namespace System.Xml.Xsl.Runtime
{
	// Token: 0x020000AC RID: 172
	internal class XmlNavNameFilter : XmlNavigatorFilter
	{
		// Token: 0x06000800 RID: 2048 RVA: 0x0002869D File Offset: 0x0002769D
		public static XmlNavigatorFilter Create(string localName, string namespaceUri)
		{
			return new XmlNavNameFilter(localName, namespaceUri);
		}

		// Token: 0x06000801 RID: 2049 RVA: 0x000286A6 File Offset: 0x000276A6
		private XmlNavNameFilter(string localName, string namespaceUri)
		{
			this.localName = localName;
			this.namespaceUri = namespaceUri;
		}

		// Token: 0x06000802 RID: 2050 RVA: 0x000286BC File Offset: 0x000276BC
		public override bool MoveToContent(XPathNavigator navigator)
		{
			return navigator.MoveToChild(this.localName, this.namespaceUri);
		}

		// Token: 0x06000803 RID: 2051 RVA: 0x000286D0 File Offset: 0x000276D0
		public override bool MoveToNextContent(XPathNavigator navigator)
		{
			return navigator.MoveToNext(this.localName, this.namespaceUri);
		}

		// Token: 0x06000804 RID: 2052 RVA: 0x000286E4 File Offset: 0x000276E4
		public override bool MoveToFollowingSibling(XPathNavigator navigator)
		{
			return navigator.MoveToNext(this.localName, this.namespaceUri);
		}

		// Token: 0x06000805 RID: 2053 RVA: 0x000286F8 File Offset: 0x000276F8
		public override bool MoveToPreviousSibling(XPathNavigator navigator)
		{
			return navigator.MoveToPrevious(this.localName, this.namespaceUri);
		}

		// Token: 0x06000806 RID: 2054 RVA: 0x0002870C File Offset: 0x0002770C
		public override bool MoveToFollowing(XPathNavigator navigator, XPathNavigator navEnd)
		{
			return navigator.MoveToFollowing(this.localName, this.namespaceUri, navEnd);
		}

		// Token: 0x06000807 RID: 2055 RVA: 0x00028721 File Offset: 0x00027721
		public override bool IsFiltered(XPathNavigator navigator)
		{
			return navigator.LocalName != this.localName || navigator.NamespaceURI != this.namespaceUri;
		}

		// Token: 0x04000569 RID: 1385
		private string localName;

		// Token: 0x0400056A RID: 1386
		private string namespaceUri;
	}
}
