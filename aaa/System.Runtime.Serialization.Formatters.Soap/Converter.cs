using System;
using System.Globalization;
using System.Reflection;
using System.Runtime.Remoting.Activation;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Text;

namespace System.Runtime.Serialization.Formatters.Soap
{
	// Token: 0x02000025 RID: 37
	internal sealed class Converter
	{
		// Token: 0x06000083 RID: 131 RVA: 0x00005FDC File Offset: 0x00004FDC
		private Converter()
		{
		}

		// Token: 0x06000084 RID: 132 RVA: 0x00005FE4 File Offset: 0x00004FE4
		internal static InternalPrimitiveTypeE SoapToCode(Type type)
		{
			return Converter.ToCode(type);
		}

		// Token: 0x06000085 RID: 133 RVA: 0x00005FEC File Offset: 0x00004FEC
		internal static InternalPrimitiveTypeE ToCode(Type type)
		{
			InternalPrimitiveTypeE internalPrimitiveTypeE = InternalPrimitiveTypeE.Invalid;
			if (type.IsEnum)
			{
				return InternalPrimitiveTypeE.Invalid;
			}
			TypeCode typeCode = Type.GetTypeCode(type);
			if (typeCode == TypeCode.Object)
			{
				if (Converter.typeofISoapXsd.IsAssignableFrom(type))
				{
					if (type == Converter.typeofSoapTime)
					{
						internalPrimitiveTypeE = InternalPrimitiveTypeE.Time;
					}
					else if (type == Converter.typeofSoapDate)
					{
						internalPrimitiveTypeE = InternalPrimitiveTypeE.Date;
					}
					else if (type == Converter.typeofSoapYearMonth)
					{
						internalPrimitiveTypeE = InternalPrimitiveTypeE.YearMonth;
					}
					else if (type == Converter.typeofSoapYear)
					{
						internalPrimitiveTypeE = InternalPrimitiveTypeE.Year;
					}
					else if (type == Converter.typeofSoapMonthDay)
					{
						internalPrimitiveTypeE = InternalPrimitiveTypeE.MonthDay;
					}
					else if (type == Converter.typeofSoapDay)
					{
						internalPrimitiveTypeE = InternalPrimitiveTypeE.Day;
					}
					else if (type == Converter.typeofSoapMonth)
					{
						internalPrimitiveTypeE = InternalPrimitiveTypeE.Month;
					}
					else if (type == Converter.typeofSoapHexBinary)
					{
						internalPrimitiveTypeE = InternalPrimitiveTypeE.HexBinary;
					}
					else if (type == Converter.typeofSoapBase64Binary)
					{
						internalPrimitiveTypeE = InternalPrimitiveTypeE.Base64Binary;
					}
					else if (type == Converter.typeofSoapInteger)
					{
						internalPrimitiveTypeE = InternalPrimitiveTypeE.Integer;
					}
					else if (type == Converter.typeofSoapPositiveInteger)
					{
						internalPrimitiveTypeE = InternalPrimitiveTypeE.PositiveInteger;
					}
					else if (type == Converter.typeofSoapNonPositiveInteger)
					{
						internalPrimitiveTypeE = InternalPrimitiveTypeE.NonPositiveInteger;
					}
					else if (type == Converter.typeofSoapNonNegativeInteger)
					{
						internalPrimitiveTypeE = InternalPrimitiveTypeE.NonNegativeInteger;
					}
					else if (type == Converter.typeofSoapNegativeInteger)
					{
						internalPrimitiveTypeE = InternalPrimitiveTypeE.NegativeInteger;
					}
					else if (type == Converter.typeofSoapAnyUri)
					{
						internalPrimitiveTypeE = InternalPrimitiveTypeE.AnyUri;
					}
					else if (type == Converter.typeofSoapQName)
					{
						internalPrimitiveTypeE = InternalPrimitiveTypeE.QName;
					}
					else if (type == Converter.typeofSoapNotation)
					{
						internalPrimitiveTypeE = InternalPrimitiveTypeE.Notation;
					}
					else if (type == Converter.typeofSoapNormalizedString)
					{
						internalPrimitiveTypeE = InternalPrimitiveTypeE.NormalizedString;
					}
					else if (type == Converter.typeofSoapToken)
					{
						internalPrimitiveTypeE = InternalPrimitiveTypeE.Token;
					}
					else if (type == Converter.typeofSoapLanguage)
					{
						internalPrimitiveTypeE = InternalPrimitiveTypeE.Language;
					}
					else if (type == Converter.typeofSoapName)
					{
						internalPrimitiveTypeE = InternalPrimitiveTypeE.Name;
					}
					else if (type == Converter.typeofSoapIdrefs)
					{
						internalPrimitiveTypeE = InternalPrimitiveTypeE.Idrefs;
					}
					else if (type == Converter.typeofSoapEntities)
					{
						internalPrimitiveTypeE = InternalPrimitiveTypeE.Entities;
					}
					else if (type == Converter.typeofSoapNmtoken)
					{
						internalPrimitiveTypeE = InternalPrimitiveTypeE.Nmtoken;
					}
					else if (type == Converter.typeofSoapNmtokens)
					{
						internalPrimitiveTypeE = InternalPrimitiveTypeE.Nmtokens;
					}
					else if (type == Converter.typeofSoapNcName)
					{
						internalPrimitiveTypeE = InternalPrimitiveTypeE.NcName;
					}
					else if (type == Converter.typeofSoapId)
					{
						internalPrimitiveTypeE = InternalPrimitiveTypeE.Id;
					}
					else if (type == Converter.typeofSoapIdref)
					{
						internalPrimitiveTypeE = InternalPrimitiveTypeE.Idref;
					}
					else if (type == Converter.typeofSoapEntity)
					{
						internalPrimitiveTypeE = InternalPrimitiveTypeE.Entity;
					}
				}
				else if (type == Converter.typeofTimeSpan)
				{
					internalPrimitiveTypeE = InternalPrimitiveTypeE.TimeSpan;
				}
				else
				{
					internalPrimitiveTypeE = InternalPrimitiveTypeE.Invalid;
				}
			}
			else
			{
				internalPrimitiveTypeE = Converter.ToPrimitiveTypeEnum(typeCode);
			}
			return internalPrimitiveTypeE;
		}

