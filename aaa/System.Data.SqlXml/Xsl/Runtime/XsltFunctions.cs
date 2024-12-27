using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text;
using System.Xml.Schema;
using System.Xml.XPath;

namespace System.Xml.Xsl.Runtime
{
	// Token: 0x020000CC RID: 204
	[EditorBrowsable(EditorBrowsableState.Never)]
	public static class XsltFunctions
	{
		// Token: 0x06000998 RID: 2456 RVA: 0x0002D510 File Offset: 0x0002C510
		public static bool StartsWith(string s1, string s2)
		{
			return s1.Length >= s2.Length && string.CompareOrdinal(s1, 0, s2, 0, s2.Length) == 0;
		}

		// Token: 0x06000999 RID: 2457 RVA: 0x0002D534 File Offset: 0x0002C534
		public static bool Contains(string s1, string s2)
		{
			return XsltFunctions.compareInfo.IndexOf(s1, s2, CompareOptions.Ordinal) >= 0;
		}

		// Token: 0x0600099A RID: 2458 RVA: 0x0002D550 File Offset: 0x0002C550
		public static string SubstringBefore(string s1, string s2)
		{
			if (s2.Length == 0)
			{
				return s2;
			}
			int num = XsltFunctions.compareInfo.IndexOf(s1, s2, CompareOptions.Ordinal);
			if (num >= 1)
			{
				return s1.Substring(0, num);
			}
			return string.Empty;
		}

		// Token: 0x0600099B RID: 2459 RVA: 0x0002D58C File Offset: 0x0002C58C
		public static string SubstringAfter(string s1, string s2)
		{
			if (s2.Length == 0)
			{
				return s1;
			}
			int num = XsltFunctions.compareInfo.IndexOf(s1, s2, CompareOptions.Ordinal);
			if (num >= 0)
			{
				return s1.Substring(num + s2.Length);
			}
			return string.Empty;
		}

		// Token: 0x0600099C RID: 2460 RVA: 0x0002D5CD File Offset: 0x0002C5CD
		public static string Substring(string value, double startIndex)
		{
			startIndex = XsltFunctions.Round(startIndex);
			if (startIndex <= 0.0)
			{
				return value;
			}
			if (startIndex <= (double)value.Length)
			{
				return value.Substring((int)startIndex - 1);
			}
			return string.Empty;
		}

		// Token: 0x0600099D RID: 2461 RVA: 0x0002D600 File Offset: 0x0002C600
		public static string Substring(string value, double startIndex, double length)
		{
			startIndex = XsltFunctions.Round(startIndex) - 1.0;
			if (startIndex >= (double)value.Length)
			{
				return string.Empty;
			}
			double num = startIndex + XsltFunctions.Round(length);
			startIndex = ((startIndex <= 0.0) ? 0.0 : startIndex);
			if (startIndex < num)
			{
				if (num > (double)value.Length)
				{
					num = (double)value.Length;
				}
				return value.Substring((int)startIndex, (int)(num - startIndex));
			}
			return string.Empty;
		}

