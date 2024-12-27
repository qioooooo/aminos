using System;
using System.Runtime.InteropServices;

namespace System.Net.NetworkInformation
{
	// Token: 0x0200060C RID: 1548
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	internal struct Ipv6Address
	{
		// Token: 0x04002DB5 RID: 11701
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
		internal byte[] Goo;

		// Token: 0x04002DB6 RID: 11702
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
		internal byte[] Address;

		// Token: 0x04002DB7 RID: 11703
		internal uint ScopeID;
	}
}
