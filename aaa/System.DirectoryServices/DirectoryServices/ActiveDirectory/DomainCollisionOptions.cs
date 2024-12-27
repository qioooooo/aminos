using System;

namespace System.DirectoryServices.ActiveDirectory
{
	// Token: 0x02000101 RID: 257
	[Flags]
	public enum DomainCollisionOptions
	{
		// Token: 0x04000636 RID: 1590
		None = 0,
		// Token: 0x04000637 RID: 1591
		SidDisabledByAdmin = 1,
		// Token: 0x04000638 RID: 1592
		SidDisabledByConflict = 2,
		// Token: 0x04000639 RID: 1593
		NetBiosNameDisabledByAdmin = 4,
		// Token: 0x0400063A RID: 1594
		NetBiosNameDisabledByConflict = 8
	}
}
