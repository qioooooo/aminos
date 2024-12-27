using System;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x02000218 RID: 536
	internal enum StoreTransactionOperationType
	{
		// Token: 0x040008A3 RID: 2211
		Invalid,
		// Token: 0x040008A4 RID: 2212
		SetCanonicalizationContext = 14,
		// Token: 0x040008A5 RID: 2213
		StageComponent = 20,
		// Token: 0x040008A6 RID: 2214
		PinDeployment,
		// Token: 0x040008A7 RID: 2215
		UnpinDeployment,
		// Token: 0x040008A8 RID: 2216
		StageComponentFile,
		// Token: 0x040008A9 RID: 2217
		InstallDeployment,
		// Token: 0x040008AA RID: 2218
		UninstallDeployment,
		// Token: 0x040008AB RID: 2219
		SetDeploymentMetadata,
		// Token: 0x040008AC RID: 2220
		Scavenge
	}
}
