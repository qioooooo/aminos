using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x020001BA RID: 442
	[StructLayout(LayoutKind.Sequential)]
	internal class EntryPointEntry
	{
		// Token: 0x0400078F RID: 1935
		[MarshalAs(UnmanagedType.LPWStr)]
		public string Name;

		// Token: 0x04000790 RID: 1936
		[MarshalAs(UnmanagedType.LPWStr)]
		public string CommandLine_File;

		// Token: 0x04000791 RID: 1937
		[MarshalAs(UnmanagedType.LPWStr)]
		public string CommandLine_Parameters;

		// Token: 0x04000792 RID: 1938
		public IReferenceIdentity Identity;

		// Token: 0x04000793 RID: 1939
		public uint Flags;
	}
}
