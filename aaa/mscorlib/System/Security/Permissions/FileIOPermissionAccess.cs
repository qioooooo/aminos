using System;
using System.Runtime.InteropServices;

namespace System.Security.Permissions
{
	// Token: 0x02000616 RID: 1558
	[ComVisible(true)]
	[Flags]
	[Serializable]
	public enum FileIOPermissionAccess
	{
		// Token: 0x04001D3C RID: 7484
		NoAccess = 0,
		// Token: 0x04001D3D RID: 7485
		Read = 1,
		// Token: 0x04001D3E RID: 7486
		Write = 2,
		// Token: 0x04001D3F RID: 7487
		Append = 4,
		// Token: 0x04001D40 RID: 7488
		PathDiscovery = 8,
		// Token: 0x04001D41 RID: 7489
		AllAccess = 15
	}
}
