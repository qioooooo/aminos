using System;
using System.Data.Common;
using System.Data.ProviderBase;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Threading;
using System.Transactions;

namespace System.Data.SqlClient
{
	// Token: 0x020002F7 RID: 759
	internal abstract class SqlInternalConnection : DbConnectionInternal
	{
		// Token: 0x06002754 RID: 10068 RVA: 0x00289F58 File Offset: 0x00289358
		internal SqlInternalConnection(SqlConnectionString connectionOptions)
		{
			this._connectionOptions = connectionOptions;
		}

		// Token: 0x17000647 RID: 1607
		// (get) Token: 0x06002755 RID: 10069 RVA: 0x00289F74 File Offset: 0x00289374
		internal SqlConnection Connection
		{
			get
			{
				return (SqlConnection)base.Owner;
			}
		}

		// Token: 0x17000648 RID: 1608
		// (get) Token: 0x06002756 RID: 10070 RVA: 0x00289F8C File Offset: 0x0028938C
		internal SqlConnectionString ConnectionOptions
		{
			get
			{
				return this._connectionOptions;
			}
		}

		// Token: 0x17000649 RID: 1609
		// (get) Token: 0x06002757 RID: 10071 RVA: 0x00289FA0 File Offset: 0x002893A0
		// (set) Token: 0x06002758 RID: 10072 RVA: 0x00289FB4 File Offset: 0x002893B4
		internal string CurrentDatabase
		{
			get
			{
				return this._currentDatabase;
			}
			set
			{
				this._currentDatabase = value;
			}
		}

		// Token: 0x1700064A RID: 1610
		// (get) Token: 0x06002759 RID: 10073 RVA: 0x00289FC8 File Offset: 0x002893C8
		// (set) Token: 0x0600275A RID: 10074 RVA: 0x00289FDC File Offset: 0x002893DC
		internal string CurrentDataSource
		{
			get
			{
				return this._currentDataSource;
			}
			set
			{
				this._currentDataSource = value;
			}
		}

		// Token: 0x1700064B RID: 1611
		// (get) Token: 0x0600275B RID: 10075 RVA: 0x00289FF0 File Offset: 0x002893F0
		// (set) Token: 0x0600275C RID: 10076 RVA: 0x0028A004 File Offset: 0x00289404
		internal SqlDelegatedTransaction DelegatedTransaction
		{
			get
			{
				return this._delegatedTransaction;
			}
			set
			{
				this._delegatedTransaction = value;
			}
		}

		// Token: 0x1700064C RID: 1612
		// (get) Token: 0x0600275D RID: 10077
		internal abstract SqlInternalTransaction CurrentTransaction { get; }

		// Token: 0x1700064D RID: 1613
		// (get) Token: 0x0600275E RID: 10078 RVA: 0x0028A018 File Offset: 0x00289418
		internal virtual SqlInternalTransaction AvailableInternalTransaction
		{
			get
			{
				return this.CurrentTransaction;
			}
		}

		// Token: 0x1700064E RID: 1614
		// (get) Token: 0x0600275F RID: 10079
		internal abstract SqlInternalTransaction PendingTransaction { get; }

		// Token: 0x1700064F RID: 1615
		// (get) Token: 0x06002760 RID: 10080 RVA: 0x0028A02C File Offset: 0x0028942C
		internal override bool RequireExplicitTransactionUnbind
		{
			get
			{
				return this._connectionOptions.TransactionBinding == SqlConnectionString.TransactionBindingEnum.ExplicitUnbind;
			}
		}

		// Token: 0x17000650 RID: 1616
		// (get) Token: 0x06002761 RID: 10081 RVA: 0x0028A048 File Offset: 0x00289448
		protected internal override bool IsNonPoolableTransactionRoot
		{
			get
			{
				return this.IsTransactionRoot;
			}
		}

		// Token: 0x17000651 RID: 1617
		// (get) Token: 0x06002762 RID: 10082 RVA: 0x0028A05C File Offset: 0x0028945C
		internal override bool IsTransactionRoot
		{
			get
			{
				if (this._delegatedTransaction == null)
				{
					return false;
				}
				bool flag;
				lock (this)
				{
					flag = this._delegatedTransaction != null && this._delegatedTransaction.IsActive;
				}
				return flag;
			}
		}

		// Token: 0x17000652 RID: 1618
		// (get) Token: 0x06002763 RID: 10083 RVA: 0x0028A0B8 File Offset: 0x002894B8
		internal bool HasLocalTransaction
		{
			get
			{
				SqlInternalTransaction currentTransaction = this.CurrentTransaction;
				return currentTransaction != null && currentTransaction.IsLocal;
			}
		}

