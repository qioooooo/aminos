using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x020001C9 RID: 457
	[StructLayout(LayoutKind.Sequential)]
	internal class DependentOSMetadataEntry
	{
		// Token: 0x040007B9 RID: 1977
		[MarshalAs(UnmanagedType.LPWStr)]
		public string SupportUrl;

		// Token: 0x040007BA RID: 1978
		[MarshalAs(UnmanagedType.LPWStr)]
		public string Description;

		// Token: 0x040007BB RID: 1979
		public ushort MajorVersion;

		// Token: 0x040007BC RID: 1980
		public ushort MinorVersion;

		// Token: 0x040007BD RID: 1981
		public ushort BuildNumber;

		// Token: 0x040007BE RID: 1982
		public byte ServicePackMajor;

		// Token: 0x040007BF RID: 1983
		public byte ServicePackMinor;
	}
}
