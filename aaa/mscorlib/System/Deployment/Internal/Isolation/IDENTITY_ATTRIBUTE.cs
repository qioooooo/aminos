using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x020001D1 RID: 465
	internal struct IDENTITY_ATTRIBUTE
	{
		// Token: 0x040007F5 RID: 2037
		[MarshalAs(UnmanagedType.LPWStr)]
		public string Namespace;

		// Token: 0x040007F6 RID: 2038
		[MarshalAs(UnmanagedType.LPWStr)]
		public string Name;

		// Token: 0x040007F7 RID: 2039
		[MarshalAs(UnmanagedType.LPWStr)]
		public string Value;
	}
}
