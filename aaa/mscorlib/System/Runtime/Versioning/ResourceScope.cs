using System;

namespace System.Runtime.Versioning
{
	// Token: 0x02000936 RID: 2358
	[Flags]
	public enum ResourceScope
	{
		// Token: 0x04002C8E RID: 11406
		None = 0,
		// Token: 0x04002C8F RID: 11407
		Machine = 1,
		// Token: 0x04002C90 RID: 11408
		Process = 2,
		// Token: 0x04002C91 RID: 11409
		AppDomain = 4,
		// Token: 0x04002C92 RID: 11410
		Library = 8,
		// Token: 0x04002C93 RID: 11411
		Private = 16,
		// Token: 0x04002C94 RID: 11412
		Assembly = 32
	}
}
