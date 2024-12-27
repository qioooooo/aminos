using System;

namespace System.Deployment.Application
{
	// Token: 0x02000075 RID: 117
	[Flags]
	internal enum MaintenanceFlags
	{
		// Token: 0x0400029A RID: 666
		ClearFlag = 0,
		// Token: 0x0400029B RID: 667
		RestorationPossible = 1,
		// Token: 0x0400029C RID: 668
		RestoreSelected = 2,
		// Token: 0x0400029D RID: 669
		RemoveSelected = 4
	}
}
