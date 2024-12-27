using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x02000199 RID: 409
	[StructLayout(LayoutKind.Sequential)]
	internal class FileAssociationEntry
	{
		// Token: 0x04000736 RID: 1846
		[MarshalAs(UnmanagedType.LPWStr)]
		public string Extension;

		// Token: 0x04000737 RID: 1847
		[MarshalAs(UnmanagedType.LPWStr)]
		public string Description;

		// Token: 0x04000738 RID: 1848
		[MarshalAs(UnmanagedType.LPWStr)]
		public string ProgID;

		// Token: 0x04000739 RID: 1849
		[MarshalAs(UnmanagedType.LPWStr)]
		public string DefaultIcon;

		// Token: 0x0400073A RID: 1850
		[MarshalAs(UnmanagedType.LPWStr)]
		public string Parameter;
	}
}
