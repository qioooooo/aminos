using System;
using System.Security.Cryptography;

namespace HEDS
{
	// Token: 0x02000013 RID: 19
	internal class HedsCrypt
	{
		// Token: 0x06000069 RID: 105 RVA: 0x00002F94 File Offset: 0x00001194
		public void Init(string s)
		{
			this.rsaProvider = new RSACryptoServiceProvider();
			this.rsaProvider.FromXmlString(s);
		}

		// Token: 0x0600006A RID: 106 RVA: 0x00002FB0 File Offset: 0x000011B0
		public bool VerifyData(byte[] bSource, byte[] bSign)
		{
			return this.rsaProvider.VerifyData(bSource, new SHA1CryptoServiceProvider(), bSign);
		}

		// Token: 0x0400004A RID: 74
		private RSACryptoServiceProvider rsaProvider;
	}
}
