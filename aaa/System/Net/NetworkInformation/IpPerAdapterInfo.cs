using System;

namespace System.Net.NetworkInformation
{
	// Token: 0x020005FD RID: 1533
	internal struct IpPerAdapterInfo
	{
		// Token: 0x04002D39 RID: 11577
		internal bool autoconfigEnabled;

		// Token: 0x04002D3A RID: 11578
		internal bool autoconfigActive;

		// Token: 0x04002D3B RID: 11579
		internal IntPtr currentDnsServer;

		// Token: 0x04002D3C RID: 11580
		internal IpAddrString dnsServerList;
	}
}
