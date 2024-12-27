using System;
using System.Runtime.InteropServices;

namespace System.Net.NetworkInformation
{
	// Token: 0x020005FC RID: 1532
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	internal struct IpAdapterAddresses
	{
		// Token: 0x04002D24 RID: 11556
		internal const int MAX_ADAPTER_ADDRESS_LENGTH = 8;

		// Token: 0x04002D25 RID: 11557
		internal uint length;

		// Token: 0x04002D26 RID: 11558
		internal uint index;

		// Token: 0x04002D27 RID: 11559
		internal IntPtr next;

		// Token: 0x04002D28 RID: 11560
		[MarshalAs(UnmanagedType.LPStr)]
		internal string AdapterName;

		// Token: 0x04002D29 RID: 11561
		internal IntPtr FirstUnicastAddress;

		// Token: 0x04002D2A RID: 11562
		internal IntPtr FirstAnycastAddress;

		// Token: 0x04002D2B RID: 11563
		internal IntPtr FirstMulticastAddress;

		// Token: 0x04002D2C RID: 11564
		internal IntPtr FirstDnsServerAddress;

		// Token: 0x04002D2D RID: 11565
		internal string dnsSuffix;

		// Token: 0x04002D2E RID: 11566
		internal string description;

		// Token: 0x04002D2F RID: 11567
		internal string friendlyName;

		// Token: 0x04002D30 RID: 11568
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
		internal byte[] address;

		// Token: 0x04002D31 RID: 11569
		internal uint addressLength;

		// Token: 0x04002D32 RID: 11570
		internal AdapterFlags flags;

		// Token: 0x04002D33 RID: 11571
		internal uint mtu;

		// Token: 0x04002D34 RID: 11572
		internal NetworkInterfaceType type;

		// Token: 0x04002D35 RID: 11573
		internal OperationalStatus operStatus;

		// Token: 0x04002D36 RID: 11574
		internal uint ipv6Index;

		// Token: 0x04002D37 RID: 11575
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
		internal uint[] zoneIndices;

		// Token: 0x04002D38 RID: 11576
		internal IntPtr firstPrefix;
	}
}
