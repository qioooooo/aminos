using System;
using System.Security.Permissions;

namespace System.Threading
{
	// Token: 0x02000159 RID: 345
	internal static class ThreadPoolGlobals
	{
		// Token: 0x06001356 RID: 4950 RVA: 0x00035171 File Offset: 0x00034171
		[EnvironmentPermission(SecurityAction.Assert, Read = "NUMBER_OF_PROCESSORS")]
		internal static int GetProcessorCount()
		{
			return Environment.ProcessorCount;
		}

		// Token: 0x0400065E RID: 1630
		public static uint tpQuantum = 2U;

		// Token: 0x0400065F RID: 1631
		public static int tpWarmupCount = ThreadPoolGlobals.GetProcessorCount() * 2;

		// Token: 0x04000660 RID: 1632
		public static bool tpHosted = ThreadPool.IsThreadPoolHosted();

		// Token: 0x04000661 RID: 1633
		public static bool vmTpInitialized;

		// Token: 0x04000662 RID: 1634
		public static ThreadPoolRequestQueue tpQueue = new ThreadPoolRequestQueue();
	}
}
