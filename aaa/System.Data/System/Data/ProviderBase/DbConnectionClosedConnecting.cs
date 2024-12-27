using System;

namespace System.Data.ProviderBase
{
	// Token: 0x0200026A RID: 618
	internal sealed class DbConnectionClosedConnecting : DbConnectionBusy
	{
		// Token: 0x06002109 RID: 8457 RVA: 0x002652EC File Offset: 0x002646EC
		private DbConnectionClosedConnecting()
			: base(ConnectionState.Connecting)
		{
		}

		// Token: 0x04001547 RID: 5447
		internal static readonly DbConnectionInternal SingletonInstance = new DbConnectionClosedConnecting();
	}
}
