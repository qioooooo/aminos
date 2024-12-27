using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x0200008F RID: 143
	[StructLayout(LayoutKind.Sequential)]
	internal class WindowClassEntry
	{
		// Token: 0x04000C8B RID: 3211
		[MarshalAs(UnmanagedType.LPWStr)]
		public string ClassName;

		// Token: 0x04000C8C RID: 3212
		[MarshalAs(UnmanagedType.LPWStr)]
		public string HostDll;

		// Token: 0x04000C8D RID: 3213
		public bool fVersioned;
	}
}
