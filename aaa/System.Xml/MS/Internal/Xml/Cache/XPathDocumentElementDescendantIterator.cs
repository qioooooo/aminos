using System;
using System.Xml.XPath;

namespace MS.Internal.Xml.Cache
{
	// Token: 0x02000106 RID: 262
	internal class XPathDocumentElementDescendantIterator : XPathDocumentBaseIterator
	{
		// Token: 0x06000FF6 RID: 4086 RVA: 0x000495BC File Offset: 0x000485BC
		public XPathDocumentElementDescendantIterator(XPathDocumentNavigator root, string name, string namespaceURI, bool matchSelf)
			: base(root)
		{
			if (namespaceURI == null)
			{
				throw new ArgumentNullException("namespaceURI");
			}
			this.localName = root.NameTable.Get(name);
			this.namespaceUri = namespaceURI;
			this.matchSelf = matchSelf;
			if (root.NodeType != XPathNodeType.Root)
			{
				this.end = new XPathDocumentNavigator(root);
				this.end.MoveToNonDescendant();
			}
		}

		// Token: 0x06000FF7 RID: 4087 RVA: 0x0004961F File Offset: 0x0004861F
		public XPathDocumentElementDescendantIterator(XPathDocumentElementDescendantIterator iter)
			: base(iter)
		{
			this.end = iter.end;
			this.localName = iter.localName;
			this.namespaceUri = iter.namespaceUri;
			this.matchSelf = iter.matchSelf;
		}

		// Token: 0x06000FF8 RID: 4088 RVA: 0x00049658 File Offset: 0x00048658
		public override XPathNodeIterator Clone()
		{
			return new XPathDocumentElementDescendantIterator(this);
		}

		// Token: 0x06000FF9 RID: 4089 RVA: 0x00049660 File Offset: 0x00048660
		public override bool MoveNext()
		{
			if (this.matchSelf)
			{
				this.matchSelf = false;
				if (this.ctxt.IsElementMatch(this.localName, this.namespaceUri))
				{
					this.pos++;
					return true;
				}
			}
			if (!this.ctxt.MoveToFollowing(this.localName, this.namespaceUri, this.end))
			{
				return false;
			}
			this.pos++;
			return true;
		}

		// Token: 0x04000A90 RID: 2704
		private XPathDocumentNavigator end;

		// Token: 0x04000A91 RID: 2705
		private string localName;

		// Token: 0x04000A92 RID: 2706
		private string namespaceUri;

		// Token: 0x04000A93 RID: 2707
		private bool matchSelf;
	}
}
