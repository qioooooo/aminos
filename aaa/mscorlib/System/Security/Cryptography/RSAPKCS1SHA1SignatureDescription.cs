using System;

namespace System.Security.Cryptography
{
	// Token: 0x0200089E RID: 2206
	internal class RSAPKCS1SHA1SignatureDescription : SignatureDescription
	{
		// Token: 0x060050A3 RID: 20643 RVA: 0x001220F8 File Offset: 0x001210F8
		public RSAPKCS1SHA1SignatureDescription()
		{
			base.KeyAlgorithm = "System.Security.Cryptography.RSACryptoServiceProvider";
			base.DigestAlgorithm = "System.Security.Cryptography.SHA1CryptoServiceProvider";
			base.FormatterAlgorithm = "System.Security.Cryptography.RSAPKCS1SignatureFormatter";
			base.DeformatterAlgorithm = "System.Security.Cryptography.RSAPKCS1SignatureDeformatter";
		}

		// Token: 0x060050A4 RID: 20644 RVA: 0x0012212C File Offset: 0x0012112C
		public override AsymmetricSignatureDeformatter CreateDeformatter(AsymmetricAlgorithm key)
		{
			AsymmetricSignatureDeformatter asymmetricSignatureDeformatter = (AsymmetricSignatureDeformatter)CryptoConfig.CreateFromName(base.DeformatterAlgorithm);
			asymmetricSignatureDeformatter.SetKey(key);
			asymmetricSignatureDeformatter.SetHashAlgorithm("SHA1");
			return asymmetricSignatureDeformatter;
		}
	}
}
