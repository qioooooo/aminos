using System;
using System.Runtime.InteropServices;

namespace System.Net
{
	// Token: 0x02000503 RID: 1283
	internal struct IPv6MulticastRequest
	{
		// Token: 0x04002733 RID: 10035
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
		internal byte[] MulticastAddress;

		// Token: 0x04002734 RID: 10036
		internal int InterfaceIndex;

		// Token: 0x04002735 RID: 10037
		internal static readonly int Size = Marshal.SizeOf(typeof(IPv6MulticastRequest));
	}
}
