using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x020001B0 RID: 432
	[StructLayout(LayoutKind.Sequential)]
	internal class DescriptionMetadataEntry
	{
		// Token: 0x04000731 RID: 1841
		[MarshalAs(UnmanagedType.LPWStr)]
		public string Publisher;

		// Token: 0x04000732 RID: 1842
		[MarshalAs(UnmanagedType.LPWStr)]
		public string Product;

		// Token: 0x04000733 RID: 1843
		[MarshalAs(UnmanagedType.LPWStr)]
		public string SupportUrl;

		// Token: 0x04000734 RID: 1844
		[MarshalAs(UnmanagedType.LPWStr)]
		public string IconFile;

		// Token: 0x04000735 RID: 1845
		[MarshalAs(UnmanagedType.LPWStr)]
		public string ErrorReportUrl;

		// Token: 0x04000736 RID: 1846
		[MarshalAs(UnmanagedType.LPWStr)]
		public string SuiteName;
	}
}
