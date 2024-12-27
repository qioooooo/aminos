using System;

namespace System.Net
{
	// Token: 0x020003F6 RID: 1014
	internal enum ContextAttribute
	{
		// Token: 0x04002012 RID: 8210
		Sizes,
		// Token: 0x04002013 RID: 8211
		Names,
		// Token: 0x04002014 RID: 8212
		Lifespan,
		// Token: 0x04002015 RID: 8213
		DceInfo,
		// Token: 0x04002016 RID: 8214
		StreamSizes,
		// Token: 0x04002017 RID: 8215
		Authority = 6,
		// Token: 0x04002018 RID: 8216
		PackageInfo = 10,
		// Token: 0x04002019 RID: 8217
		NegotiationInfo = 12,
		// Token: 0x0400201A RID: 8218
		UniqueBindings = 25,
		// Token: 0x0400201B RID: 8219
		EndpointBindings,
		// Token: 0x0400201C RID: 8220
		ClientSpecifiedSpn,
		// Token: 0x0400201D RID: 8221
		RemoteCertificate = 83,
		// Token: 0x0400201E RID: 8222
		LocalCertificate,
		// Token: 0x0400201F RID: 8223
		RootStore,
		// Token: 0x04002020 RID: 8224
		IssuerListInfoEx = 89,
		// Token: 0x04002021 RID: 8225
		ConnectionInfo
	}
}
