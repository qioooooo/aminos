using System;
using System.Data.Common;
using System.Threading;
using System.Transactions;
using Microsoft.SqlServer.Server;

namespace System.Data.SqlClient
{
	// Token: 0x020002F9 RID: 761
	internal sealed class SqlInternalConnectionSmi : SqlInternalConnection
	{
		// Token: 0x0600278B RID: 10123 RVA: 0x0028A8C0 File Offset: 0x00289CC0
		internal SqlInternalConnectionSmi(SqlConnectionString connectionOptions, SmiContext smiContext)
			: base(connectionOptions)
		{
			this._smiContext = smiContext;
			this._smiContext.OutOfScope += this.OnOutOfScope;
			this._smiConnection = this._smiContext.ContextConnection;
			this._smiEventSink = new SqlInternalConnectionSmi.EventSink(this);
			if (Bid.AdvancedOn)
			{
				Bid.Trace("<sc.SqlInternalConnectionSmi.ctor|ADV> %d#, constructed new SMI internal connection\n", base.ObjectID);
			}
		}

		// Token: 0x1700065C RID: 1628
		// (get) Token: 0x0600278C RID: 10124 RVA: 0x0028A928 File Offset: 0x00289D28
		internal SmiContext InternalContext
		{
			get
			{
				return this._smiContext;
			}
		}

		// Token: 0x1700065D RID: 1629
		// (get) Token: 0x0600278D RID: 10125 RVA: 0x0028A93C File Offset: 0x00289D3C
		internal SmiConnection SmiConnection
		{
			get
			{
				return this._smiConnection;
			}
		}

		// Token: 0x1700065E RID: 1630
		// (get) Token: 0x0600278E RID: 10126 RVA: 0x0028A950 File Offset: 0x00289D50
		internal SmiEventSink CurrentEventSink
		{
			get
			{
				return this._smiEventSink;
			}
		}

		// Token: 0x1700065F RID: 1631
		// (get) Token: 0x0600278F RID: 10127 RVA: 0x0028A964 File Offset: 0x00289D64
		internal override SqlInternalTransaction CurrentTransaction
		{
			get
			{
				return this._currentTransaction;
			}
		}

		// Token: 0x17000660 RID: 1632
		// (get) Token: 0x06002790 RID: 10128 RVA: 0x0028A978 File Offset: 0x00289D78
		internal override bool IsLockedForBulkCopy
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000661 RID: 1633
		// (get) Token: 0x06002791 RID: 10129 RVA: 0x0028A988 File Offset: 0x00289D88
		internal override bool IsShiloh
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000662 RID: 1634
		// (get) Token: 0x06002792 RID: 10130 RVA: 0x0028A998 File Offset: 0x00289D98
		internal override bool IsYukonOrNewer
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000663 RID: 1635
		// (get) Token: 0x06002793 RID: 10131 RVA: 0x0028A9A8 File Offset: 0x00289DA8
		internal override bool IsKatmaiOrNewer
		{
			get
			{
				return SmiContextFactory.Instance.NegotiatedSmiVersion >= 210UL;
			}
		}

		// Token: 0x17000664 RID: 1636
		// (get) Token: 0x06002794 RID: 10132 RVA: 0x0028A9CC File Offset: 0x00289DCC
		internal override SqlInternalTransaction PendingTransaction
		{
			get
			{
				return this.CurrentTransaction;
			}
		}

		// Token: 0x17000665 RID: 1637
		// (get) Token: 0x06002795 RID: 10133 RVA: 0x0028A9E0 File Offset: 0x00289DE0
		public override string ServerVersion
		{
			get
			{
				return SmiContextFactory.Instance.ServerVersion;
			}
		}

		// Token: 0x06002796 RID: 10134 RVA: 0x0028A9F8 File Offset: 0x00289DF8
		protected override void Activate(Transaction transaction)
		{
		}

		// Token: 0x06002797 RID: 10135 RVA: 0x0028AA08 File Offset: 0x00289E08
		internal void Activate()
		{
			int num = Interlocked.Exchange(ref this._isInUse, 1);
			if (num != 0)
			{
				throw SQL.ContextConnectionIsInUse();
			}
			base.CurrentDatabase = this._smiConnection.GetCurrentDatabase(this._smiEventSink);
			this._smiEventSink.ProcessMessagesAndThrow();
		}

