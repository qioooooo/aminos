using System;
using System.Runtime.InteropServices;

namespace System.Security.Cryptography
{
	// Token: 0x0200088C RID: 2188
	[ComVisible(true)]
	public class RSAPKCS1SignatureFormatter : AsymmetricSignatureFormatter
	{
		// Token: 0x06005016 RID: 20502 RVA: 0x001198E9 File Offset: 0x001188E9
		public RSAPKCS1SignatureFormatter()
		{
		}

		// Token: 0x06005017 RID: 20503 RVA: 0x001198F1 File Offset: 0x001188F1
		public RSAPKCS1SignatureFormatter(AsymmetricAlgorithm key)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			this._rsaKey = (RSA)key;
		}

		// Token: 0x06005018 RID: 20504 RVA: 0x00119913 File Offset: 0x00118913
		public override void SetKey(AsymmetricAlgorithm key)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			this._rsaKey = (RSA)key;
		}

		// Token: 0x06005019 RID: 20505 RVA: 0x0011992F File Offset: 0x0011892F
		public override void SetHashAlgorithm(string strName)
		{
			this._strOID = CryptoConfig.MapNameToOID(strName);
		}

		// Token: 0x0600501A RID: 20506 RVA: 0x00119940 File Offset: 0x00118940
		public override byte[] CreateSignature(byte[] rgbHash)
		{
			if (this._strOID == null)
			{
				throw new CryptographicUnexpectedOperationException(Environment.GetResourceString("Cryptography_MissingOID"));
			}
			if (this._rsaKey == null)
			{
				throw new CryptographicUnexpectedOperationException(Environment.GetResourceString("Cryptography_MissingKey"));
			}
			if (rgbHash == null)
			{
				throw new ArgumentNullException("rgbHash");
			}
			if (this._rsaKey is RSACryptoServiceProvider)
			{
				return ((RSACryptoServiceProvider)this._rsaKey).SignHash(rgbHash, this._strOID);
			}
			byte[] array = Utils.RsaPkcs1Padding(this._rsaKey, CryptoConfig.EncodeOID(this._strOID), rgbHash);
			return this._rsaKey.DecryptValue(array);
		}

		// Token: 0x04002906 RID: 10502
		private RSA _rsaKey;

		// Token: 0x04002907 RID: 10503
		private string _strOID;
	}
}
