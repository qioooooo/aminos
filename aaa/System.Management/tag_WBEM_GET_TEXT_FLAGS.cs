﻿using System;

namespace System.Management
{
	// Token: 0x020000EF RID: 239
	internal enum tag_WBEM_GET_TEXT_FLAGS
	{
		// Token: 0x04000473 RID: 1139
		WBEMPATH_COMPRESSED = 1,
		// Token: 0x04000474 RID: 1140
		WBEMPATH_GET_RELATIVE_ONLY,
		// Token: 0x04000475 RID: 1141
		WBEMPATH_GET_SERVER_TOO = 4,
		// Token: 0x04000476 RID: 1142
		WBEMPATH_GET_SERVER_AND_NAMESPACE_ONLY = 8,
		// Token: 0x04000477 RID: 1143
		WBEMPATH_GET_NAMESPACE_ONLY = 16,
		// Token: 0x04000478 RID: 1144
		WBEMPATH_GET_ORIGINAL = 32
	}
}
