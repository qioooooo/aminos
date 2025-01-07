using System;
using System.Collections;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Schema;

namespace System.Xml
{
	public class XmlConvert
	{
		public static string EncodeName(string name)
		{
			return XmlConvert.EncodeName(name, true, false);
		}

		public static string EncodeNmToken(string name)
		{
			return XmlConvert.EncodeName(name, false, false);
		}

		public static string EncodeLocalName(string name)
		{
			return XmlConvert.EncodeName(name, true, true);
		}

		public static string DecodeName(string name)
		{
			if (name == null || name.Length == 0)
			{
				return name;
			}
			StringBuilder stringBuilder = null;
			int length = name.Length;
			int num = 0;
			int num2 = name.IndexOf('_');
			if (num2 < 0)
			{
				return name;
			}
			if (XmlConvert.c_DecodeCharPattern == null)
			{
				XmlConvert.c_DecodeCharPattern = new Regex("_[Xx]([0-9a-fA-F]{4}|[0-9a-fA-F]{8})_");
			}
			MatchCollection matchCollection = XmlConvert.c_DecodeCharPattern.Matches(name, num2);
			IEnumerator enumerator = matchCollection.GetEnumerator();
			int num3 = -1;
			if (enumerator != null && enumerator.MoveNext())
			{
				Match match = (Match)enumerator.Current;
				num3 = match.Index;
			}
			for (int i = 0; i < length - XmlConvert.c_EncodedCharLength + 1; i++)
			{
				if (i == num3)
				{
					if (enumerator.MoveNext())
					{
						Match match2 = (Match)enumerator.Current;
						num3 = match2.Index;
					}
					if (stringBuilder == null)
					{
						stringBuilder = new StringBuilder(length + 20);
					}
					stringBuilder.Append(name, num, i - num);
					if (name[i + 6] != '_')
					{
						int num4 = XmlConvert.FromHex(name[i + 2]) * 268435456 + XmlConvert.FromHex(name[i + 3]) * 16777216 + XmlConvert.FromHex(name[i + 4]) * 1048576 + XmlConvert.FromHex(name[i + 5]) * 65536 + XmlConvert.FromHex(name[i + 6]) * 4096 + XmlConvert.FromHex(name[i + 7]) * 256 + XmlConvert.FromHex(name[i + 8]) * 16 + XmlConvert.FromHex(name[i + 9]);
						if (num4 >= 65536)
						{
							if (num4 <= 1114111)
							{
								num = i + XmlConvert.c_EncodedCharLength + 4;
								char c = (char)((num4 - 65536) / 1024 + 55296);
								char c2 = (char)(num4 - (int)((c - '\ud800') * 'Ѐ') - 65536 + 56320);
								stringBuilder.Append(c);
								stringBuilder.Append(c2);
							}
						}
						else
						{
							num = i + XmlConvert.c_EncodedCharLength + 4;
							stringBuilder.Append((char)num4);
						}
						i += XmlConvert.c_EncodedCharLength - 1 + 4;
					}
					else
					{
						num = i + XmlConvert.c_EncodedCharLength;
						stringBuilder.Append((char)(XmlConvert.FromHex(name[i + 2]) * 4096 + XmlConvert.FromHex(name[i + 3]) * 256 + XmlConvert.FromHex(name[i + 4]) * 16 + XmlConvert.FromHex(name[i + 5])));
						i += XmlConvert.c_EncodedCharLength - 1;
					}
				}
			}
			if (num == 0)
			{
				return name;
			}
			if (num < length)
			{
				stringBuilder.Append(name, num, length - num);
			}
			return stringBuilder.ToString();
		}

