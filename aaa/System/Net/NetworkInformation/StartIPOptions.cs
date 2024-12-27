using System;

namespace System.Net.NetworkInformation
{
	// Token: 0x02000611 RID: 1553
	[Flags]
	internal enum StartIPOptions
	{
		// Token: 0x04002DC0 RID: 11712
		Both = 3,
		// Token: 0x04002DC1 RID: 11713
		None = 0,
		// Token: 0x04002DC2 RID: 11714
		StartIPv4 = 1,
		// Token: 0x04002DC3 RID: 11715
		StartIPv6 = 2
	}
}
