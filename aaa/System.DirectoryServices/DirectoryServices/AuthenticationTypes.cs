using System;

namespace System.DirectoryServices
{
	// Token: 0x0200001B RID: 27
	[Flags]
	public enum AuthenticationTypes
	{
		// Token: 0x04000158 RID: 344
		None = 0,
		// Token: 0x04000159 RID: 345
		Secure = 1,
		// Token: 0x0400015A RID: 346
		Encryption = 2,
		// Token: 0x0400015B RID: 347
		SecureSocketsLayer = 2,
		// Token: 0x0400015C RID: 348
		ReadonlyServer = 4,
		// Token: 0x0400015D RID: 349
		Anonymous = 16,
		// Token: 0x0400015E RID: 350
		FastBind = 32,
		// Token: 0x0400015F RID: 351
		Signing = 64,
		// Token: 0x04000160 RID: 352
		Sealing = 128,
		// Token: 0x04000161 RID: 353
		Delegation = 256,
		// Token: 0x04000162 RID: 354
		ServerBind = 512
	}
}
