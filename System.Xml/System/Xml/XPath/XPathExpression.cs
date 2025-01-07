﻿using System;
using System.Collections;
using MS.Internal.Xml.XPath;

namespace System.Xml.XPath
{
	public abstract class XPathExpression
	{
		internal XPathExpression()
		{
		}

		public abstract string Expression { get; }

		public abstract void AddSort(object expr, IComparer comparer);

		public abstract void AddSort(object expr, XmlSortOrder order, XmlCaseOrder caseOrder, string lang, XmlDataType dataType);

		public abstract XPathExpression Clone();

		public abstract void SetContext(XmlNamespaceManager nsManager);

		public abstract void SetContext(IXmlNamespaceResolver nsResolver);

		public abstract XPathResultType ReturnType { get; }

		public static XPathExpression Compile(string xpath)
		{
			return XPathExpression.Compile(xpath, null);
		}

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

		private void PrintQuery(XmlWriter w)
		{
			((CompiledXpathExpr)this).QueryTree.PrintQuery(w);
		}
	}
}
