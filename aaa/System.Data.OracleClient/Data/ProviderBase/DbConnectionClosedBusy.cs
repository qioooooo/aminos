using System;

namespace System.Data.ProviderBase
{
	// Token: 0x0200008E RID: 142
	internal sealed class DbConnectionClosedBusy : DbConnectionBusy
	{
		// Token: 0x060007F4 RID: 2036 RVA: 0x00072D7C File Offset: 0x0007217C
		private DbConnectionClosedBusy()
			: base(ConnectionState.Closed)
		{
		}

		// Token: 0x0400050E RID: 1294
		internal static readonly DbConnectionInternal SingletonInstance = new DbConnectionClosedBusy();
	}
}
