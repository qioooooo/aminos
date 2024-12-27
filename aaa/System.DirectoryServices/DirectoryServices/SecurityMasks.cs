using System;

namespace System.DirectoryServices
{
	// Token: 0x02000043 RID: 67
	[Flags]
	public enum SecurityMasks
	{
		// Token: 0x040001F8 RID: 504
		None = 0,
		// Token: 0x040001F9 RID: 505
		Owner = 1,
		// Token: 0x040001FA RID: 506
		Group = 2,
		// Token: 0x040001FB RID: 507
		Dacl = 4,
		// Token: 0x040001FC RID: 508
		Sacl = 8
	}
}
