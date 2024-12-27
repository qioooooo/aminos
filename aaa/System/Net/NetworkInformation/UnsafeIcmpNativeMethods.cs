using System;
using System.Runtime.InteropServices;
using System.Security;
using Microsoft.Win32.SafeHandles;

namespace System.Net.NetworkInformation
{
	// Token: 0x0200060F RID: 1551
	[SuppressUnmanagedCodeSecurity]
	internal static class UnsafeIcmpNativeMethods
	{
		// Token: 0x06002FEC RID: 12268
		[DllImport("icmp.dll", SetLastError = true)]
		internal static extern SafeCloseIcmpHandle IcmpCreateFile();

		// Token: 0x06002FED RID: 12269
		[DllImport("icmp.dll", SetLastError = true)]
		internal static extern bool IcmpCloseHandle(IntPtr icmpHandle);

		// Token: 0x06002FEE RID: 12270
		[DllImport("icmp.dll", SetLastError = true)]
		internal static extern uint IcmpSendEcho2(SafeCloseIcmpHandle icmpHandle, SafeWaitHandle Event, IntPtr apcRoutine, IntPtr apcContext, uint ipAddress, [In] SafeLocalFree data, ushort dataSize, ref IPOptions options, SafeLocalFree replyBuffer, uint replySize, uint timeout);

		// Token: 0x06002FEF RID: 12271
		[DllImport("icmp.dll", SetLastError = true)]
		internal static extern uint IcmpSendEcho2(SafeCloseIcmpHandle icmpHandle, IntPtr Event, IntPtr apcRoutine, IntPtr apcContext, uint ipAddress, [In] SafeLocalFree data, ushort dataSize, ref IPOptions options, SafeLocalFree replyBuffer, uint replySize, uint timeout);

		// Token: 0x06002FF0 RID: 12272
		[DllImport("icmp.dll", SetLastError = true)]
		internal static extern uint IcmpParseReplies(IntPtr replyBuffer, uint replySize);

		// Token: 0x04002DBD RID: 11709
		private const string ICMP = "icmp.dll";
	}
}
