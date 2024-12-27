using System;
using System.Collections;

namespace System.Xml.Schema
{
	// Token: 0x020001AC RID: 428
	internal abstract class DatatypeImplementation : XmlSchemaDatatype
	{
		// Token: 0x060015E0 RID: 5600 RVA: 0x00061020 File Offset: 0x00060020
		static DatatypeImplementation()
		{
			DatatypeImplementation[] array = new DatatypeImplementation[13];
			array[0] = DatatypeImplementation.c_string;
			array[1] = DatatypeImplementation.c_ID;
			array[2] = DatatypeImplementation.c_IDREF;
			array[3] = DatatypeImplementation.c_IDREFS;
			array[4] = DatatypeImplementation.c_ENTITY;
			array[5] = DatatypeImplementation.c_ENTITIES;
			array[6] = DatatypeImplementation.c_NMTOKEN;
			array[7] = DatatypeImplementation.c_NMTOKENS;
			array[8] = DatatypeImplementation.c_NOTATION;
			array[9] = DatatypeImplementation.c_ENUMERATION;
			array[10] = DatatypeImplementation.c_QNameXdr;
			array[11] = DatatypeImplementation.c_NCName;
			DatatypeImplementation.c_tokenizedTypes = array;
			DatatypeImplementation[] array2 = new DatatypeImplementation[13];
			array2[0] = DatatypeImplementation.c_string;
			array2[1] = DatatypeImplementation.c_ID;
			array2[2] = DatatypeImplementation.c_IDREF;
			array2[3] = DatatypeImplementation.c_IDREFS;
			array2[4] = DatatypeImplementation.c_ENTITY;
			array2[5] = DatatypeImplementation.c_ENTITIES;
			array2[6] = DatatypeImplementation.c_NMTOKEN;
			array2[7] = DatatypeImplementation.c_NMTOKENS;
			array2[8] = DatatypeImplementation.c_NOTATION;
			array2[9] = DatatypeImplementation.c_ENUMERATION;
			array2[10] = DatatypeImplementation.c_QName;
			array2[11] = DatatypeImplementation.c_NCName;
			DatatypeImplementation.c_tokenizedTypesXsd = array2;
			DatatypeImplementation.c_XdrTypes = new DatatypeImplementation.SchemaDatatypeMap[]
			{
				new DatatypeImplementation.SchemaDatatypeMap("bin.base64", DatatypeImplementation.c_base64Binary),
				new DatatypeImplementation.SchemaDatatypeMap("bin.hex", DatatypeImplementation.c_hexBinary),
				new DatatypeImplementation.SchemaDatatypeMap("boolean", DatatypeImplementation.c_boolean),
				new DatatypeImplementation.SchemaDatatypeMap("char", DatatypeImplementation.c_char),
				new DatatypeImplementation.SchemaDatatypeMap("date", DatatypeImplementation.c_date),
				new DatatypeImplementation.SchemaDatatypeMap("dateTime", DatatypeImplementation.c_dateTimeNoTz),
				new DatatypeImplementation.SchemaDatatypeMap("dateTime.tz", DatatypeImplementation.c_dateTimeTz),
				new DatatypeImplementation.SchemaDatatypeMap("decimal", DatatypeImplementation.c_decimal),
				new DatatypeImplementation.SchemaDatatypeMap("entities", DatatypeImplementation.c_ENTITIES),
				new DatatypeImplementation.SchemaDatatypeMap("entity", DatatypeImplementation.c_ENTITY),
				new DatatypeImplementation.SchemaDatatypeMap("enumeration", DatatypeImplementation.c_ENUMERATION),
				new DatatypeImplementation.SchemaDatatypeMap("fixed.14.4", DatatypeImplementation.c_fixed),
				new DatatypeImplementation.SchemaDatatypeMap("float", DatatypeImplementation.c_doubleXdr),
				new DatatypeImplementation.SchemaDatatypeMap("float.ieee.754.32", DatatypeImplementation.c_floatXdr),
				new DatatypeImplementation.SchemaDatatypeMap("float.ieee.754.64", DatatypeImplementation.c_doubleXdr),
				new DatatypeImplementation.SchemaDatatypeMap("i1", DatatypeImplementation.c_byte),
				new DatatypeImplementation.SchemaDatatypeMap("i2", DatatypeImplementation.c_short),
				new DatatypeImplementation.SchemaDatatypeMap("i4", DatatypeImplementation.c_int),
				new DatatypeImplementation.SchemaDatatypeMap("i8", DatatypeImplementation.c_long),
				new DatatypeImplementation.SchemaDatatypeMap("id", DatatypeImplementation.c_ID),
				new DatatypeImplementation.SchemaDatatypeMap("idref", DatatypeImplementation.c_IDREF),
				new DatatypeImplementation.SchemaDatatypeMap("idrefs", DatatypeImplementation.c_IDREFS),
				new DatatypeImplementation.SchemaDatatypeMap("int", DatatypeImplementation.c_int),
				new DatatypeImplementation.SchemaDatatypeMap("nmtoken", DatatypeImplementation.c_NMTOKEN),
				new DatatypeImplementation.SchemaDatatypeMap("nmtokens", DatatypeImplementation.c_NMTOKENS),
				new DatatypeImplementation.SchemaDatatypeMap("notation", DatatypeImplementation.c_NOTATION),
				new DatatypeImplementation.SchemaDatatypeMap("number", DatatypeImplementation.c_doubleXdr),
				new DatatypeImplementation.SchemaDatatypeMap("r4", DatatypeImplementation.c_floatXdr),
				new DatatypeImplementation.SchemaDatatypeMap("r8", DatatypeImplementation.c_doubleXdr),
				new DatatypeImplementation.SchemaDatatypeMap("string", DatatypeImplementation.c_string),
				new DatatypeImplementation.SchemaDatatypeMap("time", DatatypeImplementation.c_timeNoTz),
				new DatatypeImplementation.SchemaDatatypeMap("time.tz", DatatypeImplementation.c_timeTz),
				new DatatypeImplementation.SchemaDatatypeMap("ui1", DatatypeImplementation.c_unsignedByte),
				new DatatypeImplementation.SchemaDatatypeMap("ui2", DatatypeImplementation.c_unsignedShort),
				new DatatypeImplementation.SchemaDatatypeMap("ui4", DatatypeImplementation.c_unsignedInt),
				new DatatypeImplementation.SchemaDatatypeMap("ui8", DatatypeImplementation.c_unsignedLong),
				new DatatypeImplementation.SchemaDatatypeMap("uri", DatatypeImplementation.c_anyURI),
				new DatatypeImplementation.SchemaDatatypeMap("uuid", DatatypeImplementation.c_uuid)
			};
			DatatypeImplementation.c_XsdTypes = new DatatypeImplementation.SchemaDatatypeMap[]
			{
				new DatatypeImplementation.SchemaDatatypeMap("ENTITIES", DatatypeImplementation.c_ENTITIES, 11),
				new DatatypeImplementation.SchemaDatatypeMap("ENTITY", DatatypeImplementation.c_ENTITY, 11),
				new DatatypeImplementation.SchemaDatatypeMap("ID", DatatypeImplementation.c_ID, 5),
				new DatatypeImplementation.SchemaDatatypeMap("IDREF", DatatypeImplementation.c_IDREF, 5),
				new DatatypeImplementation.SchemaDatatypeMap("IDREFS", DatatypeImplementation.c_IDREFS, 11),
				new DatatypeImplementation.SchemaDatatypeMap("NCName", DatatypeImplementation.c_NCName, 9),
				new DatatypeImplementation.SchemaDatatypeMap("NMTOKEN", DatatypeImplementation.c_NMTOKEN, 40),
				new DatatypeImplementation.SchemaDatatypeMap("NMTOKENS", DatatypeImplementation.c_NMTOKENS, 11),
				new DatatypeImplementation.SchemaDatatypeMap("NOTATION", DatatypeImplementation.c_NOTATION, 11),
				new DatatypeImplementation.SchemaDatatypeMap("Name", DatatypeImplementation.c_Name, 40),
				new DatatypeImplementation.SchemaDatatypeMap("QName", DatatypeImplementation.c_QName, 11),
				new DatatypeImplementation.SchemaDatatypeMap("anySimpleType", DatatypeImplementation.c_anySimpleType, -1),
				new DatatypeImplementation.SchemaDatatypeMap("anyURI", DatatypeImplementation.c_anyURI, 11),
				new DatatypeImplementation.SchemaDatatypeMap("base64Binary", DatatypeImplementation.c_base64Binary, 11),
				new DatatypeImplementation.SchemaDatatypeMap("boolean", DatatypeImplementation.c_boolean, 11),
				new DatatypeImplementation.SchemaDatatypeMap("byte", DatatypeImplementation.c_byte, 37),
				new DatatypeImplementation.SchemaDatatypeMap("date", DatatypeImplementation.c_date, 11),
				new DatatypeImplementation.SchemaDatatypeMap("dateTime", DatatypeImplementation.c_dateTime, 11),
				new DatatypeImplementation.SchemaDatatypeMap("decimal", DatatypeImplementation.c_decimal, 11),
				new DatatypeImplementation.SchemaDatatypeMap("double", DatatypeImplementation.c_double, 11),
				new DatatypeImplementation.SchemaDatatypeMap("duration", DatatypeImplementation.c_duration, 11),
				new DatatypeImplementation.SchemaDatatypeMap("float", DatatypeImplementation.c_float, 11),
				new DatatypeImplementation.SchemaDatatypeMap("gDay", DatatypeImplementation.c_day, 11),
				new DatatypeImplementation.SchemaDatatypeMap("gMonth", DatatypeImplementation.c_month, 11),
				new DatatypeImplementation.SchemaDatatypeMap("gMonthDay", DatatypeImplementation.c_monthDay, 11),
				new DatatypeImplementation.SchemaDatatypeMap("gYear", DatatypeImplementation.c_year, 11),
				new DatatypeImplementation.SchemaDatatypeMap("gYearMonth", DatatypeImplementation.c_yearMonth, 11),
				new DatatypeImplementation.SchemaDatatypeMap("hexBinary", DatatypeImplementation.c_hexBinary, 11),
				new DatatypeImplementation.SchemaDatatypeMap("int", DatatypeImplementation.c_int, 31),
				new DatatypeImplementation.SchemaDatatypeMap("integer", DatatypeImplementation.c_integer, 18),
				new DatatypeImplementation.SchemaDatatypeMap("language", DatatypeImplementation.c_language, 40),
				new DatatypeImplementation.SchemaDatatypeMap("long", DatatypeImplementation.c_long, 29),
				new DatatypeImplementation.SchemaDatatypeMap("negativeInteger", DatatypeImplementation.c_negativeInteger, 34),
				new DatatypeImplementation.SchemaDatatypeMap("nonNegativeInteger", DatatypeImplementation.c_nonNegativeInteger, 29),
				new DatatypeImplementation.SchemaDatatypeMap("nonPositiveInteger", DatatypeImplementation.c_nonPositiveInteger, 29),
				new DatatypeImplementation.SchemaDatatypeMap("normalizedString", DatatypeImplementation.c_normalizedString, 38),
				new DatatypeImplementation.SchemaDatatypeMap("positiveInteger", DatatypeImplementation.c_positiveInteger, 33),
				new DatatypeImplementation.SchemaDatatypeMap("short", DatatypeImplementation.c_short, 28),
				new DatatypeImplementation.SchemaDatatypeMap("string", DatatypeImplementation.c_string, 11),
				new DatatypeImplementation.SchemaDatatypeMap("time", DatatypeImplementation.c_time, 11),
				new DatatypeImplementation.SchemaDatatypeMap("token", DatatypeImplementation.c_token, 35),
				new DatatypeImplementation.SchemaDatatypeMap("unsignedByte", DatatypeImplementation.c_unsignedByte, 44),
				new DatatypeImplementation.SchemaDatatypeMap("unsignedInt", DatatypeImplementation.c_unsignedInt, 43),
				new DatatypeImplementation.SchemaDatatypeMap("unsignedLong", DatatypeImplementation.c_unsignedLong, 33),
				new DatatypeImplementation.SchemaDatatypeMap("unsignedShort", DatatypeImplementation.c_unsignedShort, 42)
			};
			DatatypeImplementation.CreateBuiltinTypes();
		}

