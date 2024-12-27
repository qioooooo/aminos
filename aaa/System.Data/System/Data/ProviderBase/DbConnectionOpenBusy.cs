using System;

namespace System.Data.ProviderBase
{
	// Token: 0x02000269 RID: 617
	internal sealed class DbConnectionOpenBusy : DbConnectionBusy
	{
		// Token: 0x06002107 RID: 8455 RVA: 0x002652C0 File Offset: 0x002646C0
		private DbConnectionOpenBusy()
			: base(ConnectionState.Open)
		{
		}

		// Token: 0x04001546 RID: 5446
		internal static readonly DbConnectionInternal SingletonInstance = new DbConnectionOpenBusy();
	}
}
