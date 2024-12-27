using System;

namespace System.Web.Services.Protocols
{
	// Token: 0x02000070 RID: 112
	[Flags]
	public enum SoapHeaderDirection
	{
		// Token: 0x04000334 RID: 820
		In = 1,
		// Token: 0x04000335 RID: 821
		Out = 2,
		// Token: 0x04000336 RID: 822
		InOut = 3,
		// Token: 0x04000337 RID: 823
		Fault = 4
	}
}
