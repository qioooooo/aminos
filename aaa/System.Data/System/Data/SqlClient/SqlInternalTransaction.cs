using System;
using System.Data.Common;
using System.Threading;

namespace System.Data.SqlClient
{
	// Token: 0x020002FF RID: 767
	internal sealed class SqlInternalTransaction
	{
		// Token: 0x060027F1 RID: 10225 RVA: 0x0028CB18 File Offset: 0x0028BF18
		internal SqlInternalTransaction(SqlInternalConnection innerConnection, TransactionType type, SqlTransaction outerTransaction)
			: this(innerConnection, type, outerTransaction, 0L)
		{
		}

		// Token: 0x060027F2 RID: 10226 RVA: 0x0028CB30 File Offset: 0x0028BF30
		internal SqlInternalTransaction(SqlInternalConnection innerConnection, TransactionType type, SqlTransaction outerTransaction, long transactionId)
		{
			Bid.PoolerTrace("<sc.SqlInternalTransaction.ctor|RES|CPOOL> %d#, Created for connection %d#, outer transaction %d#, Type %d\n", this.ObjectID, innerConnection.ObjectID, (outerTransaction != null) ? outerTransaction.ObjectID : (-1), (int)type);
			this._innerConnection = innerConnection;
			this._transactionType = type;
			if (outerTransaction != null)
			{
				this._parent = new WeakReference(outerTransaction);
			}
			this._transactionId = transactionId;
		}

		// Token: 0x1700067C RID: 1660
		// (get) Token: 0x060027F3 RID: 10227 RVA: 0x0028CB9C File Offset: 0x0028BF9C
		internal bool HasParentTransaction
		{
			get
			{
				return TransactionType.LocalFromAPI == this._transactionType || (TransactionType.LocalFromTSQL == this._transactionType && this._parent != null);
			}
		}

		// Token: 0x1700067D RID: 1661
		// (get) Token: 0x060027F4 RID: 10228 RVA: 0x0028CBD0 File Offset: 0x0028BFD0
		internal bool IsAborted
		{
			get
			{
				return TransactionState.Aborted == this._transactionState;
			}
		}

		// Token: 0x1700067E RID: 1662
		// (get) Token: 0x060027F5 RID: 10229 RVA: 0x0028CBE8 File Offset: 0x0028BFE8
		internal bool IsActive
		{
			get
			{
				return TransactionState.Active == this._transactionState;
			}
		}

		// Token: 0x1700067F RID: 1663
		// (get) Token: 0x060027F6 RID: 10230 RVA: 0x0028CC00 File Offset: 0x0028C000
		internal bool IsCommitted
		{
			get
			{
				return TransactionState.Committed == this._transactionState;
			}
		}

		// Token: 0x17000680 RID: 1664
		// (get) Token: 0x060027F7 RID: 10231 RVA: 0x0028CC18 File Offset: 0x0028C018
		internal bool IsCompleted
		{
			get
			{
				return TransactionState.Aborted == this._transactionState || TransactionState.Committed == this._transactionState || TransactionState.Unknown == this._transactionState;
			}
		}

		// Token: 0x17000681 RID: 1665
		// (get) Token: 0x060027F8 RID: 10232 RVA: 0x0028CC44 File Offset: 0x0028C044
		internal bool IsContext
		{
			get
			{
				return TransactionType.Context == this._transactionType;
			}
		}

		// Token: 0x17000682 RID: 1666
		// (get) Token: 0x060027F9 RID: 10233 RVA: 0x0028CC5C File Offset: 0x0028C05C
		internal bool IsDelegated
		{
			get
			{
				return TransactionType.Delegated == this._transactionType;
			}
		}

		// Token: 0x17000683 RID: 1667
		// (get) Token: 0x060027FA RID: 10234 RVA: 0x0028CC74 File Offset: 0x0028C074
		internal bool IsDistributed
		{
			get
			{
				return TransactionType.Distributed == this._transactionType;
			}
		}

		// Token: 0x17000684 RID: 1668
		// (get) Token: 0x060027FB RID: 10235 RVA: 0x0028CC8C File Offset: 0x0028C08C
		internal bool IsLocal
		{
			get
			{
				return TransactionType.LocalFromTSQL == this._transactionType || TransactionType.LocalFromAPI == this._transactionType || TransactionType.Context == this._transactionType;
			}
		}