		// Token: 0x17000653 RID: 1619
		// (get) Token: 0x06002764 RID: 10084 RVA: 0x0028A0DC File Offset: 0x002894DC
		internal bool HasLocalTransactionFromAPI
		{
			get
			{
				SqlInternalTransaction currentTransaction = this.CurrentTransaction;
				return currentTransaction != null && currentTransaction.HasParentTransaction;
			}
		}

		// Token: 0x17000654 RID: 1620
		// (get) Token: 0x06002765 RID: 10085 RVA: 0x0028A100 File Offset: 0x00289500
		internal bool IsEnlistedInTransaction
		{
			get
			{
				return this._isEnlistedInTransaction;
			}
		}

		// Token: 0x17000655 RID: 1621
		// (get) Token: 0x06002766 RID: 10086 RVA: 0x0028A114 File Offset: 0x00289514
		// (set) Token: 0x06002767 RID: 10087 RVA: 0x0028A128 File Offset: 0x00289528
		protected internal Transaction ContextTransaction
		{
			get
			{
				return this._contextTransaction;
			}
			set
			{
				this._contextTransaction = value;
			}
		}

		// Token: 0x17000656 RID: 1622
		// (get) Token: 0x06002768 RID: 10088 RVA: 0x0028A13C File Offset: 0x0028953C
		internal Transaction InternalEnlistedTransaction
		{
			get
			{
				Transaction transaction = base.EnlistedTransaction;
				if (null == transaction)
				{
					transaction = this.ContextTransaction;
				}
				return transaction;
			}
		}

		// Token: 0x17000657 RID: 1623
		// (get) Token: 0x06002769 RID: 10089
		internal abstract bool IsLockedForBulkCopy { get; }

		// Token: 0x17000658 RID: 1624
		// (get) Token: 0x0600276A RID: 10090
		internal abstract bool IsShiloh { get; }

		// Token: 0x17000659 RID: 1625
		// (get) Token: 0x0600276B RID: 10091
		internal abstract bool IsYukonOrNewer { get; }

		// Token: 0x1700065A RID: 1626
		// (get) Token: 0x0600276C RID: 10092
		internal abstract bool IsKatmaiOrNewer { get; }

		// Token: 0x1700065B RID: 1627
		// (get) Token: 0x0600276D RID: 10093 RVA: 0x0028A164 File Offset: 0x00289564
		// (set) Token: 0x0600276E RID: 10094 RVA: 0x0028A178 File Offset: 0x00289578
		internal byte[] PromotedDTCToken
		{
			get
			{
				return this._promotedDTCToken;
			}
			set
			{
				this._promotedDTCToken = value;
			}
		}

		// Token: 0x0600276F RID: 10095
		internal abstract void AddPreparedCommand(SqlCommand cmd);

		// Token: 0x06002770 RID: 10096 RVA: 0x0028A18C File Offset: 0x0028958C
		public override DbTransaction BeginTransaction(IsolationLevel iso)
		{
			return this.BeginSqlTransaction(iso, null);
		}

		// Token: 0x06002771 RID: 10097 RVA: 0x0028A1A4 File Offset: 0x002895A4
		internal virtual SqlTransaction BeginSqlTransaction(IsolationLevel iso, string transactionName)
		{
			SqlStatistics sqlStatistics = null;
			SNIHandle snihandle = null;
			RuntimeHelpers.PrepareConstrainedRegions();
			SqlTransaction sqlTransaction2;
			try
			{
				snihandle = SqlInternalConnection.GetBestEffortCleanupTarget(this.Connection);
				sqlStatistics = SqlStatistics.StartTimer(this.Connection.Statistics);
				SqlConnection.ExecutePermission.Demand();
				this.ValidateConnectionForExecute(null);
				if (this.HasLocalTransactionFromAPI)
				{
					throw ADP.ParallelTransactionsNotSupported(this.Connection);
				}
				if (iso == IsolationLevel.Unspecified)
				{
					iso = IsolationLevel.ReadCommitted;
				}
				SqlTransaction sqlTransaction = new SqlTransaction(this, this.Connection, iso, this.AvailableInternalTransaction);
				this.ExecuteTransaction(SqlInternalConnection.TransactionRequest.Begin, transactionName, iso, sqlTransaction.InternalTransaction, false);
				sqlTransaction2 = sqlTransaction;
			}
			catch (OutOfMemoryException ex)
			{
				this.Connection.Abort(ex);
				throw;
			}
			catch (StackOverflowException ex2)
			{
				this.Connection.Abort(ex2);
				throw;
			}
			catch (ThreadAbortException ex3)
			{
				this.Connection.Abort(ex3);
				SqlInternalConnection.BestEffortCleanup(snihandle);
				throw;
			}
			finally
			{
				SqlStatistics.StopTimer(sqlStatistics);
			}
			return sqlTransaction2;
		}

