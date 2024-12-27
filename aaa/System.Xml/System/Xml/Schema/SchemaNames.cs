using System;

namespace System.Xml.Schema
{
	// Token: 0x02000214 RID: 532
	internal sealed class SchemaNames
	{
		// Token: 0x1700065C RID: 1628
		// (get) Token: 0x060019A7 RID: 6567 RVA: 0x0007A576 File Offset: 0x00079576
		public XmlNameTable NameTable
		{
			get
			{
				return this.nameTable;
			}
		}

		// Token: 0x060019A8 RID: 6568 RVA: 0x0007A580 File Offset: 0x00079580
		public SchemaNames(XmlNameTable nameTable)
		{
			this.nameTable = nameTable;
			this.NsDataType = nameTable.Add("urn:schemas-microsoft-com:datatypes");
			this.NsDataTypeAlias = nameTable.Add("uuid:C2F41010-65B3-11D1-A29F-00AA00C14882");
			this.NsDataTypeOld = nameTable.Add("urn:uuid:C2F41010-65B3-11D1-A29F-00AA00C14882/");
			this.NsXml = nameTable.Add("http://www.w3.org/XML/1998/namespace");
			this.NsXmlNs = nameTable.Add("http://www.w3.org/2000/xmlns/");
			this.NsXdr = nameTable.Add("urn:schemas-microsoft-com:xml-data");
			this.NsXdrAlias = nameTable.Add("uuid:BDC6E3F0-6DA3-11D1-A2A3-00AA00C14882");
			this.NsXs = nameTable.Add("http://www.w3.org/2001/XMLSchema");
			this.NsXsi = nameTable.Add("http://www.w3.org/2001/XMLSchema-instance");
			this.XsiType = nameTable.Add("type");
			this.XsiNil = nameTable.Add("nil");
			this.XsiSchemaLocation = nameTable.Add("schemaLocation");
			this.XsiNoNamespaceSchemaLocation = nameTable.Add("noNamespaceSchemaLocation");
			this.XsdSchema = nameTable.Add("schema");
			this.XdrSchema = nameTable.Add("Schema");
			this.QnPCData = new XmlQualifiedName(nameTable.Add("#PCDATA"));
			this.QnXml = new XmlQualifiedName(nameTable.Add("xml"));
			this.QnXmlNs = new XmlQualifiedName(nameTable.Add("xmlns"), this.NsXmlNs);
			this.QnDtDt = new XmlQualifiedName(nameTable.Add("dt"), this.NsDataType);
			this.QnXmlLang = new XmlQualifiedName(nameTable.Add("lang"), this.NsXml);
			this.QnName = new XmlQualifiedName(nameTable.Add("name"));
			this.QnType = new XmlQualifiedName(nameTable.Add("type"));
			this.QnMaxOccurs = new XmlQualifiedName(nameTable.Add("maxOccurs"));
			this.QnMinOccurs = new XmlQualifiedName(nameTable.Add("minOccurs"));
			this.QnInfinite = new XmlQualifiedName(nameTable.Add("*"));
			this.QnModel = new XmlQualifiedName(nameTable.Add("model"));
			this.QnOpen = new XmlQualifiedName(nameTable.Add("open"));
			this.QnClosed = new XmlQualifiedName(nameTable.Add("closed"));
			this.QnContent = new XmlQualifiedName(nameTable.Add("content"));
			this.QnMixed = new XmlQualifiedName(nameTable.Add("mixed"));
			this.QnEmpty = new XmlQualifiedName(nameTable.Add("empty"));
			this.QnEltOnly = new XmlQualifiedName(nameTable.Add("eltOnly"));
			this.QnTextOnly = new XmlQualifiedName(nameTable.Add("textOnly"));
			this.QnOrder = new XmlQualifiedName(nameTable.Add("order"));
			this.QnSeq = new XmlQualifiedName(nameTable.Add("seq"));
			this.QnOne = new XmlQualifiedName(nameTable.Add("one"));
			this.QnMany = new XmlQualifiedName(nameTable.Add("many"));
			this.QnRequired = new XmlQualifiedName(nameTable.Add("required"));
			this.QnYes = new XmlQualifiedName(nameTable.Add("yes"));
			this.QnNo = new XmlQualifiedName(nameTable.Add("no"));
			this.QnString = new XmlQualifiedName(nameTable.Add("string"));
			this.QnID = new XmlQualifiedName(nameTable.Add("id"));
			this.QnIDRef = new XmlQualifiedName(nameTable.Add("idref"));
			this.QnIDRefs = new XmlQualifiedName(nameTable.Add("idrefs"));
			this.QnEntity = new XmlQualifiedName(nameTable.Add("entity"));
			this.QnEntities = new XmlQualifiedName(nameTable.Add("entities"));
			this.QnNmToken = new XmlQualifiedName(nameTable.Add("nmtoken"));
			this.QnNmTokens = new XmlQualifiedName(nameTable.Add("nmtokens"));
			this.QnEnumeration = new XmlQualifiedName(nameTable.Add("enumeration"));
			this.QnDefault = new XmlQualifiedName(nameTable.Add("default"));
			this.QnTargetNamespace = new XmlQualifiedName(nameTable.Add("targetNamespace"));
			this.QnVersion = new XmlQualifiedName(nameTable.Add("version"));
			this.QnFinalDefault = new XmlQualifiedName(nameTable.Add("finalDefault"));
			this.QnBlockDefault = new XmlQualifiedName(nameTable.Add("blockDefault"));
			this.QnFixed = new XmlQualifiedName(nameTable.Add("fixed"));
			this.QnAbstract = new XmlQualifiedName(nameTable.Add("abstract"));
			this.QnBlock = new XmlQualifiedName(nameTable.Add("block"));
			this.QnSubstitutionGroup = new XmlQualifiedName(nameTable.Add("substitutionGroup"));
			this.QnFinal = new XmlQualifiedName(nameTable.Add("final"));
			this.QnNillable = new XmlQualifiedName(nameTable.Add("nillable"));
			this.QnRef = new XmlQualifiedName(nameTable.Add("ref"));
			this.QnBase = new XmlQualifiedName(nameTable.Add("base"));
			this.QnDerivedBy = new XmlQualifiedName(nameTable.Add("derivedBy"));
			this.QnNamespace = new XmlQualifiedName(nameTable.Add("namespace"));
			this.QnProcessContents = new XmlQualifiedName(nameTable.Add("processContents"));
			this.QnRefer = new XmlQualifiedName(nameTable.Add("refer"));
			this.QnPublic = new XmlQualifiedName(nameTable.Add("public"));
			this.QnSystem = new XmlQualifiedName(nameTable.Add("system"));
			this.QnSchemaLocation = new XmlQualifiedName(nameTable.Add("schemaLocation"));
			this.QnValue = new XmlQualifiedName(nameTable.Add("value"));
			this.QnUse = new XmlQualifiedName(nameTable.Add("use"));
			this.QnForm = new XmlQualifiedName(nameTable.Add("form"));
			this.QnAttributeFormDefault = new XmlQualifiedName(nameTable.Add("attributeFormDefault"));
			this.QnElementFormDefault = new XmlQualifiedName(nameTable.Add("elementFormDefault"));
			this.QnSource = new XmlQualifiedName(nameTable.Add("source"));
			this.QnMemberTypes = new XmlQualifiedName(nameTable.Add("memberTypes"));
			this.QnItemType = new XmlQualifiedName(nameTable.Add("itemType"));
			this.QnXPath = new XmlQualifiedName(nameTable.Add("xpath"));
			this.QnXdrSchema = new XmlQualifiedName(this.XdrSchema, this.NsXdr);
			this.QnXdrElementType = new XmlQualifiedName(nameTable.Add("ElementType"), this.NsXdr);
			this.QnXdrElement = new XmlQualifiedName(nameTable.Add("element"), this.NsXdr);
			this.QnXdrGroup = new XmlQualifiedName(nameTable.Add("group"), this.NsXdr);
			this.QnXdrAttributeType = new XmlQualifiedName(nameTable.Add("AttributeType"), this.NsXdr);
			this.QnXdrAttribute = new XmlQualifiedName(nameTable.Add("attribute"), this.NsXdr);
			this.QnXdrDataType = new XmlQualifiedName(nameTable.Add("datatype"), this.NsXdr);
			this.QnXdrDescription = new XmlQualifiedName(nameTable.Add("description"), this.NsXdr);
			this.QnXdrExtends = new XmlQualifiedName(nameTable.Add("extends"), this.NsXdr);
			this.QnXdrAliasSchema = new XmlQualifiedName(nameTable.Add("Schema"), this.NsDataTypeAlias);
			this.QnDtType = new XmlQualifiedName(nameTable.Add("type"), this.NsDataType);
			this.QnDtValues = new XmlQualifiedName(nameTable.Add("values"), this.NsDataType);
			this.QnDtMaxLength = new XmlQualifiedName(nameTable.Add("maxLength"), this.NsDataType);
			this.QnDtMinLength = new XmlQualifiedName(nameTable.Add("minLength"), this.NsDataType);
			this.QnDtMax = new XmlQualifiedName(nameTable.Add("max"), this.NsDataType);
			this.QnDtMin = new XmlQualifiedName(nameTable.Add("min"), this.NsDataType);
			this.QnDtMinExclusive = new XmlQualifiedName(nameTable.Add("minExclusive"), this.NsDataType);
			this.QnDtMaxExclusive = new XmlQualifiedName(nameTable.Add("maxExclusive"), this.NsDataType);
			this.QnXsdSchema = new XmlQualifiedName(this.XsdSchema, this.NsXs);
			this.QnXsdAnnotation = new XmlQualifiedName(nameTable.Add("annotation"), this.NsXs);
			this.QnXsdInclude = new XmlQualifiedName(nameTable.Add("include"), this.NsXs);
			this.QnXsdImport = new XmlQualifiedName(nameTable.Add("import"), this.NsXs);
			this.QnXsdElement = new XmlQualifiedName(nameTable.Add("element"), this.NsXs);
			this.QnXsdAttribute = new XmlQualifiedName(nameTable.Add("attribute"), this.NsXs);
			this.QnXsdAttributeGroup = new XmlQualifiedName(nameTable.Add("attributeGroup"), this.NsXs);
			this.QnXsdAnyAttribute = new XmlQualifiedName(nameTable.Add("anyAttribute"), this.NsXs);
			this.QnXsdGroup = new XmlQualifiedName(nameTable.Add("group"), this.NsXs);
			this.QnXsdAll = new XmlQualifiedName(nameTable.Add("all"), this.NsXs);
			this.QnXsdChoice = new XmlQualifiedName(nameTable.Add("choice"), this.NsXs);
			this.QnXsdSequence = new XmlQualifiedName(nameTable.Add("sequence"), this.NsXs);
			this.QnXsdAny = new XmlQualifiedName(nameTable.Add("any"), this.NsXs);
			this.QnXsdNotation = new XmlQualifiedName(nameTable.Add("notation"), this.NsXs);
			this.QnXsdSimpleType = new XmlQualifiedName(nameTable.Add("simpleType"), this.NsXs);
			this.QnXsdComplexType = new XmlQualifiedName(nameTable.Add("complexType"), this.NsXs);
			this.QnXsdUnique = new XmlQualifiedName(nameTable.Add("unique"), this.NsXs);
			this.QnXsdKey = new XmlQualifiedName(nameTable.Add("key"), this.NsXs);
			this.QnXsdKeyRef = new XmlQualifiedName(nameTable.Add("keyref"), this.NsXs);
			this.QnXsdSelector = new XmlQualifiedName(nameTable.Add("selector"), this.NsXs);
			this.QnXsdField = new XmlQualifiedName(nameTable.Add("field"), this.NsXs);
			this.QnXsdMinExclusive = new XmlQualifiedName(nameTable.Add("minExclusive"), this.NsXs);
			this.QnXsdMinInclusive = new XmlQualifiedName(nameTable.Add("minInclusive"), this.NsXs);
			this.QnXsdMaxInclusive = new XmlQualifiedName(nameTable.Add("maxInclusive"), this.NsXs);
			this.QnXsdMaxExclusive = new XmlQualifiedName(nameTable.Add("maxExclusive"), this.NsXs);
			this.QnXsdTotalDigits = new XmlQualifiedName(nameTable.Add("totalDigits"), this.NsXs);
			this.QnXsdFractionDigits = new XmlQualifiedName(nameTable.Add("fractionDigits"), this.NsXs);
			this.QnXsdLength = new XmlQualifiedName(nameTable.Add("length"), this.NsXs);
			this.QnXsdMinLength = new XmlQualifiedName(nameTable.Add("minLength"), this.NsXs);
			this.QnXsdMaxLength = new XmlQualifiedName(nameTable.Add("maxLength"), this.NsXs);
			this.QnXsdEnumeration = new XmlQualifiedName(nameTable.Add("enumeration"), this.NsXs);
			this.QnXsdPattern = new XmlQualifiedName(nameTable.Add("pattern"), this.NsXs);
			this.QnXsdDocumentation = new XmlQualifiedName(nameTable.Add("documentation"), this.NsXs);
			this.QnXsdAppinfo = new XmlQualifiedName(nameTable.Add("appinfo"), this.NsXs);
			this.QnXsdComplexContent = new XmlQualifiedName(nameTable.Add("complexContent"), this.NsXs);
			this.QnXsdSimpleContent = new XmlQualifiedName(nameTable.Add("simpleContent"), this.NsXs);
			this.QnXsdRestriction = new XmlQualifiedName(nameTable.Add("restriction"), this.NsXs);
			this.QnXsdExtension = new XmlQualifiedName(nameTable.Add("extension"), this.NsXs);
			this.QnXsdUnion = new XmlQualifiedName(nameTable.Add("union"), this.NsXs);
			this.QnXsdList = new XmlQualifiedName(nameTable.Add("list"), this.NsXs);
			this.QnXsdWhiteSpace = new XmlQualifiedName(nameTable.Add("whiteSpace"), this.NsXs);
			this.QnXsdRedefine = new XmlQualifiedName(nameTable.Add("redefine"), this.NsXs);
			this.QnXsdAnyType = new XmlQualifiedName(nameTable.Add("anyType"), this.NsXs);
			this.CreateTokenToQNameTable();
		}

