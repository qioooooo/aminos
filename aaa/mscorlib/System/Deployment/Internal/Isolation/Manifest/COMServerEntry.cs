using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x020001A5 RID: 421
	[StructLayout(LayoutKind.Sequential)]
	internal class COMServerEntry
	{
		// Token: 0x0400074E RID: 1870
		public Guid Clsid;

		// Token: 0x0400074F RID: 1871
		public uint Flags;

		// Token: 0x04000750 RID: 1872
		public Guid ConfiguredGuid;

		// Token: 0x04000751 RID: 1873
		public Guid ImplementedClsid;

		// Token: 0x04000752 RID: 1874
		public Guid TypeLibrary;

		// Token: 0x04000753 RID: 1875
		public uint ThreadingModel;

		// Token: 0x04000754 RID: 1876
		[MarshalAs(UnmanagedType.LPWStr)]
		public string RuntimeVersion;

		// Token: 0x04000755 RID: 1877
		[MarshalAs(UnmanagedType.LPWStr)]
		public string HostFile;
	}
}
