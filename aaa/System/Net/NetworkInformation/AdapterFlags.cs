using System;

namespace System.Net.NetworkInformation
{
	// Token: 0x020005EF RID: 1519
	[Flags]
	internal enum AdapterFlags
	{
		// Token: 0x04002CCE RID: 11470
		DnsEnabled = 1,
		// Token: 0x04002CCF RID: 11471
		RegisterAdapterSuffix = 2,
		// Token: 0x04002CD0 RID: 11472
		DhcpEnabled = 4,
		// Token: 0x04002CD1 RID: 11473
		ReceiveOnly = 8,
		// Token: 0x04002CD2 RID: 11474
		NoMulticast = 16,
		// Token: 0x04002CD3 RID: 11475
		Ipv6OtherStatefulConfig = 32
	}
}
