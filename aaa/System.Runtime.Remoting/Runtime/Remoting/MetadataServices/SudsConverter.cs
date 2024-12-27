using System;
using System.Globalization;
using System.Runtime.Remoting.Metadata.W3cXsd2001;

namespace System.Runtime.Remoting.MetadataServices
{
	// Token: 0x02000079 RID: 121
	internal static class SudsConverter
	{
		// Token: 0x06000392 RID: 914 RVA: 0x00010E2C File Offset: 0x0000FE2C
		internal static string GetXsdVersion(XsdVersion xsdVersion)
		{
			string text;
			if (xsdVersion == XsdVersion.V1999)
			{
				text = SudsConverter.Xsd1999;
			}
			else if (xsdVersion == XsdVersion.V2000)
			{
				text = SudsConverter.Xsd2000;
			}
			else
			{
				text = SudsConverter.Xsd2001;
			}
			return text;
		}

		// Token: 0x06000393 RID: 915 RVA: 0x00010E5C File Offset: 0x0000FE5C
		internal static string GetXsiVersion(XsdVersion xsdVersion)
		{
			string text;
			if (xsdVersion == XsdVersion.V1999)
			{
				text = SudsConverter.Xsi1999;
			}
			else if (xsdVersion == XsdVersion.V2000)
			{
				text = SudsConverter.Xsi2000;
			}
			else
			{
				text = SudsConverter.Xsi2001;
			}
			return text;
		}

		// Token: 0x06000394 RID: 916 RVA: 0x00010E8C File Offset: 0x0000FE8C
		internal static string MapClrTypeToXsdType(Type clrType)
		{
			string text = null;
			if (clrType == SudsConverter.typeofChar)
			{
				return null;
			}
			if (clrType.IsPrimitive)
			{
				if (clrType == SudsConverter.typeofByte)
				{
					text = "xsd:unsignedByte";
				}
				else if (clrType == SudsConverter.typeofSByte)
				{
					text = "xsd:byte";
				}
				else if (clrType == SudsConverter.typeofBoolean)
				{
					text = "xsd:boolean";
				}
				else if (clrType == SudsConverter.typeofChar)
				{
					text = "xsd:char";
				}
				else if (clrType == SudsConverter.typeofDouble)
				{
					text = "xsd:double";
				}
				else if (clrType == SudsConverter.typeofSingle)
				{
					text = "xsd:float";
				}
				else if (clrType == SudsConverter.typeofDecimal)
				{
					text = "xsd:decimal";
				}
				else if (clrType == SudsConverter.typeofDateTime)
				{
					text = "xsd:dateTime";
				}
				else if (clrType == SudsConverter.typeofInt16)
				{
					text = "xsd:short";
				}
				else if (clrType == SudsConverter.typeofInt32)
				{
					text = "xsd:int";
				}
				else if (clrType == SudsConverter.typeofInt64)
				{
					text = "xsd:long";
				}
				else if (clrType == SudsConverter.typeofUInt16)
				{
					text = "xsd:unsignedShort";
				}
				else if (clrType == SudsConverter.typeofUInt32)
				{
					text = "xsd:unsignedInt";
				}
				else if (clrType == SudsConverter.typeofUInt64)
				{
					text = "xsd:unsignedLong";
				}
				else if (clrType == SudsConverter.typeofTimeSpan)
				{
					text = "xsd:duration";
				}
			}
			else if (SudsConverter.typeofISoapXsd.IsAssignableFrom(clrType))
			{
				if (clrType == SudsConverter.typeofSoapTime)
				{
					text = SoapTime.XsdType;
				}
				else if (clrType == SudsConverter.typeofSoapDate)
				{
					text = SoapDate.XsdType;
				}
				else if (clrType == SudsConverter.typeofSoapYearMonth)
				{
					text = SoapYearMonth.XsdType;
				}
				else if (clrType == SudsConverter.typeofSoapYear)
				{
					text = SoapYear.XsdType;
				}
				else if (clrType == SudsConverter.typeofSoapMonthDay)
				{
					text = SoapMonthDay.XsdType;
				}
				else if (clrType == SudsConverter.typeofSoapDay)
				{
					text = SoapDay.XsdType;
				}
				else if (clrType == SudsConverter.typeofSoapMonth)
				{
					text = SoapMonth.XsdType;
				}
				else if (clrType == SudsConverter.typeofSoapHexBinary)
				{
					text = SoapHexBinary.XsdType;
				}
				else if (clrType == SudsConverter.typeofSoapBase64Binary)
				{
					text = SoapBase64Binary.XsdType;
				}
				else if (clrType == SudsConverter.typeofSoapInteger)
				{
					text = SoapInteger.XsdType;
				}
				else if (clrType == SudsConverter.typeofSoapPositiveInteger)
				{
					text = SoapPositiveInteger.XsdType;
				}
				else if (clrType == SudsConverter.typeofSoapNonPositiveInteger)
				{
					text = SoapNonPositiveInteger.XsdType;
				}
				else if (clrType == SudsConverter.typeofSoapNonNegativeInteger)
				{
					text = SoapNonNegativeInteger.XsdType;
				}
				else if (clrType == SudsConverter.typeofSoapNegativeInteger)
				{
					text = SoapNegativeInteger.XsdType;
				}
				else if (clrType == SudsConverter.typeofSoapAnyUri)
				{
					text = SoapAnyUri.XsdType;
				}
				else if (clrType == SudsConverter.typeofSoapQName)
				{
					text = SoapQName.XsdType;
				}
				else if (clrType == SudsConverter.typeofSoapNotation)
				{
					text = SoapNotation.XsdType;
				}
				else if (clrType == SudsConverter.typeofSoapNormalizedString)
				{
					text = SoapNormalizedString.XsdType;
				}
				else if (clrType == SudsConverter.typeofSoapToken)
				{
					text = SoapToken.XsdType;
				}
				else if (clrType == SudsConverter.typeofSoapLanguage)
				{
					text = SoapLanguage.XsdType;
				}
				else if (clrType == SudsConverter.typeofSoapName)
				{
					text = SoapName.XsdType;
				}
				else if (clrType == SudsConverter.typeofSoapIdrefs)
				{
					text = SoapIdrefs.XsdType;
				}
				else if (clrType == SudsConverter.typeofSoapEntities)
				{
					text = SoapEntities.XsdType;
				}
				else if (clrType == SudsConverter.typeofSoapNmtoken)
				{
					text = SoapNmtoken.XsdType;
				}
				else if (clrType == SudsConverter.typeofSoapNmtokens)
				{
					text = SoapNmtokens.XsdType;
				}
				else if (clrType == SudsConverter.typeofSoapNcName)
				{
					text = SoapNcName.XsdType;
				}
				else if (clrType == SudsConverter.typeofSoapId)
				{
					text = SoapId.XsdType;
				}
				else if (clrType == SudsConverter.typeofSoapIdref)
				{
					text = SoapIdref.XsdType;
				}
				else if (clrType == SudsConverter.typeofSoapEntity)
				{
					text = SoapEntity.XsdType;
				}
				text = "xsd:" + text;
			}
			else if (clrType == SudsConverter.typeofString)
			{
				text = "xsd:string";
			}
			else if (clrType == SudsConverter.typeofDecimal)
			{
				text = "xsd:decimal";
			}
			else if (clrType == SudsConverter.typeofObject)
			{
				text = "xsd:anyType";
			}
			else if (clrType == SudsConverter.typeofVoid)
			{
				text = "void";
			}
			else if (clrType == SudsConverter.typeofDateTime)
			{
				text = "xsd:dateTime";
			}
			else if (clrType == SudsConverter.typeofTimeSpan)
			{
				text = "xsd:duration";
			}
			return text;
		}