		// Token: 0x06002772 RID: 10098 RVA: 0x0028A2DC File Offset: 0x002896DC
		public override void ChangeDatabase(string database)
		{
			SqlConnection.ExecutePermission.Demand();
			if (ADP.IsEmpty(database))
			{
				throw ADP.EmptyDatabaseName();
			}
			this.ValidateConnectionForExecute(null);
			this.ChangeDatabaseInternal(database);
		}

		// Token: 0x06002773 RID: 10099
		protected abstract void ChangeDatabaseInternal(string database);

		// Token: 0x06002774 RID: 10100
		internal abstract void ClearPreparedCommands();

		// Token: 0x06002775 RID: 10101 RVA: 0x0028A310 File Offset: 0x00289710
		internal override void CloseConnection(DbConnection owningObject, DbConnectionFactory connectionFactory)
		{
			if (!base.IsConnectionDoomed)
			{
				this.ClearPreparedCommands();
			}
			base.CloseConnection(owningObject, connectionFactory);
		}

		// Token: 0x06002776 RID: 10102 RVA: 0x0028A334 File Offset: 0x00289734
		protected override void CleanupTransactionOnCompletion(Transaction transaction)
		{
			SqlDelegatedTransaction delegatedTransaction = this.DelegatedTransaction;
			if (delegatedTransaction != null)
			{
				delegatedTransaction.TransactionEnded(transaction);
			}
		}

		// Token: 0x06002777 RID: 10103 RVA: 0x0028A354 File Offset: 0x00289754
		protected override DbReferenceCollection CreateReferenceCollection()
		{
			return new SqlReferenceCollection();
		}

		// Token: 0x06002778 RID: 10104 RVA: 0x0028A368 File Offset: 0x00289768
		protected override void Deactivate()
		{
			if (Bid.AdvancedOn)
			{
				Bid.Trace("<sc.SqlInternalConnection.Deactivate|ADV> %d# deactivating\n", base.ObjectID);
			}
			SNIHandle snihandle = null;
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
				snihandle = SqlInternalConnection.GetBestEffortCleanupTarget(this.Connection);
				SqlReferenceCollection sqlReferenceCollection = (SqlReferenceCollection)base.ReferenceCollection;
				if (sqlReferenceCollection != null)
				{
					sqlReferenceCollection.Deactivate();
				}
				this.InternalDeactivate();
			}
			catch (OutOfMemoryException)
			{
				base.DoomThisConnection();
				throw;
			}
			catch (StackOverflowException)
			{
				base.DoomThisConnection();
				throw;
			}
			catch (ThreadAbortException)
			{
				base.DoomThisConnection();
				SqlInternalConnection.BestEffortCleanup(snihandle);
				throw;
			}
			catch (Exception ex)
			{
				if (!ADP.IsCatchableExceptionType(ex))
				{
					throw;
				}
				base.DoomThisConnection();
				ADP.TraceExceptionWithoutRethrow(ex);
			}
		}

		// Token: 0x06002779 RID: 10105
		internal abstract void DisconnectTransaction(SqlInternalTransaction internalTransaction);

		// Token: 0x0600277A RID: 10106 RVA: 0x0028A460 File Offset: 0x00289860
		public override void Dispose()
		{
			this._whereAbouts = null;
			base.Dispose();
		}

		// Token: 0x0600277B RID: 10107 RVA: 0x0028A47C File Offset: 0x0028987C
		protected void Enlist(Transaction tx)
		{
			if (null == tx)
			{
				if (this.IsEnlistedInTransaction)
				{
					this.EnlistNull();
					return;
				}
			}
			else if (!tx.Equals(base.EnlistedTransaction))
			{
				this.EnlistNonNull(tx);
			}
		}

