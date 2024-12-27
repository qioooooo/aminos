using System;
using System.Xml.XPath;

namespace MS.Internal.Xml.XPath
{
	// Token: 0x02000120 RID: 288
	internal class ContextQuery : Query
	{
		// Token: 0x0600113F RID: 4415 RVA: 0x0004D741 File Offset: 0x0004C741
		public ContextQuery()
		{
			this.count = 0;
		}

		// Token: 0x06001140 RID: 4416 RVA: 0x0004D750 File Offset: 0x0004C750
		protected ContextQuery(ContextQuery other)
			: base(other)
		{
			this.contextNode = other.contextNode;
		}

		// Token: 0x06001141 RID: 4417 RVA: 0x0004D765 File Offset: 0x0004C765
		public override void Reset()
		{
			this.count = 0;
		}

		// Token: 0x17000436 RID: 1078
		// (get) Token: 0x06001142 RID: 4418 RVA: 0x0004D76E File Offset: 0x0004C76E
		public override XPathNavigator Current
		{
			get
			{
				return this.contextNode;
			}
		}

		// Token: 0x06001143 RID: 4419 RVA: 0x0004D776 File Offset: 0x0004C776
		public override object Evaluate(XPathNodeIterator context)
		{
			this.contextNode = context.Current;
			this.count = 0;
			return this;
		}

		// Token: 0x06001144 RID: 4420 RVA: 0x0004D78C File Offset: 0x0004C78C
		public override XPathNavigator Advance()
		{
			if (this.count == 0)
			{
				this.count = 1;
				return this.contextNode;
			}
			return null;
		}

		// Token: 0x06001145 RID: 4421 RVA: 0x0004D7A5 File Offset: 0x0004C7A5
		public override XPathNavigator MatchNode(XPathNavigator current)
		{
			return current;
		}

		// Token: 0x06001146 RID: 4422 RVA: 0x0004D7A8 File Offset: 0x0004C7A8
		public override XPathNodeIterator Clone()
		{
			return new ContextQuery(this);
		}

		// Token: 0x17000437 RID: 1079
		// (get) Token: 0x06001147 RID: 4423 RVA: 0x0004D7B0 File Offset: 0x0004C7B0
		public override XPathResultType StaticType
		{
			get
			{
				return XPathResultType.NodeSet;
			}
		}

		// Token: 0x17000438 RID: 1080
		// (get) Token: 0x06001148 RID: 4424 RVA: 0x0004D7B3 File Offset: 0x0004C7B3
		public override int CurrentPosition
		{
			get
			{
				return this.count;
			}
		}

		// Token: 0x17000439 RID: 1081
		// (get) Token: 0x06001149 RID: 4425 RVA: 0x0004D7BB File Offset: 0x0004C7BB
		public override int Count
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x1700043A RID: 1082
		// (get) Token: 0x0600114A RID: 4426 RVA: 0x0004D7BE File Offset: 0x0004C7BE
		public override QueryProps Properties
		{
			get
			{
				return (QueryProps)23;
			}
		}

		// Token: 0x04000B13 RID: 2835
		protected XPathNavigator contextNode;
	}
}