		// Token: 0x06000395 RID: 917 RVA: 0x00011258 File Offset: 0x00010258
		internal static string MapXsdToClrTypes(string xsdType)
		{
			string text = xsdType.ToLower(CultureInfo.InvariantCulture);
			string text2 = null;
			if (xsdType == null || xsdType.Length == 0)
			{
				return null;
			}
			switch (text[0])
			{
			case 'a':
				if (text == "anyuri")
				{
					text2 = "SoapAnyUri";
				}
				else if (text == "anytype" || text == "ur-type")
				{
					text2 = "Object";
				}
				break;
			case 'b':
				if (text == "boolean")
				{
					text2 = "Boolean";
				}
				else if (text == "byte")
				{
					text2 = "SByte";
				}
				else if (text == "base64binary")
				{
					text2 = "SoapBase64Binary";
				}
				break;
			case 'c':
				if (text == "char")
				{
					text2 = "Char";
				}
				break;
			case 'd':
				if (text == "double")
				{
					text2 = "Double";
				}
				else if (text == "datetime")
				{
					text2 = "DateTime";
				}
				else if (text == "decimal")
				{
					text2 = "Decimal";
				}
				else if (text == "duration")
				{
					text2 = "TimeSpan";
				}
				else if (text == "date")
				{
					text2 = "SoapDate";
				}
				break;
			case 'e':
				if (text == "entities")
				{
					text2 = "SoapEntities";
				}
				else if (text == "entity")
				{
					text2 = "SoapEntity";
				}
				break;
			case 'f':
				if (text == "float")
				{
					text2 = "Single";
				}
				break;
			case 'g':
				if (text == "gyearmonth")
				{
					text2 = "SoapYearMonth";
				}
				else if (text == "gyear")
				{
					text2 = "SoapYear";
				}
				else if (text == "gmonthday")
				{
					text2 = "SoapMonthDay";
				}
				else if (text == "gday")
				{
					text2 = "SoapDay";
				}
				else if (text == "gmonth")
				{
					text2 = "SoapMonth";
				}
				break;
			case 'h':
				if (text == "hexbinary")
				{
					text2 = "SoapHexBinary";
				}
				break;
			case 'i':
				if (text == "int")
				{
					text2 = "Int32";
				}
				else if (text == "integer")
				{
					text2 = "SoapInteger";
				}
				else if (text == "idrefs")
				{
					text2 = "SoapIdrefs";
				}
				else if (text == "id")
				{
					text2 = "SoapId";
				}
				else if (text == "idref")
				{
					text2 = "SoapIdref";
				}
				break;
			case 'l':
				if (text == "long")
				{
					text2 = "Int64";
				}
				else if (text == "language")
				{
					text2 = "SoapLanguage";
				}
				break;
			case 'n':
				if (text == "number")
				{
					text2 = "Decimal";
				}
				else if (text == "normalizedstring")
				{
					text2 = "SoapNormalizedString";
				}
				else if (text == "nonpositiveinteger")
				{
					text2 = "SoapNonPositiveInteger";
				}
				else if (text == "negativeinteger")
				{
					text2 = "SoapNegativeInteger";
				}
				else if (text == "nonnegativeinteger")
				{
					text2 = "SoapNonNegativeInteger";
				}
				else if (text == "notation")
				{
					text2 = "SoapNotation";
				}
				else if (text == "nmtoken")
				{
					text2 = "SoapNmtoken";
				}
				else if (text == "nmtokens")
				{
					text2 = "SoapNmtokens";
				}
				else if (text == "name")
				{
					text2 = "SoapName";
				}
				else if (text == "ncname")
				{
					text2 = "SoapNcName";
				}
				break;
			case 'p':
				if (text == "positiveinteger")
				{
					text2 = "SoapPositiveInteger";
				}
				break;
			case 'q':
				if (text == "qname")
				{
					text2 = "SoapQName";
				}
				break;
			case 's':
				if (text == "string")
				{
					text2 = "String";
				}
				else if (text == "short")
				{
					text2 = "Int16";
				}
				break;
			case 't':
				if (text == "time")
				{
					text2 = "SoapTime";
				}
				else if (text == "token")
				{
					text2 = "SoapToken";
				}
				break;
			case 'u':
				if (text == "unsignedlong")
				{
					text2 = "UInt64";
				}
				else if (text == "unsignedint")
				{
					text2 = "UInt32";
				}
				else if (text == "unsignedshort")
				{
					text2 = "UInt16";
				}
				else if (text == "unsignedbyte")
				{
					text2 = "Byte";
				}
				break;
			}
			return text2;
		}

