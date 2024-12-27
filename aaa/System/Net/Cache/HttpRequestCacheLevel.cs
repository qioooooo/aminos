using System;

namespace System.Net.Cache
{
	// Token: 0x0200056D RID: 1389
	public enum HttpRequestCacheLevel
	{
		// Token: 0x04002916 RID: 10518
		Default,
		// Token: 0x04002917 RID: 10519
		BypassCache,
		// Token: 0x04002918 RID: 10520
		CacheOnly,
		// Token: 0x04002919 RID: 10521
		CacheIfAvailable,
		// Token: 0x0400291A RID: 10522
		Revalidate,
		// Token: 0x0400291B RID: 10523
		Reload,
		// Token: 0x0400291C RID: 10524
		NoCacheNoStore,
		// Token: 0x0400291D RID: 10525
		CacheOrNextCacheOnly,
		// Token: 0x0400291E RID: 10526
		Refresh
	}
}
