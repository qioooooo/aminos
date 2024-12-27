using System;
using System.Runtime.InteropServices;

namespace System.Security.Cryptography
{
	// Token: 0x02000888 RID: 2184
	[ComVisible(true)]
	public class RSAOAEPKeyExchangeFormatter : AsymmetricKeyExchangeFormatter
	{
		// Token: 0x06004FF7 RID: 20471 RVA: 0x0011949F File Offset: 0x0011849F
		public RSAOAEPKeyExchangeFormatter()
		{
		}

		// Token: 0x06004FF8 RID: 20472 RVA: 0x001194A7 File Offset: 0x001184A7
		public RSAOAEPKeyExchangeFormatter(AsymmetricAlgorithm key)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			this._rsaKey = (RSA)key;
		}

		// Token: 0x17000E04 RID: 3588
		// (get) Token: 0x06004FF9 RID: 20473 RVA: 0x001194C9 File Offset: 0x001184C9
		// (set) Token: 0x06004FFA RID: 20474 RVA: 0x001194E5 File Offset: 0x001184E5
		public byte[] Parameter
		{
			get
			{
				if (this.ParameterValue != null)
				{
					return (byte[])this.ParameterValue.Clone();
				}
				return null;
			}
			set
			{
				if (value != null)
				{
					this.ParameterValue = (byte[])value.Clone();
					return;
				}
				this.ParameterValue = null;
			}
		}

		// Token: 0x17000E05 RID: 3589
		// (get) Token: 0x06004FFB RID: 20475 RVA: 0x00119503 File Offset: 0x00118503
		public override string Parameters
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000E06 RID: 3590
		// (get) Token: 0x06004FFC RID: 20476 RVA: 0x00119506 File Offset: 0x00118506
		// (set) Token: 0x06004FFD RID: 20477 RVA: 0x0011950E File Offset: 0x0011850E
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

		// Token: 0x06004FFE RID: 20478 RVA: 0x00119517 File Offset: 0x00118517
		public override void SetKey(AsymmetricAlgorithm key)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			this._rsaKey = (RSA)key;
		}

		// Token: 0x06004FFF RID: 20479 RVA: 0x00119534 File Offset: 0x00118534
		public override byte[] CreateKeyExchange(byte[] rgbData)
		{
			if (this._rsaKey == null)
			{
				throw new CryptographicUnexpectedOperationException(Environment.GetResourceString("Cryptography_MissingKey"));
			}
			if (this._rsaKey is RSACryptoServiceProvider)
			{
				return ((RSACryptoServiceProvider)this._rsaKey).Encrypt(rgbData, true);
			}
			return Utils.RsaOaepEncrypt(this._rsaKey, SHA1.Create(), new PKCS1MaskGenerationMethod(), RandomNumberGenerator.Create(), rgbData);
		}

		// Token: 0x06005000 RID: 20480 RVA: 0x00119594 File Offset: 0x00118594
		public override byte[] CreateKeyExchange(byte[] rgbData, Type symAlgType)
		{
			return this.CreateKeyExchange(rgbData);
		}

		// Token: 0x040028FD RID: 10493
		private byte[] ParameterValue;

		// Token: 0x040028FE RID: 10494
		private RSA _rsaKey;

		// Token: 0x040028FF RID: 10495
		private RandomNumberGenerator RngValue;
	}
}
