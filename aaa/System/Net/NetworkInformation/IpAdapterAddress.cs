using System;

namespace System.Net.NetworkInformation
{
	// Token: 0x020005F9 RID: 1529
	internal struct IpAdapterAddress
	{
		// Token: 0x04002D11 RID: 11537
		internal uint length;

		// Token: 0x04002D12 RID: 11538
		internal AdapterAddressFlags flags;

		// Token: 0x04002D13 RID: 11539
		internal IntPtr next;

		// Token: 0x04002D14 RID: 11540
		internal IpSocketAddress address;
	}
}
