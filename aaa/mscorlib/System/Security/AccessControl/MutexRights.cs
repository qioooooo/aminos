using System;

namespace System.Security.AccessControl
{
	// Token: 0x02000913 RID: 2323
	[Flags]
	public enum MutexRights
	{
		// Token: 0x04002B96 RID: 11158
		Modify = 1,
		// Token: 0x04002B97 RID: 11159
		Delete = 65536,
		// Token: 0x04002B98 RID: 11160
		ReadPermissions = 131072,
		// Token: 0x04002B99 RID: 11161
		ChangePermissions = 262144,
		// Token: 0x04002B9A RID: 11162
		TakeOwnership = 524288,
		// Token: 0x04002B9B RID: 11163
		Synchronize = 1048576,
		// Token: 0x04002B9C RID: 11164
		FullControl = 2031617
	}
}
