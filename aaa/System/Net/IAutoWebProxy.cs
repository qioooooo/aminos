using System;

namespace System.Net
{
	// Token: 0x020003AE RID: 942
	internal interface IAutoWebProxy : IWebProxy
	{
		// Token: 0x06001D94 RID: 7572
		ProxyChain GetProxies(Uri destination);
	}
}
