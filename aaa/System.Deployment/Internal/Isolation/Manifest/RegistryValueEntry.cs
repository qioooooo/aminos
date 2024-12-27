using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x020001C5 RID: 453
	[StructLayout(LayoutKind.Sequential)]
	internal class RegistryValueEntry
	{
		// Token: 0x0400079F RID: 1951
		public uint Flags;

		// Token: 0x040007A0 RID: 1952
		public uint OperationHint;

		// Token: 0x040007A1 RID: 1953
		public uint Type;

		// Token: 0x040007A2 RID: 1954
		[MarshalAs(UnmanagedType.LPWStr)]
		public string Value;

		// Token: 0x040007A3 RID: 1955
		[MarshalAs(UnmanagedType.LPWStr)]
		public string BuildFilter;
	}
}
