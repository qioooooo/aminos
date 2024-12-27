using System;
using System.Xml.XPath;

namespace System.Xml
{
	// Token: 0x020000C6 RID: 198
	internal sealed class DocumentXPathNodeIterator_ElemChildren_AndSelf : DocumentXPathNodeIterator_ElemChildren
	{
		// Token: 0x06000B81 RID: 2945 RVA: 0x0003507E File Offset: 0x0003407E
		internal DocumentXPathNodeIterator_ElemChildren_AndSelf(DocumentXPathNavigator nav, string localNameAtom, string nsAtom)
			: base(nav, localNameAtom, nsAtom)
		{
		}

		// Token: 0x06000B82 RID: 2946 RVA: 0x00035089 File Offset: 0x00034089
		internal DocumentXPathNodeIterator_ElemChildren_AndSelf(DocumentXPathNodeIterator_ElemChildren_AndSelf other)
			: base(other)
		{
		}

		// Token: 0x06000B83 RID: 2947 RVA: 0x00035092 File Offset: 0x00034092
		public override XPathNodeIterator Clone()
		{
			return new DocumentXPathNodeIterator_ElemChildren_AndSelf(this);
		}

		// Token: 0x06000B84 RID: 2948 RVA: 0x0003509C File Offset: 0x0003409C
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
