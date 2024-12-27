using System;

namespace System.Data.ProviderBase
{
	// Token: 0x02000268 RID: 616
	internal sealed class DbConnectionClosedBusy : DbConnectionBusy
	{
		// Token: 0x06002105 RID: 8453 RVA: 0x00265294 File Offset: 0x00264694
		private DbConnectionClosedBusy()
			: base(ConnectionState.Closed)
		{
		}

		// Token: 0x04001545 RID: 5445
		internal static readonly DbConnectionInternal SingletonInstance = new DbConnectionClosedBusy();
	}
}
