using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x0200014E RID: 334
	internal struct IStore_BindingResult
	{
		// Token: 0x040005BA RID: 1466
		[MarshalAs(UnmanagedType.U4)]
		public uint Flags;

		// Token: 0x040005BB RID: 1467
		[MarshalAs(UnmanagedType.U4)]
		public uint Disposition;

		// Token: 0x040005BC RID: 1468
		public IStore_BindingResult_BoundVersion Component;

		// Token: 0x040005BD RID: 1469
		public Guid CacheCoherencyGuid;

		// Token: 0x040005BE RID: 1470
		[MarshalAs(UnmanagedType.SysInt)]
		public IntPtr Reserved;
	}
}
