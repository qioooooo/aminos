using System;
using System.Xml.XPath;

namespace System.Xml
{
	// Token: 0x020000C3 RID: 195
	internal class DocumentXPathNodeIterator_ElemChildren_NoLocalName : DocumentXPathNodeIterator_ElemDescendants
	{
		// Token: 0x06000B75 RID: 2933 RVA: 0x00034F6A File Offset: 0x00033F6A
		internal DocumentXPathNodeIterator_ElemChildren_NoLocalName(DocumentXPathNavigator nav, string nsAtom)
			: base(nav)
		{
			this.nsAtom = nsAtom;
		}

		// Token: 0x06000B76 RID: 2934 RVA: 0x00034F7A File Offset: 0x00033F7A
		internal DocumentXPathNodeIterator_ElemChildren_NoLocalName(DocumentXPathNodeIterator_ElemChildren_NoLocalName other)
			: base(other)
		{
			this.nsAtom = other.nsAtom;
		}

		// Token: 0x06000B77 RID: 2935 RVA: 0x00034F8F File Offset: 0x00033F8F
		public override XPathNodeIterator Clone()
		{
			return new DocumentXPathNodeIterator_ElemChildren_NoLocalName(this);
		}

		// Token: 0x06000B78 RID: 2936 RVA: 0x00034F97 File Offset: 0x00033F97
		protected override bool Match(XmlNode node)
		{
			return Ref.Equal(node.NamespaceURI, this.nsAtom);
		}

		// Token: 0x040008E2 RID: 2274
		private string nsAtom;
	}
}
