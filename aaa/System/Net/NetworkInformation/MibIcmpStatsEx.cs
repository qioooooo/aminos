using System;
using System.Runtime.InteropServices;

namespace System.Net.NetworkInformation
{
	// Token: 0x02000605 RID: 1541
	internal struct MibIcmpStatsEx
	{
		// Token: 0x04002D94 RID: 11668
		internal uint dwMsgs;

		// Token: 0x04002D95 RID: 11669
		internal uint dwErrors;

		// Token: 0x04002D96 RID: 11670
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
		internal uint[] rgdwTypeCount;
	}
}
