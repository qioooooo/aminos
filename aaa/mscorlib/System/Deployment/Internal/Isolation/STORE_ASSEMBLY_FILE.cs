using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x020001D5 RID: 469
	internal struct STORE_ASSEMBLY_FILE
	{
		// Token: 0x04000803 RID: 2051
		public uint Size;

		// Token: 0x04000804 RID: 2052
		public uint Flags;

		// Token: 0x04000805 RID: 2053
		[MarshalAs(UnmanagedType.LPWStr)]
		public string FileName;

		// Token: 0x04000806 RID: 2054
		public uint FileStatusFlags;
	}
}