		// Token: 0x06000086 RID: 134 RVA: 0x000061F8 File Offset: 0x000051F8
		internal static InternalPrimitiveTypeE ToCode(string value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("serParser", string.Format(CultureInfo.CurrentCulture, SoapUtil.GetResourceString("ArgumentNull_WithParamName"), new object[] { value }));
			}
			string text = value.ToLower(CultureInfo.InvariantCulture);
			char c = text[0];
			InternalPrimitiveTypeE internalPrimitiveTypeE = InternalPrimitiveTypeE.Invalid;
			switch (c)
			{
			case 'a':
				if (text == "anyuri")
				{
					return InternalPrimitiveTypeE.AnyUri;
				}
				return internalPrimitiveTypeE;
			case 'b':
				if (text == "boolean")
				{
					return InternalPrimitiveTypeE.Boolean;
				}
				if (text == "byte")
				{
					return InternalPrimitiveTypeE.SByte;
				}
				if (text == "base64binary")
				{
					return InternalPrimitiveTypeE.Base64Binary;
				}
				if (text == "base64")
				{
					return InternalPrimitiveTypeE.Base64Binary;
				}
				return internalPrimitiveTypeE;
			case 'c':
				if (text == "char" || text == "character")
				{
					return InternalPrimitiveTypeE.Char;
				}
				return internalPrimitiveTypeE;
			case 'd':
				if (text == "double")
				{
					internalPrimitiveTypeE = InternalPrimitiveTypeE.Double;
				}
				if (text == "datetime")
				{
					return InternalPrimitiveTypeE.DateTime;
				}
				if (text == "duration")
				{
					return InternalPrimitiveTypeE.TimeSpan;
				}
				if (text == "date")
				{
					return InternalPrimitiveTypeE.Date;
				}
				if (text == "decimal")
				{
					return InternalPrimitiveTypeE.Decimal;
				}
				return internalPrimitiveTypeE;
			case 'e':
				if (text == "entities")
				{
					return InternalPrimitiveTypeE.Entities;
				}
				if (text == "entity")
				{
					return InternalPrimitiveTypeE.Entity;
				}
				return internalPrimitiveTypeE;
			case 'f':
				if (text == "float")
				{
					return InternalPrimitiveTypeE.Single;
				}
				return internalPrimitiveTypeE;
			case 'g':
				if (text == "gyearmonth")
				{
					return InternalPrimitiveTypeE.YearMonth;
				}
				if (text == "gyear")
				{
					return InternalPrimitiveTypeE.Year;
				}
				if (text == "gmonthday")
				{
					return InternalPrimitiveTypeE.MonthDay;
				}
				if (text == "gday")
				{
					return InternalPrimitiveTypeE.Day;
				}
				if (text == "gmonth")
				{
					return InternalPrimitiveTypeE.Month;
				}
				return internalPrimitiveTypeE;
			case 'h':
				if (text == "hexbinary")
				{
					return InternalPrimitiveTypeE.HexBinary;
				}
				return internalPrimitiveTypeE;
			case 'i':
				if (text == "int")
				{
					internalPrimitiveTypeE = InternalPrimitiveTypeE.Int32;
				}
				if (text == "integer")
				{
					return InternalPrimitiveTypeE.Integer;
				}
				if (text == "idrefs")
				{
					return InternalPrimitiveTypeE.Idrefs;
				}
				if (text == "id")
				{
					return InternalPrimitiveTypeE.Id;
				}
				if (text == "idref")
				{
					return InternalPrimitiveTypeE.Idref;
				}
				return internalPrimitiveTypeE;
			case 'l':
				if (text == "long")
				{
					return InternalPrimitiveTypeE.Int64;
				}
				if (text == "language")
				{
					return InternalPrimitiveTypeE.Language;
				}
				return internalPrimitiveTypeE;
			case 'n':
				if (text == "number")
				{
					return InternalPrimitiveTypeE.Decimal;
				}
				if (text == "normalizedstring")
				{
					return InternalPrimitiveTypeE.NormalizedString;
				}
				if (text == "nonpositiveinteger")
				{
					return InternalPrimitiveTypeE.NonPositiveInteger;
				}
				if (text == "negativeinteger")
				{
					return InternalPrimitiveTypeE.NegativeInteger;
				}
				if (text == "nonnegativeinteger")
				{
					return InternalPrimitiveTypeE.NonNegativeInteger;
				}
				if (text == "notation")
				{
					return InternalPrimitiveTypeE.Notation;
				}
				if (text == "nmtoken")
				{
					return InternalPrimitiveTypeE.Nmtoken;
				}
				if (text == "nmtokens")
				{
					return InternalPrimitiveTypeE.Nmtokens;
				}
				if (text == "name")
				{
					return InternalPrimitiveTypeE.Name;
				}
				if (text == "ncname")
				{
					return InternalPrimitiveTypeE.NcName;
				}
				return internalPrimitiveTypeE;
			case 'p':
				if (text == "positiveinteger")
				{
					return InternalPrimitiveTypeE.PositiveInteger;
				}
				return internalPrimitiveTypeE;
			case 'q':
				if (text == "qname")
				{
					return InternalPrimitiveTypeE.QName;
				}
				return internalPrimitiveTypeE;
			case 's':
				if (text == "short")
				{
					return InternalPrimitiveTypeE.Int16;
				}
				if (text == "system.byte")
				{
					return InternalPrimitiveTypeE.Byte;
				}
				if (text == "system.sbyte")
				{
					return InternalPrimitiveTypeE.SByte;
				}
				if (text == "system")
				{
					return Converter.ToCode(value.Substring(7));
				}
				if (text == "system.runtime.remoting.metadata")
				{
					return Converter.ToCode(value.Substring(33));
				}
				return internalPrimitiveTypeE;
			case 't':
				if (text == "time")
				{
					return InternalPrimitiveTypeE.Time;
				}
				if (text == "token")
				{
					return InternalPrimitiveTypeE.Token;
				}
				if (text == "timeinstant")
				{
					return InternalPrimitiveTypeE.DateTime;
				}
				if (text == "timeduration")
				{
					return InternalPrimitiveTypeE.TimeSpan;
				}
				return internalPrimitiveTypeE;
			case 'u':
				if (text == "unsignedlong")
				{
					return InternalPrimitiveTypeE.UInt64;
				}
				if (text == "unsignedint")
				{
					return InternalPrimitiveTypeE.UInt32;
				}
				if (text == "unsignedshort")
				{
					return InternalPrimitiveTypeE.UInt16;
				}
				if (text == "unsignedbyte")
				{
					return InternalPrimitiveTypeE.Byte;
				}
				return internalPrimitiveTypeE;
			}
			internalPrimitiveTypeE = InternalPrimitiveTypeE.Invalid;
			return internalPrimitiveTypeE;
		}

