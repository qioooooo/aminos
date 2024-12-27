using System;
using System.Runtime.InteropServices;

namespace System.Net.NetworkInformation
{
	// Token: 0x020005F7 RID: 1527
	internal struct IpAdapterInfo
	{
		// Token: 0x04002CFA RID: 11514
		internal const int MAX_ADAPTER_DESCRIPTION_LENGTH = 128;

		// Token: 0x04002CFB RID: 11515
		internal const int MAX_ADAPTER_NAME_LENGTH = 256;

		// Token: 0x04002CFC RID: 11516
		internal const int MAX_ADAPTER_ADDRESS_LENGTH = 8;

		// Token: 0x04002CFD RID: 11517
		internal IntPtr Next;

		// Token: 0x04002CFE RID: 11518
		internal uint comboIndex;

		// Token: 0x04002CFF RID: 11519
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
		internal string adapterName;

		// Token: 0x04002D00 RID: 11520
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 132)]
		internal string description;

		// Token: 0x04002D01 RID: 11521
		internal uint addressLength;

		// Token: 0x04002D02 RID: 11522
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
		internal byte[] address;

		// Token: 0x04002D03 RID: 11523
		internal uint index;

		// Token: 0x04002D04 RID: 11524
		internal OldInterfaceType type;

		// Token: 0x04002D05 RID: 11525
		internal bool dhcpEnabled;

		// Token: 0x04002D06 RID: 11526
		internal IntPtr currentIpAddress;

		// Token: 0x04002D07 RID: 11527
		internal IpAddrString ipAddressList;

		// Token: 0x04002D08 RID: 11528
		internal IpAddrString gatewayList;

		// Token: 0x04002D09 RID: 11529
		internal IpAddrString dhcpServer;

		// Token: 0x04002D0A RID: 11530
		[MarshalAs(UnmanagedType.Bool)]
		internal bool haveWins;

		// Token: 0x04002D0B RID: 11531
		internal IpAddrString primaryWinsServer;

		// Token: 0x04002D0C RID: 11532
		internal IpAddrString secondaryWinsServer;

		// Token: 0x04002D0D RID: 11533
		internal uint leaseObtained;

		// Token: 0x04002D0E RID: 11534
		internal uint leaseExpires;
	}
}
