using System;
using System.Runtime.InteropServices;

namespace System.Security.Permissions
{
	// Token: 0x02000624 RID: 1572
	[ComVisible(true)]
	[Serializable]
	public enum SecurityAction
	{
		// Token: 0x04001D9F RID: 7583
		Demand = 2,
		// Token: 0x04001DA0 RID: 7584
		Assert,
		// Token: 0x04001DA1 RID: 7585
		Deny,
		// Token: 0x04001DA2 RID: 7586
		PermitOnly,
		// Token: 0x04001DA3 RID: 7587
		LinkDemand,
		// Token: 0x04001DA4 RID: 7588
		InheritanceDemand,
		// Token: 0x04001DA5 RID: 7589
		RequestMinimum,
		// Token: 0x04001DA6 RID: 7590
		RequestOptional,
		// Token: 0x04001DA7 RID: 7591
		RequestRefuse
	}
}
