using System;

namespace System.Security.AccessControl
{
	// Token: 0x02000905 RID: 2309
	[Flags]
	public enum SecurityInfos
	{
		// Token: 0x04002B57 RID: 11095
		Owner = 1,
		// Token: 0x04002B58 RID: 11096
		Group = 2,
		// Token: 0x04002B59 RID: 11097
		DiscretionaryAcl = 4,
		// Token: 0x04002B5A RID: 11098
		SystemAcl = 8
	}
}
