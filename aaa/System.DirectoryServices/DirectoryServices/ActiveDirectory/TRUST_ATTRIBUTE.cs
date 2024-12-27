﻿using System;

namespace System.DirectoryServices.ActiveDirectory
{
	// Token: 0x020000F5 RID: 245
	[Flags]
	internal enum TRUST_ATTRIBUTE
	{
		// Token: 0x040005E1 RID: 1505
		TRUST_ATTRIBUTE_NON_TRANSITIVE = 1,
		// Token: 0x040005E2 RID: 1506
		TRUST_ATTRIBUTE_UPLEVEL_ONLY = 2,
		// Token: 0x040005E3 RID: 1507
		TRUST_ATTRIBUTE_QUARANTINED_DOMAIN = 4,
		// Token: 0x040005E4 RID: 1508
		TRUST_ATTRIBUTE_FOREST_TRANSITIVE = 8,
		// Token: 0x040005E5 RID: 1509
		TRUST_ATTRIBUTE_CROSS_ORGANIZATION = 16,
		// Token: 0x040005E6 RID: 1510
		TRUST_ATTRIBUTE_WITHIN_FOREST = 32,
		// Token: 0x040005E7 RID: 1511
		TRUST_ATTRIBUTE_TREAT_AS_EXTERNAL = 64
	}
}
