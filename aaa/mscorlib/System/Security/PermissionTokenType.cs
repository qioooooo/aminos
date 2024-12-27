using System;

namespace System.Security
{
	// Token: 0x02000664 RID: 1636
	[Flags]
	internal enum PermissionTokenType
	{
		// Token: 0x04001E96 RID: 7830
		Normal = 1,
		// Token: 0x04001E97 RID: 7831
		IUnrestricted = 2,
		// Token: 0x04001E98 RID: 7832
		DontKnow = 4,
		// Token: 0x04001E99 RID: 7833
		BuiltIn = 8
	}
}
