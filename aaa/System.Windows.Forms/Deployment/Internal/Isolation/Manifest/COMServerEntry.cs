using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x02000080 RID: 128
	[StructLayout(LayoutKind.Sequential)]
	internal class COMServerEntry
	{
		// Token: 0x04000C54 RID: 3156
		public Guid Clsid;

		// Token: 0x04000C55 RID: 3157
		public uint Flags;

		// Token: 0x04000C56 RID: 3158
		public Guid ConfiguredGuid;

		// Token: 0x04000C57 RID: 3159
		public Guid ImplementedClsid;

		// Token: 0x04000C58 RID: 3160
		public Guid TypeLibrary;

		// Token: 0x04000C59 RID: 3161
		public uint ThreadingModel;

		// Token: 0x04000C5A RID: 3162
		[MarshalAs(UnmanagedType.LPWStr)]
		public string RuntimeVersion;

		// Token: 0x04000C5B RID: 3163
		[MarshalAs(UnmanagedType.LPWStr)]
		public string HostFile;
	}
}
