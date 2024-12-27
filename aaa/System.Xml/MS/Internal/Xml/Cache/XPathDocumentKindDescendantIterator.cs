using System;
using System.Xml.XPath;

namespace MS.Internal.Xml.Cache
{
	// Token: 0x02000107 RID: 263
	internal class XPathDocumentKindDescendantIterator : XPathDocumentBaseIterator
	{
		// Token: 0x06000FFA RID: 4090 RVA: 0x000496D5 File Offset: 0x000486D5
		public XPathDocumentKindDescendantIterator(XPathDocumentNavigator root, XPathNodeType typ, bool matchSelf)
			: base(root)
		{
			this.typ = typ;
			this.matchSelf = matchSelf;
			if (root.NodeType != XPathNodeType.Root)
			{
				this.end = new XPathDocumentNavigator(root);
				this.end.MoveToNonDescendant();
			}
		}

		// Token: 0x06000FFB RID: 4091 RVA: 0x0004970C File Offset: 0x0004870C
		public XPathDocumentKindDescendantIterator(XPathDocumentKindDescendantIterator iter)
			: base(iter)
		{
			this.end = iter.end;
			this.typ = iter.typ;
			this.matchSelf = iter.matchSelf;
		}

		// Token: 0x06000FFC RID: 4092 RVA: 0x00049739 File Offset: 0x00048739
		public override XPathNodeIterator Clone()
		{
			return new XPathDocumentKindDescendantIterator(this);
		}

		// Token: 0x06000FFD RID: 4093 RVA: 0x00049744 File Offset: 0x00048744
		public override bool MoveNext()
		{
			if (this.matchSelf)
			{
				this.matchSelf = false;
				if (this.ctxt.IsKindMatch(this.typ))
				{
					this.pos++;
					return true;
				}
			}
			if (!this.ctxt.MoveToFollowing(this.typ, this.end))
			{
				return false;
			}
			this.pos++;
			return true;
		}

		// Token: 0x04000A94 RID: 2708
		private XPathDocumentNavigator end;

		// Token: 0x04000A95 RID: 2709
		private XPathNodeType typ;

		// Token: 0x04000A96 RID: 2710
		private bool matchSelf;
	}
}
