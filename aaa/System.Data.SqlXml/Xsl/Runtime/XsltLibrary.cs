using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Xml.XPath;
using System.Xml.Xsl.Xslt;

namespace System.Xml.Xsl.Runtime
{
	// Token: 0x020000CF RID: 207
	[EditorBrowsable(EditorBrowsableState.Never)]
	public sealed class XsltLibrary
	{
		// Token: 0x060009B9 RID: 2489 RVA: 0x0002E455 File Offset: 0x0002D455
		internal XsltLibrary(XmlQueryRuntime runtime)
		{
			this.runtime = runtime;
		}

		// Token: 0x060009BA RID: 2490 RVA: 0x0002E464 File Offset: 0x0002D464
		public string FormatMessage(string res, IList<string> args)
		{
			string[] array = new string[args.Count];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = args[i];
			}
			return XslTransformException.CreateMessage(res, array);
		}

		// Token: 0x060009BB RID: 2491 RVA: 0x0002E49C File Offset: 0x0002D49C
		public int CheckScriptNamespace(string nsUri)
		{
			if (this.runtime.ExternalContext.GetLateBoundObject(nsUri) != null)
			{
				throw new XslTransformException("Xslt_ScriptAndExtensionClash", new string[] { nsUri });
			}
			return 0;
		}

		// Token: 0x060009BC RID: 2492 RVA: 0x0002E4D4 File Offset: 0x0002D4D4
		public bool ElementAvailable(XmlQualifiedName name)
		{
			return QilGenerator.IsElementAvailable(name);
		}

		// Token: 0x060009BD RID: 2493 RVA: 0x0002E4DC File Offset: 0x0002D4DC
		public bool FunctionAvailable(XmlQualifiedName name)
		{
			if (this.functionsAvail == null)
			{
				this.functionsAvail = new HybridDictionary();
			}
			else
			{
				object obj = this.functionsAvail[name];
				if (obj != null)
				{
					return (bool)obj;
				}
			}
			bool flag = this.FunctionAvailableHelper(name);
			this.functionsAvail[name] = flag;
			return flag;
		}

		// Token: 0x060009BE RID: 2494 RVA: 0x0002E530 File Offset: 0x0002D530
		private bool FunctionAvailableHelper(XmlQualifiedName name)
		{
			return QilGenerator.IsFunctionAvailable(name.Name, name.Namespace) || (name.Namespace.Length != 0 && !(name.Namespace == "http://www.w3.org/1999/XSL/Transform") && (this.runtime.ExternalContext.LateBoundFunctionExists(name.Name, name.Namespace) || this.runtime.EarlyBoundFunctionExists(name.Name, name.Namespace)));
		}

		// Token: 0x060009BF RID: 2495 RVA: 0x0002E5AC File Offset: 0x0002D5AC
		public int RegisterDecimalFormat(XmlQualifiedName name, string infinitySymbol, string nanSymbol, string characters)
		{
			if (this.decimalFormats == null)
			{
				this.decimalFormats = new DecimalFormats();
			}
			DecimalFormatDecl decimalFormatDecl = new DecimalFormatDecl(name, infinitySymbol, nanSymbol, characters);
			this.decimalFormats.Add(decimalFormatDecl);
			return 0;
		}

		// Token: 0x060009C0 RID: 2496 RVA: 0x0002E5E4 File Offset: 0x0002D5E4
		private DecimalFormatter CreateDecimalFormatter(string formatPicture, string infinitySymbol, string nanSymbol, string characters)
		{
			NumberFormatInfo numberFormatInfo = new NumberFormatInfo();
			numberFormatInfo.NumberDecimalSeparator = char.ToString(characters[0]);
			numberFormatInfo.NumberGroupSeparator = char.ToString(characters[1]);
			numberFormatInfo.PositiveInfinitySymbol = infinitySymbol;
			numberFormatInfo.NegativeSign = char.ToString(characters[7]);
			numberFormatInfo.NaNSymbol = nanSymbol;
			numberFormatInfo.PercentSymbol = char.ToString(characters[2]);
			numberFormatInfo.PerMilleSymbol = char.ToString(characters[3]);
			numberFormatInfo.NegativeInfinitySymbol = numberFormatInfo.NegativeSign + numberFormatInfo.PositiveInfinitySymbol;
			DecimalFormat decimalFormat = new DecimalFormat(numberFormatInfo, characters[5], characters[4], characters[6]);
			return new DecimalFormatter(formatPicture, decimalFormat);
		}

		// Token: 0x060009C1 RID: 2497 RVA: 0x0002E6A1 File Offset: 0x0002D6A1
		public double RegisterDecimalFormatter(string formatPicture, string infinitySymbol, string nanSymbol, string characters)
		{
			if (this.decimalFormatters == null)
			{
				this.decimalFormatters = new List<DecimalFormatter>();
			}
			this.decimalFormatters.Add(this.CreateDecimalFormatter(formatPicture, infinitySymbol, nanSymbol, characters));
			return (double)(this.decimalFormatters.Count - 1);
		}

