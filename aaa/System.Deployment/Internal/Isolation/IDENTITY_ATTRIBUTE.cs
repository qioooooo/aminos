using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x020000F1 RID: 241
	internal struct IDENTITY_ATTRIBUTE
	{
		// Token: 0x040004D3 RID: 1235
		[MarshalAs(UnmanagedType.LPWStr)]
		public string Namespace;

		// Token: 0x040004D4 RID: 1236
		[MarshalAs(UnmanagedType.LPWStr)]
		public string Name;

		// Token: 0x040004D5 RID: 1237
		[MarshalAs(UnmanagedType.LPWStr)]
		public string Value;
	}
}
