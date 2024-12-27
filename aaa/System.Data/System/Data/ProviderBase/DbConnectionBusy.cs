using System;
using System.Data.Common;

namespace System.Data.ProviderBase
{
	// Token: 0x02000267 RID: 615
	internal abstract class DbConnectionBusy : DbConnectionClosed
	{
		// Token: 0x06002103 RID: 8451 RVA: 0x00265264 File Offset: 0x00264664
		protected DbConnectionBusy(ConnectionState state)
			: base(state, true, false)
		{
		}

		// Token: 0x06002104 RID: 8452 RVA: 0x0026527C File Offset: 0x0026467C
		internal override void OpenConnection(DbConnection outerConnection, DbConnectionFactory connectionFactory)
		{
			throw ADP.ConnectionAlreadyOpen(base.State);
		}
	}
}
