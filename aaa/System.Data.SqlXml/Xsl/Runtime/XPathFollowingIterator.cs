using System;
using System.ComponentModel;
using System.Xml.XPath;

namespace System.Xml.Xsl.Runtime
{
	// Token: 0x02000091 RID: 145
	[EditorBrowsable(EditorBrowsableState.Never)]
	public struct XPathFollowingIterator
	{
		// Token: 0x06000750 RID: 1872 RVA: 0x000261DB File Offset: 0x000251DB
		public void Create(XPathNavigator input, XmlNavigatorFilter filter)
		{
			this.navCurrent = XmlQueryRuntime.SyncToNavigator(this.navCurrent, input);
			this.filter = filter;
			this.needFirst = true;
		}

		// Token: 0x06000751 RID: 1873 RVA: 0x000261FD File Offset: 0x000251FD
		public bool MoveNext()
		{
			if (!this.needFirst)
			{
				return this.filter.MoveToFollowing(this.navCurrent, null);
			}
			if (!XPathFollowingIterator.MoveFirst(this.filter, this.navCurrent))
			{
				return false;
			}
			this.needFirst = false;
			return true;
		}

		// Token: 0x17000112 RID: 274
		// (get) Token: 0x06000752 RID: 1874 RVA: 0x00026237 File Offset: 0x00025237
		public XPathNavigator Current
		{
			get
			{
				return this.navCurrent;
			}
		}

		// Token: 0x06000753 RID: 1875 RVA: 0x00026240 File Offset: 0x00025240
		internal static bool MoveFirst(XmlNavigatorFilter filter, XPathNavigator nav)
		{
			if (nav.NodeType == XPathNodeType.Attribute || nav.NodeType == XPathNodeType.Namespace)
			{
				if (!nav.MoveToParent())
				{
					return false;
				}
				if (!filter.MoveToFollowing(nav, null))
				{
					return false;
				}
			}
			else
			{
				if (!nav.MoveToNonDescendant())
				{
					return false;
				}
				if (filter.IsFiltered(nav) && !filter.MoveToFollowing(nav, null))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x040004E7 RID: 1255
		private XmlNavigatorFilter filter;

		// Token: 0x040004E8 RID: 1256
		private XPathNavigator navCurrent;

		// Token: 0x040004E9 RID: 1257
		private bool needFirst;
	}
}
