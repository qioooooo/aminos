using System;
using System.Runtime.InteropServices;

namespace System.Net
{
	// Token: 0x020004FB RID: 1275
	internal struct IPMulticastRequest
	{
		// Token: 0x0400270F RID: 9999
		internal int MulticastAddress;

		// Token: 0x04002710 RID: 10000
		internal int InterfaceAddress;

		// Token: 0x04002711 RID: 10001
		internal static readonly int Size = Marshal.SizeOf(typeof(IPMulticastRequest));
	}
}
