using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x02000122 RID: 290
	internal struct IStore_BindingResult_BoundVersion
	{
		// Token: 0x04000E42 RID: 3650
		[MarshalAs(UnmanagedType.U2)]
		public ushort Revision;

		// Token: 0x04000E43 RID: 3651
		[MarshalAs(UnmanagedType.U2)]
		public ushort Build;

		// Token: 0x04000E44 RID: 3652
		[MarshalAs(UnmanagedType.U2)]
		public ushort Minor;

		// Token: 0x04000E45 RID: 3653
		[MarshalAs(UnmanagedType.U2)]
		public ushort Major;
	}
}
