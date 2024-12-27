using System;

namespace System.Net.NetworkInformation
{
	// Token: 0x020005FF RID: 1535
	internal struct MibUdpStats
	{
		// Token: 0x04002D58 RID: 11608
		internal uint datagramsReceived;

		// Token: 0x04002D59 RID: 11609
		internal uint incomingDatagramsDiscarded;

		// Token: 0x04002D5A RID: 11610
		internal uint incomingDatagramsWithErrors;

		// Token: 0x04002D5B RID: 11611
		internal uint datagramsSent;

		// Token: 0x04002D5C RID: 11612
		internal uint udpListeners;
	}
}
