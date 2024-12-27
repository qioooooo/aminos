﻿using System;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x0200017A RID: 378
	internal enum CMS_ASSEMBLY_REFERENCE_FLAG
	{
		// Token: 0x040006B5 RID: 1717
		CMS_ASSEMBLY_REFERENCE_FLAG_OPTIONAL = 1,
		// Token: 0x040006B6 RID: 1718
		CMS_ASSEMBLY_REFERENCE_FLAG_VISIBLE,
		// Token: 0x040006B7 RID: 1719
		CMS_ASSEMBLY_REFERENCE_FLAG_FOLLOW = 4,
		// Token: 0x040006B8 RID: 1720
		CMS_ASSEMBLY_REFERENCE_FLAG_IS_PLATFORM = 8,
		// Token: 0x040006B9 RID: 1721
		CMS_ASSEMBLY_REFERENCE_FLAG_CULTURE_WILDCARDED = 16,
		// Token: 0x040006BA RID: 1722
		CMS_ASSEMBLY_REFERENCE_FLAG_PROCESSOR_ARCHITECTURE_WILDCARDED = 32,
		// Token: 0x040006BB RID: 1723
		CMS_ASSEMBLY_REFERENCE_FLAG_PREREQUISITE = 128
	}
}