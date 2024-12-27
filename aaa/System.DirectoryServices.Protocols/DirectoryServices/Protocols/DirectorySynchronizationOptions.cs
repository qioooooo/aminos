using System;

namespace System.DirectoryServices.Protocols
{
	// Token: 0x02000013 RID: 19
	[Flags]
	public enum DirectorySynchronizationOptions : long
	{
		// Token: 0x040000D1 RID: 209
		None = 0L,
		// Token: 0x040000D2 RID: 210
		ObjectSecurity = 1L,
		// Token: 0x040000D3 RID: 211
		ParentsFirst = 2048L,
		// Token: 0x040000D4 RID: 212
		PublicDataOnly = 8192L,
		// Token: 0x040000D5 RID: 213
		IncrementalValues = 2147483648L
	}
}
