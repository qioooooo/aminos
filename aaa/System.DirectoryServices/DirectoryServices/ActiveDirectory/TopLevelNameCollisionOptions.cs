using System;

namespace System.DirectoryServices.ActiveDirectory
{
	// Token: 0x02000100 RID: 256
	[Flags]
	public enum TopLevelNameCollisionOptions
	{
		// Token: 0x04000631 RID: 1585
		None = 0,
		// Token: 0x04000632 RID: 1586
		NewlyCreated = 1,
		// Token: 0x04000633 RID: 1587
		DisabledByAdmin = 2,
		// Token: 0x04000634 RID: 1588
		DisabledByConflict = 4
	}
}
