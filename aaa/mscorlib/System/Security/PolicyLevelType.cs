using System;
using System.Runtime.InteropServices;

namespace System.Security
{
	// Token: 0x0200067C RID: 1660
	[ComVisible(true)]
	[Serializable]
	public enum PolicyLevelType
	{
		// Token: 0x04001EF8 RID: 7928
		User,
		// Token: 0x04001EF9 RID: 7929
		Machine,
		// Token: 0x04001EFA RID: 7930
		Enterprise,
		// Token: 0x04001EFB RID: 7931
		AppDomain
	}
}
