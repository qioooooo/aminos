using System;
using System.Security.Cryptography;

namespace HEDS
{
	internal class HedsCrypt
	{
		public void Init(string s)
		{
			this.rsaProvider = new RSACryptoServiceProvider();
			this.rsaProvider.FromXmlString(s);
		}

		public bool VerifyData(byte[] bSource, byte[] bSign)
		{
			return this.rsaProvider.VerifyData(bSource, new SHA1CryptoServiceProvider(), bSign);
		}

		private RSACryptoServiceProvider rsaProvider;
	}
}
