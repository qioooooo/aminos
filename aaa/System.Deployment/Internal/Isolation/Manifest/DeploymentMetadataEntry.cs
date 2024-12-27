using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x020001B3 RID: 435
	[StructLayout(LayoutKind.Sequential)]
	internal class DeploymentMetadataEntry
	{
		// Token: 0x0400073E RID: 1854
		[MarshalAs(UnmanagedType.LPWStr)]
		public string DeploymentProviderCodebase;

		// Token: 0x0400073F RID: 1855
		[MarshalAs(UnmanagedType.LPWStr)]
		public string MinimumRequiredVersion;

		// Token: 0x04000740 RID: 1856
		public ushort MaximumAge;

		// Token: 0x04000741 RID: 1857
		public byte MaximumAge_Unit;

		// Token: 0x04000742 RID: 1858
		public uint DeploymentFlags;
	}
}
