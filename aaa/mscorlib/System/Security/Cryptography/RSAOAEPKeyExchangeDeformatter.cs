using System;
using System.Runtime.InteropServices;

namespace System.Security.Cryptography
{
	// Token: 0x02000887 RID: 2183
	[ComVisible(true)]
	public class RSAOAEPKeyExchangeDeformatter : AsymmetricKeyExchangeDeformatter
	{
		// Token: 0x06004FF1 RID: 20465 RVA: 0x001193F6 File Offset: 0x001183F6
		public RSAOAEPKeyExchangeDeformatter()
		{
		}

		// Token: 0x06004FF2 RID: 20466 RVA: 0x001193FE File Offset: 0x001183FE
		public RSAOAEPKeyExchangeDeformatter(AsymmetricAlgorithm key)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			this._rsaKey = (RSA)key;
		}

		// Token: 0x17000E03 RID: 3587
		// (get) Token: 0x06004FF3 RID: 20467 RVA: 0x00119420 File Offset: 0x00118420
		// (set) Token: 0x06004FF4 RID: 20468 RVA: 0x00119423 File Offset: 0x00118423
		public override string Parameters
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		// Token: 0x06004FF5 RID: 20469 RVA: 0x00119428 File Offset: 0x00118428
		public override byte[] DecryptKeyExchange(byte[] rgbData)
		{
			if (this._rsaKey == null)
			{
				throw new CryptographicUnexpectedOperationException(Environment.GetResourceString("Cryptography_MissingKey"));
			}
			if (this._rsaKey is RSACryptoServiceProvider)
			{
				return ((RSACryptoServiceProvider)this._rsaKey).Decrypt(rgbData, true);
			}
			return Utils.RsaOaepDecrypt(this._rsaKey, SHA1.Create(), new PKCS1MaskGenerationMethod(), rgbData);
		}

		// Token: 0x06004FF6 RID: 20470 RVA: 0x00119483 File Offset: 0x00118483
		public override void SetKey(AsymmetricAlgorithm key)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			this._rsaKey = (RSA)key;
		}

		// Token: 0x040028FC RID: 10492
		private RSA _rsaKey;
	}
}
