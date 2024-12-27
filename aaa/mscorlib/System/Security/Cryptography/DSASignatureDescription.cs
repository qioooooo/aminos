using System;

namespace System.Security.Cryptography
{
	// Token: 0x0200089F RID: 2207
	internal class DSASignatureDescription : SignatureDescription
	{
		// Token: 0x060050A5 RID: 20645 RVA: 0x0012215D File Offset: 0x0012115D
		public DSASignatureDescription()
		{
			base.KeyAlgorithm = "System.Security.Cryptography.DSACryptoServiceProvider";
			base.DigestAlgorithm = "System.Security.Cryptography.SHA1CryptoServiceProvider";
			base.FormatterAlgorithm = "System.Security.Cryptography.DSASignatureFormatter";
			base.DeformatterAlgorithm = "System.Security.Cryptography.DSASignatureDeformatter";
		}
	}
}
