using System;
using System.Data.Common;
using System.Runtime.ConstrainedExecution;
using System.Security.Permissions;
using System.Threading;
using System.Transactions;

namespace System.Data.ProviderBase
{
	// Token: 0x02000069 RID: 105
	internal abstract class DbConnectionInternal
	{
		// Token: 0x170000E0 RID: 224
		// (get) Token: 0x060004BC RID: 1212 RVA: 0x000671AC File Offset: 0x000665AC
		// (set) Token: 0x060004BD RID: 1213 RVA: 0x000671C0 File Offset: 0x000665C0
		internal DbConnectionInternal NextPooledObject
		{
			get
			{
				return this._nextPooledObject;
			}
			set
			{
				this._nextPooledObject = value;
			}
		}

		// Token: 0x060004BE RID: 1214 RVA: 0x000671D4 File Offset: 0x000665D4
		protected DbConnectionInternal()
			: this(ConnectionState.Open, true, false)
		{
		}

		// Token: 0x060004BF RID: 1215 RVA: 0x000671EC File Offset: 0x000665EC
		internal DbConnectionInternal(ConnectionState state, bool hidePassword, bool allowSetConnectionString)
		{
			this._allowSetConnectionString = allowSetConnectionString;
			this._hidePassword = hidePassword;
			this._state = state;
		}

		// Token: 0x170000E1 RID: 225
		// (get) Token: 0x060004C0 RID: 1216 RVA: 0x00067234 File Offset: 0x00066634
		internal bool AllowSetConnectionString
		{
			get
			{
				return this._allowSetConnectionString;
			}
		}

		// Token: 0x170000E2 RID: 226
		// (get) Token: 0x060004C1 RID: 1217 RVA: 0x00067248 File Offset: 0x00066648
		internal bool CanBePooled
		{
			get
			{
				return !this._connectionIsDoomed && !this._cannotBePooled && !this._owningObject.IsAlive;
			}
		}

		// Token: 0x170000E3 RID: 227
		// (get) Token: 0x060004C2 RID: 1218 RVA: 0x00067278 File Offset: 0x00066678
		// (set) Token: 0x060004C3 RID: 1219 RVA: 0x0006728C File Offset: 0x0006668C
		protected internal Transaction EnlistedTransaction
		{
			get
			{
				return this._enlistedTransaction;
			}
			set
			{
				if ((null == this._enlistedTransaction && null != value) || (null != this._enlistedTransaction && !this._enlistedTransaction.Equals(value)))
				{
					Transaction transaction = null;
					Transaction transaction2 = null;
					try
					{
						if (null != value)
						{
							transaction = value.Clone();
						}
						lock (this)
						{
							transaction2 = this._enlistedTransaction;
							this._enlistedTransaction = transaction;
							value = transaction;
							transaction = null;
						}
					}
					finally
					{
						if (null != transaction2)
						{
							transaction2.Dispose();
						}
						if (null != transaction)
						{
							transaction.Dispose();
						}
					}
					if (null != value)
					{
						if (Bid.IsOn((Bid.ApiGroup)4096U))
						{
							int hashCode = value.GetHashCode();
							Bid.PoolerTrace("<prov.DbConnectionInternal.set_EnlistedTransaction|RES|CPOOL> %d#, Transaction %d#, Enlisting.\n", this.ObjectID, hashCode);
						}
						this.TransactionOutcomeEnlist(value);
					}
				}
			}
		}

		// Token: 0x170000E4 RID: 228
		// (get) Token: 0x060004C4 RID: 1220 RVA: 0x00067394 File Offset: 0x00066794
		internal bool IsTxRootWaitingForTxEnd
		{
			get
			{
				return this._isInStasis;
			}
		}

