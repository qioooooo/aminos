using System;

namespace System.Net.NetworkInformation
{
	// Token: 0x020005FB RID: 1531
	internal struct IpAdapterPrefix
	{
		// Token: 0x04002D1F RID: 11551
		internal uint length;

		// Token: 0x04002D20 RID: 11552
		internal uint ifIndex;

		// Token: 0x04002D21 RID: 11553
		internal IntPtr next;

		// Token: 0x04002D22 RID: 11554
		internal IpSocketAddress address;

		// Token: 0x04002D23 RID: 11555
		internal uint prefixLength;
	}
}