		// Token: 0x1700053E RID: 1342
		// (get) Token: 0x060015E1 RID: 5601 RVA: 0x00061AC6 File Offset: 0x00060AC6
		internal static XmlSchemaSimpleType AnySimpleType
		{
			get
			{
				return DatatypeImplementation.anySimpleType;
			}
		}

		// Token: 0x1700053F RID: 1343
		// (get) Token: 0x060015E2 RID: 5602 RVA: 0x00061ACD File Offset: 0x00060ACD
		internal static XmlSchemaSimpleType AnyAtomicType
		{
			get
			{
				return DatatypeImplementation.anyAtomicType;
			}
		}

		// Token: 0x17000540 RID: 1344
		// (get) Token: 0x060015E3 RID: 5603 RVA: 0x00061AD4 File Offset: 0x00060AD4
		internal static XmlSchemaSimpleType UntypedAtomicType
		{
			get
			{
				return DatatypeImplementation.untypedAtomicType;
			}
		}

		// Token: 0x17000541 RID: 1345
		// (get) Token: 0x060015E4 RID: 5604 RVA: 0x00061ADB File Offset: 0x00060ADB
		internal static XmlSchemaSimpleType YearMonthDurationType
		{
			get
			{
				return DatatypeImplementation.yearMonthDurationType;
			}
		}

		// Token: 0x17000542 RID: 1346
		// (get) Token: 0x060015E5 RID: 5605 RVA: 0x00061AE2 File Offset: 0x00060AE2
		internal static XmlSchemaSimpleType DayTimeDurationType
		{
			get
			{
				return DatatypeImplementation.dayTimeDurationType;
			}
		}

