using System;
using System.Runtime.InteropServices;

namespace System.Security.Permissions
{
	// Token: 0x0200064D RID: 1613
	[ComVisible(true)]
	[Flags]
	[Serializable]
	public enum RegistryPermissionAccess
	{
		// Token: 0x04001E38 RID: 7736
		NoAccess = 0,
		// Token: 0x04001E39 RID: 7737
		Read = 1,
		// Token: 0x04001E3A RID: 7738
		Write = 2,
		// Token: 0x04001E3B RID: 7739
		Create = 4,
		// Token: 0x04001E3C RID: 7740
		AllAccess = 7
	}
}
