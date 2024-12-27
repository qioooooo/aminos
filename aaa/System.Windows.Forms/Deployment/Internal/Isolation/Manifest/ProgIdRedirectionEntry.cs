using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x02000083 RID: 131
	[StructLayout(LayoutKind.Sequential)]
	internal class ProgIdRedirectionEntry
	{
		// Token: 0x04000C64 RID: 3172
		[MarshalAs(UnmanagedType.LPWStr)]
		public string ProgId;

		// Token: 0x04000C65 RID: 3173
		public Guid RedirectedGuid;
	}
}
