using System;

namespace System.Net.NetworkInformation
{
	// Token: 0x0200062C RID: 1580
	internal enum IcmpV6StatType
	{
		// Token: 0x04002E32 RID: 11826
		DestinationUnreachable = 1,
		// Token: 0x04002E33 RID: 11827
		PacketTooBig,
		// Token: 0x04002E34 RID: 11828
		TimeExceeded,
		// Token: 0x04002E35 RID: 11829
		ParameterProblem,
		// Token: 0x04002E36 RID: 11830
		EchoRequest = 128,
		// Token: 0x04002E37 RID: 11831
		EchoReply,
		// Token: 0x04002E38 RID: 11832
		MembershipQuery,
		// Token: 0x04002E39 RID: 11833
		MembershipReport,
		// Token: 0x04002E3A RID: 11834
		MembershipReduction,
		// Token: 0x04002E3B RID: 11835
		RouterSolicit,
		// Token: 0x04002E3C RID: 11836
		RouterAdvertisement,
		// Token: 0x04002E3D RID: 11837
		NeighborSolict,
		// Token: 0x04002E3E RID: 11838
		NeighborAdvertisement,
		// Token: 0x04002E3F RID: 11839
		Redirect
	}
}
