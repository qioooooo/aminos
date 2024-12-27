using System;

namespace System.Web.Configuration
{
	// Token: 0x0200023D RID: 573
	internal enum RpcAuthent
	{
		// Token: 0x040019E8 RID: 6632
		None,
		// Token: 0x040019E9 RID: 6633
		DcePrivate,
		// Token: 0x040019EA RID: 6634
		DcePublic,
		// Token: 0x040019EB RID: 6635
		DecPublic = 4,
		// Token: 0x040019EC RID: 6636
		GssNegotiate = 9,
		// Token: 0x040019ED RID: 6637
		WinNT,
		// Token: 0x040019EE RID: 6638
		GssSchannel = 14,
		// Token: 0x040019EF RID: 6639
		GssKerberos = 16,
		// Token: 0x040019F0 RID: 6640
		DPA,
		// Token: 0x040019F1 RID: 6641
		MSN,
		// Token: 0x040019F2 RID: 6642
		Digest = 21,
		// Token: 0x040019F3 RID: 6643
		MQ = 100,
		// Token: 0x040019F4 RID: 6644
		Default = -1
	}
}
