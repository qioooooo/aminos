using System;

namespace System.DirectoryServices.Interop
{
	// Token: 0x02000049 RID: 73
	internal enum AdsAuthentication
	{
		// Token: 0x04000207 RID: 519
		ADS_SECURE_AUTHENTICATION = 1,
		// Token: 0x04000208 RID: 520
		ADS_USE_ENCRYPTION,
		// Token: 0x04000209 RID: 521
		ADS_USE_SSL = 2,
		// Token: 0x0400020A RID: 522
		ADS_READONLY_SERVER = 4,
		// Token: 0x0400020B RID: 523
		ADS_PROMPT_CREDENTIALS = 8,
		// Token: 0x0400020C RID: 524
		ADS_NO_AUTHENTICATION = 16,
		// Token: 0x0400020D RID: 525
		ADS_FAST_BIND = 32,
		// Token: 0x0400020E RID: 526
		ADS_USE_SIGNING = 64,
		// Token: 0x0400020F RID: 527
		ADS_USE_SEALING = 128
	}
}
