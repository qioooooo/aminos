using System;
using System.Runtime.InteropServices;

namespace System.Security.Cryptography
{
	// Token: 0x02000852 RID: 2130
	[ComVisible(true)]
	public abstract class AsymmetricAlgorithm : IDisposable
	{
		// Token: 0x06004E35 RID: 20021 RVA: 0x0010FCC3 File Offset: 0x0010ECC3
		void IDisposable.Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06004E36 RID: 20022 RVA: 0x0010FCD2 File Offset: 0x0010ECD2
		public void Clear()
		{
			((IDisposable)this).Dispose();
		}

		// Token: 0x06004E37 RID: 20023
		protected abstract void Dispose(bool disposing);

		// Token: 0x17000D9E RID: 3486
		// (get) Token: 0x06004E38 RID: 20024 RVA: 0x0010FCDA File Offset: 0x0010ECDA
		// (set) Token: 0x06004E39 RID: 20025 RVA: 0x0010FCE4 File Offset: 0x0010ECE4
		public virtual int KeySize
		{
			get
			{
				return this.KeySizeValue;
			}
			set
			{
				for (int i = 0; i < this.LegalKeySizesValue.Length; i++)
				{
					if (this.LegalKeySizesValue[i].SkipSize == 0)
					{
						if (this.LegalKeySizesValue[i].MinSize == value)
						{
							this.KeySizeValue = value;
							return;
						}
					}
					else
					{
						for (int j = this.LegalKeySizesValue[i].MinSize; j <= this.LegalKeySizesValue[i].MaxSize; j += this.LegalKeySizesValue[i].SkipSize)
						{
							if (j == value)
							{
								this.KeySizeValue = value;
								return;
							}
						}
					}
				}
				throw new CryptographicException(Environment.GetResourceString("Cryptography_InvalidKeySize"));
			}
		}

		// Token: 0x17000D9F RID: 3487
		// (get) Token: 0x06004E3A RID: 20026 RVA: 0x0010FD76 File Offset: 0x0010ED76
		public virtual KeySizes[] LegalKeySizes
		{
			get
			{
				return (KeySizes[])this.LegalKeySizesValue.Clone();
			}
		}

		// Token: 0x17000DA0 RID: 3488
		// (get) Token: 0x06004E3B RID: 20027
		public abstract string SignatureAlgorithm { get; }

		// Token: 0x17000DA1 RID: 3489
		// (get) Token: 0x06004E3C RID: 20028
		public abstract string KeyExchangeAlgorithm { get; }

		// Token: 0x06004E3D RID: 20029 RVA: 0x0010FD88 File Offset: 0x0010ED88
		public static AsymmetricAlgorithm Create()
		{
			return AsymmetricAlgorithm.Create("System.Security.Cryptography.AsymmetricAlgorithm");
		}

		// Token: 0x06004E3E RID: 20030 RVA: 0x0010FD94 File Offset: 0x0010ED94
		public static AsymmetricAlgorithm Create(string algName)
		{
			return (AsymmetricAlgorithm)CryptoConfig.CreateFromName(algName);
		}

		// Token: 0x06004E3F RID: 20031
		public abstract void FromXmlString(string xmlString);

		// Token: 0x06004E40 RID: 20032
		public abstract string ToXmlString(bool includePrivateParameters);

		// Token: 0x04002846 RID: 10310
		protected int KeySizeValue;

		// Token: 0x04002847 RID: 10311
		protected KeySizes[] LegalKeySizesValue;
	}
}
