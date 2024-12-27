using System;
using System.Runtime.InteropServices;

namespace System.Net.NetworkInformation
{
	// Token: 0x020005FE RID: 1534
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	internal struct MibIfRow
	{
		// Token: 0x04002D3D RID: 11581
		internal const int MAX_INTERFACE_NAME_LEN = 256;

		// Token: 0x04002D3E RID: 11582
		internal const int MAXLEN_IFDESCR = 256;

		// Token: 0x04002D3F RID: 11583
		internal const int MAXLEN_PHYSADDR = 8;

		// Token: 0x04002D40 RID: 11584
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
		internal string wszName;

		// Token: 0x04002D41 RID: 11585
		internal uint dwIndex;

		// Token: 0x04002D42 RID: 11586
		internal uint dwType;

		// Token: 0x04002D43 RID: 11587
		internal uint dwMtu;

		// Token: 0x04002D44 RID: 11588
		internal uint dwSpeed;

		// Token: 0x04002D45 RID: 11589
		internal uint dwPhysAddrLen;

		// Token: 0x04002D46 RID: 11590
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
		internal byte[] bPhysAddr;

		// Token: 0x04002D47 RID: 11591
		internal uint dwAdminStatus;

		// Token: 0x04002D48 RID: 11592
		internal OldOperationalStatus operStatus;

		// Token: 0x04002D49 RID: 11593
		internal uint dwLastChange;

		// Token: 0x04002D4A RID: 11594
		internal uint dwInOctets;

		// Token: 0x04002D4B RID: 11595
		internal uint dwInUcastPkts;

		// Token: 0x04002D4C RID: 11596
		internal uint dwInNUcastPkts;

		// Token: 0x04002D4D RID: 11597
		internal uint dwInDiscards;

		// Token: 0x04002D4E RID: 11598
		internal uint dwInErrors;

		// Token: 0x04002D4F RID: 11599
		internal uint dwInUnknownProtos;

		// Token: 0x04002D50 RID: 11600
		internal uint dwOutOctets;

		// Token: 0x04002D51 RID: 11601
		internal uint dwOutUcastPkts;

		// Token: 0x04002D52 RID: 11602
		internal uint dwOutNUcastPkts;

		// Token: 0x04002D53 RID: 11603
		internal uint dwOutDiscards;

		// Token: 0x04002D54 RID: 11604
		internal uint dwOutErrors;

		// Token: 0x04002D55 RID: 11605
		internal uint dwOutQLen;

		// Token: 0x04002D56 RID: 11606
		internal uint dwDescrLen;

		// Token: 0x04002D57 RID: 11607
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
		internal byte[] bDescr;
	}
}
