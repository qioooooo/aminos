using System;

namespace System.Web.UI
{
	// Token: 0x02000439 RID: 1081
	[Flags]
	internal enum OutputCacheParameter
	{
		// Token: 0x04002444 RID: 9284
		CacheProfile = 1,
		// Token: 0x04002445 RID: 9285
		Duration = 2,
		// Token: 0x04002446 RID: 9286
		Enabled = 4,
		// Token: 0x04002447 RID: 9287
		Location = 8,
		// Token: 0x04002448 RID: 9288
		NoStore = 16,
		// Token: 0x04002449 RID: 9289
		SqlDependency = 32,
		// Token: 0x0400244A RID: 9290
		VaryByControl = 64,
		// Token: 0x0400244B RID: 9291
		VaryByCustom = 128,
		// Token: 0x0400244C RID: 9292
		VaryByHeader = 256,
		// Token: 0x0400244D RID: 9293
		VaryByParam = 512,
		// Token: 0x0400244E RID: 9294
		VaryByContentEncoding = 1024
	}
}
