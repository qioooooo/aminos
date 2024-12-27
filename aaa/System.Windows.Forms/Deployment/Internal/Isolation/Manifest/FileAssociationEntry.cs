using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x02000074 RID: 116
	[StructLayout(LayoutKind.Sequential)]
	internal class FileAssociationEntry
	{
		// Token: 0x04000C3C RID: 3132
		[MarshalAs(UnmanagedType.LPWStr)]
		public string Extension;

		// Token: 0x04000C3D RID: 3133
		[MarshalAs(UnmanagedType.LPWStr)]
		public string Description;

		// Token: 0x04000C3E RID: 3134
		[MarshalAs(UnmanagedType.LPWStr)]
		public string ProgID;

		// Token: 0x04000C3F RID: 3135
		[MarshalAs(UnmanagedType.LPWStr)]
		public string DefaultIcon;

		// Token: 0x04000C40 RID: 3136
		[MarshalAs(UnmanagedType.LPWStr)]
		public string Parameter;
	}
}