		// Token: 0x0600277C RID: 10108 RVA: 0x0028A4B8 File Offset: 0x002898B8
		private void EnlistNonNull(Transaction tx)
		{
			if (Bid.AdvancedOn)
			{
				Bid.Trace("<sc.SqlInternalConnection.EnlistNonNull|ADV> %d#, transaction %d#.\n", base.ObjectID, tx.GetHashCode());
			}
			bool flag = false;
			if (this.IsYukonOrNewer)
			{
				if (Bid.AdvancedOn)
				{
					Bid.Trace("<sc.SqlInternalConnection.EnlistNonNull|ADV> %d#, attempting to delegate\n", base.ObjectID);
				}
				SqlDelegatedTransaction sqlDelegatedTransaction = new SqlDelegatedTransaction(this, tx);
				try
				{
					if (tx.EnlistPromotableSinglePhase(sqlDelegatedTransaction))
					{
						flag = true;
						this._delegatedTransaction = sqlDelegatedTransaction;
						if (Bid.AdvancedOn)
						{
							long num = 0L;
							int num2 = 0;
							if (this.CurrentTransaction != null)
							{
								num = this.CurrentTransaction.TransactionId;
								num2 = this.CurrentTransaction.ObjectID;
							}
							Bid.Trace("<sc.SqlInternalConnection.EnlistNonNull|ADV> %d#, delegated to transaction %d# with transactionId=0x%I64x\n", base.ObjectID, num2, num);
						}
					}
				}
				catch (SqlException ex)
				{
					if (ex.Class >= 20)
					{
						throw;
					}
					SqlInternalConnectionTds sqlInternalConnectionTds = this as SqlInternalConnectionTds;
					if (sqlInternalConnectionTds != null && sqlInternalConnectionTds.Parser.State != TdsParserState.OpenLoggedIn)
					{
						throw;
					}
					ADP.TraceExceptionWithoutRethrow(ex);
				}
			}
			if (!flag)
			{
				if (Bid.AdvancedOn)
				{
					Bid.Trace("<sc.SqlInternalConnection.EnlistNonNull|ADV> %d#, delegation not possible, enlisting.\n", base.ObjectID);
				}
				if (this._whereAbouts == null)
				{
					byte[] dtcaddress = this.GetDTCAddress();
					if (dtcaddress == null)
					{
						throw SQL.CannotGetDTCAddress();
					}
					this._whereAbouts = dtcaddress;
				}
				byte[] transactionCookie = SqlInternalConnection.GetTransactionCookie(tx, this._whereAbouts);
				this.PropagateTransactionCookie(transactionCookie);
				this._isEnlistedInTransaction = true;
				if (Bid.AdvancedOn)
				{
					long num3 = 0L;
					int num4 = 0;
					if (this.CurrentTransaction != null)
					{
						num3 = this.CurrentTransaction.TransactionId;
						num4 = this.CurrentTransaction.ObjectID;
					}
					Bid.Trace("<sc.SqlInternalConnection.EnlistNonNull|ADV> %d#, enlisted with transaction %d# with transactionId=0x%I64x\n", base.ObjectID, num4, num3);
				}
			}
			base.EnlistedTransaction = tx;
		}

		// Token: 0x0600277D RID: 10109 RVA: 0x0028A65C File Offset: 0x00289A5C
		internal void EnlistNull()
		{
			if (Bid.AdvancedOn)
			{
				Bid.Trace("<sc.SqlInternalConnection.EnlistNull|ADV> %d#, unenlisting.\n", base.ObjectID);
			}
			this.PropagateTransactionCookie(null);
			this._isEnlistedInTransaction = false;
			base.EnlistedTransaction = null;
			if (Bid.AdvancedOn)
			{
				Bid.Trace("<sc.SqlInternalConnection.EnlistNull|ADV> %d#, unenlisted.\n", base.ObjectID);
			}
		}

