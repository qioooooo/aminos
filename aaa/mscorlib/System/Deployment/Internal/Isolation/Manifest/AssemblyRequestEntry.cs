using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x020001C0 RID: 448
	[StructLayout(LayoutKind.Sequential)]
	internal class AssemblyRequestEntry
	{
		// Token: 0x0400079D RID: 1949
		[MarshalAs(UnmanagedType.LPWStr)]
		public string Name;

		// Token: 0x0400079E RID: 1950
		[MarshalAs(UnmanagedType.LPWStr)]
		public string permissionSetID;
	}
}
