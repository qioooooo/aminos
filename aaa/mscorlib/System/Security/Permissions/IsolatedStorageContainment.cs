using System;
using System.Runtime.InteropServices;

namespace System.Security.Permissions
{
	// Token: 0x02000620 RID: 1568
	[ComVisible(true)]
	[Serializable]
	public enum IsolatedStorageContainment
	{
		// Token: 0x04001D86 RID: 7558
		None,
		// Token: 0x04001D87 RID: 7559
		DomainIsolationByUser = 16,
		// Token: 0x04001D88 RID: 7560
		ApplicationIsolationByUser = 21,
		// Token: 0x04001D89 RID: 7561
		AssemblyIsolationByUser = 32,
		// Token: 0x04001D8A RID: 7562
		DomainIsolationByMachine = 48,
		// Token: 0x04001D8B RID: 7563
		AssemblyIsolationByMachine = 64,
		// Token: 0x04001D8C RID: 7564
		ApplicationIsolationByMachine = 69,
		// Token: 0x04001D8D RID: 7565
		DomainIsolationByRoamingUser = 80,
		// Token: 0x04001D8E RID: 7566
		AssemblyIsolationByRoamingUser = 96,
		// Token: 0x04001D8F RID: 7567
		ApplicationIsolationByRoamingUser = 101,
		// Token: 0x04001D90 RID: 7568
		AdministerIsolatedStorageByUser = 112,
		// Token: 0x04001D91 RID: 7569
		UnrestrictedIsolatedStorage = 240
	}
}
