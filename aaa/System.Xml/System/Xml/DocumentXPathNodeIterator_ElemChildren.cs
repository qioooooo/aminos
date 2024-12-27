using System;
using System.Xml.XPath;

namespace System.Xml
{
	// Token: 0x020000C5 RID: 197
	internal class DocumentXPathNodeIterator_ElemChildren : DocumentXPathNodeIterator_ElemDescendants
	{
		// Token: 0x06000B7D RID: 2941 RVA: 0x00035016 File Offset: 0x00034016
		internal DocumentXPathNodeIterator_ElemChildren(DocumentXPathNavigator nav, string localNameAtom, string nsAtom)
			: base(nav)
		{
			this.localNameAtom = localNameAtom;
			this.nsAtom = nsAtom;
		}

		// Token: 0x06000B7E RID: 2942 RVA: 0x0003502D File Offset: 0x0003402D
		internal DocumentXPathNodeIterator_ElemChildren(DocumentXPathNodeIterator_ElemChildren other)
			: base(other)
		{
			this.localNameAtom = other.localNameAtom;
			this.nsAtom = other.nsAtom;
		}

		// Token: 0x06000B7F RID: 2943 RVA: 0x0003504E File Offset: 0x0003404E
		public override XPathNodeIterator Clone()
		{
			return new DocumentXPathNodeIterator_ElemChildren(this);
		}

		// Token: 0x06000B80 RID: 2944 RVA: 0x00035056 File Offset: 0x00034056
		protected override bool Match(XmlNode node)
		{
			return Ref.Equal(node.LocalName, this.localNameAtom) && Ref.Equal(node.NamespaceURI, this.nsAtom);
		}

		// Token: 0x040008E3 RID: 2275
		protected string localNameAtom;

		// Token: 0x040008E4 RID: 2276
		protected string nsAtom;
	}
}
