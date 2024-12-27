using System;
using System.Runtime.InteropServices;

namespace System.Security.Cryptography
{
	// Token: 0x02000889 RID: 2185
	[ComVisible(true)]
	public class RSAPKCS1KeyExchangeDeformatter : AsymmetricKeyExchangeDeformatter
	{
		// Token: 0x06005001 RID: 20481 RVA: 0x0011959D File Offset: 0x0011859D
		public RSAPKCS1KeyExchangeDeformatter()
		{
		}

		// Token: 0x06005002 RID: 20482 RVA: 0x001195A5 File Offset: 0x001185A5
		public RSAPKCS1KeyExchangeDeformatter(AsymmetricAlgorithm key)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			this._rsaKey = (RSA)key;
		}

		// Token: 0x17000E07 RID: 3591
		// (get) Token: 0x06005003 RID: 20483 RVA: 0x001195C7 File Offset: 0x001185C7
		// (set) Token: 0x06005004 RID: 20484 RVA: 0x001195CF File Offset: 0x001185CF
		public RandomNumberGenerator RNG
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

		// Token: 0x17000E08 RID: 3592
		// (get) Token: 0x06005005 RID: 20485 RVA: 0x001195D8 File Offset: 0x001185D8
		// (set) Token: 0x06005006 RID: 20486 RVA: 0x001195DB File Offset: 0x001185DB
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

		// Token: 0x06005007 RID: 20487 RVA: 0x001195E0 File Offset: 0x001185E0
		public override byte[] DecryptKeyExchange(byte[] rgbIn)
		{
			if (this._rsaKey == null)
			{
				throw new CryptographicUnexpectedOperationException(Environment.GetResourceString("Cryptography_MissingKey"));
			}
			byte[] array;
			if (this._rsaKey is RSACryptoServiceProvider)
			{
				array = ((RSACryptoServiceProvider)this._rsaKey).Decrypt(rgbIn, false);
			}
			else
			{
				byte[] array2 = this._rsaKey.DecryptValue(rgbIn);
				int num = 2;
				while (num < array2.Length && array2[num] != 0)
				{
					num++;
				}
				if (num >= array2.Length)
				{
					throw new CryptographicUnexpectedOperationException(Environment.GetResourceString("Cryptography_PKCS1Decoding"));
				}
				num++;
				array = new byte[array2.Length - num];
				Buffer.InternalBlockCopy(array2, num, array, 0, array.Length);
			}
			return array;
		}

		// Token: 0x06005008 RID: 20488 RVA: 0x00119679 File Offset: 0x00118679
		public override void SetKey(AsymmetricAlgorithm key)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			this._rsaKey = (RSA)key;
		}

		// Token: 0x04002900 RID: 10496
		private RSA _rsaKey;

		// Token: 0x04002901 RID: 10497
		private RandomNumberGenerator RngValue;
	}
}