		// Token: 0x060019A9 RID: 6569 RVA: 0x0007B2CC File Offset: 0x0007A2CC
		public void CreateTokenToQNameTable()
		{
			this.TokenToQName[1] = this.QnName;
			this.TokenToQName[2] = this.QnType;
			this.TokenToQName[3] = this.QnMaxOccurs;
			this.TokenToQName[4] = this.QnMinOccurs;
			this.TokenToQName[5] = this.QnInfinite;
			this.TokenToQName[6] = this.QnModel;
			this.TokenToQName[7] = this.QnOpen;
			this.TokenToQName[8] = this.QnClosed;
			this.TokenToQName[9] = this.QnContent;
			this.TokenToQName[10] = this.QnMixed;
			this.TokenToQName[11] = this.QnEmpty;
			this.TokenToQName[12] = this.QnEltOnly;
			this.TokenToQName[13] = this.QnTextOnly;
			this.TokenToQName[14] = this.QnOrder;
			this.TokenToQName[15] = this.QnSeq;
			this.TokenToQName[16] = this.QnOne;
			this.TokenToQName[17] = this.QnMany;
			this.TokenToQName[18] = this.QnRequired;
			this.TokenToQName[19] = this.QnYes;
			this.TokenToQName[20] = this.QnNo;
			this.TokenToQName[21] = this.QnString;
			this.TokenToQName[22] = this.QnID;
			this.TokenToQName[23] = this.QnIDRef;
			this.TokenToQName[24] = this.QnIDRefs;
			this.TokenToQName[25] = this.QnEntity;
			this.TokenToQName[26] = this.QnEntities;
			this.TokenToQName[27] = this.QnNmToken;
			this.TokenToQName[28] = this.QnNmTokens;
			this.TokenToQName[29] = this.QnEnumeration;
			this.TokenToQName[30] = this.QnDefault;
			this.TokenToQName[31] = this.QnXdrSchema;
			this.TokenToQName[32] = this.QnXdrElementType;
			this.TokenToQName[33] = this.QnXdrElement;
			this.TokenToQName[34] = this.QnXdrGroup;
			this.TokenToQName[35] = this.QnXdrAttributeType;
			this.TokenToQName[36] = this.QnXdrAttribute;
			this.TokenToQName[37] = this.QnXdrDataType;
			this.TokenToQName[38] = this.QnXdrDescription;
			this.TokenToQName[39] = this.QnXdrExtends;
			this.TokenToQName[40] = this.QnXdrAliasSchema;
			this.TokenToQName[41] = this.QnDtType;
			this.TokenToQName[42] = this.QnDtValues;
			this.TokenToQName[43] = this.QnDtMaxLength;
			this.TokenToQName[44] = this.QnDtMinLength;
			this.TokenToQName[45] = this.QnDtMax;
			this.TokenToQName[46] = this.QnDtMin;
			this.TokenToQName[47] = this.QnDtMinExclusive;
			this.TokenToQName[48] = this.QnDtMaxExclusive;
			this.TokenToQName[49] = this.QnTargetNamespace;
			this.TokenToQName[50] = this.QnVersion;
			this.TokenToQName[51] = this.QnFinalDefault;
			this.TokenToQName[52] = this.QnBlockDefault;
			this.TokenToQName[53] = this.QnFixed;
			this.TokenToQName[54] = this.QnAbstract;
			this.TokenToQName[55] = this.QnBlock;
			this.TokenToQName[56] = this.QnSubstitutionGroup;
			this.TokenToQName[57] = this.QnFinal;
			this.TokenToQName[58] = this.QnNillable;
			this.TokenToQName[59] = this.QnRef;
			this.TokenToQName[60] = this.QnBase;
			this.TokenToQName[61] = this.QnDerivedBy;
			this.TokenToQName[62] = this.QnNamespace;
			this.TokenToQName[63] = this.QnProcessContents;
			this.TokenToQName[64] = this.QnRefer;
			this.TokenToQName[65] = this.QnPublic;
			this.TokenToQName[66] = this.QnSystem;
			this.TokenToQName[67] = this.QnSchemaLocation;
			this.TokenToQName[68] = this.QnValue;
			this.TokenToQName[119] = this.QnItemType;
			this.TokenToQName[120] = this.QnMemberTypes;
			this.TokenToQName[121] = this.QnXPath;
			this.TokenToQName[74] = this.QnXsdSchema;
			this.TokenToQName[75] = this.QnXsdAnnotation;
			this.TokenToQName[76] = this.QnXsdInclude;
			this.TokenToQName[77] = this.QnXsdImport;
			this.TokenToQName[78] = this.QnXsdElement;
			this.TokenToQName[79] = this.QnXsdAttribute;
			this.TokenToQName[80] = this.QnXsdAttributeGroup;
			this.TokenToQName[81] = this.QnXsdAnyAttribute;
			this.TokenToQName[82] = this.QnXsdGroup;
			this.TokenToQName[83] = this.QnXsdAll;
			this.TokenToQName[84] = this.QnXsdChoice;
			this.TokenToQName[85] = this.QnXsdSequence;
			this.TokenToQName[86] = this.QnXsdAny;
			this.TokenToQName[87] = this.QnXsdNotation;
			this.TokenToQName[88] = this.QnXsdSimpleType;
			this.TokenToQName[89] = this.QnXsdComplexType;
			this.TokenToQName[90] = this.QnXsdUnique;
			this.TokenToQName[91] = this.QnXsdKey;
			this.TokenToQName[92] = this.QnXsdKeyRef;
			this.TokenToQName[93] = this.QnXsdSelector;
			this.TokenToQName[94] = this.QnXsdField;
			this.TokenToQName[95] = this.QnXsdMinExclusive;
			this.TokenToQName[96] = this.QnXsdMinInclusive;
			this.TokenToQName[97] = this.QnXsdMaxExclusive;
			this.TokenToQName[98] = this.QnXsdMaxInclusive;
			this.TokenToQName[99] = this.QnXsdTotalDigits;
			this.TokenToQName[100] = this.QnXsdFractionDigits;
			this.TokenToQName[101] = this.QnXsdLength;
			this.TokenToQName[102] = this.QnXsdMinLength;
			this.TokenToQName[103] = this.QnXsdMaxLength;
			this.TokenToQName[104] = this.QnXsdEnumeration;
			this.TokenToQName[105] = this.QnXsdPattern;
			this.TokenToQName[117] = this.QnXsdWhiteSpace;
			this.TokenToQName[106] = this.QnXsdDocumentation;
			this.TokenToQName[107] = this.QnXsdAppinfo;
			this.TokenToQName[108] = this.QnXsdComplexContent;
			this.TokenToQName[110] = this.QnXsdRestriction;
			this.TokenToQName[113] = this.QnXsdRestriction;
			this.TokenToQName[115] = this.QnXsdRestriction;
			this.TokenToQName[109] = this.QnXsdExtension;
			this.TokenToQName[112] = this.QnXsdExtension;
			this.TokenToQName[111] = this.QnXsdSimpleContent;
			this.TokenToQName[116] = this.QnXsdUnion;
			this.TokenToQName[114] = this.QnXsdList;
			this.TokenToQName[118] = this.QnXsdRedefine;
			this.TokenToQName[69] = this.QnSource;
			this.TokenToQName[72] = this.QnUse;
			this.TokenToQName[73] = this.QnForm;
			this.TokenToQName[71] = this.QnElementFormDefault;
			this.TokenToQName[70] = this.QnAttributeFormDefault;
			this.TokenToQName[122] = this.QnXmlLang;
			this.TokenToQName[0] = XmlQualifiedName.Empty;
		}