		private static string EncodeName(string name, bool first, bool local)
		{
			if (name == null)
			{
				return name;
			}
			if (name.Length == 0)
			{
				if (!first)
				{
					throw new XmlException("Xml_InvalidNmToken", name);
				}
				return name;
			}
			else
			{
				StringBuilder stringBuilder = null;
				int length = name.Length;
				int num = 0;
				int i = 0;
				XmlCharType instance = XmlCharType.Instance;
				int num2 = name.IndexOf('_');
				IEnumerator enumerator = null;
				if (num2 >= 0)
				{
					if (XmlConvert.c_EncodeCharPattern == null)
					{
						XmlConvert.c_EncodeCharPattern = new Regex("(?<=_)[Xx]([0-9a-fA-F]{4}|[0-9a-fA-F]{8})_");
					}
					MatchCollection matchCollection = XmlConvert.c_EncodeCharPattern.Matches(name, num2);
					enumerator = matchCollection.GetEnumerator();
				}
				int num3 = -1;
				if (enumerator != null && enumerator.MoveNext())
				{
					Match match = (Match)enumerator.Current;
					num3 = match.Index - 1;
				}
				if (first && ((!instance.IsStartNCNameChar(name[0]) && (local || (!local && name[0] != ':'))) || num3 == 0))
				{
					if (stringBuilder == null)
					{
						stringBuilder = new StringBuilder(length + 20);
					}
					stringBuilder.Append("_x");
					if (length > 1 && name[0] >= '\ud800' && name[0] <= '\udbff' && name[1] >= '\udc00' && name[1] <= '\udfff')
					{
						int num4 = (int)name[0];
						int num5 = (int)name[1];
						stringBuilder.Append(((num4 - 55296) * 1024 + (num5 - 56320) + 65536).ToString("X8", CultureInfo.InvariantCulture));
						i++;
						num = 2;
					}
					else
					{
						stringBuilder.Append(((int)name[0]).ToString("X4", CultureInfo.InvariantCulture));
						num = 1;
					}
					stringBuilder.Append("_");
					i++;
					if (num3 == 0 && enumerator.MoveNext())
					{
						Match match2 = (Match)enumerator.Current;
						num3 = match2.Index - 1;
					}
				}
				while (i < length)
				{
					if ((local && !instance.IsNCNameChar(name[i])) || (!local && !instance.IsNameChar(name[i])) || num3 == i)
					{
						if (stringBuilder == null)
						{
							stringBuilder = new StringBuilder(length + 20);
						}
						if (num3 == i && enumerator.MoveNext())
						{
							Match match3 = (Match)enumerator.Current;
							num3 = match3.Index - 1;
						}
						stringBuilder.Append(name, num, i - num);
						stringBuilder.Append("_x");
						if (length > i + 1 && name[i] >= '\ud800' && name[i] <= '\udbff' && name[i + 1] >= '\udc00' && name[i + 1] <= '\udfff')
						{
							int num6 = (int)name[i];
							int num7 = (int)name[i + 1];
							stringBuilder.Append(((num6 - 55296) * 1024 + (num7 - 56320) + 65536).ToString("X8", CultureInfo.InvariantCulture));
							num = i + 2;
							i++;
						}
						else
						{
							stringBuilder.Append(((int)name[i]).ToString("X4", CultureInfo.InvariantCulture));
							num = i + 1;
						}
						stringBuilder.Append("_");
					}
					i++;
				}
				if (num == 0)
				{
					return name;
				}
				if (num < length)
				{
					stringBuilder.Append(name, num, length - num);
				}
				return stringBuilder.ToString();
			}
		}

		private static int FromHex(char digit)
		{
			if (digit > '9')
			{
				return (int)(((digit <= 'F') ? (digit - 'A') : (digit - 'a')) + '\n');
			}
			return (int)(digit - '0');
		}

		internal static byte[] FromBinHexString(string s)
		{
			return XmlConvert.FromBinHexString(s, true);
		}

		internal static byte[] FromBinHexString(string s, bool allowOddCount)
		{
			return BinHexDecoder.Decode(s.ToCharArray(), allowOddCount);
		}

		internal static string ToBinHexString(byte[] inArray)
		{
			return BinHexEncoder.Encode(inArray, 0, inArray.Length);
		}

		public unsafe static string VerifyName(string name)
		{
			if (name == null || name.Length == 0)
			{
				throw new ArgumentNullException("name");
			}
			XmlCharType instance = XmlCharType.Instance;
			char c = name[0];
			if ((instance.charProperties[c] & 4) == 0 && c != ':')
			{
				throw new XmlException("Xml_BadStartNameChar", XmlException.BuildCharExceptionStr(c));
			}
			for (int i = 1; i < name.Length; i++)
			{
				if ((instance.charProperties[name[i]] & 8) == 0 && name[i] != ':')
				{
					throw new XmlException("Xml_BadNameChar", XmlException.BuildCharExceptionStr(name[i]));
				}
			}
			return name;
		}

		internal static Exception TryVerifyName(string name)
		{
			if (name == null || name.Length == 0)
			{
				return new XmlException("Xml_EmptyName", string.Empty);
			}
			XmlCharType instance = XmlCharType.Instance;
			char c = name[0];
			if (!instance.IsStartNameChar(c) && c != ':')
			{
				return new XmlException("Xml_BadStartNameChar", XmlException.BuildCharExceptionStr(c));
			}
			for (int i = 1; i < name.Length; i++)
			{
				c = name[i];
				if (!instance.IsNameChar(c) && c != ':')
				{
					return new XmlException("Xml_BadNameChar", XmlException.BuildCharExceptionStr(c));
				}
			}
			return null;
		}

		internal static string VerifyQName(string name)
		{
			return XmlConvert.VerifyQName(name, ExceptionType.XmlException);
		}

		internal unsafe static string VerifyQName(string name, ExceptionType exceptionType)
		{
			if (name == null || name.Length == 0)
			{
				throw new ArgumentException("name");
			}
			XmlCharType instance = XmlCharType.Instance;
			int length = name.Length;
			int num = 0;
			int num2 = -1;
			while ((instance.charProperties[name[num]] & 4) != 0)
			{
				num++;
				while (num < length && (instance.charProperties[name[num]] & 8) != 0)
				{
					num++;
				}
				if (num == length)
				{
					return name;
				}
				if (name[num] != ':' || num2 != -1 || num + 1 >= length)
				{
					break;
				}
				num2 = num;
				num++;
			}
			throw XmlConvert.CreateException("Xml_BadNameChar", XmlException.BuildCharExceptionStr(name[num]), exceptionType);
		}

		public static string VerifyNCName(string name)
		{
			if (name == null || name.Length == 0)
			{
				throw new ArgumentNullException("name");
			}
			return ValidateNames.ParseNCNameThrow(name);
		}

