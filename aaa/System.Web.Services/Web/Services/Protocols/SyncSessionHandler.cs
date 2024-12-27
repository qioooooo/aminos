using System;
using System.Web.SessionState;

namespace System.Web.Services.Protocols
{
	// Token: 0x0200008B RID: 139
	internal class SyncSessionHandler : SyncSessionlessHandler, IRequiresSessionState
	{
		// Token: 0x060003B0 RID: 944 RVA: 0x00012A7E File Offset: 0x00011A7E
		internal SyncSessionHandler(ServerProtocol protocol)
			: base(protocol)
		{
		}
	}
}