		// Token: 0x06000087 RID: 135 RVA: 0x00006718 File Offset: 0x00005718
		internal static bool IsWriteAsByteArray(InternalPrimitiveTypeE code)
		{
			bool flag = false;
			switch (code)
			{
			case InternalPrimitiveTypeE.Boolean:
			case InternalPrimitiveTypeE.Byte:
			case InternalPrimitiveTypeE.Char:
			case InternalPrimitiveTypeE.Double:
			case InternalPrimitiveTypeE.Int16:
			case InternalPrimitiveTypeE.Int32:
			case InternalPrimitiveTypeE.Int64:
			case InternalPrimitiveTypeE.SByte:
			case InternalPrimitiveTypeE.Single:
			case InternalPrimitiveTypeE.UInt16:
			case InternalPrimitiveTypeE.UInt32:
			case InternalPrimitiveTypeE.UInt64:
				flag = true;
				break;
			}
			return flag;
		}

		// Token: 0x06000088 RID: 136 RVA: 0x00006778 File Offset: 0x00005778
		internal static InternalNameSpaceE GetNameSpaceEnum(InternalPrimitiveTypeE code, Type type, WriteObjectInfo objectInfo, out string typeName)
		{
			InternalNameSpaceE internalNameSpaceE = InternalNameSpaceE.None;
			typeName = null;
			if (code != InternalPrimitiveTypeE.Invalid)
			{
				if (code == InternalPrimitiveTypeE.Char)
				{
					internalNameSpaceE = InternalNameSpaceE.UrtSystem;
					typeName = "System.Char";
				}
				else
				{
					internalNameSpaceE = InternalNameSpaceE.XdrPrimitive;
					typeName = Converter.ToXmlDataType(code);
				}
			}
			if (internalNameSpaceE == InternalNameSpaceE.None && type != null)
			{
				if (type == Converter.typeofString)
				{
					internalNameSpaceE = InternalNameSpaceE.XdrString;
				}
				else if (objectInfo == null)
				{
					typeName = type.FullName;
					if (type.Module.Assembly == Converter.urtAssembly)
					{
						internalNameSpaceE = InternalNameSpaceE.UrtSystem;
					}
					else
					{
						internalNameSpaceE = InternalNameSpaceE.UrtUser;
					}
				}
				else
				{
					typeName = objectInfo.GetTypeFullName();
					if (objectInfo.GetAssemblyString().Equals(Converter.urtAssemblyString))
					{
						internalNameSpaceE = InternalNameSpaceE.UrtSystem;
					}
					else
					{
						internalNameSpaceE = InternalNameSpaceE.UrtUser;
					}
				}
			}
			if (objectInfo != null)
			{
				if (!objectInfo.isSi && (objectInfo.IsAttributeNameSpace() || objectInfo.IsCustomXmlAttribute() || objectInfo.IsCustomXmlElement()))
				{
					internalNameSpaceE = InternalNameSpaceE.Interop;
				}
				else if (objectInfo.IsCallElement())
				{
					internalNameSpaceE = InternalNameSpaceE.CallElement;
				}
			}
			return internalNameSpaceE;
		}

		// Token: 0x06000089 RID: 137 RVA: 0x00006830 File Offset: 0x00005830
		internal static bool IsSiTransmitType(InternalPrimitiveTypeE code)
		{
			switch (code)
			{
			case InternalPrimitiveTypeE.Invalid:
			case InternalPrimitiveTypeE.TimeSpan:
			case InternalPrimitiveTypeE.DateTime:
			case InternalPrimitiveTypeE.Time:
			case InternalPrimitiveTypeE.Date:
			case InternalPrimitiveTypeE.YearMonth:
			case InternalPrimitiveTypeE.Year:
			case InternalPrimitiveTypeE.MonthDay:
			case InternalPrimitiveTypeE.Day:
			case InternalPrimitiveTypeE.Month:
			case InternalPrimitiveTypeE.HexBinary:
			case InternalPrimitiveTypeE.Base64Binary:
			case InternalPrimitiveTypeE.Integer:
			case InternalPrimitiveTypeE.PositiveInteger:
			case InternalPrimitiveTypeE.NonPositiveInteger:
			case InternalPrimitiveTypeE.NonNegativeInteger:
			case InternalPrimitiveTypeE.NegativeInteger:
			case InternalPrimitiveTypeE.AnyUri:
			case InternalPrimitiveTypeE.QName:
			case InternalPrimitiveTypeE.Notation:
			case InternalPrimitiveTypeE.NormalizedString:
			case InternalPrimitiveTypeE.Token:
			case InternalPrimitiveTypeE.Language:
			case InternalPrimitiveTypeE.Name:
			case InternalPrimitiveTypeE.Idrefs:
			case InternalPrimitiveTypeE.Entities:
			case InternalPrimitiveTypeE.Nmtoken:
			case InternalPrimitiveTypeE.Nmtokens:
			case InternalPrimitiveTypeE.NcName:
			case InternalPrimitiveTypeE.Id:
			case InternalPrimitiveTypeE.Idref:
			case InternalPrimitiveTypeE.Entity:
				return true;
			}
			return false;
		}

		// Token: 0x0600008A RID: 138 RVA: 0x00006904 File Offset: 0x00005904
		private static void InitTypeA()
		{
			Converter.typeA = new Type[Converter.primitiveTypeEnumLength];
			Converter.typeA[0] = null;
			Converter.typeA[1] = Converter.typeofBoolean;
			Converter.typeA[2] = Converter.typeofByte;
			Converter.typeA[3] = Converter.typeofChar;
			Converter.typeA[5] = Converter.typeofDecimal;
			Converter.typeA[6] = Converter.typeofDouble;
			Converter.typeA[7] = Converter.typeofInt16;
			Converter.typeA[8] = Converter.typeofInt32;
			Converter.typeA[9] = Converter.typeofInt64;
			Converter.typeA[10] = Converter.typeofSByte;
			Converter.typeA[11] = Converter.typeofSingle;
			Converter.typeA[12] = Converter.typeofTimeSpan;
			Converter.typeA[13] = Converter.typeofDateTime;
			Converter.typeA[14] = Converter.typeofUInt16;
			Converter.typeA[15] = Converter.typeofUInt32;
			Converter.typeA[16] = Converter.typeofUInt64;
			Converter.typeA[17] = Converter.typeofSoapTime;
			Converter.typeA[18] = Converter.typeofSoapDate;
			Converter.typeA[19] = Converter.typeofSoapYearMonth;
			Converter.typeA[20] = Converter.typeofSoapYear;
			Converter.typeA[21] = Converter.typeofSoapMonthDay;
			Converter.typeA[22] = Converter.typeofSoapDay;
			Converter.typeA[23] = Converter.typeofSoapMonth;
			Converter.typeA[24] = Converter.typeofSoapHexBinary;
			Converter.typeA[25] = Converter.typeofSoapBase64Binary;
			Converter.typeA[26] = Converter.typeofSoapInteger;
			Converter.typeA[27] = Converter.typeofSoapPositiveInteger;
			Converter.typeA[28] = Converter.typeofSoapNonPositiveInteger;
			Converter.typeA[29] = Converter.typeofSoapNonNegativeInteger;
			Converter.typeA[30] = Converter.typeofSoapNegativeInteger;
			Converter.typeA[31] = Converter.typeofSoapAnyUri;
			Converter.typeA[32] = Converter.typeofSoapQName;
			Converter.typeA[33] = Converter.typeofSoapNotation;
			Converter.typeA[34] = Converter.typeofSoapNormalizedString;
			Converter.typeA[35] = Converter.typeofSoapToken;
			Converter.typeA[36] = Converter.typeofSoapLanguage;
			Converter.typeA[37] = Converter.typeofSoapName;
			Converter.typeA[38] = Converter.typeofSoapIdrefs;
			Converter.typeA[39] = Converter.typeofSoapEntities;
			Converter.typeA[40] = Converter.typeofSoapNmtoken;
			Converter.typeA[41] = Converter.typeofSoapNmtokens;
			Converter.typeA[42] = Converter.typeofSoapNcName;
			Converter.typeA[43] = Converter.typeofSoapId;
			Converter.typeA[44] = Converter.typeofSoapIdref;
			Converter.typeA[45] = Converter.typeofSoapEntity;
		}