		internal static Exception TryVerifyNCName(string name)
		{
			int num = ValidateNames.ParseNCName(name, 0);
			if (num == 0 || num != name.Length)
			{
				return ValidateNames.GetInvalidNameException(name, 0, num);
			}
			return null;
		}

		public static string VerifyTOKEN(string token)
		{
			if (token == null || token.Length == 0)
			{
				return token;
			}
			if (token[0] == ' ' || token[token.Length - 1] == ' ' || token.IndexOfAny(XmlConvert.crt) != -1 || token.IndexOf("  ", StringComparison.Ordinal) != -1)
			{
				throw new XmlException("Sch_NotTokenString", token);
			}
			return token;
		}

		internal static Exception TryVerifyTOKEN(string token)
		{
			if (token == null || token.Length == 0)
			{
				return null;
			}
			if (token[0] == ' ' || token[token.Length - 1] == ' ' || token.IndexOfAny(XmlConvert.crt) != -1 || token.IndexOf("  ", StringComparison.Ordinal) != -1)
			{
				return new XmlException("Sch_NotTokenString", token);
			}
			return null;
		}

		public static string VerifyNMTOKEN(string name)
		{
			return XmlConvert.VerifyNMTOKEN(name, ExceptionType.XmlException);
		}

		internal static string VerifyNMTOKEN(string name, ExceptionType exceptionType)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			if (name.Length == 0)
			{
				throw XmlConvert.CreateException("Xml_InvalidNmToken", name, exceptionType);
			}
			XmlCharType instance = XmlCharType.Instance;
			for (int i = 0; i < name.Length; i++)
			{
				if (!instance.IsNameChar(name[i]))
				{
					throw XmlConvert.CreateException("Xml_BadNameChar", XmlException.BuildCharExceptionStr(name[i]), exceptionType);
				}
			}
			return name;
		}

		internal static Exception TryVerifyNMTOKEN(string name)
		{
			if (name == null || name.Length == 0)
			{
				return new XmlException("Xml_EmptyName", string.Empty);
			}
			XmlCharType instance = XmlCharType.Instance;
			for (int i = 0; i < name.Length; i++)
			{
				if (!instance.IsNameChar(name[i]))
				{
					return new XmlException("Xml_BadNameChar", XmlException.BuildCharExceptionStr(name[i]));
				}
			}
			return null;
		}

		internal static string VerifyNormalizedString(string str)
		{
			if (str.IndexOfAny(XmlConvert.crt) != -1)
			{
				throw new XmlSchemaException("Sch_NotNormalizedString", str);
			}
			return str;
		}

		internal static Exception TryVerifyNormalizedString(string str)
		{
			if (str.IndexOfAny(XmlConvert.crt) != -1)
			{
				return new XmlSchemaException("Sch_NotNormalizedString", str);
			}
			return null;
		}

		public static string ToString(bool value)
		{
			if (!value)
			{
				return "false";
			}
			return "true";
		}

		public static string ToString(char value)
		{
			return value.ToString(null);
		}

		public static string ToString(decimal value)
		{
			return value.ToString(null, NumberFormatInfo.InvariantInfo);
		}

		[CLSCompliant(false)]
		public static string ToString(sbyte value)
		{
			return value.ToString(null, NumberFormatInfo.InvariantInfo);
		}

		public static string ToString(short value)
		{
			return value.ToString(null, NumberFormatInfo.InvariantInfo);
		}

		public static string ToString(int value)
		{
			return value.ToString(null, NumberFormatInfo.InvariantInfo);
		}

		public static string ToString(long value)
		{
			return value.ToString(null, NumberFormatInfo.InvariantInfo);
		}

		public static string ToString(byte value)
		{
			return value.ToString(null, NumberFormatInfo.InvariantInfo);
		}

		[CLSCompliant(false)]
		public static string ToString(ushort value)
		{
			return value.ToString(null, NumberFormatInfo.InvariantInfo);
		}

		[CLSCompliant(false)]
		public static string ToString(uint value)
		{
			return value.ToString(null, NumberFormatInfo.InvariantInfo);
		}

		[CLSCompliant(false)]
		public static string ToString(ulong value)
		{
			return value.ToString(null, NumberFormatInfo.InvariantInfo);
		}

		public static string ToString(float value)
		{
			if (float.IsNegativeInfinity(value))
			{
				return "-INF";
			}
			if (float.IsPositiveInfinity(value))
			{
				return "INF";
			}
			if (XmlConvert.IsNegativeZero((double)value))
			{
				return "-0";
			}
			return value.ToString("R", NumberFormatInfo.InvariantInfo);
		}

		public static string ToString(double value)
		{
			if (double.IsNegativeInfinity(value))
			{
				return "-INF";
			}
			if (double.IsPositiveInfinity(value))
			{
				return "INF";
			}
			if (XmlConvert.IsNegativeZero(value))
			{
				return "-0";
			}
			return value.ToString("R", NumberFormatInfo.InvariantInfo);
		}

		public static string ToString(TimeSpan value)
		{
			return new XsdDuration(value).ToString();
		}

		[Obsolete("Use XmlConvert.ToString() that takes in XmlDateTimeSerializationMode")]
		public static string ToString(DateTime value)
		{
			return XmlConvert.ToString(value, "yyyy-MM-ddTHH:mm:ss.fffffffzzzzzz");
		}

