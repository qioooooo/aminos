using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x020001B4 RID: 436
	[StructLayout(LayoutKind.Sequential)]
	internal class WindowClassEntry
	{
		// Token: 0x04000785 RID: 1925
		[MarshalAs(UnmanagedType.LPWStr)]
		public string ClassName;

		// Token: 0x04000786 RID: 1926
		[MarshalAs(UnmanagedType.LPWStr)]
		public string HostDll;

		// Token: 0x04000787 RID: 1927
		public bool fVersioned;
	}
}
