using System;

namespace System.DirectoryServices.Protocols
{
	// Token: 0x02000048 RID: 72
	[Flags]
	public enum ReferralChasingOptions
	{
		// Token: 0x0400012C RID: 300
		None = 0,
		// Token: 0x0400012D RID: 301
		Subordinate = 32,
		// Token: 0x0400012E RID: 302
		External = 64,
		// Token: 0x0400012F RID: 303
		All = 96
	}
}