		public static string ToString(DateTime value, string format)
		{
			return value.ToString(format, DateTimeFormatInfo.InvariantInfo);
		}

		public static string ToString(DateTime value, XmlDateTimeSerializationMode dateTimeOption)
		{
			switch (dateTimeOption)
			{
			case XmlDateTimeSerializationMode.Local:
				value = XmlConvert.SwitchToLocalTime(value);
				break;
			case XmlDateTimeSerializationMode.Utc:
				value = XmlConvert.SwitchToUtcTime(value);
				break;
			case XmlDateTimeSerializationMode.Unspecified:
				value = new DateTime(value.Ticks, DateTimeKind.Unspecified);
				break;
			case XmlDateTimeSerializationMode.RoundtripKind:
				break;
			default:
				throw new ArgumentException(Res.GetString("Sch_InvalidDateTimeOption", new object[] { dateTimeOption }));
			}
			XsdDateTime xsdDateTime = new XsdDateTime(value, XsdDateTimeFlags.DateTime);
			return xsdDateTime.ToString();
		}

		public static string ToString(DateTimeOffset value)
		{
			XsdDateTime xsdDateTime = new XsdDateTime(value);
			return xsdDateTime.ToString();
		}

		public static string ToString(DateTimeOffset value, string format)
		{
			return value.ToString(format, DateTimeFormatInfo.InvariantInfo);
		}

		public static string ToString(Guid value)
		{
			return value.ToString();
		}

		public static bool ToBoolean(string s)
		{
			s = XmlConvert.TrimString(s);
			if (s == "1" || s == "true")
			{
				return true;
			}
			if (s == "0" || s == "false")
			{
				return false;
			}
			throw new FormatException(Res.GetString("XmlConvert_BadFormat", new object[] { s, "Boolean" }));
		}

		internal static Exception TryToBoolean(string s, out bool result)
		{
			s = XmlConvert.TrimString(s);
			if (s == "0" || s == "false")
			{
				result = false;
				return null;
			}
			if (s == "1" || s == "true")
			{
				result = true;
				return null;
			}
			result = false;
			return new FormatException(Res.GetString("XmlConvert_BadFormat", new object[] { s, "Boolean" }));
		}

		public static char ToChar(string s)
		{
			return char.Parse(s);
		}

		internal static Exception TryToChar(string s, out char result)
		{
			if (!char.TryParse(s, out result))
			{
				return new FormatException(Res.GetString("XmlConvert_BadFormat", new object[] { s, "Char" }));
			}
			return null;
		}

		public static decimal ToDecimal(string s)
		{
			return decimal.Parse(s, NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite | NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint, NumberFormatInfo.InvariantInfo);
		}

		internal static Exception TryToDecimal(string s, out decimal result)
		{
			if (!decimal.TryParse(s, NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite | NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint, NumberFormatInfo.InvariantInfo, out result))
			{
				return new FormatException(Res.GetString("XmlConvert_BadFormat", new object[] { s, "Decimal" }));
			}
			return null;
		}

		internal static decimal ToInteger(string s)
		{
			return decimal.Parse(s, NumberStyles.Integer, NumberFormatInfo.InvariantInfo);
		}

		internal static Exception TryToInteger(string s, out decimal result)
		{
			if (!decimal.TryParse(s, NumberStyles.Integer, NumberFormatInfo.InvariantInfo, out result))
			{
				return new FormatException(Res.GetString("XmlConvert_BadFormat", new object[] { s, "Integer" }));
			}
			return null;
		}

		[CLSCompliant(false)]
		public static sbyte ToSByte(string s)
		{
			return sbyte.Parse(s, NumberStyles.Integer, NumberFormatInfo.InvariantInfo);
		}

		internal static Exception TryToSByte(string s, out sbyte result)
		{
			if (!sbyte.TryParse(s, NumberStyles.Integer, NumberFormatInfo.InvariantInfo, out result))
			{
				return new FormatException(Res.GetString("XmlConvert_BadFormat", new object[] { s, "SByte" }));
			}
			return null;
		}

		public static short ToInt16(string s)
		{
			return short.Parse(s, NumberStyles.Integer, NumberFormatInfo.InvariantInfo);
		}

		internal static Exception TryToInt16(string s, out short result)
		{
			if (!short.TryParse(s, NumberStyles.Integer, NumberFormatInfo.InvariantInfo, out result))
			{
				return new FormatException(Res.GetString("XmlConvert_BadFormat", new object[] { s, "Int16" }));
			}
			return null;
		}

		public static int ToInt32(string s)
		{
			return int.Parse(s, NumberStyles.Integer, NumberFormatInfo.InvariantInfo);
		}

		internal static Exception TryToInt32(string s, out int result)
		{
			if (!int.TryParse(s, NumberStyles.Integer, NumberFormatInfo.InvariantInfo, out result))
			{
				return new FormatException(Res.GetString("XmlConvert_BadFormat", new object[] { s, "Int32" }));
			}
			return null;
		}

		public static long ToInt64(string s)
		{
			return long.Parse(s, NumberStyles.Integer, NumberFormatInfo.InvariantInfo);
		}

		internal static Exception TryToInt64(string s, out long result)
		{
			if (!long.TryParse(s, NumberStyles.Integer, NumberFormatInfo.InvariantInfo, out result))
			{
				return new FormatException(Res.GetString("XmlConvert_BadFormat", new object[] { s, "Int64" }));
			}
			return null;
		}

