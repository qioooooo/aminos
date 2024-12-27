using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x020001B6 RID: 438
	[StructLayout(LayoutKind.Sequential)]
	internal class DependentOSMetadataEntry
	{
		// Token: 0x04000749 RID: 1865
		[MarshalAs(UnmanagedType.LPWStr)]
		public string SupportUrl;

		// Token: 0x0400074A RID: 1866
		[MarshalAs(UnmanagedType.LPWStr)]
		public string Description;

		// Token: 0x0400074B RID: 1867
		public ushort MajorVersion;

		// Token: 0x0400074C RID: 1868
		public ushort MinorVersion;

		// Token: 0x0400074D RID: 1869
		public ushort BuildNumber;

		// Token: 0x0400074E RID: 1870
		public byte ServicePackMajor;

		// Token: 0x0400074F RID: 1871
		public byte ServicePackMinor;
	}
}
