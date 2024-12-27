using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x020000B3 RID: 179
	[StructLayout(LayoutKind.Sequential)]
	internal class RegistryValueEntry
	{
		// Token: 0x04000D15 RID: 3349
		public uint Flags;

		// Token: 0x04000D16 RID: 3350
		public uint OperationHint;

		// Token: 0x04000D17 RID: 3351
		public uint Type;

		// Token: 0x04000D18 RID: 3352
		[MarshalAs(UnmanagedType.LPWStr)]
		public string Value;

		// Token: 0x04000D19 RID: 3353
		[MarshalAs(UnmanagedType.LPWStr)]
		public string BuildFilter;
	}
}