		public static byte ToByte(string s)
		{
			return byte.Parse(s, NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite, NumberFormatInfo.InvariantInfo);
		}

		internal static Exception TryToByte(string s, out byte result)
		{
			if (!byte.TryParse(s, NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite, NumberFormatInfo.InvariantInfo, out result))
			{
				return new FormatException(Res.GetString("XmlConvert_BadFormat", new object[] { s, "Byte" }));
			}
			return null;
		}

		[CLSCompliant(false)]
		public static ushort ToUInt16(string s)
		{
			return ushort.Parse(s, NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite, NumberFormatInfo.InvariantInfo);
		}

		internal static Exception TryToUInt16(string s, out ushort result)
		{
			if (!ushort.TryParse(s, NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite, NumberFormatInfo.InvariantInfo, out result))
			{
				return new FormatException(Res.GetString("XmlConvert_BadFormat", new object[] { s, "UInt16" }));
			}
			return null;
		}

		[CLSCompliant(false)]
		public static uint ToUInt32(string s)
		{
			return uint.Parse(s, NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite, NumberFormatInfo.InvariantInfo);
		}

		internal static Exception TryToUInt32(string s, out uint result)
		{
			if (!uint.TryParse(s, NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite, NumberFormatInfo.InvariantInfo, out result))
			{
				return new FormatException(Res.GetString("XmlConvert_BadFormat", new object[] { s, "UInt32" }));
			}
			return null;
		}

		[CLSCompliant(false)]
		public static ulong ToUInt64(string s)
		{
			return ulong.Parse(s, NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite, NumberFormatInfo.InvariantInfo);
		}

		internal static Exception TryToUInt64(string s, out ulong result)
		{
			if (!ulong.TryParse(s, NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite, NumberFormatInfo.InvariantInfo, out result))
			{
				return new FormatException(Res.GetString("XmlConvert_BadFormat", new object[] { s, "UInt64" }));
			}
			return null;
		}

		public static float ToSingle(string s)
		{
			s = XmlConvert.TrimString(s);
			if (s == "-INF")
			{
				return float.NegativeInfinity;
			}
			if (s == "INF")
			{
				return float.PositiveInfinity;
			}
			float num = float.Parse(s, NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint | NumberStyles.AllowExponent, NumberFormatInfo.InvariantInfo);
			if (num == 0f && s[0] == '-')
			{
				return -0f;
			}
			return num;
		}

		internal static Exception TryToSingle(string s, out float result)
		{
			s = XmlConvert.TrimString(s);
			if (s == "-INF")
			{
				result = float.NegativeInfinity;
				return null;
			}
			if (s == "INF")
			{
				result = float.PositiveInfinity;
				return null;
			}
			if (!float.TryParse(s, NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint | NumberStyles.AllowExponent, NumberFormatInfo.InvariantInfo, out result))
			{
				return new FormatException(Res.GetString("XmlConvert_BadFormat", new object[] { s, "Single" }));
			}
			if (result == 0f && s[0] == '-')
			{
				result = -0f;
			}
			return null;
		}

		public static double ToDouble(string s)
		{
			s = XmlConvert.TrimString(s);
			if (s == "-INF")
			{
				return double.NegativeInfinity;
			}
			if (s == "INF")
			{
				return double.PositiveInfinity;
			}
			double num = double.Parse(s, NumberStyles.Float, NumberFormatInfo.InvariantInfo);
			if (num == 0.0 && s[0] == '-')
			{
				return -0.0;
			}
			return num;
		}

		internal static Exception TryToDouble(string s, out double result)
		{
			s = XmlConvert.TrimString(s);
			if (s == "-INF")
			{
				result = double.NegativeInfinity;
				return null;
			}
			if (s == "INF")
			{
				result = double.PositiveInfinity;
				return null;
			}
			if (!double.TryParse(s, NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint | NumberStyles.AllowExponent, NumberFormatInfo.InvariantInfo, out result))
			{
				return new FormatException(Res.GetString("XmlConvert_BadFormat", new object[] { s, "Double" }));
			}
			if (result == 0.0 && s[0] == '-')
			{
				result = -0.0;
			}
			return null;
		}

		internal static double ToXPathDouble(object o)
		{
			string text = o as string;
			if (text != null)
			{
				text = XmlConvert.TrimString(text);
				double num;
				if (text.Length != 0 && text[0] != '+' && double.TryParse(text, NumberStyles.AllowTrailingWhite | NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint, NumberFormatInfo.InvariantInfo, out num))
				{
					return num;
				}
				return double.NaN;
			}
			else
			{
				if (o is double)
				{
					return (double)o;
				}
				if (!(o is bool))
				{
					try
					{
						return Convert.ToDouble(o, NumberFormatInfo.InvariantInfo);
					}
					catch (FormatException)
					{
					}
					catch (OverflowException)
					{
					}
					catch (ArgumentNullException)
					{
					}
					return double.NaN;
				}
				if (!(bool)o)
				{
					return 0.0;
				}
				return 1.0;
			}
		}

