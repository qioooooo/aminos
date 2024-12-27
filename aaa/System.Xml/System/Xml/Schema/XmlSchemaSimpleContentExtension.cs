using System;
using System.Xml.Serialization;

namespace System.Xml.Schema
{
	// Token: 0x02000276 RID: 630
	public class XmlSchemaSimpleContentExtension : XmlSchemaContent
	{
		// Token: 0x1700078E RID: 1934
		// (get) Token: 0x06001D30 RID: 7472 RVA: 0x00085C1E File Offset: 0x00084C1E
		// (set) Token: 0x06001D31 RID: 7473 RVA: 0x00085C26 File Offset: 0x00084C26
		[XmlAttribute("base")]
		public XmlQualifiedName BaseTypeName
		{
			get
			{
				return this.baseTypeName;
			}
			set
			{
				this.baseTypeName = ((value == null) ? XmlQualifiedName.Empty : value);
			}
		}

		// Token: 0x1700078F RID: 1935
		// (get) Token: 0x06001D32 RID: 7474 RVA: 0x00085C3F File Offset: 0x00084C3F
		[XmlElement("attribute", typeof(XmlSchemaAttribute))]
		[XmlElement("attributeGroup", typeof(XmlSchemaAttributeGroupRef))]
		public XmlSchemaObjectCollection Attributes
		{
			get
			{
				return this.attributes;
			}
		}

		// Token: 0x17000790 RID: 1936
		// (get) Token: 0x06001D33 RID: 7475 RVA: 0x00085C47 File Offset: 0x00084C47
		// (set) Token: 0x06001D34 RID: 7476 RVA: 0x00085C4F File Offset: 0x00084C4F
		[XmlElement("anyAttribute")]
		public XmlSchemaAnyAttribute AnyAttribute
		{
			get
			{
				return this.anyAttribute;
			}
			set
			{
				this.anyAttribute = value;
			}
		}

		// Token: 0x06001D35 RID: 7477 RVA: 0x00085C58 File Offset: 0x00084C58
		internal void SetAttributes(XmlSchemaObjectCollection newAttributes)
		{
			this.attributes = newAttributes;
		}

		// Token: 0x040011D0 RID: 4560
		private XmlSchemaObjectCollection attributes = new XmlSchemaObjectCollection();

		// Token: 0x040011D1 RID: 4561
		private XmlSchemaAnyAttribute anyAttribute;

		// Token: 0x040011D2 RID: 4562
		private XmlQualifiedName baseTypeName = XmlQualifiedName.Empty;
	}
}
