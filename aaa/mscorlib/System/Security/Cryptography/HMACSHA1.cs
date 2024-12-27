using System;
using System.Runtime.InteropServices;

namespace System.Security.Cryptography
{
	// Token: 0x02000871 RID: 2161
	[ComVisible(true)]
	public class HMACSHA1 : HMAC
	{
		// Token: 0x06004F3A RID: 20282 RVA: 0x001147FD File Offset: 0x001137FD
		public HMACSHA1()
			: this(Utils.GenerateRandom(64))
		{
		}

		// Token: 0x06004F3B RID: 20283 RVA: 0x0011480C File Offset: 0x0011380C
		public HMACSHA1(byte[] key)
			: this(key, false)
		{
		}

		// Token: 0x06004F3C RID: 20284 RVA: 0x00114818 File Offset: 0x00113818
		public HMACSHA1(byte[] key, bool useManagedSha1)
		{
			this.m_hashName = "SHA1";
			if (useManagedSha1)
			{
				this.m_hash1 = new SHA1Managed();
				this.m_hash2 = new SHA1Managed();
			}
			else
			{
				this.m_hash1 = new SHA1CryptoServiceProvider();
				this.m_hash2 = new SHA1CryptoServiceProvider();
			}
			this.HashSizeValue = 160;
			base.InitializeKey(key);
		}
	}
}