		// Token: 0x17000685 RID: 1669
		// (get) Token: 0x060027FC RID: 10236 RVA: 0x0028CCBC File Offset: 0x0028C0BC
		internal bool IsOrphaned
		{
			get
			{
				return this._parent != null && this._parent.Target == null;
			}
		}

		// Token: 0x17000686 RID: 1670
		// (get) Token: 0x060027FD RID: 10237 RVA: 0x0028CCEC File Offset: 0x0028C0EC
		internal bool IsZombied
		{
			get
			{
				return null == this._innerConnection;
			}
		}

		// Token: 0x17000687 RID: 1671
		// (get) Token: 0x060027FE RID: 10238 RVA: 0x0028CD04 File Offset: 0x0028C104
		internal int ObjectID
		{
			get
			{
				return this._objectID;
			}
		}

		// Token: 0x17000688 RID: 1672
		// (get) Token: 0x060027FF RID: 10239 RVA: 0x0028CD18 File Offset: 0x0028C118
		internal int OpenResultsCount
		{
			get
			{
				return this._openResultCount;
			}
		}

		// Token: 0x17000689 RID: 1673
		// (get) Token: 0x06002800 RID: 10240 RVA: 0x0028CD2C File Offset: 0x0028C12C
		internal SqlTransaction Parent
		{
			get
			{
				SqlTransaction sqlTransaction = null;
				if (this._parent != null)
				{
					sqlTransaction = (SqlTransaction)this._parent.Target;
				}
				return sqlTransaction;
			}
		}

		// Token: 0x1700068A RID: 1674
		// (get) Token: 0x06002801 RID: 10241 RVA: 0x0028CD58 File Offset: 0x0028C158
		// (set) Token: 0x06002802 RID: 10242 RVA: 0x0028CD6C File Offset: 0x0028C16C
		internal long TransactionId
		{
			get
			{
				return this._transactionId;
			}
			set
			{
				this._transactionId = value;
			}
		}

		// Token: 0x06002803 RID: 10243 RVA: 0x0028CD80 File Offset: 0x0028C180
		internal void Activate()
		{
			this._transactionState = TransactionState.Active;
		}

		// Token: 0x06002804 RID: 10244 RVA: 0x0028CD94 File Offset: 0x0028C194
		private void CheckTransactionLevelAndZombie()
		{
			try
			{
				if (!this.IsZombied && this.GetServerTransactionLevel() == 0)
				{
					this.Zombie();
				}
			}
			catch (Exception ex)
			{
				if (!ADP.IsCatchableExceptionType(ex))
				{
					throw;
				}
				ADP.TraceExceptionWithoutRethrow(ex);
				this.Zombie();
			}
		}

		// Token: 0x06002805 RID: 10245 RVA: 0x0028CDF0 File Offset: 0x0028C1F0
		internal void CloseFromConnection()
		{
			SqlInternalConnection innerConnection = this._innerConnection;
			Bid.PoolerTrace("<sc.SqlInteralTransaction.CloseFromConnection|RES|CPOOL> %d#, Closing\n", this.ObjectID);
			bool flag = true;
			try
			{
				innerConnection.ExecuteTransaction(SqlInternalConnection.TransactionRequest.IfRollback, null, IsolationLevel.Unspecified, null, false);
			}
			catch (Exception ex)
			{
				flag = ADP.IsCatchableExceptionType(ex);
				throw;
			}
			finally
			{
				if (flag)
				{
					this.Zombie();
				}
			}
		}

		// Token: 0x06002806 RID: 10246 RVA: 0x0028CE70 File Offset: 0x0028C270
		internal void Commit()
		{
			IntPtr intPtr;
			Bid.ScopeEnter(out intPtr, "<sc.SqlInternalTransaction.Commit|API> %d#", this.ObjectID);
			if (this._innerConnection.IsLockedForBulkCopy)
			{
				throw SQL.ConnectionLockedForBcpEvent();
			}
			this._innerConnection.ValidateConnectionForExecute(null);
			try
			{
				this._innerConnection.ExecuteTransaction(SqlInternalConnection.TransactionRequest.Commit, null, IsolationLevel.Unspecified, null, false);
				if (!this.IsZombied && !this._innerConnection.IsYukonOrNewer)
				{
					this.Zombie();
				}
				else
				{
					this.ZombieParent();
				}
			}
			catch (Exception ex)
			{
				if (ADP.IsCatchableExceptionType(ex))
				{
					this.CheckTransactionLevelAndZombie();
				}
				throw;
			}
			finally
			{
				Bid.ScopeLeave(ref intPtr);
			}
		}

