using System;

namespace System.Runtime.Serialization.Formatters.Binary
{
	// Token: 0x020007B3 RID: 1971
	[Serializable]
	internal enum InternalParseStateE
	{
		// Token: 0x04002324 RID: 8996
		Initial,
		// Token: 0x04002325 RID: 8997
		Object,
		// Token: 0x04002326 RID: 8998
		Member,
		// Token: 0x04002327 RID: 8999
		MemberChild
	}
}
