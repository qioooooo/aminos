using System;

namespace System.Net.Cache
{
	// Token: 0x02000571 RID: 1393
	internal enum CacheValidationStatus
	{
		// Token: 0x04002931 RID: 10545
		DoNotUseCache,
		// Token: 0x04002932 RID: 10546
		Fail,
		// Token: 0x04002933 RID: 10547
		DoNotTakeFromCache,
		// Token: 0x04002934 RID: 10548
		RetryResponseFromCache,
		// Token: 0x04002935 RID: 10549
		RetryResponseFromServer,
		// Token: 0x04002936 RID: 10550
		ReturnCachedResponse,
		// Token: 0x04002937 RID: 10551
		CombineCachedAndServerResponse,
		// Token: 0x04002938 RID: 10552
		CacheResponse,
		// Token: 0x04002939 RID: 10553
		UpdateResponseInformation,
		// Token: 0x0400293A RID: 10554
		RemoveFromCache,
		// Token: 0x0400293B RID: 10555
		DoNotUpdateCache,
		// Token: 0x0400293C RID: 10556
		Continue
	}
}
