using System;
using System.Runtime.InteropServices;

namespace System.Security.Cryptography
{
	// Token: 0x02000874 RID: 2164
	[ComVisible(true)]
	public class HMACSHA512 : HMAC
	{
		// Token: 0x06004F44 RID: 20292 RVA: 0x00114975 File Offset: 0x00113975
		public HMACSHA512()
			: this(Utils.GenerateRandom(128))
		{
		}

		// Token: 0x06004F45 RID: 20293 RVA: 0x00114988 File Offset: 0x00113988
		public HMACSHA512(byte[] key)
		{
			Utils._ShowLegacyHmacWarning();
			this.m_hashName = "SHA512";
			this.m_hash1 = new SHA512Managed();
			this.m_hash2 = new SHA512Managed();
			this.HashSizeValue = 512;
			base.BlockSizeValue = this.BlockSize;
			base.InitializeKey(key);
		}

		// Token: 0x17000DDB RID: 3547
		// (get) Token: 0x06004F46 RID: 20294 RVA: 0x001149EA File Offset: 0x001139EA
		private int BlockSize
		{
			get
			{
				if (!this.m_useLegacyBlockSize)
				{
					return 128;
				}
				return 64;
			}
		}

		// Token: 0x17000DDC RID: 3548
		// (get) Token: 0x06004F47 RID: 20295 RVA: 0x001149FC File Offset: 0x001139FC
		// (set) Token: 0x06004F48 RID: 20296 RVA: 0x00114A04 File Offset: 0x00113A04
		public bool ProduceLegacyHmacValues
		{
			get
			{
				return this.m_useLegacyBlockSize;
			}
			set
			{
				this.m_useLegacyBlockSize = value;
				base.BlockSizeValue = this.BlockSize;
				base.InitializeKey(this.KeyValue);
			}
		}

		// Token: 0x040028B7 RID: 10423
		private bool m_useLegacyBlockSize = Utils._ProduceLegacyHmacValues();
	}
}
