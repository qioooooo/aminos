using System;
using System.ComponentModel;
using System.Xml.XPath;

namespace System.Xml.Xsl.Runtime
{
	// Token: 0x02000087 RID: 135
	[EditorBrowsable(EditorBrowsableState.Never)]
	public struct FollowingSiblingMergeIterator
	{
		// Token: 0x06000731 RID: 1841 RVA: 0x00025C18 File Offset: 0x00024C18
		public void Create(XmlNavigatorFilter filter)
		{
			this.wrapped.Create(filter);
		}

		// Token: 0x06000732 RID: 1842 RVA: 0x00025C26 File Offset: 0x00024C26
		public IteratorResult MoveNext(XPathNavigator navigator)
		{
			return this.wrapped.MoveNext(navigator, false);
		}

		// Token: 0x17000108 RID: 264
		// (get) Token: 0x06000733 RID: 1843 RVA: 0x00025C35 File Offset: 0x00024C35
		public XPathNavigator Current
		{
			get
			{
				return this.wrapped.Current;
			}
		}

		// Token: 0x040004C3 RID: 1219
		private ContentMergeIterator wrapped;
	}
}
