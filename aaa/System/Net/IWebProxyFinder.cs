using System;
using System.Collections.Generic;

namespace System.Net
{
	// Token: 0x0200037F RID: 895
	internal interface IWebProxyFinder : IDisposable
	{
		// Token: 0x06001BF4 RID: 7156
		bool GetProxies(Uri destination, out IList<string> proxyList);

		// Token: 0x06001BF5 RID: 7157
		void Abort();

		// Token: 0x06001BF6 RID: 7158
		void Reset();

		// Token: 0x1700055E RID: 1374
		// (get) Token: 0x06001BF7 RID: 7159
		bool IsValid { get; }
	}
}