		// Token: 0x060015E6 RID: 5606 RVA: 0x00061AE9 File Offset: 0x00060AE9
		internal new static DatatypeImplementation FromXmlTokenizedType(XmlTokenizedType token)
		{
			return DatatypeImplementation.c_tokenizedTypes[(int)token];
		}

		// Token: 0x060015E7 RID: 5607 RVA: 0x00061AF2 File Offset: 0x00060AF2
		internal new static DatatypeImplementation FromXmlTokenizedTypeXsd(XmlTokenizedType token)
		{
			return DatatypeImplementation.c_tokenizedTypesXsd[(int)token];
		}

		// Token: 0x060015E8 RID: 5608 RVA: 0x00061AFC File Offset: 0x00060AFC
		internal new static DatatypeImplementation FromXdrName(string name)
		{
			int num = Array.BinarySearch(DatatypeImplementation.c_XdrTypes, name, null);
			if (num >= 0)
			{
				return (DatatypeImplementation)DatatypeImplementation.c_XdrTypes[num];
			}
			return null;
		}

		// Token: 0x060015E9 RID: 5609 RVA: 0x00061B28 File Offset: 0x00060B28
		private static DatatypeImplementation FromTypeName(string name)
		{
			int num = Array.BinarySearch(DatatypeImplementation.c_XsdTypes, name, null);
			if (num >= 0)
			{
				return (DatatypeImplementation)DatatypeImplementation.c_XsdTypes[num];
			}
			return null;
		}

		// Token: 0x060015EA RID: 5610 RVA: 0x00061B54 File Offset: 0x00060B54
		internal static XmlSchemaSimpleType StartBuiltinType(XmlQualifiedName qname, XmlSchemaDatatype dataType)
		{
			XmlSchemaSimpleType xmlSchemaSimpleType = new XmlSchemaSimpleType();
			xmlSchemaSimpleType.SetQualifiedName(qname);
			xmlSchemaSimpleType.SetDatatype(dataType);
			xmlSchemaSimpleType.ElementDecl = new SchemaElementDecl(dataType);
			xmlSchemaSimpleType.ElementDecl.SchemaType = xmlSchemaSimpleType;
			return xmlSchemaSimpleType;
		}

		// Token: 0x060015EB RID: 5611 RVA: 0x00061B90 File Offset: 0x00060B90
		internal static void FinishBuiltinType(XmlSchemaSimpleType derivedType, XmlSchemaSimpleType baseType)
		{
			derivedType.SetBaseSchemaType(baseType);
			derivedType.SetDerivedBy(XmlSchemaDerivationMethod.Restriction);
			if (derivedType.Datatype.Variety == XmlSchemaDatatypeVariety.Atomic)
			{
				derivedType.Content = new XmlSchemaSimpleTypeRestriction
				{
					BaseTypeName = baseType.QualifiedName
				};
			}
			if (derivedType.Datatype.Variety == XmlSchemaDatatypeVariety.List)
			{
				XmlSchemaSimpleTypeList xmlSchemaSimpleTypeList = new XmlSchemaSimpleTypeList();
				derivedType.SetDerivedBy(XmlSchemaDerivationMethod.List);
				XmlTypeCode typeCode = derivedType.Datatype.TypeCode;
				if (typeCode != XmlTypeCode.NmToken)
				{
					switch (typeCode)
					{
					case XmlTypeCode.Idref:
						xmlSchemaSimpleTypeList.ItemType = (xmlSchemaSimpleTypeList.BaseItemType = DatatypeImplementation.enumToTypeCode[38]);
						break;
					case XmlTypeCode.Entity:
						xmlSchemaSimpleTypeList.ItemType = (xmlSchemaSimpleTypeList.BaseItemType = DatatypeImplementation.enumToTypeCode[39]);
						break;
					}
				}
				else
				{
					xmlSchemaSimpleTypeList.ItemType = (xmlSchemaSimpleTypeList.BaseItemType = DatatypeImplementation.enumToTypeCode[34]);
				}
				derivedType.Content = xmlSchemaSimpleTypeList;
			}
		}

