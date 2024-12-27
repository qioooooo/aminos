using System;

namespace System.Net.Cache
{
	// Token: 0x0200056B RID: 1387
	public enum RequestCacheLevel
	{
		// Token: 0x0400290D RID: 10509
		Default,
		// Token: 0x0400290E RID: 10510
		BypassCache,
		// Token: 0x0400290F RID: 10511
		CacheOnly,
		// Token: 0x04002910 RID: 10512
		CacheIfAvailable,
		// Token: 0x04002911 RID: 10513
		Revalidate,
		// Token: 0x04002912 RID: 10514
		Reload,
		// Token: 0x04002913 RID: 10515
		NoCacheNoStore
	}
}
