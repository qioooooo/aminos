using System;
using System.Collections.Generic;
using System.Reflection;
using System.Xml.XPath;

namespace System.Xml.Xsl.Runtime
{
	// Token: 0x020000CE RID: 206
	internal static class XsltMethods
	{
		// Token: 0x060009B6 RID: 2486 RVA: 0x0002DFE8 File Offset: 0x0002CFE8
		public static MethodInfo GetMethod(Type className, string methName)
		{
			return className.GetMethod(methName);
		}

		// Token: 0x060009B7 RID: 2487 RVA: 0x0002E000 File Offset: 0x0002D000
		public static MethodInfo GetMethod(Type className, string methName, params Type[] args)
		{
			return className.GetMethod(methName, args);
		}

		// Token: 0x04000600 RID: 1536
		public static readonly MethodInfo FormatMessage = XsltMethods.GetMethod(typeof(XsltLibrary), "FormatMessage");

		// Token: 0x04000601 RID: 1537
		public static readonly MethodInfo EnsureNodeSet = XsltMethods.GetMethod(typeof(XsltConvert), "EnsureNodeSet", new Type[] { typeof(IList<XPathItem>) });

		// Token: 0x04000602 RID: 1538
		public static readonly MethodInfo EqualityOperator = XsltMethods.GetMethod(typeof(XsltLibrary), "EqualityOperator");

		// Token: 0x04000603 RID: 1539
		public static readonly MethodInfo RelationalOperator = XsltMethods.GetMethod(typeof(XsltLibrary), "RelationalOperator");

		// Token: 0x04000604 RID: 1540
		public static readonly MethodInfo StartsWith = XsltMethods.GetMethod(typeof(XsltFunctions), "StartsWith");

		// Token: 0x04000605 RID: 1541
		public static readonly MethodInfo Contains = XsltMethods.GetMethod(typeof(XsltFunctions), "Contains");

		// Token: 0x04000606 RID: 1542
		public static readonly MethodInfo SubstringBefore = XsltMethods.GetMethod(typeof(XsltFunctions), "SubstringBefore");

		// Token: 0x04000607 RID: 1543
		public static readonly MethodInfo SubstringAfter = XsltMethods.GetMethod(typeof(XsltFunctions), "SubstringAfter");

		// Token: 0x04000608 RID: 1544
		public static readonly MethodInfo Substring2 = XsltMethods.GetMethod(typeof(XsltFunctions), "Substring", new Type[]
		{
			typeof(string),
			typeof(double)
		});

		// Token: 0x04000609 RID: 1545
		public static readonly MethodInfo Substring3 = XsltMethods.GetMethod(typeof(XsltFunctions), "Substring", new Type[]
		{
			typeof(string),
			typeof(double),
			typeof(double)
		});

		// Token: 0x0400060A RID: 1546
		public static readonly MethodInfo NormalizeSpace = XsltMethods.GetMethod(typeof(XsltFunctions), "NormalizeSpace");

		// Token: 0x0400060B RID: 1547
		public static readonly MethodInfo Translate = XsltMethods.GetMethod(typeof(XsltFunctions), "Translate");

		// Token: 0x0400060C RID: 1548
		public static readonly MethodInfo Lang = XsltMethods.GetMethod(typeof(XsltFunctions), "Lang");

		// Token: 0x0400060D RID: 1549
		public static readonly MethodInfo Floor = XsltMethods.GetMethod(typeof(Math), "Floor", new Type[] { typeof(double) });

		// Token: 0x0400060E RID: 1550
		public static readonly MethodInfo Ceiling = XsltMethods.GetMethod(typeof(Math), "Ceiling", new Type[] { typeof(double) });

		// Token: 0x0400060F RID: 1551
		public static readonly MethodInfo Round = XsltMethods.GetMethod(typeof(XsltFunctions), "Round");

		// Token: 0x04000610 RID: 1552
		public static readonly MethodInfo SystemProperty = XsltMethods.GetMethod(typeof(XsltFunctions), "SystemProperty");

