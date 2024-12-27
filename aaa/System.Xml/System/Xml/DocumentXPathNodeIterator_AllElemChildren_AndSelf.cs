using System;
using System.Xml.XPath;

namespace System.Xml
{
	// Token: 0x020000C2 RID: 194
	internal sealed class DocumentXPathNodeIterator_AllElemChildren_AndSelf : DocumentXPathNodeIterator_AllElemChildren
	{
		// Token: 0x06000B71 RID: 2929 RVA: 0x00034EFF File Offset: 0x00033EFF
		internal DocumentXPathNodeIterator_AllElemChildren_AndSelf(DocumentXPathNavigator nav)
			: base(nav)
		{
		}

		// Token: 0x06000B72 RID: 2930 RVA: 0x00034F08 File Offset: 0x00033F08
		internal DocumentXPathNodeIterator_AllElemChildren_AndSelf(DocumentXPathNodeIterator_AllElemChildren_AndSelf other)
			: base(other)
		{
		}

		// Token: 0x06000B73 RID: 2931 RVA: 0x00034F11 File Offset: 0x00033F11
		public override XPathNodeIterator Clone()
		{
			return new DocumentXPathNodeIterator_AllElemChildren_AndSelf(this);
		}

		// Token: 0x06000B74 RID: 2932 RVA: 0x00034F1C File Offset: 0x00033F1C
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