		// Token: 0x060009C2 RID: 2498 RVA: 0x0002E6DC File Offset: 0x0002D6DC
		public string FormatNumberStatic(double value, double decimalFormatterIndex)
		{
			int num = (int)decimalFormatterIndex;
			return this.decimalFormatters[num].Format(value);
		}

		// Token: 0x060009C3 RID: 2499 RVA: 0x0002E700 File Offset: 0x0002D700
		public string FormatNumberDynamic(double value, string formatPicture, XmlQualifiedName decimalFormatName, string errorMessageName)
		{
			DecimalFormatDecl decimalFormatDecl;
			if (this.decimalFormats != null && this.decimalFormats.Contains(decimalFormatName))
			{
				decimalFormatDecl = this.decimalFormats[decimalFormatName];
			}
			else
			{
				if (decimalFormatName != DecimalFormatDecl.Default.Name)
				{
					throw new XslTransformException("Xslt_NoDecimalFormat", new string[] { errorMessageName });
				}
				decimalFormatDecl = DecimalFormatDecl.Default;
			}
			DecimalFormatter decimalFormatter = this.CreateDecimalFormatter(formatPicture, decimalFormatDecl.InfinitySymbol, decimalFormatDecl.NanSymbol, new string(decimalFormatDecl.Characters));
			return decimalFormatter.Format(value);
		}

		// Token: 0x060009C4 RID: 2500 RVA: 0x0002E788 File Offset: 0x0002D788
		public string NumberFormat(IList<XPathItem> value, string formatString, double lang, string letterValue, string groupingSeparator, double groupingSize)
		{
			NumberFormatter numberFormatter = new NumberFormatter(formatString, (int)lang, letterValue, groupingSeparator, (int)groupingSize);
			return numberFormatter.FormatSequence(value);
		}

		// Token: 0x060009C5 RID: 2501 RVA: 0x0002E7AC File Offset: 0x0002D7AC
		public int LangToLcid(string lang, bool forwardCompatibility)
		{
			return XsltLibrary.LangToLcidInternal(lang, forwardCompatibility, null);
		}

		// Token: 0x060009C6 RID: 2502 RVA: 0x0002E7B8 File Offset: 0x0002D7B8
		internal static int LangToLcidInternal(string lang, bool forwardCompatibility, IErrorHelper errorHelper)
		{
			int num = 127;
			if (lang != null)
			{
				if (lang.Length == 0)
				{
					if (!forwardCompatibility)
					{
						if (errorHelper == null)
						{
							throw new XslTransformException("Xslt_InvalidAttrValue", new string[] { "lang", lang });
						}
						errorHelper.ReportError("Xslt_InvalidAttrValue", new string[] { "lang", lang });
					}
				}
				else
				{
					try
					{
						num = new CultureInfo(lang).LCID;
					}
					catch (ArgumentException)
					{
						if (!forwardCompatibility)
						{
							if (errorHelper == null)
							{
								throw new XslTransformException("Xslt_InvalidLanguage", new string[] { lang });
							}
							errorHelper.ReportError("Xslt_InvalidLanguage", new string[] { lang });
						}
					}
				}
			}
			return num;
		}

		// Token: 0x060009C7 RID: 2503 RVA: 0x0002E87C File Offset: 0x0002D87C
		private static TypeCode GetTypeCode(XPathItem item)
		{
			Type valueType = item.ValueType;
			if (valueType == XsltConvert.StringType)
			{
				return TypeCode.String;
			}
			if (valueType == XsltConvert.DoubleType)
			{
				return TypeCode.Double;
			}
			return TypeCode.Boolean;
		}

		// Token: 0x060009C8 RID: 2504 RVA: 0x0002E8A7 File Offset: 0x0002D8A7
		private static TypeCode WeakestTypeCode(TypeCode typeCode1, TypeCode typeCode2)
		{
			if (typeCode1 >= typeCode2)
			{
				return typeCode2;
			}
			return typeCode1;
		}

		// Token: 0x060009C9 RID: 2505 RVA: 0x0002E8B0 File Offset: 0x0002D8B0
		private static bool CompareNumbers(XsltLibrary.ComparisonOperator op, double left, double right)
		{
			switch (op)
			{
			case XsltLibrary.ComparisonOperator.Eq:
				return left == right;
			case XsltLibrary.ComparisonOperator.Ne:
				return left != right;
			case XsltLibrary.ComparisonOperator.Lt:
				return left < right;
			case XsltLibrary.ComparisonOperator.Le:
				return left <= right;
			case XsltLibrary.ComparisonOperator.Gt:
				return left > right;
			default:
				return left >= right;
			}
		}

