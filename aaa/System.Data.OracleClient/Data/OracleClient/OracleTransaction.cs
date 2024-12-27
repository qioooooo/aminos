using System;
using System.Data.Common;
using System.Threading;

namespace System.Data.OracleClient
{
	// Token: 0x0200007E RID: 126
	public sealed class OracleTransaction : DbTransaction
	{
		// Token: 0x060006DF RID: 1759 RVA: 0x0007002C File Offset: 0x0006F42C
		internal OracleTransaction(OracleConnection connection)
			: this(connection, IsolationLevel.Unspecified)
		{
		}

		// Token: 0x060006E0 RID: 1760 RVA: 0x00070044 File Offset: 0x0006F444
		internal OracleTransaction(OracleConnection connection, IsolationLevel isolationLevel)
		{
			TransactionState transactionState = connection.TransactionState;
			if (TransactionState.GlobalStarted == transactionState)
			{
				throw ADP.NoLocalTransactionInDistributedContext();
			}
			this._connection = connection;
			this._connectionCloseCount = connection.CloseCount;
			this._isolationLevel = isolationLevel;
			this._connection.TransactionState = TransactionState.LocalStarted;
			try
			{
				if (isolationLevel != IsolationLevel.Unspecified)
				{
					if (isolationLevel != IsolationLevel.ReadCommitted)
					{
						if (isolationLevel != IsolationLevel.Serializable)
						{
							goto IL_00C4;
						}
					}
					else
					{
						using (OracleCommand oracleCommand = this.Connection.CreateCommand())
						{
							oracleCommand.CommandText = "set transaction isolation level read committed";
							oracleCommand.ExecuteNonQuery();
							goto IL_00CA;
						}
					}
					using (OracleCommand oracleCommand2 = this.Connection.CreateCommand())
					{
						oracleCommand2.CommandText = "set transaction isolation level serializable";
						oracleCommand2.ExecuteNonQuery();
						goto IL_00CA;
					}
					IL_00C4:
					throw ADP.UnsupportedIsolationLevel();
				}
				IL_00CA:;
			}
			catch
			{
				this._connection.TransactionState = transactionState;
				throw;
			}
		}

		// Token: 0x17000149 RID: 329
		// (get) Token: 0x060006E1 RID: 1761 RVA: 0x0007017C File Offset: 0x0006F57C
		public new OracleConnection Connection
		{
			get
			{
				return this._connection;
			}
		}

		// Token: 0x1700014A RID: 330
		// (get) Token: 0x060006E2 RID: 1762 RVA: 0x00070190 File Offset: 0x0006F590
		protected override DbConnection DbConnection
		{
			get
			{
				return this.Connection;
			}
		}

		// Token: 0x1700014B RID: 331
		// (get) Token: 0x060006E3 RID: 1763 RVA: 0x000701A4 File Offset: 0x0006F5A4
		public override IsolationLevel IsolationLevel
		{
			get
			{
				this.AssertNotCompleted();
				if (IsolationLevel.Unspecified == this._isolationLevel)
				{
					using (OracleCommand oracleCommand = this.Connection.CreateCommand())
					{
						oracleCommand.Transaction = this;
						oracleCommand.CommandText = "select decode(value,'FALSE',0,1) from V$SYSTEM_PARAMETER where name = 'serializable'";
						decimal num = (decimal)oracleCommand.ExecuteScalar();
						if (0m == num)
						{
							this._isolationLevel = IsolationLevel.ReadCommitted;
						}
						else
						{
							this._isolationLevel = IsolationLevel.Serializable;
						}
					}
				}
				return this._isolationLevel;
			}
		}

		// Token: 0x1700014C RID: 332
		// (get) Token: 0x060006E4 RID: 1764 RVA: 0x00070240 File Offset: 0x0006F640
		internal int ObjectID
		{
			get
			{
				return this._objectID;
			}
		}

		// Token: 0x060006E5 RID: 1765 RVA: 0x00070254 File Offset: 0x0006F654
		private void AssertNotCompleted()
		{
			if (this.Connection == null || this._connectionCloseCount != this.Connection.CloseCount)
			{
				throw ADP.TransactionCompleted();
			}
		}

		// Token: 0x060006E6 RID: 1766 RVA: 0x00070284 File Offset: 0x0006F684
		public override void Commit()
		{
			OracleConnection.ExecutePermission.Demand();
			IntPtr intPtr;
			Bid.ScopeEnter(out intPtr, "<ora.OracleTransaction.Commit|API> %d#\n", this.ObjectID);
			try
			{
				this.AssertNotCompleted();
				this.Connection.Commit();
				this.Dispose(true);
			}
			finally
			{
				Bid.ScopeLeave(ref intPtr);
			}
		}

		// Token: 0x060006E7 RID: 1767 RVA: 0x000702EC File Offset: 0x0006F6EC
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (this.Connection != null)
				{
					this.Connection.Rollback();
				}
				this._connection = null;
			}
			base.Dispose(disposing);
		}

		// Token: 0x060006E8 RID: 1768 RVA: 0x00070320 File Offset: 0x0006F720
		public override void Rollback()
		{
			IntPtr intPtr;
			Bid.ScopeEnter(out intPtr, "<ora.OracleTransaction.Rollback|API> %d#\n", this.ObjectID);
			try
			{
				this.AssertNotCompleted();
				this.Dispose(true);
			}
			finally
			{
				Bid.ScopeLeave(ref intPtr);
			}
		}

		// Token: 0x040004CB RID: 1227
		private OracleConnection _connection;

		// Token: 0x040004CC RID: 1228
		private int _connectionCloseCount;

		// Token: 0x040004CD RID: 1229
		private IsolationLevel _isolationLevel = IsolationLevel.ReadCommitted;

		// Token: 0x040004CE RID: 1230
		private static int _objectTypeCount;

		// Token: 0x040004CF RID: 1231
		internal readonly int _objectID = Interlocked.Increment(ref OracleTransaction._objectTypeCount);
	}
}
