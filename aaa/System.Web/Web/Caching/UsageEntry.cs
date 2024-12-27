using System;
using System.Runtime.InteropServices;

namespace System.Web.Caching
{
	// Token: 0x02000117 RID: 279
	[StructLayout(LayoutKind.Explicit)]
	internal struct UsageEntry
	{
		// Token: 0x0400148F RID: 5263
		[FieldOffset(0)]
		internal UsageEntryLink _ref1;

		// Token: 0x04001490 RID: 5264
		[FieldOffset(4)]
		internal int _cFree;

		// Token: 0x04001491 RID: 5265
		[FieldOffset(8)]
		internal UsageEntryLink _ref2;

		// Token: 0x04001492 RID: 5266
		[FieldOffset(16)]
		internal DateTime _utcDate;

		// Token: 0x04001493 RID: 5267
		[FieldOffset(24)]
		internal CacheEntry _cacheEntry;
	}
}