		// Token: 0x170000E5 RID: 229
		// (get) Token: 0x060004C5 RID: 1221 RVA: 0x000673A8 File Offset: 0x000667A8
		internal virtual bool RequireExplicitTransactionUnbind
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170000E6 RID: 230
		// (get) Token: 0x060004C6 RID: 1222 RVA: 0x000673B8 File Offset: 0x000667B8
		protected internal virtual bool IsNonPoolableTransactionRoot
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170000E7 RID: 231
		// (get) Token: 0x060004C7 RID: 1223 RVA: 0x000673C8 File Offset: 0x000667C8
		internal virtual bool IsTransactionRoot
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170000E8 RID: 232
		// (get) Token: 0x060004C8 RID: 1224 RVA: 0x000673D8 File Offset: 0x000667D8
		public bool HasEnlistedTransaction
		{
			get
			{
				return null != this.EnlistedTransaction;
			}
		}

		// Token: 0x170000E9 RID: 233
		// (get) Token: 0x060004C9 RID: 1225 RVA: 0x000673F4 File Offset: 0x000667F4
		protected internal bool IsConnectionDoomed
		{
			get
			{
				return this._connectionIsDoomed;
			}
		}

		// Token: 0x170000EA RID: 234
		// (get) Token: 0x060004CA RID: 1226 RVA: 0x00067408 File Offset: 0x00066808
		internal bool IsEmancipated
		{
			get
			{
				return !this.IsTxRootWaitingForTxEnd && this._pooledCount < 1 && !this._owningObject.IsAlive;
			}
		}

		// Token: 0x170000EB RID: 235
		// (get) Token: 0x060004CB RID: 1227 RVA: 0x0006743C File Offset: 0x0006683C
		internal int ObjectID
		{
			get
			{
				return this._objectID;
			}
		}

		// Token: 0x170000EC RID: 236
		// (get) Token: 0x060004CC RID: 1228 RVA: 0x00067450 File Offset: 0x00066850
		protected internal object Owner
		{
			get
			{
				return this._owningObject.Target;
			}
		}

		// Token: 0x170000ED RID: 237
		// (get) Token: 0x060004CD RID: 1229 RVA: 0x00067468 File Offset: 0x00066868
		internal DbConnectionPool Pool
		{
			get
			{
				return this._connectionPool;
			}
		}

		// Token: 0x170000EE RID: 238
		// (get) Token: 0x060004CE RID: 1230 RVA: 0x0006747C File Offset: 0x0006687C
		protected DbConnectionPoolCounters PerformanceCounters
		{
			get
			{
				return this._performanceCounters;
			}
		}

		// Token: 0x170000EF RID: 239
		// (get) Token: 0x060004CF RID: 1231 RVA: 0x00067490 File Offset: 0x00066890
		protected virtual bool ReadyToPrepareTransaction
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170000F0 RID: 240
		// (get) Token: 0x060004D0 RID: 1232 RVA: 0x000674A0 File Offset: 0x000668A0
		protected internal DbReferenceCollection ReferenceCollection
		{
			get
			{
				return this._referenceCollection;
			}
		}

		// Token: 0x170000F1 RID: 241
		// (get) Token: 0x060004D1 RID: 1233
		public abstract string ServerVersion { get; }

		// Token: 0x170000F2 RID: 242
		// (get) Token: 0x060004D2 RID: 1234 RVA: 0x000674B4 File Offset: 0x000668B4
		public virtual string ServerVersionNormalized
		{
			get
			{
				throw ADP.NotSupported();
			}
		}

		// Token: 0x170000F3 RID: 243
		// (get) Token: 0x060004D3 RID: 1235 RVA: 0x000674C8 File Offset: 0x000668C8
		public bool ShouldHidePassword
		{
			get
			{
				return this._hidePassword;
			}
		}

		// Token: 0x170000F4 RID: 244
		// (get) Token: 0x060004D4 RID: 1236 RVA: 0x000674DC File Offset: 0x000668DC
		public ConnectionState State
		{
			get
			{
				return this._state;
			}
		}

		// Token: 0x060004D5 RID: 1237
		protected abstract void Activate(Transaction transaction);

