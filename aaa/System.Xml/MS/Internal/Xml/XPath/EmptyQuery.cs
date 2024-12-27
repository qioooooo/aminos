using System;
using System.Xml.XPath;

namespace MS.Internal.Xml.XPath
{
	// Token: 0x02000138 RID: 312
	internal sealed class EmptyQuery : Query
	{
		// Token: 0x060011EB RID: 4587 RVA: 0x0004EE03 File Offset: 0x0004DE03
		public override XPathNavigator Advance()
		{
			return null;
		}

		// Token: 0x060011EC RID: 4588 RVA: 0x0004EE06 File Offset: 0x0004DE06
		public override XPathNodeIterator Clone()
		{
			return this;
		}

		// Token: 0x060011ED RID: 4589 RVA: 0x0004EE09 File Offset: 0x0004DE09
		public override object Evaluate(XPathNodeIterator context)
		{
			return this;
		}

		// Token: 0x17000463 RID: 1123
		// (get) Token: 0x060011EE RID: 4590 RVA: 0x0004EE0C File Offset: 0x0004DE0C
		public override int CurrentPosition
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x17000464 RID: 1124
		// (get) Token: 0x060011EF RID: 4591 RVA: 0x0004EE0F File Offset: 0x0004DE0F
		public override int Count
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x17000465 RID: 1125
		// (get) Token: 0x060011F0 RID: 4592 RVA: 0x0004EE12 File Offset: 0x0004DE12
		public override QueryProps Properties
		{
			get
			{
				return (QueryProps)23;
			}
		}

		// Token: 0x17000466 RID: 1126
		// (get) Token: 0x060011F1 RID: 4593 RVA: 0x0004EE16 File Offset: 0x0004DE16
		public override XPathResultType StaticType
		{
			get
			{
				return XPathResultType.NodeSet;
			}
		}

		// Token: 0x060011F2 RID: 4594 RVA: 0x0004EE19 File Offset: 0x0004DE19
		public override void Reset()
		{
		}

		// Token: 0x17000467 RID: 1127
		// (get) Token: 0x060011F3 RID: 4595 RVA: 0x0004EE1B File Offset: 0x0004DE1B
		public override XPathNavigator Current
		{
			get
			{
				return null;
			}
		}
	}
}
