using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x02000123 RID: 291
	internal struct IStore_BindingResult
	{
		// Token: 0x04000E46 RID: 3654
		[MarshalAs(UnmanagedType.U4)]
		public uint Flags;

		// Token: 0x04000E47 RID: 3655
		[MarshalAs(UnmanagedType.U4)]
		public uint Disposition;

		// Token: 0x04000E48 RID: 3656
		public IStore_BindingResult_BoundVersion Component;

		// Token: 0x04000E49 RID: 3657
		public Guid CacheCoherencyGuid;

		// Token: 0x04000E4A RID: 3658
		[MarshalAs(UnmanagedType.SysInt)]
		public IntPtr Reserved;
	}
}
