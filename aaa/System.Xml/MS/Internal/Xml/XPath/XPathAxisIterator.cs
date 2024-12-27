using System;
using System.Xml.XPath;

namespace MS.Internal.Xml.XPath
{
	// Token: 0x02000162 RID: 354
	internal abstract class XPathAxisIterator : XPathNodeIterator
	{
		// Token: 0x06001326 RID: 4902 RVA: 0x00053160 File Offset: 0x00052160
		public XPathAxisIterator(XPathNavigator nav, bool matchSelf)
		{
			this.nav = nav;
			this.matchSelf = matchSelf;
		}

		// Token: 0x06001327 RID: 4903 RVA: 0x0005317D File Offset: 0x0005217D
		public XPathAxisIterator(XPathNavigator nav, XPathNodeType type, bool matchSelf)
			: this(nav, matchSelf)
		{
			this.type = type;
		}

		// Token: 0x06001328 RID: 4904 RVA: 0x0005318E File Offset: 0x0005218E
		public XPathAxisIterator(XPathNavigator nav, string name, string namespaceURI, bool matchSelf)
			: this(nav, matchSelf)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			if (namespaceURI == null)
			{
				throw new ArgumentNullException("namespaceURI");
			}
			this.name = name;
			this.uri = namespaceURI;
		}

		// Token: 0x06001329 RID: 4905 RVA: 0x000531C4 File Offset: 0x000521C4
		public XPathAxisIterator(XPathAxisIterator it)
		{
			this.nav = it.nav.Clone();
			this.type = it.type;
			this.name = it.name;
			this.uri = it.uri;
			this.position = it.position;
			this.matchSelf = it.matchSelf;
			this.first = it.first;
		}

		// Token: 0x170004A8 RID: 1192
		// (get) Token: 0x0600132A RID: 4906 RVA: 0x00053237 File Offset: 0x00052237
		public override XPathNavigator Current
		{
			get
			{
				return this.nav;
			}
		}

		// Token: 0x170004A9 RID: 1193
		// (get) Token: 0x0600132B RID: 4907 RVA: 0x0005323F File Offset: 0x0005223F
		public override int CurrentPosition
		{
			get
			{
				return this.position;
			}
		}

		// Token: 0x170004AA RID: 1194
		// (get) Token: 0x0600132C RID: 4908 RVA: 0x00053248 File Offset: 0x00052248
		protected virtual bool Matches
		{
			get
			{
				if (this.name == null)
				{
					return this.type == this.nav.NodeType || this.type == XPathNodeType.All || (this.type == XPathNodeType.Text && (this.nav.NodeType == XPathNodeType.Whitespace || this.nav.NodeType == XPathNodeType.SignificantWhitespace));
				}
				return this.nav.NodeType == XPathNodeType.Element && (this.name.Length == 0 || this.name == this.nav.LocalName) && this.uri == this.nav.NamespaceURI;
			}
		}

		// Token: 0x04000BE4 RID: 3044
		internal XPathNavigator nav;

		// Token: 0x04000BE5 RID: 3045
		internal XPathNodeType type;

		// Token: 0x04000BE6 RID: 3046
		internal string name;

		// Token: 0x04000BE7 RID: 3047
		internal string uri;

		// Token: 0x04000BE8 RID: 3048
		internal int position;

		// Token: 0x04000BE9 RID: 3049
		internal bool matchSelf;

		// Token: 0x04000BEA RID: 3050
		internal bool first = true;
	}
}
