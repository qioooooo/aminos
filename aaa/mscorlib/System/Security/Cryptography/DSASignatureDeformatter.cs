using System;
using System.Runtime.InteropServices;

namespace System.Security.Cryptography
{
	// Token: 0x0200086A RID: 2154
	[ComVisible(true)]
	public class DSASignatureDeformatter : AsymmetricSignatureDeformatter
	{
		// Token: 0x06004F03 RID: 20227 RVA: 0x00113E21 File Offset: 0x00112E21
		public DSASignatureDeformatter()
		{
			this._oid = CryptoConfig.MapNameToOID("SHA1");
		}

		// Token: 0x06004F04 RID: 20228 RVA: 0x00113E39 File Offset: 0x00112E39
		public DSASignatureDeformatter(AsymmetricAlgorithm key)
			: this()
		{
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			this._dsaKey = (DSA)key;
		}

		// Token: 0x06004F05 RID: 20229 RVA: 0x00113E5B File Offset: 0x00112E5B
		public override void SetKey(AsymmetricAlgorithm key)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			this._dsaKey = (DSA)key;
		}

		// Token: 0x06004F06 RID: 20230 RVA: 0x00113E77 File Offset: 0x00112E77
		public override void SetHashAlgorithm(string strName)
		{
			if (CryptoConfig.MapNameToOID(strName) != this._oid)
			{
				throw new CryptographicUnexpectedOperationException(Environment.GetResourceString("Cryptography_InvalidOperation"));
			}
		}

		// Token: 0x06004F07 RID: 20231 RVA: 0x00113E9C File Offset: 0x00112E9C
		public override bool VerifySignature(byte[] rgbHash, byte[] rgbSignature)
		{
			if (this._dsaKey == null)
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
			return this._dsaKey.VerifySignature(rgbHash, rgbSignature);
		}

		// Token: 0x040028A6 RID: 10406
		private DSA _dsaKey;

		// Token: 0x040028A7 RID: 10407
		private string _oid;
	}
}
