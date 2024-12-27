using System;

namespace System.IO.Ports
{
	// Token: 0x020007AE RID: 1966
	public enum SerialPinChange
	{
		// Token: 0x04003521 RID: 13601
		CtsChanged = 8,
		// Token: 0x04003522 RID: 13602
		DsrChanged = 16,
		// Token: 0x04003523 RID: 13603
		CDChanged = 32,
		// Token: 0x04003524 RID: 13604
		Ring = 256,
		// Token: 0x04003525 RID: 13605
		Break = 64
	}
}
