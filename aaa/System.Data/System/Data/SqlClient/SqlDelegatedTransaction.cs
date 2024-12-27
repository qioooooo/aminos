using System;
using System.Data.Common;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Transactions;

namespace System.Data.SqlClient
{
	// Token: 0x020002E3 RID: 739
	internal sealed class SqlDelegatedTransaction : IPromotableSinglePhaseNotification, ITransactionPromoter
	{
		// Token: 0x1700060D RID: 1549
		// (get) Token: 0x0600268C RID: 9868 RVA: 0x00282FF0 File Offset: 0x002823F0
		internal int ObjectID
		{
			get
			{
				return this._objectID;
			}
		}

		// Token: 0x0600268D RID: 9869 RVA: 0x00283004 File Offset: 0x00282404
		internal SqlDelegatedTransaction(SqlInternalConnection connection, Transaction tx)
		{
			this._connection = connection;
			this._atomicTransaction = tx;
			this._active = false;
			IsolationLevel isolationLevel = tx.IsolationLevel;
			switch (isolationLevel)
			{
			case IsolationLevel.Serializable:
				this._isolationLevel = IsolationLevel.Serializable;
				return;
			case IsolationLevel.RepeatableRead:
				this._isolationLevel = IsolationLevel.RepeatableRead;
				return;
			case IsolationLevel.ReadCommitted:
				this._isolationLevel = IsolationLevel.ReadCommitted;
				return;
			case IsolationLevel.ReadUncommitted:
				this._isolationLevel = IsolationLevel.ReadUncommitted;
				return;
			case IsolationLevel.Snapshot:
				this._isolationLevel = IsolationLevel.Snapshot;
				return;
			default:
				throw SQL.UnknownSysTxIsolationLevel(isolationLevel);
			}
		}

		// Token: 0x1700060E RID: 1550
		// (get) Token: 0x0600268E RID: 9870 RVA: 0x002830A4 File Offset: 0x002824A4
		internal Transaction Transaction
		{
			get
			{
				return this._atomicTransaction;
			}
		}

		// Token: 0x0600268F RID: 9871 RVA: 0x002830B8 File Offset: 0x002824B8
		public void Initialize()
		{
			SqlInternalConnection connection = this._connection;
			SqlConnection connection2 = connection.Connection;
			Bid.Trace("<sc.SqlDelegatedTransaction.Initialize|RES|CPOOL> %d#, Connection %d#, delegating transaction.\n", this.ObjectID, connection.ObjectID);
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
				if (connection.IsEnlistedInTransaction)
				{
					Bid.Trace("<sc.SqlDelegatedTransaction.Initialize|RES|CPOOL> %d#, Connection %d#, was enlisted, now defecting.\n", this.ObjectID, connection.ObjectID);
					connection.EnlistNull();
				}
				this._internalTransaction = new SqlInternalTransaction(connection, TransactionType.Delegated, null);
				connection.ExecuteTransaction(SqlInternalConnection.TransactionRequest.Begin, null, this._isolationLevel, this._internalTransaction, true);
				if (connection.CurrentTransaction == null)
				{
					connection.DoomThisConnection();
					throw ADP.InternalError(ADP.InternalErrorCode.UnknownTransactionFailure);
				}
				this._active = true;
			}
			catch (OutOfMemoryException ex)
			{
				connection2.Abort(ex);
				throw;
			}
			catch (StackOverflowException ex2)
			{
				connection2.Abort(ex2);
				throw;
			}
			catch (ThreadAbortException ex3)
			{
				connection2.Abort(ex3);
				throw;
			}
		}

		// Token: 0x1700060F RID: 1551
		// (get) Token: 0x06002690 RID: 9872 RVA: 0x002831C8 File Offset: 0x002825C8
		internal bool IsActive
		{
			get
			{
				return this._active;
			}
		}

