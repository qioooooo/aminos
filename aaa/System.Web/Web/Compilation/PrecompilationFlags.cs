using System;

namespace System.Web.Compilation
{
	// Token: 0x0200015B RID: 347
	[Flags]
	public enum PrecompilationFlags
	{
		// Token: 0x04001608 RID: 5640
		Default = 0,
		// Token: 0x04001609 RID: 5641
		Updatable = 1,
		// Token: 0x0400160A RID: 5642
		OverwriteTarget = 2,
		// Token: 0x0400160B RID: 5643
		ForceDebug = 4,
		// Token: 0x0400160C RID: 5644
		Clean = 8,
		// Token: 0x0400160D RID: 5645
		CodeAnalysis = 16,
		// Token: 0x0400160E RID: 5646
		AllowPartiallyTrustedCallers = 32,
		// Token: 0x0400160F RID: 5647
		DelaySign = 64,
		// Token: 0x04001610 RID: 5648
		FixedNames = 128
	}
}
