﻿using System;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x02000160 RID: 352
	internal enum CMS_ASSEMBLY_REFERENCE_FLAG
	{
		// Token: 0x0400061A RID: 1562
		CMS_ASSEMBLY_REFERENCE_FLAG_OPTIONAL = 1,
		// Token: 0x0400061B RID: 1563
		CMS_ASSEMBLY_REFERENCE_FLAG_VISIBLE,
		// Token: 0x0400061C RID: 1564
		CMS_ASSEMBLY_REFERENCE_FLAG_FOLLOW = 4,
		// Token: 0x0400061D RID: 1565
		CMS_ASSEMBLY_REFERENCE_FLAG_IS_PLATFORM = 8,
		// Token: 0x0400061E RID: 1566
		CMS_ASSEMBLY_REFERENCE_FLAG_CULTURE_WILDCARDED = 16,
		// Token: 0x0400061F RID: 1567
		CMS_ASSEMBLY_REFERENCE_FLAG_PROCESSOR_ARCHITECTURE_WILDCARDED = 32,
		// Token: 0x04000620 RID: 1568
		CMS_ASSEMBLY_REFERENCE_FLAG_PREREQUISITE = 128
	}
}
