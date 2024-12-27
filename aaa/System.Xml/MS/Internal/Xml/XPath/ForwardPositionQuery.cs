using System;
using System.Xml.XPath;

namespace MS.Internal.Xml.XPath
{
	// Token: 0x0200013F RID: 319
	internal class ForwardPositionQuery : CacheOutputQuery
	{
		// Token: 0x06001223 RID: 4643 RVA: 0x0004FB6B File Offset: 0x0004EB6B
		public ForwardPositionQuery(Query input)
			: base(input)
		{
		}

		// Token: 0x06001224 RID: 4644 RVA: 0x0004FB74 File Offset: 0x0004EB74
		protected ForwardPositionQuery(ForwardPositionQuery other)
			: base(other)
		{
		}

		// Token: 0x06001225 RID: 4645 RVA: 0x0004FB80 File Offset: 0x0004EB80
		public override object Evaluate(XPathNodeIterator context)
		{
			base.Evaluate(context);
			XPathNavigator xpathNavigator;
			while ((xpathNavigator = this.input.Advance()) != null)
			{
				this.outputBuffer.Add(xpathNavigator.Clone());
			}
			return this;
		}

		// Token: 0x06001226 RID: 4646 RVA: 0x0004FBB8 File Offset: 0x0004EBB8
		public override XPathNavigator MatchNode(XPathNavigator context)
		{
			return this.input.MatchNode(context);
		}

		// Token: 0x06001227 RID: 4647 RVA: 0x0004FBC6 File Offset: 0x0004EBC6
		public override XPathNodeIterator Clone()
		{
			return new ForwardPositionQuery(this);
		}
	}
}