		// Token: 0x060019AA RID: 6570 RVA: 0x0007BA04 File Offset: 0x0007AA04
		public SchemaType SchemaTypeFromRoot(string localName, string ns)
		{
			if (this.IsXSDRoot(localName, ns))
			{
				return SchemaType.XSD;
			}
			if (this.IsXDRRoot(localName, XmlSchemaDatatype.XdrCanonizeUri(ns, this.nameTable, this)))
			{
				return SchemaType.XDR;
			}
			return SchemaType.None;
		}

		// Token: 0x060019AB RID: 6571 RVA: 0x0007BA2B File Offset: 0x0007AA2B
		public bool IsXSDRoot(string localName, string ns)
		{
			return Ref.Equal(ns, this.NsXs) && Ref.Equal(localName, this.XsdSchema);
		}

		// Token: 0x060019AC RID: 6572 RVA: 0x0007BA49 File Offset: 0x0007AA49
		public bool IsXDRRoot(string localName, string ns)
		{
			return Ref.Equal(ns, this.NsXdr) && Ref.Equal(localName, this.XdrSchema);
		}

		// Token: 0x060019AD RID: 6573 RVA: 0x0007BA67 File Offset: 0x0007AA67
		public XmlQualifiedName GetName(SchemaNames.Token token)
		{
			return this.TokenToQName[(int)token];
		}

