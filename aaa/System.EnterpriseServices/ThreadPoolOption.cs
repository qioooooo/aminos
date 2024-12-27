using System;
using System.Runtime.InteropServices;

namespace System.EnterpriseServices
{
	// Token: 0x020000E2 RID: 226
	[ComVisible(false)]
	[Serializable]
	public enum ThreadPoolOption
	{
		// Token: 0x04000210 RID: 528
		None,
		// Token: 0x04000211 RID: 529
		Inherit,
		// Token: 0x04000212 RID: 530
		STA,
		// Token: 0x04000213 RID: 531
		MTA
	}
}
