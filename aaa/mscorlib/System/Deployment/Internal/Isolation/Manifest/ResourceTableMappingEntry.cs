using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x020001B7 RID: 439
	[StructLayout(LayoutKind.Sequential)]
	internal class ResourceTableMappingEntry
	{
		// Token: 0x0400078B RID: 1931
		[MarshalAs(UnmanagedType.LPWStr)]
		public string id;

		// Token: 0x0400078C RID: 1932
		[MarshalAs(UnmanagedType.LPWStr)]
		public string FinalStringMapped;
	}
}
