using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace System.Xml.Schema
{
	// Token: 0x02000244 RID: 580
	public class XmlSchemaType : XmlSchemaAnnotated
	{
		// Token: 0x06001B8E RID: 7054 RVA: 0x00081B54 File Offset: 0x00080B54
		public static XmlSchemaSimpleType GetBuiltInSimpleType(XmlQualifiedName qualifiedName)
		{
			if (qualifiedName == null)
			{
				throw new ArgumentNullException("qualifiedName");
			}
			return DatatypeImplementation.GetSimpleTypeFromXsdType(qualifiedName);
		}

		// Token: 0x06001B8F RID: 7055 RVA: 0x00081B70 File Offset: 0x00080B70
		public static XmlSchemaSimpleType GetBuiltInSimpleType(XmlTypeCode typeCode)
		{
			return DatatypeImplementation.GetSimpleTypeFromTypeCode(typeCode);
		}

		// Token: 0x06001B90 RID: 7056 RVA: 0x00081B78 File Offset: 0x00080B78
		public static XmlSchemaComplexType GetBuiltInComplexType(XmlTypeCode typeCode)
		{
			if (typeCode == XmlTypeCode.Item)
			{
				return XmlSchemaComplexType.AnyType;
			}
			return null;
		}

		// Token: 0x06001B91 RID: 7057 RVA: 0x00081B88 File Offset: 0x00080B88
		public static XmlSchemaComplexType GetBuiltInComplexType(XmlQualifiedName qualifiedName)
		{
			if (qualifiedName == null)
			{
				throw new ArgumentNullException("qualifiedName");
			}
			if (qualifiedName.Equals(XmlSchemaComplexType.AnyType.QualifiedName))
			{
				return XmlSchemaComplexType.AnyType;
			}
			if (qualifiedName.Equals(XmlSchemaComplexType.UntypedAnyType.QualifiedName))
			{
				return XmlSchemaComplexType.UntypedAnyType;
			}
			return null;
		}

		// Token: 0x170006EE RID: 1774
		// (get) Token: 0x06001B92 RID: 7058 RVA: 0x00081BDA File Offset: 0x00080BDA
		// (set) Token: 0x06001B93 RID: 7059 RVA: 0x00081BE2 File Offset: 0x00080BE2
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

		// Token: 0x170006EF RID: 1775
		// (get) Token: 0x06001B94 RID: 7060 RVA: 0x00081BEB File Offset: 0x00080BEB
		// (set) Token: 0x06001B95 RID: 7061 RVA: 0x00081BF3 File Offset: 0x00080BF3
		[XmlAttribute("final")]
		[DefaultValue(XmlSchemaDerivationMethod.None)]
		public XmlSchemaDerivationMethod Final
		{
			get
			{
				return this.final;
			}
			set
			{
				this.final = value;
			}
		}

		// Token: 0x170006F0 RID: 1776
		// (get) Token: 0x06001B96 RID: 7062 RVA: 0x00081BFC File Offset: 0x00080BFC
		[XmlIgnore]
		public XmlQualifiedName QualifiedName
		{
			get
			{
				return this.qname;
			}
		}

		// Token: 0x170006F1 RID: 1777
		// (get) Token: 0x06001B97 RID: 7063 RVA: 0x00081C04 File Offset: 0x00080C04
		[XmlIgnore]
		public XmlSchemaDerivationMethod FinalResolved
		{
			get
			{
				return this.finalResolved;
			}
		}

		// Token: 0x170006F2 RID: 1778
		// (get) Token: 0x06001B98 RID: 7064 RVA: 0x00081C0C File Offset: 0x00080C0C
		[XmlIgnore]
		[Obsolete("This property has been deprecated. Please use BaseXmlSchemaType property that returns a strongly typed base schema type. http://go.microsoft.com/fwlink/?linkid=14202")]
		public object BaseSchemaType
		{
			get
			{
				if (this.baseSchemaType.QualifiedName.Namespace == "http://www.w3.org/2001/XMLSchema")
				{
					return this.baseSchemaType.Datatype;
				}
				return this.baseSchemaType;
			}
		}

		// Token: 0x170006F3 RID: 1779
		// (get) Token: 0x06001B99 RID: 7065 RVA: 0x00081C3C File Offset: 0x00080C3C
		[XmlIgnore]
		public XmlSchemaType BaseXmlSchemaType
		{
			get
			{
				return this.baseSchemaType;
			}
		}

		// Token: 0x170006F4 RID: 1780
		// (get) Token: 0x06001B9A RID: 7066 RVA: 0x00081C44 File Offset: 0x00080C44
		[XmlIgnore]
		public XmlSchemaDerivationMethod DerivedBy
		{
			get
			{
				return this.derivedBy;
			}
		}

		// Token: 0x170006F5 RID: 1781
		// (get) Token: 0x06001B9B RID: 7067 RVA: 0x00081C4C File Offset: 0x00080C4C
		[XmlIgnore]
		public XmlSchemaDatatype Datatype
		{
			get
			{
				return this.datatype;
			}
		}

		// Token: 0x170006F6 RID: 1782
		// (get) Token: 0x06001B9C RID: 7068 RVA: 0x00081C54 File Offset: 0x00080C54
		// (set) Token: 0x06001B9D RID: 7069 RVA: 0x00081C57 File Offset: 0x00080C57
		[XmlIgnore]
		public virtual bool IsMixed
		{
			get
			{
				return false;
			}
			set
			{
			}
		}

		// Token: 0x170006F7 RID: 1783
		// (get) Token: 0x06001B9E RID: 7070 RVA: 0x00081C59 File Offset: 0x00080C59
		[XmlIgnore]
		public XmlTypeCode TypeCode
		{
			get
			{
				if (this == XmlSchemaComplexType.AnyType)
				{
					return XmlTypeCode.Item;
				}
				if (this.datatype == null)
				{
					return XmlTypeCode.None;
				}
				return this.datatype.TypeCode;
			}
		}

		// Token: 0x170006F8 RID: 1784
		// (get) Token: 0x06001B9F RID: 7071 RVA: 0x00081C7A File Offset: 0x00080C7A
		[XmlIgnore]
		internal XmlValueConverter ValueConverter
		{
			get
			{
				if (this.datatype == null)
				{
					return XmlUntypedConverter.Untyped;
				}
				return this.datatype.ValueConverter;
			}
		}

		// Token: 0x06001BA0 RID: 7072 RVA: 0x00081C98 File Offset: 0x00080C98
		internal XmlReader Validate(XmlReader reader, XmlResolver resolver, XmlSchemaSet schemaSet, ValidationEventHandler valEventHandler)
		{
			if (schemaSet != null)
			{
				XmlReaderSettings xmlReaderSettings = new XmlReaderSettings();
				xmlReaderSettings.ValidationType = ValidationType.Schema;
				xmlReaderSettings.Schemas = schemaSet;
				xmlReaderSettings.ValidationEventHandler += valEventHandler;
				return new XsdValidatingReader(reader, resolver, xmlReaderSettings, this);
			}
			return null;
		}

		// Token: 0x170006F9 RID: 1785
		// (get) Token: 0x06001BA1 RID: 7073 RVA: 0x00081CCF File Offset: 0x00080CCF
		internal XmlSchemaContentType SchemaContentType
		{
			get
			{
				return this.contentType;
			}
		}

		// Token: 0x06001BA2 RID: 7074 RVA: 0x00081CD7 File Offset: 0x00080CD7
		internal void SetQualifiedName(XmlQualifiedName value)
		{
			this.qname = value;
		}

		// Token: 0x06001BA3 RID: 7075 RVA: 0x00081CE0 File Offset: 0x00080CE0
		internal void SetFinalResolved(XmlSchemaDerivationMethod value)
		{
			this.finalResolved = value;
		}

		// Token: 0x06001BA4 RID: 7076 RVA: 0x00081CE9 File Offset: 0x00080CE9
		internal void SetBaseSchemaType(XmlSchemaType value)
		{
			this.baseSchemaType = value;
		}

		// Token: 0x06001BA5 RID: 7077 RVA: 0x00081CF2 File Offset: 0x00080CF2
		internal void SetDerivedBy(XmlSchemaDerivationMethod value)
		{
			this.derivedBy = value;
		}

		// Token: 0x06001BA6 RID: 7078 RVA: 0x00081CFB File Offset: 0x00080CFB
		internal void SetDatatype(XmlSchemaDatatype value)
		{
			this.datatype = value;
		}

		// Token: 0x170006FA RID: 1786
		// (get) Token: 0x06001BA7 RID: 7079 RVA: 0x00081D04 File Offset: 0x00080D04
		// (set) Token: 0x06001BA8 RID: 7080 RVA: 0x00081D0C File Offset: 0x00080D0C
		internal SchemaElementDecl ElementDecl
		{
			get
			{
				return this.elementDecl;
			}
			set
			{
				this.elementDecl = value;
			}
		}

		// Token: 0x170006FB RID: 1787
		// (get) Token: 0x06001BA9 RID: 7081 RVA: 0x00081D15 File Offset: 0x00080D15
		// (set) Token: 0x06001BAA RID: 7082 RVA: 0x00081D1D File Offset: 0x00080D1D
		[XmlIgnore]
		internal XmlSchemaType Redefined
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

		// Token: 0x170006FC RID: 1788
		// (get) Token: 0x06001BAB RID: 7083 RVA: 0x00081D26 File Offset: 0x00080D26
		internal virtual XmlQualifiedName DerivedFrom
		{
			get
			{
				return XmlQualifiedName.Empty;
			}
		}

		// Token: 0x06001BAC RID: 7084 RVA: 0x00081D2D File Offset: 0x00080D2D
		internal void SetContentType(XmlSchemaContentType value)
		{
			this.contentType = value;
		}

		// Token: 0x06001BAD RID: 7085 RVA: 0x00081D38 File Offset: 0x00080D38
		public static bool IsDerivedFrom(XmlSchemaType derivedType, XmlSchemaType baseType, XmlSchemaDerivationMethod except)
		{
			if (derivedType == null || baseType == null)
			{
				return false;
			}
			if (derivedType == baseType)
			{
				return true;
			}
			if (baseType == XmlSchemaComplexType.AnyType)
			{
				return true;
			}
			XmlSchemaSimpleType xmlSchemaSimpleType;
			XmlSchemaSimpleType xmlSchemaSimpleType2;
			for (;;)
			{
				xmlSchemaSimpleType = derivedType as XmlSchemaSimpleType;
				xmlSchemaSimpleType2 = baseType as XmlSchemaSimpleType;
				if (xmlSchemaSimpleType2 != null && xmlSchemaSimpleType != null)
				{
					break;
				}
				if ((except & derivedType.DerivedBy) != XmlSchemaDerivationMethod.Empty)
				{
					return false;
				}
				derivedType = derivedType.BaseXmlSchemaType;
				if (derivedType == baseType)
				{
					return true;
				}
				if (derivedType == null)
				{
					return false;
				}
			}
			return xmlSchemaSimpleType2 == DatatypeImplementation.AnySimpleType || ((except & derivedType.DerivedBy) == XmlSchemaDerivationMethod.Empty && xmlSchemaSimpleType.Datatype.IsDerivedFrom(xmlSchemaSimpleType2.Datatype));
		}

		// Token: 0x06001BAE RID: 7086 RVA: 0x00081DBA File Offset: 0x00080DBA
		internal static bool IsDerivedFromDatatype(XmlSchemaDatatype derivedDataType, XmlSchemaDatatype baseDataType, XmlSchemaDerivationMethod except)
		{
			return DatatypeImplementation.AnySimpleType.Datatype == baseDataType || derivedDataType.IsDerivedFrom(baseDataType);
		}

		// Token: 0x170006FD RID: 1789
		// (get) Token: 0x06001BAF RID: 7087 RVA: 0x00081DD2 File Offset: 0x00080DD2
		// (set) Token: 0x06001BB0 RID: 7088 RVA: 0x00081DDA File Offset: 0x00080DDA
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

		// Token: 0x04001118 RID: 4376
		private string name;

		// Token: 0x04001119 RID: 4377
		private XmlSchemaDerivationMethod final = XmlSchemaDerivationMethod.None;

		// Token: 0x0400111A RID: 4378
		private XmlSchemaDerivationMethod derivedBy;

		// Token: 0x0400111B RID: 4379
		private XmlSchemaType baseSchemaType;

		// Token: 0x0400111C RID: 4380
		private XmlSchemaDatatype datatype;

		// Token: 0x0400111D RID: 4381
		private XmlSchemaDerivationMethod finalResolved;

		// Token: 0x0400111E RID: 4382
		private SchemaElementDecl elementDecl;

		// Token: 0x0400111F RID: 4383
		private XmlQualifiedName qname = XmlQualifiedName.Empty;

		// Token: 0x04001120 RID: 4384
		private XmlSchemaType redefined;

		// Token: 0x04001121 RID: 4385
		private XmlSchemaContentType contentType;
	}
}