		// Token: 0x04000EFA RID: 3834
		private XmlNameTable nameTable;

		// Token: 0x04000EFB RID: 3835
		public string NsDataType;

		// Token: 0x04000EFC RID: 3836
		public string NsDataTypeAlias;

		// Token: 0x04000EFD RID: 3837
		public string NsDataTypeOld;

		// Token: 0x04000EFE RID: 3838
		public string NsXml;

		// Token: 0x04000EFF RID: 3839
		public string NsXmlNs;

		// Token: 0x04000F00 RID: 3840
		public string NsXdr;

		// Token: 0x04000F01 RID: 3841
		public string NsXdrAlias;

		// Token: 0x04000F02 RID: 3842
		public string NsXs;

		// Token: 0x04000F03 RID: 3843
		public string NsXsi;

		// Token: 0x04000F04 RID: 3844
		public string XsiType;

		// Token: 0x04000F05 RID: 3845
		public string XsiNil;

		// Token: 0x04000F06 RID: 3846
		public string XsiSchemaLocation;

		// Token: 0x04000F07 RID: 3847
		public string XsiNoNamespaceSchemaLocation;

		// Token: 0x04000F08 RID: 3848
		public string XsdSchema;

		// Token: 0x04000F09 RID: 3849
		public string XdrSchema;

