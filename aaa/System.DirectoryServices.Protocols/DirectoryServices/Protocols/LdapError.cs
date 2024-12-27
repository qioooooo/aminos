using System;

namespace System.DirectoryServices.Protocols
{
	// Token: 0x02000076 RID: 118
	internal enum LdapError
	{
		// Token: 0x04000257 RID: 599
		IsLeaf = 35,
		// Token: 0x04000258 RID: 600
		InvalidCredentials = 49,
		// Token: 0x04000259 RID: 601
		ServerDown = 81,
		// Token: 0x0400025A RID: 602
		LocalError,
		// Token: 0x0400025B RID: 603
		EncodingError,
		// Token: 0x0400025C RID: 604
		DecodingError,
		// Token: 0x0400025D RID: 605
		TimeOut,
		// Token: 0x0400025E RID: 606
		AuthUnknown,
		// Token: 0x0400025F RID: 607
		FilterError,
		// Token: 0x04000260 RID: 608
		UserCancelled,
		// Token: 0x04000261 RID: 609
		ParameterError,
		// Token: 0x04000262 RID: 610
		NoMemory,
		// Token: 0x04000263 RID: 611
		ConnectError,
		// Token: 0x04000264 RID: 612
		NotSupported,
		// Token: 0x04000265 RID: 613
		NoResultsReturned = 94,
		// Token: 0x04000266 RID: 614
		ControlNotFound = 93,
		// Token: 0x04000267 RID: 615
		MoreResults = 95,
		// Token: 0x04000268 RID: 616
		ClientLoop,
		// Token: 0x04000269 RID: 617
		ReferralLimitExceeded,
		// Token: 0x0400026A RID: 618
		SendTimeOut = 112
	}
}