		// Token: 0x06002798 RID: 10136 RVA: 0x0028AA50 File Offset: 0x00289E50
		internal override void AddPreparedCommand(SqlCommand cmd)
		{
		}

		// Token: 0x06002799 RID: 10137 RVA: 0x0028AA60 File Offset: 0x00289E60
		internal void AutomaticEnlistment()
		{
			Transaction currentTransaction = ADP.GetCurrentTransaction();
			Transaction contextTransaction = this._smiContext.ContextTransaction;
			long contextTransactionId = this._smiContext.ContextTransactionId;
			if (Bid.AdvancedOn)
			{
				Bid.Trace("<sc.SqlInternalConnectionSmi.AutomaticEnlistment|ADV> %d#, contextTransactionId=0x%I64x, contextTransaction=%d#, currentSystemTransaction=%d#.\n", base.ObjectID, contextTransactionId, (null != contextTransaction) ? contextTransaction.GetHashCode() : 0, (null != currentTransaction) ? currentTransaction.GetHashCode() : 0);
			}
			if (0L == contextTransactionId)
			{
				if (null == currentTransaction)
				{
					this._currentTransaction = null;
					if (Bid.AdvancedOn)
					{
						Bid.Trace("<sc.SqlInternalConnectionSmi.AutomaticEnlistment|ADV> %d#, no transaction.\n", base.ObjectID);
						return;
					}
				}
				else
				{
					if (Bid.AdvancedOn)
					{
						Bid.Trace("<sc.SqlInternalConnectionSmi.AutomaticEnlistment|ADV> %d#, using current System.Transaction.\n", base.ObjectID);
					}
					base.Enlist(currentTransaction);
				}
				return;
			}
			if (null != currentTransaction && contextTransaction != currentTransaction)
			{
				throw SQL.NestedTransactionScopesNotSupported();
			}
			if (Bid.AdvancedOn)
			{
				Bid.Trace("<sc.SqlInternalConnectionSmi.AutomaticEnlistment|ADV> %d#, using context transaction with transactionId=0x%I64x\n", base.ObjectID, contextTransactionId);
			}
			this._currentTransaction = new SqlInternalTransaction(this, TransactionType.Context, null, contextTransactionId);
			base.ContextTransaction = contextTransaction;
		}

		// Token: 0x0600279A RID: 10138 RVA: 0x0028AB5C File Offset: 0x00289F5C
		internal override void ClearPreparedCommands()
		{
		}

		// Token: 0x0600279B RID: 10139 RVA: 0x0028AB6C File Offset: 0x00289F6C
		protected override void ChangeDatabaseInternal(string database)
		{
			this._smiConnection.SetCurrentDatabase(database, this._smiEventSink);
			this._smiEventSink.ProcessMessagesAndThrow();
		}

		// Token: 0x0600279C RID: 10140 RVA: 0x0028AB98 File Offset: 0x00289F98
		protected override void InternalDeactivate()
		{
			if (Bid.AdvancedOn)
			{
				Bid.Trace("<sc.SqlInternalConnectionSmi.Deactivate|ADV> %d#, Deactivating.\n", base.ObjectID);
			}
			if (!this.IsNonPoolableTransactionRoot)
			{
				base.Enlist(null);
			}
			if (this._currentTransaction != null)
			{
				if (this._currentTransaction.IsContext)
				{
					this._currentTransaction = null;
				}
				else if (this._currentTransaction.IsLocal)
				{
					this._currentTransaction.CloseFromConnection();
				}
			}
			base.ContextTransaction = null;
			this._isInUse = 0;
		}

		// Token: 0x0600279D RID: 10141 RVA: 0x0028AC10 File Offset: 0x0028A010
		internal override void DelegatedTransactionEnded()
		{
			base.DelegatedTransactionEnded();
			if (Bid.AdvancedOn)
			{
				Bid.Trace("<sc.SqlInternalConnectionSmi.DelegatedTransactionEnded|ADV> %d#, cleaning up after Delegated Transaction Completion\n", base.ObjectID);
			}
			this._currentTransaction = null;
		}

		// Token: 0x0600279E RID: 10142 RVA: 0x0028AC44 File Offset: 0x0028A044
		internal override void DisconnectTransaction(SqlInternalTransaction internalTransaction)
		{
			if (Bid.AdvancedOn)
			{
				Bid.Trace("<sc.SqlInternalConnectionSmi.DisconnectTransaction|ADV> %d#, Disconnecting Transaction %d#.\n", base.ObjectID, internalTransaction.ObjectID);
			}
			if (this._currentTransaction != null && this._currentTransaction == internalTransaction)
			{
				this._currentTransaction = null;
			}
		}