		// Token: 0x0600099E RID: 2462 RVA: 0x0002D67C File Offset: 0x0002C67C
		public static string NormalizeSpace(string value)
		{
			XmlCharType instance = XmlCharType.Instance;
			StringBuilder stringBuilder = null;
			int num = 0;
			int num2 = 0;
			int i;
			for (i = 0; i < value.Length; i++)
			{
				if (instance.IsWhiteSpace(value[i]))
				{
					if (i == num)
					{
						num++;
					}
					else if (value[i] != ' ' || num2 == i)
					{
						if (stringBuilder == null)
						{
							stringBuilder = new StringBuilder(value.Length);
						}
						else
						{
							stringBuilder.Append(' ');
						}
						if (num2 == i)
						{
							stringBuilder.Append(value, num, i - num - 1);
						}
						else
						{
							stringBuilder.Append(value, num, i - num);
						}
						num = i + 1;
					}
					else
					{
						num2 = i + 1;
					}
				}
			}
			if (stringBuilder == null)
			{
				if (num == i)
				{
					return string.Empty;
				}
				if (num == 0 && num2 != i)
				{
					return value;
				}
				stringBuilder = new StringBuilder(value.Length);
			}
			else if (i != num)
			{
				stringBuilder.Append(' ');
			}
			if (num2 == i)
			{
				stringBuilder.Append(value, num, i - num - 1);
			}
			else
			{
				stringBuilder.Append(value, num, i - num);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0600099F RID: 2463 RVA: 0x0002D770 File Offset: 0x0002C770
		public static string Translate(string arg, string mapString, string transString)
		{
			if (mapString.Length == 0)
			{
				return arg;
			}
			StringBuilder stringBuilder = new StringBuilder(arg.Length);
			for (int i = 0; i < arg.Length; i++)
			{
				int num = mapString.IndexOf(arg[i]);
				if (num < 0)
				{
					stringBuilder.Append(arg[i]);
				}
				else if (num < transString.Length)
				{
					stringBuilder.Append(transString[num]);
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x060009A0 RID: 2464 RVA: 0x0002D7E4 File Offset: 0x0002C7E4
		public static bool Lang(string value, XPathNavigator context)
		{
			string xmlLang = context.XmlLang;
			return xmlLang.StartsWith(value, StringComparison.OrdinalIgnoreCase) && (xmlLang.Length == value.Length || xmlLang[value.Length] == '-');
		}

		// Token: 0x060009A1 RID: 2465 RVA: 0x0002D824 File Offset: 0x0002C824
		public static double Round(double value)
		{
			double num = Math.Round(value);
			if (value - num != 0.5)
			{
				return num;
			}
			return num + 1.0;
		}

		// Token: 0x060009A2 RID: 2466 RVA: 0x0002D854 File Offset: 0x0002C854
		public static XPathItem SystemProperty(XmlQualifiedName name)
		{
			if (name.Namespace == "http://www.w3.org/1999/XSL/Transform")
			{
				string name2;
				if ((name2 = name.Name) != null)
				{
					if (name2 == "version")
					{
						return new XmlAtomicValue(XmlSchemaType.GetBuiltInSimpleType(XmlTypeCode.Double), 1.0);
					}
					if (name2 == "vendor")
					{
						return new XmlAtomicValue(XmlSchemaType.GetBuiltInSimpleType(XmlTypeCode.String), "Microsoft");
					}
					if (name2 == "vendor-url")
					{
						return new XmlAtomicValue(XmlSchemaType.GetBuiltInSimpleType(XmlTypeCode.String), "http://www.microsoft.com");
					}
				}
			}
			else if (name.Namespace == "urn:schemas-microsoft-com:xslt" && name.Name == "version")
			{
				return new XmlAtomicValue(XmlSchemaType.GetBuiltInSimpleType(XmlTypeCode.String), typeof(XsltLibrary).Assembly.ImageRuntimeVersion);
			}
			return new XmlAtomicValue(XmlSchemaType.GetBuiltInSimpleType(XmlTypeCode.String), string.Empty);
		}

		// Token: 0x060009A3 RID: 2467 RVA: 0x0002D939 File Offset: 0x0002C939
		public static string BaseUri(XPathNavigator navigator)
		{
			return navigator.BaseURI;
		}

		// Token: 0x060009A4 RID: 2468 RVA: 0x0002D944 File Offset: 0x0002C944
		public static string OuterXml(XPathNavigator navigator)
		{
			RtfNavigator rtfNavigator = navigator as RtfNavigator;
			if (rtfNavigator == null)
			{
				return navigator.OuterXml;
			}
			StringBuilder stringBuilder = new StringBuilder();
			XmlWriter xmlWriter = XmlWriter.Create(stringBuilder, new XmlWriterSettings
			{
				OmitXmlDeclaration = true,
				ConformanceLevel = ConformanceLevel.Fragment,
				CheckCharacters = false
			});
			rtfNavigator.CopyToWriter(xmlWriter);
			xmlWriter.Close();
			return stringBuilder.ToString();
		}

		// Token: 0x060009A5 RID: 2469 RVA: 0x0002D9A0 File Offset: 0x0002C9A0
		public static string EXslObjectType(IList<XPathItem> value)
		{
			if (value.Count != 1)
			{
				return "node-set";
			}
			XPathItem xpathItem = value[0];
			if (xpathItem is RtfNavigator)
			{
				return "RTF";
			}
			if (xpathItem.IsNode)
			{
				return "node-set";
			}
			object typedValue = xpathItem.TypedValue;
			if (typedValue is string)
			{
				return "string";
			}
			if (typedValue is double)
			{
				return "number";
			}
			if (typedValue is bool)
			{
				return "boolean";
			}
			return "external";
		}

		// Token: 0x060009A6 RID: 2470 RVA: 0x0002DA18 File Offset: 0x0002CA18
		public static double MSNumber(IList<XPathItem> value)
		{
			if (value.Count == 0)
			{
				return double.NaN;
			}
			XPathItem xpathItem = value[0];
			string text;
			if (xpathItem.IsNode)
			{
				text = xpathItem.Value;
			}
			else
			{
				Type valueType = xpathItem.ValueType;
				if (valueType == XsltConvert.StringType)
				{
					text = xpathItem.Value;
				}
				else
				{
					if (valueType == XsltConvert.DoubleType)
					{
						return xpathItem.ValueAsDouble;
					}
					if (!xpathItem.ValueAsBoolean)
					{
						return 0.0;
					}
					return 1.0;
				}
			}
			double naN;
			if (XmlConvert.TryToDouble(text, out naN) != null)
			{
				naN = double.NaN;
			}
			return naN;
		}

		// Token: 0x060009A7 RID: 2471
		[DllImport("kernel32.dll", CharSet = CharSet.Auto)]
		private static extern int GetDateFormat(int locale, uint dwFlags, ref XsltFunctions.SystemTime sysTime, string format, StringBuilder sb, int sbSize);

		// Token: 0x060009A8 RID: 2472
		[DllImport("kernel32.dll", CharSet = CharSet.Auto)]
		private static extern int GetTimeFormat(int locale, uint dwFlags, ref XsltFunctions.SystemTime sysTime, string format, StringBuilder sb, int sbSize);

		// Token: 0x060009A9 RID: 2473 RVA: 0x0002DAA8 File Offset: 0x0002CAA8
		public static string MSFormatDateTime(string dateTime, string format, string lang, bool isDate)
		{
			string text;
			try
			{
				int lcid = XsltFunctions.GetCultureInfo(lang).LCID;
				XsdDateTime xsdDateTime;
				if (!XsdDateTime.TryParse(dateTime, XsdDateTimeFlags.DateTime | XsdDateTimeFlags.Time | XsdDateTimeFlags.Date | XsdDateTimeFlags.GYearMonth | XsdDateTimeFlags.GYear | XsdDateTimeFlags.GMonthDay | XsdDateTimeFlags.GDay | XsdDateTimeFlags.GMonth | XsdDateTimeFlags.XdrDateTime | XsdDateTimeFlags.XdrTimeNoTz, out xsdDateTime))
				{
					text = string.Empty;
				}
				else
				{
					XsltFunctions.SystemTime systemTime = new XsltFunctions.SystemTime(xsdDateTime.ToZulu());
					StringBuilder stringBuilder = new StringBuilder(format.Length + 16);
					if (format.Length == 0)
					{
						format = null;
					}
					if (isDate)
					{
						if (XsltFunctions.GetDateFormat(lcid, 0U, ref systemTime, format, stringBuilder, stringBuilder.Capacity) == 0)
						{
							int num = XsltFunctions.GetDateFormat(lcid, 0U, ref systemTime, format, stringBuilder, 0);
							if (num != 0)
							{
								stringBuilder = new StringBuilder(num);
								num = XsltFunctions.GetDateFormat(lcid, 0U, ref systemTime, format, stringBuilder, stringBuilder.Capacity);
							}
						}
					}
					else if (XsltFunctions.GetTimeFormat(lcid, 0U, ref systemTime, format, stringBuilder, stringBuilder.Capacity) == 0)
					{
						int num2 = XsltFunctions.GetTimeFormat(lcid, 0U, ref systemTime, format, stringBuilder, 0);
						if (num2 != 0)
						{
							stringBuilder = new StringBuilder(num2);
							num2 = XsltFunctions.GetTimeFormat(lcid, 0U, ref systemTime, format, stringBuilder, stringBuilder.Capacity);
						}
					}
					text = stringBuilder.ToString();
				}
			}
			catch (ArgumentException)
			{
				text = string.Empty;
			}
			return text;
		}

		// Token: 0x060009AA RID: 2474 RVA: 0x0002DBBC File Offset: 0x0002CBBC
		public static double MSStringCompare(string s1, string s2, string lang, string options)
		{
			CultureInfo cultureInfo = XsltFunctions.GetCultureInfo(lang);
			CompareOptions compareOptions = CompareOptions.None;
			bool flag = false;
			foreach (char c in options)
			{
				if (c != 'i')
				{
					if (c != 'u')
					{
						flag = true;
						compareOptions = CompareOptions.IgnoreCase;
					}
					else
					{
						flag = true;
					}
				}
				else
				{
					compareOptions = CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth;
				}
			}
			if (flag)
			{
				if (compareOptions != CompareOptions.None)
				{
					throw new XslTransformException("Xslt_InvalidCompareOption", new string[] { options });
				}
				compareOptions = CompareOptions.IgnoreCase;
			}
			int num = cultureInfo.CompareInfo.Compare(s1, s2, compareOptions);
			if (flag && num == 0)
			{
				num = -cultureInfo.CompareInfo.Compare(s1, s2, CompareOptions.None);
			}
			return (double)num;
		}

		// Token: 0x060009AB RID: 2475 RVA: 0x0002DC58 File Offset: 0x0002CC58
		public static string MSUtc(string dateTime)
		{
			XsdDateTime xsdDateTime;
			DateTime dateTime2;
			try
			{
				if (!XsdDateTime.TryParse(dateTime, XsdDateTimeFlags.DateTime | XsdDateTimeFlags.Time | XsdDateTimeFlags.Date | XsdDateTimeFlags.GYearMonth | XsdDateTimeFlags.GYear | XsdDateTimeFlags.GMonthDay | XsdDateTimeFlags.GDay | XsdDateTimeFlags.GMonth | XsdDateTimeFlags.XdrDateTime | XsdDateTimeFlags.XdrTimeNoTz, out xsdDateTime))
				{
					return string.Empty;
				}
				dateTime2 = xsdDateTime.ToZulu();
			}
			catch (ArgumentException)
			{
				return string.Empty;
			}
			char[] array = "----------T00:00:00.000".ToCharArray();
			switch (xsdDateTime.TypeCode)
			{
			case XmlTypeCode.DateTime:
				XsltFunctions.PrintDate(array, dateTime2);
				XsltFunctions.PrintTime(array, dateTime2);
				break;
			case XmlTypeCode.Time:
				XsltFunctions.PrintTime(array, dateTime2);
				break;
			case XmlTypeCode.Date:
				XsltFunctions.PrintDate(array, dateTime2);
				break;
			case XmlTypeCode.GYearMonth:
				XsltFunctions.PrintYear(array, dateTime2.Year);
				XsltFunctions.ShortToCharArray(array, 5, dateTime2.Month);
				break;
			case XmlTypeCode.GYear:
				XsltFunctions.PrintYear(array, dateTime2.Year);
				break;
			case XmlTypeCode.GMonthDay:
				XsltFunctions.ShortToCharArray(array, 5, dateTime2.Month);
				XsltFunctions.ShortToCharArray(array, 8, dateTime2.Day);
				break;
			case XmlTypeCode.GDay:
				XsltFunctions.ShortToCharArray(array, 8, dateTime2.Day);
				break;
			case XmlTypeCode.GMonth:
				XsltFunctions.ShortToCharArray(array, 5, dateTime2.Month);
				break;
			}
			return new string(array);
		}

		// Token: 0x060009AC RID: 2476 RVA: 0x0002DD7C File Offset: 0x0002CD7C
		public static string MSLocalName(string name)
		{
			int num2;
			int num = ValidateNames.ParseQName(name, 0, out num2);
			if (num != name.Length)
			{
				return string.Empty;
			}
			if (num2 == 0)
			{
				return name;
			}
			return name.Substring(num2 + 1);
		}

		// Token: 0x060009AD RID: 2477 RVA: 0x0002DDB0 File Offset: 0x0002CDB0
		public static string MSNamespaceUri(string name, XPathNavigator currentNode)
		{
			int num2;
			int num = ValidateNames.ParseQName(name, 0, out num2);
			if (num != name.Length)
			{
				return string.Empty;
			}
			string text = name.Substring(0, num2);
			if (text == "xmlns")
			{
				return string.Empty;
			}
			string text2 = currentNode.LookupNamespace(text);
			if (text2 != null)
			{
				return text2;
			}
			if (text == "xml")
			{
				return "http://www.w3.org/XML/1998/namespace";
			}
			return string.Empty;
		}

		// Token: 0x060009AE RID: 2478 RVA: 0x0002DE18 File Offset: 0x0002CE18
		private static CultureInfo GetCultureInfo(string lang)
		{
			if (lang.Length == 0)
			{
				return CultureInfo.CurrentCulture;
			}
			CultureInfo cultureInfo;
			try
			{
				cultureInfo = new CultureInfo(lang);
			}
			catch (ArgumentException)
			{
				throw new XslTransformException("Xslt_InvalidLanguage", new string[] { lang });
			}
			return cultureInfo;
		}

		// Token: 0x060009AF RID: 2479 RVA: 0x0002DE68 File Offset: 0x0002CE68
		private static void PrintDate(char[] text, DateTime dt)
		{
			XsltFunctions.PrintYear(text, dt.Year);
			XsltFunctions.ShortToCharArray(text, 5, dt.Month);
			XsltFunctions.ShortToCharArray(text, 8, dt.Day);
		}

		// Token: 0x060009B0 RID: 2480 RVA: 0x0002DE93 File Offset: 0x0002CE93
		private static void PrintTime(char[] text, DateTime dt)
		{
			XsltFunctions.ShortToCharArray(text, 11, dt.Hour);
			XsltFunctions.ShortToCharArray(text, 14, dt.Minute);
			XsltFunctions.ShortToCharArray(text, 17, dt.Second);
			XsltFunctions.PrintMsec(text, dt.Millisecond);
		}

		// Token: 0x060009B1 RID: 2481 RVA: 0x0002DECF File Offset: 0x0002CECF
		private static void PrintYear(char[] text, int value)
		{
			text[0] = (char)(value / 1000 % 10 + 48);
			text[1] = (char)(value / 100 % 10 + 48);
			text[2] = (char)(value / 10 % 10 + 48);
			text[3] = (char)(value / 1 % 10 + 48);
		}

		// Token: 0x060009B2 RID: 2482 RVA: 0x0002DF0B File Offset: 0x0002CF0B
		private static void PrintMsec(char[] text, int value)
		{
			if (value == 0)
			{
				return;
			}
			text[20] = (char)(value / 100 % 10 + 48);
			text[21] = (char)(value / 10 % 10 + 48);
			text[22] = (char)(value / 1 % 10 + 48);
		}

		// Token: 0x060009B3 RID: 2483 RVA: 0x0002DF3D File Offset: 0x0002CF3D
		private static void ShortToCharArray(char[] text, int start, int value)
		{
			text[start] = (char)(value / 10 + 48);
			text[start + 1] = (char)(value % 10 + 48);
		}

		// Token: 0x040005F7 RID: 1527
		private static readonly CompareInfo compareInfo = CultureInfo.InvariantCulture.CompareInfo;

		// Token: 0x020000CD RID: 205
		private struct SystemTime
		{
			// Token: 0x060009B5 RID: 2485 RVA: 0x0002DF68 File Offset: 0x0002CF68
			public SystemTime(DateTime dateTime)
			{
				this.Year = (ushort)dateTime.Year;
				this.Month = (ushort)dateTime.Month;
				this.DayOfWeek = (ushort)dateTime.DayOfWeek;
				this.Day = (ushort)dateTime.Day;
				this.Hour = (ushort)dateTime.Hour;
				this.Minute = (ushort)dateTime.Minute;
				this.Second = (ushort)dateTime.Second;
				this.Milliseconds = (ushort)dateTime.Millisecond;
			}

			// Token: 0x040005F8 RID: 1528
			[MarshalAs(UnmanagedType.U2)]
			public ushort Year;

			// Token: 0x040005F9 RID: 1529
			[MarshalAs(UnmanagedType.U2)]
			public ushort Month;

			// Token: 0x040005FA RID: 1530
			[MarshalAs(UnmanagedType.U2)]
			public ushort DayOfWeek;

			// Token: 0x040005FB RID: 1531
			[MarshalAs(UnmanagedType.U2)]
			public ushort Day;

			// Token: 0x040005FC RID: 1532
			[MarshalAs(UnmanagedType.U2)]
			public ushort Hour;

			// Token: 0x040005FD RID: 1533
			[MarshalAs(UnmanagedType.U2)]
			public ushort Minute;

			// Token: 0x040005FE RID: 1534
			[MarshalAs(UnmanagedType.U2)]
			public ushort Second;

			// Token: 0x040005FF RID: 1535
			[MarshalAs(UnmanagedType.U2)]
			public ushort Milliseconds;
		}
	}
}