		// Token: 0x06002807 RID: 10247 RVA: 0x0028CF34 File Offset: 0x0028C334
		internal void Completed(TransactionState transactionState)
		{
			this._transactionState = transactionState;
			this.Zombie();
		}

		// Token: 0x06002808 RID: 10248 RVA: 0x0028CF50 File Offset: 0x0028C350
		internal int DecrementAndObtainOpenResultCount()
		{
			int num = Interlocked.Decrement(ref this._openResultCount);
			if (num < 0)
			{
				throw ADP.InvalidOperation("Internal Error: Open Result Count Exceeded");
			}
			return num;
		}

		// Token: 0x06002809 RID: 10249 RVA: 0x0028CF7C File Offset: 0x0028C37C
		internal void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x0600280A RID: 10250 RVA: 0x0028CF98 File Offset: 0x0028C398
		private void Dispose(bool disposing)
		{
			Bid.PoolerTrace("<sc.SqlInteralTransaction.Dispose|RES|CPOOL> %d#, Disposing\n", this.ObjectID);
			if (disposing && this._innerConnection != null)
			{
				this._disposing = true;
				this.Rollback();
			}
		}

		// Token: 0x0600280B RID: 10251 RVA: 0x0028CFD0 File Offset: 0x0028C3D0
		private int GetServerTransactionLevel()
		{
			int num;
			using (SqlCommand sqlCommand = new SqlCommand("set @out = @@trancount", (SqlConnection)this._innerConnection.Owner))
			{
				sqlCommand.Transaction = this.Parent;
				SqlParameter sqlParameter = new SqlParameter("@out", SqlDbType.Int);
				sqlParameter.Direction = ParameterDirection.Output;
				sqlCommand.Parameters.Add(sqlParameter);
				sqlCommand.RunExecuteReader(CommandBehavior.Default, RunBehavior.UntilDone, false, "GetServerTransactionLevel");
				num = (int)sqlParameter.Value;
			}
			return num;
		}

		// Token: 0x0600280C RID: 10252 RVA: 0x0028D068 File Offset: 0x0028C468
		internal int IncrementAndObtainOpenResultCount()
		{
			int num = Interlocked.Increment(ref this._openResultCount);
			if (num < 0)
			{
				throw ADP.InvalidOperation("Internal Error: Open Result Count Exceeded");
			}
			return num;
		}

		// Token: 0x0600280D RID: 10253 RVA: 0x0028D094 File Offset: 0x0028C494
		internal void InitParent(SqlTransaction transaction)
		{
			this._parent = new WeakReference(transaction);
		}

		// Token: 0x0600280E RID: 10254 RVA: 0x0028D0B0 File Offset: 0x0028C4B0
		internal void Rollback()
		{
			IntPtr intPtr;
			Bid.ScopeEnter(out intPtr, "<sc.SqlInternalTransaction.Rollback|API> %d#", this.ObjectID);
			if (this._innerConnection.IsLockedForBulkCopy)
			{
				throw SQL.ConnectionLockedForBcpEvent();
			}
			this._innerConnection.ValidateConnectionForExecute(null);
			try
			{
				this._innerConnection.ExecuteTransaction(SqlInternalConnection.TransactionRequest.IfRollback, null, IsolationLevel.Unspecified, null, false);
				this.Zombie();
			}
			catch (Exception ex)
			{
				if (!ADP.IsCatchableExceptionType(ex))
				{
					throw;
				}
				this.CheckTransactionLevelAndZombie();
				if (!this._disposing)
				{
					throw;
				}
			}
			finally
			{
				Bid.ScopeLeave(ref intPtr);
			}
		}

