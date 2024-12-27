using System;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace MS.Internal.Xml.XPath
{
	// Token: 0x02000159 RID: 345
	internal sealed class UnionExpr : Query
	{
		// Token: 0x060012D3 RID: 4819 RVA: 0x00052104 File Offset: 0x00051104
		public UnionExpr(Query query1, Query query2)
		{
			this.qy1 = query1;
			this.qy2 = query2;
			this.advance1 = true;
			this.advance2 = true;
		}

		// Token: 0x060012D4 RID: 4820 RVA: 0x00052128 File Offset: 0x00051128
		private UnionExpr(UnionExpr other)
			: base(other)
		{
			this.qy1 = Query.Clone(other.qy1);
			this.qy2 = Query.Clone(other.qy2);
			this.advance1 = other.advance1;
			this.advance2 = other.advance2;
			this.currentNode = Query.Clone(other.currentNode);
			this.nextNode = Query.Clone(other.nextNode);
		}

		// Token: 0x060012D5 RID: 4821 RVA: 0x00052198 File Offset: 0x00051198
		public override void Reset()
		{
			this.qy1.Reset();
			this.qy2.Reset();
			this.advance1 = true;
			this.advance2 = true;
			this.nextNode = null;
		}

		// Token: 0x060012D6 RID: 4822 RVA: 0x000521C5 File Offset: 0x000511C5
		public override void SetXsltContext(XsltContext xsltContext)
		{
			this.qy1.SetXsltContext(xsltContext);
			this.qy2.SetXsltContext(xsltContext);
		}

		// Token: 0x060012D7 RID: 4823 RVA: 0x000521DF File Offset: 0x000511DF
		public override object Evaluate(XPathNodeIterator context)
		{
			this.qy1.Evaluate(context);
			this.qy2.Evaluate(context);
			this.advance1 = true;
			this.advance2 = true;
			this.nextNode = null;
			base.ResetCount();
			return this;
		}

		// Token: 0x060012D8 RID: 4824 RVA: 0x00052218 File Offset: 0x00051218
		private XPathNavigator ProcessSamePosition(XPathNavigator result)
		{
			this.currentNode = result;
			this.advance1 = (this.advance2 = true);
			return result;
		}

		// Token: 0x060012D9 RID: 4825 RVA: 0x0005223D File Offset: 0x0005123D
		private XPathNavigator ProcessBeforePosition(XPathNavigator res1, XPathNavigator res2)
		{
			this.nextNode = res2;
			this.advance2 = false;
			this.advance1 = true;
			this.currentNode = res1;
			return res1;
		}

		// Token: 0x060012DA RID: 4826 RVA: 0x0005225C File Offset: 0x0005125C
		private XPathNavigator ProcessAfterPosition(XPathNavigator res1, XPathNavigator res2)
		{
			this.nextNode = res1;
			this.advance1 = false;
			this.advance2 = true;
			this.currentNode = res2;
			return res2;
		}

		// Token: 0x060012DB RID: 4827 RVA: 0x0005227C File Offset: 0x0005127C
		public override XPathNavigator Advance()
		{
			XPathNavigator xpathNavigator;
			if (this.advance1)
			{
				xpathNavigator = this.qy1.Advance();
			}
			else
			{
				xpathNavigator = this.nextNode;
			}
			XPathNavigator xpathNavigator2;
			if (this.advance2)
			{
				xpathNavigator2 = this.qy2.Advance();
			}
			else
			{
				xpathNavigator2 = this.nextNode;
			}
			if (xpathNavigator != null && xpathNavigator2 != null)
			{
				XmlNodeOrder xmlNodeOrder = Query.CompareNodes(xpathNavigator, xpathNavigator2);
				if (xmlNodeOrder == XmlNodeOrder.Before)
				{
					return this.ProcessBeforePosition(xpathNavigator, xpathNavigator2);
				}
				if (xmlNodeOrder == XmlNodeOrder.After)
				{
					return this.ProcessAfterPosition(xpathNavigator, xpathNavigator2);
				}
				return this.ProcessSamePosition(xpathNavigator);
			}
			else
			{
				if (xpathNavigator2 == null)
				{
					this.advance1 = true;
					this.advance2 = false;
					this.currentNode = xpathNavigator;
					this.nextNode = null;
					return xpathNavigator;
				}
				this.advance1 = false;
				this.advance2 = true;
				this.currentNode = xpathNavigator2;
				this.nextNode = null;
				return xpathNavigator2;
			}
		}

		// Token: 0x060012DC RID: 4828 RVA: 0x00052334 File Offset: 0x00051334
		public override XPathNavigator MatchNode(XPathNavigator xsltContext)
		{
			if (xsltContext == null)
			{
				return null;
			}
			XPathNavigator xpathNavigator = this.qy1.MatchNode(xsltContext);
			if (xpathNavigator != null)
			{
				return xpathNavigator;
			}
			return this.qy2.MatchNode(xsltContext);
		}

		// Token: 0x17000491 RID: 1169
		// (get) Token: 0x060012DD RID: 4829 RVA: 0x00052364 File Offset: 0x00051364
		public override XPathResultType StaticType
		{
			get
			{
				return XPathResultType.NodeSet;
			}
		}

		// Token: 0x060012DE RID: 4830 RVA: 0x00052367 File Offset: 0x00051367
		public override XPathNodeIterator Clone()
		{
			return new UnionExpr(this);
		}

		// Token: 0x17000492 RID: 1170
		// (get) Token: 0x060012DF RID: 4831 RVA: 0x0005236F File Offset: 0x0005136F
		public override XPathNavigator Current
		{
			get
			{
				return this.currentNode;
			}
		}

		// Token: 0x17000493 RID: 1171
		// (get) Token: 0x060012E0 RID: 4832 RVA: 0x00052377 File Offset: 0x00051377
		public override int CurrentPosition
		{
			get
			{
				throw new InvalidOperationException();
			}
		}

		// Token: 0x060012E1 RID: 4833 RVA: 0x00052380 File Offset: 0x00051380
		public override void PrintQuery(XmlWriter w)
		{
			w.WriteStartElement(base.GetType().Name);
			if (this.qy1 != null)
			{
				this.qy1.PrintQuery(w);
			}
			if (this.qy2 != null)
			{
				this.qy2.PrintQuery(w);
			}
			w.WriteEndElement();
		}

		// Token: 0x04000BCD RID: 3021
		internal Query qy1;

		// Token: 0x04000BCE RID: 3022
		internal Query qy2;

		// Token: 0x04000BCF RID: 3023
		private bool advance1;

		// Token: 0x04000BD0 RID: 3024
		private bool advance2;

		// Token: 0x04000BD1 RID: 3025
		private XPathNavigator currentNode;

		// Token: 0x04000BD2 RID: 3026
		private XPathNavigator nextNode;
	}
}
