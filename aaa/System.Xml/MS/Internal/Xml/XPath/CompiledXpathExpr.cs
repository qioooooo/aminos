using System;
using System.Collections;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace MS.Internal.Xml.XPath
{
	// Token: 0x02000130 RID: 304
	internal class CompiledXpathExpr : XPathExpression
	{
		// Token: 0x060011BA RID: 4538 RVA: 0x0004E686 File Offset: 0x0004D686
		internal CompiledXpathExpr(Query query, string expression, bool needContext)
		{
			this.query = query;
			this.expr = expression;
			this.needContext = needContext;
		}

		// Token: 0x1700045D RID: 1117
		// (get) Token: 0x060011BB RID: 4539 RVA: 0x0004E6A3 File Offset: 0x0004D6A3
		internal Query QueryTree
		{
			get
			{
				if (this.needContext)
				{
					throw XPathException.Create("Xp_NoContext");
				}
				return this.query;
			}
		}

		// Token: 0x1700045E RID: 1118
		// (get) Token: 0x060011BC RID: 4540 RVA: 0x0004E6BE File Offset: 0x0004D6BE
		public override string Expression
		{
			get
			{
				return this.expr;
			}
		}

		// Token: 0x060011BD RID: 4541 RVA: 0x0004E6C6 File Offset: 0x0004D6C6
		public virtual void CheckErrors()
		{
		}

		// Token: 0x060011BE RID: 4542 RVA: 0x0004E6C8 File Offset: 0x0004D6C8
		public override void AddSort(object expr, IComparer comparer)
		{
			Query query;
			if (expr is string)
			{
				query = new QueryBuilder().Build((string)expr, out this.needContext);
			}
			else
			{
				if (!(expr is CompiledXpathExpr))
				{
					throw XPathException.Create("Xp_BadQueryObject");
				}
				query = ((CompiledXpathExpr)expr).QueryTree;
			}
			SortQuery sortQuery = this.query as SortQuery;
			if (sortQuery == null)
			{
				sortQuery = (this.query = new SortQuery(this.query));
			}
			sortQuery.AddSort(query, comparer);
		}

		// Token: 0x060011BF RID: 4543 RVA: 0x0004E741 File Offset: 0x0004D741
		public override void AddSort(object expr, XmlSortOrder order, XmlCaseOrder caseOrder, string lang, XmlDataType dataType)
		{
			this.AddSort(expr, new XPathComparerHelper(order, caseOrder, lang, dataType));
		}

		// Token: 0x060011C0 RID: 4544 RVA: 0x0004E755 File Offset: 0x0004D755
		public override XPathExpression Clone()
		{
			return new CompiledXpathExpr(Query.Clone(this.query), this.expr, this.needContext);
		}

		// Token: 0x060011C1 RID: 4545 RVA: 0x0004E774 File Offset: 0x0004D774
		public override void SetContext(XmlNamespaceManager nsManager)
		{
			XsltContext xsltContext = nsManager as XsltContext;
			if (xsltContext == null)
			{
				if (nsManager == null)
				{
					nsManager = new XmlNamespaceManager(new NameTable());
				}
				xsltContext = new CompiledXpathExpr.UndefinedXsltContext(nsManager);
			}
			this.query.SetXsltContext(xsltContext);
			this.needContext = false;
		}

		// Token: 0x060011C2 RID: 4546 RVA: 0x0004E7B4 File Offset: 0x0004D7B4
		public override void SetContext(IXmlNamespaceResolver nsResolver)
		{
			XmlNamespaceManager xmlNamespaceManager = nsResolver as XmlNamespaceManager;
			if (xmlNamespaceManager == null && nsResolver != null)
			{
				xmlNamespaceManager = new XmlNamespaceManager(new NameTable());
			}
			this.SetContext(xmlNamespaceManager);
		}

		// Token: 0x1700045F RID: 1119
		// (get) Token: 0x060011C3 RID: 4547 RVA: 0x0004E7E0 File Offset: 0x0004D7E0
		public override XPathResultType ReturnType
		{
			get
			{
				return this.query.StaticType;
			}
		}

		// Token: 0x04000B4A RID: 2890
		private Query query;

		// Token: 0x04000B4B RID: 2891
		private string expr;

		// Token: 0x04000B4C RID: 2892
		private bool needContext;

		// Token: 0x02000132 RID: 306
		private class UndefinedXsltContext : XsltContext
		{
			// Token: 0x060011CC RID: 4556 RVA: 0x0004E80B File Offset: 0x0004D80B
			public UndefinedXsltContext(XmlNamespaceManager nsManager)
				: base(false)
			{
				this.nsManager = nsManager;
			}

			// Token: 0x17000461 RID: 1121
			// (get) Token: 0x060011CD RID: 4557 RVA: 0x0004E81B File Offset: 0x0004D81B
			public override string DefaultNamespace
			{
				get
				{
					return string.Empty;
				}
			}

			// Token: 0x060011CE RID: 4558 RVA: 0x0004E824 File Offset: 0x0004D824
			public override string LookupNamespace(string prefix)
			{
				if (prefix.Length == 0)
				{
					return string.Empty;
				}
				string text = this.nsManager.LookupNamespace(prefix);
				if (text == null)
				{
					throw XPathException.Create("XmlUndefinedAlias", prefix);
				}
				return text;
			}

			// Token: 0x060011CF RID: 4559 RVA: 0x0004E85C File Offset: 0x0004D85C
			public override IXsltContextVariable ResolveVariable(string prefix, string name)
			{
				throw XPathException.Create("Xp_UndefinedXsltContext");
			}

			// Token: 0x060011D0 RID: 4560 RVA: 0x0004E868 File Offset: 0x0004D868
			public override IXsltContextFunction ResolveFunction(string prefix, string name, XPathResultType[] ArgTypes)
			{
				throw XPathException.Create("Xp_UndefinedXsltContext");
			}

			// Token: 0x17000462 RID: 1122
			// (get) Token: 0x060011D1 RID: 4561 RVA: 0x0004E874 File Offset: 0x0004D874
			public override bool Whitespace
			{
				get
				{
					return false;
				}
			}

			// Token: 0x060011D2 RID: 4562 RVA: 0x0004E877 File Offset: 0x0004D877
			public override bool PreserveWhitespace(XPathNavigator node)
			{
				return false;
			}

			// Token: 0x060011D3 RID: 4563 RVA: 0x0004E87A File Offset: 0x0004D87A
			public override int CompareDocument(string baseUri, string nextbaseUri)
			{
				return string.CompareOrdinal(baseUri, nextbaseUri);
			}

			// Token: 0x04000B4D RID: 2893
			private XmlNamespaceManager nsManager;
		}
	}
}
