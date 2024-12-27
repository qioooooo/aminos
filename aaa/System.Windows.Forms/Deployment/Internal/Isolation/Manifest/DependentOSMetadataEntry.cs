using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x020000A4 RID: 164
	[StructLayout(LayoutKind.Sequential)]
	internal class DependentOSMetadataEntry
	{
		// Token: 0x04000CBF RID: 3263
		[MarshalAs(UnmanagedType.LPWStr)]
		public string SupportUrl;

		// Token: 0x04000CC0 RID: 3264
		[MarshalAs(UnmanagedType.LPWStr)]
		public string Description;

		// Token: 0x04000CC1 RID: 3265
		public ushort MajorVersion;

		// Token: 0x04000CC2 RID: 3266
		public ushort MinorVersion;

		// Token: 0x04000CC3 RID: 3267
		public ushort BuildNumber;

		// Token: 0x04000CC4 RID: 3268
		public byte ServicePackMajor;

		// Token: 0x04000CC5 RID: 3269
		public byte ServicePackMinor;
	}
}
