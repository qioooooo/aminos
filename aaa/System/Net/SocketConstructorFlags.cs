using System;

namespace System.Net
{
	// Token: 0x02000504 RID: 1284
	[Flags]
	internal enum SocketConstructorFlags
	{
		// Token: 0x04002737 RID: 10039
		WSA_FLAG_OVERLAPPED = 1,
		// Token: 0x04002738 RID: 10040
		WSA_FLAG_MULTIPOINT_C_ROOT = 2,
		// Token: 0x04002739 RID: 10041
		WSA_FLAG_MULTIPOINT_C_LEAF = 4,
		// Token: 0x0400273A RID: 10042
		WSA_FLAG_MULTIPOINT_D_ROOT = 8,
		// Token: 0x0400273B RID: 10043
		WSA_FLAG_MULTIPOINT_D_LEAF = 16
	}
}