		// Token: 0x04000611 RID: 1553
		public static readonly MethodInfo BaseUri = XsltMethods.GetMethod(typeof(XsltFunctions), "BaseUri");

		// Token: 0x04000612 RID: 1554
		public static readonly MethodInfo OuterXml = XsltMethods.GetMethod(typeof(XsltFunctions), "OuterXml");

		// Token: 0x04000613 RID: 1555
		public static readonly MethodInfo OnCurrentNodeChanged = XsltMethods.GetMethod(typeof(XmlQueryRuntime), "OnCurrentNodeChanged");

		// Token: 0x04000614 RID: 1556
		public static readonly MethodInfo MSFormatDateTime = XsltMethods.GetMethod(typeof(XsltFunctions), "MSFormatDateTime");

		// Token: 0x04000615 RID: 1557
		public static readonly MethodInfo MSStringCompare = XsltMethods.GetMethod(typeof(XsltFunctions), "MSStringCompare");

		// Token: 0x04000616 RID: 1558
		public static readonly MethodInfo MSUtc = XsltMethods.GetMethod(typeof(XsltFunctions), "MSUtc");

		// Token: 0x04000617 RID: 1559
		public static readonly MethodInfo MSNumber = XsltMethods.GetMethod(typeof(XsltFunctions), "MSNumber");

		// Token: 0x04000618 RID: 1560
		public static readonly MethodInfo MSLocalName = XsltMethods.GetMethod(typeof(XsltFunctions), "MSLocalName");

		// Token: 0x04000619 RID: 1561
		public static readonly MethodInfo MSNamespaceUri = XsltMethods.GetMethod(typeof(XsltFunctions), "MSNamespaceUri");

		// Token: 0x0400061A RID: 1562
		public static readonly MethodInfo EXslObjectType = XsltMethods.GetMethod(typeof(XsltFunctions), "EXslObjectType");

		// Token: 0x0400061B RID: 1563
		public static readonly MethodInfo CheckScriptNamespace = XsltMethods.GetMethod(typeof(XsltLibrary), "CheckScriptNamespace");

		// Token: 0x0400061C RID: 1564
		public static readonly MethodInfo FunctionAvailable = XsltMethods.GetMethod(typeof(XsltLibrary), "FunctionAvailable");

		// Token: 0x0400061D RID: 1565
		public static readonly MethodInfo ElementAvailable = XsltMethods.GetMethod(typeof(XsltLibrary), "ElementAvailable");

		// Token: 0x0400061E RID: 1566
		public static readonly MethodInfo RegisterDecimalFormat = XsltMethods.GetMethod(typeof(XsltLibrary), "RegisterDecimalFormat");

		// Token: 0x0400061F RID: 1567
		public static readonly MethodInfo RegisterDecimalFormatter = XsltMethods.GetMethod(typeof(XsltLibrary), "RegisterDecimalFormatter");

		// Token: 0x04000620 RID: 1568
		public static readonly MethodInfo FormatNumberStatic = XsltMethods.GetMethod(typeof(XsltLibrary), "FormatNumberStatic");

		// Token: 0x04000621 RID: 1569
		public static readonly MethodInfo FormatNumberDynamic = XsltMethods.GetMethod(typeof(XsltLibrary), "FormatNumberDynamic");

		// Token: 0x04000622 RID: 1570
		public static readonly MethodInfo IsSameNodeSort = XsltMethods.GetMethod(typeof(XsltLibrary), "IsSameNodeSort");

		// Token: 0x04000623 RID: 1571
		public static readonly MethodInfo LangToLcid = XsltMethods.GetMethod(typeof(XsltLibrary), "LangToLcid");

		// Token: 0x04000624 RID: 1572
		public static readonly MethodInfo NumberFormat = XsltMethods.GetMethod(typeof(XsltLibrary), "NumberFormat");
	}
}
