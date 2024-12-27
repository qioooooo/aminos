using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x020001D1 RID: 465
	[StructLayout(LayoutKind.Sequential)]
	internal class CounterSetEntry
	{
		// Token: 0x040007CD RID: 1997
		public Guid CounterSetGuid;

		// Token: 0x040007CE RID: 1998
		public Guid ProviderGuid;

		// Token: 0x040007CF RID: 1999
		[MarshalAs(UnmanagedType.LPWStr)]
		public string Name;

		// Token: 0x040007D0 RID: 2000
		[MarshalAs(UnmanagedType.LPWStr)]
		public string Description;

		// Token: 0x040007D1 RID: 2001
		public bool InstanceType;
	}
}
