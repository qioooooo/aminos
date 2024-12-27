using System;

namespace System.Net
{
	// Token: 0x02000502 RID: 1282
	[Flags]
	internal enum NameInfoFlags
	{
		// Token: 0x0400272E RID: 10030
		NI_NOFQDN = 1,
		// Token: 0x0400272F RID: 10031
		NI_NUMERICHOST = 2,
		// Token: 0x04002730 RID: 10032
		NI_NAMEREQD = 4,
		// Token: 0x04002731 RID: 10033
		NI_NUMERICSERV = 8,
		// Token: 0x04002732 RID: 10034
		NI_DGRAM = 16
	}
}
