using System;

namespace System
{
	// Token: 0x02000369 RID: 873
	[Flags]
	public enum UriComponents
	{
		// Token: 0x04001C56 RID: 7254
		Scheme = 1,
		// Token: 0x04001C57 RID: 7255
		UserInfo = 2,
		// Token: 0x04001C58 RID: 7256
		Host = 4,
		// Token: 0x04001C59 RID: 7257
		Port = 8,
		// Token: 0x04001C5A RID: 7258
		Path = 16,
		// Token: 0x04001C5B RID: 7259
		Query = 32,
		// Token: 0x04001C5C RID: 7260
		Fragment = 64,
		// Token: 0x04001C5D RID: 7261
		StrongPort = 128,
		// Token: 0x04001C5E RID: 7262
		KeepDelimiter = 1073741824,
		// Token: 0x04001C5F RID: 7263
		SerializationInfoString = -2147483648,
		// Token: 0x04001C60 RID: 7264
		AbsoluteUri = 127,
		// Token: 0x04001C61 RID: 7265
		HostAndPort = 132,
		// Token: 0x04001C62 RID: 7266
		StrongAuthority = 134,
		// Token: 0x04001C63 RID: 7267
		SchemeAndServer = 13,
		// Token: 0x04001C64 RID: 7268
		HttpRequestUrl = 61,
		// Token: 0x04001C65 RID: 7269
		PathAndQuery = 48
	}
}