		// Token: 0x0600279F RID: 10143 RVA: 0x0028AC88 File Offset: 0x0028A088
		public override void Dispose()
		{
			this._smiContext.OutOfScope -= this.OnOutOfScope;
			base.Dispose();
		}

		// Token: 0x060027A0 RID: 10144 RVA: 0x0028ACB4 File Offset: 0x0028A0B4
		internal override void ExecuteTransaction(SqlInternalConnection.TransactionRequest transactionRequest, string transactionName, IsolationLevel iso, SqlInternalTransaction internalTransaction, bool isDelegateControlRequest)
		{
			if (Bid.AdvancedOn)
			{
				Bid.Trace("<sc.SqlInternalConnectionSmi.ExecuteTransaction|ADV> %d#, transactionRequest=%s, transactionName='%ls', isolationLevel=%s, internalTransaction=#%d transactionId=0x%I64x.\n", base.ObjectID, transactionRequest.ToString(), (transactionName != null) ? transactionName : "null", iso.ToString(), (internalTransaction != null) ? internalTransaction.ObjectID : 0, (internalTransaction != null) ? internalTransaction.TransactionId : 0L);
			}
			switch (transactionRequest)
			{
			case SqlInternalConnection.TransactionRequest.Begin:
				try
				{
					this._pendingTransaction = internalTransaction;
					this._smiConnection.BeginTransaction(transactionName, iso, this._smiEventSink);
					goto IL_0121;
				}
				finally
				{
					this._pendingTransaction = null;
				}
				break;
			case SqlInternalConnection.TransactionRequest.Promote:
				base.PromotedDTCToken = this._smiConnection.PromoteTransaction(this._currentTransaction.TransactionId, this._smiEventSink);
				goto IL_0121;
			case SqlInternalConnection.TransactionRequest.Commit:
				break;
			case SqlInternalConnection.TransactionRequest.Rollback:
			case SqlInternalConnection.TransactionRequest.IfRollback:
				this._smiConnection.RollbackTransaction(this._currentTransaction.TransactionId, transactionName, this._smiEventSink);
				goto IL_0121;
			case SqlInternalConnection.TransactionRequest.Save:
				this._smiConnection.CreateTransactionSavePoint(this._currentTransaction.TransactionId, transactionName, this._smiEventSink);
				goto IL_0121;
			default:
				goto IL_0121;
			}
			this._smiConnection.CommitTransaction(this._currentTransaction.TransactionId, this._smiEventSink);
			IL_0121:
			this._smiEventSink.ProcessMessagesAndThrow();
		}

		// Token: 0x060027A1 RID: 10145 RVA: 0x0028AE0C File Offset: 0x0028A20C
		protected override byte[] GetDTCAddress()
		{
			byte[] dtcaddress = this._smiConnection.GetDTCAddress(this._smiEventSink);
			this._smiEventSink.ProcessMessagesAndThrow();
			if (Bid.AdvancedOn)
			{
				if (dtcaddress != null)
				{
					Bid.TraceBin("<sc.SqlInternalConnectionSmi.GetDTCAddress|ADV> whereAbouts", dtcaddress, (ushort)dtcaddress.Length);
				}
				else
				{
					Bid.Trace("<sc.SqlInternalConnectionSmi.GetDTCAddress|ADV> whereAbouts=null\n");
				}
			}
			return dtcaddress;
		}

		// Token: 0x060027A2 RID: 10146 RVA: 0x0028AE5C File Offset: 0x0028A25C
		internal void GetCurrentTransactionPair(out long transactionId, out Transaction transaction)
		{
			lock (this)
			{
				transactionId = ((this.CurrentTransaction != null) ? this.CurrentTransaction.TransactionId : 0L);
				transaction = null;
				if (0L != transactionId)
				{
					transaction = base.InternalEnlistedTransaction;
				}
			}
		}

