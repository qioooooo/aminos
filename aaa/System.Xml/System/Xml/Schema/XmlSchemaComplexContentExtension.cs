using System;
using System.Xml.Serialization;

namespace System.Xml.Schema
{
	// Token: 0x02000242 RID: 578
	public class XmlSchemaComplexContentExtension : XmlSchemaContent
	{
		// Token: 0x170006E6 RID: 1766
		// (get) Token: 0x06001B7C RID: 7036 RVA: 0x00081A70 File Offset: 0x00080A70
		// (set) Token: 0x06001B7D RID: 7037 RVA: 0x00081A78 File Offset: 0x00080A78
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

		// Token: 0x170006E7 RID: 1767
		// (get) Token: 0x06001B7E RID: 7038 RVA: 0x00081A91 File Offset: 0x00080A91
		// (set) Token: 0x06001B7F RID: 7039 RVA: 0x00081A99 File Offset: 0x00080A99
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

		// Token: 0x170006E8 RID: 1768
		// (get) Token: 0x06001B80 RID: 7040 RVA: 0x00081AA2 File Offset: 0x00080AA2
		[XmlElement("attribute", typeof(XmlSchemaAttribute))]
		[XmlElement("attributeGroup", typeof(XmlSchemaAttributeGroupRef))]
		public XmlSchemaObjectCollection Attributes
		{
			get
			{
				return this.attributes;
			}
		}

		// Token: 0x170006E9 RID: 1769
		// (get) Token: 0x06001B81 RID: 7041 RVA: 0x00081AAA File Offset: 0x00080AAA
		// (set) Token: 0x06001B82 RID: 7042 RVA: 0x00081AB2 File Offset: 0x00080AB2
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

		// Token: 0x06001B83 RID: 7043 RVA: 0x00081ABB File Offset: 0x00080ABB
		internal void SetAttributes(XmlSchemaObjectCollection newAttributes)
		{
			this.attributes = newAttributes;
		}

		// Token: 0x04001110 RID: 4368
		private XmlSchemaParticle particle;

		// Token: 0x04001111 RID: 4369
		private XmlSchemaObjectCollection attributes = new XmlSchemaObjectCollection();

		// Token: 0x04001112 RID: 4370
		private XmlSchemaAnyAttribute anyAttribute;

		// Token: 0x04001113 RID: 4371
		private XmlQualifiedName baseTypeName = XmlQualifiedName.Empty;
	}
}