		// Token: 0x060015EC RID: 5612 RVA: 0x00061C68 File Offset: 0x00060C68
		internal static void CreateBuiltinTypes()
		{
			DatatypeImplementation.SchemaDatatypeMap schemaDatatypeMap = DatatypeImplementation.c_XsdTypes[11];
			XmlQualifiedName xmlQualifiedName = new XmlQualifiedName(schemaDatatypeMap.Name, "http://www.w3.org/2001/XMLSchema");
			DatatypeImplementation datatypeImplementation = DatatypeImplementation.FromTypeName(xmlQualifiedName.Name);
			DatatypeImplementation.anySimpleType = DatatypeImplementation.StartBuiltinType(xmlQualifiedName, datatypeImplementation);
			datatypeImplementation.parentSchemaType = DatatypeImplementation.anySimpleType;
			DatatypeImplementation.builtinTypes.Add(xmlQualifiedName, DatatypeImplementation.anySimpleType);
			for (int i = 0; i < DatatypeImplementation.c_XsdTypes.Length; i++)
			{
				if (i != 11)
				{
					schemaDatatypeMap = DatatypeImplementation.c_XsdTypes[i];
					xmlQualifiedName = new XmlQualifiedName(schemaDatatypeMap.Name, "http://www.w3.org/2001/XMLSchema");
					datatypeImplementation = DatatypeImplementation.FromTypeName(xmlQualifiedName.Name);
					XmlSchemaSimpleType xmlSchemaSimpleType = DatatypeImplementation.StartBuiltinType(xmlQualifiedName, datatypeImplementation);
					datatypeImplementation.parentSchemaType = xmlSchemaSimpleType;
					DatatypeImplementation.builtinTypes.Add(xmlQualifiedName, xmlSchemaSimpleType);
					if (datatypeImplementation.variety == XmlSchemaDatatypeVariety.Atomic)
					{
						DatatypeImplementation.enumToTypeCode[(int)datatypeImplementation.TypeCode] = xmlSchemaSimpleType;
					}
				}
			}
			for (int j = 0; j < DatatypeImplementation.c_XsdTypes.Length; j++)
			{
				if (j != 11)
				{
					schemaDatatypeMap = DatatypeImplementation.c_XsdTypes[j];
					XmlSchemaSimpleType xmlSchemaSimpleType2 = (XmlSchemaSimpleType)DatatypeImplementation.builtinTypes[new XmlQualifiedName(schemaDatatypeMap.Name, "http://www.w3.org/2001/XMLSchema")];
					if (schemaDatatypeMap.ParentIndex == 11)
					{
						DatatypeImplementation.FinishBuiltinType(xmlSchemaSimpleType2, DatatypeImplementation.anySimpleType);
					}
					else
					{
						XmlSchemaSimpleType xmlSchemaSimpleType3 = (XmlSchemaSimpleType)DatatypeImplementation.builtinTypes[new XmlQualifiedName(DatatypeImplementation.c_XsdTypes[schemaDatatypeMap.ParentIndex].Name, "http://www.w3.org/2001/XMLSchema")];
						DatatypeImplementation.FinishBuiltinType(xmlSchemaSimpleType2, xmlSchemaSimpleType3);
					}
				}
			}
			xmlQualifiedName = new XmlQualifiedName("anyAtomicType", "http://www.w3.org/2003/11/xpath-datatypes");
			DatatypeImplementation.anyAtomicType = DatatypeImplementation.StartBuiltinType(xmlQualifiedName, DatatypeImplementation.c_anyAtomicType);
			DatatypeImplementation.c_anyAtomicType.parentSchemaType = DatatypeImplementation.anyAtomicType;
			DatatypeImplementation.FinishBuiltinType(DatatypeImplementation.anyAtomicType, DatatypeImplementation.anySimpleType);
			DatatypeImplementation.builtinTypes.Add(xmlQualifiedName, DatatypeImplementation.anyAtomicType);
			DatatypeImplementation.enumToTypeCode[10] = DatatypeImplementation.anyAtomicType;
			xmlQualifiedName = new XmlQualifiedName("untypedAtomic", "http://www.w3.org/2003/11/xpath-datatypes");
			DatatypeImplementation.untypedAtomicType = DatatypeImplementation.StartBuiltinType(xmlQualifiedName, DatatypeImplementation.c_untypedAtomicType);
			DatatypeImplementation.c_untypedAtomicType.parentSchemaType = DatatypeImplementation.untypedAtomicType;
			DatatypeImplementation.FinishBuiltinType(DatatypeImplementation.untypedAtomicType, DatatypeImplementation.anyAtomicType);
			DatatypeImplementation.builtinTypes.Add(xmlQualifiedName, DatatypeImplementation.untypedAtomicType);
			DatatypeImplementation.enumToTypeCode[11] = DatatypeImplementation.untypedAtomicType;
			xmlQualifiedName = new XmlQualifiedName("yearMonthDuration", "http://www.w3.org/2003/11/xpath-datatypes");
			DatatypeImplementation.yearMonthDurationType = DatatypeImplementation.StartBuiltinType(xmlQualifiedName, DatatypeImplementation.c_yearMonthDuration);
			DatatypeImplementation.c_yearMonthDuration.parentSchemaType = DatatypeImplementation.yearMonthDurationType;
			DatatypeImplementation.FinishBuiltinType(DatatypeImplementation.yearMonthDurationType, DatatypeImplementation.enumToTypeCode[17]);
			DatatypeImplementation.builtinTypes.Add(xmlQualifiedName, DatatypeImplementation.yearMonthDurationType);
			DatatypeImplementation.enumToTypeCode[53] = DatatypeImplementation.yearMonthDurationType;
			xmlQualifiedName = new XmlQualifiedName("dayTimeDuration", "http://www.w3.org/2003/11/xpath-datatypes");
			DatatypeImplementation.dayTimeDurationType = DatatypeImplementation.StartBuiltinType(xmlQualifiedName, DatatypeImplementation.c_dayTimeDuration);
			DatatypeImplementation.c_dayTimeDuration.parentSchemaType = DatatypeImplementation.dayTimeDurationType;
			DatatypeImplementation.FinishBuiltinType(DatatypeImplementation.dayTimeDurationType, DatatypeImplementation.enumToTypeCode[17]);
			DatatypeImplementation.builtinTypes.Add(xmlQualifiedName, DatatypeImplementation.dayTimeDurationType);
			DatatypeImplementation.enumToTypeCode[54] = DatatypeImplementation.dayTimeDurationType;
		}

		// Token: 0x060015ED RID: 5613 RVA: 0x00061F3F File Offset: 0x00060F3F
		internal static XmlSchemaSimpleType GetSimpleTypeFromTypeCode(XmlTypeCode typeCode)
		{
			return DatatypeImplementation.enumToTypeCode[(int)typeCode];
		}

		// Token: 0x060015EE RID: 5614 RVA: 0x00061F48 File Offset: 0x00060F48
		internal static XmlSchemaSimpleType GetSimpleTypeFromXsdType(XmlQualifiedName qname)
		{
			return (XmlSchemaSimpleType)DatatypeImplementation.builtinTypes[qname];
		}

		// Token: 0x060015EF RID: 5615 RVA: 0x00061F5C File Offset: 0x00060F5C
		internal static XmlSchemaSimpleType GetNormalizedStringTypeV1Compat()
		{
			if (DatatypeImplementation.normalizedStringTypeV1Compat == null)
			{
				XmlSchemaSimpleType simpleTypeFromTypeCode = DatatypeImplementation.GetSimpleTypeFromTypeCode(XmlTypeCode.NormalizedString);
				DatatypeImplementation.normalizedStringTypeV1Compat = simpleTypeFromTypeCode.Clone() as XmlSchemaSimpleType;
				DatatypeImplementation.normalizedStringTypeV1Compat.SetDatatype(DatatypeImplementation.c_normalizedStringV1Compat);
				DatatypeImplementation.normalizedStringTypeV1Compat.ElementDecl = new SchemaElementDecl(DatatypeImplementation.c_normalizedStringV1Compat);
				DatatypeImplementation.normalizedStringTypeV1Compat.ElementDecl.SchemaType = DatatypeImplementation.normalizedStringTypeV1Compat;
			}
			return DatatypeImplementation.normalizedStringTypeV1Compat;
		}

		// Token: 0x060015F0 RID: 5616 RVA: 0x00061FC4 File Offset: 0x00060FC4
		internal static XmlSchemaSimpleType GetTokenTypeV1Compat()
		{
			if (DatatypeImplementation.tokenTypeV1Compat == null)
			{
				XmlSchemaSimpleType simpleTypeFromTypeCode = DatatypeImplementation.GetSimpleTypeFromTypeCode(XmlTypeCode.Token);
				DatatypeImplementation.tokenTypeV1Compat = simpleTypeFromTypeCode.Clone() as XmlSchemaSimpleType;
				DatatypeImplementation.tokenTypeV1Compat.SetDatatype(DatatypeImplementation.c_tokenV1Compat);
				DatatypeImplementation.tokenTypeV1Compat.ElementDecl = new SchemaElementDecl(DatatypeImplementation.c_tokenV1Compat);
				DatatypeImplementation.tokenTypeV1Compat.ElementDecl.SchemaType = DatatypeImplementation.tokenTypeV1Compat;
			}
			return DatatypeImplementation.tokenTypeV1Compat;
		}

		// Token: 0x060015F1 RID: 5617 RVA: 0x0006202C File Offset: 0x0006102C
		internal static XmlSchemaSimpleType[] GetBuiltInTypes()
		{
			return DatatypeImplementation.enumToTypeCode;
		}

		// Token: 0x060015F2 RID: 5618 RVA: 0x00062034 File Offset: 0x00061034
		internal static XmlTypeCode GetPrimitiveTypeCode(XmlTypeCode typeCode)
		{
			XmlSchemaSimpleType xmlSchemaSimpleType = DatatypeImplementation.enumToTypeCode[(int)typeCode];
			while (xmlSchemaSimpleType.BaseXmlSchemaType != DatatypeImplementation.AnySimpleType)
			{
				xmlSchemaSimpleType = xmlSchemaSimpleType.BaseXmlSchemaType as XmlSchemaSimpleType;
			}
			return xmlSchemaSimpleType.TypeCode;
		}