		// Token: 0x060009CA RID: 2506 RVA: 0x0002E904 File Offset: 0x0002D904
		private static bool CompareValues(XsltLibrary.ComparisonOperator op, XPathItem left, XPathItem right, TypeCode compType)
		{
			if (compType == TypeCode.Double)
			{
				return XsltLibrary.CompareNumbers(op, XsltConvert.ToDouble(left), XsltConvert.ToDouble(right));
			}
			if (compType == TypeCode.String)
			{
				return XsltConvert.ToString(left) == XsltConvert.ToString(right) == (op == XsltLibrary.ComparisonOperator.Eq);
			}
			return XsltConvert.ToBoolean(left) == XsltConvert.ToBoolean(right) == (op == XsltLibrary.ComparisonOperator.Eq);
		}

		// Token: 0x060009CB RID: 2507 RVA: 0x0002E95C File Offset: 0x0002D95C
		private static bool CompareNodeSetAndValue(XsltLibrary.ComparisonOperator op, IList<XPathNavigator> nodeset, XPathItem val, TypeCode compType)
		{
			if (compType == TypeCode.Boolean)
			{
				return XsltLibrary.CompareNumbers(op, (double)((nodeset.Count != 0) ? 1 : 0), (double)(XsltConvert.ToBoolean(val) ? 1 : 0));
			}
			int count = nodeset.Count;
			for (int i = 0; i < count; i++)
			{
				if (XsltLibrary.CompareValues(op, nodeset[i], val, compType))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060009CC RID: 2508 RVA: 0x0002E9B8 File Offset: 0x0002D9B8
		private static bool CompareNodeSetAndNodeSet(XsltLibrary.ComparisonOperator op, IList<XPathNavigator> left, IList<XPathNavigator> right, TypeCode compType)
		{
			int count = left.Count;
			int count2 = right.Count;
			for (int i = 0; i < count; i++)
			{
				for (int j = 0; j < count2; j++)
				{
					if (XsltLibrary.CompareValues(op, left[i], right[j], compType))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x060009CD RID: 2509 RVA: 0x0002EA08 File Offset: 0x0002DA08
		public bool EqualityOperator(double opCode, IList<XPathItem> left, IList<XPathItem> right)
		{
			XsltLibrary.ComparisonOperator comparisonOperator = (XsltLibrary.ComparisonOperator)opCode;
			if (XsltLibrary.IsNodeSetOrRtf(left))
			{
				if (XsltLibrary.IsNodeSetOrRtf(right))
				{
					return XsltLibrary.CompareNodeSetAndNodeSet(comparisonOperator, XsltLibrary.ToNodeSetOrRtf(left), XsltLibrary.ToNodeSetOrRtf(right), TypeCode.String);
				}
				XPathItem xpathItem = right[0];
				return XsltLibrary.CompareNodeSetAndValue(comparisonOperator, XsltLibrary.ToNodeSetOrRtf(left), xpathItem, XsltLibrary.GetTypeCode(xpathItem));
			}
			else
			{
				if (XsltLibrary.IsNodeSetOrRtf(right))
				{
					XPathItem xpathItem2 = left[0];
					return XsltLibrary.CompareNodeSetAndValue(comparisonOperator, XsltLibrary.ToNodeSetOrRtf(right), xpathItem2, XsltLibrary.GetTypeCode(xpathItem2));
				}
				XPathItem xpathItem3 = left[0];
				XPathItem xpathItem4 = right[0];
				return XsltLibrary.CompareValues(comparisonOperator, xpathItem3, xpathItem4, XsltLibrary.WeakestTypeCode(XsltLibrary.GetTypeCode(xpathItem3), XsltLibrary.GetTypeCode(xpathItem4)));
			}
		}

		// Token: 0x060009CE RID: 2510 RVA: 0x0002EAAC File Offset: 0x0002DAAC
		private static XsltLibrary.ComparisonOperator InvertOperator(XsltLibrary.ComparisonOperator op)
		{
			switch (op)
			{
			case XsltLibrary.ComparisonOperator.Lt:
				return XsltLibrary.ComparisonOperator.Gt;
			case XsltLibrary.ComparisonOperator.Le:
				return XsltLibrary.ComparisonOperator.Ge;
			case XsltLibrary.ComparisonOperator.Gt:
				return XsltLibrary.ComparisonOperator.Lt;
			case XsltLibrary.ComparisonOperator.Ge:
				return XsltLibrary.ComparisonOperator.Le;
			default:
				return op;
			}
		}

		// Token: 0x060009CF RID: 2511 RVA: 0x0002EAE0 File Offset: 0x0002DAE0
		public bool RelationalOperator(double opCode, IList<XPathItem> left, IList<XPathItem> right)
		{
			XsltLibrary.ComparisonOperator comparisonOperator = (XsltLibrary.ComparisonOperator)opCode;
			if (XsltLibrary.IsNodeSetOrRtf(left))
			{
				if (XsltLibrary.IsNodeSetOrRtf(right))
				{
					return XsltLibrary.CompareNodeSetAndNodeSet(comparisonOperator, XsltLibrary.ToNodeSetOrRtf(left), XsltLibrary.ToNodeSetOrRtf(right), TypeCode.Double);
				}
				XPathItem xpathItem = right[0];
				return XsltLibrary.CompareNodeSetAndValue(comparisonOperator, XsltLibrary.ToNodeSetOrRtf(left), xpathItem, XsltLibrary.WeakestTypeCode(XsltLibrary.GetTypeCode(xpathItem), TypeCode.Double));
			}
			else
			{
				if (XsltLibrary.IsNodeSetOrRtf(right))
				{
					XPathItem xpathItem2 = left[0];
					comparisonOperator = XsltLibrary.InvertOperator(comparisonOperator);
					return XsltLibrary.CompareNodeSetAndValue(comparisonOperator, XsltLibrary.ToNodeSetOrRtf(right), xpathItem2, XsltLibrary.WeakestTypeCode(XsltLibrary.GetTypeCode(xpathItem2), TypeCode.Double));
				}
				XPathItem xpathItem3 = left[0];
				XPathItem xpathItem4 = right[0];
				return XsltLibrary.CompareValues(comparisonOperator, xpathItem3, xpathItem4, TypeCode.Double);
			}
		}

		// Token: 0x060009D0 RID: 2512 RVA: 0x0002EB88 File Offset: 0x0002DB88
		public bool IsSameNodeSort(XPathNavigator nav1, XPathNavigator nav2)
		{
			XPathNodeType nodeType = nav1.NodeType;
			XPathNodeType nodeType2 = nav2.NodeType;
			if (XPathNodeType.Text <= nodeType && nodeType <= XPathNodeType.Whitespace)
			{
				return XPathNodeType.Text <= nodeType2 && nodeType2 <= XPathNodeType.Whitespace;
			}
			return nodeType == nodeType2 && Ref.Equal(nav1.LocalName, nav2.LocalName) && Ref.Equal(nav1.NamespaceURI, nav2.NamespaceURI);
		}

		// Token: 0x060009D1 RID: 2513 RVA: 0x0002EBE3 File Offset: 0x0002DBE3
		[Conditional("DEBUG")]
		internal static void CheckXsltValue(XPathItem item)
		{
		}

		// Token: 0x060009D2 RID: 2514 RVA: 0x0002EBE8 File Offset: 0x0002DBE8
		[Conditional("DEBUG")]
		internal static void CheckXsltValue(IList<XPathItem> val)
		{
			if (val.Count == 1)
			{
				XsltFunctions.EXslObjectType(val);
				return;
			}
			int count = val.Count;
			for (int i = 0; i < count; i++)
			{
				if (!val[i].IsNode)
				{
					return;
				}
				if (i == 1)
				{
					i += Math.Max(count - 4, 0);
				}
			}
		}

		// Token: 0x060009D3 RID: 2515 RVA: 0x0002EC38 File Offset: 0x0002DC38
		private static bool IsNodeSetOrRtf(IList<XPathItem> val)
		{
			return val.Count != 1 || val[0].IsNode;
		}

		// Token: 0x060009D4 RID: 2516 RVA: 0x0002EC51 File Offset: 0x0002DC51
		private static IList<XPathNavigator> ToNodeSetOrRtf(IList<XPathItem> val)
		{
			return XmlILStorageConverter.ItemsToNavigators(val);
		}

		// Token: 0x04000625 RID: 1573
		internal const int InvariantCultureLcid = 127;

		// Token: 0x04000626 RID: 1574
		private XmlQueryRuntime runtime;

		// Token: 0x04000627 RID: 1575
		private HybridDictionary functionsAvail;

		// Token: 0x04000628 RID: 1576
		private DecimalFormats decimalFormats;

		// Token: 0x04000629 RID: 1577
		private List<DecimalFormatter> decimalFormatters;

		// Token: 0x020000D0 RID: 208
		internal enum ComparisonOperator
		{
			// Token: 0x0400062B RID: 1579
			Eq,
			// Token: 0x0400062C RID: 1580
			Ne,
			// Token: 0x0400062D RID: 1581
			Lt,
			// Token: 0x0400062E RID: 1582
			Le,
			// Token: 0x0400062F RID: 1583
			Gt,
			// Token: 0x04000630 RID: 1584
			Ge
		}
	}
}
