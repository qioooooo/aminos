using System;

namespace System.EnterpriseServices
{
	// Token: 0x0200007D RID: 125
	[Flags]
	[Serializable]
	public enum InstallationFlags
	{
		// Token: 0x0400011D RID: 285
		Default = 0,
		// Token: 0x0400011E RID: 286
		ExpectExistingTypeLib = 1,
		// Token: 0x0400011F RID: 287
		CreateTargetApplication = 2,
		// Token: 0x04000120 RID: 288
		FindOrCreateTargetApplication = 4,
		// Token: 0x04000121 RID: 289
		ReconfigureExistingApplication = 8,
		// Token: 0x04000122 RID: 290
		ConfigureComponentsOnly = 16,
		// Token: 0x04000123 RID: 291
		ReportWarningsToConsole = 32,
		// Token: 0x04000124 RID: 292
		Register = 256,
		// Token: 0x04000125 RID: 293
		Install = 512,
		// Token: 0x04000126 RID: 294
		Configure = 1024
	}
}
