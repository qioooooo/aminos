using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x020001BD RID: 445
	[StructLayout(LayoutKind.Sequential)]
	internal class PermissionSetEntry
	{
		// Token: 0x04000799 RID: 1945
		[MarshalAs(UnmanagedType.LPWStr)]
		public string Id;

		// Token: 0x0400079A RID: 1946
		[MarshalAs(UnmanagedType.LPWStr)]
		public string XmlSegment;
	}
}