		// Token: 0x060015F3 RID: 5619 RVA: 0x0006206C File Offset: 0x0006106C
		internal override XmlSchemaDatatype DeriveByRestriction(XmlSchemaObjectCollection facets, XmlNameTable nameTable, XmlSchemaType schemaType)
		{
			DatatypeImplementation datatypeImplementation = (DatatypeImplementation)base.MemberwiseClone();
			datatypeImplementation.restriction = this.FacetsChecker.ConstructRestriction(this, facets, nameTable);
			datatypeImplementation.baseType = this;
			datatypeImplementation.parentSchemaType = schemaType;
			datatypeImplementation.valueConverter = null;
			return datatypeImplementation;
		}

		// Token: 0x060015F4 RID: 5620 RVA: 0x000620AF File Offset: 0x000610AF
		internal override XmlSchemaDatatype DeriveByList(XmlSchemaType schemaType)
		{
			return this.DeriveByList(0, schemaType);
		}

		// Token: 0x060015F5 RID: 5621 RVA: 0x000620BC File Offset: 0x000610BC
		internal XmlSchemaDatatype DeriveByList(int minSize, XmlSchemaType schemaType)
		{
			if (this.variety == XmlSchemaDatatypeVariety.List)
			{
				throw new XmlSchemaException("Sch_ListFromNonatomic", string.Empty);
			}
			if (this.variety == XmlSchemaDatatypeVariety.Union && !((Datatype_union)this).HasAtomicMembers())
			{
				throw new XmlSchemaException("Sch_ListFromNonatomic", string.Empty);
			}
			return new Datatype_List(this, minSize)
			{
				variety = XmlSchemaDatatypeVariety.List,
				restriction = null,
				baseType = DatatypeImplementation.c_anySimpleType,
				parentSchemaType = schemaType
			};
		}

		// Token: 0x060015F6 RID: 5622 RVA: 0x00062134 File Offset: 0x00061134
		internal new static DatatypeImplementation DeriveByUnion(XmlSchemaSimpleType[] types, XmlSchemaType schemaType)
		{
			return new Datatype_union(types)
			{
				baseType = DatatypeImplementation.c_anySimpleType,
				variety = XmlSchemaDatatypeVariety.Union,
				parentSchemaType = schemaType
			};
		}

		// Token: 0x060015F7 RID: 5623 RVA: 0x00062162 File Offset: 0x00061162
		internal override void VerifySchemaValid(XmlSchemaObjectTable notations, XmlSchemaObject caller)
		{
		}

		// Token: 0x060015F8 RID: 5624 RVA: 0x00062164 File Offset: 0x00061164
		public override bool IsDerivedFrom(XmlSchemaDatatype datatype)
		{
			if (datatype == null)
			{
				return false;
			}
			for (DatatypeImplementation datatypeImplementation = this; datatypeImplementation != null; datatypeImplementation = datatypeImplementation.baseType)
			{
				if (datatypeImplementation == datatype)
				{
					return true;
				}
			}
			if (((DatatypeImplementation)datatype).baseType == null)
			{
				Type type = base.GetType();
				Type type2 = datatype.GetType();
				return type2 == type || type.IsSubclassOf(type2);
			}
			if (datatype.Variety == XmlSchemaDatatypeVariety.Union && !datatype.HasLexicalFacets && !datatype.HasValueFacets && this.variety != XmlSchemaDatatypeVariety.Union)
			{
				return ((Datatype_union)datatype).IsUnionBaseOf(this);
			}
			return (this.variety == XmlSchemaDatatypeVariety.Union || this.variety == XmlSchemaDatatypeVariety.List) && this.restriction == null && datatype == DatatypeImplementation.anySimpleType.Datatype;
		}

		// Token: 0x060015F9 RID: 5625 RVA: 0x0006220B File Offset: 0x0006120B
		internal override bool IsEqual(object o1, object o2)
		{
			return this.Compare(o1, o2) == 0;
		}

		// Token: 0x060015FA RID: 5626 RVA: 0x00062218 File Offset: 0x00061218
		internal override bool IsComparable(XmlSchemaDatatype dtype)
		{
			XmlTypeCode typeCode = this.TypeCode;
			XmlTypeCode typeCode2 = dtype.TypeCode;
			return typeCode == typeCode2 || DatatypeImplementation.GetPrimitiveTypeCode(typeCode) == DatatypeImplementation.GetPrimitiveTypeCode(typeCode2) || (this.IsDerivedFrom(dtype) || dtype.IsDerivedFrom(this));
		}

		// Token: 0x060015FB RID: 5627 RVA: 0x0006225E File Offset: 0x0006125E
		internal virtual XmlValueConverter CreateValueConverter(XmlSchemaType schemaType)
		{
			return null;
		}

		// Token: 0x17000543 RID: 1347
		// (get) Token: 0x060015FC RID: 5628 RVA: 0x00062261 File Offset: 0x00061261
		internal override FacetsChecker FacetsChecker
		{
			get
			{
				return DatatypeImplementation.miscFacetsChecker;
			}
		}

		// Token: 0x17000544 RID: 1348
		// (get) Token: 0x060015FD RID: 5629 RVA: 0x00062268 File Offset: 0x00061268
		internal override XmlValueConverter ValueConverter
		{
			get
			{
				if (this.valueConverter == null)
				{
					this.valueConverter = this.CreateValueConverter(this.parentSchemaType);
				}
				return this.valueConverter;
			}
		}

		// Token: 0x17000545 RID: 1349
		// (get) Token: 0x060015FE RID: 5630 RVA: 0x0006228A File Offset: 0x0006128A
		public override XmlTokenizedType TokenizedType
		{
			get
			{
				return XmlTokenizedType.None;
			}
		}

		// Token: 0x17000546 RID: 1350
		// (get) Token: 0x060015FF RID: 5631 RVA: 0x0006228E File Offset: 0x0006128E
		public override Type ValueType
		{
			get
			{
				return typeof(string);
			}
		}

		// Token: 0x17000547 RID: 1351
		// (get) Token: 0x06001600 RID: 5632 RVA: 0x0006229A File Offset: 0x0006129A
		public override XmlSchemaDatatypeVariety Variety
		{
			get
			{
				return this.variety;
			}
		}

		// Token: 0x17000548 RID: 1352
		// (get) Token: 0x06001601 RID: 5633 RVA: 0x000622A2 File Offset: 0x000612A2
		public override XmlTypeCode TypeCode
		{
			get
			{
				return XmlTypeCode.None;
			}
		}

		// Token: 0x17000549 RID: 1353
		// (get) Token: 0x06001602 RID: 5634 RVA: 0x000622A5 File Offset: 0x000612A5
		// (set) Token: 0x06001603 RID: 5635 RVA: 0x000622AD File Offset: 0x000612AD
		internal override RestrictionFacets Restriction
		{
			get
			{
				return this.restriction;
			}
			set
			{
				this.restriction = value;
			}
		}

