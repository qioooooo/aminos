using System;
using System.ComponentModel;
using System.Xml.XPath;

namespace System.Xml.Xsl.Runtime
{
	// Token: 0x0200008B RID: 139
	[EditorBrowsable(EditorBrowsableState.Never)]
	public struct DescendantIterator
	{
		// Token: 0x06000741 RID: 1857 RVA: 0x00025F14 File Offset: 0x00024F14
		public void Create(XPathNavigator input, XmlNavigatorFilter filter, bool orSelf)
		{
			this.navCurrent = XmlQueryRuntime.SyncToNavigator(this.navCurrent, input);
			this.filter = filter;
			if (input.NodeType == XPathNodeType.Root)
			{
				this.navEnd = null;
			}
			else
			{
				this.navEnd = XmlQueryRuntime.SyncToNavigator(this.navEnd, input);
				this.navEnd.MoveToNonDescendant();
			}
			this.hasFirst = orSelf && !this.filter.IsFiltered(this.navCurrent);
		}

		// Token: 0x06000742 RID: 1858 RVA: 0x00025F89 File Offset: 0x00024F89
		public bool MoveNext()
		{
			if (this.hasFirst)
			{
				this.hasFirst = false;
				return true;
			}
			return this.filter.MoveToFollowing(this.navCurrent, this.navEnd);
		}

		// Token: 0x1700010D RID: 269
		// (get) Token: 0x06000743 RID: 1859 RVA: 0x00025FB3 File Offset: 0x00024FB3
		public XPathNavigator Current
		{
			get
			{
				return this.navCurrent;
			}
		}

		// Token: 0x040004D2 RID: 1234
		private XmlNavigatorFilter filter;

		// Token: 0x040004D3 RID: 1235
		private XPathNavigator navCurrent;

		// Token: 0x040004D4 RID: 1236
		private XPathNavigator navEnd;

		// Token: 0x040004D5 RID: 1237
		private bool hasFirst;
	}
}
