using System;
using System.Runtime.InteropServices;

namespace System.Security.Principal
{
	// Token: 0x020004B4 RID: 1204
	[ComVisible(true)]
	[Serializable]
	public enum TokenImpersonationLevel
	{
		// Token: 0x0400183A RID: 6202
		None,
		// Token: 0x0400183B RID: 6203
		Anonymous,
		// Token: 0x0400183C RID: 6204
		Identification,
		// Token: 0x0400183D RID: 6205
		Impersonation,
		// Token: 0x0400183E RID: 6206
		Delegation
	}
}
