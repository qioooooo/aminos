using System;

namespace System
{
	// Token: 0x02000366 RID: 870
	[Flags]
	internal enum UriSyntaxFlags
	{
		// Token: 0x04001C34 RID: 7220
		MustHaveAuthority = 1,
		// Token: 0x04001C35 RID: 7221
		OptionalAuthority = 2,
		// Token: 0x04001C36 RID: 7222
		MayHaveUserInfo = 4,
		// Token: 0x04001C37 RID: 7223
		MayHavePort = 8,
		// Token: 0x04001C38 RID: 7224
		MayHavePath = 16,
		// Token: 0x04001C39 RID: 7225
		MayHaveQuery = 32,
		// Token: 0x04001C3A RID: 7226
		MayHaveFragment = 64,
		// Token: 0x04001C3B RID: 7227
		AllowEmptyHost = 128,
		// Token: 0x04001C3C RID: 7228
		AllowUncHost = 256,
		// Token: 0x04001C3D RID: 7229
		AllowDnsHost = 512,
		// Token: 0x04001C3E RID: 7230
		AllowIPv4Host = 1024,
		// Token: 0x04001C3F RID: 7231
		AllowIPv6Host = 2048,
		// Token: 0x04001C40 RID: 7232
		AllowAnInternetHost = 3584,
		// Token: 0x04001C41 RID: 7233
		AllowAnyOtherHost = 4096,
		// Token: 0x04001C42 RID: 7234
		FileLikeUri = 8192,
		// Token: 0x04001C43 RID: 7235
		MailToLikeUri = 16384,
		// Token: 0x04001C44 RID: 7236
		V1_UnknownUri = 65536,
		// Token: 0x04001C45 RID: 7237
		SimpleUserSyntax = 131072,
		// Token: 0x04001C46 RID: 7238
		BuiltInSyntax = 262144,
		// Token: 0x04001C47 RID: 7239
		ParserSchemeOnly = 524288,
		// Token: 0x04001C48 RID: 7240
		AllowDOSPath = 1048576,
		// Token: 0x04001C49 RID: 7241
		PathIsRooted = 2097152,
		// Token: 0x04001C4A RID: 7242
		ConvertPathSlashes = 4194304,
		// Token: 0x04001C4B RID: 7243
		CompressPath = 8388608,
		// Token: 0x04001C4C RID: 7244
		CanonicalizeAsFilePath = 16777216,
		// Token: 0x04001C4D RID: 7245
		UnEscapeDotsAndSlashes = 33554432,
		// Token: 0x04001C4E RID: 7246
		AllowIdn = 67108864,
		// Token: 0x04001C4F RID: 7247
		AllowIriParsing = 268435456
	}
}
