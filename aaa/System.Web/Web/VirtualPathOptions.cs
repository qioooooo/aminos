using System;

namespace System.Web
{
	// Token: 0x020000E8 RID: 232
	[Flags]
	internal enum VirtualPathOptions
	{
		// Token: 0x04001337 RID: 4919
		AllowNull = 1,
		// Token: 0x04001338 RID: 4920
		EnsureTrailingSlash = 2,
		// Token: 0x04001339 RID: 4921
		AllowAbsolutePath = 4,
		// Token: 0x0400133A RID: 4922
		AllowAppRelativePath = 8,
		// Token: 0x0400133B RID: 4923
		AllowRelativePath = 16,
		// Token: 0x0400133C RID: 4924
		FailIfMalformed = 32,
		// Token: 0x0400133D RID: 4925
		AllowAllPath = 28
	}
}