		// Token: 0x0400029E RID: 670
		internal static string Xsd1999 = "http://www.w3.org/1999/XMLSchema";

		// Token: 0x0400029F RID: 671
		internal static string Xsi1999 = "http://www.w3.org/1999/XMLSchema-instance";

		// Token: 0x040002A0 RID: 672
		internal static string Xsd2000 = "http://www.w3.org/2000/10/XMLSchema";

		// Token: 0x040002A1 RID: 673
		internal static string Xsi2000 = "http://www.w3.org/2000/10/XMLSchema-instance";

		// Token: 0x040002A2 RID: 674
		internal static string Xsd2001 = "http://www.w3.org/2001/XMLSchema";

		// Token: 0x040002A3 RID: 675
		internal static string Xsi2001 = "http://www.w3.org/2001/XMLSchema-instance";

		// Token: 0x040002A4 RID: 676
		internal static Type typeofByte = typeof(byte);

		// Token: 0x040002A5 RID: 677
		internal static Type typeofSByte = typeof(sbyte);

		// Token: 0x040002A6 RID: 678
		internal static Type typeofBoolean = typeof(bool);

		// Token: 0x040002A7 RID: 679
		internal static Type typeofChar = typeof(char);

		// Token: 0x040002A8 RID: 680
		internal static Type typeofDouble = typeof(double);

		// Token: 0x040002A9 RID: 681
		internal static Type typeofSingle = typeof(float);

		// Token: 0x040002AA RID: 682
		internal static Type typeofDecimal = typeof(decimal);

		// Token: 0x040002AB RID: 683
		internal static Type typeofInt16 = typeof(short);