		internal static string ToXPathString(object value)
		{
			string text = value as string;
			if (text != null)
			{
				return text;
			}
			if (value is double)
			{
				return ((double)value).ToString("R", NumberFormatInfo.InvariantInfo);
			}
			if (!(value is bool))
			{
				return Convert.ToString(value, NumberFormatInfo.InvariantInfo);
			}
			if (!(bool)value)
			{
				return "false";
			}
			return "true";
		}

		internal static double XPathRound(double value)
		{
			double num = Math.Round(value);
			if (value - num != 0.5)
			{
				return num;
			}
			return num + 1.0;
		}

		public static TimeSpan ToTimeSpan(string s)
		{
			XsdDuration xsdDuration;
			try
			{
				xsdDuration = new XsdDuration(s);
			}
			catch (Exception)
			{
				throw new FormatException(Res.GetString("XmlConvert_BadFormat", new object[] { s, "TimeSpan" }));
			}
			return xsdDuration.ToTimeSpan();
		}

		internal static Exception TryToTimeSpan(string s, out TimeSpan result)
		{
			XsdDuration xsdDuration;
			Exception ex = XsdDuration.TryParse(s, out xsdDuration);
			if (ex != null)
			{
				result = TimeSpan.MinValue;
				return ex;
			}
			return xsdDuration.TryToTimeSpan(out result);
		}

		private static string[] AllDateTimeFormats
		{
			get
			{
				if (XmlConvert.s_allDateTimeFormats == null)
				{
					XmlConvert.CreateAllDateTimeFormats();
				}
				return XmlConvert.s_allDateTimeFormats;
			}
		}

		private static void CreateAllDateTimeFormats()
		{
			if (XmlConvert.s_allDateTimeFormats == null)
			{
				XmlConvert.s_allDateTimeFormats = new string[]
				{
					"yyyy-MM-ddTHH:mm:ss.FFFFFFFzzzzzz", "yyyy-MM-ddTHH:mm:ss.FFFFFFF", "yyyy-MM-ddTHH:mm:ss.FFFFFFFZ", "HH:mm:ss.FFFFFFF", "HH:mm:ss.FFFFFFFZ", "HH:mm:ss.FFFFFFFzzzzzz", "yyyy-MM-dd", "yyyy-MM-ddZ", "yyyy-MM-ddzzzzzz", "yyyy-MM",
					"yyyy-MMZ", "yyyy-MMzzzzzz", "yyyy", "yyyyZ", "yyyyzzzzzz", "--MM-dd", "--MM-ddZ", "--MM-ddzzzzzz", "---dd", "---ddZ",
					"---ddzzzzzz", "--MM--", "--MM--Z", "--MM--zzzzzz"
				};
			}
		}

		[Obsolete("Use XmlConvert.ToDateTime() that takes in XmlDateTimeSerializationMode")]
		public static DateTime ToDateTime(string s)
		{
			return XmlConvert.ToDateTime(s, XmlConvert.AllDateTimeFormats);
		}

		public static DateTime ToDateTime(string s, string format)
		{
			return DateTime.ParseExact(s, format, DateTimeFormatInfo.InvariantInfo, DateTimeStyles.AllowLeadingWhite | DateTimeStyles.AllowTrailingWhite);
		}

		public static DateTime ToDateTime(string s, string[] formats)
		{
			return DateTime.ParseExact(s, formats, DateTimeFormatInfo.InvariantInfo, DateTimeStyles.AllowLeadingWhite | DateTimeStyles.AllowTrailingWhite);
		}

		public static DateTime ToDateTime(string s, XmlDateTimeSerializationMode dateTimeOption)
		{
			XsdDateTime xsdDateTime = new XsdDateTime(s, XsdDateTimeFlags.AllXsd);
			DateTime dateTime = xsdDateTime;
			switch (dateTimeOption)
			{
			case XmlDateTimeSerializationMode.Local:
				dateTime = XmlConvert.SwitchToLocalTime(dateTime);
				break;
			case XmlDateTimeSerializationMode.Utc:
				dateTime = XmlConvert.SwitchToUtcTime(dateTime);
				break;
			case XmlDateTimeSerializationMode.Unspecified:
				dateTime = new DateTime(dateTime.Ticks, DateTimeKind.Unspecified);
				break;
			case XmlDateTimeSerializationMode.RoundtripKind:
				break;
			default:
				throw new ArgumentException(Res.GetString("Sch_InvalidDateTimeOption", new object[] { dateTimeOption }));
			}
			return dateTime;
		}

		public static DateTimeOffset ToDateTimeOffset(string s)
		{
			if (s == null)
			{
				throw new ArgumentNullException("s");
			}
			XsdDateTime xsdDateTime = new XsdDateTime(s, XsdDateTimeFlags.AllXsd);
			return xsdDateTime;
		}

		public static DateTimeOffset ToDateTimeOffset(string s, string format)
		{
			if (s == null)
			{
				throw new ArgumentNullException("s");
			}
			return DateTimeOffset.ParseExact(s, format, DateTimeFormatInfo.InvariantInfo, DateTimeStyles.AllowLeadingWhite | DateTimeStyles.AllowTrailingWhite);
		}

		public static DateTimeOffset ToDateTimeOffset(string s, string[] formats)
		{
			if (s == null)
			{
				throw new ArgumentNullException("s");
			}
			return DateTimeOffset.ParseExact(s, formats, DateTimeFormatInfo.InvariantInfo, DateTimeStyles.AllowLeadingWhite | DateTimeStyles.AllowTrailingWhite);
		}

