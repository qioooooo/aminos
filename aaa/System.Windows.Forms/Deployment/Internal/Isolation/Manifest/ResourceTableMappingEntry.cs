using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x02000092 RID: 146
	[StructLayout(LayoutKind.Sequential)]
	internal class ResourceTableMappingEntry
	{
		// Token: 0x04000C91 RID: 3217
		[MarshalAs(UnmanagedType.LPWStr)]
		public string id;

		// Token: 0x04000C92 RID: 3218
		[MarshalAs(UnmanagedType.LPWStr)]
		public string FinalStringMapped;
	}
}
