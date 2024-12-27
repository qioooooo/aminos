using System;
using System.Xml.Serialization;

namespace System.Xml.Schema
{
	// Token: 0x0200027B RID: 635
	public class XmlSchemaSimpleTypeRestriction : XmlSchemaSimpleTypeContent
	{
		// Token: 0x1700079B RID: 1947
		// (get) Token: 0x06001D4F RID: 7503 RVA: 0x00085E12 File Offset: 0x00084E12
		// (set) Token: 0x06001D50 RID: 7504 RVA: 0x00085E1A File Offset: 0x00084E1A
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

		// Token: 0x1700079C RID: 1948
		// (get) Token: 0x06001D51 RID: 7505 RVA: 0x00085E33 File Offset: 0x00084E33
		// (set) Token: 0x06001D52 RID: 7506 RVA: 0x00085E3B File Offset: 0x00084E3B
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

		// Token: 0x1700079D RID: 1949
		// (get) Token: 0x06001D53 RID: 7507 RVA: 0x00085E44 File Offset: 0x00084E44
		[XmlElement("totalDigits", typeof(XmlSchemaTotalDigitsFacet))]
		[XmlElement("maxExclusive", typeof(XmlSchemaMaxExclusiveFacet))]
		[XmlElement("fractionDigits", typeof(XmlSchemaFractionDigitsFacet))]
		[XmlElement("minLength", typeof(XmlSchemaMinLengthFacet))]
		[XmlElement("pattern", typeof(XmlSchemaPatternFacet))]
		[XmlElement("enumeration", typeof(XmlSchemaEnumerationFacet))]
		[XmlElement("maxInclusive", typeof(XmlSchemaMaxInclusiveFacet))]
		[XmlElement("minInclusive", typeof(XmlSchemaMinInclusiveFacet))]
		[XmlElement("minExclusive", typeof(XmlSchemaMinExclusiveFacet))]
		[XmlElement("length", typeof(XmlSchemaLengthFacet))]
		[XmlElement("maxLength", typeof(XmlSchemaMaxLengthFacet))]
		[XmlElement("whiteSpace", typeof(XmlSchemaWhiteSpaceFacet))]
		public XmlSchemaObjectCollection Facets
		{
			get
			{
				return this.facets;
			}
		}

		// Token: 0x06001D54 RID: 7508 RVA: 0x00085E4C File Offset: 0x00084E4C
		internal override XmlSchemaObject Clone()
		{
			XmlSchemaSimpleTypeRestriction xmlSchemaSimpleTypeRestriction = (XmlSchemaSimpleTypeRestriction)base.MemberwiseClone();
			xmlSchemaSimpleTypeRestriction.BaseTypeName = this.baseTypeName.Clone();
			return xmlSchemaSimpleTypeRestriction;
		}

		// Token: 0x040011DC RID: 4572
		private XmlQualifiedName baseTypeName = XmlQualifiedName.Empty;

		// Token: 0x040011DD RID: 4573
		private XmlSchemaSimpleType baseType;

		// Token: 0x040011DE RID: 4574
		private XmlSchemaObjectCollection facets = new XmlSchemaObjectCollection();
	}
}
