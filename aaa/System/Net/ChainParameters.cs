using System;
using System.Runtime.InteropServices;

namespace System.Net
{
	// Token: 0x02000403 RID: 1027
	internal struct ChainParameters
	{
		// Token: 0x04002067 RID: 8295
		public uint cbSize;

		// Token: 0x04002068 RID: 8296
		public CertUsageMatch RequestedUsage;

		// Token: 0x04002069 RID: 8297
		public CertUsageMatch RequestedIssuancePolicy;

		// Token: 0x0400206A RID: 8298
		public uint UrlRetrievalTimeout;

		// Token: 0x0400206B RID: 8299
		public int BoolCheckRevocationFreshnessTime;

		// Token: 0x0400206C RID: 8300
		public uint RevocationFreshnessTime;

		// Token: 0x0400206D RID: 8301
		public static readonly uint StructSize = (uint)Marshal.SizeOf(typeof(ChainParameters));
	}
}
