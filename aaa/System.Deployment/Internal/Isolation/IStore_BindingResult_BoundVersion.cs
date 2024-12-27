using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x0200014D RID: 333
	internal struct IStore_BindingResult_BoundVersion
	{
		// Token: 0x040005B6 RID: 1462
		[MarshalAs(UnmanagedType.U2)]
		public ushort Revision;

		// Token: 0x040005B7 RID: 1463
		[MarshalAs(UnmanagedType.U2)]
		public ushort Build;

		// Token: 0x040005B8 RID: 1464
		[MarshalAs(UnmanagedType.U2)]
		public ushort Minor;

		// Token: 0x040005B9 RID: 1465
		[MarshalAs(UnmanagedType.U2)]
		public ushort Major;
	}
}
