using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x020000CA RID: 202
	internal struct STORE_ASSEMBLY_FILE
	{
		// Token: 0x04000D6D RID: 3437
		public uint Size;

		// Token: 0x04000D6E RID: 3438
		public uint Flags;

		// Token: 0x04000D6F RID: 3439
		[MarshalAs(UnmanagedType.LPWStr)]
		public string FileName;

		// Token: 0x04000D70 RID: 3440
		public uint FileStatusFlags;
	}
}
