using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x0200009E RID: 158
	[StructLayout(LayoutKind.Sequential)]
	internal class DescriptionMetadataEntry
	{
		// Token: 0x04000CA7 RID: 3239
		[MarshalAs(UnmanagedType.LPWStr)]
		public string Publisher;

		// Token: 0x04000CA8 RID: 3240
		[MarshalAs(UnmanagedType.LPWStr)]
		public string Product;

		// Token: 0x04000CA9 RID: 3241
		[MarshalAs(UnmanagedType.LPWStr)]
		public string SupportUrl;

		// Token: 0x04000CAA RID: 3242
		[MarshalAs(UnmanagedType.LPWStr)]
		public string IconFile;

		// Token: 0x04000CAB RID: 3243
		[MarshalAs(UnmanagedType.LPWStr)]
		public string ErrorReportUrl;

		// Token: 0x04000CAC RID: 3244
		[MarshalAs(UnmanagedType.LPWStr)]
		public string SuiteName;
	}
}
