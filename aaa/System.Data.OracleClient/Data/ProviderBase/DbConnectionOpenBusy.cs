using System;

namespace System.Data.ProviderBase
{
	// Token: 0x0200008F RID: 143
	internal sealed class DbConnectionOpenBusy : DbConnectionBusy
	{
		// Token: 0x060007F6 RID: 2038 RVA: 0x00072DA8 File Offset: 0x000721A8
		private DbConnectionOpenBusy()
			: base(ConnectionState.Open)
		{
		}

		// Token: 0x0400050F RID: 1295
		internal static readonly DbConnectionInternal SingletonInstance = new DbConnectionOpenBusy();
	}
}
