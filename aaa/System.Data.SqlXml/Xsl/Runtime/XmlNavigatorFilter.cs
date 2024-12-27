using System;
using System.ComponentModel;
using System.Xml.XPath;

namespace System.Xml.Xsl.Runtime
{
	// Token: 0x020000AB RID: 171
	[EditorBrowsable(EditorBrowsableState.Never)]
	public abstract class XmlNavigatorFilter
	{
		// Token: 0x060007F9 RID: 2041
		public abstract bool MoveToContent(XPathNavigator navigator);

		// Token: 0x060007FA RID: 2042
		public abstract bool MoveToNextContent(XPathNavigator navigator);

		// Token: 0x060007FB RID: 2043
		public abstract bool MoveToFollowingSibling(XPathNavigator navigator);

		// Token: 0x060007FC RID: 2044
		public abstract bool MoveToPreviousSibling(XPathNavigator navigator);

		// Token: 0x060007FD RID: 2045
		public abstract bool MoveToFollowing(XPathNavigator navigator, XPathNavigator navigatorEnd);

		// Token: 0x060007FE RID: 2046
		public abstract bool IsFiltered(XPathNavigator navigator);
	}
}
