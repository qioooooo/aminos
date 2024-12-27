using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x020001A1 RID: 417
	[StructLayout(LayoutKind.Sequential)]
	internal class WindowClassEntry
	{
		// Token: 0x04000715 RID: 1813
		[MarshalAs(UnmanagedType.LPWStr)]
		public string ClassName;

		// Token: 0x04000716 RID: 1814
		[MarshalAs(UnmanagedType.LPWStr)]
		public string HostDll;

		// Token: 0x04000717 RID: 1815
		public bool fVersioned;
	}
}
