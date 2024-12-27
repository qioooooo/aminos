using System;
using System.Net;
using System.Security.Principal;

namespace System.Runtime.Remoting.Channels
{
	// Token: 0x02000022 RID: 34
	public interface IAuthorizeRemotingConnection
	{
		// Token: 0x060000EB RID: 235
		bool IsConnectingEndPointAuthorized(EndPoint endPoint);

		// Token: 0x060000EC RID: 236
		bool IsConnectingIdentityAuthorized(IIdentity identity);
	}
}
