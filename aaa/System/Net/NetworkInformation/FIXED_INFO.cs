using System;
using System.Runtime.InteropServices;

namespace System.Net.NetworkInformation
{
	// Token: 0x020005F6 RID: 1526
	internal struct FIXED_INFO
	{
		// Token: 0x04002CEE RID: 11502
		internal const int MAX_HOSTNAME_LEN = 128;

		// Token: 0x04002CEF RID: 11503
		internal const int MAX_DOMAIN_NAME_LEN = 128;

		// Token: 0x04002CF0 RID: 11504
		internal const int MAX_SCOPE_ID_LEN = 256;

		// Token: 0x04002CF1 RID: 11505
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 132)]
		internal string hostName;

		// Token: 0x04002CF2 RID: 11506
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 132)]
		internal string domainName;

		// Token: 0x04002CF3 RID: 11507
		internal uint currentDnsServer;

		// Token: 0x04002CF4 RID: 11508
		internal IpAddrString DnsServerList;

		// Token: 0x04002CF5 RID: 11509
		internal NetBiosNodeType nodeType;

		// Token: 0x04002CF6 RID: 11510
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
		internal string scopeId;

		// Token: 0x04002CF7 RID: 11511
		internal bool enableRouting;

		// Token: 0x04002CF8 RID: 11512
		internal bool enableProxy;

		// Token: 0x04002CF9 RID: 11513
		internal bool enableDns;
	}
}
