using System;
using System.Runtime.InteropServices;

namespace System.Security.Cryptography
{
	// Token: 0x02000861 RID: 2145
	[ComVisible(true)]
	public abstract class SymmetricAlgorithm : IDisposable
	{
		// Token: 0x06004EA7 RID: 20135 RVA: 0x00112941 File Offset: 0x00111941
		protected SymmetricAlgorithm()
		{
			this.ModeValue = CipherMode.CBC;
			this.PaddingValue = PaddingMode.PKCS7;
		}

		// Token: 0x06004EA8 RID: 20136 RVA: 0x00112957 File Offset: 0x00111957
		void IDisposable.Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06004EA9 RID: 20137 RVA: 0x00112966 File Offset: 0x00111966
		public void Clear()
		{
			((IDisposable)this).Dispose();
		}

		// Token: 0x06004EAA RID: 20138 RVA: 0x00112970 File Offset: 0x00111970
		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (this.KeyValue != null)
				{
					Array.Clear(this.KeyValue, 0, this.KeyValue.Length);
					this.KeyValue = null;
				}
				if (this.IVValue != null)
				{
					Array.Clear(this.IVValue, 0, this.IVValue.Length);
					this.IVValue = null;
				}
			}
		}

		// Token: 0x17000DBD RID: 3517
		// (get) Token: 0x06004EAB RID: 20139 RVA: 0x001129C6 File Offset: 0x001119C6
		// (set) Token: 0x06004EAC RID: 20140 RVA: 0x001129D0 File Offset: 0x001119D0
		public virtual int BlockSize
		{
			get
			{
				return this.BlockSizeValue;
			}
			set
			{
				for (int i = 0; i < this.LegalBlockSizesValue.Length; i++)
				{
					if (this.LegalBlockSizesValue[i].SkipSize == 0)
					{
						if (this.LegalBlockSizesValue[i].MinSize == value)
						{
							this.BlockSizeValue = value;
							this.IVValue = null;
							return;
						}
					}
					else
					{
						for (int j = this.LegalBlockSizesValue[i].MinSize; j <= this.LegalBlockSizesValue[i].MaxSize; j += this.LegalBlockSizesValue[i].SkipSize)
						{
							if (j == value)
							{
								if (this.BlockSizeValue != value)
								{
									this.BlockSizeValue = value;
									this.IVValue = null;
								}
								return;
							}
						}
					}
				}
				throw new CryptographicException(Environment.GetResourceString("Cryptography_InvalidBlockSize"));
			}
		}

		// Token: 0x17000DBE RID: 3518
		// (get) Token: 0x06004EAD RID: 20141 RVA: 0x00112A7C File Offset: 0x00111A7C
		// (set) Token: 0x06004EAE RID: 20142 RVA: 0x00112A84 File Offset: 0x00111A84
		public virtual int FeedbackSize
		{
			get
			{
				return this.FeedbackSizeValue;
			}
			set
			{
				if (value <= 0 || value > this.BlockSizeValue || value % 8 != 0)
				{
					throw new CryptographicException(Environment.GetResourceString("Cryptography_InvalidFeedbackSize"));
				}
				this.FeedbackSizeValue = value;
			}
		}

		// Token: 0x17000DBF RID: 3519
		// (get) Token: 0x06004EAF RID: 20143 RVA: 0x00112AAF File Offset: 0x00111AAF
		// (set) Token: 0x06004EB0 RID: 20144 RVA: 0x00112ACF File Offset: 0x00111ACF
		public virtual byte[] IV
		{
			get
			{
				if (this.IVValue == null)
				{
					this.GenerateIV();
				}
				return (byte[])this.IVValue.Clone();
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				if (value.Length != this.BlockSizeValue / 8)
				{
					throw new CryptographicException(Environment.GetResourceString("Cryptography_InvalidIVSize"));
				}
				this.IVValue = (byte[])value.Clone();
			}
		}

		// Token: 0x17000DC0 RID: 3520
		// (get) Token: 0x06004EB1 RID: 20145 RVA: 0x00112B0D File Offset: 0x00111B0D
		// (set) Token: 0x06004EB2 RID: 20146 RVA: 0x00112B30 File Offset: 0x00111B30
		public virtual byte[] Key
		{
			get
			{
				if (this.KeyValue == null)
				{
					this.GenerateKey();
				}
				return (byte[])this.KeyValue.Clone();
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				if (!this.ValidKeySize(value.Length * 8))
				{
					throw new CryptographicException(Environment.GetResourceString("Cryptography_InvalidKeySize"));
				}
				this.KeyValue = (byte[])value.Clone();
				this.KeySizeValue = value.Length * 8;
			}
		}

		// Token: 0x17000DC1 RID: 3521
		// (get) Token: 0x06004EB3 RID: 20147 RVA: 0x00112B84 File Offset: 0x00111B84
		public virtual KeySizes[] LegalBlockSizes
		{
			get
			{
				return (KeySizes[])this.LegalBlockSizesValue.Clone();
			}
		}

		// Token: 0x17000DC2 RID: 3522
		// (get) Token: 0x06004EB4 RID: 20148 RVA: 0x00112B96 File Offset: 0x00111B96
		public virtual KeySizes[] LegalKeySizes
		{
			get
			{
				return (KeySizes[])this.LegalKeySizesValue.Clone();
			}
		}

		// Token: 0x17000DC3 RID: 3523
		// (get) Token: 0x06004EB5 RID: 20149 RVA: 0x00112BA8 File Offset: 0x00111BA8
		// (set) Token: 0x06004EB6 RID: 20150 RVA: 0x00112BB0 File Offset: 0x00111BB0
		public virtual int KeySize
		{
			get
			{
				return this.KeySizeValue;
			}
			set
			{
				if (!this.ValidKeySize(value))
				{
					throw new CryptographicException(Environment.GetResourceString("Cryptography_InvalidKeySize"));
				}
				this.KeySizeValue = value;
				this.KeyValue = null;
			}
		}

		// Token: 0x17000DC4 RID: 3524
		// (get) Token: 0x06004EB7 RID: 20151 RVA: 0x00112BD9 File Offset: 0x00111BD9
		// (set) Token: 0x06004EB8 RID: 20152 RVA: 0x00112BE1 File Offset: 0x00111BE1
		public virtual CipherMode Mode
		{
			get
			{
				return this.ModeValue;
			}
			set
			{
				if (value < CipherMode.CBC || CipherMode.CFB < value)
				{
					throw new CryptographicException(Environment.GetResourceString("Cryptography_InvalidCipherMode"));
				}
				this.ModeValue = value;
			}
		}

		// Token: 0x17000DC5 RID: 3525
		// (get) Token: 0x06004EB9 RID: 20153 RVA: 0x00112C02 File Offset: 0x00111C02
		// (set) Token: 0x06004EBA RID: 20154 RVA: 0x00112C0A File Offset: 0x00111C0A
		public virtual PaddingMode Padding
		{
			get
			{
				return this.PaddingValue;
			}
			set
			{
				if (value < PaddingMode.None || PaddingMode.ISO10126 < value)
				{
					throw new CryptographicException(Environment.GetResourceString("Cryptography_InvalidPaddingMode"));
				}
				this.PaddingValue = value;
			}
		}

		// Token: 0x06004EBB RID: 20155 RVA: 0x00112C2C File Offset: 0x00111C2C
		public bool ValidKeySize(int bitLength)
		{
			KeySizes[] legalKeySizes = this.LegalKeySizes;
			if (legalKeySizes == null)
			{
				return false;
			}
			for (int i = 0; i < legalKeySizes.Length; i++)
			{
				if (legalKeySizes[i].SkipSize == 0)
				{
					if (legalKeySizes[i].MinSize == bitLength)
					{
						return true;
					}
				}
				else
				{
					for (int j = legalKeySizes[i].MinSize; j <= legalKeySizes[i].MaxSize; j += legalKeySizes[i].SkipSize)
					{
						if (j == bitLength)
						{
							return true;
						}
					}
				}
			}
			return false;
		}

		// Token: 0x06004EBC RID: 20156 RVA: 0x00112C92 File Offset: 0x00111C92
		public static SymmetricAlgorithm Create()
		{
			return SymmetricAlgorithm.Create("System.Security.Cryptography.SymmetricAlgorithm");
		}

		// Token: 0x06004EBD RID: 20157 RVA: 0x00112C9E File Offset: 0x00111C9E
		public static SymmetricAlgorithm Create(string algName)
		{
			return (SymmetricAlgorithm)CryptoConfig.CreateFromName(algName);
		}

		// Token: 0x06004EBE RID: 20158 RVA: 0x00112CAB File Offset: 0x00111CAB
		public virtual ICryptoTransform CreateEncryptor()
		{
			return this.CreateEncryptor(this.Key, this.IV);
		}

		// Token: 0x06004EBF RID: 20159
		public abstract ICryptoTransform CreateEncryptor(byte[] rgbKey, byte[] rgbIV);

		// Token: 0x06004EC0 RID: 20160 RVA: 0x00112CBF File Offset: 0x00111CBF
		public virtual ICryptoTransform CreateDecryptor()
		{
			return this.CreateDecryptor(this.Key, this.IV);
		}

		// Token: 0x06004EC1 RID: 20161
		public abstract ICryptoTransform CreateDecryptor(byte[] rgbKey, byte[] rgbIV);

		// Token: 0x06004EC2 RID: 20162
		public abstract void GenerateKey();

		// Token: 0x06004EC3 RID: 20163
		public abstract void GenerateIV();

		// Token: 0x04002884 RID: 10372
		protected int BlockSizeValue;

		// Token: 0x04002885 RID: 10373
		protected int FeedbackSizeValue;

		// Token: 0x04002886 RID: 10374
		protected byte[] IVValue;

		// Token: 0x04002887 RID: 10375
		protected byte[] KeyValue;

		// Token: 0x04002888 RID: 10376
		protected KeySizes[] LegalBlockSizesValue;

		// Token: 0x04002889 RID: 10377
		protected KeySizes[] LegalKeySizesValue;

		// Token: 0x0400288A RID: 10378
		protected int KeySizeValue;

		// Token: 0x0400288B RID: 10379
		protected CipherMode ModeValue;

		// Token: 0x0400288C RID: 10380
		protected PaddingMode PaddingValue;
	}
}
