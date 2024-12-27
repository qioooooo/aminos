using System;

namespace System.DirectoryServices.ActiveDirectory
{
	// Token: 0x020000AB RID: 171
	public enum ForestTrustDomainStatus
	{
		// Token: 0x04000465 RID: 1125
		Enabled,
		// Token: 0x04000466 RID: 1126
		SidAdminDisabled,
		// Token: 0x04000467 RID: 1127
		SidConflictDisabled,
		// Token: 0x04000468 RID: 1128
		NetBiosNameAdminDisabled = 4,
		// Token: 0x04000469 RID: 1129
		NetBiosNameConflictDisabled = 8
	}
}