		// Token: 0x0600008B RID: 139 RVA: 0x00006B5D File Offset: 0x00005B5D
		internal static Type SoapToType(InternalPrimitiveTypeE code)
		{
			return Converter.ToType(code);
		}

		// Token: 0x0600008C RID: 140 RVA: 0x00006B68 File Offset: 0x00005B68
		internal static Type ToType(InternalPrimitiveTypeE code)
		{
			lock (Converter.typeofConverter)
			{
				if (Converter.typeA == null)
				{
					Converter.InitTypeA();
				}
			}
			return Converter.typeA[(int)code];
		}

		// Token: 0x0600008D RID: 141 RVA: 0x00006BB0 File Offset: 0x00005BB0
		private static void InitValueA()
		{
			Converter.valueA = new string[Converter.primitiveTypeEnumLength];
			Converter.valueA[0] = null;
			Converter.valueA[1] = "System.Boolean";
			Converter.valueA[2] = "System.Byte";
			Converter.valueA[3] = "System.Char";
			Converter.valueA[5] = "System.Decimal";
			Converter.valueA[6] = "System.Double";
			Converter.valueA[7] = "System.Int16";
			Converter.valueA[8] = "System.Int32";
			Converter.valueA[9] = "System.Int64";
			Converter.valueA[10] = "System.SByte";
			Converter.valueA[11] = "System.Single";
			Converter.valueA[12] = "System.TimeSpan";
			Converter.valueA[13] = "System.DateTime";
			Converter.valueA[14] = "System.UInt16";
			Converter.valueA[15] = "System.UInt32";
			Converter.valueA[16] = "System.UInt64";
			Converter.valueA[17] = "System.Runtime.Remoting.Metadata.W3cXsd2001.SoapTime";
			Converter.valueA[18] = "System.Runtime.Remoting.Metadata.W3cXsd2001.SoapDate";
			Converter.valueA[19] = "System.Runtime.Remoting.Metadata.W3cXsd2001.SoapYearMonth";
			Converter.valueA[20] = "System.Runtime.Remoting.Metadata.W3cXsd2001.SoapYear";
			Converter.valueA[21] = "System.Runtime.Remoting.Metadata.W3cXsd2001.SoapMonthDay";
			Converter.valueA[22] = "System.Runtime.Remoting.Metadata.W3cXsd2001.SoapDay";
			Converter.valueA[23] = "System.Runtime.Remoting.Metadata.W3cXsd2001.SoapMonth";
			Converter.valueA[24] = "System.Runtime.Remoting.Metadata.W3cXsd2001.SoapHexBinary";
			Converter.valueA[25] = "System.Runtime.Remoting.Metadata.W3cXsd2001.SoapBase64Binary";
			Converter.valueA[26] = "System.Runtime.Remoting.Metadata.W3cXsd2001.SoapInteger";
			Converter.valueA[27] = "System.Runtime.Remoting.Metadata.W3cXsd2001.SoapPositiveInteger";
			Converter.valueA[28] = "System.Runtime.Remoting.Metadata.W3cXsd2001.SoapNonPositiveInteger";
			Converter.valueA[29] = "System.Runtime.Remoting.Metadata.W3cXsd2001.SoapNonNegativeInteger";
			Converter.valueA[30] = "System.Runtime.Remoting.Metadata.W3cXsd2001.SoapNegativeInteger";
			Converter.valueA[31] = "System.Runtime.Remoting.Metadata.W3cXsd2001.SoapAnyUri";
			Converter.valueA[32] = "System.Runtime.Remoting.Metadata.W3cXsd2001.SoapQName";
			Converter.valueA[33] = "System.Runtime.Remoting.Metadata.W3cXsd2001.SoapNotation";
			Converter.valueA[34] = "System.Runtime.Remoting.Metadata.W3cXsd2001.SoapNormalizedString";
			Converter.valueA[35] = "System.Runtime.Remoting.Metadata.W3cXsd2001.SoapToken";
			Converter.valueA[36] = "System.Runtime.Remoting.Metadata.W3cXsd2001.SoapLanguage";
			Converter.valueA[37] = "System.Runtime.Remoting.Metadata.W3cXsd2001.SoapName";
			Converter.valueA[38] = "System.Runtime.Remoting.Metadata.W3cXsd2001.SoapIdrefs";
			Converter.valueA[39] = "System.Runtime.Remoting.Metadata.W3cXsd2001.SoapEntities";
			Converter.valueA[40] = "System.Runtime.Remoting.Metadata.W3cXsd2001.SoapNmtoken";
			Converter.valueA[41] = "System.Runtime.Remoting.Metadata.W3cXsd2001.SoapNmtokens";
			Converter.valueA[42] = "System.Runtime.Remoting.Metadata.W3cXsd2001.SoapNcName";
			Converter.valueA[43] = "System.Runtime.Remoting.Metadata.W3cXsd2001.SoapId";
			Converter.valueA[44] = "System.Runtime.Remoting.Metadata.W3cXsd2001.SoapIdref";
			Converter.valueA[45] = "System.Runtime.Remoting.Metadata.W3cXsd2001.SoapEntity";
		}

		// Token: 0x0600008E RID: 142 RVA: 0x00006E09 File Offset: 0x00005E09
		internal static string SoapToComType(InternalPrimitiveTypeE code)
		{
			return Converter.ToComType(code);
		}

