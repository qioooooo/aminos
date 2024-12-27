using System;

namespace System.Threading
{
	// Token: 0x02000156 RID: 342
	[Serializable]
	internal enum StackCrawlMark
	{
		// Token: 0x0400065A RID: 1626
		LookForMe,
		// Token: 0x0400065B RID: 1627
		LookForMyCaller,
		// Token: 0x0400065C RID: 1628
		LookForMyCallersCaller,
		// Token: 0x0400065D RID: 1629
		LookForThread
	}
}
