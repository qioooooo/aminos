using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x020000F5 RID: 245
	internal struct STORE_ASSEMBLY_FILE
	{
		// Token: 0x040004E1 RID: 1249
		public uint Size;

		// Token: 0x040004E2 RID: 1250
		public uint Flags;

		// Token: 0x040004E3 RID: 1251
		[MarshalAs(UnmanagedType.LPWStr)]
		public string FileName;

		// Token: 0x040004E4 RID: 1252
		public uint FileStatusFlags;
	}
}
