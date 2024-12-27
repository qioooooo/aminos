using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x02000227 RID: 551
	internal struct IStore_BindingResult_BoundVersion
	{
		// Token: 0x040008CE RID: 2254
		[MarshalAs(UnmanagedType.U2)]
		public ushort Revision;

		// Token: 0x040008CF RID: 2255
		[MarshalAs(UnmanagedType.U2)]
		public ushort Build;

		// Token: 0x040008D0 RID: 2256
		[MarshalAs(UnmanagedType.U2)]
		public ushort Minor;

		// Token: 0x040008D1 RID: 2257
		[MarshalAs(UnmanagedType.U2)]
		public ushort Major;
	}
}
