using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x02000095 RID: 149
	[StructLayout(LayoutKind.Sequential)]
	internal class EntryPointEntry
	{
		// Token: 0x04000C95 RID: 3221
		[MarshalAs(UnmanagedType.LPWStr)]
		public string Name;

		// Token: 0x04000C96 RID: 3222
		[MarshalAs(UnmanagedType.LPWStr)]
		public string CommandLine_File;

		// Token: 0x04000C97 RID: 3223
		[MarshalAs(UnmanagedType.LPWStr)]
		public string CommandLine_Parameters;

		// Token: 0x04000C98 RID: 3224
		public IReferenceIdentity Identity;

		// Token: 0x04000C99 RID: 3225
		public uint Flags;
	}
}