		// Token: 0x060027A3 RID: 10147 RVA: 0x0028AEC0 File Offset: 0x0028A2C0
		private void OnOutOfScope(object s, EventArgs e)
		{
			if (Bid.AdvancedOn)
			{
				Bid.Trace("<sc.SqlInternalConnectionSmi.OutOfScope|ADV> %d# context is out of scope\n", base.ObjectID);
			}
			base.DelegatedTransaction = null;
			DbConnection dbConnection = (DbConnection)base.Owner;
			try
			{
				if (dbConnection != null && 1 == this._isInUse)
				{
					dbConnection.Close();
				}
			}
			finally
			{
				base.ContextTransaction = null;
				this._isInUse = 0;
			}
		}

		// Token: 0x060027A4 RID: 10148 RVA: 0x0028AF38 File Offset: 0x0028A338
		protected override void PropagateTransactionCookie(byte[] transactionCookie)
		{
			if (Bid.AdvancedOn)
			{
				if (transactionCookie != null)
				{
					Bid.TraceBin("<sc.SqlInternalConnectionSmi.PropagateTransactionCookie|ADV> transactionCookie", transactionCookie, (ushort)transactionCookie.Length);
				}
				else
				{
					Bid.Trace("<sc.SqlInternalConnectionSmi.PropagateTransactionCookie|ADV> null\n");
				}
			}
			this._smiConnection.EnlistTransaction(transactionCookie, this._smiEventSink);
			this._smiEventSink.ProcessMessagesAndThrow();
		}

		// Token: 0x060027A5 RID: 10149 RVA: 0x0028AF88 File Offset: 0x0028A388
		private void TransactionEndedByServer(long transactionId)
		{
			SqlDelegatedTransaction delegatedTransaction = base.DelegatedTransaction;
			if (delegatedTransaction != null)
			{
				delegatedTransaction.Transaction.Rollback();
				base.DelegatedTransaction = null;
			}
			this.TransactionEnded(transactionId, TransactionState.Unknown);
		}

		// Token: 0x060027A6 RID: 10150 RVA: 0x0028AFBC File Offset: 0x0028A3BC
		private void TransactionEnded(long transactionId, TransactionState transactionState)
		{
			if (this._currentTransaction != null)
			{
				this._currentTransaction.Completed(transactionState);
				this._currentTransaction = null;
			}
		}

		// Token: 0x060027A7 RID: 10151 RVA: 0x0028AFE4 File Offset: 0x0028A3E4
		private void TransactionStarted(long transactionId, bool isDistributed)
		{
			this._currentTransaction = this._pendingTransaction;
			this._pendingTransaction = null;
			if (this._currentTransaction != null)
			{
				this._currentTransaction.TransactionId = transactionId;
			}
			else
			{
				TransactionType transactionType = (isDistributed ? TransactionType.Distributed : TransactionType.LocalFromTSQL);
				this._currentTransaction = new SqlInternalTransaction(this, transactionType, null, transactionId);
			}
			this._currentTransaction.Activate();
		}

		// Token: 0x060027A8 RID: 10152 RVA: 0x0028B03C File Offset: 0x0028A43C
		internal override void RemovePreparedCommand(SqlCommand cmd)
		{
		}

		// Token: 0x060027A9 RID: 10153 RVA: 0x0028B04C File Offset: 0x0028A44C
		internal override void ValidateConnectionForExecute(SqlCommand command)
		{
			SqlDataReader sqlDataReader = base.FindLiveReader(null);
			if (sqlDataReader != null)
			{
				throw ADP.OpenReaderExists();
			}
		}

		// Token: 0x040018F4 RID: 6388
		private SmiContext _smiContext;

		// Token: 0x040018F5 RID: 6389
		private SmiConnection _smiConnection;

		// Token: 0x040018F6 RID: 6390
		private SmiEventSink_Default _smiEventSink;

		// Token: 0x040018F7 RID: 6391
		private int _isInUse;

		// Token: 0x040018F8 RID: 6392
		private SqlInternalTransaction _pendingTransaction;

		// Token: 0x040018F9 RID: 6393
		private SqlInternalTransaction _currentTransaction;

		// Token: 0x020002FA RID: 762
		private sealed class EventSink : SmiEventSink_Default
		{
			// Token: 0x17000666 RID: 1638
			// (get) Token: 0x060027AA RID: 10154 RVA: 0x0028B06C File Offset: 0x0028A46C
			internal override string ServerVersion
			{
				get
				{
					return SmiContextFactory.Instance.ServerVersion;
				}
			}

