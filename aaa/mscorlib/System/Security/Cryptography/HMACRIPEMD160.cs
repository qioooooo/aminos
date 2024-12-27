using System;
using System.Runtime.InteropServices;

namespace System.Security.Cryptography
{
	// Token: 0x02000870 RID: 2160
	[ComVisible(true)]
	public class HMACRIPEMD160 : HMAC
	{
		// Token: 0x06004F38 RID: 20280 RVA: 0x001147B3 File Offset: 0x001137B3
		public HMACRIPEMD160()
			: this(Utils.GenerateRandom(64))
		{
		}

		// Token: 0x06004F39 RID: 20281 RVA: 0x001147C2 File Offset: 0x001137C2
		public HMACRIPEMD160(byte[] key)
		{
			this.m_hashName = "RIPEMD160";
			this.m_hash1 = new RIPEMD160Managed();
			this.m_hash2 = new RIPEMD160Managed();
			this.HashSizeValue = 160;
			base.InitializeKey(key);
		}
	}
}
