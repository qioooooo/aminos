using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x02000192 RID: 402
	[StructLayout(LayoutKind.Sequential)]
	internal class COMServerEntry
	{
		// Token: 0x040006DE RID: 1758
		public Guid Clsid;

		// Token: 0x040006DF RID: 1759
		public uint Flags;

		// Token: 0x040006E0 RID: 1760
		public Guid ConfiguredGuid;

		// Token: 0x040006E1 RID: 1761
		public Guid ImplementedClsid;

		// Token: 0x040006E2 RID: 1762
		public Guid TypeLibrary;

		// Token: 0x040006E3 RID: 1763
		public uint ThreadingModel;

		// Token: 0x040006E4 RID: 1764
		[MarshalAs(UnmanagedType.LPWStr)]
		public string RuntimeVersion;

		// Token: 0x040006E5 RID: 1765
		[MarshalAs(UnmanagedType.LPWStr)]
		public string HostFile;
	}
}
