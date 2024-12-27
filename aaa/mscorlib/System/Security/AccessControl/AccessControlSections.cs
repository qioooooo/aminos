using System;

namespace System.Security.AccessControl
{
	// Token: 0x02000907 RID: 2311
	[Flags]
	public enum AccessControlSections
	{
		// Token: 0x04002B6A RID: 11114
		None = 0,
		// Token: 0x04002B6B RID: 11115
		Audit = 1,
		// Token: 0x04002B6C RID: 11116
		Access = 2,
		// Token: 0x04002B6D RID: 11117
		Owner = 4,
		// Token: 0x04002B6E RID: 11118
		Group = 8,
		// Token: 0x04002B6F RID: 11119
		All = 15
	}
}