		// Token: 0x04000F0A RID: 3850
		public XmlQualifiedName QnPCData;

		// Token: 0x04000F0B RID: 3851
		public XmlQualifiedName QnXml;

		// Token: 0x04000F0C RID: 3852
		public XmlQualifiedName QnXmlNs;

		// Token: 0x04000F0D RID: 3853
		public XmlQualifiedName QnDtDt;

		// Token: 0x04000F0E RID: 3854
		public XmlQualifiedName QnXmlLang;

		// Token: 0x04000F0F RID: 3855
		public XmlQualifiedName QnName;

		// Token: 0x04000F10 RID: 3856
		public XmlQualifiedName QnType;

		// Token: 0x04000F11 RID: 3857
		public XmlQualifiedName QnMaxOccurs;

		// Token: 0x04000F12 RID: 3858
		public XmlQualifiedName QnMinOccurs;

		// Token: 0x04000F13 RID: 3859
		public XmlQualifiedName QnInfinite;

		// Token: 0x04000F14 RID: 3860
		public XmlQualifiedName QnModel;

		// Token: 0x04000F15 RID: 3861
		public XmlQualifiedName QnOpen;

		// Token: 0x04000F16 RID: 3862
		public XmlQualifiedName QnClosed;

		// Token: 0x04000F17 RID: 3863
		public XmlQualifiedName QnContent;

		// Token: 0x04000F18 RID: 3864
		public XmlQualifiedName QnMixed;

		// Token: 0x04000F19 RID: 3865
		public XmlQualifiedName QnEmpty;

		// Token: 0x04000F1A RID: 3866
		public XmlQualifiedName QnEltOnly;

		// Token: 0x04000F1B RID: 3867
		public XmlQualifiedName QnTextOnly;

		// Token: 0x04000F1C RID: 3868
		public XmlQualifiedName QnOrder;

		// Token: 0x04000F1D RID: 3869
		public XmlQualifiedName QnSeq;

		// Token: 0x04000F1E RID: 3870
		public XmlQualifiedName QnOne;

		// Token: 0x04000F1F RID: 3871
		public XmlQualifiedName QnMany;

		// Token: 0x04000F20 RID: 3872
		public XmlQualifiedName QnRequired;

		// Token: 0x04000F21 RID: 3873
		public XmlQualifiedName QnYes;

		// Token: 0x04000F22 RID: 3874
		public XmlQualifiedName QnNo;

		// Token: 0x04000F23 RID: 3875
		public XmlQualifiedName QnString;

		// Token: 0x04000F24 RID: 3876
		public XmlQualifiedName QnID;

		// Token: 0x04000F25 RID: 3877
		public XmlQualifiedName QnIDRef;

		// Token: 0x04000F26 RID: 3878
		public XmlQualifiedName QnIDRefs;

		// Token: 0x04000F27 RID: 3879
		public XmlQualifiedName QnEntity;

		// Token: 0x04000F28 RID: 3880
		public XmlQualifiedName QnEntities;

		// Token: 0x04000F29 RID: 3881
		public XmlQualifiedName QnNmToken;

		// Token: 0x04000F2A RID: 3882
		public XmlQualifiedName QnNmTokens;

		// Token: 0x04000F2B RID: 3883
		public XmlQualifiedName QnEnumeration;

		// Token: 0x04000F2C RID: 3884
		public XmlQualifiedName QnDefault;

		// Token: 0x04000F2D RID: 3885
		public XmlQualifiedName QnXdrSchema;

		// Token: 0x04000F2E RID: 3886
		public XmlQualifiedName QnXdrElementType;

		// Token: 0x04000F2F RID: 3887
		public XmlQualifiedName QnXdrElement;

		// Token: 0x04000F30 RID: 3888
		public XmlQualifiedName QnXdrGroup;

		// Token: 0x04000F31 RID: 3889
		public XmlQualifiedName QnXdrAttributeType;

		// Token: 0x04000F32 RID: 3890
		public XmlQualifiedName QnXdrAttribute;

		// Token: 0x04000F33 RID: 3891
		public XmlQualifiedName QnXdrDataType;

		// Token: 0x04000F34 RID: 3892
		public XmlQualifiedName QnXdrDescription;

		// Token: 0x04000F35 RID: 3893
		public XmlQualifiedName QnXdrExtends;

		// Token: 0x04000F36 RID: 3894
		public XmlQualifiedName QnXdrAliasSchema;

		// Token: 0x04000F37 RID: 3895
		public XmlQualifiedName QnDtType;

		// Token: 0x04000F38 RID: 3896
		public XmlQualifiedName QnDtValues;

		// Token: 0x04000F39 RID: 3897
		public XmlQualifiedName QnDtMaxLength;

