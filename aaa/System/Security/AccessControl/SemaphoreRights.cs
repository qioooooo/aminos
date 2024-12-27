using System;
using System.Runtime.InteropServices;

namespace System.Security.AccessControl
{
	// Token: 0x02000223 RID: 547
	[ComVisible(false)]
	[Flags]
	public enum SemaphoreRights
	{
		// Token: 0x040010BD RID: 4285
		Modify = 2,
		// Token: 0x040010BE RID: 4286
		Delete = 65536,
		// Token: 0x040010BF RID: 4287
		ReadPermissions = 131072,
		// Token: 0x040010C0 RID: 4288
		ChangePermissions = 262144,
		// Token: 0x040010C1 RID: 4289
		TakeOwnership = 524288,
		// Token: 0x040010C2 RID: 4290
		Synchronize = 1048576,
		// Token: 0x040010C3 RID: 4291
		FullControl = 2031619
	}
}
