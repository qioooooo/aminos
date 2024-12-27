using System;
using System.Data.Common;

namespace System.Data.ProviderBase
{
	// Token: 0x0200008D RID: 141
	internal abstract class DbConnectionBusy : DbConnectionClosed
	{
		// Token: 0x060007F2 RID: 2034 RVA: 0x00072D4C File Offset: 0x0007214C
		protected DbConnectionBusy(ConnectionState state)
			: base(state, true, false)
		{
		}

		// Token: 0x060007F3 RID: 2035 RVA: 0x00072D64 File Offset: 0x00072164
		internal override void OpenConnection(DbConnection outerConnection, DbConnectionFactory connectionFactory)
		{
			throw ADP.ConnectionAlreadyOpen(base.State);
		}
	}
}