		// Token: 0x0600008F RID: 143 RVA: 0x00006E14 File Offset: 0x00005E14
		internal static string ToComType(InternalPrimitiveTypeE code)
		{
			lock (Converter.typeofConverter)
			{
				if (Converter.valueA == null)
				{
					Converter.InitValueA();
				}
			}
			return Converter.valueA[(int)code];
		}

		// Token: 0x06000090 RID: 144 RVA: 0x00006E5C File Offset: 0x00005E5C
		private static void InitValueB()
		{
			Converter.valueB = new string[Converter.primitiveTypeEnumLength];
			Converter.valueB[0] = null;
			Converter.valueB[1] = "boolean";
			Converter.valueB[2] = "unsignedByte";
			Converter.valueB[3] = "char";
			Converter.valueB[5] = "decimal";
			Converter.valueB[6] = "double";
			Converter.valueB[7] = "short";
			Converter.valueB[8] = "int";
			Converter.valueB[9] = "long";
			Converter.valueB[10] = "byte";
			Converter.valueB[11] = "float";
			Converter.valueB[12] = "duration";
			Converter.valueB[13] = "dateTime";
			Converter.valueB[14] = "unsignedShort";
			Converter.valueB[15] = "unsignedInt";
			Converter.valueB[16] = "unsignedLong";
			Converter.valueB[17] = SoapTime.XsdType;
			Converter.valueB[18] = SoapDate.XsdType;
			Converter.valueB[19] = SoapYearMonth.XsdType;
			Converter.valueB[20] = SoapYear.XsdType;
			Converter.valueB[21] = SoapMonthDay.XsdType;
			Converter.valueB[22] = SoapDay.XsdType;
			Converter.valueB[23] = SoapMonth.XsdType;
			Converter.valueB[24] = SoapHexBinary.XsdType;
			Converter.valueB[25] = SoapBase64Binary.XsdType;
			Converter.valueB[26] = SoapInteger.XsdType;
			Converter.valueB[27] = SoapPositiveInteger.XsdType;
			Converter.valueB[28] = SoapNonPositiveInteger.XsdType;
			Converter.valueB[29] = SoapNonNegativeInteger.XsdType;
			Converter.valueB[30] = SoapNegativeInteger.XsdType;
			Converter.valueB[31] = SoapAnyUri.XsdType;
			Converter.valueB[32] = SoapQName.XsdType;
			Converter.valueB[33] = SoapNotation.XsdType;
			Converter.valueB[34] = SoapNormalizedString.XsdType;
			Converter.valueB[35] = SoapToken.XsdType;
			Converter.valueB[36] = SoapLanguage.XsdType;
			Converter.valueB[37] = SoapName.XsdType;
			Converter.valueB[38] = SoapIdrefs.XsdType;
			Converter.valueB[39] = SoapEntities.XsdType;
			Converter.valueB[40] = SoapNmtoken.XsdType;
			Converter.valueB[41] = SoapNmtokens.XsdType;
			Converter.valueB[42] = SoapNcName.XsdType;
			Converter.valueB[43] = SoapId.XsdType;
			Converter.valueB[44] = SoapIdref.XsdType;
			Converter.valueB[45] = SoapEntity.XsdType;
		}

		// Token: 0x06000091 RID: 145 RVA: 0x000070B8 File Offset: 0x000060B8
		internal static string ToXmlDataType(InternalPrimitiveTypeE code)
		{
			lock (Converter.typeofConverter)
			{
				if (Converter.valueB == null)
				{
					Converter.InitValueB();
				}
			}
			return Converter.valueB[(int)code];
		}

		// Token: 0x06000092 RID: 146 RVA: 0x00007100 File Offset: 0x00006100
		private static void InitTypeCodeA()
		{
			Converter.typeCodeA = new TypeCode[Converter.primitiveTypeEnumLength];
			Converter.typeCodeA[0] = TypeCode.Object;
			Converter.typeCodeA[1] = TypeCode.Boolean;
			Converter.typeCodeA[2] = TypeCode.Byte;
			Converter.typeCodeA[3] = TypeCode.Char;
			Converter.typeCodeA[5] = TypeCode.Decimal;
			Converter.typeCodeA[6] = TypeCode.Double;
			Converter.typeCodeA[7] = TypeCode.Int16;
			Converter.typeCodeA[8] = TypeCode.Int32;
			Converter.typeCodeA[9] = TypeCode.Int64;
			Converter.typeCodeA[10] = TypeCode.SByte;
			Converter.typeCodeA[11] = TypeCode.Single;
			Converter.typeCodeA[12] = TypeCode.Object;
			Converter.typeCodeA[13] = TypeCode.DateTime;
			Converter.typeCodeA[14] = TypeCode.UInt16;
			Converter.typeCodeA[15] = TypeCode.UInt32;
			Converter.typeCodeA[16] = TypeCode.UInt64;
			Converter.typeCodeA[17] = TypeCode.Object;
			Converter.typeCodeA[18] = TypeCode.Object;
			Converter.typeCodeA[19] = TypeCode.Object;
			Converter.typeCodeA[20] = TypeCode.Object;
			Converter.typeCodeA[21] = TypeCode.Object;
			Converter.typeCodeA[22] = TypeCode.Object;
			Converter.typeCodeA[23] = TypeCode.Object;
			Converter.typeCodeA[24] = TypeCode.Object;
			Converter.typeCodeA[25] = TypeCode.Object;
			Converter.typeCodeA[26] = TypeCode.Object;
			Converter.typeCodeA[27] = TypeCode.Object;
			Converter.typeCodeA[28] = TypeCode.Object;
			Converter.typeCodeA[29] = TypeCode.Object;
			Converter.typeCodeA[30] = TypeCode.Object;
			Converter.typeCodeA[31] = TypeCode.Object;
			Converter.typeCodeA[32] = TypeCode.Object;
			Converter.typeCodeA[33] = TypeCode.Object;
			Converter.typeCodeA[34] = TypeCode.Object;
			Converter.typeCodeA[35] = TypeCode.Object;
			Converter.typeCodeA[36] = TypeCode.Object;
			Converter.typeCodeA[37] = TypeCode.Object;
			Converter.typeCodeA[38] = TypeCode.Object;
			Converter.typeCodeA[39] = TypeCode.Object;
			Converter.typeCodeA[40] = TypeCode.Object;
			Converter.typeCodeA[41] = TypeCode.Object;
			Converter.typeCodeA[42] = TypeCode.Object;
			Converter.typeCodeA[43] = TypeCode.Object;
			Converter.typeCodeA[44] = TypeCode.Object;
			Converter.typeCodeA[45] = TypeCode.Object;
		}

