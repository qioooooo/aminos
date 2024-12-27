using System;

namespace System.Deployment.Internal.CodeSigning
{
	// Token: 0x0200013C RID: 316
	[Flags]
	internal enum CmiManifestVerifyFlags
	{
		// Token: 0x04000EC2 RID: 3778
		None = 0,
		// Token: 0x04000EC3 RID: 3779
		RevocationNoCheck = 1,
		// Token: 0x04000EC4 RID: 3780
		RevocationCheckEndCertOnly = 2,
		// Token: 0x04000EC5 RID: 3781
		RevocationCheckEntireChain = 4,
		// Token: 0x04000EC6 RID: 3782
		UrlCacheOnlyRetrieval = 8,
		// Token: 0x04000EC7 RID: 3783
		LifetimeSigning = 16,
		// Token: 0x04000EC8 RID: 3784
		TrustMicrosoftRootOnly = 32,
		// Token: 0x04000EC9 RID: 3785
		StrongNameOnly = 65536
	}
}
