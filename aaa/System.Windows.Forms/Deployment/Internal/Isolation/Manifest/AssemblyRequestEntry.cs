using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x0200009B RID: 155
	[StructLayout(LayoutKind.Sequential)]
	internal class AssemblyRequestEntry
	{
		// Token: 0x04000CA3 RID: 3235
		[MarshalAs(UnmanagedType.LPWStr)]
		public string Name;

		// Token: 0x04000CA4 RID: 3236
		[MarshalAs(UnmanagedType.LPWStr)]
		public string permissionSetID;
	}
}
