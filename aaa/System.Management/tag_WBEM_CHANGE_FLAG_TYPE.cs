﻿using System;

namespace System.Management
{
	// Token: 0x020000D3 RID: 211
	internal enum tag_WBEM_CHANGE_FLAG_TYPE
	{
		// Token: 0x040002EB RID: 747
		WBEM_FLAG_CREATE_OR_UPDATE,
		// Token: 0x040002EC RID: 748
		WBEM_FLAG_UPDATE_ONLY,
		// Token: 0x040002ED RID: 749
		WBEM_FLAG_CREATE_ONLY,
		// Token: 0x040002EE RID: 750
		WBEM_FLAG_UPDATE_COMPATIBLE = 0,
		// Token: 0x040002EF RID: 751
		WBEM_FLAG_UPDATE_SAFE_MODE = 32,
		// Token: 0x040002F0 RID: 752
		WBEM_FLAG_UPDATE_FORCE_MODE = 64,
		// Token: 0x040002F1 RID: 753
		WBEM_MASK_UPDATE_MODE = 96,
		// Token: 0x040002F2 RID: 754
		WBEM_FLAG_ADVISORY = 65536
	}
}
