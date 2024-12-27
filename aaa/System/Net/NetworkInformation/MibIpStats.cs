using System;

namespace System.Net.NetworkInformation
{
	// Token: 0x02000601 RID: 1537
	internal struct MibIpStats
	{
		// Token: 0x04002D6C RID: 11628
		internal bool forwardingEnabled;

		// Token: 0x04002D6D RID: 11629
		internal uint defaultTtl;

		// Token: 0x04002D6E RID: 11630
		internal uint packetsReceived;

		// Token: 0x04002D6F RID: 11631
		internal uint receivedPacketsWithHeaderErrors;

		// Token: 0x04002D70 RID: 11632
		internal uint receivedPacketsWithAddressErrors;

		// Token: 0x04002D71 RID: 11633
		internal uint packetsForwarded;

		// Token: 0x04002D72 RID: 11634
		internal uint receivedPacketsWithUnknownProtocols;

		// Token: 0x04002D73 RID: 11635
		internal uint receivedPacketsDiscarded;

		// Token: 0x04002D74 RID: 11636
		internal uint receivedPacketsDelivered;

		// Token: 0x04002D75 RID: 11637
		internal uint packetOutputRequests;

		// Token: 0x04002D76 RID: 11638
		internal uint outputPacketRoutingDiscards;

		// Token: 0x04002D77 RID: 11639
		internal uint outputPacketsDiscarded;

		// Token: 0x04002D78 RID: 11640
		internal uint outputPacketsWithNoRoute;

		// Token: 0x04002D79 RID: 11641
		internal uint packetReassemblyTimeout;

		// Token: 0x04002D7A RID: 11642
		internal uint packetsReassemblyRequired;

		// Token: 0x04002D7B RID: 11643
		internal uint packetsReassembled;

		// Token: 0x04002D7C RID: 11644
		internal uint packetsReassemblyFailed;

		// Token: 0x04002D7D RID: 11645
		internal uint packetsFragmented;

		// Token: 0x04002D7E RID: 11646
		internal uint packetsFragmentFailed;

		// Token: 0x04002D7F RID: 11647
		internal uint packetsFragmentCreated;

		// Token: 0x04002D80 RID: 11648
		internal uint interfaces;

		// Token: 0x04002D81 RID: 11649
		internal uint ipAddresses;

		// Token: 0x04002D82 RID: 11650
		internal uint routes;
	}
}
