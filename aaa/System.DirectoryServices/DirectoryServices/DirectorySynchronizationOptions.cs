using System;

namespace System.DirectoryServices
{
	// Token: 0x02000029 RID: 41
	[Flags]
	public enum DirectorySynchronizationOptions : long
	{
		// Token: 0x040001AE RID: 430
		None = 0L,
		// Token: 0x040001AF RID: 431
		ObjectSecurity = 1L,
		// Token: 0x040001B0 RID: 432
		ParentsFirst = 2048L,
		// Token: 0x040001B1 RID: 433
		PublicDataOnly = 8192L,
		// Token: 0x040001B2 RID: 434
		IncrementalValues = 2147483648L
	}
}
