using System;
using System.Runtime.InteropServices;

namespace System.Security.Permissions
{
	// Token: 0x02000647 RID: 1607
	[ComVisible(true)]
	[Flags]
	[Serializable]
	public enum KeyContainerPermissionFlags
	{
		// Token: 0x04001E1E RID: 7710
		NoFlags = 0,
		// Token: 0x04001E1F RID: 7711
		Create = 1,
		// Token: 0x04001E20 RID: 7712
		Open = 2,
		// Token: 0x04001E21 RID: 7713
		Delete = 4,
		// Token: 0x04001E22 RID: 7714
		Import = 16,
		// Token: 0x04001E23 RID: 7715
		Export = 32,
		// Token: 0x04001E24 RID: 7716
		Sign = 256,
		// Token: 0x04001E25 RID: 7717
		Decrypt = 512,
		// Token: 0x04001E26 RID: 7718
		ViewAcl = 4096,
		// Token: 0x04001E27 RID: 7719
		ChangeAcl = 8192,
		// Token: 0x04001E28 RID: 7720
		AllFlags = 13111
	}
}
