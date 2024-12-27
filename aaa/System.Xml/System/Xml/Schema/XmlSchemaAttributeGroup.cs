using System;
using System.Xml.Serialization;

namespace System.Xml.Schema
{
	// Token: 0x02000239 RID: 569
	public class XmlSchemaAttributeGroup : XmlSchemaAnnotated
	{
		// Token: 0x170006C7 RID: 1735
		// (get) Token: 0x06001B2B RID: 6955 RVA: 0x00081249 File Offset: 0x00080249
		// (set) Token: 0x06001B2C RID: 6956 RVA: 0x00081251 File Offset: 0x00080251
		[XmlAttribute("name")]
		public string Name
		{
			get
			{
				return this.name;
			}
			set
			{
				this.name = value;
			}
		}

		// Token: 0x170006C8 RID: 1736
		// (get) Token: 0x06001B2D RID: 6957 RVA: 0x0008125A File Offset: 0x0008025A
		[XmlElement("attributeGroup", typeof(XmlSchemaAttributeGroupRef))]
		[XmlElement("attribute", typeof(XmlSchemaAttribute))]
		public XmlSchemaObjectCollection Attributes
		{
			get
			{
				return this.attributes;
			}
		}

		// Token: 0x170006C9 RID: 1737
		// (get) Token: 0x06001B2E RID: 6958 RVA: 0x00081262 File Offset: 0x00080262
		// (set) Token: 0x06001B2F RID: 6959 RVA: 0x0008126A File Offset: 0x0008026A
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

		// Token: 0x170006CA RID: 1738
		// (get) Token: 0x06001B30 RID: 6960 RVA: 0x00081273 File Offset: 0x00080273
		[XmlIgnore]
		public XmlQualifiedName QualifiedName
		{
			get
			{
				return this.qname;
			}
		}

		// Token: 0x170006CB RID: 1739
		// (get) Token: 0x06001B31 RID: 6961 RVA: 0x0008127B File Offset: 0x0008027B
		[XmlIgnore]
		internal XmlSchemaObjectTable AttributeUses
		{
			get
			{
				if (this.attributeUses == null)
				{
					this.attributeUses = new XmlSchemaObjectTable();
				}
				return this.attributeUses;
			}
		}

		// Token: 0x170006CC RID: 1740
		// (get) Token: 0x06001B32 RID: 6962 RVA: 0x00081296 File Offset: 0x00080296
		// (set) Token: 0x06001B33 RID: 6963 RVA: 0x0008129E File Offset: 0x0008029E
		[XmlIgnore]
		internal XmlSchemaAnyAttribute AttributeWildcard
		{
			get
			{
				return this.attributeWildcard;
			}
			set
			{
				this.attributeWildcard = value;
			}
		}

		// Token: 0x170006CD RID: 1741
		// (get) Token: 0x06001B34 RID: 6964 RVA: 0x000812A7 File Offset: 0x000802A7
		[XmlIgnore]
		public XmlSchemaAttributeGroup RedefinedAttributeGroup
		{
			get
			{
				return this.redefined;
			}
		}

		// Token: 0x170006CE RID: 1742
		// (get) Token: 0x06001B35 RID: 6965 RVA: 0x000812AF File Offset: 0x000802AF
		// (set) Token: 0x06001B36 RID: 6966 RVA: 0x000812B7 File Offset: 0x000802B7
		[XmlIgnore]
		internal XmlSchemaAttributeGroup Redefined
		{
			get
			{
				return this.redefined;
			}
			set
			{
				this.redefined = value;
			}
		}

		// Token: 0x170006CF RID: 1743
		// (get) Token: 0x06001B37 RID: 6967 RVA: 0x000812C0 File Offset: 0x000802C0
		// (set) Token: 0x06001B38 RID: 6968 RVA: 0x000812C8 File Offset: 0x000802C8
		[XmlIgnore]
		internal int SelfReferenceCount
		{
			get
			{
				return this.selfReferenceCount;
			}
			set
			{
				this.selfReferenceCount = value;
			}
		}

		// Token: 0x170006D0 RID: 1744
		// (get) Token: 0x06001B39 RID: 6969 RVA: 0x000812D1 File Offset: 0x000802D1
		// (set) Token: 0x06001B3A RID: 6970 RVA: 0x000812D9 File Offset: 0x000802D9
		[XmlIgnore]
		internal override string NameAttribute
		{
			get
			{
				return this.Name;
			}
			set
			{
				this.Name = value;
			}
		}

		// Token: 0x06001B3B RID: 6971 RVA: 0x000812E2 File Offset: 0x000802E2
		internal void SetQualifiedName(XmlQualifiedName value)
		{
			this.qname = value;
		}

		// Token: 0x06001B3C RID: 6972 RVA: 0x000812EC File Offset: 0x000802EC
		internal override XmlSchemaObject Clone()
		{
			XmlSchemaAttributeGroup xmlSchemaAttributeGroup = (XmlSchemaAttributeGroup)base.MemberwiseClone();
			if (XmlSchemaComplexType.HasAttributeQNameRef(this.attributes))
			{
				xmlSchemaAttributeGroup.attributes = XmlSchemaComplexType.CloneAttributes(this.attributes);
				xmlSchemaAttributeGroup.attributeUses = null;
			}
			return xmlSchemaAttributeGroup;
		}

		// Token: 0x040010F7 RID: 4343
		private string name;

		// Token: 0x040010F8 RID: 4344
		private XmlSchemaObjectCollection attributes = new XmlSchemaObjectCollection();

		// Token: 0x040010F9 RID: 4345
		private XmlSchemaAnyAttribute anyAttribute;

		// Token: 0x040010FA RID: 4346
		private XmlQualifiedName qname = XmlQualifiedName.Empty;

		// Token: 0x040010FB RID: 4347
		private XmlSchemaAttributeGroup redefined;

		// Token: 0x040010FC RID: 4348
		private XmlSchemaObjectTable attributeUses;

		// Token: 0x040010FD RID: 4349
		private XmlSchemaAnyAttribute attributeWildcard;

		// Token: 0x040010FE RID: 4350
		private int selfReferenceCount;
	}
}
