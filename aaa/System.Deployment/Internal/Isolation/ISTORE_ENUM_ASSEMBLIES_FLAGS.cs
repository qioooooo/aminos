﻿using System;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x02000120 RID: 288
	[Flags]
	internal enum ISTORE_ENUM_ASSEMBLIES_FLAGS
	{
		// Token: 0x04000519 RID: 1305
		ISTORE_ENUM_ASSEMBLIES_FLAG_LIMIT_TO_VISIBLE_ONLY = 1,
		// Token: 0x0400051A RID: 1306
		ISTORE_ENUM_ASSEMBLIES_FLAG_MATCH_SERVICING = 2,
		// Token: 0x0400051B RID: 1307
		ISTORE_ENUM_ASSEMBLIES_FLAG_FORCE_LIBRARY_SEMANTICS = 4
	}
}