		// Token: 0x06000093 RID: 147 RVA: 0x000072B4 File Offset: 0x000062B4
		internal static TypeCode ToTypeCode(InternalPrimitiveTypeE code)
		{
			lock (Converter.typeofConverter)
			{
				if (Converter.typeCodeA == null)
				{
					Converter.InitTypeCodeA();
				}
			}
			return Converter.typeCodeA[(int)code];
		}

		// Token: 0x06000094 RID: 148 RVA: 0x000072FC File Offset: 0x000062FC
		private static void InitCodeA()
		{
			Converter.codeA = new InternalPrimitiveTypeE[19];
			Converter.codeA[0] = InternalPrimitiveTypeE.Invalid;
			Converter.codeA[1] = InternalPrimitiveTypeE.Invalid;
			Converter.codeA[2] = InternalPrimitiveTypeE.Invalid;
			Converter.codeA[3] = InternalPrimitiveTypeE.Boolean;
			Converter.codeA[4] = InternalPrimitiveTypeE.Char;
			Converter.codeA[5] = InternalPrimitiveTypeE.SByte;
			Converter.codeA[6] = InternalPrimitiveTypeE.Byte;
			Converter.codeA[7] = InternalPrimitiveTypeE.Int16;
			Converter.codeA[8] = InternalPrimitiveTypeE.UInt16;
			Converter.codeA[9] = InternalPrimitiveTypeE.Int32;
			Converter.codeA[10] = InternalPrimitiveTypeE.UInt32;
			Converter.codeA[11] = InternalPrimitiveTypeE.Int64;
			Converter.codeA[12] = InternalPrimitiveTypeE.UInt64;
			Converter.codeA[13] = InternalPrimitiveTypeE.Single;
			Converter.codeA[14] = InternalPrimitiveTypeE.Double;
			Converter.codeA[15] = InternalPrimitiveTypeE.Decimal;
			Converter.codeA[16] = InternalPrimitiveTypeE.DateTime;
			Converter.codeA[17] = InternalPrimitiveTypeE.Invalid;
			Converter.codeA[18] = InternalPrimitiveTypeE.Invalid;
		}

		// Token: 0x06000095 RID: 149 RVA: 0x000073C0 File Offset: 0x000063C0
		internal static InternalPrimitiveTypeE ToPrimitiveTypeEnum(TypeCode typeCode)
		{
			lock (Converter.typeofConverter)
			{
				if (Converter.codeA == null)
				{
					Converter.InitCodeA();
				}
			}
			return Converter.codeA[(int)typeCode];
		}

		// Token: 0x06000096 RID: 150 RVA: 0x00007408 File Offset: 0x00006408
		private static void InitEscapeA()
		{
			Converter.escapeA = new bool[Converter.primitiveTypeEnumLength];
			Converter.escapeA[0] = true;
			Converter.escapeA[1] = false;
			Converter.escapeA[2] = false;
			Converter.escapeA[3] = true;
			Converter.escapeA[5] = false;
			Converter.escapeA[6] = false;
			Converter.escapeA[7] = false;
			Converter.escapeA[8] = false;
			Converter.escapeA[9] = false;
			Converter.escapeA[10] = false;
			Converter.escapeA[11] = false;
			Converter.escapeA[12] = false;
			Converter.escapeA[13] = false;
			Converter.escapeA[14] = false;
			Converter.escapeA[15] = false;
			Converter.escapeA[16] = false;
			Converter.escapeA[17] = false;
			Converter.escapeA[18] = false;
			Converter.escapeA[19] = false;
			Converter.escapeA[20] = false;
			Converter.escapeA[21] = false;
			Converter.escapeA[22] = false;
			Converter.escapeA[23] = false;
			Converter.escapeA[24] = false;
			Converter.escapeA[25] = false;
			Converter.escapeA[26] = false;
			Converter.escapeA[27] = false;
			Converter.escapeA[28] = false;
			Converter.escapeA[29] = false;
			Converter.escapeA[30] = false;
			Converter.escapeA[31] = true;
			Converter.escapeA[32] = true;
			Converter.escapeA[33] = true;
			Converter.escapeA[34] = false;
			Converter.escapeA[35] = true;
			Converter.escapeA[36] = true;
			Converter.escapeA[37] = true;
			Converter.escapeA[38] = true;
			Converter.escapeA[39] = true;
			Converter.escapeA[40] = true;
			Converter.escapeA[41] = true;
			Converter.escapeA[42] = true;
			Converter.escapeA[43] = true;
			Converter.escapeA[44] = true;
			Converter.escapeA[45] = true;
		}

		// Token: 0x06000097 RID: 151 RVA: 0x000075B4 File Offset: 0x000065B4
		internal static bool IsEscaped(InternalPrimitiveTypeE code)
		{
			lock (Converter.typeofConverter)
			{
				if (Converter.escapeA == null)
				{
					Converter.InitEscapeA();
				}
			}
			return Converter.escapeA[(int)code];
		}

		// Token: 0x06000098 RID: 152 RVA: 0x000075FC File Offset: 0x000065FC
		internal static string SoapToString(object data, InternalPrimitiveTypeE code)
		{
			return Converter.ToString(data, code);
		}

		// Token: 0x06000099 RID: 153 RVA: 0x00007608 File Offset: 0x00006608
		internal static string ToString(object data, InternalPrimitiveTypeE code)
		{
			switch (code)
			{
			case InternalPrimitiveTypeE.Invalid:
				return data.ToString();
			case InternalPrimitiveTypeE.Boolean:
			{
				bool flag = (bool)data;
				if (flag)
				{
					return "true";
				}
				return "false";
			}
			case InternalPrimitiveTypeE.Double:
			{
				double num = (double)data;
				if (double.IsPositiveInfinity(num))
				{
					return "INF";
				}
				if (double.IsNegativeInfinity(num))
				{
					return "-INF";
				}
				return num.ToString("R", CultureInfo.InvariantCulture);
			}
			case InternalPrimitiveTypeE.Single:
			{
				float num2 = (float)data;
				if (float.IsPositiveInfinity(num2))
				{
					return "INF";
				}
				if (float.IsNegativeInfinity(num2))
				{
					return "-INF";
				}
				return num2.ToString("R", CultureInfo.InvariantCulture);
			}
			case InternalPrimitiveTypeE.TimeSpan:
				return SoapDuration.ToString((TimeSpan)data);
			case InternalPrimitiveTypeE.DateTime:
				return SoapDateTime.ToString((DateTime)data);
			case InternalPrimitiveTypeE.Time:
			case InternalPrimitiveTypeE.Date:
			case InternalPrimitiveTypeE.YearMonth:
			case InternalPrimitiveTypeE.Year:
			case InternalPrimitiveTypeE.MonthDay:
			case InternalPrimitiveTypeE.Day:
			case InternalPrimitiveTypeE.Month:
			case InternalPrimitiveTypeE.HexBinary:
			case InternalPrimitiveTypeE.Base64Binary:
			case InternalPrimitiveTypeE.Integer:
			case InternalPrimitiveTypeE.PositiveInteger:
			case InternalPrimitiveTypeE.NonPositiveInteger:
			case InternalPrimitiveTypeE.NonNegativeInteger:
			case InternalPrimitiveTypeE.NegativeInteger:
			case InternalPrimitiveTypeE.AnyUri:
			case InternalPrimitiveTypeE.QName:
			case InternalPrimitiveTypeE.Notation:
			case InternalPrimitiveTypeE.NormalizedString:
			case InternalPrimitiveTypeE.Token:
			case InternalPrimitiveTypeE.Language:
			case InternalPrimitiveTypeE.Name:
			case InternalPrimitiveTypeE.Idrefs:
			case InternalPrimitiveTypeE.Entities:
			case InternalPrimitiveTypeE.Nmtoken:
			case InternalPrimitiveTypeE.Nmtokens:
			case InternalPrimitiveTypeE.NcName:
			case InternalPrimitiveTypeE.Id:
			case InternalPrimitiveTypeE.Idref:
			case InternalPrimitiveTypeE.Entity:
				return data.ToString();
			}
			return (string)Convert.ChangeType(data, Converter.typeofString, CultureInfo.InvariantCulture);
		}

