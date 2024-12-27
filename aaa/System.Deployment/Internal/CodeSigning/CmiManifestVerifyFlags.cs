using System;

namespace System.Deployment.Internal.CodeSigning
{
	// Token: 0x020001E4 RID: 484
	[Flags]
	internal enum CmiManifestVerifyFlags
	{
		// Token: 0x0400081F RID: 2079
		None = 0,
		// Token: 0x04000820 RID: 2080
		RevocationNoCheck = 1,
		// Token: 0x04000821 RID: 2081
		RevocationCheckEndCertOnly = 2,
		// Token: 0x04000822 RID: 2082
		RevocationCheckEntireChain = 4,
		// Token: 0x04000823 RID: 2083
		UrlCacheOnlyRetrieval = 8,
		// Token: 0x04000824 RID: 2084
		LifetimeSigning = 16,
		// Token: 0x04000825 RID: 2085
		TrustMicrosoftRootOnly = 32,
		// Token: 0x04000826 RID: 2086
		StrongNameOnly = 65536
	}
}
