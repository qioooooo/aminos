using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x02000228 RID: 552
	internal struct IStore_BindingResult
	{
		// Token: 0x040008D2 RID: 2258
		[MarshalAs(UnmanagedType.U4)]
		public uint Flags;

		// Token: 0x040008D3 RID: 2259
		[MarshalAs(UnmanagedType.U4)]
		public uint Disposition;

		// Token: 0x040008D4 RID: 2260
		public IStore_BindingResult_BoundVersion Component;

		// Token: 0x040008D5 RID: 2261
		public Guid CacheCoherencyGuid;

		// Token: 0x040008D6 RID: 2262
		[MarshalAs(UnmanagedType.SysInt)]
		public IntPtr Reserved;
	}
}
