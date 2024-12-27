using System;

namespace System.Security.Permissions
{
	// Token: 0x020000D0 RID: 208
	[Flags]
	[Serializable]
	public enum DataProtectionPermissionFlags
	{
		// Token: 0x040005D1 RID: 1489
		NoFlags = 0,
		// Token: 0x040005D2 RID: 1490
		ProtectData = 1,
		// Token: 0x040005D3 RID: 1491
		UnprotectData = 2,
		// Token: 0x040005D4 RID: 1492
		ProtectMemory = 4,
		// Token: 0x040005D5 RID: 1493
		UnprotectMemory = 8,
		// Token: 0x040005D6 RID: 1494
		AllFlags = 15
	}
}
