using System;
using System.Xml.XPath;

namespace MS.Internal.Xml.XPath
{
	// Token: 0x02000170 RID: 368
	internal class XPathSingletonIterator : ResetableIterator
	{
		// Token: 0x060013A8 RID: 5032 RVA: 0x0005554A File Offset: 0x0005454A
		public XPathSingletonIterator(XPathNavigator nav)
		{
			this.nav = nav;
		}

		// Token: 0x060013A9 RID: 5033 RVA: 0x00055559 File Offset: 0x00054559
		public XPathSingletonIterator(XPathNavigator nav, bool moved)
			: this(nav)
		{
			if (moved)
			{
				this.position = 1;
			}
		}

		// Token: 0x060013AA RID: 5034 RVA: 0x0005556C File Offset: 0x0005456C
		public XPathSingletonIterator(XPathSingletonIterator it)
		{
			this.nav = it.nav.Clone();
			this.position = it.position;
		}

		// Token: 0x060013AB RID: 5035 RVA: 0x00055591 File Offset: 0x00054591
		public override XPathNodeIterator Clone()
		{
			return new XPathSingletonIterator(this);
		}

		// Token: 0x170004C6 RID: 1222
		// (get) Token: 0x060013AC RID: 5036 RVA: 0x00055599 File Offset: 0x00054599
		public override XPathNavigator Current
		{
			get
			{
				return this.nav;
			}
		}

		// Token: 0x170004C7 RID: 1223
		// (get) Token: 0x060013AD RID: 5037 RVA: 0x000555A1 File Offset: 0x000545A1
		public override int CurrentPosition
		{
			get
			{
				return this.position;
			}
		}

		// Token: 0x170004C8 RID: 1224
		// (get) Token: 0x060013AE RID: 5038 RVA: 0x000555A9 File Offset: 0x000545A9
		public override int Count
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x060013AF RID: 5039 RVA: 0x000555AC File Offset: 0x000545AC
		public override bool MoveNext()
		{
			if (this.position == 0)
			{
				this.position = 1;
				return true;
			}
			return false;
		}

		// Token: 0x060013B0 RID: 5040 RVA: 0x000555C0 File Offset: 0x000545C0
		public override void Reset()
		{
			this.position = 0;
		}

		// Token: 0x04000C30 RID: 3120
		private XPathNavigator nav;

		// Token: 0x04000C31 RID: 3121
		private int position;
	}
}