		// Token: 0x1700054A RID: 1354
		// (get) Token: 0x06001604 RID: 5636 RVA: 0x000622B8 File Offset: 0x000612B8
		internal override bool HasLexicalFacets
		{
			get
			{
				RestrictionFlags restrictionFlags = ((this.restriction != null) ? this.restriction.Flags : ((RestrictionFlags)0));
				return restrictionFlags != (RestrictionFlags)0 && (restrictionFlags & (RestrictionFlags.Pattern | RestrictionFlags.WhiteSpace | RestrictionFlags.TotalDigits | RestrictionFlags.FractionDigits)) != (RestrictionFlags)0;
			}
		}

		// Token: 0x1700054B RID: 1355
		// (get) Token: 0x06001605 RID: 5637 RVA: 0x000622EC File Offset: 0x000612EC
		internal override bool HasValueFacets
		{
			get
			{
				RestrictionFlags restrictionFlags = ((this.restriction != null) ? this.restriction.Flags : ((RestrictionFlags)0));
				return restrictionFlags != (RestrictionFlags)0 && (restrictionFlags & (RestrictionFlags.Length | RestrictionFlags.MinLength | RestrictionFlags.MaxLength | RestrictionFlags.Enumeration | RestrictionFlags.MaxInclusive | RestrictionFlags.MaxExclusive | RestrictionFlags.MinInclusive | RestrictionFlags.MinExclusive | RestrictionFlags.TotalDigits | RestrictionFlags.FractionDigits)) != (RestrictionFlags)0;
			}
		}

		// Token: 0x1700054C RID: 1356
		// (get) Token: 0x06001606 RID: 5638 RVA: 0x0006231F File Offset: 0x0006131F
		protected DatatypeImplementation Base
		{
			get
			{
				return this.baseType;
			}
		}

		// Token: 0x1700054D RID: 1357
		// (get) Token: 0x06001607 RID: 5639
		internal abstract Type ListValueType { get; }

		// Token: 0x1700054E RID: 1358
		// (get) Token: 0x06001608 RID: 5640
		internal abstract RestrictionFlags ValidRestrictionFlags { get; }

		// Token: 0x1700054F RID: 1359
		// (get) Token: 0x06001609 RID: 5641 RVA: 0x00062327 File Offset: 0x00061327
		internal override XmlSchemaWhiteSpace BuiltInWhitespaceFacet
		{
			get
			{
				return XmlSchemaWhiteSpace.Preserve;
			}
		}

		// Token: 0x0600160A RID: 5642 RVA: 0x0006232A File Offset: 0x0006132A
		internal override object ParseValue(string s, Type typDest, XmlNameTable nameTable, IXmlNamespaceResolver nsmgr)
		{
			return this.ValueConverter.ChangeType(this.ParseValue(s, nameTable, nsmgr), typDest, nsmgr);
		}

		// Token: 0x0600160B RID: 5643 RVA: 0x00062344 File Offset: 0x00061344
		public override object ParseValue(string s, XmlNameTable nameTable, IXmlNamespaceResolver nsmgr)
		{
			object obj;
			Exception ex = this.TryParseValue(s, nameTable, nsmgr, out obj);
			if (ex != null)
			{
				throw new XmlSchemaException("Sch_InvalidValueDetailed", new string[]
				{
					s,
					this.GetTypeName(),
					ex.Message
				}, ex, null, 0, 0, null);
			}
			if (this.Variety == XmlSchemaDatatypeVariety.Union)
			{
				XsdSimpleValue xsdSimpleValue = obj as XsdSimpleValue;
				return xsdSimpleValue.TypedValue;
			}
			return obj;
		}

		// Token: 0x0600160C RID: 5644 RVA: 0x000623A8 File Offset: 0x000613A8
		internal override object ParseValue(string s, XmlNameTable nameTable, IXmlNamespaceResolver nsmgr, bool createAtomicValue)
		{
			if (!createAtomicValue)
			{
				return this.ParseValue(s, nameTable, nsmgr);
			}
			object obj;
			Exception ex = this.TryParseValue(s, nameTable, nsmgr, out obj);
			if (ex != null)
			{
				throw new XmlSchemaException("Sch_InvalidValueDetailed", new string[]
				{
					s,
					this.GetTypeName(),
					ex.Message
				}, ex, null, 0, 0, null);
			}
			return obj;
		}

		// Token: 0x0600160D RID: 5645 RVA: 0x00062404 File Offset: 0x00061404
		internal override Exception TryParseValue(object value, XmlNameTable nameTable, IXmlNamespaceResolver namespaceResolver, out object typedValue)
		{
			Exception ex = null;
			typedValue = null;
			if (value == null)
			{
				return new ArgumentNullException("value");
			}
			string text = value as string;
			if (text != null)
			{
				return this.TryParseValue(text, nameTable, namespaceResolver, out typedValue);
			}
			try
			{
				object obj = value;
				if (value.GetType() != this.ValueType)
				{
					obj = this.ValueConverter.ChangeType(value, this.ValueType, namespaceResolver);
				}
				if (this.HasLexicalFacets)
				{
					string text2 = (string)this.ValueConverter.ChangeType(value, typeof(string), namespaceResolver);
					ex = this.FacetsChecker.CheckLexicalFacets(ref text2, this);
					if (ex != null)
					{
						return ex;
					}
				}
				if (this.HasValueFacets)
				{
					ex = this.FacetsChecker.CheckValueFacets(obj, this);
					if (ex != null)
					{
						return ex;
					}
				}
				typedValue = obj;
				return null;
			}
			catch (FormatException ex2)
			{
				ex = ex2;
			}
			catch (InvalidCastException ex3)
			{
				ex = ex3;
			}
			catch (OverflowException ex4)
			{
				ex = ex4;
			}
			catch (ArgumentException ex5)
			{
				ex = ex5;
			}
			return ex;
		}

		// Token: 0x0600160E RID: 5646 RVA: 0x00062510 File Offset: 0x00061510
		internal string GetTypeName()
		{
			XmlSchemaType xmlSchemaType = this.parentSchemaType;
			string text;
			if (xmlSchemaType == null || xmlSchemaType.QualifiedName.IsEmpty)
			{
				text = base.TypeCodeString;
			}
			else
			{
				text = xmlSchemaType.QualifiedName.ToString();
			}
			return text;
		}

		// Token: 0x0600160F RID: 5647 RVA: 0x0006254C File Offset: 0x0006154C
		protected int Compare(byte[] value1, byte[] value2)
		{
			int num = value1.Length;
			if (num != value2.Length)
			{
				return -1;
			}
			for (int i = 0; i < num; i++)
			{
				if (value1[i] != value2[i])
				{
					return -1;
				}
			}
			return 0;
		}

		// Token: 0x04000D13 RID: 3347
		private const int anySimpleTypeIndex = 11;

		// Token: 0x04000D14 RID: 3348
		private XmlSchemaDatatypeVariety variety;

		// Token: 0x04000D15 RID: 3349
		private RestrictionFacets restriction;

