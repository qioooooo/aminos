using System;
using System.Xml.XPath;

namespace MS.Internal.Xml.XPath
{
	// Token: 0x02000168 RID: 360
	internal sealed class XPathEmptyIterator : ResetableIterator
	{
		// Token: 0x0600134F RID: 4943 RVA: 0x00053774 File Offset: 0x00052774
		private XPathEmptyIterator()
		{
		}

		// Token: 0x06001350 RID: 4944 RVA: 0x0005377C File Offset: 0x0005277C
		public override XPathNodeIterator Clone()
		{
			return this;
		}

		// Token: 0x170004B2 RID: 1202
		// (get) Token: 0x06001351 RID: 4945 RVA: 0x0005377F File Offset: 0x0005277F
		public override XPathNavigator Current
		{
			get
			{
				return null;
			}
		}

		// Token: 0x170004B3 RID: 1203
		// (get) Token: 0x06001352 RID: 4946 RVA: 0x00053782 File Offset: 0x00052782
		public override int CurrentPosition
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x170004B4 RID: 1204
		// (get) Token: 0x06001353 RID: 4947 RVA: 0x00053785 File Offset: 0x00052785
		public override int Count
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x06001354 RID: 4948 RVA: 0x00053788 File Offset: 0x00052788
		public override bool MoveNext()
		{
			return false;
		}

		// Token: 0x06001355 RID: 4949 RVA: 0x0005378B File Offset: 0x0005278B
		public override void Reset()
		{
		}

		// Token: 0x04000BEF RID: 3055
		public static XPathEmptyIterator Instance = new XPathEmptyIterator();
	}
}