		// Token: 0x060004D6 RID: 1238 RVA: 0x000674F0 File Offset: 0x000668F0
		internal void ActivateConnection(Transaction transaction)
		{
			Bid.PoolerTrace("<prov.DbConnectionInternal.ActivateConnection|RES|INFO|CPOOL> %d#, Activating\n", this.ObjectID);
			this.Activate(transaction);
			this.PerformanceCounters.NumberOfActiveConnections.Increment();
		}

		// Token: 0x060004D7 RID: 1239 RVA: 0x00067524 File Offset: 0x00066924
		internal void AddWeakReference(object value, int tag)
		{
			if (this._referenceCollection == null)
			{
				this._referenceCollection = this.CreateReferenceCollection();
				if (this._referenceCollection == null)
				{
					throw ADP.InternalError(ADP.InternalErrorCode.CreateReferenceCollectionReturnedNull);
				}
			}
			this._referenceCollection.Add(value, tag);
		}

		// Token: 0x060004D8 RID: 1240
		public abstract DbTransaction BeginTransaction(IsolationLevel il);

		// Token: 0x060004D9 RID: 1241 RVA: 0x00067564 File Offset: 0x00066964
		public virtual void ChangeDatabase(string value)
		{
			throw ADP.MethodNotImplemented("ChangeDatabase");
		}

		// Token: 0x060004DA RID: 1242 RVA: 0x0006757C File Offset: 0x0006697C
		internal virtual void CloseConnection(DbConnection owningObject, DbConnectionFactory connectionFactory)
		{
			Bid.PoolerTrace("<prov.DbConnectionInternal.CloseConnection|RES|CPOOL> %d# Closing.\n", this.ObjectID);
			if (connectionFactory.SetInnerConnectionFrom(owningObject, DbConnectionOpenBusy.SingletonInstance, this))
			{
				try
				{
					DbConnectionPool pool = this.Pool;
					Transaction enlistedTransaction = this.EnlistedTransaction;
					if (null != enlistedTransaction && enlistedTransaction.TransactionInformation.Status != TransactionStatus.Active)
					{
						this.DetachTransaction(enlistedTransaction);
					}
					if (pool != null)
					{
						pool.PutObject(this, owningObject);
					}
					else
					{
						this.Deactivate();
						this.PerformanceCounters.HardDisconnectsPerSecond.Increment();
						this._owningObject.Target = null;
						if (this.IsTransactionRoot)
						{
							this.SetInStasis();
						}
						else
						{
							this.PerformanceCounters.NumberOfNonPooledConnections.Decrement();
							this.Dispose();
						}
					}
				}
				finally
				{
					connectionFactory.SetInnerConnectionEvent(owningObject, DbConnectionClosedPreviouslyOpened.SingletonInstance);
				}
			}
		}

		// Token: 0x060004DB RID: 1243 RVA: 0x00067658 File Offset: 0x00066A58
		protected virtual DbReferenceCollection CreateReferenceCollection()
		{
			throw ADP.InternalError(ADP.InternalErrorCode.AttemptingToConstructReferenceCollectionOnStaticObject);
		}

		// Token: 0x060004DC RID: 1244
		protected abstract void Deactivate();

		// Token: 0x060004DD RID: 1245 RVA: 0x0006766C File Offset: 0x00066A6C
		internal void DeactivateConnection()
		{
			Bid.PoolerTrace("<prov.DbConnectionInternal.DeactivateConnection|RES|INFO|CPOOL> %d#, Deactivating\n", this.ObjectID);
			this.PerformanceCounters.NumberOfActiveConnections.Decrement();
			if (!this._connectionIsDoomed && this.Pool.UseLoadBalancing && DateTime.UtcNow.Ticks - this._createTime.Ticks > this.Pool.LoadBalanceTimeout.Ticks)
			{
				this.DoNotPoolThisConnection();
			}
			this.Deactivate();
		}

