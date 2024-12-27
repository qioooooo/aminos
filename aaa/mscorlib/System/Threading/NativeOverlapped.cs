using System;
using System.Runtime.InteropServices;

namespace System.Threading
{
	// Token: 0x0200014A RID: 330
	[ComVisible(true)]
	public struct NativeOverlapped
	{
		// Token: 0x04000617 RID: 1559
		public IntPtr InternalLow;

		// Token: 0x04000618 RID: 1560
		public IntPtr InternalHigh;

		// Token: 0x04000619 RID: 1561
		public int OffsetLow;

		// Token: 0x0400061A RID: 1562
		public int OffsetHigh;

		// Token: 0x0400061B RID: 1563
		public IntPtr EventHandle;
	}
}
