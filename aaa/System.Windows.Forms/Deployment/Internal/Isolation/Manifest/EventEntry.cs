using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x020000AA RID: 170
	[StructLayout(LayoutKind.Sequential)]
	internal class EventEntry
	{
		// Token: 0x04000CF9 RID: 3321
		public uint EventID;

		// Token: 0x04000CFA RID: 3322
		public uint Level;

		// Token: 0x04000CFB RID: 3323
		public uint Version;

		// Token: 0x04000CFC RID: 3324
		public Guid Guid;

		// Token: 0x04000CFD RID: 3325
		[MarshalAs(UnmanagedType.LPWStr)]
		public string SubTypeName;

		// Token: 0x04000CFE RID: 3326
		public uint SubTypeValue;

		// Token: 0x04000CFF RID: 3327
		[MarshalAs(UnmanagedType.LPWStr)]
		public string DisplayName;

		// Token: 0x04000D00 RID: 3328
		public uint EventNameMicrodomIndex;
	}
}
