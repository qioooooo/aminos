using System;
using System.Threading;
using Microsoft.Internal.Performance;

namespace System.Deployment.Application
{
	// Token: 0x020000E2 RID: 226
	internal static class CodeMarker_Singleton
	{
		// Token: 0x17000145 RID: 325
		// (get) Token: 0x060005D4 RID: 1492 RVA: 0x0001E27C File Offset: 0x0001D27C
		public static CodeMarkers Instance
		{
			get
			{
				if (CodeMarker_Singleton.codemarkers == null)
				{
					lock (CodeMarker_Singleton.syncObject)
					{
						if (CodeMarker_Singleton.codemarkers == null)
						{
							CodeMarkers instance = CodeMarkers.Instance;
							instance.InitPerformanceDll(CodeMarkerApp.CLICKONCEPERF, "Software\\Microsoft\\VisualStudio\\8.0");
							Thread.MemoryBarrier();
							CodeMarker_Singleton.codemarkers = instance;
						}
					}
				}
				return CodeMarker_Singleton.codemarkers;
			}
		}

		// Token: 0x040004AD RID: 1197
		private static CodeMarkers codemarkers = null;

		// Token: 0x040004AE RID: 1198
		private static object syncObject = new object();
	}
}