		// Token: 0x0600009A RID: 154 RVA: 0x000077C0 File Offset: 0x000067C0
		internal static object FromString(string value, InternalPrimitiveTypeE code)
		{
			switch (code)
			{
			case InternalPrimitiveTypeE.Boolean:
				if (value == "1" || value == "true")
				{
					return true;
				}
				if (value == "0" || value == "false")
				{
					return false;
				}
				throw new SerializationException(string.Format(CultureInfo.CurrentCulture, SoapUtil.GetResourceString("Serialization_typeCoercion"), new object[] { value, "Boolean" }));
			case InternalPrimitiveTypeE.Double:
				if (value == "INF")
				{
					return double.PositiveInfinity;
				}
				if (value == "-INF")
				{
					return double.NegativeInfinity;
				}
				return double.Parse(value, CultureInfo.InvariantCulture);
			case InternalPrimitiveTypeE.Single:
				if (value == "INF")
				{
					return float.PositiveInfinity;
				}
				if (value == "-INF")
				{
					return float.NegativeInfinity;
				}
				return float.Parse(value, CultureInfo.InvariantCulture);
			case InternalPrimitiveTypeE.TimeSpan:
				return SoapDuration.Parse(value);
			case InternalPrimitiveTypeE.DateTime:
				return SoapDateTime.Parse(value);
			case InternalPrimitiveTypeE.Time:
				return SoapTime.Parse(value);
			case InternalPrimitiveTypeE.Date:
				return SoapDate.Parse(value);
			case InternalPrimitiveTypeE.YearMonth:
				return SoapYearMonth.Parse(value);
			case InternalPrimitiveTypeE.Year:
				return SoapYear.Parse(value);
			case InternalPrimitiveTypeE.MonthDay:
				return SoapMonthDay.Parse(value);
			case InternalPrimitiveTypeE.Day:
				return SoapDay.Parse(value);
			case InternalPrimitiveTypeE.Month:
				return SoapMonth.Parse(value);
			case InternalPrimitiveTypeE.HexBinary:
				return SoapHexBinary.Parse(value);
			case InternalPrimitiveTypeE.Base64Binary:
				return SoapBase64Binary.Parse(value);
			case InternalPrimitiveTypeE.Integer:
				return SoapInteger.Parse(value);
			case InternalPrimitiveTypeE.PositiveInteger:
				return SoapPositiveInteger.Parse(value);
			case InternalPrimitiveTypeE.NonPositiveInteger:
				return SoapNonPositiveInteger.Parse(value);
			case InternalPrimitiveTypeE.NonNegativeInteger:
				return SoapNonNegativeInteger.Parse(value);
			case InternalPrimitiveTypeE.NegativeInteger:
				return SoapNegativeInteger.Parse(value);
			case InternalPrimitiveTypeE.AnyUri:
				return SoapAnyUri.Parse(value);
			case InternalPrimitiveTypeE.QName:
				return SoapQName.Parse(value);
			case InternalPrimitiveTypeE.Notation:
				return SoapNotation.Parse(value);
			case InternalPrimitiveTypeE.NormalizedString:
				return SoapNormalizedString.Parse(value);
			case InternalPrimitiveTypeE.Token:
				return SoapToken.Parse(value);
			case InternalPrimitiveTypeE.Language:
				return SoapLanguage.Parse(value);
			case InternalPrimitiveTypeE.Name:
				return SoapName.Parse(value);
			case InternalPrimitiveTypeE.Idrefs:
				return SoapIdrefs.Parse(value);
			case InternalPrimitiveTypeE.Entities:
				return SoapEntities.Parse(value);
			case InternalPrimitiveTypeE.Nmtoken:
				return SoapNmtoken.Parse(value);
			case InternalPrimitiveTypeE.Nmtokens:
				return SoapNmtokens.Parse(value);
			case InternalPrimitiveTypeE.NcName:
				return SoapNcName.Parse(value);
			case InternalPrimitiveTypeE.Id:
				return SoapId.Parse(value);
			case InternalPrimitiveTypeE.Idref:
				return SoapIdref.Parse(value);
			case InternalPrimitiveTypeE.Entity:
				return SoapEntity.Parse(value);
			}
			object obj;
			if (code != InternalPrimitiveTypeE.Invalid)
			{
				obj = Convert.ChangeType(value, Converter.ToTypeCode(code), CultureInfo.InvariantCulture);
			}
			else
			{
				obj = value;
			}
			return obj;
		}

		// Token: 0x04000136 RID: 310
		private static int primitiveTypeEnumLength = 46;

		// Token: 0x04000137 RID: 311
		private static Type[] typeA;

		// Token: 0x04000138 RID: 312
		private static string[] valueA;

		// Token: 0x04000139 RID: 313
		private static string[] valueB;

		// Token: 0x0400013A RID: 314
		private static TypeCode[] typeCodeA;

		// Token: 0x0400013B RID: 315
		private static InternalPrimitiveTypeE[] codeA;

		// Token: 0x0400013C RID: 316
		private static bool[] escapeA;

		// Token: 0x0400013D RID: 317
		private static StringBuilder sb = new StringBuilder(30);

		// Token: 0x0400013E RID: 318
		internal static Type typeofISerializable = typeof(ISerializable);