		// Token: 0x0600280F RID: 10255 RVA: 0x0028D164 File Offset: 0x0028C564
		internal void Rollback(string transactionName)
		{
			IntPtr intPtr;
			Bid.ScopeEnter(out intPtr, "<sc.SqlInternalTransaction.Rollback|API> %d#, transactionName='%ls'", this.ObjectID, transactionName);
			if (this._innerConnection.IsLockedForBulkCopy)
			{
				throw SQL.ConnectionLockedForBcpEvent();
			}
			this._innerConnection.ValidateConnectionForExecute(null);
			try
			{
				if (ADP.IsEmpty(transactionName))
				{
					throw SQL.NullEmptyTransactionName();
				}
				try
				{
					this._innerConnection.ExecuteTransaction(SqlInternalConnection.TransactionRequest.Rollback, transactionName, IsolationLevel.Unspecified, null, false);
					if (!this.IsZombied && !this._innerConnection.IsYukonOrNewer)
					{
						this.CheckTransactionLevelAndZombie();
					}
				}
				catch (Exception ex)
				{
					if (ADP.IsCatchableExceptionType(ex))
					{
						this.CheckTransactionLevelAndZombie();
					}
					throw;
				}
			}
			finally
			{
				Bid.ScopeLeave(ref intPtr);
			}
		}

		// Token: 0x06002810 RID: 10256 RVA: 0x0028D230 File Offset: 0x0028C630
		internal void Save(string savePointName)
		{
			IntPtr intPtr;
			Bid.ScopeEnter(out intPtr, "<sc.SqlInternalTransaction.Save|API> %d#, savePointName='%ls'", this.ObjectID, savePointName);
			this._innerConnection.ValidateConnectionForExecute(null);
			try
			{
				if (ADP.IsEmpty(savePointName))
				{
					throw SQL.NullEmptyTransactionName();
				}
				try
				{
					this._innerConnection.ExecuteTransaction(SqlInternalConnection.TransactionRequest.Save, savePointName, IsolationLevel.Unspecified, null, false);
				}
				catch (Exception ex)
				{
					if (ADP.IsCatchableExceptionType(ex))
					{
						this.CheckTransactionLevelAndZombie();
					}
					throw;
				}
			}
			finally
			{
				Bid.ScopeLeave(ref intPtr);
			}
		}

		// Token: 0x06002811 RID: 10257 RVA: 0x0028D2D0 File Offset: 0x0028C6D0
		internal void Zombie()
		{
			this.ZombieParent();
			SqlInternalConnection innerConnection = this._innerConnection;
			this._innerConnection = null;
			if (innerConnection != null)
			{
				innerConnection.DisconnectTransaction(this);
			}
		}

		// Token: 0x06002812 RID: 10258 RVA: 0x0028D2FC File Offset: 0x0028C6FC
		private void ZombieParent()
		{
			if (this._parent != null)
			{
				SqlTransaction sqlTransaction = (SqlTransaction)this._parent.Target;
				if (sqlTransaction != null)
				{
					sqlTransaction.Zombie();
				}
				this._parent = null;
			}
		}

		// Token: 0x06002813 RID: 10259 RVA: 0x0028D334 File Offset: 0x0028C734
		internal string TraceString()
		{
			return string.Format(null, "(ObjId={0}, tranId={1}, state={2}, type={3}, open={4}, disp={5}", new object[] { this.ObjectID, this._transactionId, this._transactionState, this._transactionType, this._openResultCount, this._disposing });
		}

		// Token: 0x0400191B RID: 6427
		internal const long NullTransactionId = 0L;

		// Token: 0x0400191C RID: 6428
		private TransactionState _transactionState;

		// Token: 0x0400191D RID: 6429
		private TransactionType _transactionType;

		// Token: 0x0400191E RID: 6430
		private long _transactionId;

		// Token: 0x0400191F RID: 6431
		private int _openResultCount;

		// Token: 0x04001920 RID: 6432
		private SqlInternalConnection _innerConnection;

		// Token: 0x04001921 RID: 6433
		private bool _disposing;

		// Token: 0x04001922 RID: 6434
		private WeakReference _parent;

		// Token: 0x04001923 RID: 6435
		private static int _objectTypeCount;

		// Token: 0x04001924 RID: 6436
		internal readonly int _objectID = Interlocked.Increment(ref SqlInternalTransaction._objectTypeCount);
	}
}
