using System;
using System.Runtime.InteropServices;

namespace System.Security.Permissions
{
	// Token: 0x02000636 RID: 1590
	[ComVisible(true)]
	[Flags]
	[Serializable]
	public enum ReflectionPermissionFlag
	{
		// Token: 0x04001DD6 RID: 7638
		NoFlags = 0,
		// Token: 0x04001DD7 RID: 7639
		[Obsolete("This API has been deprecated. http://go.microsoft.com/fwlink/?linkid=14202")]
		TypeInformation = 1,
		// Token: 0x04001DD8 RID: 7640
		MemberAccess = 2,
		// Token: 0x04001DD9 RID: 7641
		ReflectionEmit = 4,
		// Token: 0x04001DDA RID: 7642
		[ComVisible(false)]
		RestrictedMemberAccess = 8,
		// Token: 0x04001DDB RID: 7643
		AllFlags = 7
	}
}
