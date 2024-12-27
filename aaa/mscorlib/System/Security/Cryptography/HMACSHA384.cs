using System;
using System.Runtime.InteropServices;

namespace System.Security.Cryptography
{
	// Token: 0x02000873 RID: 2163
	[ComVisible(true)]
	public class HMACSHA384 : HMAC
	{
		// Token: 0x06004F3F RID: 20287 RVA: 0x001148C3 File Offset: 0x001138C3
		public HMACSHA384()
			: this(Utils.GenerateRandom(128))
		{
		}

		// Token: 0x06004F40 RID: 20288 RVA: 0x001148D8 File Offset: 0x001138D8
		public HMACSHA384(byte[] key)
		{
			Utils._ShowLegacyHmacWarning();
			this.m_hashName = "SHA384";
			this.m_hash1 = new SHA384Managed();
			this.m_hash2 = new SHA384Managed();
			this.HashSizeValue = 384;
			base.BlockSizeValue = this.BlockSize;
			base.InitializeKey(key);
		}

		// Token: 0x17000DD9 RID: 3545
		// (get) Token: 0x06004F41 RID: 20289 RVA: 0x0011493A File Offset: 0x0011393A
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

		// Token: 0x17000DDA RID: 3546
		// (get) Token: 0x06004F42 RID: 20290 RVA: 0x0011494C File Offset: 0x0011394C
		// (set) Token: 0x06004F43 RID: 20291 RVA: 0x00114954 File Offset: 0x00113954
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

		// Token: 0x040028B6 RID: 10422
		private bool m_useLegacyBlockSize = Utils._ProduceLegacyHmacValues();
	}
}
