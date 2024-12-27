using System;
using System.Runtime.InteropServices;

namespace System.Security.Cryptography
{
	// Token: 0x0200089B RID: 2203
	[ComVisible(true)]
	public abstract class SHA512 : HashAlgorithm
	{
		// Token: 0x06005082 RID: 20610 RVA: 0x001215B9 File Offset: 0x001205B9
		protected SHA512()
		{
			this.HashSizeValue = 512;
		}

		// Token: 0x06005083 RID: 20611 RVA: 0x001215CC File Offset: 0x001205CC
		public new static SHA512 Create()
		{
			return SHA512.Create("System.Security.Cryptography.SHA512");
		}

		// Token: 0x06005084 RID: 20612 RVA: 0x001215D8 File Offset: 0x001205D8
		public new static SHA512 Create(string hashName)
		{
			return (SHA512)CryptoConfig.CreateFromName(hashName);
		}
	}
}
