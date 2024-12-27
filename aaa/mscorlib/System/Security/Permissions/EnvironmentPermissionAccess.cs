using System;
using System.Runtime.InteropServices;

namespace System.Security.Permissions
{
	// Token: 0x0200060C RID: 1548
	[ComVisible(true)]
	[Flags]
	[Serializable]
	public enum EnvironmentPermissionAccess
	{
		// Token: 0x04001D2E RID: 7470
		NoAccess = 0,
		// Token: 0x04001D2F RID: 7471
		Read = 1,
		// Token: 0x04001D30 RID: 7472
		Write = 2,
		// Token: 0x04001D31 RID: 7473
		AllAccess = 3
	}
}
