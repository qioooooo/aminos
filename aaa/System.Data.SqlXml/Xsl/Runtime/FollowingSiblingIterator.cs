using System;
using System.ComponentModel;
using System.Xml.XPath;

namespace System.Xml.Xsl.Runtime
{
	// Token: 0x02000086 RID: 134
	[EditorBrowsable(EditorBrowsableState.Never)]
	public struct FollowingSiblingIterator
	{
		// Token: 0x0600072E RID: 1838 RVA: 0x00025BE2 File Offset: 0x00024BE2
		public void Create(XPathNavigator context, XmlNavigatorFilter filter)
		{
			this.navCurrent = XmlQueryRuntime.SyncToNavigator(this.navCurrent, context);
			this.filter = filter;
		}

		// Token: 0x0600072F RID: 1839 RVA: 0x00025BFD File Offset: 0x00024BFD
		public bool MoveNext()
		{
			return this.filter.MoveToFollowingSibling(this.navCurrent);
		}

		// Token: 0x17000107 RID: 263
		// (get) Token: 0x06000730 RID: 1840 RVA: 0x00025C10 File Offset: 0x00024C10
		public XPathNavigator Current
		{
			get
			{
				return this.navCurrent;
			}
		}

		// Token: 0x040004C1 RID: 1217
		private XmlNavigatorFilter filter;

		// Token: 0x040004C2 RID: 1218
		private XPathNavigator navCurrent;
	}
}
