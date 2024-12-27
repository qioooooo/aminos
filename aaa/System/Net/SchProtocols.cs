using System;

namespace System.Net
{
	// Token: 0x02000544 RID: 1348
	internal enum SchProtocols
	{
		// Token: 0x040027E4 RID: 10212
		Zero,
		// Token: 0x040027E5 RID: 10213
		PctClient = 2,
		// Token: 0x040027E6 RID: 10214
		PctServer = 1,
		// Token: 0x040027E7 RID: 10215
		Pct = 3,
		// Token: 0x040027E8 RID: 10216
		Ssl2Client = 8,
		// Token: 0x040027E9 RID: 10217
		Ssl2Server = 4,
		// Token: 0x040027EA RID: 10218
		Ssl2 = 12,
		// Token: 0x040027EB RID: 10219
		Ssl3Client = 32,
		// Token: 0x040027EC RID: 10220
		Ssl3Server = 16,
		// Token: 0x040027ED RID: 10221
		Ssl3 = 48,
		// Token: 0x040027EE RID: 10222
		TlsClient = 128,
		// Token: 0x040027EF RID: 10223
		TlsServer = 64,
		// Token: 0x040027F0 RID: 10224
		Tls = 192,
		// Token: 0x040027F1 RID: 10225
		Tls11Client = 512,
		// Token: 0x040027F2 RID: 10226
		Tls11Server = 256,
		// Token: 0x040027F3 RID: 10227
		Tls11 = 768,
		// Token: 0x040027F4 RID: 10228
		Tls12Client = 2048,
		// Token: 0x040027F5 RID: 10229
		Tls12Server = 1024,
		// Token: 0x040027F6 RID: 10230
		Tls12 = 3072,
		// Token: 0x040027F7 RID: 10231
		Ssl3Tls = 240,
		// Token: 0x040027F8 RID: 10232
		UniClient = -2147483648,
		// Token: 0x040027F9 RID: 10233
		UniServer = 1073741824,
		// Token: 0x040027FA RID: 10234
		Unified = -1073741824,
		// Token: 0x040027FB RID: 10235
		ClientMask = -2147480918,
		// Token: 0x040027FC RID: 10236
		ServerMask = 1073743189
	}
}
