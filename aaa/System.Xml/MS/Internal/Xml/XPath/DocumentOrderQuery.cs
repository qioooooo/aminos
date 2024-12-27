using System;
using System.Xml.XPath;

namespace MS.Internal.Xml.XPath
{
	// Token: 0x02000137 RID: 311
	internal sealed class DocumentOrderQuery : CacheOutputQuery
	{
		// Token: 0x060011E6 RID: 4582 RVA: 0x0004EDA3 File Offset: 0x0004DDA3
		public DocumentOrderQuery(Query qyParent)
			: base(qyParent)
		{
		}

		// Token: 0x060011E7 RID: 4583 RVA: 0x0004EDAC File Offset: 0x0004DDAC
		private DocumentOrderQuery(DocumentOrderQuery other)
			: base(other)
		{
		}

		// Token: 0x060011E8 RID: 4584 RVA: 0x0004EDB8 File Offset: 0x0004DDB8
		public override object Evaluate(XPathNodeIterator context)
		{
			base.Evaluate(context);
			XPathNavigator xpathNavigator;
			while ((xpathNavigator = this.input.Advance()) != null)
			{
				base.Insert(this.outputBuffer, xpathNavigator);
			}
			return this;
		}

		// Token: 0x060011E9 RID: 4585 RVA: 0x0004EDED File Offset: 0x0004DDED
		public override XPathNavigator MatchNode(XPathNavigator context)
		{
			return this.input.MatchNode(context);
		}

		// Token: 0x060011EA RID: 4586 RVA: 0x0004EDFB File Offset: 0x0004DDFB
		public override XPathNodeIterator Clone()
		{
			return new DocumentOrderQuery(this);
		}
	}
}
