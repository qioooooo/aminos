using System;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace MS.Internal.Xml.XPath
{
	// Token: 0x0200014A RID: 330
	internal sealed class MergeFilterQuery : CacheOutputQuery
	{
		// Token: 0x06001279 RID: 4729 RVA: 0x000509D2 File Offset: 0x0004F9D2
		public MergeFilterQuery(Query input, Query child)
			: base(input)
		{
			this.child = child;
		}

		// Token: 0x0600127A RID: 4730 RVA: 0x000509E2 File Offset: 0x0004F9E2
		private MergeFilterQuery(MergeFilterQuery other)
			: base(other)
		{
			this.child = Query.Clone(other.child);
		}

		// Token: 0x0600127B RID: 4731 RVA: 0x000509FC File Offset: 0x0004F9FC
		public override void SetXsltContext(XsltContext xsltContext)
		{
			base.SetXsltContext(xsltContext);
			this.child.SetXsltContext(xsltContext);
		}

		// Token: 0x0600127C RID: 4732 RVA: 0x00050A14 File Offset: 0x0004FA14
		public override object Evaluate(XPathNodeIterator nodeIterator)
		{
			base.Evaluate(nodeIterator);
			while (this.input.Advance() != null)
			{
				this.child.Evaluate(this.input);
				XPathNavigator xpathNavigator;
				while ((xpathNavigator = this.child.Advance()) != null)
				{
					base.Insert(this.outputBuffer, xpathNavigator);
				}
			}
			return this;
		}

		// Token: 0x0600127D RID: 4733 RVA: 0x00050A6C File Offset: 0x0004FA6C
		public override XPathNavigator MatchNode(XPathNavigator current)
		{
			XPathNavigator xpathNavigator = this.child.MatchNode(current);
			if (xpathNavigator == null)
			{
				return null;
			}
			xpathNavigator = this.input.MatchNode(xpathNavigator);
			if (xpathNavigator == null)
			{
				return null;
			}
			this.Evaluate(new XPathSingletonIterator(xpathNavigator.Clone(), true));
			for (XPathNavigator xpathNavigator2 = this.Advance(); xpathNavigator2 != null; xpathNavigator2 = this.Advance())
			{
				if (xpathNavigator2.IsSamePosition(current))
				{
					return xpathNavigator;
				}
			}
			return null;
		}

		// Token: 0x0600127E RID: 4734 RVA: 0x00050ACF File Offset: 0x0004FACF
		public override XPathNodeIterator Clone()
		{
			return new MergeFilterQuery(this);
		}

		// Token: 0x0600127F RID: 4735 RVA: 0x00050AD7 File Offset: 0x0004FAD7
		public override void PrintQuery(XmlWriter w)
		{
			w.WriteStartElement(base.GetType().Name);
			this.input.PrintQuery(w);
			this.child.PrintQuery(w);
			w.WriteEndElement();
		}

		// Token: 0x04000B9A RID: 2970
		private Query child;
	}
}
