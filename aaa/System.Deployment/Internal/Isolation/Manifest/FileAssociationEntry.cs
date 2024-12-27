using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x02000186 RID: 390
	[StructLayout(LayoutKind.Sequential)]
	internal class FileAssociationEntry
	{
		// Token: 0x040006C6 RID: 1734
		[MarshalAs(UnmanagedType.LPWStr)]
		public string Extension;

		// Token: 0x040006C7 RID: 1735
		[MarshalAs(UnmanagedType.LPWStr)]
		public string Description;

		// Token: 0x040006C8 RID: 1736
		[MarshalAs(UnmanagedType.LPWStr)]
		public string ProgID;

		// Token: 0x040006C9 RID: 1737
		[MarshalAs(UnmanagedType.LPWStr)]
		public string DefaultIcon;

		// Token: 0x040006CA RID: 1738
		[MarshalAs(UnmanagedType.LPWStr)]
		public string Parameter;
	}
}