			// Token: 0x060027AB RID: 10155 RVA: 0x0028B084 File Offset: 0x0028A484
			protected override void DispatchMessages(bool ignoreNonFatalMessages)
			{
				SqlException ex = base.ProcessMessages(false, ignoreNonFatalMessages);
				if (ex != null)
				{
					if (this._connection.Connection != null && this._connection.Connection.FireInfoMessageEventOnUserErrors)
					{
						this._connection.Connection.OnInfoMessage(new SqlInfoMessageEventArgs(ex));
						return;
					}
					this._connection.OnError(ex, false);
				}
			}

			// Token: 0x060027AC RID: 10156 RVA: 0x0028B0E0 File Offset: 0x0028A4E0
			internal EventSink(SqlInternalConnectionSmi connection)
			{
				this._connection = connection;
			}

			// Token: 0x060027AD RID: 10157 RVA: 0x0028B0FC File Offset: 0x0028A4FC
			internal override void DefaultDatabaseChanged(string databaseName)
			{
				if (Bid.AdvancedOn)
				{
					Bid.Trace("<sc.SqlInternalConnectionSmi.EventSink.DefaultDatabaseChanged|ADV> %d#, databaseName='%ls'.\n", this._connection.ObjectID, databaseName);
				}
				this._connection.CurrentDatabase = databaseName;
			}

			// Token: 0x060027AE RID: 10158 RVA: 0x0028B134 File Offset: 0x0028A534
			internal override void TransactionCommitted(long transactionId)
			{
				if (Bid.AdvancedOn)
				{
					Bid.Trace("<sc.SqlInternalConnectionSmi.EventSink.TransactionCommitted|ADV> %d#, transactionId=0x%I64x.\n", this._connection.ObjectID, transactionId);
				}
				this._connection.TransactionEnded(transactionId, TransactionState.Committed);
			}

			// Token: 0x060027AF RID: 10159 RVA: 0x0028B16C File Offset: 0x0028A56C
			internal override void TransactionDefected(long transactionId)
			{
				if (Bid.AdvancedOn)
				{
					Bid.Trace("<sc.SqlInternalConnectionSmi.EventSink.TransactionDefected|ADV> %d#, transactionId=0x%I64x.\n", this._connection.ObjectID, transactionId);
				}
				this._connection.TransactionEnded(transactionId, TransactionState.Unknown);
			}

			// Token: 0x060027B0 RID: 10160 RVA: 0x0028B1A4 File Offset: 0x0028A5A4
			internal override void TransactionEnlisted(long transactionId)
			{
				if (Bid.AdvancedOn)
				{
					Bid.Trace("<sc.SqlInternalConnectionSmi.EventSink.TransactionEnlisted|ADV> %d#, transactionId=0x%I64x.\n", this._connection.ObjectID, transactionId);
				}
				this._connection.TransactionStarted(transactionId, true);
			}

			// Token: 0x060027B1 RID: 10161 RVA: 0x0028B1DC File Offset: 0x0028A5DC
			internal override void TransactionEnded(long transactionId)
			{
				if (Bid.AdvancedOn)
				{
					Bid.Trace("<sc.SqlInternalConnectionSmi.EventSink.TransactionEnded|ADV> %d#, transactionId=0x%I64x.\n", this._connection.ObjectID, transactionId);
				}
				this._connection.TransactionEndedByServer(transactionId);
			}

			// Token: 0x060027B2 RID: 10162 RVA: 0x0028B214 File Offset: 0x0028A614
			internal override void TransactionRolledBack(long transactionId)
			{
				if (Bid.AdvancedOn)
				{
					Bid.Trace("<sc.SqlInternalConnectionSmi.EventSink.TransactionRolledBack|ADV> %d#, transactionId=0x%I64x.\n", this._connection.ObjectID, transactionId);
				}
				this._connection.TransactionEnded(transactionId, TransactionState.Aborted);
			}

			// Token: 0x060027B3 RID: 10163 RVA: 0x0028B24C File Offset: 0x0028A64C
			internal override void TransactionStarted(long transactionId)
			{
				if (Bid.AdvancedOn)
				{
					Bid.Trace("<sc.SqlInternalConnectionSmi.EventSink.TransactionStarted|ADV> %d#, transactionId=0x%I64x.\n", this._connection.ObjectID, transactionId);
				}
				this._connection.TransactionStarted(transactionId, false);
			}

			// Token: 0x040018FA RID: 6394
			private SqlInternalConnectionSmi _connection;
		}
	}
}
