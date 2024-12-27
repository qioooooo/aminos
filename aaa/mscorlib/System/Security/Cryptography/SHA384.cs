using System;
using System.Runtime.InteropServices;

namespace System.Security.Cryptography
{
	// Token: 0x02000899 RID: 2201
	[ComVisible(true)]
	public abstract class SHA384 : HashAlgorithm
	{
		// Token: 0x0600506E RID: 20590 RVA: 0x00120B91 File Offset: 0x0011FB91
		protected SHA384()
		{
			this.HashSizeValue = 384;
		}

		// Token: 0x0600506F RID: 20591 RVA: 0x00120BA4 File Offset: 0x0011FBA4
		public new static SHA384 Create()
		{
			return SHA384.Create("System.Security.Cryptography.SHA384");
		}

		// Token: 0x06005070 RID: 20592 RVA: 0x00120BB0 File Offset: 0x0011FBB0
		public new static SHA384 Create(string hashName)
		{
			return (SHA384)CryptoConfig.CreateFromName(hashName);
		}
	}
}
