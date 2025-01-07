using System;
using System.Collections;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace MS.Internal.Xml.XPath
{
	internal class CompiledXpathExpr : XPathExpression
	{
		internal CompiledXpathExpr(Query query, string expression, bool needContext)
		{
			this.query = query;
			this.expr = expression;
			this.needContext = needContext;
		}

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

		public override string Expression
		{
			get
			{
				return this.expr;
			}
		}

		public virtual void CheckErrors()
		{
		}

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

		public override void AddSort(object expr, XmlSortOrder order, XmlCaseOrder caseOrder, string lang, XmlDataType dataType)
		{
			this.AddSort(expr, new XPathComparerHelper(order, caseOrder, lang, dataType));
		}

		public override XPathExpression Clone()
		{
			return new CompiledXpathExpr(Query.Clone(this.query), this.expr, this.needContext);
		}

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

		public override void SetContext(IXmlNamespaceResolver nsResolver)
		{
			XmlNamespaceManager xmlNamespaceManager = nsResolver as XmlNamespaceManager;
			if (xmlNamespaceManager == null && nsResolver != null)
			{
				xmlNamespaceManager = new XmlNamespaceManager(new NameTable());
			}
			this.SetContext(xmlNamespaceManager);
		}

		public override XPathResultType ReturnType
		{
			get
			{
				return this.query.StaticType;
			}
		}

		private Query query;

		private string expr;

		private bool needContext;

		private class UndefinedXsltContext : XsltContext
		{
			public UndefinedXsltContext(XmlNamespaceManager nsManager)
				: base(false)
			{
				this.nsManager = nsManager;
			}

			public override string DefaultNamespace
			{
				get
				{
					return string.Empty;
				}
			}

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

			public override IXsltContextVariable ResolveVariable(string prefix, string name)
			{
				throw XPathException.Create("Xp_UndefinedXsltContext");
			}

			public override IXsltContextFunction ResolveFunction(string prefix, string name, XPathResultType[] ArgTypes)
			{
				throw XPathException.Create("Xp_UndefinedXsltContext");
			}

			public override bool Whitespace
			{
				get
				{
					return false;
				}
			}

			public override bool PreserveWhitespace(XPathNavigator node)
			{
				return false;
			}

			public override int CompareDocument(string baseUri, string nextbaseUri)
			{
				return string.CompareOrdinal(baseUri, nextbaseUri);
			}

			private XmlNamespaceManager nsManager;
		}
	}
}
