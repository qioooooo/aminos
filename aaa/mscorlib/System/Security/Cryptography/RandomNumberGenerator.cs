using System;
using System.Runtime.InteropServices;

namespace System.Security.Cryptography
{
	// Token: 0x02000850 RID: 2128
	[ComVisible(true)]
	public abstract class RandomNumberGenerator
	{
		// Token: 0x06004E2A RID: 20010 RVA: 0x0010FC2C File Offset: 0x0010EC2C
		public static RandomNumberGenerator Create()
		{
			return RandomNumberGenerator.Create("System.Security.Cryptography.RandomNumberGenerator");
		}

		// Token: 0x06004E2B RID: 20011 RVA: 0x0010FC38 File Offset: 0x0010EC38
		public static RandomNumberGenerator Create(string rngName)
		{
			return (RandomNumberGenerator)CryptoConfig.CreateFromName(rngName);
		}

		// Token: 0x06004E2C RID: 20012
		public abstract void GetBytes(byte[] data);

		// Token: 0x06004E2D RID: 20013
		public abstract void GetNonZeroBytes(byte[] data);
	}
}
