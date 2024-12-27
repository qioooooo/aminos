using System;
using System.Runtime.InteropServices;

namespace System.Security.Cryptography
{
	// Token: 0x02000872 RID: 2162
	[ComVisible(true)]
	public class HMACSHA256 : HMAC
	{
		// Token: 0x06004F3D RID: 20285 RVA: 0x00114879 File Offset: 0x00113879
		public HMACSHA256()
			: this(Utils.GenerateRandom(64))
		{
		}

		// Token: 0x06004F3E RID: 20286 RVA: 0x00114888 File Offset: 0x00113888
		public HMACSHA256(byte[] key)
		{
			this.m_hashName = "SHA256";
			this.m_hash1 = new SHA256Managed();
			this.m_hash2 = new SHA256Managed();
			this.HashSizeValue = 256;
			base.InitializeKey(key);
		}
	}
}