		public static Guid ToGuid(string s)
		{
			return new Guid(s);
		}

		internal static Exception TryToGuid(string s, out Guid result)
		{
			Exception ex = null;
			result = Guid.Empty;
			try
			{
				result = new Guid(s);
			}
			catch (ArgumentException)
			{
				ex = new FormatException(Res.GetString("XmlConvert_BadFormat", new object[] { s, "Guid" }));
			}
			catch (FormatException)
			{
				ex = new FormatException(Res.GetString("XmlConvert_BadFormat", new object[] { s, "Guid" }));
			}
			return ex;
		}

		private static DateTime SwitchToLocalTime(DateTime value)
		{
			switch (value.Kind)
			{
			case DateTimeKind.Unspecified:
				return new DateTime(value.Ticks, DateTimeKind.Local);
			case DateTimeKind.Utc:
				return value.ToLocalTime();
			case DateTimeKind.Local:
				return value;
			default:
				return value;
			}
		}

		private static DateTime SwitchToUtcTime(DateTime value)
		{
			switch (value.Kind)
			{
			case DateTimeKind.Unspecified:
				return new DateTime(value.Ticks, DateTimeKind.Utc);
			case DateTimeKind.Utc:
				return value;
			case DateTimeKind.Local:
				return value.ToUniversalTime();
			default:
				return value;
			}
		}

		internal static Uri ToUri(string s)
		{
			if (s != null && s.Length > 0)
			{
				s = XmlConvert.TrimString(s);
				if (s.Length == 0 || s.IndexOf("##", StringComparison.Ordinal) != -1)
				{
					throw new FormatException(Res.GetString("XmlConvert_BadFormat", new object[] { s, "Uri" }));
				}
			}
			Uri uri;
			if (!Uri.TryCreate(s, UriKind.RelativeOrAbsolute, out uri))
			{
				throw new FormatException(Res.GetString("XmlConvert_BadFormat", new object[] { s, "Uri" }));
			}
			return uri;
		}

		internal static Exception TryToUri(string s, out Uri result)
		{
			result = null;
			if (s != null && s.Length > 0)
			{
				s = XmlConvert.TrimString(s);
				if (s.Length == 0 || s.IndexOf("##", StringComparison.Ordinal) != -1)
				{
					return new FormatException(Res.GetString("XmlConvert_BadFormat", new object[] { s, "Uri" }));
				}
			}
			if (!Uri.TryCreate(s, UriKind.RelativeOrAbsolute, out result))
			{
				return new FormatException(Res.GetString("XmlConvert_BadFormat", new object[] { s, "Uri" }));
			}
			return null;
		}

		internal static bool StrEqual(char[] chars, int strPos1, int strLen1, string str2)
		{
			if (strLen1 != str2.Length)
			{
				return false;
			}
			int num = 0;
			while (num < strLen1 && chars[strPos1 + num] == str2[num])
			{
				num++;
			}
			return num == strLen1;
		}

		internal static string TrimString(string value)
		{
			return value.Trim(XmlConvert.WhitespaceChars);
		}

		internal static string[] SplitString(string value)
		{
			return value.Split(XmlConvert.WhitespaceChars, StringSplitOptions.RemoveEmptyEntries);
		}

		internal static bool IsNegativeZero(double value)
		{
			return value == 0.0 && BitConverter.DoubleToInt64Bits(value) == BitConverter.DoubleToInt64Bits(-0.0);
		}

		internal unsafe static void VerifyCharData(string data, ExceptionType exceptionType)
		{
			if (data == null || data.Length == 0)
			{
				return;
			}
			XmlCharType instance = XmlCharType.Instance;
			int num = 0;
			int length = data.Length;
			for (;;)
			{
				if (num >= length || (instance.charProperties[data[num]] & 16) == 0)
				{
					if (num == length)
					{
						break;
					}
					char c = data[num];
					if (c < '\ud800' || c > '\udbff')
					{
						goto IL_00A0;
					}
					if (num + 1 == length)
					{
						goto Block_6;
					}
					c = data[num + 1];
					if (c < '\udc00' || c > '\udfff')
					{
						goto IL_0089;
					}
					num += 2;
				}
				else
				{
					num++;
				}
			}
			return;
			Block_6:
			throw XmlConvert.CreateException("Xml_InvalidSurrogateMissingLowChar", exceptionType);
			IL_0089:
			throw XmlConvert.CreateInvalidSurrogatePairException(data[num + 1], data[num], exceptionType);
			IL_00A0:
			throw XmlConvert.CreateInvalidCharException(data[num]);
		}

		internal unsafe static void VerifyCharData(char[] data, int offset, int len, ExceptionType exceptionType)
		{
			if (data == null || len == 0)
			{
				return;
			}
			XmlCharType instance = XmlCharType.Instance;
			int num = offset;
			int num2 = offset + len;
			for (;;)
			{
				if (num >= num2 || (instance.charProperties[data[num]] & 16) == 0)
				{
					if (num == num2)
					{
						break;
					}
					char c = data[num];
					if (c < '\ud800' || c > '\udbff')
					{
						goto IL_0084;
					}
					if (num + 1 == num2)
					{
						goto Block_6;
					}
					c = data[num + 1];
					if (c < '\udc00' || c > '\udfff')
					{
						goto IL_0075;
					}
					num += 2;
				}
				else
				{
					num++;
				}
			}
			return;
			Block_6:
			throw XmlConvert.CreateException("Xml_InvalidSurrogateMissingLowChar", exceptionType);
			IL_0075:
			throw XmlConvert.CreateInvalidSurrogatePairException(data[num + 1], data[num], exceptionType);
			IL_0084:
			throw XmlConvert.CreateInvalidCharException(data[num]);
		}

