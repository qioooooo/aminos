using System;

namespace System.DirectoryServices.Protocols
{
	// Token: 0x02000087 RID: 135
	public enum SecurityProtocol
	{
		// Token: 0x04000289 RID: 649
		Pct1Server = 1,
		// Token: 0x0400028A RID: 650
		Pct1Client,
		// Token: 0x0400028B RID: 651
		Ssl2Server = 4,
		// Token: 0x0400028C RID: 652
		Ssl2Client = 8,
		// Token: 0x0400028D RID: 653
		Ssl3Server = 16,
		// Token: 0x0400028E RID: 654
		Ssl3Client = 32,
		// Token: 0x0400028F RID: 655
		Tls1Server = 64,
		// Token: 0x04000290 RID: 656
		Tls1Client = 128
	}
}
