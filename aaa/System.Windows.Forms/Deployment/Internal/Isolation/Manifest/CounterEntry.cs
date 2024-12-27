using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x020000C2 RID: 194
	[StructLayout(LayoutKind.Sequential)]
	internal class CounterEntry
	{
		// Token: 0x04000D4D RID: 3405
		public Guid CounterSetGuid;

		// Token: 0x04000D4E RID: 3406
		public uint CounterId;

		// Token: 0x04000D4F RID: 3407
		[MarshalAs(UnmanagedType.LPWStr)]
		public string Name;

		// Token: 0x04000D50 RID: 3408
		[MarshalAs(UnmanagedType.LPWStr)]
		public string Description;

		// Token: 0x04000D51 RID: 3409
		public uint CounterType;

		// Token: 0x04000D52 RID: 3410
		public ulong Attributes;

		// Token: 0x04000D53 RID: 3411
		public uint BaseId;

		// Token: 0x04000D54 RID: 3412
		public uint DefaultScale;
	}
}
