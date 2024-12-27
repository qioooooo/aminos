using System;

namespace System.Web.Caching
{
	// Token: 0x020000F7 RID: 247
	public enum CacheItemPriority
	{
		// Token: 0x04001398 RID: 5016
		Low = 1,
		// Token: 0x04001399 RID: 5017
		BelowNormal,
		// Token: 0x0400139A RID: 5018
		Normal,
		// Token: 0x0400139B RID: 5019
		AboveNormal,
		// Token: 0x0400139C RID: 5020
		High,
		// Token: 0x0400139D RID: 5021
		NotRemovable,
		// Token: 0x0400139E RID: 5022
		Default = 3
	}
}
