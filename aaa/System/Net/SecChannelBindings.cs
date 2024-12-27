using System;
using System.Runtime.InteropServices;

namespace System.Net
{
	// Token: 0x0200040D RID: 1037
	[StructLayout(LayoutKind.Sequential)]
	internal class SecChannelBindings
	{
		// Token: 0x040020BD RID: 8381
		internal int dwInitiatorAddrType;

		// Token: 0x040020BE RID: 8382
		internal int cbInitiatorLength;

		// Token: 0x040020BF RID: 8383
		internal int dwInitiatorOffset;

		// Token: 0x040020C0 RID: 8384
		internal int dwAcceptorAddrType;

		// Token: 0x040020C1 RID: 8385
		internal int cbAcceptorLength;

		// Token: 0x040020C2 RID: 8386
		internal int dwAcceptorOffset;

		// Token: 0x040020C3 RID: 8387
		internal int cbApplicationDataLength;

		// Token: 0x040020C4 RID: 8388
		internal int dwApplicationDataOffset;
	}
}
