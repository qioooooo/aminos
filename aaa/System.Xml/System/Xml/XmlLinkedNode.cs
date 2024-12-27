using System;

namespace System.Xml
{
	// Token: 0x020000CC RID: 204
	public abstract class XmlLinkedNode : XmlNode
	{
		// Token: 0x06000C37 RID: 3127 RVA: 0x000379FA File Offset: 0x000369FA
		internal XmlLinkedNode()
		{
			this.next = null;
		}

		// Token: 0x06000C38 RID: 3128 RVA: 0x00037A09 File Offset: 0x00036A09
		internal XmlLinkedNode(XmlDocument doc)
			: base(doc)
		{
			this.next = null;
		}

		// Token: 0x170002BD RID: 701
		// (get) Token: 0x06000C39 RID: 3129 RVA: 0x00037A1C File Offset: 0x00036A1C
		public override XmlNode PreviousSibling
		{
			get
			{
				XmlNode parentNode = this.ParentNode;
				if (parentNode != null)
				{
					XmlNode xmlNode;
					XmlNode nextSibling;
					for (xmlNode = parentNode.FirstChild; xmlNode != null; xmlNode = nextSibling)
					{
						nextSibling = xmlNode.NextSibling;
						if (nextSibling == this)
						{
							break;
						}
					}
					return xmlNode;
				}
				return null;
			}
		}

		// Token: 0x170002BE RID: 702
		// (get) Token: 0x06000C3A RID: 3130 RVA: 0x00037A50 File Offset: 0x00036A50
		public override XmlNode NextSibling
		{
			get
			{
				XmlNode parentNode = this.ParentNode;
				if (parentNode != null && this.next != parentNode.FirstChild)
				{
					return this.next;
				}
				return null;
			}
		}

		// Token: 0x040008F0 RID: 2288
		internal XmlLinkedNode next;
	}
}