		// Token: 0x0600277E RID: 10110 RVA: 0x0028A6AC File Offset: 0x00289AAC
		public override void EnlistTransaction(Transaction transaction)
		{
			this.ValidateConnectionForExecute(null);
			if (this.HasLocalTransaction)
			{
				throw ADP.LocalTransactionPresent();
			}
			if (null != transaction && transaction.Equals(base.EnlistedTransaction))
			{
				return;
			}
			SNIHandle snihandle = null;
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
				snihandle = SqlInternalConnection.GetBestEffortCleanupTarget(this.Connection);
				this.Enlist(transaction);
			}
			catch (OutOfMemoryException ex)
			{
				this.Connection.Abort(ex);
				throw;
			}
			catch (StackOverflowException ex2)
			{
				this.Connection.Abort(ex2);
				throw;
			}
			catch (ThreadAbortException ex3)
			{
				this.Connection.Abort(ex3);
				SqlInternalConnection.BestEffortCleanup(snihandle);
				throw;
			}
		}

		// Token: 0x0600277F RID: 10111
		internal abstract void ExecuteTransaction(SqlInternalConnection.TransactionRequest transactionRequest, string name, IsolationLevel iso, SqlInternalTransaction internalTransaction, bool isDelegateControlRequest);

		// Token: 0x06002780 RID: 10112 RVA: 0x0028A788 File Offset: 0x00289B88
		internal SqlDataReader FindLiveReader(SqlCommand command)
		{
			SqlDataReader sqlDataReader = null;
			SqlReferenceCollection sqlReferenceCollection = (SqlReferenceCollection)base.ReferenceCollection;
			if (sqlReferenceCollection != null)
			{
				sqlDataReader = sqlReferenceCollection.FindLiveReader(command);
			}
			return sqlDataReader;
		}

		// Token: 0x06002781 RID: 10113 RVA: 0x0028A7B0 File Offset: 0x00289BB0
		internal static SNIHandle GetBestEffortCleanupTarget(SqlConnection connection)
		{
			if (connection != null)
			{
				SqlInternalConnectionTds sqlInternalConnectionTds = connection.InnerConnection as SqlInternalConnectionTds;
				if (sqlInternalConnectionTds != null)
				{
					TdsParser parser = sqlInternalConnectionTds.Parser;
					if (parser != null)
					{
						return parser.GetBestEffortCleanupTarget();
					}
				}
			}
			return null;
		}

		// Token: 0x06002782 RID: 10114 RVA: 0x0028A7E4 File Offset: 0x00289BE4
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		internal static void BestEffortCleanup(SNIHandle target)
		{
			if (target != null)
			{
				target.Dispose();
			}
		}

		// Token: 0x06002783 RID: 10115
		protected abstract byte[] GetDTCAddress();

		// Token: 0x06002784 RID: 10116 RVA: 0x0028A7FC File Offset: 0x00289BFC
		private static byte[] GetTransactionCookie(Transaction transaction, byte[] whereAbouts)
		{
			byte[] array = null;
			if (null != transaction)
			{
				array = TransactionInterop.GetExportCookie(transaction, whereAbouts);
			}
			return array;
		}

		// Token: 0x06002785 RID: 10117 RVA: 0x0028A820 File Offset: 0x00289C20
		protected virtual void InternalDeactivate()
		{
		}

		// Token: 0x06002786 RID: 10118 RVA: 0x0028A830 File Offset: 0x00289C30
		internal void OnError(SqlException exception, bool breakConnection)
		{
			if (breakConnection)
			{
				base.DoomThisConnection();
			}
			if (this.Connection != null)
			{
				this.Connection.OnError(exception, breakConnection);
				return;
			}
			if (exception.Class >= 11)
			{
				throw exception;
			}
		}

		// Token: 0x06002787 RID: 10119
		protected abstract void PropagateTransactionCookie(byte[] transactionCookie);

		// Token: 0x06002788 RID: 10120
		internal abstract void RemovePreparedCommand(SqlCommand cmd);

		// Token: 0x06002789 RID: 10121
		internal abstract void ValidateConnectionForExecute(SqlCommand command);

		// Token: 0x0600278A RID: 10122 RVA: 0x0028A868 File Offset: 0x00289C68
		internal void ValidateTransaction()
		{
			if (this.RequireExplicitTransactionUnbind && null != this.InternalEnlistedTransaction)
			{
				Transaction transaction = Transaction.Current;
				if (this.InternalEnlistedTransaction.TransactionInformation.Status != TransactionStatus.Active || null == transaction || !this.InternalEnlistedTransaction.Equals(transaction))
				{
					throw ADP.TransactionConnectionMismatch();
				}
			}
		}

		// Token: 0x040018E5 RID: 6373
		private readonly SqlConnectionString _connectionOptions;

		// Token: 0x040018E6 RID: 6374
		private string _currentDatabase;

		// Token: 0x040018E7 RID: 6375
		private string _currentDataSource;

		// Token: 0x040018E8 RID: 6376
		private bool _isEnlistedInTransaction;

		// Token: 0x040018E9 RID: 6377
		private byte[] _promotedDTCToken;

		// Token: 0x040018EA RID: 6378
		private SqlDelegatedTransaction _delegatedTransaction;

		// Token: 0x040018EB RID: 6379
		private byte[] _whereAbouts;

		// Token: 0x040018EC RID: 6380
		private Transaction _contextTransaction;

		// Token: 0x020002F8 RID: 760
		internal enum TransactionRequest
		{
			// Token: 0x040018EE RID: 6382
			Begin,
			// Token: 0x040018EF RID: 6383
			Promote,
			// Token: 0x040018F0 RID: 6384
			Commit,
			// Token: 0x040018F1 RID: 6385
			Rollback,
			// Token: 0x040018F2 RID: 6386
			IfRollback,
			// Token: 0x040018F3 RID: 6387
			Save
		}
	}
}
