using System;

namespace System
{
	// Token: 0x0200003D RID: 61
	internal enum DelegateBindingFlags
	{
		// Token: 0x0400010E RID: 270
		StaticMethodOnly = 1,
		// Token: 0x0400010F RID: 271
		InstanceMethodOnly,
		// Token: 0x04000110 RID: 272
		OpenDelegateOnly = 4,
		// Token: 0x04000111 RID: 273
		ClosedDelegateOnly = 8,
		// Token: 0x04000112 RID: 274
		NeverCloseOverNull = 16,
		// Token: 0x04000113 RID: 275
		CaselessMatching = 32,
		// Token: 0x04000114 RID: 276
		SkipSecurityChecks = 64,
		// Token: 0x04000115 RID: 277
		RelaxedSignature = 128
	}
}