		// Token: 0x04000D16 RID: 3350
		private DatatypeImplementation baseType;

		// Token: 0x04000D17 RID: 3351
		private XmlValueConverter valueConverter;

		// Token: 0x04000D18 RID: 3352
		private XmlSchemaType parentSchemaType;

		// Token: 0x04000D19 RID: 3353
		private static Hashtable builtinTypes = new Hashtable();

		// Token: 0x04000D1A RID: 3354
		private static XmlSchemaSimpleType[] enumToTypeCode = new XmlSchemaSimpleType[55];

		// Token: 0x04000D1B RID: 3355
		private static XmlSchemaSimpleType anySimpleType;

		// Token: 0x04000D1C RID: 3356
		private static XmlSchemaSimpleType anyAtomicType;

		// Token: 0x04000D1D RID: 3357
		private static XmlSchemaSimpleType untypedAtomicType;

		// Token: 0x04000D1E RID: 3358
		private static XmlSchemaSimpleType yearMonthDurationType;

		// Token: 0x04000D1F RID: 3359
		private static XmlSchemaSimpleType dayTimeDurationType;

		// Token: 0x04000D20 RID: 3360
		private static XmlSchemaSimpleType normalizedStringTypeV1Compat;

		// Token: 0x04000D21 RID: 3361
		private static XmlSchemaSimpleType tokenTypeV1Compat;

		// Token: 0x04000D22 RID: 3362
		internal static XmlQualifiedName QnAnySimpleType = new XmlQualifiedName("anySimpleType", "http://www.w3.org/2001/XMLSchema");

		// Token: 0x04000D23 RID: 3363
		internal static XmlQualifiedName QnAnyType = new XmlQualifiedName("anyType", "http://www.w3.org/2001/XMLSchema");

		// Token: 0x04000D24 RID: 3364
		internal static FacetsChecker stringFacetsChecker = new StringFacetsChecker();

		// Token: 0x04000D25 RID: 3365
		internal static FacetsChecker miscFacetsChecker = new MiscFacetsChecker();

		// Token: 0x04000D26 RID: 3366
		internal static FacetsChecker numeric2FacetsChecker = new Numeric2FacetsChecker();

		// Token: 0x04000D27 RID: 3367
		internal static FacetsChecker binaryFacetsChecker = new BinaryFacetsChecker();

		// Token: 0x04000D28 RID: 3368
		internal static FacetsChecker dateTimeFacetsChecker = new DateTimeFacetsChecker();

		// Token: 0x04000D29 RID: 3369
		internal static FacetsChecker durationFacetsChecker = new DurationFacetsChecker();

		// Token: 0x04000D2A RID: 3370
		internal static FacetsChecker listFacetsChecker = new ListFacetsChecker();

		// Token: 0x04000D2B RID: 3371
		internal static FacetsChecker qnameFacetsChecker = new QNameFacetsChecker();

		// Token: 0x04000D2C RID: 3372
		internal static FacetsChecker unionFacetsChecker = new UnionFacetsChecker();

		// Token: 0x04000D2D RID: 3373
		private static readonly DatatypeImplementation c_anySimpleType = new Datatype_anySimpleType();

		// Token: 0x04000D2E RID: 3374
		private static readonly DatatypeImplementation c_anyURI = new Datatype_anyURI();

		// Token: 0x04000D2F RID: 3375
		private static readonly DatatypeImplementation c_base64Binary = new Datatype_base64Binary();

		// Token: 0x04000D30 RID: 3376
		private static readonly DatatypeImplementation c_boolean = new Datatype_boolean();

		// Token: 0x04000D31 RID: 3377
		private static readonly DatatypeImplementation c_byte = new Datatype_byte();

		// Token: 0x04000D32 RID: 3378
		private static readonly DatatypeImplementation c_char = new Datatype_char();

		// Token: 0x04000D33 RID: 3379
		private static readonly DatatypeImplementation c_date = new Datatype_date();

		// Token: 0x04000D34 RID: 3380
		private static readonly DatatypeImplementation c_dateTime = new Datatype_dateTime();

		// Token: 0x04000D35 RID: 3381
		private static readonly DatatypeImplementation c_dateTimeNoTz = new Datatype_dateTimeNoTimeZone();

		// Token: 0x04000D36 RID: 3382
		private static readonly DatatypeImplementation c_dateTimeTz = new Datatype_dateTimeTimeZone();

		// Token: 0x04000D37 RID: 3383
		private static readonly DatatypeImplementation c_day = new Datatype_day();

		// Token: 0x04000D38 RID: 3384
		private static readonly DatatypeImplementation c_decimal = new Datatype_decimal();

		// Token: 0x04000D39 RID: 3385
		private static readonly DatatypeImplementation c_double = new Datatype_double();

		// Token: 0x04000D3A RID: 3386
		private static readonly DatatypeImplementation c_doubleXdr = new Datatype_doubleXdr();

		// Token: 0x04000D3B RID: 3387
		private static readonly DatatypeImplementation c_duration = new Datatype_duration();

		// Token: 0x04000D3C RID: 3388
		private static readonly DatatypeImplementation c_ENTITY = new Datatype_ENTITY();

		// Token: 0x04000D3D RID: 3389
		private static readonly DatatypeImplementation c_ENTITIES = (DatatypeImplementation)DatatypeImplementation.c_ENTITY.DeriveByList(1, null);

		// Token: 0x04000D3E RID: 3390
		private static readonly DatatypeImplementation c_ENUMERATION = new Datatype_ENUMERATION();

		// Token: 0x04000D3F RID: 3391
		private static readonly DatatypeImplementation c_fixed = new Datatype_fixed();

		// Token: 0x04000D40 RID: 3392
		private static readonly DatatypeImplementation c_float = new Datatype_float();

		// Token: 0x04000D41 RID: 3393
		private static readonly DatatypeImplementation c_floatXdr = new Datatype_floatXdr();

		// Token: 0x04000D42 RID: 3394
		private static readonly DatatypeImplementation c_hexBinary = new Datatype_hexBinary();

		// Token: 0x04000D43 RID: 3395
		private static readonly DatatypeImplementation c_ID = new Datatype_ID();

		// Token: 0x04000D44 RID: 3396
		private static readonly DatatypeImplementation c_IDREF = new Datatype_IDREF();

		// Token: 0x04000D45 RID: 3397
		private static readonly DatatypeImplementation c_IDREFS = (DatatypeImplementation)DatatypeImplementation.c_IDREF.DeriveByList(1, null);

		// Token: 0x04000D46 RID: 3398
		private static readonly DatatypeImplementation c_int = new Datatype_int();

		// Token: 0x04000D47 RID: 3399
		private static readonly DatatypeImplementation c_integer = new Datatype_integer();

		// Token: 0x04000D48 RID: 3400
		private static readonly DatatypeImplementation c_language = new Datatype_language();

		// Token: 0x04000D49 RID: 3401
		private static readonly DatatypeImplementation c_long = new Datatype_long();

