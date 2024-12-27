using System;

namespace System.Net
{
	// Token: 0x020003EA RID: 1002
	internal static class IntPtrHelper
	{
		// Token: 0x06002074 RID: 8308 RVA: 0x0007FD50 File Offset: 0x0007ED50
		internal static IntPtr Add(IntPtr a, int b)
		{
			return (IntPtr)((long)a + (long)b);
		}

		// Token: 0x06002075 RID: 8309 RVA: 0x0007FD60 File Offset: 0x0007ED60
		internal static long Subtract(IntPtr a, IntPtr b)
		{
			return (long)a - (long)b;
		}
	}
}
