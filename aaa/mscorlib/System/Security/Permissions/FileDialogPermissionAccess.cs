using System;
using System.Runtime.InteropServices;

namespace System.Security.Permissions
{
	// Token: 0x02000614 RID: 1556
	[Flags]
	[ComVisible(true)]
	[Serializable]
	public enum FileDialogPermissionAccess
	{
		// Token: 0x04001D36 RID: 7478
		None = 0,
		// Token: 0x04001D37 RID: 7479
		Open = 1,
		// Token: 0x04001D38 RID: 7480
		Save = 2,
		// Token: 0x04001D39 RID: 7481
		OpenSave = 3
	}
}
