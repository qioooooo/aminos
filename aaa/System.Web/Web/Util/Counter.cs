using System;

namespace System.Web.Util
{
	// Token: 0x0200075A RID: 1882
	internal sealed class Counter
	{
		// Token: 0x06005BAF RID: 23471 RVA: 0x00170545 File Offset: 0x0016F545
		private Counter()
		{
		}

		// Token: 0x170017A7 RID: 6055
		// (get) Token: 0x06005BB0 RID: 23472 RVA: 0x00170550 File Offset: 0x0016F550
		internal static long Value
		{
			get
			{
				long num = 0L;
				SafeNativeMethods.QueryPerformanceCounter(ref num);
				return num;
			}
		}

		// Token: 0x170017A8 RID: 6056
		// (get) Token: 0x06005BB1 RID: 23473 RVA: 0x0017056C File Offset: 0x0016F56C
		internal static long Frequency
		{
			get
			{
				long num = 0L;
				SafeNativeMethods.QueryPerformanceFrequency(ref num);
				return num;
			}
		}
	}
}
