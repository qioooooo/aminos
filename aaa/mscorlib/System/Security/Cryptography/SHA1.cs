using System;
using System.Runtime.InteropServices;

namespace System.Security.Cryptography
{
	// Token: 0x02000894 RID: 2196
	[ComVisible(true)]
	public abstract class SHA1 : HashAlgorithm
	{
		// Token: 0x06005049 RID: 20553 RVA: 0x0011FAA4 File Offset: 0x0011EAA4
		protected SHA1()
		{
			this.HashSizeValue = 160;
		}

		// Token: 0x0600504A RID: 20554 RVA: 0x0011FAB7 File Offset: 0x0011EAB7
		public new static SHA1 Create()
		{
			return SHA1.Create("System.Security.Cryptography.SHA1");
		}

		// Token: 0x0600504B RID: 20555 RVA: 0x0011FAC3 File Offset: 0x0011EAC3
		public new static SHA1 Create(string hashName)
		{
			return (SHA1)CryptoConfig.CreateFromName(hashName);
		}
	}
}
