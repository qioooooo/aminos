using System;

namespace System.Net.NetworkInformation
{
	// Token: 0x020005FA RID: 1530
	internal struct IpAdapterUnicastAddress
	{
		// Token: 0x04002D15 RID: 11541
		internal uint length;

		// Token: 0x04002D16 RID: 11542
		internal AdapterAddressFlags flags;

		// Token: 0x04002D17 RID: 11543
		internal IntPtr next;

		// Token: 0x04002D18 RID: 11544
		internal IpSocketAddress address;

		// Token: 0x04002D19 RID: 11545
		internal PrefixOrigin prefixOrigin;

		// Token: 0x04002D1A RID: 11546
		internal SuffixOrigin suffixOrigin;

		// Token: 0x04002D1B RID: 11547
		internal DuplicateAddressDetectionState dadState;

		// Token: 0x04002D1C RID: 11548
		internal uint validLifetime;

		// Token: 0x04002D1D RID: 11549
		internal uint preferredLifetime;

		// Token: 0x04002D1E RID: 11550
		internal uint leaseLifetime;
	}
}
