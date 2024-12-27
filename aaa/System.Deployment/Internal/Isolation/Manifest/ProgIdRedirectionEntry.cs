using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x02000195 RID: 405
	[StructLayout(LayoutKind.Sequential)]
	internal class ProgIdRedirectionEntry
	{
		// Token: 0x040006EE RID: 1774
		[MarshalAs(UnmanagedType.LPWStr)]
		public string ProgId;

		// Token: 0x040006EF RID: 1775
		public Guid RedirectedGuid;
	}
}