		// Token: 0x06002691 RID: 9873 RVA: 0x002831DC File Offset: 0x002825DC
		public byte[] Promote()
		{
			SqlInternalConnection validConnection = this.GetValidConnection();
			byte[] array = null;
			SqlConnection connection = validConnection.Connection;
			Bid.Trace("<sc.SqlDelegatedTransaction.Promote|RES|CPOOL> %d#, Connection %d#, promoting transaction.\n", this.ObjectID, validConnection.ObjectID);
			RuntimeHelpers.PrepareConstrainedRegions();
			Exception ex;
			try
			{
				lock (validConnection)
				{
					try
					{
						this.ValidateActiveOnConnection(validConnection);
						validConnection.ExecuteTransaction(SqlInternalConnection.TransactionRequest.Promote, null, IsolationLevel.Unspecified, this._internalTransaction, true);
						array = this._connection.PromotedDTCToken;
						ex = null;
					}
					catch (SqlException ex2)
					{
						ex = ex2;
						ADP.TraceExceptionWithoutRethrow(ex2);
						validConnection.DoomThisConnection();
					}
					catch (InvalidOperationException ex3)
					{
						ex = ex3;
						ADP.TraceExceptionWithoutRethrow(ex3);
						validConnection.DoomThisConnection();
					}
				}
			}
			catch (OutOfMemoryException ex4)
			{
				connection.Abort(ex4);
				throw;
			}
			catch (StackOverflowException ex5)
			{
				connection.Abort(ex5);
				throw;
			}
			catch (ThreadAbortException ex6)
			{
				connection.Abort(ex6);
				throw;
			}
			if (ex != null)
			{
				throw SQL.PromotionFailed(ex);
			}
			return array;
		}