		// Token: 0x060004DE RID: 1246 RVA: 0x000676E8 File Offset: 0x00066AE8
		internal virtual void DelegatedTransactionEnded()
		{
			Bid.Trace("<prov.DbConnectionInternal.DelegatedTransactionEnded|RES|CPOOL> %d#, Delegated Transaction Completed.\n", this.ObjectID);
			if (1 != this._pooledCount)
			{
				if (-1 == this._pooledCount && !this._owningObject.IsAlive)
				{
					this.TerminateStasis(false);
					this.Deactivate();
					this.PerformanceCounters.NumberOfNonPooledConnections.Decrement();
					this.Dispose();
				}
				return;
			}
			this.TerminateStasis(true);
			this.Deactivate();
			DbConnectionPool pool = this.Pool;
			if (pool == null)
			{
				throw ADP.InternalError(ADP.InternalErrorCode.PooledObjectWithoutPool);
			}
			pool.PutObjectFromTransactedPool(this);
		}

		// Token: 0x060004DF RID: 1247 RVA: 0x00067770 File Offset: 0x00066B70
		public virtual void Dispose()
		{
			this._connectionPool = null;
			this._performanceCounters = null;
			this._connectionIsDoomed = true;
			this._enlistedTransaction = null;
		}

		// Token: 0x060004E0 RID: 1248 RVA: 0x0006779C File Offset: 0x00066B9C
		protected internal void DoNotPoolThisConnection()
		{
			this._cannotBePooled = true;
			Bid.PoolerTrace("<prov.DbConnectionInternal.DoNotPoolThisConnection|RES|INFO|CPOOL> %d#, Marking pooled object as non-poolable so it will be disposed\n", this.ObjectID);
		}

		// Token: 0x060004E1 RID: 1249 RVA: 0x000677C0 File Offset: 0x00066BC0
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		protected internal void DoomThisConnection()
		{
			this._connectionIsDoomed = true;
			Bid.PoolerTrace("<prov.DbConnectionInternal.DoomThisConnection|RES|INFO|CPOOL> %d#, Dooming\n", this.ObjectID);
		}

		// Token: 0x060004E2 RID: 1250
		public abstract void EnlistTransaction(Transaction transaction);

		// Token: 0x060004E3 RID: 1251 RVA: 0x000677E4 File Offset: 0x00066BE4
		protected internal virtual DataTable GetSchema(DbConnectionFactory factory, DbConnectionPoolGroup poolGroup, DbConnection outerConnection, string collectionName, string[] restrictions)
		{
			DbMetaDataFactory metaDataFactory = factory.GetMetaDataFactory(poolGroup, this);
			return metaDataFactory.GetSchema(outerConnection, collectionName, restrictions);
		}

		// Token: 0x060004E4 RID: 1252 RVA: 0x00067808 File Offset: 0x00066C08
		internal void MakeNonPooledObject(object owningObject, DbConnectionPoolCounters performanceCounters)
		{
			this._connectionPool = null;
			this._performanceCounters = performanceCounters;
			this._owningObject.Target = owningObject;
			this._pooledCount = -1;
		}

		// Token: 0x060004E5 RID: 1253 RVA: 0x00067838 File Offset: 0x00066C38
		internal void MakePooledConnection(DbConnectionPool connectionPool)
		{
			this._createTime = DateTime.UtcNow;
			this._connectionPool = connectionPool;
			this._performanceCounters = connectionPool.PerformanceCounters;
		}

		// Token: 0x060004E6 RID: 1254 RVA: 0x00067864 File Offset: 0x00066C64
		internal void NotifyWeakReference(int message)
		{
			DbReferenceCollection referenceCollection = this.ReferenceCollection;
			if (referenceCollection != null)
			{
				referenceCollection.Notify(message);
			}
		}

		// Token: 0x060004E7 RID: 1255 RVA: 0x00067884 File Offset: 0x00066C84
		internal virtual void OpenConnection(DbConnection outerConnection, DbConnectionFactory connectionFactory)
		{
			throw ADP.ConnectionAlreadyOpen(this.State);
		}

