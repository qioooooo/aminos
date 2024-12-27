using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace MS.Internal.Xml.XPath
{
	// Token: 0x0200011F RID: 287
	[DebuggerDisplay("{ToString()}")]
	internal abstract class Query : ResetableIterator
	{
		// Token: 0x0600112A RID: 4394 RVA: 0x0004D43F File Offset: 0x0004C43F
		public Query()
		{
		}

		// Token: 0x0600112B RID: 4395 RVA: 0x0004D447 File Offset: 0x0004C447
		protected Query(Query other)
			: base(other)
		{
		}

		// Token: 0x0600112C RID: 4396 RVA: 0x0004D450 File Offset: 0x0004C450
		public override bool MoveNext()
		{
			return this.Advance() != null;
		}

		// Token: 0x17000432 RID: 1074
		// (get) Token: 0x0600112D RID: 4397 RVA: 0x0004D460 File Offset: 0x0004C460
		public override int Count
		{
			get
			{
				if (this.count == -1)
				{
					Query query = (Query)this.Clone();
					query.Reset();
					this.count = 0;
					while (query.MoveNext())
					{
						this.count++;
					}
				}
				return this.count;
			}
		}

		// Token: 0x0600112E RID: 4398 RVA: 0x0004D4AD File Offset: 0x0004C4AD
		public virtual void SetXsltContext(XsltContext context)
		{
		}

		// Token: 0x0600112F RID: 4399
		public abstract object Evaluate(XPathNodeIterator nodeIterator);

		// Token: 0x06001130 RID: 4400
		public abstract XPathNavigator Advance();

		// Token: 0x06001131 RID: 4401 RVA: 0x0004D4AF File Offset: 0x0004C4AF
		public virtual XPathNavigator MatchNode(XPathNavigator current)
		{
			throw XPathException.Create("Xp_InvalidPattern");
		}

		// Token: 0x17000433 RID: 1075
		// (get) Token: 0x06001132 RID: 4402 RVA: 0x0004D4BB File Offset: 0x0004C4BB
		public virtual double XsltDefaultPriority
		{
			get
			{
				return 0.5;
			}
		}

		// Token: 0x17000434 RID: 1076
		// (get) Token: 0x06001133 RID: 4403
		public abstract XPathResultType StaticType { get; }

		// Token: 0x17000435 RID: 1077
		// (get) Token: 0x06001134 RID: 4404 RVA: 0x0004D4C6 File Offset: 0x0004C4C6
		public virtual QueryProps Properties
		{
			get
			{
				return QueryProps.Merge;
			}
		}

		// Token: 0x06001135 RID: 4405 RVA: 0x0004D4CA File Offset: 0x0004C4CA
		public static Query Clone(Query input)
		{
			if (input != null)
			{
				return (Query)input.Clone();
			}
			return null;
		}

		// Token: 0x06001136 RID: 4406 RVA: 0x0004D4DC File Offset: 0x0004C4DC
		protected static XPathNodeIterator Clone(XPathNodeIterator input)
		{
			if (input != null)
			{
				return input.Clone();
			}
			return null;
		}

		// Token: 0x06001137 RID: 4407 RVA: 0x0004D4E9 File Offset: 0x0004C4E9
		protected static XPathNavigator Clone(XPathNavigator input)
		{
			if (input != null)
			{
				return input.Clone();
			}
			return null;
		}

		// Token: 0x06001138 RID: 4408 RVA: 0x0004D4F8 File Offset: 0x0004C4F8
		public bool Insert(List<XPathNavigator> buffer, XPathNavigator nav)
		{
			int i = 0;
			int num = buffer.Count;
			if (num != 0)
			{
				switch (Query.CompareNodes(buffer[num - 1], nav))
				{
				case XmlNodeOrder.Before:
					buffer.Add(nav.Clone());
					return true;
				case XmlNodeOrder.Same:
					return false;
				}
				num--;
			}
			while (i < num)
			{
				int median = Query.GetMedian(i, num);
				switch (Query.CompareNodes(buffer[median], nav))
				{
				case XmlNodeOrder.Before:
					i = median + 1;
					continue;
				case XmlNodeOrder.Same:
					return false;
				}
				num = median;
			}
			buffer.Insert(i, nav.Clone());
			return true;
		}

		// Token: 0x06001139 RID: 4409 RVA: 0x0004D593 File Offset: 0x0004C593
		private static int GetMedian(int l, int r)
		{
			return (int)((uint)(l + r) >> 1);
		}

		// Token: 0x0600113A RID: 4410 RVA: 0x0004D59C File Offset: 0x0004C59C
		public static XmlNodeOrder CompareNodes(XPathNavigator l, XPathNavigator r)
		{
			XmlNodeOrder xmlNodeOrder = l.ComparePosition(r);
			if (xmlNodeOrder == XmlNodeOrder.Unknown)
			{
				XPathNavigator xpathNavigator = l.Clone();
				xpathNavigator.MoveToRoot();
				string baseURI = xpathNavigator.BaseURI;
				if (!xpathNavigator.MoveTo(r))
				{
					xpathNavigator = r.Clone();
				}
				xpathNavigator.MoveToRoot();
				string baseURI2 = xpathNavigator.BaseURI;
				int num = string.CompareOrdinal(baseURI, baseURI2);
				xmlNodeOrder = ((num < 0) ? XmlNodeOrder.Before : ((num > 0) ? XmlNodeOrder.After : XmlNodeOrder.Unknown));
			}
			return xmlNodeOrder;
		}

		// Token: 0x0600113B RID: 4411 RVA: 0x0004D604 File Offset: 0x0004C604
		[Conditional("DEBUG")]
		private void AssertDOD(List<XPathNavigator> buffer, XPathNavigator nav, int pos)
		{
			if (nav.GetType().ToString() == "Microsoft.VisualStudio.Modeling.StoreNavigator")
			{
				return;
			}
			if (nav.GetType().ToString() == "System.Xml.DataDocumentXPathNavigator")
			{
				return;
			}
			if (0 < pos)
			{
				Query.CompareNodes(buffer[pos - 1], nav);
			}
			if (pos < buffer.Count)
			{
				Query.CompareNodes(nav, buffer[pos]);
			}
		}

		// Token: 0x0600113C RID: 4412 RVA: 0x0004D66C File Offset: 0x0004C66C
		[Conditional("DEBUG")]
		public static void AssertQuery(Query query)
		{
			if (query is FunctionQuery)
			{
				return;
			}
			query = Query.Clone(query);
			XPathNavigator xpathNavigator = null;
			int count = query.Clone().Count;
			int num = 0;
			XPathNavigator xpathNavigator2;
			while ((xpathNavigator2 = query.Advance()) != null)
			{
				if (xpathNavigator2.GetType().ToString() == "Microsoft.VisualStudio.Modeling.StoreNavigator")
				{
					return;
				}
				if (xpathNavigator2.GetType().ToString() == "System.Xml.DataDocumentXPathNavigator")
				{
					return;
				}
				if (xpathNavigator != null && (xpathNavigator.NodeType != XPathNodeType.Namespace || xpathNavigator2.NodeType != XPathNodeType.Namespace))
				{
					Query.CompareNodes(xpathNavigator, xpathNavigator2);
				}
				xpathNavigator = xpathNavigator2.Clone();
				num++;
			}
		}

		// Token: 0x0600113D RID: 4413 RVA: 0x0004D6FE File Offset: 0x0004C6FE
		protected XPathResultType GetXPathType(object value)
		{
			if (value is XPathNodeIterator)
			{
				return XPathResultType.NodeSet;
			}
			if (value is string)
			{
				return XPathResultType.String;
			}
			if (value is double)
			{
				return XPathResultType.Number;
			}
			if (value is bool)
			{
				return XPathResultType.Boolean;
			}
			return (XPathResultType)4;
		}

		// Token: 0x0600113E RID: 4414 RVA: 0x0004D729 File Offset: 0x0004C729
		public virtual void PrintQuery(XmlWriter w)
		{
			w.WriteElementString(base.GetType().Name, string.Empty);
		}

		// Token: 0x04000B12 RID: 2834
		public const XPathResultType XPathResultType_Navigator = (XPathResultType)4;
	}
}
