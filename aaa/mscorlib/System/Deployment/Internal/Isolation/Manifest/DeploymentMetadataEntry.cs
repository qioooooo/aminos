using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x020001C6 RID: 454
	[StructLayout(LayoutKind.Sequential)]
	internal class DeploymentMetadataEntry
	{
		// Token: 0x040007AE RID: 1966
		[MarshalAs(UnmanagedType.LPWStr)]
		public string DeploymentProviderCodebase;

		// Token: 0x040007AF RID: 1967
		[MarshalAs(UnmanagedType.LPWStr)]
		public string MinimumRequiredVersion;

		// Token: 0x040007B0 RID: 1968
		public ushort MaximumAge;

		// Token: 0x040007B1 RID: 1969
		public byte MaximumAge_Unit;

		// Token: 0x040007B2 RID: 1970
		public uint DeploymentFlags;
	}
}
