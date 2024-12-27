using System;
using System.Diagnostics;
using System.Xml.Schema;
using System.Xml.Xsl.Qil;
using System.Xml.Xsl.Runtime;
using System.Xml.Xsl.XPath;

namespace System.Xml.Xsl.Xslt
{
	// Token: 0x0200012B RID: 299
	internal class XsltQilFactory : XPathQilFactory
	{
		// Token: 0x06000D1E RID: 3358 RVA: 0x00044780 File Offset: 0x00043780
		public XsltQilFactory(QilFactory f, bool debug)
			: base(f, debug)
		{
		}

		// Token: 0x06000D1F RID: 3359 RVA: 0x0004478C File Offset: 0x0004378C
		[Conditional("DEBUG")]
		public void CheckXsltType(QilNode n)
		{
			XmlQueryType xmlType = n.XmlType;
			XmlTypeCode typeCode = xmlType.TypeCode;
			switch (typeCode)
			{
			case XmlTypeCode.None:
			case XmlTypeCode.Item:
				break;
			default:
				switch (typeCode)
				{
				case XmlTypeCode.String:
				case XmlTypeCode.Boolean:
				case XmlTypeCode.Decimal:
				case XmlTypeCode.Float:
				case XmlTypeCode.Double:
					break;
				default:
					if (typeCode != XmlTypeCode.QName)
					{
					}
					break;
				}
				break;
			}
		}

		// Token: 0x06000D20 RID: 3360 RVA: 0x000447D7 File Offset: 0x000437D7
		[Conditional("DEBUG")]
		public void CheckQName(QilNode n)
		{
		}

		// Token: 0x06000D21 RID: 3361 RVA: 0x000447D9 File Offset: 0x000437D9
		public QilNode DefaultValueMarker()
		{
			return base.QName("default-value", "urn:schemas-microsoft-com:xslt-debug");
		}

		// Token: 0x06000D22 RID: 3362 RVA: 0x000447EB File Offset: 0x000437EB
		public QilNode IsDefaultValueMarker(QilNode n)
		{
			return base.IsType(n, XmlQueryTypeFactory.QNameX);
		}

		// Token: 0x06000D23 RID: 3363 RVA: 0x000447FC File Offset: 0x000437FC
		public QilNode InvokeIsSameNodeSort(QilNode n1, QilNode n2)
		{
			return base.XsltInvokeEarlyBound(base.QName("is-same-node-sort"), XsltMethods.IsSameNodeSort, XmlQueryTypeFactory.BooleanX, new QilNode[] { n1, n2 });
		}

		// Token: 0x06000D24 RID: 3364 RVA: 0x00044834 File Offset: 0x00043834
		public QilNode InvokeSystemProperty(QilNode n)
		{
			return base.XsltInvokeEarlyBound(base.QName("system-property"), XsltMethods.SystemProperty, XmlQueryTypeFactory.Choice(XmlQueryTypeFactory.DoubleX, XmlQueryTypeFactory.StringX), new QilNode[] { n });
		}

		// Token: 0x06000D25 RID: 3365 RVA: 0x00044874 File Offset: 0x00043874
		public QilNode InvokeElementAvailable(QilNode n)
		{
			return base.XsltInvokeEarlyBound(base.QName("element-available"), XsltMethods.ElementAvailable, XmlQueryTypeFactory.BooleanX, new QilNode[] { n });
		}

		// Token: 0x06000D26 RID: 3366 RVA: 0x000448A8 File Offset: 0x000438A8
		public QilNode InvokeCheckScriptNamespace(string nsUri)
		{
			return base.XsltInvokeEarlyBound(base.QName("register-script-namespace"), XsltMethods.CheckScriptNamespace, XmlQueryTypeFactory.IntX, new QilNode[] { base.String(nsUri) });
		}

		// Token: 0x06000D27 RID: 3367 RVA: 0x000448E4 File Offset: 0x000438E4
		public QilNode InvokeFunctionAvailable(QilNode n)
		{
			return base.XsltInvokeEarlyBound(base.QName("function-available"), XsltMethods.FunctionAvailable, XmlQueryTypeFactory.BooleanX, new QilNode[] { n });
		}

		// Token: 0x06000D28 RID: 3368 RVA: 0x00044918 File Offset: 0x00043918
		public QilNode InvokeBaseUri(QilNode n)
		{
			return base.XsltInvokeEarlyBound(base.QName("base-uri"), XsltMethods.BaseUri, XmlQueryTypeFactory.StringX, new QilNode[] { n });
		}

		// Token: 0x06000D29 RID: 3369 RVA: 0x0004494C File Offset: 0x0004394C
		public QilNode InvokeOnCurrentNodeChanged(QilNode n)
		{
			return base.XsltInvokeEarlyBound(base.QName("on-current-node-changed"), XsltMethods.OnCurrentNodeChanged, XmlQueryTypeFactory.IntX, new QilNode[] { n });
		}

		// Token: 0x06000D2A RID: 3370 RVA: 0x00044980 File Offset: 0x00043980
		public QilNode InvokeLangToLcid(QilNode n, bool fwdCompat)
		{
			return base.XsltInvokeEarlyBound(base.QName("lang-to-lcid"), XsltMethods.LangToLcid, XmlQueryTypeFactory.IntX, new QilNode[]
			{
				n,
				base.Boolean(fwdCompat)
			});
		}

		// Token: 0x06000D2B RID: 3371 RVA: 0x000449C0 File Offset: 0x000439C0
		public QilNode InvokeNumberFormat(QilNode value, QilNode format, QilNode lang, QilNode letterValue, QilNode groupingSeparator, QilNode groupingSize)
		{
			return base.XsltInvokeEarlyBound(base.QName("number-format"), XsltMethods.NumberFormat, XmlQueryTypeFactory.StringX, new QilNode[] { value, format, lang, letterValue, groupingSeparator, groupingSize });
		}

