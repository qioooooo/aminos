using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x020001A8 RID: 424
	[StructLayout(LayoutKind.Sequential)]
	internal class ProgIdRedirectionEntry
	{
		// Token: 0x0400075E RID: 1886
		[MarshalAs(UnmanagedType.LPWStr)]
		public string ProgId;

		// Token: 0x0400075F RID: 1887
		public Guid RedirectedGuid;
	}
}
