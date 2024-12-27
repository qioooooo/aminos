using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x020000BC RID: 188
	[StructLayout(LayoutKind.Sequential)]
	internal class SecurityDescriptorReferenceEntry
	{
		// Token: 0x04000D3E RID: 3390
		[MarshalAs(UnmanagedType.LPWStr)]
		public string Name;

		// Token: 0x04000D3F RID: 3391
		[MarshalAs(UnmanagedType.LPWStr)]
		public string BuildFilter;
	}
}