		// Token: 0x06000D2C RID: 3372 RVA: 0x00044A0C File Offset: 0x00043A0C
		public QilNode InvokeRegisterDecimalFormat(DecimalFormatDecl format)
		{
			return base.XsltInvokeEarlyBound(base.QName("register-decimal-format"), XsltMethods.RegisterDecimalFormat, XmlQueryTypeFactory.IntX, new QilNode[]
			{
				base.QName(format.Name.Name, format.Name.Namespace),
				base.String(format.InfinitySymbol),
				base.String(format.NanSymbol),
				base.String(new string(format.Characters))
			});
		}

		// Token: 0x06000D2D RID: 3373 RVA: 0x00044A90 File Offset: 0x00043A90
		public QilNode InvokeRegisterDecimalFormatter(QilNode formatPicture, DecimalFormatDecl format)
		{
			return base.XsltInvokeEarlyBound(base.QName("register-decimal-formatter"), XsltMethods.RegisterDecimalFormatter, XmlQueryTypeFactory.DoubleX, new QilNode[]
			{
				formatPicture,
				base.String(format.InfinitySymbol),
				base.String(format.NanSymbol),
				base.String(new string(format.Characters))
			});
		}

		// Token: 0x06000D2E RID: 3374 RVA: 0x00044AF8 File Offset: 0x00043AF8
		public QilNode InvokeFormatNumberStatic(QilNode value, QilNode decimalFormatIndex)
		{
			return base.XsltInvokeEarlyBound(base.QName("format-number-static"), XsltMethods.FormatNumberStatic, XmlQueryTypeFactory.StringX, new QilNode[] { value, decimalFormatIndex });
		}

		// Token: 0x06000D2F RID: 3375 RVA: 0x00044B30 File Offset: 0x00043B30
		public QilNode InvokeFormatNumberDynamic(QilNode value, QilNode formatPicture, QilNode decimalFormatName, QilNode errorMessageName)
		{
			return base.XsltInvokeEarlyBound(base.QName("format-number-dynamic"), XsltMethods.FormatNumberDynamic, XmlQueryTypeFactory.StringX, new QilNode[] { value, formatPicture, decimalFormatName, errorMessageName });
		}

		// Token: 0x06000D30 RID: 3376 RVA: 0x00044B74 File Offset: 0x00043B74
		public QilNode InvokeOuterXml(QilNode n)
		{
			return base.XsltInvokeEarlyBound(base.QName("outer-xml"), XsltMethods.OuterXml, XmlQueryTypeFactory.StringX, new QilNode[] { n });
		}

		// Token: 0x06000D31 RID: 3377 RVA: 0x00044BA8 File Offset: 0x00043BA8
		public QilNode InvokeMsFormatDateTime(QilNode datetime, QilNode format, QilNode lang, QilNode isDate)
		{
			return base.XsltInvokeEarlyBound(base.QName("ms:format-date-time"), XsltMethods.MSFormatDateTime, XmlQueryTypeFactory.StringX, new QilNode[] { datetime, format, lang, isDate });
		}

		// Token: 0x06000D32 RID: 3378 RVA: 0x00044BEC File Offset: 0x00043BEC
		public QilNode InvokeMsStringCompare(QilNode x, QilNode y, QilNode lang, QilNode options)
		{
			return base.XsltInvokeEarlyBound(base.QName("ms:string-compare"), XsltMethods.MSStringCompare, XmlQueryTypeFactory.DoubleX, new QilNode[] { x, y, lang, options });
		}

		// Token: 0x06000D33 RID: 3379 RVA: 0x00044C30 File Offset: 0x00043C30
		public QilNode InvokeMsUtc(QilNode n)
		{
			return base.XsltInvokeEarlyBound(base.QName("ms:utc"), XsltMethods.MSUtc, XmlQueryTypeFactory.StringX, new QilNode[] { n });
		}

		// Token: 0x06000D34 RID: 3380 RVA: 0x00044C64 File Offset: 0x00043C64
		public QilNode InvokeMsNumber(QilNode n)
		{
			return base.XsltInvokeEarlyBound(base.QName("ms:number"), XsltMethods.MSNumber, XmlQueryTypeFactory.DoubleX, new QilNode[] { n });
		}

		// Token: 0x06000D35 RID: 3381 RVA: 0x00044C98 File Offset: 0x00043C98
		public QilNode InvokeMsLocalName(QilNode n)
		{
			return base.XsltInvokeEarlyBound(base.QName("ms:local-name"), XsltMethods.MSLocalName, XmlQueryTypeFactory.StringX, new QilNode[] { n });
		}

		// Token: 0x06000D36 RID: 3382 RVA: 0x00044CCC File Offset: 0x00043CCC
		public QilNode InvokeMsNamespaceUri(QilNode n, QilNode currentNode)
		{
			return base.XsltInvokeEarlyBound(base.QName("ms:namespace-uri"), XsltMethods.MSNamespaceUri, XmlQueryTypeFactory.StringX, new QilNode[] { n, currentNode });
		}

		// Token: 0x06000D37 RID: 3383 RVA: 0x00044D04 File Offset: 0x00043D04
		public QilNode InvokeEXslObjectType(QilNode n)
		{
			return base.XsltInvokeEarlyBound(base.QName("exsl:object-type"), XsltMethods.EXslObjectType, XmlQueryTypeFactory.StringX, new QilNode[] { n });
		}
	}
}
