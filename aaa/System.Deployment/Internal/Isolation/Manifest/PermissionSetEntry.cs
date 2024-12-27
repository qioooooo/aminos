using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x020001AA RID: 426
	[StructLayout(LayoutKind.Sequential)]
	internal class PermissionSetEntry
	{
		// Token: 0x04000729 RID: 1833
		[MarshalAs(UnmanagedType.LPWStr)]
		public string Id;

		// Token: 0x0400072A RID: 1834
		[MarshalAs(UnmanagedType.LPWStr)]
		public string XmlSegment;
	}
}
