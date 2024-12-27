using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace MS.Internal.Xml.XPath
{
	// Token: 0x0200015C RID: 348
	internal sealed class SortQuery : Query
	{
		// Token: 0x060012EA RID: 4842 RVA: 0x00052414 File Offset: 0x00051414
		public SortQuery(Query qyInput)
		{
			this.results = new List<SortKey>();
			this.comparer = new XPathSortComparer();
			this.qyInput = qyInput;
			this.count = 0;
		}

		// Token: 0x060012EB RID: 4843 RVA: 0x00052440 File Offset: 0x00051440
		private SortQuery(SortQuery other)
			: base(other)
		{
			this.results = new List<SortKey>(other.results);
			this.comparer = other.comparer.Clone();
			this.qyInput = Query.Clone(other.qyInput);
			this.count = 0;
		}

		// Token: 0x060012EC RID: 4844 RVA: 0x0005248E File Offset: 0x0005148E
		public override void Reset()
		{
			this.count = 0;
		}

		// Token: 0x060012ED RID: 4845 RVA: 0x00052497 File Offset: 0x00051497
		public override void SetXsltContext(XsltContext xsltContext)
		{
			this.qyInput.SetXsltContext(xsltContext);
			if (this.qyInput.StaticType != XPathResultType.NodeSet && this.qyInput.StaticType != XPathResultType.Any)
			{
				throw XPathException.Create("Xp_NodeSetExpected");
			}
		}

		// Token: 0x060012EE RID: 4846 RVA: 0x000524CC File Offset: 0x000514CC
		private void BuildResultsList()
		{
			int numSorts = this.comparer.NumSorts;
			XPathNavigator xpathNavigator;
			while ((xpathNavigator = this.qyInput.Advance()) != null)
			{
				SortKey sortKey = new SortKey(numSorts, this.results.Count, xpathNavigator.Clone());
				for (int i = 0; i < numSorts; i++)
				{
					sortKey[i] = this.comparer.Expression(i).Evaluate(this.qyInput);
				}
				this.results.Add(sortKey);
			}
			this.results.Sort(this.comparer);
		}

		// Token: 0x060012EF RID: 4847 RVA: 0x00052555 File Offset: 0x00051555
		public override object Evaluate(XPathNodeIterator context)
		{
			this.qyInput.Evaluate(context);
			this.results.Clear();
			this.BuildResultsList();
			this.count = 0;
			return this;
		}

		// Token: 0x060012F0 RID: 4848 RVA: 0x00052580 File Offset: 0x00051580
		public override XPathNavigator Advance()
		{
			if (this.count < this.results.Count)
			{
				return this.results[this.count++].Node;
			}
			return null;
		}

		// Token: 0x17000498 RID: 1176
		// (get) Token: 0x060012F1 RID: 4849 RVA: 0x000525C3 File Offset: 0x000515C3
		public override XPathNavigator Current
		{
			get
			{
				if (this.count == 0)
				{
					return null;
				}
				return this.results[this.count - 1].Node;
			}
		}

		// Token: 0x060012F2 RID: 4850 RVA: 0x000525E7 File Offset: 0x000515E7
		internal void AddSort(Query evalQuery, IComparer comparer)
		{
			this.comparer.AddSort(evalQuery, comparer);
		}

		// Token: 0x060012F3 RID: 4851 RVA: 0x000525F6 File Offset: 0x000515F6
		public override XPathNodeIterator Clone()
		{
			return new SortQuery(this);
		}

		// Token: 0x17000499 RID: 1177
		// (get) Token: 0x060012F4 RID: 4852 RVA: 0x000525FE File Offset: 0x000515FE
		public override XPathResultType StaticType
		{
			get
			{
				return XPathResultType.NodeSet;
			}
		}

		// Token: 0x1700049A RID: 1178
		// (get) Token: 0x060012F5 RID: 4853 RVA: 0x00052601 File Offset: 0x00051601
		public override int CurrentPosition
		{
			get
			{
				return this.count;
			}
		}

		// Token: 0x1700049B RID: 1179
		// (get) Token: 0x060012F6 RID: 4854 RVA: 0x00052609 File Offset: 0x00051609
		public override int Count
		{
			get
			{
				return this.results.Count;
			}
		}

		// Token: 0x1700049C RID: 1180
		// (get) Token: 0x060012F7 RID: 4855 RVA: 0x00052616 File Offset: 0x00051616
		public override QueryProps Properties
		{
			get
			{
				return (QueryProps)7;
			}
		}

		// Token: 0x060012F8 RID: 4856 RVA: 0x00052619 File Offset: 0x00051619
		public override void PrintQuery(XmlWriter w)
		{
			w.WriteStartElement(base.GetType().Name);
			this.qyInput.PrintQuery(w);
			w.WriteElementString("XPathSortComparer", "... PrintTree() not implemented ...");
			w.WriteEndElement();
		}

		// Token: 0x04000BD3 RID: 3027
		private List<SortKey> results;

		// Token: 0x04000BD4 RID: 3028
		private XPathSortComparer comparer;

		// Token: 0x04000BD5 RID: 3029
		private Query qyInput;
	}
}
