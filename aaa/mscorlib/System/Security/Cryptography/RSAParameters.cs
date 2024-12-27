using System;
using System.Runtime.InteropServices;

namespace System.Security.Cryptography
{
	// Token: 0x02000883 RID: 2179
	[ComVisible(true)]
	[Serializable]
	public struct RSAParameters
	{
		// Token: 0x040028E5 RID: 10469
		public byte[] Exponent;

		// Token: 0x040028E6 RID: 10470
		public byte[] Modulus;

		// Token: 0x040028E7 RID: 10471
		[NonSerialized]
		public byte[] P;

		// Token: 0x040028E8 RID: 10472
		[NonSerialized]
		public byte[] Q;

		// Token: 0x040028E9 RID: 10473
		[NonSerialized]
		public byte[] DP;

		// Token: 0x040028EA RID: 10474
		[NonSerialized]
		public byte[] DQ;

		// Token: 0x040028EB RID: 10475
		[NonSerialized]
		public byte[] InverseQ;

		// Token: 0x040028EC RID: 10476
		[NonSerialized]
		public byte[] D;
	}
}
