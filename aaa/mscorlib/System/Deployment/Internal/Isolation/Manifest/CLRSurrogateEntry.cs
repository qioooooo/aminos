using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x020001AB RID: 427
	[StructLayout(LayoutKind.Sequential)]
	internal class CLRSurrogateEntry
	{
		// Token: 0x04000762 RID: 1890
		public Guid Clsid;

		// Token: 0x04000763 RID: 1891
		[MarshalAs(UnmanagedType.LPWStr)]
		public string RuntimeVersion;

		// Token: 0x04000764 RID: 1892
		[MarshalAs(UnmanagedType.LPWStr)]
		public string ClassName;
	}
}
