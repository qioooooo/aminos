using System;
using System.Web.SessionState;

namespace System.Web.Services.Protocols
{
	// Token: 0x0200008D RID: 141
	internal class AsyncSessionHandler : AsyncSessionlessHandler, IRequiresSessionState
	{
		// Token: 0x060003B4 RID: 948 RVA: 0x00012B48 File Offset: 0x00011B48
		internal AsyncSessionHandler(ServerProtocol protocol)
			: base(protocol)
		{
		}
	}
}
