using System;

namespace System.Web.SessionState
{
	// Token: 0x02000361 RID: 865
	[Flags]
	internal enum SessionStateItemFlags
	{
		// Token: 0x04001F31 RID: 7985
		None = 0,
		// Token: 0x04001F32 RID: 7986
		Uninitialized = 1,
		// Token: 0x04001F33 RID: 7987
		IgnoreCacheItemRemoved = 2
	}
}
