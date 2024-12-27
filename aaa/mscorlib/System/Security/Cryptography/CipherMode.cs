using System;
using System.Runtime.InteropServices;

namespace System.Security.Cryptography
{
	// Token: 0x0200084A RID: 2122
	[ComVisible(true)]
	[Serializable]
	public enum CipherMode
	{
		// Token: 0x04002834 RID: 10292
		CBC = 1,
		// Token: 0x04002835 RID: 10293
		ECB,
		// Token: 0x04002836 RID: 10294
		OFB,
		// Token: 0x04002837 RID: 10295
		CFB,
		// Token: 0x04002838 RID: 10296
		CTS
	}
}
