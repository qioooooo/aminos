using System;

namespace System.IO.Ports
{
	// Token: 0x020007AB RID: 1963
	public enum SerialError
	{
		// Token: 0x0400351A RID: 13594
		TXFull = 256,
		// Token: 0x0400351B RID: 13595
		RXOver = 1,
		// Token: 0x0400351C RID: 13596
		Overrun,
		// Token: 0x0400351D RID: 13597
		RXParity = 4,
		// Token: 0x0400351E RID: 13598
		Frame = 8
	}
}
