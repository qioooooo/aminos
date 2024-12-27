using System;

namespace MS.Internal.Xml.Cache
{
	// Token: 0x0200010C RID: 268
	internal sealed class XPathNodePageInfo
	{
		// Token: 0x06001075 RID: 4213 RVA: 0x0004B33B File Offset: 0x0004A33B
		public XPathNodePageInfo(XPathNode[] pagePrev, int pageNum)
		{
			this.pagePrev = pagePrev;
			this.pageNum = pageNum;
			this.nodeCount = 1;
		}

		// Token: 0x170003F1 RID: 1009
		// (get) Token: 0x06001076 RID: 4214 RVA: 0x0004B358 File Offset: 0x0004A358
		public int PageNumber
		{
			get
			{
				return this.pageNum;
			}
		}

		// Token: 0x170003F2 RID: 1010
		// (get) Token: 0x06001077 RID: 4215 RVA: 0x0004B360 File Offset: 0x0004A360
		// (set) Token: 0x06001078 RID: 4216 RVA: 0x0004B368 File Offset: 0x0004A368
		public int NodeCount
		{
			get
			{
				return this.nodeCount;
			}
			set
			{
				this.nodeCount = value;
			}
		}

		// Token: 0x170003F3 RID: 1011
		// (get) Token: 0x06001079 RID: 4217 RVA: 0x0004B371 File Offset: 0x0004A371
		public XPathNode[] PreviousPage
		{
			get
			{
				return this.pagePrev;
			}
		}

		// Token: 0x170003F4 RID: 1012
		// (get) Token: 0x0600107A RID: 4218 RVA: 0x0004B379 File Offset: 0x0004A379
		// (set) Token: 0x0600107B RID: 4219 RVA: 0x0004B381 File Offset: 0x0004A381
		public XPathNode[] NextPage
		{
			get
			{
				return this.pageNext;
			}
			set
			{
				this.pageNext = value;
			}
		}

		// Token: 0x04000AB2 RID: 2738
		private int pageNum;

		// Token: 0x04000AB3 RID: 2739
		private int nodeCount;

		// Token: 0x04000AB4 RID: 2740
		private XPathNode[] pagePrev;

		// Token: 0x04000AB5 RID: 2741
		private XPathNode[] pageNext;
	}
}
