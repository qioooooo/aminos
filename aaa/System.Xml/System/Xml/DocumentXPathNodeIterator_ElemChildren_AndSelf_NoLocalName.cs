using System;
using System.Xml.XPath;

namespace System.Xml
{
	// Token: 0x020000C4 RID: 196
	internal sealed class DocumentXPathNodeIterator_ElemChildren_AndSelf_NoLocalName : DocumentXPathNodeIterator_ElemChildren_NoLocalName
	{
		// Token: 0x06000B79 RID: 2937 RVA: 0x00034FAA File Offset: 0x00033FAA
		internal DocumentXPathNodeIterator_ElemChildren_AndSelf_NoLocalName(DocumentXPathNavigator nav, string nsAtom)
			: base(nav, nsAtom)
		{
		}

		// Token: 0x06000B7A RID: 2938 RVA: 0x00034FB4 File Offset: 0x00033FB4
		internal DocumentXPathNodeIterator_ElemChildren_AndSelf_NoLocalName(DocumentXPathNodeIterator_ElemChildren_AndSelf_NoLocalName other)
			: base(other)
		{
		}

		// Token: 0x06000B7B RID: 2939 RVA: 0x00034FBD File Offset: 0x00033FBD
		public override XPathNodeIterator Clone()
		{
			return new DocumentXPathNodeIterator_ElemChildren_AndSelf_NoLocalName(this);
		}

		// Token: 0x06000B7C RID: 2940 RVA: 0x00034FC8 File Offset: 0x00033FC8
		public override bool MoveNext()
		{
			if (this.CurrentPosition == 0)
			{
				DocumentXPathNavigator documentXPathNavigator = (DocumentXPathNavigator)this.Current;
				XmlNode xmlNode = (XmlNode)documentXPathNavigator.UnderlyingObject;
				if (xmlNode.NodeType == XmlNodeType.Element && this.Match(xmlNode))
				{
					base.SetPosition(1);
					return true;
				}
			}
			return base.MoveNext();
		}
	}
}
