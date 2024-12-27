using System;
using System.Xml.XPath;

namespace MS.Internal.Xml.XPath
{
	// Token: 0x0200016E RID: 366
	internal class XPathSelectionIterator : ResetableIterator
	{
		// Token: 0x0600139C RID: 5020 RVA: 0x00055419 File Offset: 0x00054419
		internal XPathSelectionIterator(XPathNavigator nav, Query query)
		{
			this.nav = nav.Clone();
			this.query = query;
		}

		// Token: 0x0600139D RID: 5021 RVA: 0x00055434 File Offset: 0x00054434
		protected XPathSelectionIterator(XPathSelectionIterator it)
		{
			this.nav = it.nav.Clone();
			this.query = (Query)it.query.Clone();
			this.position = it.position;
		}

		// Token: 0x0600139E RID: 5022 RVA: 0x0005546F File Offset: 0x0005446F
		public override void Reset()
		{
			this.query.Reset();
		}

		// Token: 0x0600139F RID: 5023 RVA: 0x0005547C File Offset: 0x0005447C
		public override bool MoveNext()
		{
			XPathNavigator xpathNavigator = this.query.Advance();
			if (xpathNavigator != null)
			{
				this.position++;
				if (!this.nav.MoveTo(xpathNavigator))
				{
					this.nav = xpathNavigator.Clone();
				}
				return true;
			}
			return false;
		}

		// Token: 0x170004C3 RID: 1219
		// (get) Token: 0x060013A0 RID: 5024 RVA: 0x000554C3 File Offset: 0x000544C3
		public override int Count
		{
			get
			{
				return this.query.Count;
			}
		}

		// Token: 0x170004C4 RID: 1220
		// (get) Token: 0x060013A1 RID: 5025 RVA: 0x000554D0 File Offset: 0x000544D0
		public override XPathNavigator Current
		{
			get
			{
				return this.nav;
			}
		}

		// Token: 0x170004C5 RID: 1221
		// (get) Token: 0x060013A2 RID: 5026 RVA: 0x000554D8 File Offset: 0x000544D8
		public override int CurrentPosition
		{
			get
			{
				return this.position;
			}
		}

		// Token: 0x060013A3 RID: 5027 RVA: 0x000554E0 File Offset: 0x000544E0
		public override XPathNodeIterator Clone()
		{
			return new XPathSelectionIterator(this);
		}

		// Token: 0x04000C2D RID: 3117
		private XPathNavigator nav;

		// Token: 0x04000C2E RID: 3118
		private Query query;

		// Token: 0x04000C2F RID: 3119
		private int position;
	}
}
