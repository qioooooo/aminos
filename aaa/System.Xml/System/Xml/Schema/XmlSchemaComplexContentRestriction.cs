using System;
using System.Xml.Serialization;

namespace System.Xml.Schema
{
	// Token: 0x02000243 RID: 579
	public class XmlSchemaComplexContentRestriction : XmlSchemaContent
	{
		// Token: 0x170006EA RID: 1770
		// (get) Token: 0x06001B85 RID: 7045 RVA: 0x00081AE2 File Offset: 0x00080AE2
		// (set) Token: 0x06001B86 RID: 7046 RVA: 0x00081AEA File Offset: 0x00080AEA
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

		// Token: 0x170006EB RID: 1771
		// (get) Token: 0x06001B87 RID: 7047 RVA: 0x00081B03 File Offset: 0x00080B03
		// (set) Token: 0x06001B88 RID: 7048 RVA: 0x00081B0B File Offset: 0x00080B0B
		[XmlElement("sequence", typeof(XmlSchemaSequence))]
		[XmlElement("group", typeof(XmlSchemaGroupRef))]
		[XmlElement("choice", typeof(XmlSchemaChoice))]
		[XmlElement("all", typeof(XmlSchemaAll))]
		public XmlSchemaParticle Particle
		{
			get
			{
				return this.particle;
			}
			set
			{
				this.particle = value;
			}
		}

		// Token: 0x170006EC RID: 1772
		// (get) Token: 0x06001B89 RID: 7049 RVA: 0x00081B14 File Offset: 0x00080B14
		[XmlElement("attribute", typeof(XmlSchemaAttribute))]
		[XmlElement("attributeGroup", typeof(XmlSchemaAttributeGroupRef))]
		public XmlSchemaObjectCollection Attributes
		{
			get
			{
				return this.attributes;
			}
		}

		// Token: 0x170006ED RID: 1773
		// (get) Token: 0x06001B8A RID: 7050 RVA: 0x00081B1C File Offset: 0x00080B1C
		// (set) Token: 0x06001B8B RID: 7051 RVA: 0x00081B24 File Offset: 0x00080B24
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

		// Token: 0x06001B8C RID: 7052 RVA: 0x00081B2D File Offset: 0x00080B2D
		internal void SetAttributes(XmlSchemaObjectCollection newAttributes)
		{
			this.attributes = newAttributes;
		}

		// Token: 0x04001114 RID: 4372
		private XmlSchemaParticle particle;

		// Token: 0x04001115 RID: 4373
		private XmlSchemaObjectCollection attributes = new XmlSchemaObjectCollection();

		// Token: 0x04001116 RID: 4374
		private XmlSchemaAnyAttribute anyAttribute;

		// Token: 0x04001117 RID: 4375
		private XmlQualifiedName baseTypeName = XmlQualifiedName.Empty;
	}
}
