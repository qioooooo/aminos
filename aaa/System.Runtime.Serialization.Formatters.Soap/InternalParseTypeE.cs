using System;

namespace System.Runtime.Serialization.Formatters.Soap
{
	// Token: 0x02000019 RID: 25
	[Serializable]
	internal enum InternalParseTypeE
	{
		// Token: 0x040000C1 RID: 193
		Empty,
		// Token: 0x040000C2 RID: 194
		SerializedStreamHeader,
		// Token: 0x040000C3 RID: 195
		Object,
		// Token: 0x040000C4 RID: 196
		Member,
		// Token: 0x040000C5 RID: 197
		ObjectEnd,
		// Token: 0x040000C6 RID: 198
		MemberEnd,
		// Token: 0x040000C7 RID: 199
		Headers,
		// Token: 0x040000C8 RID: 200
		HeadersEnd,
		// Token: 0x040000C9 RID: 201
		SerializedStreamHeaderEnd,
		// Token: 0x040000CA RID: 202
		Envelope,
		// Token: 0x040000CB RID: 203
		EnvelopeEnd,
		// Token: 0x040000CC RID: 204
		Body,
		// Token: 0x040000CD RID: 205
		BodyEnd
	}
}
