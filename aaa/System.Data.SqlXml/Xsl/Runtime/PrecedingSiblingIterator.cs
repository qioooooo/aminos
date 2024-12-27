using System;
using System.ComponentModel;
using System.Xml.XPath;

namespace System.Xml.Xsl.Runtime
{
	// Token: 0x02000088 RID: 136
	[EditorBrowsable(EditorBrowsableState.Never)]
	public struct PrecedingSiblingIterator
	{
		// Token: 0x06000734 RID: 1844 RVA: 0x00025C42 File Offset: 0x00024C42
		public void Create(XPathNavigator context, XmlNavigatorFilter filter)
		{
			this.navCurrent = XmlQueryRuntime.SyncToNavigator(this.navCurrent, context);
			this.filter = filter;
		}

		// Token: 0x06000735 RID: 1845 RVA: 0x00025C5D File Offset: 0x00024C5D
		public bool MoveNext()
		{
			return this.filter.MoveToPreviousSibling(this.navCurrent);
		}

		// Token: 0x17000109 RID: 265
		// (get) Token: 0x06000736 RID: 1846 RVA: 0x00025C70 File Offset: 0x00024C70
		public XPathNavigator Current
		{
			get
			{
				return this.navCurrent;
			}
		}

		// Token: 0x040004C4 RID: 1220
		private XmlNavigatorFilter filter;

		// Token: 0x040004C5 RID: 1221
		private XPathNavigator navCurrent;
	}
}
