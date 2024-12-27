using System;

namespace System.Security.Cryptography.Xml
{
	// Token: 0x020000CC RID: 204
	internal class RSAPKCS1SHA2Formatter : AsymmetricSignatureFormatter
	{
		// Token: 0x06000501 RID: 1281 RVA: 0x00019305 File Offset: 0x00018305
		public override void SetKey(AsymmetricAlgorithm key)
		{
			this._key = (RSA)key;
		}

		// Token: 0x06000502 RID: 1282 RVA: 0x00019313 File Offset: 0x00018313
		public override void SetHashAlgorithm(string strName)
		{
			this._hashAlgorithm = strName;
		}

		// Token: 0x06000503 RID: 1283 RVA: 0x0001931C File Offset: 0x0001831C
		public override byte[] CreateSignature(byte[] rgbHash)
		{
			RSACryptoServiceProvider rsacryptoServiceProvider = this._key as RSACryptoServiceProvider;
			if (rsacryptoServiceProvider != null)
			{
				using (RSACryptoServiceProvider rsacryptoServiceProvider2 = RSAPKCS1SHA2Formatter.UpgradeCspIfNeeded(rsacryptoServiceProvider))
				{
					RSACryptoServiceProvider rsacryptoServiceProvider3 = rsacryptoServiceProvider2 ?? rsacryptoServiceProvider;
					return rsacryptoServiceProvider3.SignHash(rgbHash, this._hashAlgorithm);
				}
			}
			AsymmetricSignatureFormatter asymmetricSignatureFormatter = new RSAPKCS1SignatureFormatter(this._key);
			asymmetricSignatureFormatter.SetHashAlgorithm(this._hashAlgorithm);
			return asymmetricSignatureFormatter.CreateSignature(rgbHash);
		}

		// Token: 0x06000504 RID: 1284 RVA: 0x00019394 File Offset: 0x00018394
		private static bool ShouldUpgrade(CspKeyContainerInfo keyContainerInfo)
		{
			int providerType = keyContainerInfo.ProviderType;
			switch (providerType)
			{
			case 1:
			case 2:
				break;
			default:
				if (providerType != 12)
				{
					return providerType == 24 && false;
				}
				break;
			}
			string providerName = keyContainerInfo.ProviderName;
			StringComparison stringComparison = StringComparison.OrdinalIgnoreCase;
			return providerName.Equals("Microsoft Base Cryptographic Provider v1.0", stringComparison) || providerName.Equals("Microsoft RSA Schannel Cryptographic Provider", stringComparison) || providerName.Equals("Microsoft RSA Signature Cryptographic Provider", stringComparison) || providerName.Equals("Microsoft Enhanced Cryptographic Provider v1.0", stringComparison) || providerName.Equals("Microsoft Strong Cryptographic Provider", stringComparison) || providerName.Equals("Microsoft Enhanced RSA and AES Cryptographic Provider", stringComparison) || providerName.Equals("Microsoft Enhanced RSA and AES Cryptographic Provider (Prototype)", stringComparison);
		}

		// Token: 0x06000505 RID: 1285 RVA: 0x00019434 File Offset: 0x00018434
		private static RSACryptoServiceProvider UpgradeCspIfNeeded(RSACryptoServiceProvider rsaCsp)
		{
			CspKeyContainerInfo cspKeyContainerInfo = rsaCsp.CspKeyContainerInfo;
			if (!RSAPKCS1SHA2Formatter.ShouldUpgrade(cspKeyContainerInfo))
			{
				return null;
			}
			CspParameters cspParameters = new CspParameters(24);
			cspParameters.KeyContainerName = cspKeyContainerInfo.KeyContainerName;
			cspParameters.Flags = CspProviderFlags.UseExistingKey;
			if (cspKeyContainerInfo.MachineKeyStore)
			{
				cspParameters.Flags |= CspProviderFlags.UseMachineKeyStore;
			}
			cspParameters.KeyNumber = (int)cspKeyContainerInfo.KeyNumber;
			RSACryptoServiceProvider rsacryptoServiceProvider;
			try
			{
				rsacryptoServiceProvider = new RSACryptoServiceProvider(cspParameters);
			}
			catch (CryptographicException)
			{
				rsacryptoServiceProvider = null;
			}
			return rsacryptoServiceProvider;
		}

		// Token: 0x040005CA RID: 1482
		private RSA _key;

		// Token: 0x040005CB RID: 1483
		private string _hashAlgorithm;
	}
}
