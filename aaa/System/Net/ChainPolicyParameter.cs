using System;
using System.Runtime.InteropServices;

namespace System.Net
{
	// Token: 0x020003FD RID: 1021
	internal struct ChainPolicyParameter
	{
		// Token: 0x04002052 RID: 8274
		public uint cbSize;

		// Token: 0x04002053 RID: 8275
		public uint dwFlags;

		// Token: 0x04002054 RID: 8276
		public unsafe SSL_EXTRA_CERT_CHAIN_POLICY_PARA* pvExtraPolicyPara;

		// Token: 0x04002055 RID: 8277
		public static readonly uint StructSize = (uint)Marshal.SizeOf(typeof(ChainPolicyParameter));
	}
}
