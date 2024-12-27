using System;

namespace System.Security.Cryptography.Xml
{
	// Token: 0x020000CD RID: 205
	internal class RSAPKCS1SHA2Deformatter : AsymmetricSignatureDeformatter
	{
		// Token: 0x06000507 RID: 1287 RVA: 0x000194B8 File Offset: 0x000184B8
		public override void SetKey(AsymmetricAlgorithm key)
		{
			this._key = (RSA)key;
		}

		// Token: 0x06000508 RID: 1288 RVA: 0x000194C6 File Offset: 0x000184C6
		public override void SetHashAlgorithm(string strName)
		{
			this._hashAlgorithm = strName;
		}

		// Token: 0x06000509 RID: 1289 RVA: 0x000194D0 File Offset: 0x000184D0
		public override bool VerifySignature(byte[] rgbHash, byte[] rgbSignature)
		{
			RSACryptoServiceProvider rsacryptoServiceProvider = this._key as RSACryptoServiceProvider;
			if (rsacryptoServiceProvider != null && rsacryptoServiceProvider.CspKeyContainerInfo.ProviderType != 24)
			{
				RSAParameters rsaparameters = this._key.ExportParameters(false);
				using (RSACryptoServiceProvider rsacryptoServiceProvider2 = new RSACryptoServiceProvider())
				{
					rsacryptoServiceProvider2.ImportParameters(rsaparameters);
					return rsacryptoServiceProvider2.VerifyHash(rgbHash, this._hashAlgorithm, rgbSignature);
				}
			}
			AsymmetricSignatureDeformatter asymmetricSignatureDeformatter = new RSAPKCS1SignatureDeformatter(this._key);
			asymmetricSignatureDeformatter.SetHashAlgorithm(this._hashAlgorithm);
			return asymmetricSignatureDeformatter.VerifySignature(rgbHash, rgbSignature);
		}

		// Token: 0x040005CC RID: 1484
		private RSA _key;

		// Token: 0x040005CD RID: 1485
		private string _hashAlgorithm;
	}
}
