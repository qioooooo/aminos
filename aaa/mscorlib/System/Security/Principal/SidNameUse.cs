using System;

namespace System.Security.Principal
{
	// Token: 0x0200092E RID: 2350
	internal enum SidNameUse
	{
		// Token: 0x04002C23 RID: 11299
		User = 1,
		// Token: 0x04002C24 RID: 11300
		Group,
		// Token: 0x04002C25 RID: 11301
		Domain,
		// Token: 0x04002C26 RID: 11302
		Alias,
		// Token: 0x04002C27 RID: 11303
		WellKnownGroup,
		// Token: 0x04002C28 RID: 11304
		DeletedAccount,
		// Token: 0x04002C29 RID: 11305
		Invalid,
		// Token: 0x04002C2A RID: 11306
		Unknown,
		// Token: 0x04002C2B RID: 11307
		Computer
	}
}
