using System;
using System.Runtime.InteropServices;

namespace System.Security.Cryptography
{
	// Token: 0x0200086B RID: 2155
	[ComVisible(true)]
	public class DSASignatureFormatter : AsymmetricSignatureFormatter
	{
		// Token: 0x06004F08 RID: 20232 RVA: 0x00113EEA File Offset: 0x00112EEA
		public DSASignatureFormatter()
		{
			this._oid = CryptoConfig.MapNameToOID("SHA1");
		}

		// Token: 0x06004F09 RID: 20233 RVA: 0x00113F02 File Offset: 0x00112F02
		public DSASignatureFormatter(AsymmetricAlgorithm key)
			: this()
		{
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			this._dsaKey = (DSA)key;
		}

		// Token: 0x06004F0A RID: 20234 RVA: 0x00113F24 File Offset: 0x00112F24
		public override void SetKey(AsymmetricAlgorithm key)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			this._dsaKey = (DSA)key;
		}

		// Token: 0x06004F0B RID: 20235 RVA: 0x00113F40 File Offset: 0x00112F40
		public override void SetHashAlgorithm(string strName)
		{
			if (CryptoConfig.MapNameToOID(strName) != this._oid)
			{
				throw new CryptographicUnexpectedOperationException(Environment.GetResourceString("Cryptography_InvalidOperation"));
			}
		}

		// Token: 0x06004F0C RID: 20236 RVA: 0x00113F68 File Offset: 0x00112F68
		public override byte[] CreateSignature(byte[] rgbHash)
		{
			if (this._oid == null)
			{
				throw new CryptographicUnexpectedOperationException(Environment.GetResourceString("Cryptography_MissingOID"));
			}
			if (this._dsaKey == null)
			{
				throw new CryptographicUnexpectedOperationException(Environment.GetResourceString("Cryptography_MissingKey"));
			}
			if (rgbHash == null)
			{
				throw new ArgumentNullException("rgbHash");
			}
			return this._dsaKey.CreateSignature(rgbHash);
		}

		// Token: 0x040028A8 RID: 10408
		private DSA _dsaKey;

		// Token: 0x040028A9 RID: 10409
		private string _oid;
	}
}