		// Token: 0x04000F3A RID: 3898
		public XmlQualifiedName QnDtMinLength;

		// Token: 0x04000F3B RID: 3899
		public XmlQualifiedName QnDtMax;

		// Token: 0x04000F3C RID: 3900
		public XmlQualifiedName QnDtMin;

		// Token: 0x04000F3D RID: 3901
		public XmlQualifiedName QnDtMinExclusive;

		// Token: 0x04000F3E RID: 3902
		public XmlQualifiedName QnDtMaxExclusive;

		// Token: 0x04000F3F RID: 3903
		public XmlQualifiedName QnTargetNamespace;

		// Token: 0x04000F40 RID: 3904
		public XmlQualifiedName QnVersion;

		// Token: 0x04000F41 RID: 3905
		public XmlQualifiedName QnFinalDefault;

		// Token: 0x04000F42 RID: 3906
		public XmlQualifiedName QnBlockDefault;

		// Token: 0x04000F43 RID: 3907
		public XmlQualifiedName QnFixed;

		// Token: 0x04000F44 RID: 3908
		public XmlQualifiedName QnAbstract;

		// Token: 0x04000F45 RID: 3909
		public XmlQualifiedName QnBlock;

		// Token: 0x04000F46 RID: 3910
		public XmlQualifiedName QnSubstitutionGroup;

		// Token: 0x04000F47 RID: 3911
		public XmlQualifiedName QnFinal;

		// Token: 0x04000F48 RID: 3912
		public XmlQualifiedName QnNillable;

		// Token: 0x04000F49 RID: 3913
		public XmlQualifiedName QnRef;

		// Token: 0x04000F4A RID: 3914
		public XmlQualifiedName QnBase;

		// Token: 0x04000F4B RID: 3915
		public XmlQualifiedName QnDerivedBy;

		// Token: 0x04000F4C RID: 3916
		public XmlQualifiedName QnNamespace;

		// Token: 0x04000F4D RID: 3917
		public XmlQualifiedName QnProcessContents;

		// Token: 0x04000F4E RID: 3918
		public XmlQualifiedName QnRefer;

		// Token: 0x04000F4F RID: 3919
		public XmlQualifiedName QnPublic;

		// Token: 0x04000F50 RID: 3920
		public XmlQualifiedName QnSystem;

		// Token: 0x04000F51 RID: 3921
		public XmlQualifiedName QnSchemaLocation;

		// Token: 0x04000F52 RID: 3922
		public XmlQualifiedName QnValue;

		// Token: 0x04000F53 RID: 3923
		public XmlQualifiedName QnUse;

		// Token: 0x04000F54 RID: 3924
		public XmlQualifiedName QnForm;

		// Token: 0x04000F55 RID: 3925
		public XmlQualifiedName QnElementFormDefault;

		// Token: 0x04000F56 RID: 3926
		public XmlQualifiedName QnAttributeFormDefault;

		// Token: 0x04000F57 RID: 3927
		public XmlQualifiedName QnItemType;

		// Token: 0x04000F58 RID: 3928
		public XmlQualifiedName QnMemberTypes;

		// Token: 0x04000F59 RID: 3929
		public XmlQualifiedName QnXPath;

		// Token: 0x04000F5A RID: 3930
		public XmlQualifiedName QnXsdSchema;

		// Token: 0x04000F5B RID: 3931
		public XmlQualifiedName QnXsdAnnotation;

		// Token: 0x04000F5C RID: 3932
		public XmlQualifiedName QnXsdInclude;

		// Token: 0x04000F5D RID: 3933
		public XmlQualifiedName QnXsdImport;

		// Token: 0x04000F5E RID: 3934
		public XmlQualifiedName QnXsdElement;

		// Token: 0x04000F5F RID: 3935
		public XmlQualifiedName QnXsdAttribute;

		// Token: 0x04000F60 RID: 3936
		public XmlQualifiedName QnXsdAttributeGroup;

		// Token: 0x04000F61 RID: 3937
		public XmlQualifiedName QnXsdAnyAttribute;

		// Token: 0x04000F62 RID: 3938
		public XmlQualifiedName QnXsdGroup;

		// Token: 0x04000F63 RID: 3939
		public XmlQualifiedName QnXsdAll;

		// Token: 0x04000F64 RID: 3940
		public XmlQualifiedName QnXsdChoice;

		// Token: 0x04000F65 RID: 3941
		public XmlQualifiedName QnXsdSequence;

		// Token: 0x04000F66 RID: 3942
		public XmlQualifiedName QnXsdAny;

		// Token: 0x04000F67 RID: 3943
		public XmlQualifiedName QnXsdNotation;

		// Token: 0x04000F68 RID: 3944
		public XmlQualifiedName QnXsdSimpleType;

		// Token: 0x04000F69 RID: 3945
		public XmlQualifiedName QnXsdComplexType;

		// Token: 0x04000F6A RID: 3946
		public XmlQualifiedName QnXsdUnique;

		// Token: 0x04000F6B RID: 3947
		public XmlQualifiedName QnXsdKey;

		// Token: 0x04000F6C RID: 3948
		public XmlQualifiedName QnXsdKeyRef;

		// Token: 0x04000F6D RID: 3949
		public XmlQualifiedName QnXsdSelector;

		// Token: 0x04000F6E RID: 3950
		public XmlQualifiedName QnXsdField;

		// Token: 0x04000F6F RID: 3951
		public XmlQualifiedName QnXsdMinExclusive;

		// Token: 0x04000F70 RID: 3952
		public XmlQualifiedName QnXsdMinInclusive;

		// Token: 0x04000F71 RID: 3953
		public XmlQualifiedName QnXsdMaxInclusive;

		// Token: 0x04000F72 RID: 3954
		public XmlQualifiedName QnXsdMaxExclusive;

		// Token: 0x04000F73 RID: 3955
		public XmlQualifiedName QnXsdTotalDigits;

