using System;
using System.Runtime.InteropServices;

namespace System.Net
{
	// Token: 0x02000400 RID: 1024
	internal struct ChainPolicyStatus
	{
		// Token: 0x0400205D RID: 8285
		public uint cbSize;

		// Token: 0x0400205E RID: 8286
		public uint dwError;

		// Token: 0x0400205F RID: 8287
		public uint lChainIndex;

		// Token: 0x04002060 RID: 8288
		public uint lElementIndex;

		// Token: 0x04002061 RID: 8289
		public unsafe void* pvExtraPolicyStatus;

		// Token: 0x04002062 RID: 8290
		public static readonly uint StructSize = (uint)Marshal.SizeOf(typeof(ChainPolicyStatus));
	}
}
