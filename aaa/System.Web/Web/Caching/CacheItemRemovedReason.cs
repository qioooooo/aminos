using System;

namespace System.Web.Caching
{
	// Token: 0x020000F8 RID: 248
	public enum CacheItemRemovedReason
	{
		// Token: 0x040013A0 RID: 5024
		Removed = 1,
		// Token: 0x040013A1 RID: 5025
		Expired,
		// Token: 0x040013A2 RID: 5026
		Underused,
		// Token: 0x040013A3 RID: 5027
		DependencyChanged
	}
}
