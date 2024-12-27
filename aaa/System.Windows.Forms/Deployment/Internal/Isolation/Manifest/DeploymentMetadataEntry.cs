using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x020000A1 RID: 161
	[StructLayout(LayoutKind.Sequential)]
	internal class DeploymentMetadataEntry
	{
		// Token: 0x04000CB4 RID: 3252
		[MarshalAs(UnmanagedType.LPWStr)]
		public string DeploymentProviderCodebase;

		// Token: 0x04000CB5 RID: 3253
		[MarshalAs(UnmanagedType.LPWStr)]
		public string MinimumRequiredVersion;

		// Token: 0x04000CB6 RID: 3254
		public ushort MaximumAge;

		// Token: 0x04000CB7 RID: 3255
		public byte MaximumAge_Unit;

		// Token: 0x04000CB8 RID: 3256
		public uint DeploymentFlags;
	}
}
