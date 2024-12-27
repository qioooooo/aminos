using System;

namespace System.Net
{
	// Token: 0x0200055B RID: 1371
	internal static class Win32
	{
		// Token: 0x0400287E RID: 10366
		internal const int OverlappedInternalOffset = 0;

		// Token: 0x0400287F RID: 10367
		internal static int OverlappedInternalHighOffset = IntPtr.Size;

		// Token: 0x04002880 RID: 10368
		internal static int OverlappedOffsetOffset = IntPtr.Size * 2;

		// Token: 0x04002881 RID: 10369
		internal static int OverlappedOffsetHighOffset = IntPtr.Size * 2 + 4;

		// Token: 0x04002882 RID: 10370
		internal static int OverlappedhEventOffset = IntPtr.Size * 2 + 8;

		// Token: 0x04002883 RID: 10371
		internal static int OverlappedSize = IntPtr.Size * 3 + 8;
	}
}
