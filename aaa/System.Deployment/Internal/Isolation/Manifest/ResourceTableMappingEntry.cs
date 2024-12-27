using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x020001A4 RID: 420
	[StructLayout(LayoutKind.Sequential)]
	internal class ResourceTableMappingEntry
	{
		// Token: 0x0400071B RID: 1819
		[MarshalAs(UnmanagedType.LPWStr)]
		public string id;

		// Token: 0x0400071C RID: 1820
		[MarshalAs(UnmanagedType.LPWStr)]
		public string FinalStringMapped;
	}
}
