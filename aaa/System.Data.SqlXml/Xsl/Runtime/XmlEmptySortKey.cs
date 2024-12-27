using System;

namespace System.Xml.Xsl.Runtime
{
	// Token: 0x020000C1 RID: 193
	internal class XmlEmptySortKey : XmlSortKey
	{
		// Token: 0x0600095D RID: 2397 RVA: 0x0002C2F3 File Offset: 0x0002B2F3
		public XmlEmptySortKey(XmlCollation collation)
		{
			this.isEmptyGreatest = collation.EmptyGreatest != collation.DescendingOrder;
		}

		// Token: 0x17000165 RID: 357
		// (get) Token: 0x0600095E RID: 2398 RVA: 0x0002C312 File Offset: 0x0002B312
		public bool IsEmptyGreatest
		{
			get
			{
				return this.isEmptyGreatest;
			}
		}

		// Token: 0x0600095F RID: 2399 RVA: 0x0002C31C File Offset: 0x0002B31C
		public override int CompareTo(object obj)
		{
			XmlEmptySortKey xmlEmptySortKey = obj as XmlEmptySortKey;
			if (xmlEmptySortKey == null)
			{
				return -(obj as XmlSortKey).CompareTo(this);
			}
			return base.BreakSortingTie(xmlEmptySortKey);
		}

		// Token: 0x040005C1 RID: 1473
		private bool isEmptyGreatest;
	}
}
