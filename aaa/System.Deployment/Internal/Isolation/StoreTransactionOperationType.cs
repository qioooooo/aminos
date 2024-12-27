using System;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x0200013E RID: 318
	internal enum StoreTransactionOperationType
	{
		// Token: 0x0400058B RID: 1419
		Invalid,
		// Token: 0x0400058C RID: 1420
		SetCanonicalizationContext = 14,
		// Token: 0x0400058D RID: 1421
		StageComponent = 20,
		// Token: 0x0400058E RID: 1422
		PinDeployment,
		// Token: 0x0400058F RID: 1423
		UnpinDeployment,
		// Token: 0x04000590 RID: 1424
		StageComponentFile,
		// Token: 0x04000591 RID: 1425
		InstallDeployment,
		// Token: 0x04000592 RID: 1426
		UninstallDeployment,
		// Token: 0x04000593 RID: 1427
		SetDeploymentMetadata,
		// Token: 0x04000594 RID: 1428
		Scavenge
	}
}
