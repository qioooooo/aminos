using System;
using System.Globalization;
using System.Runtime.InteropServices;

namespace System.Security.Cryptography
{
	// Token: 0x0200088A RID: 2186
	[ComVisible(true)]
	public class RSAPKCS1KeyExchangeFormatter : AsymmetricKeyExchangeFormatter
	{
		// Token: 0x06005009 RID: 20489 RVA: 0x00119695 File Offset: 0x00118695
		public RSAPKCS1KeyExchangeFormatter()
		{
		}

		// Token: 0x0600500A RID: 20490 RVA: 0x0011969D File Offset: 0x0011869D
		public RSAPKCS1KeyExchangeFormatter(AsymmetricAlgorithm key)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			this._rsaKey = (RSA)key;
		}

		// Token: 0x17000E09 RID: 3593
		// (get) Token: 0x0600500B RID: 20491 RVA: 0x001196BF File Offset: 0x001186BF
		public override string Parameters
		{
			get
			{
				return "<enc:KeyEncryptionMethod enc:Algorithm=\"http://www.microsoft.com/xml/security/algorithm/PKCS1-v1.5-KeyEx\" xmlns:enc=\"http://www.microsoft.com/xml/security/encryption/v1.0\" />";
			}
		}

		// Token: 0x17000E0A RID: 3594
		// (get) Token: 0x0600500C RID: 20492 RVA: 0x001196C6 File Offset: 0x001186C6
		// (set) Token: 0x0600500D RID: 20493 RVA: 0x001196CE File Offset: 0x001186CE
		public RandomNumberGenerator Rng
		{
			get
			{
				return this.RngValue;
			}
			set
			{
				this.RngValue = value;
			}
		}

		// Token: 0x0600500E RID: 20494 RVA: 0x001196D7 File Offset: 0x001186D7
		public override void SetKey(AsymmetricAlgorithm key)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			this._rsaKey = (RSA)key;
		}

		// Token: 0x0600500F RID: 20495 RVA: 0x001196F4 File Offset: 0x001186F4
		public override byte[] CreateKeyExchange(byte[] rgbData)
		{
			if (this._rsaKey == null)
			{
				throw new CryptographicUnexpectedOperationException(Environment.GetResourceString("Cryptography_MissingKey"));
			}
			byte[] array;
			if (this._rsaKey is RSACryptoServiceProvider)
			{
				array = ((RSACryptoServiceProvider)this._rsaKey).Encrypt(rgbData, false);
			}
			else
			{
				int num = this._rsaKey.KeySize / 8;
				if (rgbData.Length + 11 > num)
				{
					throw new CryptographicException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Cryptography_Padding_EncDataTooBig"), new object[] { num - 11 }));
				}
				byte[] array2 = new byte[num];
				if (this.RngValue == null)
				{
					this.RngValue = RandomNumberGenerator.Create();
				}
				this.Rng.GetNonZeroBytes(array2);
				array2[0] = 0;
				array2[1] = 2;
				array2[num - rgbData.Length - 1] = 0;
				Buffer.InternalBlockCopy(rgbData, 0, array2, num - rgbData.Length, rgbData.Length);
				array = this._rsaKey.EncryptValue(array2);
			}
			return array;
		}

		// Token: 0x06005010 RID: 20496 RVA: 0x001197D9 File Offset: 0x001187D9
		public override byte[] CreateKeyExchange(byte[] rgbData, Type symAlgType)
		{
			return this.CreateKeyExchange(rgbData);
		}

		// Token: 0x04002902 RID: 10498
		private RandomNumberGenerator RngValue;

		// Token: 0x04002903 RID: 10499
		private RSA _rsaKey;
	}
}
