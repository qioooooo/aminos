using System;
using System.Data.Common;
using System.Runtime.CompilerServices;
using System.Threading;

namespace System.Data.SqlClient
{
	// Token: 0x02000311 RID: 785
	public sealed class SqlTransaction : DbTransaction
	{
		// Token: 0x06002906 RID: 10502 RVA: 0x00291D94 File Offset: 0x00291194
		internal SqlTransaction(SqlInternalConnection internalConnection, SqlConnection con, IsolationLevel iso, SqlInternalTransaction internalTransaction)
		{
			this._isolationLevel = iso;
			this._connection = con;
			if (internalTransaction == null)
			{
				this._internalTransaction = new SqlInternalTransaction(internalConnection, TransactionType.LocalFromAPI, this);
				return;
			}
			this._internalTransaction = internalTransaction;
			this._internalTransaction.InitParent(this);
		}

		// Token: 0x170006C7 RID: 1735
		// (get) Token: 0x06002907 RID: 10503 RVA: 0x00291DF8 File Offset: 0x002911F8
		public new SqlConnection Connection
		{
			get
			{
				if (this.IsZombied)
				{
					return null;
				}
				return this._connection;
			}
		}

		// Token: 0x170006C8 RID: 1736
		// (get) Token: 0x06002908 RID: 10504 RVA: 0x00291E18 File Offset: 0x00291218
		protected override DbConnection DbConnection
		{
			get
			{
				return this.Connection;
			}
		}

		// Token: 0x170006C9 RID: 1737
		// (get) Token: 0x06002909 RID: 10505 RVA: 0x00291E2C File Offset: 0x0029122C
		internal SqlInternalTransaction InternalTransaction
		{
			get
			{
				return this._internalTransaction;
			}
		}

		// Token: 0x170006CA RID: 1738
		// (get) Token: 0x0600290A RID: 10506 RVA: 0x00291E40 File Offset: 0x00291240
		public override IsolationLevel IsolationLevel
		{
			get
			{
				this.ZombieCheck();
				return this._isolationLevel;
			}
		}

		// Token: 0x170006CB RID: 1739
		// (get) Token: 0x0600290B RID: 10507 RVA: 0x00291E5C File Offset: 0x0029125C
		private bool IsYukonPartialZombie
		{
			get
			{
				return this._internalTransaction != null && this._internalTransaction.IsCompleted;
			}
		}

		// Token: 0x170006CC RID: 1740
		// (get) Token: 0x0600290C RID: 10508 RVA: 0x00291E80 File Offset: 0x00291280
		internal bool IsZombied
		{
			get
			{
				return this._internalTransaction == null || this._internalTransaction.IsCompleted;
			}
		}

		// Token: 0x170006CD RID: 1741
		// (get) Token: 0x0600290D RID: 10509 RVA: 0x00291EA4 File Offset: 0x002912A4
		internal int ObjectID
		{
			get
			{
				return this._objectID;
			}
		}

		// Token: 0x170006CE RID: 1742
		// (get) Token: 0x0600290E RID: 10510 RVA: 0x00291EB8 File Offset: 0x002912B8
		internal SqlStatistics Statistics
		{
			get
			{
				if (this._connection != null && this._connection.StatisticsEnabled)
				{
					return this._connection.Statistics;
				}
				return null;
			}
		}

		// Token: 0x0600290F RID: 10511 RVA: 0x00291EE8 File Offset: 0x002912E8
		public override void Commit()
		{
			SqlConnection.ExecutePermission.Demand();
			this.ZombieCheck();
			SqlStatistics sqlStatistics = null;
			IntPtr intPtr;
			Bid.ScopeEnter(out intPtr, "<sc.SqlTransaction.Commit|API> %d#", this.ObjectID);
			SNIHandle snihandle = null;
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
				snihandle = SqlInternalConnection.GetBestEffortCleanupTarget(this._connection);
				sqlStatistics = SqlStatistics.StartTimer(this.Statistics);
				this._isFromAPI = true;
				this._internalTransaction.Commit();
			}
			catch (OutOfMemoryException ex)
			{
				this._connection.Abort(ex);
				throw;
			}
			catch (StackOverflowException ex2)
			{
				this._connection.Abort(ex2);
				throw;
			}
			catch (ThreadAbortException ex3)
			{
				this._connection.Abort(ex3);
				SqlInternalConnection.BestEffortCleanup(snihandle);
				throw;
			}
			finally
			{
				this._isFromAPI = false;
				SqlStatistics.StopTimer(sqlStatistics);
				Bid.ScopeLeave(ref intPtr);
			}
		}

