using System;

namespace System.Net.NetworkInformation
{
	// Token: 0x020005F2 RID: 1522
	[Flags]
	internal enum GetAdaptersAddressesFlags
	{
		// Token: 0x04002CDF RID: 11487
		SkipUnicast = 1,
		// Token: 0x04002CE0 RID: 11488
		SkipAnycast = 2,
		// Token: 0x04002CE1 RID: 11489
		SkipMulticast = 4,
		// Token: 0x04002CE2 RID: 11490
		SkipDnsServer = 8,
		// Token: 0x04002CE3 RID: 11491
		IncludePrefix = 16,
		// Token: 0x04002CE4 RID: 11492
		SkipFriendlyName = 32
	}
}
