using System;

namespace System.Security.AccessControl
{
	// Token: 0x02000903 RID: 2307
	[Flags]
	public enum PropagationFlags
	{
		// Token: 0x04002B4F RID: 11087
		None = 0,
		// Token: 0x04002B50 RID: 11088
		NoPropagateInherit = 1,
		// Token: 0x04002B51 RID: 11089
		InheritOnly = 2
	}
}
