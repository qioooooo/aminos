using System;

namespace System.Security.Cryptography.Xml
{
	// Token: 0x020000C8 RID: 200
	internal abstract class RSAPKCS1SignatureDescription : SignatureDescription
	{
		// Token: 0x060004F7 RID: 1271 RVA: 0x00019229 File Offset: 0x00018229
		public RSAPKCS1SignatureDescription(string hashAlgorithmName)
		{
			base.KeyAlgorithm = "System.Security.Cryptography.RSA";
			base.DigestAlgorithm = hashAlgorithmName;
			base.FormatterAlgorithm = "System.Security.Cryptography.RSAPKCS1SignatureFormatter";
			base.DeformatterAlgorithm = "System.Security.Cryptography.RSAPKCS1SignatureDeformatter";
		}

		// Token: 0x060004F8 RID: 1272 RVA: 0x0001925C File Offset: 0x0001825C
		public sealed override AsymmetricSignatureDeformatter CreateDeformatter(AsymmetricAlgorithm key)
		{
			AsymmetricSignatureDeformatter asymmetricSignatureDeformatter = new RSAPKCS1SHA2Deformatter();
			asymmetricSignatureDeformatter.SetKey(key);
			asymmetricSignatureDeformatter.SetHashAlgorithm(base.DigestAlgorithm);
			return asymmetricSignatureDeformatter;
		}

		// Token: 0x060004F9 RID: 1273 RVA: 0x00019284 File Offset: 0x00018284
		public sealed override AsymmetricSignatureFormatter CreateFormatter(AsymmetricAlgorithm key)
		{
			AsymmetricSignatureFormatter asymmetricSignatureFormatter = new RSAPKCS1SHA2Formatter();
			asymmetricSignatureFormatter.SetKey(key);
			asymmetricSignatureFormatter.SetHashAlgorithm(base.DigestAlgorithm);
			return asymmetricSignatureFormatter;
		}

		// Token: 0x060004FA RID: 1274
		public abstract override HashAlgorithm CreateDigest();
	}
}
