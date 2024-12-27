using System;
using System.Runtime.InteropServices;

namespace System.Net
{
	// Token: 0x020004FF RID: 1279
	internal struct WSAData
	{
		// Token: 0x0400271A RID: 10010
		internal short wVersion;

		// Token: 0x0400271B RID: 10011
		internal short wHighVersion;

		// Token: 0x0400271C RID: 10012
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 257)]
		internal string szDescription;

		// Token: 0x0400271D RID: 10013
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 129)]
		internal string szSystemStatus;

		// Token: 0x0400271E RID: 10014
		internal short iMaxSockets;

		// Token: 0x0400271F RID: 10015
		internal short iMaxUdpDg;

		// Token: 0x04002720 RID: 10016
		internal IntPtr lpVendorInfo;
	}
}
