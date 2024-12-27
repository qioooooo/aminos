using System;

namespace System.Runtime.Serialization.Formatters.Binary
{
	// Token: 0x020007AD RID: 1965
	[Serializable]
	internal enum InternalParseTypeE
	{
		// Token: 0x040022FC RID: 8956
		Empty,
		// Token: 0x040022FD RID: 8957
		SerializedStreamHeader,
		// Token: 0x040022FE RID: 8958
		Object,
		// Token: 0x040022FF RID: 8959
		Member,
		// Token: 0x04002300 RID: 8960
		ObjectEnd,
		// Token: 0x04002301 RID: 8961
		MemberEnd,
		// Token: 0x04002302 RID: 8962
		Headers,
		// Token: 0x04002303 RID: 8963
		HeadersEnd,
		// Token: 0x04002304 RID: 8964
		SerializedStreamHeaderEnd,
		// Token: 0x04002305 RID: 8965
		Envelope,
		// Token: 0x04002306 RID: 8966
		EnvelopeEnd,
		// Token: 0x04002307 RID: 8967
		Body,
		// Token: 0x04002308 RID: 8968
		BodyEnd
	}
}
