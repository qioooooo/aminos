using System;
using System.Runtime.InteropServices;

namespace System.Security.Cryptography
{
	// Token: 0x0200084B RID: 2123
	[ComVisible(true)]
	[Serializable]
	public enum PaddingMode
	{
		// Token: 0x0400283A RID: 10298
		None = 1,
		// Token: 0x0400283B RID: 10299
		PKCS7,
		// Token: 0x0400283C RID: 10300
		Zeros,
		// Token: 0x0400283D RID: 10301
		ANSIX923,
		// Token: 0x0400283E RID: 10302
		ISO10126
	}
}