		// Token: 0x060004E8 RID: 1256 RVA: 0x0006789C File Offset: 0x00066C9C
		internal void PrePush(object expectedOwner)
		{
			if (expectedOwner == null)
			{
				if (this._owningObject.Target != null)
				{
					throw ADP.InternalError(ADP.InternalErrorCode.UnpooledObjectHasOwner);
				}
			}
			else if (this._owningObject.Target != expectedOwner)
			{
				throw ADP.InternalError(ADP.InternalErrorCode.UnpooledObjectHasWrongOwner);
			}
			if (this._pooledCount != 0)
			{
				throw ADP.InternalError(ADP.InternalErrorCode.PushingObjectSecondTime);
			}
			if (Bid.IsOn((Bid.ApiGroup)4096U))
			{
				Bid.PoolerTrace("<prov.DbConnectionInternal.PrePush|RES|CPOOL> %d#, Preparing to push into pool, owning connection %d#, pooledCount=%d\n", this.ObjectID, 0, this._pooledCount);
			}
			this._pooledCount++;
			this._owningObject.Target = null;
		}

		// Token: 0x060004E9 RID: 1257 RVA: 0x00067924 File Offset: 0x00066D24
		internal void PostPop(object newOwner)
		{
			if (this._owningObject.Target != null)
			{
				throw ADP.InternalError(ADP.InternalErrorCode.PooledObjectHasOwner);
			}
			this._owningObject.Target = newOwner;
			this._pooledCount--;
			if (Bid.IsOn((Bid.ApiGroup)4096U))
			{
				Bid.PoolerTrace("<prov.DbConnectionInternal.PostPop|RES|CPOOL> %d#, Preparing to pop from pool,  owning connection %d#, pooledCount=%d\n", this.ObjectID, 0, this._pooledCount);
			}
			if (this.Pool != null)
			{
				if (this._pooledCount != 0)
				{
					throw ADP.InternalError(ADP.InternalErrorCode.PooledObjectInPoolMoreThanOnce);
				}
			}
			else if (-1 != this._pooledCount)
			{
				throw ADP.InternalError(ADP.InternalErrorCode.NonPooledObjectUsedMoreThanOnce);
			}
		}

		// Token: 0x060004EA RID: 1258 RVA: 0x000679AC File Offset: 0x00066DAC
		internal void RemoveWeakReference(object value)
		{
			DbReferenceCollection referenceCollection = this.ReferenceCollection;
			if (referenceCollection != null)
			{
				referenceCollection.Remove(value);
			}
		}

		// Token: 0x060004EB RID: 1259 RVA: 0x000679CC File Offset: 0x00066DCC
		protected virtual void CleanupTransactionOnCompletion(Transaction transaction)
		{
		}

		// Token: 0x060004EC RID: 1260 RVA: 0x000679DC File Offset: 0x00066DDC
		internal void DetachTransaction(Transaction transaction)
		{
			Bid.Trace("<prov.DbConnectionInternal.DetachTransaction|RES|CPOOL> %d#, Transaction Completed. (pooledCount=%d)\n", this.ObjectID, this._pooledCount);
			lock (this)
			{
				DbConnection dbConnection = (DbConnection)this.Owner;
				if ((!this.RequireExplicitTransactionUnbind || dbConnection == null) && this._enlistedTransaction != null && transaction.Equals(this._enlistedTransaction))
				{
					this.EnlistedTransaction = null;
					if (this.IsTxRootWaitingForTxEnd)
					{
						this.DelegatedTransactionEnded();
					}
				}
			}
		}

		// Token: 0x060004ED RID: 1261 RVA: 0x00067A74 File Offset: 0x00066E74
		internal void CleanupConnectionOnTransactionCompletion(Transaction transaction)
		{
			this.DetachTransaction(transaction);
			DbConnectionPool pool = this.Pool;
			if (pool != null)
			{
				pool.TransactionEnded(transaction, this);
			}
		}

