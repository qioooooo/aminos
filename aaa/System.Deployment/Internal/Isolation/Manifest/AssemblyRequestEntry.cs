using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x020001AD RID: 429
	[StructLayout(LayoutKind.Sequential)]
	internal class AssemblyRequestEntry
	{
		// Token: 0x0400072D RID: 1837
		[MarshalAs(UnmanagedType.LPWStr)]
		public string Name;

		// Token: 0x0400072E RID: 1838
		[MarshalAs(UnmanagedType.LPWStr)]
		public string permissionSetID;
	}
}
