using System;
using System.Diagnostics;

namespace System.EnterpriseServices
{
	// Token: 0x02000095 RID: 149
	internal static class Perf
	{
		// Token: 0x0600038B RID: 907 RVA: 0x0000BB1C File Offset: 0x0000AB1C
		static Perf()
		{
			Util.QueryPerformanceFrequency(out Perf._freq);
		}

		// Token: 0x0600038C RID: 908 RVA: 0x0000BB2C File Offset: 0x0000AB2C
		[Conditional("_DEBUG_PERF")]
		internal static void Tick(string name)
		{
			long num;
			Util.QueryPerformanceCounter(out num);
			if (Perf._count != 0L)
			{
				double num2 = (double)(num - Perf._count) / (double)Perf._freq;
			}
			Perf._count = num;
		}

		// Token: 0x0400016D RID: 365
		private static long _count;

		// Token: 0x0400016E RID: 366
		private static long _freq;
	}
}
