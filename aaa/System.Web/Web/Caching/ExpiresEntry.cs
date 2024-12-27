using System;
using System.Runtime.InteropServices;

namespace System.Web.Caching
{
	// Token: 0x0200011D RID: 285
	[StructLayout(LayoutKind.Explicit)]
	internal struct ExpiresEntry
	{
		// Token: 0x040014B8 RID: 5304
		[FieldOffset(0)]
		internal DateTime _utcExpires;

		// Token: 0x040014B9 RID: 5305
		[FieldOffset(0)]
		internal ExpiresEntryRef _next;

		// Token: 0x040014BA RID: 5306
		[FieldOffset(4)]
		internal int _cFree;

		// Token: 0x040014BB RID: 5307
		[FieldOffset(8)]
		internal CacheEntry _cacheEntry;
	}
}
