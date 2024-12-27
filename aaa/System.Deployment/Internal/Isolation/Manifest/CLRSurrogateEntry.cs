using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x02000198 RID: 408
	[StructLayout(LayoutKind.Sequential)]
	internal class CLRSurrogateEntry
	{
		// Token: 0x040006F2 RID: 1778
		public Guid Clsid;

		// Token: 0x040006F3 RID: 1779
		[MarshalAs(UnmanagedType.LPWStr)]
		public string RuntimeVersion;

		// Token: 0x040006F4 RID: 1780
		[MarshalAs(UnmanagedType.LPWStr)]
		public string ClassName;
	}
}
