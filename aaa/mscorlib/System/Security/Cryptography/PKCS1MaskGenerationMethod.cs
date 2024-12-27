using System;
using System.Runtime.InteropServices;

namespace System.Security.Cryptography
{
	// Token: 0x0200087D RID: 2173
	[ComVisible(true)]
	public class PKCS1MaskGenerationMethod : MaskGenerationMethod
	{
		// Token: 0x06004F90 RID: 20368 RVA: 0x00115ADF File Offset: 0x00114ADF
		public PKCS1MaskGenerationMethod()
		{
			this.HashNameValue = "SHA1";
		}

		// Token: 0x17000DF5 RID: 3573
		// (get) Token: 0x06004F91 RID: 20369 RVA: 0x00115AF2 File Offset: 0x00114AF2
		// (set) Token: 0x06004F92 RID: 20370 RVA: 0x00115AFA File Offset: 0x00114AFA
		public string HashName
		{
			get
			{
				return this.HashNameValue;
			}
			set
			{
				this.HashNameValue = value;
				if (this.HashNameValue == null)
				{
					this.HashNameValue = "SHA1";
				}
			}
		}

		// Token: 0x06004F93 RID: 20371 RVA: 0x00115B18 File Offset: 0x00114B18
		public override byte[] GenerateMask(byte[] rgbSeed, int cbReturn)
		{
			HashAlgorithm hashAlgorithm = (HashAlgorithm)CryptoConfig.CreateFromName(this.HashNameValue);
			byte[] array = new byte[4];
			byte[] array2 = new byte[cbReturn];
			uint num = 0U;
			for (int i = 0; i < array2.Length; i += hashAlgorithm.Hash.Length)
			{
				Utils.ConvertIntToByteArray(num++, ref array);
				hashAlgorithm.TransformBlock(rgbSeed, 0, rgbSeed.Length, rgbSeed, 0);
				hashAlgorithm.TransformFinalBlock(array, 0, 4);
				byte[] hash = hashAlgorithm.Hash;
				hashAlgorithm.Initialize();
				if (array2.Length - i > hash.Length)
				{
					Buffer.BlockCopy(hash, 0, array2, i, hash.Length);
				}
				else
				{
					Buffer.BlockCopy(hash, 0, array2, i, array2.Length - i);
				}
			}
			return array2;
		}

		// Token: 0x040028D3 RID: 10451
		private string HashNameValue;
	}
}