		// Token: 0x040002AC RID: 684
		internal static Type typeofInt32 = typeof(int);

		// Token: 0x040002AD RID: 685
		internal static Type typeofInt64 = typeof(long);

		// Token: 0x040002AE RID: 686
		internal static Type typeofUInt16 = typeof(ushort);

		// Token: 0x040002AF RID: 687
		internal static Type typeofUInt32 = typeof(uint);

		// Token: 0x040002B0 RID: 688
		internal static Type typeofUInt64 = typeof(ulong);

		// Token: 0x040002B1 RID: 689
		internal static Type typeofSoapTime = typeof(SoapTime);

		// Token: 0x040002B2 RID: 690
		internal static Type typeofSoapDate = typeof(SoapDate);

		// Token: 0x040002B3 RID: 691
		internal static Type typeofSoapYearMonth = typeof(SoapYearMonth);

		// Token: 0x040002B4 RID: 692
		internal static Type typeofSoapYear = typeof(SoapYear);

		// Token: 0x040002B5 RID: 693
		internal static Type typeofSoapMonthDay = typeof(SoapMonthDay);

		// Token: 0x040002B6 RID: 694
		internal static Type typeofSoapDay = typeof(SoapDay);

		// Token: 0x040002B7 RID: 695
		internal static Type typeofSoapMonth = typeof(SoapMonth);

		// Token: 0x040002B8 RID: 696
		internal static Type typeofSoapHexBinary = typeof(SoapHexBinary);

		// Token: 0x040002B9 RID: 697
		internal static Type typeofSoapBase64Binary = typeof(SoapBase64Binary);

		// Token: 0x040002BA RID: 698
		internal static Type typeofSoapInteger = typeof(SoapInteger);

		// Token: 0x040002BB RID: 699
		internal static Type typeofSoapPositiveInteger = typeof(SoapPositiveInteger);

		// Token: 0x040002BC RID: 700
		internal static Type typeofSoapNonPositiveInteger = typeof(SoapNonPositiveInteger);

		// Token: 0x040002BD RID: 701
		internal static Type typeofSoapNonNegativeInteger = typeof(SoapNonNegativeInteger);

		// Token: 0x040002BE RID: 702
		internal static Type typeofSoapNegativeInteger = typeof(SoapNegativeInteger);

		// Token: 0x040002BF RID: 703
		internal static Type typeofSoapAnyUri = typeof(SoapAnyUri);

		// Token: 0x040002C0 RID: 704
		internal static Type typeofSoapQName = typeof(SoapQName);

		// Token: 0x040002C1 RID: 705
		internal static Type typeofSoapNotation = typeof(SoapNotation);

		// Token: 0x040002C2 RID: 706
		internal static Type typeofSoapNormalizedString = typeof(SoapNormalizedString);

		// Token: 0x040002C3 RID: 707
		internal static Type typeofSoapToken = typeof(SoapToken);

		// Token: 0x040002C4 RID: 708
		internal static Type typeofSoapLanguage = typeof(SoapLanguage);

		// Token: 0x040002C5 RID: 709
		internal static Type typeofSoapName = typeof(SoapName);

		// Token: 0x040002C6 RID: 710
		internal static Type typeofSoapIdrefs = typeof(SoapIdrefs);

		// Token: 0x040002C7 RID: 711
		internal static Type typeofSoapEntities = typeof(SoapEntities);

		// Token: 0x040002C8 RID: 712
		internal static Type typeofSoapNmtoken = typeof(SoapNmtoken);

		// Token: 0x040002C9 RID: 713
		internal static Type typeofSoapNmtokens = typeof(SoapNmtokens);

		// Token: 0x040002CA RID: 714
		internal static Type typeofSoapNcName = typeof(SoapNcName);

		// Token: 0x040002CB RID: 715
		internal static Type typeofSoapId = typeof(SoapId);

		// Token: 0x040002CC RID: 716
		internal static Type typeofSoapIdref = typeof(SoapIdref);

		// Token: 0x040002CD RID: 717
		internal static Type typeofSoapEntity = typeof(SoapEntity);

		// Token: 0x040002CE RID: 718
		internal static Type typeofString = typeof(string);

		// Token: 0x040002CF RID: 719
		internal static Type typeofObject = typeof(object);

		// Token: 0x040002D0 RID: 720
		internal static Type typeofVoid = typeof(void);

		// Token: 0x040002D1 RID: 721
		internal static Type typeofDateTime = typeof(DateTime);

		// Token: 0x040002D2 RID: 722
		internal static Type typeofTimeSpan = typeof(TimeSpan);

		// Token: 0x040002D3 RID: 723
		internal static Type typeofISoapXsd = typeof(ISoapXsd);
	}
}
