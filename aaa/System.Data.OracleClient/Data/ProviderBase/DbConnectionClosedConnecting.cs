using System;

namespace System.Data.ProviderBase
{
	// Token: 0x02000090 RID: 144
	internal sealed class DbConnectionClosedConnecting : DbConnectionBusy
	{
		// Token: 0x060007F8 RID: 2040 RVA: 0x00072DD4 File Offset: 0x000721D4
		private DbConnectionClosedConnecting()
			: base(ConnectionState.Connecting)
		{
		}

		// Token: 0x04000510 RID: 1296
		internal static readonly DbConnectionInternal SingletonInstance = new DbConnectionClosedConnecting();
	}
}
