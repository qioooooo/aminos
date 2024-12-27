using System;
using System.Runtime.InteropServices;

namespace System.Security.Cryptography
{
	// Token: 0x02000865 RID: 2149
	[ComVisible(true)]
	[Serializable]
	public struct DSAParameters
	{
		// Token: 0x0400288F RID: 10383
		public byte[] P;

		// Token: 0x04002890 RID: 10384
		public byte[] Q;

		// Token: 0x04002891 RID: 10385
		public byte[] G;

		// Token: 0x04002892 RID: 10386
		public byte[] Y;

		// Token: 0x04002893 RID: 10387
		public byte[] J;

		// Token: 0x04002894 RID: 10388
		[NonSerialized]
		public byte[] X;

		// Token: 0x04002895 RID: 10389
		public byte[] Seed;

		// Token: 0x04002896 RID: 10390
		public int Counter;
	}
}
