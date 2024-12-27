using System;

namespace System.Reflection.Cache
{
	// Token: 0x02000843 RID: 2115
	[Serializable]
	internal enum CacheAction
	{
		// Token: 0x04002819 RID: 10265
		AllocateCache = 1,
		// Token: 0x0400281A RID: 10266
		AddItem,
		// Token: 0x0400281B RID: 10267
		ClearCache,
		// Token: 0x0400281C RID: 10268
		LookupItemHit,
		// Token: 0x0400281D RID: 10269
		LookupItemMiss,
		// Token: 0x0400281E RID: 10270
		GrowCache,
		// Token: 0x0400281F RID: 10271
		SetItemReplace,
		// Token: 0x04002820 RID: 10272
		ReplaceFailed
	}
}
