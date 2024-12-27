using System;

namespace System.Security.Cryptography.Xml
{
	// Token: 0x020000CB RID: 203
	internal class RSAPKCS1SHA512SignatureDescription : RSAPKCS1SignatureDescription
	{
		// Token: 0x060004FF RID: 1279 RVA: 0x000192E7 File Offset: 0x000182E7
		public RSAPKCS1SHA512SignatureDescription()
			: base("SHA512")
		{
		}

		// Token: 0x06000500 RID: 1280 RVA: 0x000192F4 File Offset: 0x000182F4
		public sealed override HashAlgorithm CreateDigest()
		{
			return (HashAlgorithm)CryptoConfig.CreateFromName("http://www.w3.org/2001/04/xmlenc#sha512");
		}
	}
}
