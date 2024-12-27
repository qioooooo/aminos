using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x020001C3 RID: 451
	[StructLayout(LayoutKind.Sequential)]
	internal class DescriptionMetadataEntry
	{
		// Token: 0x040007A1 RID: 1953
		[MarshalAs(UnmanagedType.LPWStr)]
		public string Publisher;

		// Token: 0x040007A2 RID: 1954
		[MarshalAs(UnmanagedType.LPWStr)]
		public string Product;

		// Token: 0x040007A3 RID: 1955
		[MarshalAs(UnmanagedType.LPWStr)]
		public string SupportUrl;

		// Token: 0x040007A4 RID: 1956
		[MarshalAs(UnmanagedType.LPWStr)]
		public string IconFile;

		// Token: 0x040007A5 RID: 1957
		[MarshalAs(UnmanagedType.LPWStr)]
		public string ErrorReportUrl;

		// Token: 0x040007A6 RID: 1958
		[MarshalAs(UnmanagedType.LPWStr)]
		public string SuiteName;
	}
}
