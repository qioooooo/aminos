using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x020000C6 RID: 198
	internal struct IDENTITY_ATTRIBUTE
	{
		// Token: 0x04000D5F RID: 3423
		[MarshalAs(UnmanagedType.LPWStr)]
		public string Namespace;

		// Token: 0x04000D60 RID: 3424
		[MarshalAs(UnmanagedType.LPWStr)]
		public string Name;

		// Token: 0x04000D61 RID: 3425
		[MarshalAs(UnmanagedType.LPWStr)]
		public string Value;
	}
}
