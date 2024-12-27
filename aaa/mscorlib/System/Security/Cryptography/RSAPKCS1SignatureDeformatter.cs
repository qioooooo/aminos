using System;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;

namespace System.Security.Cryptography
{
	// Token: 0x0200088B RID: 2187
	[ComVisible(true)]
	public class RSAPKCS1SignatureDeformatter : AsymmetricSignatureDeformatter
	{
		// Token: 0x06005011 RID: 20497 RVA: 0x001197E2 File Offset: 0x001187E2
		public RSAPKCS1SignatureDeformatter()
		{
		}

		// Token: 0x06005012 RID: 20498 RVA: 0x001197EA File Offset: 0x001187EA
		public RSAPKCS1SignatureDeformatter(AsymmetricAlgorithm key)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			this._rsaKey = (RSA)key;
		}

		// Token: 0x06005013 RID: 20499 RVA: 0x0011980C File Offset: 0x0011880C
		public override void SetKey(AsymmetricAlgorithm key)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			this._rsaKey = (RSA)key;
		}

		// Token: 0x06005014 RID: 20500 RVA: 0x00119828 File Offset: 0x00118828
		public override void SetHashAlgorithm(string strName)
		{
			this._strOID = CryptoConfig.MapNameToOID(strName, OidGroup.HashAlgorithm);
		}

		// Token: 0x06005015 RID: 20501 RVA: 0x00119838 File Offset: 0x00118838
		public override bool VerifySignature(byte[] rgbHash, byte[] rgbSignature)
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
			if (rgbSignature == null)
			{
				throw new ArgumentNullException("rgbSignature");
			}
			if (this._rsaKey is RSACryptoServiceProvider)
			{
				int num = X509Utils.OidToAlgIdStrict(this._strOID, OidGroup.HashAlgorithm);
				return ((RSACryptoServiceProvider)this._rsaKey).VerifyHash(rgbHash, num, rgbSignature);
			}
			byte[] array = Utils.RsaPkcs1Padding(this._rsaKey, CryptoConfig.EncodeOID(this._strOID), rgbHash);
			return Utils.CompareBigIntArrays(this._rsaKey.EncryptValue(rgbSignature), array);
		}

		// Token: 0x04002904 RID: 10500
		private RSA _rsaKey;

		// Token: 0x04002905 RID: 10501
		private string _strOID;
	}
}
