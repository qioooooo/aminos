using System;
using System.Collections;
using MS.Internal.Xml.XPath;

namespace System.Xml.XPath
{
	// Token: 0x02000116 RID: 278
	public abstract class XPathExpression
	{
		// Token: 0x060010BA RID: 4282 RVA: 0x0004C12B File Offset: 0x0004B12B
		internal XPathExpression()
		{
		}

		// Token: 0x17000405 RID: 1029
		// (get) Token: 0x060010BB RID: 4283
		public abstract string Expression { get; }

		// Token: 0x060010BC RID: 4284
		public abstract void AddSort(object expr, IComparer comparer);

		// Token: 0x060010BD RID: 4285
		public abstract void AddSort(object expr, XmlSortOrder order, XmlCaseOrder caseOrder, string lang, XmlDataType dataType);

		// Token: 0x060010BE RID: 4286
		public abstract XPathExpression Clone();

		// Token: 0x060010BF RID: 4287
		public abstract void SetContext(XmlNamespaceManager nsManager);

		// Token: 0x060010C0 RID: 4288
		public abstract void SetContext(IXmlNamespaceResolver nsResolver);

		// Token: 0x17000406 RID: 1030
		// (get) Token: 0x060010C1 RID: 4289
		public abstract XPathResultType ReturnType { get; }

		// Token: 0x060010C2 RID: 4290 RVA: 0x0004C133 File Offset: 0x0004B133
		public static XPathExpression Compile(string xpath)
		{
			return XPathExpression.Compile(xpath, null);
		}

		// Token: 0x060010C3 RID: 4291 RVA: 0x0004C13C File Offset: 0x0004B13C
		public static XPathExpression Compile(string xpath, IXmlNamespaceResolver nsResolver)
		{
			bool flag;
			Query query = new QueryBuilder().Build(xpath, out flag);
			CompiledXpathExpr compiledXpathExpr = new CompiledXpathExpr(query, xpath, flag);
			if (nsResolver != null)
			{
				XmlNamespaceManager namespaces = XPathNavigator.GetNamespaces(nsResolver);
				compiledXpathExpr.SetContext(namespaces);
			}
			return compiledXpathExpr;
		}

		// Token: 0x060010C4 RID: 4292 RVA: 0x0004C172 File Offset: 0x0004B172
		private void PrintQuery(XmlWriter w)
		{
			((CompiledXpathExpr)this).QueryTree.PrintQuery(w);
		}
	}
}