		// Token: 0x04000F74 RID: 3956
		public XmlQualifiedName QnXsdFractionDigits;

		// Token: 0x04000F75 RID: 3957
		public XmlQualifiedName QnXsdLength;

		// Token: 0x04000F76 RID: 3958
		public XmlQualifiedName QnXsdMinLength;

		// Token: 0x04000F77 RID: 3959
		public XmlQualifiedName QnXsdMaxLength;

		// Token: 0x04000F78 RID: 3960
		public XmlQualifiedName QnXsdEnumeration;

		// Token: 0x04000F79 RID: 3961
		public XmlQualifiedName QnXsdPattern;

		// Token: 0x04000F7A RID: 3962
		public XmlQualifiedName QnXsdDocumentation;

		// Token: 0x04000F7B RID: 3963
		public XmlQualifiedName QnXsdAppinfo;

		// Token: 0x04000F7C RID: 3964
		public XmlQualifiedName QnSource;

		// Token: 0x04000F7D RID: 3965
		public XmlQualifiedName QnXsdComplexContent;

		// Token: 0x04000F7E RID: 3966
		public XmlQualifiedName QnXsdSimpleContent;

		// Token: 0x04000F7F RID: 3967
		public XmlQualifiedName QnXsdRestriction;

		// Token: 0x04000F80 RID: 3968
		public XmlQualifiedName QnXsdExtension;

		// Token: 0x04000F81 RID: 3969
		public XmlQualifiedName QnXsdUnion;

		// Token: 0x04000F82 RID: 3970
		public XmlQualifiedName QnXsdList;

		// Token: 0x04000F83 RID: 3971
		public XmlQualifiedName QnXsdWhiteSpace;

		// Token: 0x04000F84 RID: 3972
		public XmlQualifiedName QnXsdRedefine;

		// Token: 0x04000F85 RID: 3973
		public XmlQualifiedName QnXsdAnyType;

		// Token: 0x04000F86 RID: 3974
		internal XmlQualifiedName[] TokenToQName = new XmlQualifiedName[123];

