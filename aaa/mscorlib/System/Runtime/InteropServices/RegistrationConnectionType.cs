using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x0200051A RID: 1306
	[Flags]
	public enum RegistrationConnectionType
	{
		// Token: 0x040019DF RID: 6623
		SingleUse = 0,
		// Token: 0x040019E0 RID: 6624
		MultipleUse = 1,
		// Token: 0x040019E1 RID: 6625
		MultiSeparate = 2,
		// Token: 0x040019E2 RID: 6626
		Suspended = 4,
		// Token: 0x040019E3 RID: 6627
		Surrogate = 8
	}
}
