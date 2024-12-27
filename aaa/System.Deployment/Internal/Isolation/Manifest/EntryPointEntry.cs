using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x020001A7 RID: 423
	[StructLayout(LayoutKind.Sequential)]
	internal class EntryPointEntry
	{
		// Token: 0x0400071F RID: 1823
		[MarshalAs(UnmanagedType.LPWStr)]
		public string Name;

		// Token: 0x04000720 RID: 1824
		[MarshalAs(UnmanagedType.LPWStr)]
		public string CommandLine_File;

		// Token: 0x04000721 RID: 1825
		[MarshalAs(UnmanagedType.LPWStr)]
		public string CommandLine_Parameters;

		// Token: 0x04000722 RID: 1826
		public IReferenceIdentity Identity;

		// Token: 0x04000723 RID: 1827
		public uint Flags;
	}
}