		// Token: 0x02000215 RID: 533
		public enum Token
		{
			// Token: 0x04000F88 RID: 3976
			Empty,
			// Token: 0x04000F89 RID: 3977
			SchemaName,
			// Token: 0x04000F8A RID: 3978
			SchemaType,
			// Token: 0x04000F8B RID: 3979
			SchemaMaxOccurs,
			// Token: 0x04000F8C RID: 3980
			SchemaMinOccurs,
			// Token: 0x04000F8D RID: 3981
			SchemaInfinite,
			// Token: 0x04000F8E RID: 3982
			SchemaModel,
			// Token: 0x04000F8F RID: 3983
			SchemaOpen,
			// Token: 0x04000F90 RID: 3984
			SchemaClosed,
			// Token: 0x04000F91 RID: 3985
			SchemaContent,
			// Token: 0x04000F92 RID: 3986
			SchemaMixed,
			// Token: 0x04000F93 RID: 3987
			SchemaEmpty,
			// Token: 0x04000F94 RID: 3988
			SchemaElementOnly,
			// Token: 0x04000F95 RID: 3989
			SchemaTextOnly,
			// Token: 0x04000F96 RID: 3990
			SchemaOrder,
			// Token: 0x04000F97 RID: 3991
			SchemaSeq,
			// Token: 0x04000F98 RID: 3992
			SchemaOne,
			// Token: 0x04000F99 RID: 3993
			SchemaMany,
			// Token: 0x04000F9A RID: 3994
			SchemaRequired,
			// Token: 0x04000F9B RID: 3995
			SchemaYes,
			// Token: 0x04000F9C RID: 3996
			SchemaNo,
			// Token: 0x04000F9D RID: 3997
			SchemaString,
			// Token: 0x04000F9E RID: 3998
			SchemaId,
			// Token: 0x04000F9F RID: 3999
			SchemaIdref,
			// Token: 0x04000FA0 RID: 4000
			SchemaIdrefs,
			// Token: 0x04000FA1 RID: 4001
			SchemaEntity,
			// Token: 0x04000FA2 RID: 4002
			SchemaEntities,
			// Token: 0x04000FA3 RID: 4003
			SchemaNmtoken,
			// Token: 0x04000FA4 RID: 4004
			SchemaNmtokens,
			// Token: 0x04000FA5 RID: 4005
			SchemaEnumeration,
			// Token: 0x04000FA6 RID: 4006
			SchemaDefault,
			// Token: 0x04000FA7 RID: 4007
			XdrRoot,
			// Token: 0x04000FA8 RID: 4008
			XdrElementType,
			// Token: 0x04000FA9 RID: 4009
			XdrElement,
			// Token: 0x04000FAA RID: 4010
			XdrGroup,
			// Token: 0x04000FAB RID: 4011
			XdrAttributeType,
			// Token: 0x04000FAC RID: 4012
			XdrAttribute,
			// Token: 0x04000FAD RID: 4013
			XdrDatatype,
			// Token: 0x04000FAE RID: 4014
			XdrDescription,
			// Token: 0x04000FAF RID: 4015
			XdrExtends,
			// Token: 0x04000FB0 RID: 4016
			SchemaXdrRootAlias,
			// Token: 0x04000FB1 RID: 4017
			SchemaDtType,
			// Token: 0x04000FB2 RID: 4018
			SchemaDtValues,
			// Token: 0x04000FB3 RID: 4019
			SchemaDtMaxLength,
			// Token: 0x04000FB4 RID: 4020
			SchemaDtMinLength,
			// Token: 0x04000FB5 RID: 4021
			SchemaDtMax,
			// Token: 0x04000FB6 RID: 4022
			SchemaDtMin,
			// Token: 0x04000FB7 RID: 4023
			SchemaDtMinExclusive,
			// Token: 0x04000FB8 RID: 4024
			SchemaDtMaxExclusive,
			// Token: 0x04000FB9 RID: 4025
			SchemaTargetNamespace,
			// Token: 0x04000FBA RID: 4026
			SchemaVersion,
			// Token: 0x04000FBB RID: 4027
			SchemaFinalDefault,
			// Token: 0x04000FBC RID: 4028
			SchemaBlockDefault,
			// Token: 0x04000FBD RID: 4029
			SchemaFixed,
			// Token: 0x04000FBE RID: 4030
			SchemaAbstract,
			// Token: 0x04000FBF RID: 4031
			SchemaBlock,
			// Token: 0x04000FC0 RID: 4032
			SchemaSubstitutionGroup,
			// Token: 0x04000FC1 RID: 4033
			SchemaFinal,
			// Token: 0x04000FC2 RID: 4034
			SchemaNillable,
			// Token: 0x04000FC3 RID: 4035
			SchemaRef,
			// Token: 0x04000FC4 RID: 4036
			SchemaBase,
			// Token: 0x04000FC5 RID: 4037
			SchemaDerivedBy,
			// Token: 0x04000FC6 RID: 4038
			SchemaNamespace,
			// Token: 0x04000FC7 RID: 4039
			SchemaProcessContents,
			// Token: 0x04000FC8 RID: 4040
			SchemaRefer,
			// Token: 0x04000FC9 RID: 4041
			SchemaPublic,
			// Token: 0x04000FCA RID: 4042
			SchemaSystem,
			// Token: 0x04000FCB RID: 4043
			SchemaSchemaLocation,
			// Token: 0x04000FCC RID: 4044
			SchemaValue,
			// Token: 0x04000FCD RID: 4045
			SchemaSource,
			// Token: 0x04000FCE RID: 4046
			SchemaAttributeFormDefault,
			// Token: 0x04000FCF RID: 4047
			SchemaElementFormDefault,
			// Token: 0x04000FD0 RID: 4048
			SchemaUse,
			// Token: 0x04000FD1 RID: 4049
			SchemaForm,
			// Token: 0x04000FD2 RID: 4050
			XsdSchema,
			// Token: 0x04000FD3 RID: 4051
			XsdAnnotation,
			// Token: 0x04000FD4 RID: 4052
			XsdInclude,
			// Token: 0x04000FD5 RID: 4053
			XsdImport,
			// Token: 0x04000FD6 RID: 4054
			XsdElement,
			// Token: 0x04000FD7 RID: 4055
			XsdAttribute,
			// Token: 0x04000FD8 RID: 4056
			xsdAttributeGroup,
			// Token: 0x04000FD9 RID: 4057
			XsdAnyAttribute,
			// Token: 0x04000FDA RID: 4058
			XsdGroup,
			// Token: 0x04000FDB RID: 4059
			XsdAll,
			// Token: 0x04000FDC RID: 4060
			XsdChoice,
			// Token: 0x04000FDD RID: 4061
			XsdSequence,
			// Token: 0x04000FDE RID: 4062
			XsdAny,
			// Token: 0x04000FDF RID: 4063
			XsdNotation,
			// Token: 0x04000FE0 RID: 4064
			XsdSimpleType,
			// Token: 0x04000FE1 RID: 4065
			XsdComplexType,
			// Token: 0x04000FE2 RID: 4066
			XsdUnique,
			// Token: 0x04000FE3 RID: 4067
			XsdKey,
			// Token: 0x04000FE4 RID: 4068
			XsdKeyref,
			// Token: 0x04000FE5 RID: 4069
			XsdSelector,
			// Token: 0x04000FE6 RID: 4070
			XsdField,
			// Token: 0x04000FE7 RID: 4071
			XsdMinExclusive,
			// Token: 0x04000FE8 RID: 4072
			XsdMinInclusive,
			// Token: 0x04000FE9 RID: 4073
			XsdMaxExclusive,
			// Token: 0x04000FEA RID: 4074
			XsdMaxInclusive,
			// Token: 0x04000FEB RID: 4075
			XsdTotalDigits,
			// Token: 0x04000FEC RID: 4076
			XsdFractionDigits,
			// Token: 0x04000FED RID: 4077
			XsdLength,
			// Token: 0x04000FEE RID: 4078
			XsdMinLength,
			// Token: 0x04000FEF RID: 4079
			XsdMaxLength,
			// Token: 0x04000FF0 RID: 4080
			XsdEnumeration,
			// Token: 0x04000FF1 RID: 4081
			XsdPattern,
			// Token: 0x04000FF2 RID: 4082
			XsdDocumentation,
			// Token: 0x04000FF3 RID: 4083
			XsdAppInfo,
			// Token: 0x04000FF4 RID: 4084
			XsdComplexContent,
			// Token: 0x04000FF5 RID: 4085
			XsdComplexContentExtension,
			// Token: 0x04000FF6 RID: 4086
			XsdComplexContentRestriction,
			// Token: 0x04000FF7 RID: 4087
			XsdSimpleContent,
			// Token: 0x04000FF8 RID: 4088
			XsdSimpleContentExtension,
			// Token: 0x04000FF9 RID: 4089
			XsdSimpleContentRestriction,
			// Token: 0x04000FFA RID: 4090
			XsdSimpleTypeList,
			// Token: 0x04000FFB RID: 4091
			XsdSimpleTypeRestriction,
			// Token: 0x04000FFC RID: 4092
			XsdSimpleTypeUnion,
			// Token: 0x04000FFD RID: 4093
			XsdWhitespace,
			// Token: 0x04000FFE RID: 4094
			XsdRedefine,
			// Token: 0x04000FFF RID: 4095
			SchemaItemType,
			// Token: 0x04001000 RID: 4096
			SchemaMemberTypes,
			// Token: 0x04001001 RID: 4097
			SchemaXPath,
			// Token: 0x04001002 RID: 4098
			XmlLang
		}
	}
}
