using System;
using System.Runtime.InteropServices;

namespace System.Security.Principal
{
	// Token: 0x020004B5 RID: 1205
	[ComVisible(true)]
	[Flags]
	[Serializable]
	public enum TokenAccessLevels
	{
		// Token: 0x04001840 RID: 6208
		AssignPrimary = 1,
		// Token: 0x04001841 RID: 6209
		Duplicate = 2,
		// Token: 0x04001842 RID: 6210
		Impersonate = 4,
		// Token: 0x04001843 RID: 6211
		Query = 8,
		// Token: 0x04001844 RID: 6212
		QuerySource = 16,
		// Token: 0x04001845 RID: 6213
		AdjustPrivileges = 32,
		// Token: 0x04001846 RID: 6214
		AdjustGroups = 64,
		// Token: 0x04001847 RID: 6215
		AdjustDefault = 128,
		// Token: 0x04001848 RID: 6216
		AdjustSessionId = 256,
		// Token: 0x04001849 RID: 6217
		Read = 131080,
		// Token: 0x0400184A RID: 6218
		Write = 131296,
		// Token: 0x0400184B RID: 6219
		AllAccess = 983551,
		// Token: 0x0400184C RID: 6220
		MaximumAllowed = 33554432
	}
}