		// Token: 0x060004EE RID: 1262 RVA: 0x00067A9C File Offset: 0x00066E9C
		private void TransactionCompletedEvent(object sender, TransactionEventArgs e)
		{
			Transaction transaction = e.Transaction;
			Bid.Trace("<prov.DbConnectionInternal.TransactionCompletedEvent|RES|CPOOL> %d#, Transaction Completed. (pooledCount=%d)\n", this.ObjectID, this._pooledCount);
			this.CleanupTransactionOnCompletion(transaction);
			this.CleanupConnectionOnTransactionCompletion(transaction);
		}

		// Token: 0x060004EF RID: 1263 RVA: 0x00067AD4 File Offset: 0x00066ED4
		[SecurityPermission(SecurityAction.Assert, Flags = SecurityPermissionFlag.UnmanagedCode)]
		private void TransactionOutcomeEnlist(Transaction transaction)
		{
			transaction.TransactionCompleted += this.TransactionCompletedEvent;
		}

		// Token: 0x060004F0 RID: 1264 RVA: 0x00067AF4 File Offset: 0x00066EF4
		internal void SetInStasis()
		{
			this._isInStasis = true;
			Bid.PoolerTrace("<prov.DbConnectionInternal.SetInStasis|RES|CPOOL> %d#, Non-Pooled Connection has Delegated Transaction, waiting to Dispose.\n", this.ObjectID);
			this.PerformanceCounters.NumberOfStasisConnections.Increment();
		}

		// Token: 0x060004F1 RID: 1265 RVA: 0x00067B28 File Offset: 0x00066F28
		private void TerminateStasis(bool returningToPool)
		{
			if (returningToPool)
			{
				Bid.PoolerTrace("<prov.DbConnectionInternal.TerminateStasis|RES|CPOOL> %d#, Delegated Transaction has ended, connection is closed.  Returning to general pool.\n", this.ObjectID);
			}
			else
			{
				Bid.PoolerTrace("<prov.DbConnectionInternal.TerminateStasis|RES|CPOOL> %d#, Delegated Transaction has ended, connection is closed/leaked.  Disposing.\n", this.ObjectID);
			}
			this.PerformanceCounters.NumberOfStasisConnections.Decrement();
			this._isInStasis = false;
		}

		// Token: 0x0400042E RID: 1070
		private static int _objectTypeCount;

		// Token: 0x0400042F RID: 1071
		internal readonly int _objectID = Interlocked.Increment(ref DbConnectionInternal._objectTypeCount);

		// Token: 0x04000430 RID: 1072
		internal static readonly StateChangeEventArgs StateChangeClosed = new StateChangeEventArgs(ConnectionState.Open, ConnectionState.Closed);

		// Token: 0x04000431 RID: 1073
		internal static readonly StateChangeEventArgs StateChangeOpen = new StateChangeEventArgs(ConnectionState.Closed, ConnectionState.Open);

		// Token: 0x04000432 RID: 1074
		private readonly bool _allowSetConnectionString;

		// Token: 0x04000433 RID: 1075
		private readonly bool _hidePassword;

		// Token: 0x04000434 RID: 1076
		private readonly ConnectionState _state;

		// Token: 0x04000435 RID: 1077
		private readonly WeakReference _owningObject = new WeakReference(null, false);

		// Token: 0x04000436 RID: 1078
		private DbConnectionInternal _nextPooledObject;

		// Token: 0x04000437 RID: 1079
		private DbConnectionPool _connectionPool;

		// Token: 0x04000438 RID: 1080
		private DbConnectionPoolCounters _performanceCounters;

		// Token: 0x04000439 RID: 1081
		private DbReferenceCollection _referenceCollection;

		// Token: 0x0400043A RID: 1082
		private int _pooledCount;

		// Token: 0x0400043B RID: 1083
		private bool _connectionIsDoomed;

		// Token: 0x0400043C RID: 1084
		private bool _cannotBePooled;

		// Token: 0x0400043D RID: 1085
		private bool _isInStasis;

		// Token: 0x0400043E RID: 1086
		private DateTime _createTime;

		// Token: 0x0400043F RID: 1087
		private Transaction _enlistedTransaction;
	}
}