		// Token: 0x0400013F RID: 319
		internal static Type typeofString = typeof(string);

		// Token: 0x04000140 RID: 320
		internal static Type typeofConverter = typeof(Converter);

		// Token: 0x04000141 RID: 321
		internal static Type typeofBoolean = typeof(bool);

		// Token: 0x04000142 RID: 322
		internal static Type typeofByte = typeof(byte);

		// Token: 0x04000143 RID: 323
		internal static Type typeofChar = typeof(char);

		// Token: 0x04000144 RID: 324
		internal static Type typeofDecimal = typeof(decimal);

		// Token: 0x04000145 RID: 325
		internal static Type typeofDouble = typeof(double);

		// Token: 0x04000146 RID: 326
		internal static Type typeofInt16 = typeof(short);

		// Token: 0x04000147 RID: 327
		internal static Type typeofInt32 = typeof(int);

		// Token: 0x04000148 RID: 328
		internal static Type typeofInt64 = typeof(long);

		// Token: 0x04000149 RID: 329
		internal static Type typeofSByte = typeof(sbyte);

		// Token: 0x0400014A RID: 330
		internal static Type typeofSingle = typeof(float);

		// Token: 0x0400014B RID: 331
		internal static Type typeofTimeSpan = typeof(TimeSpan);

		// Token: 0x0400014C RID: 332
		internal static Type typeofDateTime = typeof(DateTime);

		// Token: 0x0400014D RID: 333
		internal static Type typeofUInt16 = typeof(ushort);

		// Token: 0x0400014E RID: 334
		internal static Type typeofUInt32 = typeof(uint);

		// Token: 0x0400014F RID: 335
		internal static Type typeofUInt64 = typeof(ulong);

		// Token: 0x04000150 RID: 336
		internal static Type typeofSoapTime = typeof(SoapTime);

		// Token: 0x04000151 RID: 337
		internal static Type typeofSoapDate = typeof(SoapDate);

		// Token: 0x04000152 RID: 338
		internal static Type typeofSoapYear = typeof(SoapYear);

		// Token: 0x04000153 RID: 339
		internal static Type typeofSoapMonthDay = typeof(SoapMonthDay);

		// Token: 0x04000154 RID: 340
		internal static Type typeofSoapYearMonth = typeof(SoapYearMonth);

		// Token: 0x04000155 RID: 341
		internal static Type typeofSoapDay = typeof(SoapDay);

		// Token: 0x04000156 RID: 342
		internal static Type typeofSoapMonth = typeof(SoapMonth);

		// Token: 0x04000157 RID: 343
		internal static Type typeofSoapHexBinary = typeof(SoapHexBinary);

		// Token: 0x04000158 RID: 344
		internal static Type typeofSoapBase64Binary = typeof(SoapBase64Binary);

		// Token: 0x04000159 RID: 345
		internal static Type typeofSoapInteger = typeof(SoapInteger);

		// Token: 0x0400015A RID: 346
		internal static Type typeofSoapPositiveInteger = typeof(SoapPositiveInteger);

		// Token: 0x0400015B RID: 347
		internal static Type typeofSoapNonPositiveInteger = typeof(SoapNonPositiveInteger);

		// Token: 0x0400015C RID: 348
		internal static Type typeofSoapNonNegativeInteger = typeof(SoapNonNegativeInteger);

		// Token: 0x0400015D RID: 349
		internal static Type typeofSoapNegativeInteger = typeof(SoapNegativeInteger);

		// Token: 0x0400015E RID: 350
		internal static Type typeofSoapAnyUri = typeof(SoapAnyUri);

		// Token: 0x0400015F RID: 351
		internal static Type typeofSoapQName = typeof(SoapQName);

		// Token: 0x04000160 RID: 352
		internal static Type typeofSoapNotation = typeof(SoapNotation);

		// Token: 0x04000161 RID: 353
		internal static Type typeofSoapNormalizedString = typeof(SoapNormalizedString);

		// Token: 0x04000162 RID: 354
		internal static Type typeofSoapToken = typeof(SoapToken);

		// Token: 0x04000163 RID: 355
		internal static Type typeofSoapLanguage = typeof(SoapLanguage);

		// Token: 0x04000164 RID: 356
		internal static Type typeofSoapName = typeof(SoapName);

		// Token: 0x04000165 RID: 357
		internal static Type typeofSoapIdrefs = typeof(SoapIdrefs);

		// Token: 0x04000166 RID: 358
		internal static Type typeofSoapEntities = typeof(SoapEntities);

		// Token: 0x04000167 RID: 359
		internal static Type typeofSoapNmtoken = typeof(SoapNmtoken);

		// Token: 0x04000168 RID: 360
		internal static Type typeofSoapNmtokens = typeof(SoapNmtokens);

		// Token: 0x04000169 RID: 361
		internal static Type typeofSoapNcName = typeof(SoapNcName);

		// Token: 0x0400016A RID: 362
		internal static Type typeofSoapId = typeof(SoapId);

		// Token: 0x0400016B RID: 363
		internal static Type typeofSoapIdref = typeof(SoapIdref);

		// Token: 0x0400016C RID: 364
		internal static Type typeofSoapEntity = typeof(SoapEntity);

		// Token: 0x0400016D RID: 365
		internal static Type typeofISoapXsd = typeof(ISoapXsd);

		// Token: 0x0400016E RID: 366
		internal static Type typeofObject = typeof(object);

		// Token: 0x0400016F RID: 367
		internal static Type typeofSoapFault = typeof(SoapFault);

		// Token: 0x04000170 RID: 368
		internal static Type typeofTypeArray = typeof(Type[]);

		// Token: 0x04000171 RID: 369
		internal static Type typeofIConstructionCallMessage = typeof(IConstructionCallMessage);

		// Token: 0x04000172 RID: 370
		internal static Type typeofIMethodCallMessage = typeof(IMethodCallMessage);

		// Token: 0x04000173 RID: 371
		internal static Type typeofReturnMessage = typeof(ReturnMessage);

		// Token: 0x04000174 RID: 372
		internal static Type typeofSystemVoid = typeof(void);

		// Token: 0x04000175 RID: 373
		internal static Type typeofInternalSoapMessage = typeof(InternalSoapMessage);

		// Token: 0x04000176 RID: 374
		internal static Type typeofHeader = typeof(Header);

		// Token: 0x04000177 RID: 375
		internal static Type typeofMarshalByRefObject = typeof(MarshalByRefObject);

		// Token: 0x04000178 RID: 376
		internal static Assembly urtAssembly = Assembly.GetAssembly(Converter.typeofString);

		// Token: 0x04000179 RID: 377
		internal static string urtAssemblyString = Converter.urtAssembly.FullName;
	}
}
