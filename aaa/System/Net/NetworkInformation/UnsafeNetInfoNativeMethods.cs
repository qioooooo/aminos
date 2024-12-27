using System;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Security;
using Microsoft.Win32.SafeHandles;

namespace System.Net.NetworkInformation
{
	// Token: 0x0200060E RID: 1550
	[SuppressUnmanagedCodeSecurity]
	internal static class UnsafeNetInfoNativeMethods
	{
		// Token: 0x06002FD3 RID: 12243
		[DllImport("iphlpapi.dll")]
		internal static extern uint GetAdaptersInfo(SafeLocalFree pAdapterInfo, ref uint pOutBufLen);

		// Token: 0x06002FD4 RID: 12244
		[DllImport("iphlpapi.dll")]
		internal static extern uint GetAdaptersAddresses(AddressFamily family, uint flags, IntPtr pReserved, SafeLocalFree adapterAddresses, ref uint outBufLen);

		// Token: 0x06002FD5 RID: 12245
		[DllImport("iphlpapi.dll")]
		internal static extern uint GetBestInterface(int ipAddress, out int index);

		// Token: 0x06002FD6 RID: 12246
		[DllImport("iphlpapi.dll")]
		internal static extern uint GetIfEntry(ref MibIfRow pIfRow);

		// Token: 0x06002FD7 RID: 12247
		[DllImport("iphlpapi.dll")]
		internal static extern uint GetIpStatistics(out MibIpStats statistics);

		// Token: 0x06002FD8 RID: 12248
		[DllImport("iphlpapi.dll")]
		internal static extern uint GetIpStatisticsEx(out MibIpStats statistics, AddressFamily family);

		// Token: 0x06002FD9 RID: 12249
		[DllImport("iphlpapi.dll")]
		internal static extern uint GetTcpStatistics(out MibTcpStats statistics);

		// Token: 0x06002FDA RID: 12250
		[DllImport("iphlpapi.dll")]
		internal static extern uint GetTcpStatisticsEx(out MibTcpStats statistics, AddressFamily family);

		// Token: 0x06002FDB RID: 12251
		[DllImport("iphlpapi.dll")]
		internal static extern uint GetUdpStatistics(out MibUdpStats statistics);

		// Token: 0x06002FDC RID: 12252
		[DllImport("iphlpapi.dll")]
		internal static extern uint GetUdpStatisticsEx(out MibUdpStats statistics, AddressFamily family);

		// Token: 0x06002FDD RID: 12253
		[DllImport("iphlpapi.dll")]
		internal static extern uint GetIcmpStatistics(out MibIcmpInfo statistics);

		// Token: 0x06002FDE RID: 12254
		[DllImport("iphlpapi.dll")]
		internal static extern uint GetIcmpStatisticsEx(out MibIcmpInfoEx statistics, AddressFamily family);

		// Token: 0x06002FDF RID: 12255
		[DllImport("iphlpapi.dll")]
		internal static extern uint GetTcpTable(SafeLocalFree pTcpTable, ref uint dwOutBufLen, bool order);

		// Token: 0x06002FE0 RID: 12256
		[DllImport("iphlpapi.dll")]
		internal static extern uint GetUdpTable(SafeLocalFree pUdpTable, ref uint dwOutBufLen, bool order);

		// Token: 0x06002FE1 RID: 12257
		[DllImport("iphlpapi.dll")]
		internal static extern uint GetNetworkParams(SafeLocalFree pFixedInfo, ref uint pOutBufLen);

		// Token: 0x06002FE2 RID: 12258
		[DllImport("iphlpapi.dll")]
		internal static extern uint GetPerAdapterInfo(uint IfIndex, SafeLocalFree pPerAdapterInfo, ref uint pOutBufLen);

		// Token: 0x06002FE3 RID: 12259
		[DllImport("iphlpapi.dll", SetLastError = true)]
		internal static extern SafeCloseIcmpHandle IcmpCreateFile();

		// Token: 0x06002FE4 RID: 12260
		[DllImport("iphlpapi.dll", SetLastError = true)]
		internal static extern SafeCloseIcmpHandle Icmp6CreateFile();

		// Token: 0x06002FE5 RID: 12261
		[DllImport("iphlpapi.dll", SetLastError = true)]
		internal static extern bool IcmpCloseHandle(IntPtr handle);

		// Token: 0x06002FE6 RID: 12262
		[DllImport("iphlpapi.dll", SetLastError = true)]
		internal static extern uint IcmpSendEcho2(SafeCloseIcmpHandle icmpHandle, SafeWaitHandle Event, IntPtr apcRoutine, IntPtr apcContext, uint ipAddress, [In] SafeLocalFree data, ushort dataSize, ref IPOptions options, SafeLocalFree replyBuffer, uint replySize, uint timeout);

		// Token: 0x06002FE7 RID: 12263
		[DllImport("iphlpapi.dll", SetLastError = true)]
		internal static extern uint IcmpSendEcho2(SafeCloseIcmpHandle icmpHandle, IntPtr Event, IntPtr apcRoutine, IntPtr apcContext, uint ipAddress, [In] SafeLocalFree data, ushort dataSize, ref IPOptions options, SafeLocalFree replyBuffer, uint replySize, uint timeout);

		// Token: 0x06002FE8 RID: 12264
		[DllImport("iphlpapi.dll", SetLastError = true)]
		internal static extern uint Icmp6SendEcho2(SafeCloseIcmpHandle icmpHandle, SafeWaitHandle Event, IntPtr apcRoutine, IntPtr apcContext, byte[] sourceSocketAddress, byte[] destSocketAddress, [In] SafeLocalFree data, ushort dataSize, ref IPOptions options, SafeLocalFree replyBuffer, uint replySize, uint timeout);

		// Token: 0x06002FE9 RID: 12265
		[DllImport("iphlpapi.dll", SetLastError = true)]
		internal static extern uint Icmp6SendEcho2(SafeCloseIcmpHandle icmpHandle, IntPtr Event, IntPtr apcRoutine, IntPtr apcContext, byte[] sourceSocketAddress, byte[] destSocketAddress, [In] SafeLocalFree data, ushort dataSize, ref IPOptions options, SafeLocalFree replyBuffer, uint replySize, uint timeout);

		// Token: 0x06002FEA RID: 12266
		[DllImport("iphlpapi.dll", SetLastError = true)]
		internal static extern uint IcmpParseReplies(IntPtr replyBuffer, uint replySize);

		// Token: 0x06002FEB RID: 12267
		[DllImport("iphlpapi.dll", SetLastError = true)]
		internal static extern uint Icmp6ParseReplies(IntPtr replyBuffer, uint replySize);

		// Token: 0x04002DBC RID: 11708
		private const string IPHLPAPI = "iphlpapi.dll";
	}
}
