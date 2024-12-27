using System;

namespace System
{
	// Token: 0x0200036C RID: 876
	[Flags]
	public enum GenericUriParserOptions
	{
		// Token: 0x04001C6F RID: 7279
		Default = 0,
		// Token: 0x04001C70 RID: 7280
		GenericAuthority = 1,
		// Token: 0x04001C71 RID: 7281
		AllowEmptyAuthority = 2,
		// Token: 0x04001C72 RID: 7282
		NoUserInfo = 4,
		// Token: 0x04001C73 RID: 7283
		NoPort = 8,
		// Token: 0x04001C74 RID: 7284
		NoQuery = 16,
		// Token: 0x04001C75 RID: 7285
		NoFragment = 32,
		// Token: 0x04001C76 RID: 7286
		DontConvertPathBackslashes = 64,
		// Token: 0x04001C77 RID: 7287
		DontCompressPath = 128,
		// Token: 0x04001C78 RID: 7288
		DontUnescapePathDotsAndSlashes = 256,
		// Token: 0x04001C79 RID: 7289
		Idn = 512,
		// Token: 0x04001C7A RID: 7290
		IriParsing = 1024
	}
}
