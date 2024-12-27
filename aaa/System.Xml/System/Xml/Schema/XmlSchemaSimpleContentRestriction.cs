using System;
using System.Xml.Serialization;

namespace System.Xml.Schema
{
	// Token: 0x02000277 RID: 631
	public class XmlSchemaSimpleContentRestriction : XmlSchemaContent
	{
		// Token: 0x17000791 RID: 1937
		// (get) Token: 0x06001D37 RID: 7479 RVA: 0x00085C7F File Offset: 0x00084C7F
		// (set) Token: 0x06001D38 RID: 7480 RVA: 0x00085C87 File Offset: 0x00084C87
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

		// Token: 0x17000792 RID: 1938
		// (get) Token: 0x06001D39 RID: 7481 RVA: 0x00085CA0 File Offset: 0x00084CA0
		// (set) Token: 0x06001D3A RID: 7482 RVA: 0x00085CA8 File Offset: 0x00084CA8
		[XmlElement("simpleType", typeof(XmlSchemaSimpleType))]
		public XmlSchemaSimpleType BaseType
		{
			get
			{
				return this.baseType;
			}
			set
			{
				this.baseType = value;
			}
		}

		// Token: 0x17000793 RID: 1939
		// (get) Token: 0x06001D3B RID: 7483 RVA: 0x00085CB1 File Offset: 0x00084CB1
		[XmlElement("whiteSpace", typeof(XmlSchemaWhiteSpaceFacet))]
		[XmlElement("maxInclusive", typeof(XmlSchemaMaxInclusiveFacet))]
		[XmlElement("maxExclusive", typeof(XmlSchemaMaxExclusiveFacet))]
		[XmlElement("minInclusive", typeof(XmlSchemaMinInclusiveFacet))]
		[XmlElement("minExclusive", typeof(XmlSchemaMinExclusiveFacet))]
		[XmlElement("totalDigits", typeof(XmlSchemaTotalDigitsFacet))]
		[XmlElement("fractionDigits", typeof(XmlSchemaFractionDigitsFacet))]
		[XmlElement("length", typeof(XmlSchemaLengthFacet))]
		[XmlElement("minLength", typeof(XmlSchemaMinLengthFacet))]
		[XmlElement("maxLength", typeof(XmlSchemaMaxLengthFacet))]
		[XmlElement("pattern", typeof(XmlSchemaPatternFacet))]
		[XmlElement("enumeration", typeof(XmlSchemaEnumerationFacet))]
		public XmlSchemaObjectCollection Facets
		{
			get
			{
				return this.facets;
			}
		}

		// Token: 0x17000794 RID: 1940
		// (get) Token: 0x06001D3C RID: 7484 RVA: 0x00085CB9 File Offset: 0x00084CB9
		[XmlElement("attributeGroup", typeof(XmlSchemaAttributeGroupRef))]
		[XmlElement("attribute", typeof(XmlSchemaAttribute))]
		public XmlSchemaObjectCollection Attributes
		{
			get
			{
				return this.attributes;
			}
		}

		// Token: 0x17000795 RID: 1941
		// (get) Token: 0x06001D3D RID: 7485 RVA: 0x00085CC1 File Offset: 0x00084CC1
		// (set) Token: 0x06001D3E RID: 7486 RVA: 0x00085CC9 File Offset: 0x00084CC9
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

		// Token: 0x06001D3F RID: 7487 RVA: 0x00085CD2 File Offset: 0x00084CD2
		internal void SetAttributes(XmlSchemaObjectCollection newAttributes)
		{
			this.attributes = newAttributes;
		}

		// Token: 0x040011D3 RID: 4563
		private XmlQualifiedName baseTypeName = XmlQualifiedName.Empty;

		// Token: 0x040011D4 RID: 4564
		private XmlSchemaSimpleType baseType;

		// Token: 0x040011D5 RID: 4565
		private XmlSchemaObjectCollection facets = new XmlSchemaObjectCollection();

		// Token: 0x040011D6 RID: 4566
		private XmlSchemaObjectCollection attributes = new XmlSchemaObjectCollection();

		// Token: 0x040011D7 RID: 4567
		private XmlSchemaAnyAttribute anyAttribute;
	}
}
