using System;

namespace System.Runtime.Serialization.Formatters.Soap
{
	// Token: 0x0200001F RID: 31
	[Serializable]
	internal enum InternalParseStateE
	{
		// Token: 0x040000E9 RID: 233
		Initial,
		// Token: 0x040000EA RID: 234
		Object,
		// Token: 0x040000EB RID: 235
		Member,
		// Token: 0x040000EC RID: 236
		MemberChild
	}
}
