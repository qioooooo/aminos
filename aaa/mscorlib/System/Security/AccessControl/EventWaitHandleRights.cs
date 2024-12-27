using System;

namespace System.Security.AccessControl
{
	// Token: 0x02000909 RID: 2313
	[Flags]
	public enum EventWaitHandleRights
	{
		// Token: 0x04002B75 RID: 11125
		Modify = 2,
		// Token: 0x04002B76 RID: 11126
		Delete = 65536,
		// Token: 0x04002B77 RID: 11127
		ReadPermissions = 131072,
		// Token: 0x04002B78 RID: 11128
		ChangePermissions = 262144,
		// Token: 0x04002B79 RID: 11129
		TakeOwnership = 524288,
		// Token: 0x04002B7A RID: 11130
		Synchronize = 1048576,
		// Token: 0x04002B7B RID: 11131
		FullControl = 2031619
	}
}
