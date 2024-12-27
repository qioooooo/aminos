using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x020001BC RID: 444
	[StructLayout(LayoutKind.Sequential)]
	internal class EventEntry
	{
		// Token: 0x04000783 RID: 1923
		public uint EventID;

		// Token: 0x04000784 RID: 1924
		public uint Level;

		// Token: 0x04000785 RID: 1925
		public uint Version;

		// Token: 0x04000786 RID: 1926
		public Guid Guid;

		// Token: 0x04000787 RID: 1927
		[MarshalAs(UnmanagedType.LPWStr)]
		public string SubTypeName;

		// Token: 0x04000788 RID: 1928
		public uint SubTypeValue;

		// Token: 0x04000789 RID: 1929
		[MarshalAs(UnmanagedType.LPWStr)]
		public string DisplayName;

		// Token: 0x0400078A RID: 1930
		public uint EventNameMicrodomIndex;
	}
}
