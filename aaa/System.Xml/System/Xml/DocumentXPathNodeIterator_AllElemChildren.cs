using System;
using System.Xml.XPath;

namespace System.Xml
{
	// Token: 0x020000C1 RID: 193
	internal class DocumentXPathNodeIterator_AllElemChildren : DocumentXPathNodeIterator_ElemDescendants
	{
		// Token: 0x06000B6D RID: 2925 RVA: 0x00034EDA File Offset: 0x00033EDA
		internal DocumentXPathNodeIterator_AllElemChildren(DocumentXPathNavigator nav)
			: base(nav)
		{
		}

		// Token: 0x06000B6E RID: 2926 RVA: 0x00034EE3 File Offset: 0x00033EE3
		internal DocumentXPathNodeIterator_AllElemChildren(DocumentXPathNodeIterator_AllElemChildren other)
			: base(other)
		{
		}

		// Token: 0x06000B6F RID: 2927 RVA: 0x00034EEC File Offset: 0x00033EEC
		public override XPathNodeIterator Clone()
		{
			return new DocumentXPathNodeIterator_AllElemChildren(this);
		}

		// Token: 0x06000B70 RID: 2928 RVA: 0x00034EF4 File Offset: 0x00033EF4
		protected override bool Match(XmlNode node)
		{
			return node.NodeType == XmlNodeType.Element;
		}
	}
}
