using System;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x02000113 RID: 275
	internal enum StoreTransactionOperationType
	{
		// Token: 0x04000E17 RID: 3607
		Invalid,
		// Token: 0x04000E18 RID: 3608
		SetCanonicalizationContext = 14,
		// Token: 0x04000E19 RID: 3609
		StageComponent = 20,
		// Token: 0x04000E1A RID: 3610
		PinDeployment,
		// Token: 0x04000E1B RID: 3611
		UnpinDeployment,
		// Token: 0x04000E1C RID: 3612
		StageComponentFile,
		// Token: 0x04000E1D RID: 3613
		InstallDeployment,
		// Token: 0x04000E1E RID: 3614
		UninstallDeployment,
		// Token: 0x04000E1F RID: 3615
		SetDeploymentMetadata,
		// Token: 0x04000E20 RID: 3616
		Scavenge
	}
}