		// Token: 0x06002910 RID: 10512 RVA: 0x00292004 File Offset: 0x00291404
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				SNIHandle snihandle = null;
				RuntimeHelpers.PrepareConstrainedRegions();
				try
				{
					snihandle = SqlInternalConnection.GetBestEffortCleanupTarget(this._connection);
					if (!this.IsZombied && !this.IsYukonPartialZombie)
					{
						this._internalTransaction.Dispose();
					}
				}
				catch (OutOfMemoryException ex)
				{
					this._connection.Abort(ex);
					throw;
				}
				catch (StackOverflowException ex2)
				{
					this._connection.Abort(ex2);
					throw;
				}
				catch (ThreadAbortException ex3)
				{
					this._connection.Abort(ex3);
					SqlInternalConnection.BestEffortCleanup(snihandle);
					throw;
				}
			}
			base.Dispose(disposing);
		}

		// Token: 0x06002911 RID: 10513 RVA: 0x002920D0 File Offset: 0x002914D0
		public override void Rollback()
		{
			if (this.IsYukonPartialZombie)
			{
				if (Bid.AdvancedOn)
				{
					Bid.Trace("<sc.SqlTransaction.Rollback|ADV> %d# partial zombie no rollback required\n", this.ObjectID);
				}
				this._internalTransaction = null;
				return;
			}
			this.ZombieCheck();
			SqlStatistics sqlStatistics = null;
			IntPtr intPtr;
			Bid.ScopeEnter(out intPtr, "<sc.SqlTransaction.Rollback|API> %d#", this.ObjectID);
			SNIHandle snihandle = null;
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
				snihandle = SqlInternalConnection.GetBestEffortCleanupTarget(this._connection);
				sqlStatistics = SqlStatistics.StartTimer(this.Statistics);
				this._isFromAPI = true;
				this._internalTransaction.Rollback();
			}
			catch (OutOfMemoryException ex)
			{
				this._connection.Abort(ex);
				throw;
			}
			catch (StackOverflowException ex2)
			{
				this._connection.Abort(ex2);
				throw;
			}
			catch (ThreadAbortException ex3)
			{
				this._connection.Abort(ex3);
				SqlInternalConnection.BestEffortCleanup(snihandle);
				throw;
			}
			finally
			{
				this._isFromAPI = false;
				SqlStatistics.StopTimer(sqlStatistics);
				Bid.ScopeLeave(ref intPtr);
			}
		}

		// Token: 0x06002912 RID: 10514 RVA: 0x00292208 File Offset: 0x00291608
		public void Rollback(string transactionName)
		{
			SqlConnection.ExecutePermission.Demand();
			this.ZombieCheck();
			SqlStatistics sqlStatistics = null;
			IntPtr intPtr;
			Bid.ScopeEnter(out intPtr, "<sc.SqlTransaction.Rollback|API> %d# transactionName='%ls'", this.ObjectID, transactionName);
			SNIHandle snihandle = null;
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
				snihandle = SqlInternalConnection.GetBestEffortCleanupTarget(this._connection);
				sqlStatistics = SqlStatistics.StartTimer(this.Statistics);
				this._isFromAPI = true;
				this._internalTransaction.Rollback(transactionName);
			}
			catch (OutOfMemoryException ex)
			{
				this._connection.Abort(ex);
				throw;
			}
			catch (StackOverflowException ex2)
			{
				this._connection.Abort(ex2);
				throw;
			}
			catch (ThreadAbortException ex3)
			{
				this._connection.Abort(ex3);
				SqlInternalConnection.BestEffortCleanup(snihandle);
				throw;
			}
			finally
			{
				this._isFromAPI = false;
				SqlStatistics.StopTimer(sqlStatistics);
				Bid.ScopeLeave(ref intPtr);
			}
		}

		// Token: 0x06002913 RID: 10515 RVA: 0x00292324 File Offset: 0x00291724
		public void Save(string savePointName)
		{
			SqlConnection.ExecutePermission.Demand();
			this.ZombieCheck();
			SqlStatistics sqlStatistics = null;
			IntPtr intPtr;
			Bid.ScopeEnter(out intPtr, "<sc.SqlTransaction.Save|API> %d# savePointName='%ls'", this.ObjectID, savePointName);
			SNIHandle snihandle = null;
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
				snihandle = SqlInternalConnection.GetBestEffortCleanupTarget(this._connection);
				sqlStatistics = SqlStatistics.StartTimer(this.Statistics);
				this._internalTransaction.Save(savePointName);
			}
			catch (OutOfMemoryException ex)
			{
				this._connection.Abort(ex);
				throw;
			}
			catch (StackOverflowException ex2)
			{
				this._connection.Abort(ex2);
				throw;
			}
			catch (ThreadAbortException ex3)
			{
				this._connection.Abort(ex3);
				SqlInternalConnection.BestEffortCleanup(snihandle);
				throw;
			}
			finally
			{
				SqlStatistics.StopTimer(sqlStatistics);
				Bid.ScopeLeave(ref intPtr);
			}
		}

		// Token: 0x06002914 RID: 10516 RVA: 0x00292434 File Offset: 0x00291834
		internal void Zombie()
		{
			SqlInternalConnection sqlInternalConnection = this._connection.InnerConnection as SqlInternalConnection;
			if (sqlInternalConnection != null && sqlInternalConnection.IsYukonOrNewer && !this._isFromAPI)
			{
				if (Bid.AdvancedOn)
				{
					Bid.Trace("<sc.SqlTransaction.Zombie|ADV> %d# yukon deferred zombie\n", this.ObjectID);
					return;
				}
			}
			else
			{
				this._internalTransaction = null;
			}
		}

		// Token: 0x06002915 RID: 10517 RVA: 0x00292484 File Offset: 0x00291884
		private void ZombieCheck()
		{
			if (this.IsZombied)
			{
				if (this.IsYukonPartialZombie)
				{
					this._internalTransaction = null;
				}
				throw ADP.TransactionZombied(this);
			}
		}

		// Token: 0x0400199B RID: 6555
		private static int _objectTypeCount;

		// Token: 0x0400199C RID: 6556
		internal readonly int _objectID = Interlocked.Increment(ref SqlTransaction._objectTypeCount);

		// Token: 0x0400199D RID: 6557
		internal readonly IsolationLevel _isolationLevel = IsolationLevel.ReadCommitted;

		// Token: 0x0400199E RID: 6558
		private SqlInternalTransaction _internalTransaction;

		// Token: 0x0400199F RID: 6559
		private SqlConnection _connection;

		// Token: 0x040019A0 RID: 6560
		private bool _isFromAPI;
	}
}
