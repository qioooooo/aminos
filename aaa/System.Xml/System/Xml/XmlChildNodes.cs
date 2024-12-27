using System;
using System.Collections;

namespace System.Xml
{
	// Token: 0x020000D1 RID: 209
	internal class XmlChildNodes : XmlNodeList
	{
		// Token: 0x06000C60 RID: 3168 RVA: 0x00037FBB File Offset: 0x00036FBB
		public XmlChildNodes(XmlNode container)
		{
			this.container = container;
		}

		// Token: 0x06000C61 RID: 3169 RVA: 0x00037FCC File Offset: 0x00036FCC
		public override XmlNode Item(int i)
		{
			if (i < 0)
			{
				return null;
			}
			XmlNode xmlNode = this.container.FirstChild;
			while (xmlNode != null)
			{
				if (i == 0)
				{
					return xmlNode;
				}
				xmlNode = xmlNode.NextSibling;
				i--;
			}
			return null;
		}

		// Token: 0x170002CE RID: 718
		// (get) Token: 0x06000C62 RID: 3170 RVA: 0x00038004 File Offset: 0x00037004
		public override int Count
		{
			get
			{
				int num = 0;
				for (XmlNode xmlNode = this.container.FirstChild; xmlNode != null; xmlNode = xmlNode.NextSibling)
				{
					num++;
				}
				return num;
			}
		}

		// Token: 0x06000C63 RID: 3171 RVA: 0x00038030 File Offset: 0x00037030
		public override IEnumerator GetEnumerator()
		{
			if (this.container.FirstChild == null)
			{
				return XmlDocument.EmptyEnumerator;
			}
			return new XmlChildEnumerator(this.container);
		}

		// Token: 0x040008F5 RID: 2293
		private XmlNode container;
	}
}
