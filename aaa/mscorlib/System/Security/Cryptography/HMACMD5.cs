using System;
using System.Runtime.InteropServices;

namespace System.Security.Cryptography
{
	// Token: 0x0200086F RID: 2159
	[ComVisible(true)]
	public class HMACMD5 : HMAC
	{
		// Token: 0x06004F36 RID: 20278 RVA: 0x00114769 File Offset: 0x00113769
		public HMACMD5()
			: this(Utils.GenerateRandom(64))
		{
		}

		// Token: 0x06004F37 RID: 20279 RVA: 0x00114778 File Offset: 0x00113778
		public HMACMD5(byte[] key)
		{
			this.m_hashName = "MD5";
			this.m_hash1 = new MD5CryptoServiceProvider();
			this.m_hash2 = new MD5CryptoServiceProvider();
			this.HashSizeValue = 128;
			base.InitializeKey(key);
		}
	}
}
