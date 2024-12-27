using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x020001CE RID: 462
	[StructLayout(LayoutKind.Sequential)]
	internal class SecurityDescriptorReferenceEntry
	{
		// Token: 0x040007C8 RID: 1992
		[MarshalAs(UnmanagedType.LPWStr)]
		public string Name;

		// Token: 0x040007C9 RID: 1993
		[MarshalAs(UnmanagedType.LPWStr)]
		public string BuildFilter;
	}
}
