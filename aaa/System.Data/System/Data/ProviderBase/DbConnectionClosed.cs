using System;
using System.Data.Common;
using System.Transactions;

namespace System.Data.ProviderBase
{
	// Token: 0x02000266 RID: 614
	internal abstract class DbConnectionClosed : DbConnectionInternal
	{
		// Token: 0x060020F9 RID: 8441 RVA: 0x0026513C File Offset: 0x0026453C
		protected DbConnectionClosed(ConnectionState state, bool hidePassword, bool allowSetConnectionString)
			: base(state, hidePassword, allowSetConnectionString)
		{
		}

		// Token: 0x1700048D RID: 1165
		// (get) Token: 0x060020FA RID: 8442 RVA: 0x00265154 File Offset: 0x00264554
		public override string ServerVersion
		{
			get
			{
				throw ADP.ClosedConnectionError();
			}
		}

		// Token: 0x060020FB RID: 8443 RVA: 0x00265168 File Offset: 0x00264568
		protected override void Activate(Transaction transaction)
		{
			throw ADP.ClosedConnectionError();
		}

		// Token: 0x060020FC RID: 8444 RVA: 0x0026517C File Offset: 0x0026457C
		public override DbTransaction BeginTransaction(IsolationLevel il)
		{
			throw ADP.ClosedConnectionError();
		}

		// Token: 0x060020FD RID: 8445 RVA: 0x00265190 File Offset: 0x00264590
		public override void ChangeDatabase(string database)
		{
			throw ADP.ClosedConnectionError();
		}

		// Token: 0x060020FE RID: 8446 RVA: 0x002651A4 File Offset: 0x002645A4
		internal override void CloseConnection(DbConnection owningObject, DbConnectionFactory connectionFactory)
		{
		}

		// Token: 0x060020FF RID: 8447 RVA: 0x002651B4 File Offset: 0x002645B4
		protected override void Deactivate()
		{
			throw ADP.ClosedConnectionError();
		}

		// Token: 0x06002100 RID: 8448 RVA: 0x002651C8 File Offset: 0x002645C8
		public override void EnlistTransaction(Transaction transaction)
		{
			throw ADP.ClosedConnectionError();
		}

		// Token: 0x06002101 RID: 8449 RVA: 0x002651DC File Offset: 0x002645DC
		protected internal override DataTable GetSchema(DbConnectionFactory factory, DbConnectionPoolGroup poolGroup, DbConnection outerConnection, string collectionName, string[] restrictions)
		{
			throw ADP.ClosedConnectionError();
		}

		// Token: 0x06002102 RID: 8450 RVA: 0x002651F0 File Offset: 0x002645F0
		internal override void OpenConnection(DbConnection outerConnection, DbConnectionFactory connectionFactory)
		{
			if (connectionFactory.SetInnerConnectionFrom(outerConnection, DbConnectionClosedConnecting.SingletonInstance, this))
			{
				DbConnectionInternal dbConnectionInternal = null;
				try
				{
					connectionFactory.PermissionDemand(outerConnection);
					dbConnectionInternal = connectionFactory.GetConnection(outerConnection);
				}
				catch
				{
					connectionFactory.SetInnerConnectionTo(outerConnection, this);
					throw;
				}
				if (dbConnectionInternal == null)
				{
					connectionFactory.SetInnerConnectionTo(outerConnection, this);
					throw ADP.InternalConnectionError(ADP.ConnectionError.GetConnectionReturnsNull);
				}
				connectionFactory.SetInnerConnectionEvent(outerConnection, dbConnectionInternal);
			}
		}
	}
}