		internal static string EscapeValueForDebuggerDisplay(string value)
		{
			StringBuilder stringBuilder = null;
			int i = 0;
			int num = 0;
			while (i < value.Length)
			{
				char c = value[i];
				if (c < ' ' || c == '"')
				{
					if (stringBuilder == null)
					{
						stringBuilder = new StringBuilder(value.Length + 4);
					}
					if (i - num > 0)
					{
						stringBuilder.Append(value, num, i - num);
					}
					num = i + 1;
					char c2 = c;
					switch (c2)
					{
					case '\t':
						stringBuilder.Append("\\t");
						goto IL_00AE;
					case '\n':
						stringBuilder.Append("\\n");
						goto IL_00AE;
					case '\v':
					case '\f':
						break;
					case '\r':
						stringBuilder.Append("\\r");
						goto IL_00AE;
					default:
						if (c2 == '"')
						{
							stringBuilder.Append("\\\"");
							goto IL_00AE;
						}
						break;
					}
					stringBuilder.Append(c);
				}
				IL_00AE:
				i++;
			}
			if (stringBuilder == null)
			{
				return value;
			}
			if (i - num > 0)
			{
				stringBuilder.Append(value, num, i - num);
			}
			return stringBuilder.ToString();
		}

		internal static Exception CreateException(string res, ExceptionType exceptionType)
		{
			switch (exceptionType)
			{
			case ExceptionType.ArgumentException:
				return new ArgumentException(Res.GetString(res));
			}
			return new XmlException(res, string.Empty);
		}

		internal static Exception CreateException(string res, string arg, ExceptionType exceptionType)
		{
			switch (exceptionType)
			{
			case ExceptionType.ArgumentException:
				return new ArgumentException(Res.GetString(res, new object[] { arg }));
			}
			return new XmlException(res, arg);
		}

		internal static Exception CreateException(string res, string[] args, ExceptionType exceptionType)
		{
			switch (exceptionType)
			{
			case ExceptionType.ArgumentException:
				return new ArgumentException(Res.GetString(res, args));
			}
			return new XmlException(res, args);
		}

		internal static Exception CreateInvalidSurrogatePairException(char low, char hi)
		{
			return XmlConvert.CreateInvalidSurrogatePairException(low, hi, ExceptionType.ArgumentException);
		}

		internal static Exception CreateInvalidSurrogatePairException(char low, char hi, ExceptionType exceptionType)
		{
			string[] array = new string[2];
			string[] array2 = array;
			int num = 0;
			uint num2 = (uint)hi;
			array2[num] = num2.ToString("X", CultureInfo.InvariantCulture);
			string[] array3 = array;
			int num3 = 1;
			uint num4 = (uint)low;
			array3[num3] = num4.ToString("X", CultureInfo.InvariantCulture);
			string[] array4 = array;
			return XmlConvert.CreateException("Xml_InvalidSurrogatePairWithArgs", array4, exceptionType);
		}

		internal static Exception CreateInvalidHighSurrogateCharException(char hi)
		{
			return XmlConvert.CreateInvalidHighSurrogateCharException(hi, ExceptionType.ArgumentException);
		}

		internal static Exception CreateInvalidHighSurrogateCharException(char hi, ExceptionType exceptionType)
		{
			string text = "Xml_InvalidSurrogateHighChar";
			uint num = (uint)hi;
			return XmlConvert.CreateException(text, num.ToString("X", CultureInfo.InvariantCulture), exceptionType);
		}

		internal static Exception CreateInvalidCharException(char invChar)
		{
			return XmlConvert.CreateInvalidCharException(invChar, ExceptionType.ArgumentException);
		}

		internal static Exception CreateInvalidCharException(char invChar, ExceptionType exceptionType)
		{
			return XmlConvert.CreateException("Xml_InvalidCharacter", XmlException.BuildCharExceptionStr(invChar), exceptionType);
		}

		internal static ArgumentException CreateInvalidNameArgumentException(string name, string argumentName)
		{
			if (name != null)
			{
				return new ArgumentException(Res.GetString("Xml_EmptyName"), argumentName);
			}
			return new ArgumentNullException(argumentName);
		}

		internal const int SurHighStart = 55296;

		internal const int SurHighEnd = 56319;

		internal const int SurLowStart = 56320;

		internal const int SurLowEnd = 57343;

		internal const int SurMask = 64512;

		internal static char[] crt = new char[] { '\n', '\r', '\t' };

		private static readonly int c_EncodedCharLength = 7;

		private static Regex c_EncodeCharPattern;

		private static Regex c_DecodeCharPattern;

		private static string[] s_allDateTimeFormats;

		internal static readonly char[] WhitespaceChars = new char[] { ' ', '\t', '\n', '\r' };
	}
}
