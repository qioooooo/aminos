using System;
using System.Xml.Serialization;

namespace System.Xml.Schema
{
	// Token: 0x02000271 RID: 625
	public class XmlSchemaRedefine : XmlSchemaExternal
	{
		// Token: 0x06001CE6 RID: 7398 RVA: 0x00083CCE File Offset: 0x00082CCE
		public XmlSchemaRedefine()
		{
			base.Compositor = Compositor.Redefine;
		}

		// Token: 0x17000776 RID: 1910
		// (get) Token: 0x06001CE7 RID: 7399 RVA: 0x00083D09 File Offset: 0x00082D09
		[XmlElement("complexType", typeof(XmlSchemaComplexType))]
		[XmlElement("simpleType", typeof(XmlSchemaSimpleType))]
		[XmlElement("group", typeof(XmlSchemaGroup))]
		[XmlElement("annotation", typeof(XmlSchemaAnnotation))]
		[XmlElement("attributeGroup", typeof(XmlSchemaAttributeGroup))]
		public XmlSchemaObjectCollection Items
		{
			get
			{
				return this.items;
			}
		}

		// Token: 0x17000777 RID: 1911
		// (get) Token: 0x06001CE8 RID: 7400 RVA: 0x00083D11 File Offset: 0x00082D11
		[XmlIgnore]
		public XmlSchemaObjectTable AttributeGroups
		{
			get
			{
				return this.attributeGroups;
			}
		}

		// Token: 0x17000778 RID: 1912
		// (get) Token: 0x06001CE9 RID: 7401 RVA: 0x00083D19 File Offset: 0x00082D19
		[XmlIgnore]
		public XmlSchemaObjectTable SchemaTypes
		{
			get
			{
				return this.types;
			}
		}

		// Token: 0x17000779 RID: 1913
		// (get) Token: 0x06001CEA RID: 7402 RVA: 0x00083D21 File Offset: 0x00082D21
		[XmlIgnore]
		public XmlSchemaObjectTable Groups
		{
			get
			{
				return this.groups;
			}
		}

		// Token: 0x06001CEB RID: 7403 RVA: 0x00083D29 File Offset: 0x00082D29
		internal override void AddAnnotation(XmlSchemaAnnotation annotation)
		{
			this.items.Add(annotation);
		}

		// Token: 0x040011B5 RID: 4533
		private XmlSchemaObjectCollection items = new XmlSchemaObjectCollection();

		// Token: 0x040011B6 RID: 4534
		private XmlSchemaObjectTable attributeGroups = new XmlSchemaObjectTable();

		// Token: 0x040011B7 RID: 4535
		private XmlSchemaObjectTable types = new XmlSchemaObjectTable();

		// Token: 0x040011B8 RID: 4536
		private XmlSchemaObjectTable groups = new XmlSchemaObjectTable();
	}
}
