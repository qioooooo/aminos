using System;

namespace System.Configuration
{
	// Token: 0x02000042 RID: 66
	[Flags]
	internal enum ConfigurationValueFlags
	{
		// Token: 0x040002AC RID: 684
		Default = 0,
		// Token: 0x040002AD RID: 685
		Inherited = 1,
		// Token: 0x040002AE RID: 686
		Modified = 2,
		// Token: 0x040002AF RID: 687
		Locked = 4,
		// Token: 0x040002B0 RID: 688
		XMLParentInherited = 8
	}
}
