using System;
using System.Xml.XPath;

namespace MS.Internal.Xml.XPath
{
	// Token: 0x02000128 RID: 296
	internal abstract class ValueQuery : Query
	{
		// Token: 0x06001177 RID: 4471 RVA: 0x0004DCE0 File Offset: 0x0004CCE0
		public ValueQuery()
		{
		}

		// Token: 0x06001178 RID: 4472 RVA: 0x0004DCE8 File Offset: 0x0004CCE8
		protected ValueQuery(ValueQuery other)
			: base(other)
		{
		}

		// Token: 0x06001179 RID: 4473 RVA: 0x0004DCF1 File Offset: 0x0004CCF1
		public sealed override void Reset()
		{
		}

		// Token: 0x1700044F RID: 1103
		// (get) Token: 0x0600117A RID: 4474 RVA: 0x0004DCF3 File Offset: 0x0004CCF3
		public sealed override XPathNavigator Current
		{
			get
			{
				throw XPathException.Create("Xp_NodeSetExpected");
			}
		}

		// Token: 0x17000450 RID: 1104
		// (get) Token: 0x0600117B RID: 4475 RVA: 0x0004DCFF File Offset: 0x0004CCFF
		public sealed override int CurrentPosition
		{
			get
			{
				throw XPathException.Create("Xp_NodeSetExpected");
			}
		}

		// Token: 0x17000451 RID: 1105
		// (get) Token: 0x0600117C RID: 4476 RVA: 0x0004DD0B File Offset: 0x0004CD0B
		public sealed override int Count
		{
			get
			{
				throw XPathException.Create("Xp_NodeSetExpected");
			}
		}

		// Token: 0x0600117D RID: 4477 RVA: 0x0004DD17 File Offset: 0x0004CD17
		public sealed override XPathNavigator Advance()
		{
			throw XPathException.Create("Xp_NodeSetExpected");
		}
	}
}
