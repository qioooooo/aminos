using System;
using System.Data.Common;

namespace System.Data.Odbc
{
	// Token: 0x02000207 RID: 519
	public sealed class OdbcTransaction : DbTransaction
	{
		// Token: 0x06001CB1 RID: 7345 RVA: 0x0024DBB0 File Offset: 0x0024CFB0
		internal OdbcTransaction(OdbcConnection connection, IsolationLevel isolevel, OdbcConnectionHandle handle)
		{
			this._connection = connection;
			this._isolevel = isolevel;
			this._handle = handle;
		}

		// Token: 0x170003DB RID: 987
		// (get) Token: 0x06001CB2 RID: 7346 RVA: 0x0024DBE0 File Offset: 0x0024CFE0
		public new OdbcConnection Connection
		{
			get
			{
				return this._connection;
			}
		}

		// Token: 0x170003DC RID: 988
		// (get) Token: 0x06001CB3 RID: 7347 RVA: 0x0024DBF4 File Offset: 0x0024CFF4
		protected override DbConnection DbConnection
		{
			get
			{
				return this.Connection;
			}
		}

		// Token: 0x170003DD RID: 989
		// (get) Token: 0x06001CB4 RID: 7348 RVA: 0x0024DC08 File Offset: 0x0024D008
		public override IsolationLevel IsolationLevel
		{
			get
			{
				OdbcConnection connection = this._connection;
				if (connection == null)
				{
					throw ADP.TransactionZombied(this);
				}
				if (IsolationLevel.Unspecified == this._isolevel)
				{
					int connectAttr = connection.GetConnectAttr(ODBC32.SQL_ATTR.TXN_ISOLATION, ODBC32.HANDLER.THROW);
					ODBC32.SQL_TRANSACTION sql_TRANSACTION = (ODBC32.SQL_TRANSACTION)connectAttr;
					switch (sql_TRANSACTION)
					{
					case ODBC32.SQL_TRANSACTION.READ_UNCOMMITTED:
						this._isolevel = IsolationLevel.ReadUncommitted;
						goto IL_0094;
					case ODBC32.SQL_TRANSACTION.READ_COMMITTED:
						this._isolevel = IsolationLevel.ReadCommitted;
						goto IL_0094;
					case (ODBC32.SQL_TRANSACTION)3:
						break;
					case ODBC32.SQL_TRANSACTION.REPEATABLE_READ:
						this._isolevel = IsolationLevel.RepeatableRead;
						goto IL_0094;
					default:
						if (sql_TRANSACTION == ODBC32.SQL_TRANSACTION.SERIALIZABLE)
						{
							this._isolevel = IsolationLevel.Serializable;
							goto IL_0094;
						}
						if (sql_TRANSACTION == ODBC32.SQL_TRANSACTION.SNAPSHOT)
						{
							this._isolevel = IsolationLevel.Snapshot;
							goto IL_0094;
						}
						break;
					}
					throw ODBC.NoMappingForSqlTransactionLevel(connectAttr);
				}
				IL_0094:
				return this._isolevel;
			}
		}

		// Token: 0x06001CB5 RID: 7349 RVA: 0x0024DCB0 File Offset: 0x0024D0B0
		public override void Commit()
		{
			OdbcConnection.ExecutePermission.Demand();
			OdbcConnection connection = this._connection;
			if (connection == null)
			{
				throw ADP.TransactionZombied(this);
			}
			connection.CheckState("CommitTransaction");
			if (this._handle == null)
			{
				throw ODBC.NotInTransaction();
			}
			ODBC32.RetCode retCode = this._handle.CompleteTransaction(0);
			if (retCode == ODBC32.RetCode.ERROR)
			{
				connection.HandleError(this._handle, retCode);
			}
			connection.LocalTransaction = null;
			this._connection = null;
			this._handle = null;
		}

		// Token: 0x06001CB6 RID: 7350 RVA: 0x0024DD24 File Offset: 0x0024D124
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				OdbcConnectionHandle handle = this._handle;
				this._handle = null;
				if (handle != null)
				{
					try
					{
						ODBC32.RetCode retCode = handle.CompleteTransaction(1);
						if (retCode == ODBC32.RetCode.ERROR && this._connection != null)
						{
							Exception ex = this._connection.HandleErrorNoThrow(handle, retCode);
							ADP.TraceExceptionWithoutRethrow(ex);
						}
					}
					catch (Exception ex2)
					{
						if (!ADP.IsCatchableExceptionType(ex2))
						{
							throw;
						}
					}
				}
				if (this._connection != null && this._connection.IsOpen)
				{
					this._connection.LocalTransaction = null;
				}
				this._connection = null;
				this._isolevel = IsolationLevel.Unspecified;
			}
			base.Dispose(disposing);
		}

		// Token: 0x06001CB7 RID: 7351 RVA: 0x0024DDD4 File Offset: 0x0024D1D4
		public override void Rollback()
		{
			OdbcConnection connection = this._connection;
			if (connection == null)
			{
				throw ADP.TransactionZombied(this);
			}
			connection.CheckState("RollbackTransaction");
			if (this._handle == null)
			{
				throw ODBC.NotInTransaction();
			}
			ODBC32.RetCode retCode = this._handle.CompleteTransaction(1);
			if (retCode == ODBC32.RetCode.ERROR)
			{
				connection.HandleError(this._handle, retCode);
			}
			connection.LocalTransaction = null;
			this._connection = null;
			this._handle = null;
		}

		// Token: 0x04001069 RID: 4201
		private OdbcConnection _connection;

		// Token: 0x0400106A RID: 4202
		private IsolationLevel _isolevel = IsolationLevel.Unspecified;

		// Token: 0x0400106B RID: 4203
		private OdbcConnectionHandle _handle;
	}
}
