using System;
using System.Threading;

namespace System.Deployment.Application
{
	// Token: 0x020000DB RID: 219
	internal class SplashInfo
	{
		// Token: 0x040004A1 RID: 1185
		public bool initializedAsWait = true;

		// Token: 0x040004A2 RID: 1186
		public ManualResetEvent pieceReady = new ManualResetEvent(true);

		// Token: 0x040004A3 RID: 1187
		public bool cancelled;
	}
}