		// Token: 0x04000D4A RID: 3402
		private static readonly DatatypeImplementation c_month = new Datatype_month();

		// Token: 0x04000D4B RID: 3403
		private static readonly DatatypeImplementation c_monthDay = new Datatype_monthDay();

		// Token: 0x04000D4C RID: 3404
		private static readonly DatatypeImplementation c_Name = new Datatype_Name();

		// Token: 0x04000D4D RID: 3405
		private static readonly DatatypeImplementation c_NCName = new Datatype_NCName();

		// Token: 0x04000D4E RID: 3406
		private static readonly DatatypeImplementation c_negativeInteger = new Datatype_negativeInteger();

		// Token: 0x04000D4F RID: 3407
		private static readonly DatatypeImplementation c_NMTOKEN = new Datatype_NMTOKEN();

		// Token: 0x04000D50 RID: 3408
		private static readonly DatatypeImplementation c_NMTOKENS = (DatatypeImplementation)DatatypeImplementation.c_NMTOKEN.DeriveByList(1, null);

		// Token: 0x04000D51 RID: 3409
		private static readonly DatatypeImplementation c_nonNegativeInteger = new Datatype_nonNegativeInteger();

		// Token: 0x04000D52 RID: 3410
		private static readonly DatatypeImplementation c_nonPositiveInteger = new Datatype_nonPositiveInteger();

		// Token: 0x04000D53 RID: 3411
		private static readonly DatatypeImplementation c_normalizedString = new Datatype_normalizedString();

		// Token: 0x04000D54 RID: 3412
		private static readonly DatatypeImplementation c_NOTATION = new Datatype_NOTATION();

		// Token: 0x04000D55 RID: 3413
		private static readonly DatatypeImplementation c_positiveInteger = new Datatype_positiveInteger();

		// Token: 0x04000D56 RID: 3414
		private static readonly DatatypeImplementation c_QName = new Datatype_QName();

		// Token: 0x04000D57 RID: 3415
		private static readonly DatatypeImplementation c_QNameXdr = new Datatype_QNameXdr();

		// Token: 0x04000D58 RID: 3416
		private static readonly DatatypeImplementation c_short = new Datatype_short();

		// Token: 0x04000D59 RID: 3417
		private static readonly DatatypeImplementation c_string = new Datatype_string();

		// Token: 0x04000D5A RID: 3418
		private static readonly DatatypeImplementation c_time = new Datatype_time();

		// Token: 0x04000D5B RID: 3419
		private static readonly DatatypeImplementation c_timeNoTz = new Datatype_timeNoTimeZone();

		// Token: 0x04000D5C RID: 3420
		private static readonly DatatypeImplementation c_timeTz = new Datatype_timeTimeZone();

		// Token: 0x04000D5D RID: 3421
		private static readonly DatatypeImplementation c_token = new Datatype_token();

		// Token: 0x04000D5E RID: 3422
		private static readonly DatatypeImplementation c_unsignedByte = new Datatype_unsignedByte();

		// Token: 0x04000D5F RID: 3423
		private static readonly DatatypeImplementation c_unsignedInt = new Datatype_unsignedInt();

		// Token: 0x04000D60 RID: 3424
		private static readonly DatatypeImplementation c_unsignedLong = new Datatype_unsignedLong();

		// Token: 0x04000D61 RID: 3425
		private static readonly DatatypeImplementation c_unsignedShort = new Datatype_unsignedShort();

		// Token: 0x04000D62 RID: 3426
		private static readonly DatatypeImplementation c_uuid = new Datatype_uuid();

		// Token: 0x04000D63 RID: 3427
		private static readonly DatatypeImplementation c_year = new Datatype_year();

		// Token: 0x04000D64 RID: 3428
		private static readonly DatatypeImplementation c_yearMonth = new Datatype_yearMonth();

		// Token: 0x04000D65 RID: 3429
		internal static readonly DatatypeImplementation c_normalizedStringV1Compat = new Datatype_normalizedStringV1Compat();

		// Token: 0x04000D66 RID: 3430
		internal static readonly DatatypeImplementation c_tokenV1Compat = new Datatype_tokenV1Compat();

		// Token: 0x04000D67 RID: 3431
		private static readonly DatatypeImplementation c_anyAtomicType = new Datatype_anyAtomicType();

		// Token: 0x04000D68 RID: 3432
		private static readonly DatatypeImplementation c_dayTimeDuration = new Datatype_dayTimeDuration();

		// Token: 0x04000D69 RID: 3433
		private static readonly DatatypeImplementation c_untypedAtomicType = new Datatype_untypedAtomicType();

		// Token: 0x04000D6A RID: 3434
		private static readonly DatatypeImplementation c_yearMonthDuration = new Datatype_yearMonthDuration();

		// Token: 0x04000D6B RID: 3435
		private static readonly DatatypeImplementation[] c_tokenizedTypes;

		// Token: 0x04000D6C RID: 3436
		private static readonly DatatypeImplementation[] c_tokenizedTypesXsd;

		// Token: 0x04000D6D RID: 3437
		private static readonly DatatypeImplementation.SchemaDatatypeMap[] c_XdrTypes;

		// Token: 0x04000D6E RID: 3438
		private static readonly DatatypeImplementation.SchemaDatatypeMap[] c_XsdTypes;

		// Token: 0x020001AD RID: 429
		private class SchemaDatatypeMap : IComparable
		{
			// Token: 0x06001611 RID: 5649 RVA: 0x00062584 File Offset: 0x00061584
			internal SchemaDatatypeMap(string name, DatatypeImplementation type)
			{
				this.name = name;
				this.type = type;
			}

			// Token: 0x06001612 RID: 5650 RVA: 0x0006259A File Offset: 0x0006159A
			internal SchemaDatatypeMap(string name, DatatypeImplementation type, int parentIndex)
			{
				this.name = name;
				this.type = type;
				this.parentIndex = parentIndex;
			}

			// Token: 0x06001613 RID: 5651 RVA: 0x000625B7 File Offset: 0x000615B7
			public static explicit operator DatatypeImplementation(DatatypeImplementation.SchemaDatatypeMap sdm)
			{
				return sdm.type;
			}

			// Token: 0x17000550 RID: 1360
			// (get) Token: 0x06001614 RID: 5652 RVA: 0x000625BF File Offset: 0x000615BF
			public string Name
			{
				get
				{
					return this.name;
				}
			}

			// Token: 0x17000551 RID: 1361
			// (get) Token: 0x06001615 RID: 5653 RVA: 0x000625C7 File Offset: 0x000615C7
			public int ParentIndex
			{
				get
				{
					return this.parentIndex;
				}
			}

			// Token: 0x06001616 RID: 5654 RVA: 0x000625CF File Offset: 0x000615CF
			public int CompareTo(object obj)
			{
				return string.Compare(this.name, (string)obj, StringComparison.Ordinal);
			}

			// Token: 0x04000D6F RID: 3439
			private string name;

			// Token: 0x04000D70 RID: 3440
			private DatatypeImplementation type;

			// Token: 0x04000D71 RID: 3441
			private int parentIndex;
		}
	}
}
