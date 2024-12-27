using System;
using System.ComponentModel;
using System.Xml;

namespace System.DirectoryServices.Protocols
{
	// Token: 0x0200000B RID: 11
	public class DirectoryAttributeModification : DirectoryAttribute
	{
		// Token: 0x17000007 RID: 7
		// (get) Token: 0x06000033 RID: 51 RVA: 0x000035C9 File Offset: 0x000025C9
		// (set) Token: 0x06000034 RID: 52 RVA: 0x000035D1 File Offset: 0x000025D1
		public DirectoryAttributeOperation Operation
		{
			get
			{
				return this.attributeOperation;
			}
			set
			{
				if (value < DirectoryAttributeOperation.Add || value > DirectoryAttributeOperation.Replace)
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(DirectoryAttributeOperation));
				}
				this.attributeOperation = value;
			}
		}

		// Token: 0x06000035 RID: 53 RVA: 0x000035F8 File Offset: 0x000025F8
		internal XmlElement ToXmlNode(XmlDocument doc)
		{
			XmlElement xmlElement = doc.CreateElement("modification", "urn:oasis:names:tc:DSML:2:0:core");
			base.ToXmlNodeCommon(xmlElement);
			XmlAttribute xmlAttribute = doc.CreateAttribute("operation", null);
			switch (this.Operation)
			{
			case DirectoryAttributeOperation.Add:
				xmlAttribute.InnerText = "add";
				break;
			case DirectoryAttributeOperation.Delete:
				xmlAttribute.InnerText = "delete";
				break;
			case DirectoryAttributeOperation.Replace:
				xmlAttribute.InnerText = "replace";
				break;
			default:
				throw new InvalidEnumArgumentException("Operation", (int)this.Operation, typeof(DirectoryAttributeOperation));
			}
			xmlElement.Attributes.Append(xmlAttribute);
			return xmlElement;
		}

		// Token: 0x040000BE RID: 190
		private DirectoryAttributeOperation attributeOperation = DirectoryAttributeOperation.Replace;
	}
}
