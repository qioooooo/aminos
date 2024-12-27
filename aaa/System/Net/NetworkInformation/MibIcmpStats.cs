using System;

namespace System.Net.NetworkInformation
{
	// Token: 0x02000603 RID: 1539
	internal struct MibIcmpStats
	{
		// Token: 0x04002D85 RID: 11653
		internal uint messages;

		// Token: 0x04002D86 RID: 11654
		internal uint errors;

		// Token: 0x04002D87 RID: 11655
		internal uint destinationUnreachables;

		// Token: 0x04002D88 RID: 11656
		internal uint timeExceeds;

		// Token: 0x04002D89 RID: 11657
		internal uint parameterProblems;

		// Token: 0x04002D8A RID: 11658
		internal uint sourceQuenches;

		// Token: 0x04002D8B RID: 11659
		internal uint redirects;

		// Token: 0x04002D8C RID: 11660
		internal uint echoRequests;

		// Token: 0x04002D8D RID: 11661
		internal uint echoReplies;

		// Token: 0x04002D8E RID: 11662
		internal uint timestampRequests;

		// Token: 0x04002D8F RID: 11663
		internal uint timestampReplies;

		// Token: 0x04002D90 RID: 11664
		internal uint addressMaskRequests;

		// Token: 0x04002D91 RID: 11665
		internal uint addressMaskReplies;
	}
}
