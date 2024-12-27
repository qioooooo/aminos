using System;

namespace System.Net.NetworkInformation
{
	// Token: 0x02000600 RID: 1536
	internal struct MibTcpStats
	{
		// Token: 0x04002D5D RID: 11613
		internal uint reTransmissionAlgorithm;

		// Token: 0x04002D5E RID: 11614
		internal uint minimumRetransmissionTimeOut;

		// Token: 0x04002D5F RID: 11615
		internal uint maximumRetransmissionTimeOut;

		// Token: 0x04002D60 RID: 11616
		internal uint maximumConnections;

		// Token: 0x04002D61 RID: 11617
		internal uint activeOpens;

		// Token: 0x04002D62 RID: 11618
		internal uint passiveOpens;

		// Token: 0x04002D63 RID: 11619
		internal uint failedConnectionAttempts;

		// Token: 0x04002D64 RID: 11620
		internal uint resetConnections;

		// Token: 0x04002D65 RID: 11621
		internal uint currentConnections;

		// Token: 0x04002D66 RID: 11622
		internal uint segmentsReceived;

		// Token: 0x04002D67 RID: 11623
		internal uint segmentsSent;

		// Token: 0x04002D68 RID: 11624
		internal uint segmentsResent;

		// Token: 0x04002D69 RID: 11625
		internal uint errorsReceived;

		// Token: 0x04002D6A RID: 11626
		internal uint segmentsSentWithReset;

		// Token: 0x04002D6B RID: 11627
		internal uint cumulativeConnections;
	}
}