		// Token: 0x06002692 RID: 9874 RVA: 0x00283344 File Offset: 0x00282744
		public void Rollback(SinglePhaseEnlistment enlistment)
		{
			SqlInternalConnection validConnection = this.GetValidConnection();
			SqlConnection connection = validConnection.Connection;
			Bid.Trace("<sc.SqlDelegatedTransaction.Rollback|RES|CPOOL> %d#, Connection %d#, aborting transaction.\n", this.ObjectID, validConnection.ObjectID);
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
				lock (validConnection)
				{
					try
					{
						this.ValidateActiveOnConnection(validConnection);
						this._active = false;
						this._connection = null;
						validConnection.ExecuteTransaction(SqlInternalConnection.TransactionRequest.Rollback, null, IsolationLevel.Unspecified, this._internalTransaction, true);
					}
					catch (SqlException ex)
					{
						ADP.TraceExceptionWithoutRethrow(ex);
						validConnection.DoomThisConnection();
					}
					catch (InvalidOperationException ex2)
					{
						ADP.TraceExceptionWithoutRethrow(ex2);
						validConnection.DoomThisConnection();
					}
				}
				validConnection.CleanupConnectionOnTransactionCompletion(this._atomicTransaction);
				enlistment.Aborted();
			}
			catch (OutOfMemoryException ex3)
			{
				connection.Abort(ex3);
				throw;
			}
			catch (StackOverflowException ex4)
			{
				connection.Abort(ex4);
				throw;
			}
			catch (ThreadAbortException ex5)
			{
				connection.Abort(ex5);
				throw;
			}
		}

		// Token: 0x06002693 RID: 9875 RVA: 0x002834A4 File Offset: 0x002828A4
		public void SinglePhaseCommit(SinglePhaseEnlistment enlistment)
		{
			SqlInternalConnection validConnection = this.GetValidConnection();
			SqlConnection connection = validConnection.Connection;
			Bid.Trace("<sc.SqlDelegatedTransaction.SinglePhaseCommit|RES|CPOOL> %d#, Connection %d#, committing transaction.\n", this.ObjectID, validConnection.ObjectID);
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
				if (validConnection.IsConnectionDoomed)
				{
					lock (validConnection)
					{
						this._active = false;
						this._connection = null;
					}
					enlistment.Aborted(SQL.ConnectionDoomed());
				}
				else
				{
					Exception ex;
					lock (validConnection)
					{
						try
						{
							this.ValidateActiveOnConnection(validConnection);
							this._active = false;
							this._connection = null;
							validConnection.ExecuteTransaction(SqlInternalConnection.TransactionRequest.Commit, null, IsolationLevel.Unspecified, this._internalTransaction, true);
							ex = null;
						}
						catch (SqlException ex2)
						{
							ex = ex2;
							ADP.TraceExceptionWithoutRethrow(ex2);
							validConnection.DoomThisConnection();
						}
						catch (InvalidOperationException ex3)
						{
							ex = ex3;
							ADP.TraceExceptionWithoutRethrow(ex3);
							validConnection.DoomThisConnection();
						}
					}
					if (ex != null)
					{
						if (this._internalTransaction.IsCommitted)
						{
							enlistment.Committed();
						}
						else if (this._internalTransaction.IsAborted)
						{
							enlistment.Aborted(ex);
						}
						else
						{
							enlistment.InDoubt(ex);
						}
					}
					validConnection.CleanupConnectionOnTransactionCompletion(this._atomicTransaction);
					if (ex == null)
					{
						enlistment.Committed();
					}
				}
			}
			catch (OutOfMemoryException ex4)
			{
				connection.Abort(ex4);
				throw;
			}
			catch (StackOverflowException ex5)
			{
				connection.Abort(ex5);
				throw;
			}
			catch (ThreadAbortException ex6)
			{
				connection.Abort(ex6);
				throw;
			}
		}

		// Token: 0x06002694 RID: 9876 RVA: 0x00283698 File Offset: 0x00282A98
		internal void TransactionEnded(Transaction transaction)
		{
			SqlInternalConnection connection = this._connection;
			if (connection != null)
			{
				Bid.Trace("<sc.SqlDelegatedTransaction.TransactionEnded|RES|CPOOL> %d#, Connection %d#, transaction completed externally.\n", this.ObjectID, connection.ObjectID);
				lock (connection)
				{
					if (this._atomicTransaction.Equals(transaction))
					{
						this._active = false;
						this._connection = null;
					}
				}
			}
		}

		// Token: 0x06002695 RID: 9877 RVA: 0x00283710 File Offset: 0x00282B10
		private SqlInternalConnection GetValidConnection()
		{
			SqlInternalConnection connection = this._connection;
			if (connection == null)
			{
				throw ADP.ObjectDisposed(this);
			}
			return connection;
		}

		// Token: 0x06002696 RID: 9878 RVA: 0x00283730 File Offset: 0x00282B30
		private void ValidateActiveOnConnection(SqlInternalConnection connection)
		{
			if (!this._active || connection != this._connection || connection.DelegatedTransaction != this)
			{
				if (connection != null)
				{
					connection.DoomThisConnection();
				}
				if (connection != this._connection && this._connection != null)
				{
					this._connection.DoomThisConnection();
				}
				throw ADP.InternalError(ADP.InternalErrorCode.UnpooledObjectHasWrongOwner);
			}
		}

		// Token: 0x0400183E RID: 6206
		private static int _objectTypeCount;

		// Token: 0x0400183F RID: 6207
		private readonly int _objectID = Interlocked.Increment(ref SqlDelegatedTransaction._objectTypeCount);

		// Token: 0x04001840 RID: 6208
		private SqlInternalConnection _connection;

		// Token: 0x04001841 RID: 6209
		private IsolationLevel _isolationLevel;

		// Token: 0x04001842 RID: 6210
		private SqlInternalTransaction _internalTransaction;

		// Token: 0x04001843 RID: 6211
		private Transaction _atomicTransaction;

		// Token: 0x04001844 RID: 6212
		private bool _active;
	}
}
