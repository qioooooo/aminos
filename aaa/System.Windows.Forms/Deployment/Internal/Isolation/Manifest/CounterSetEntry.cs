using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x020000BF RID: 191
	[StructLayout(LayoutKind.Sequential)]
	internal class CounterSetEntry
	{
		// Token: 0x04000D43 RID: 3395
		public Guid CounterSetGuid;

		// Token: 0x04000D44 RID: 3396
		public Guid ProviderGuid;

		// Token: 0x04000D45 RID: 3397
		[MarshalAs(UnmanagedType.LPWStr)]
		public string Name;

		// Token: 0x04000D46 RID: 3398
		[MarshalAs(UnmanagedType.LPWStr)]
		public string Description;

		// Token: 0x04000D47 RID: 3399
		public bool InstanceType;
	}
}
