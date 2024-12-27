using System;
using System.Runtime.InteropServices;

namespace System.Security.Cryptography
{
	// Token: 0x02000897 RID: 2199
	[ComVisible(true)]
	public abstract class SHA256 : HashAlgorithm
	{
		// Token: 0x0600505A RID: 20570 RVA: 0x00120334 File Offset: 0x0011F334
		protected SHA256()
		{
			this.HashSizeValue = 256;
		}

		// Token: 0x0600505B RID: 20571 RVA: 0x00120347 File Offset: 0x0011F347
		public new static SHA256 Create()
		{
			return SHA256.Create("System.Security.Cryptography.SHA256");
		}

		// Token: 0x0600505C RID: 20572 RVA: 0x00120353 File Offset: 0x0011F353
		public new static SHA256 Create(string hashName)
		{
			return (SHA256)CryptoConfig.CreateFromName(hashName);
		}
	}
}
