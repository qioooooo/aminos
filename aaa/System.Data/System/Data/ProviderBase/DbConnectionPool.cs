using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security;
using System.Threading;
using System.Transactions;

namespace System.Data.ProviderBase
{
	// Token: 0x0200026D RID: 621
	internal sealed class DbConnectionPool
	{
		// Token: 0x0600210F RID: 8463 RVA: 0x00265378 File Offset: 0x00264778
		internal DbConnectionPool(DbConnectionFactory connectionFactory, DbConnectionPoolGroup connectionPoolGroup, DbConnectionPoolIdentity identity, DbConnectionPoolProviderInfo connectionPoolProviderInfo)
		{
			if (identity != null && identity.IsRestricted)
			{
				throw ADP.InternalError(ADP.InternalErrorCode.AttemptingToPoolOnRestrictedToken);
			}
			this._state = DbConnectionPool.State.Initializing;
			lock (DbConnectionPool._random)
			{
				this._cleanupWait = DbConnectionPool._random.Next(12, 24) * 10 * 1000;
			}
			this._connectionFactory = connectionFactory;
			this._connectionPoolGroup = connectionPoolGroup;
			this._connectionPoolGroupOptions = connectionPoolGroup.PoolGroupOptions;
			this._connectionPoolProviderInfo = connectionPoolProviderInfo;
			this._identity = identity;
			if (this.UseDeactivateQueue)
			{
				this._deactivateQueue = new Queue();
				this._deactivateCallback = new WaitCallback(this.ProcessDeactivateQueue);
			}
			this._waitHandles = new DbConnectionPool.PoolWaitHandles(new Semaphore(0, 1048576), new ManualResetEvent(false), new Semaphore(1, 1));
			this._errorWait = 5000;
			this._errorTimer = null;
			this._objectList = new List<DbConnectionInternal>(this.MaxPoolSize);
			if (ADP.IsPlatformNT5)
			{
				this._transactedConnectionPool = new DbConnectionPool.TransactedConnectionPool(this);
			}
			this._poolCreateRequest = new WaitCallback(this.PoolCreateRequest);
			this._state = DbConnectionPool.State.Running;
			Bid.PoolerTrace("<prov.DbConnectionPool.DbConnectionPool|RES|CPOOL> %d#, Constructed.\n", this.ObjectID);
		}

		// Token: 0x1700048E RID: 1166
		// (get) Token: 0x06002110 RID: 8464 RVA: 0x002654E8 File Offset: 0x002648E8
		private int CreationTimeout
		{
			get
			{
				return this.PoolGroupOptions.CreationTimeout;
			}
		}

		// Token: 0x1700048F RID: 1167
		// (get) Token: 0x06002111 RID: 8465 RVA: 0x00265500 File Offset: 0x00264900
		internal int Count
		{
			get
			{
				return this._totalObjects;
			}
		}

		// Token: 0x17000490 RID: 1168
		// (get) Token: 0x06002112 RID: 8466 RVA: 0x00265514 File Offset: 0x00264914
		internal DbConnectionFactory ConnectionFactory
		{
			get
			{
				return this._connectionFactory;
			}
		}

		// Token: 0x17000491 RID: 1169
		// (get) Token: 0x06002113 RID: 8467 RVA: 0x00265528 File Offset: 0x00264928
		internal bool ErrorOccurred
		{
			get
			{
				return this._errorOccurred;
			}
		}

		// Token: 0x17000492 RID: 1170
		// (get) Token: 0x06002114 RID: 8468 RVA: 0x00265540 File Offset: 0x00264940
		private bool HasTransactionAffinity
		{
			get
			{
				return this.PoolGroupOptions.HasTransactionAffinity;
			}
		}

		// Token: 0x17000493 RID: 1171
		// (get) Token: 0x06002115 RID: 8469 RVA: 0x00265558 File Offset: 0x00264958
		internal TimeSpan LoadBalanceTimeout
		{
			get
			{
				return this.PoolGroupOptions.LoadBalanceTimeout;
			}
		}

		// Token: 0x17000494 RID: 1172
		// (get) Token: 0x06002116 RID: 8470 RVA: 0x00265570 File Offset: 0x00264970
		private bool NeedToReplenish
		{
			get
			{
				if (DbConnectionPool.State.Running != this._state)
				{
					return false;
				}
				int count = this.Count;
				if (count >= this.MaxPoolSize)
				{
					return false;
				}
				if (count < this.MinPoolSize)
				{
					return true;
				}
				int num = this._stackNew.Count + this._stackOld.Count;
				int waitCount = this._waitCount;
				return num < waitCount || (num == waitCount && count > 1);
			}
		}

		// Token: 0x17000495 RID: 1173
		// (get) Token: 0x06002117 RID: 8471 RVA: 0x002655D8 File Offset: 0x002649D8
		internal DbConnectionPoolIdentity Identity
		{
			get
			{
				return this._identity;
			}
		}

		// Token: 0x17000496 RID: 1174
		// (get) Token: 0x06002118 RID: 8472 RVA: 0x002655EC File Offset: 0x002649EC
		internal bool IsRunning
		{
			get
			{
				return DbConnectionPool.State.Running == this._state;
			}
		}

		// Token: 0x17000497 RID: 1175
		// (get) Token: 0x06002119 RID: 8473 RVA: 0x00265604 File Offset: 0x00264A04
		private int MaxPoolSize
		{
			get
			{
				return this.PoolGroupOptions.MaxPoolSize;
			}
		}

		// Token: 0x17000498 RID: 1176
		// (get) Token: 0x0600211A RID: 8474 RVA: 0x0026561C File Offset: 0x00264A1C
		private int MinPoolSize
		{
			get
			{
				return this.PoolGroupOptions.MinPoolSize;
			}
		}

		// Token: 0x17000499 RID: 1177
		// (get) Token: 0x0600211B RID: 8475 RVA: 0x00265634 File Offset: 0x00264A34
		internal int ObjectID
		{
			get
			{
				return this._objectID;
			}
		}

		// Token: 0x1700049A RID: 1178
		// (get) Token: 0x0600211C RID: 8476 RVA: 0x00265648 File Offset: 0x00264A48
		internal DbConnectionPoolCounters PerformanceCounters
		{
			get
			{
				return this._connectionFactory.PerformanceCounters;
			}
		}

		// Token: 0x1700049B RID: 1179
		// (get) Token: 0x0600211D RID: 8477 RVA: 0x00265660 File Offset: 0x00264A60
		internal DbConnectionPoolGroup PoolGroup
		{
			get
			{
				return this._connectionPoolGroup;
			}
		}

		// Token: 0x1700049C RID: 1180
		// (get) Token: 0x0600211E RID: 8478 RVA: 0x00265674 File Offset: 0x00264A74
		internal DbConnectionPoolGroupOptions PoolGroupOptions
		{
			get
			{
				return this._connectionPoolGroupOptions;
			}
		}

		// Token: 0x1700049D RID: 1181
		// (get) Token: 0x0600211F RID: 8479 RVA: 0x00265688 File Offset: 0x00264A88
		internal DbConnectionPoolProviderInfo ProviderInfo
		{
			get
			{
				return this._connectionPoolProviderInfo;
			}
		}

		// Token: 0x1700049E RID: 1182
		// (get) Token: 0x06002120 RID: 8480 RVA: 0x0026569C File Offset: 0x00264A9C
		private bool UseDeactivateQueue
		{
			get
			{
				return this.PoolGroupOptions.UseDeactivateQueue;
			}
		}

		// Token: 0x1700049F RID: 1183
		// (get) Token: 0x06002121 RID: 8481 RVA: 0x002656B4 File Offset: 0x00264AB4
		internal bool UseLoadBalancing
		{
			get
			{
				return this.PoolGroupOptions.UseLoadBalancing;
			}
		}

		// Token: 0x170004A0 RID: 1184
		// (get) Token: 0x06002122 RID: 8482 RVA: 0x002656CC File Offset: 0x00264ACC
		private bool UsingIntegrateSecurity
		{
			get
			{
				return this._identity != null && DbConnectionPoolIdentity.NoIdentity != this._identity;
			}
		}

		// Token: 0x06002123 RID: 8483 RVA: 0x002656F4 File Offset: 0x00264AF4
		private void CleanupCallback(object state)
		{
			Bid.PoolerTrace("<prov.DbConnectionPool.CleanupCallback|RES|INFO|CPOOL> %d#\n", this.ObjectID);
			while (this.Count > this.MinPoolSize && this._waitHandles.PoolSemaphore.WaitOne(0, false))
			{
				DbConnectionInternal dbConnectionInternal = this._stackOld.SynchronizedPop();
				if (dbConnectionInternal == null)
				{
					this._waitHandles.PoolSemaphore.Release(1);
					break;
				}
				this.PerformanceCounters.NumberOfFreeConnections.Decrement();
				bool flag = true;
				lock (dbConnectionInternal)
				{
					if (dbConnectionInternal.IsTransactionRoot)
					{
						flag = false;
					}
				}
				if (flag)
				{
					this.DestroyObject(dbConnectionInternal);
				}
				else
				{
					dbConnectionInternal.SetInStasis();
				}
			}
			if (this._waitHandles.PoolSemaphore.WaitOne(0, false))
			{
				for (;;)
				{
					DbConnectionInternal dbConnectionInternal3 = this._stackNew.SynchronizedPop();
					if (dbConnectionInternal3 == null)
					{
						break;
					}
					Bid.PoolerTrace("<prov.DbConnectionPool.CleanupCallback|RES|INFO|CPOOL> %d#, ChangeStacks=%d#\n", this.ObjectID, dbConnectionInternal3.ObjectID);
					this._stackOld.SynchronizedPush(dbConnectionInternal3);
				}
				this._waitHandles.PoolSemaphore.Release(1);
			}
			this.QueuePoolCreateRequest();
		}

		// Token: 0x06002124 RID: 8484 RVA: 0x0026581C File Offset: 0x00264C1C
		internal void Clear()
		{
			Bid.PoolerTrace("<prov.DbConnectionPool.Clear|RES|CPOOL> %d#, Clearing.\n", this.ObjectID);
			DbConnectionInternal dbConnectionInternal;
			lock (this._objectList)
			{
				int count = this._objectList.Count;
				for (int i = 0; i < count; i++)
				{
					dbConnectionInternal = this._objectList[i];
					if (dbConnectionInternal != null)
					{
						dbConnectionInternal.DoNotPoolThisConnection();
					}
				}
				goto IL_006B;
			}
			IL_0054:
			this.PerformanceCounters.NumberOfFreeConnections.Decrement();
			this.DestroyObject(dbConnectionInternal);
			IL_006B:
			if ((dbConnectionInternal = this._stackNew.SynchronizedPop()) == null)
			{
				while ((dbConnectionInternal = this._stackOld.SynchronizedPop()) != null)
				{
					this.PerformanceCounters.NumberOfFreeConnections.Decrement();
					this.DestroyObject(dbConnectionInternal);
				}
				this.ReclaimEmancipatedObjects();
				Bid.PoolerTrace("<prov.DbConnectionPool.Clear|RES|CPOOL> %d#, Cleared.\n", this.ObjectID);
				return;
			}
			goto IL_0054;
		}

		// Token: 0x06002125 RID: 8485 RVA: 0x00265900 File Offset: 0x00264D00
		private Timer CreateCleanupTimer()
		{
			return new Timer(new TimerCallback(this.CleanupCallback), null, this._cleanupWait, this._cleanupWait);
		}

		// Token: 0x06002126 RID: 8486 RVA: 0x0026592C File Offset: 0x00264D2C
		private DbConnectionInternal CreateObject(DbConnection owningObject)
		{
			DbConnectionInternal dbConnectionInternal = null;
			try
			{
				dbConnectionInternal = this._connectionFactory.CreatePooledConnection(owningObject, this, this._connectionPoolGroup.ConnectionOptions);
				if (dbConnectionInternal == null)
				{
					throw ADP.InternalError(ADP.InternalErrorCode.CreateObjectReturnedNull);
				}
				if (!dbConnectionInternal.CanBePooled)
				{
					throw ADP.InternalError(ADP.InternalErrorCode.NewObjectCannotBePooled);
				}
				dbConnectionInternal.PrePush(null);
				lock (this._objectList)
				{
					this._objectList.Add(dbConnectionInternal);
					this._totalObjects = this._objectList.Count;
					this.PerformanceCounters.NumberOfPooledConnections.Increment();
				}
				Bid.PoolerTrace("<prov.DbConnectionPool.CreateObject|RES|CPOOL> %d#, Connection %d#, Added to pool.\n", this.ObjectID, dbConnectionInternal.ObjectID);
				this._errorWait = 5000;
			}
			catch (Exception ex)
			{
				if (!ADP.IsCatchableExceptionType(ex))
				{
					throw;
				}
				ADP.TraceExceptionForCapture(ex);
				dbConnectionInternal = null;
				this._resError = ex;
				this._waitHandles.ErrorEvent.Set();
				this._errorOccurred = true;
				this._errorTimer = new Timer(new TimerCallback(this.ErrorCallback), null, this._errorWait, this._errorWait);
				if (30000 < this._errorWait)
				{
					this._errorWait = 60000;
				}
				else
				{
					this._errorWait *= 2;
				}
				throw;
			}
			return dbConnectionInternal;
		}

		// Token: 0x06002127 RID: 8487 RVA: 0x00265A98 File Offset: 0x00264E98
		private void DeactivateObject(DbConnectionInternal obj)
		{
			Bid.PoolerTrace("<prov.DbConnectionPool.DeactivateObject|RES|CPOOL> %d#, Connection %d#, Deactivating.\n", this.ObjectID, obj.ObjectID);
			obj.DeactivateConnection();
			bool flag = false;
			bool flag2 = false;
			if (obj.IsConnectionDoomed)
			{
				flag2 = true;
			}
			else
			{
				lock (obj)
				{
					if (this._state == DbConnectionPool.State.ShuttingDown)
					{
						if (obj.IsTransactionRoot)
						{
							obj.SetInStasis();
						}
						else
						{
							flag2 = true;
						}
					}
					else if (obj.IsNonPoolableTransactionRoot)
					{
						obj.SetInStasis();
					}
					else if (obj.CanBePooled)
					{
						Transaction enlistedTransaction = obj.EnlistedTransaction;
						if (null != enlistedTransaction)
						{
							this._transactedConnectionPool.TransactionBegin(enlistedTransaction);
							this._transactedConnectionPool.PutTransactedObject(enlistedTransaction, obj);
						}
						else
						{
							flag = true;
						}
					}
					else if (obj.IsTransactionRoot && !obj.IsConnectionDoomed)
					{
						obj.SetInStasis();
					}
					else
					{
						flag2 = true;
					}
				}
			}
			if (flag)
			{
				this.PutNewObject(obj);
				return;
			}
			if (flag2)
			{
				this.DestroyObject(obj);
				this.QueuePoolCreateRequest();
			}
		}

		// Token: 0x06002128 RID: 8488 RVA: 0x00265B9C File Offset: 0x00264F9C
		private void DestroyObject(DbConnectionInternal obj)
		{
			if (obj.IsTxRootWaitingForTxEnd)
			{
				Bid.PoolerTrace("<prov.DbConnectionPool.DestroyObject|RES|CPOOL> %d#, Connection %d#, Has Delegated Transaction, waiting to Dispose.\n", this.ObjectID, obj.ObjectID);
				return;
			}
			Bid.PoolerTrace("<prov.DbConnectionPool.DestroyObject|RES|CPOOL> %d#, Connection %d#, Removing from pool.\n", this.ObjectID, obj.ObjectID);
			bool flag = false;
			lock (this._objectList)
			{
				flag = this._objectList.Remove(obj);
				this._totalObjects = this._objectList.Count;
			}
			if (flag)
			{
				Bid.PoolerTrace("<prov.DbConnectionPool.DestroyObject|RES|CPOOL> %d#, Connection %d#, Removed from pool.\n", this.ObjectID, obj.ObjectID);
				this.PerformanceCounters.NumberOfPooledConnections.Decrement();
			}
			obj.Dispose();
			Bid.PoolerTrace("<prov.DbConnectionPool.DestroyObject|RES|CPOOL> %d#, Connection %d#, Disposed.\n", this.ObjectID, obj.ObjectID);
			this.PerformanceCounters.HardDisconnectsPerSecond.Increment();
		}

		// Token: 0x06002129 RID: 8489 RVA: 0x00265C88 File Offset: 0x00265088
		private void ErrorCallback(object state)
		{
			Bid.PoolerTrace("<prov.DbConnectionPool.ErrorCallback|RES|CPOOL> %d#, Resetting Error handling.\n", this.ObjectID);
			this._errorOccurred = false;
			this._waitHandles.ErrorEvent.Reset();
			Timer errorTimer = this._errorTimer;
			this._errorTimer = null;
			if (errorTimer != null)
			{
				errorTimer.Dispose();
			}
		}

		// Token: 0x0600212A RID: 8490 RVA: 0x00265CD8 File Offset: 0x002650D8
		internal DbConnectionInternal GetConnection(DbConnection owningObject)
		{
			DbConnectionInternal dbConnectionInternal = null;
			Transaction transaction = null;
			this.PerformanceCounters.SoftConnectsPerSecond.Increment();
			if (this._state != DbConnectionPool.State.Running)
			{
				Bid.PoolerTrace("<prov.DbConnectionPool.GetConnection|RES|CPOOL> %d#, DbConnectionInternal State != Running.\n", this.ObjectID);
				return null;
			}
			Bid.PoolerTrace("<prov.DbConnectionPool.GetConnection|RES|CPOOL> %d#, Getting connection.\n", this.ObjectID);
			if (this.HasTransactionAffinity)
			{
				dbConnectionInternal = this.GetFromTransactedPool(out transaction);
			}
			if (dbConnectionInternal == null)
			{
				Interlocked.Increment(ref this._waitCount);
				uint num = 3U;
				uint creationTimeout = (uint)this.CreationTimeout;
				for (;;)
				{
					int num2 = 3;
					int num3 = 0;
					bool flag = false;
					RuntimeHelpers.PrepareConstrainedRegions();
					try
					{
						this._waitHandles.DangerousAddRef(ref flag);
						RuntimeHelpers.PrepareConstrainedRegions();
						try
						{
						}
						finally
						{
							num2 = SafeNativeMethods.WaitForMultipleObjectsEx(num, this._waitHandles.DangerousGetHandle(), false, creationTimeout, false);
						}
						int num4 = num2;
						switch (num4)
						{
						case -1:
							Bid.PoolerTrace("<prov.DbConnectionPool.GetConnection|RES|CPOOL> %d#, Wait failed.\n", this.ObjectID);
							Interlocked.Decrement(ref this._waitCount);
							Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());
							break;
						case 0:
							Interlocked.Decrement(ref this._waitCount);
							dbConnectionInternal = this.GetFromGeneralPool();
							goto IL_0291;
						case 1:
							Bid.PoolerTrace("<prov.DbConnectionPool.GetConnection|RES|CPOOL> %d#, Errors are set.\n", this.ObjectID);
							Interlocked.Decrement(ref this._waitCount);
							throw this._resError;
						case 2:
							Bid.PoolerTrace("<prov.DbConnectionPool.GetConnection|RES|CPOOL> %d#, Creating new connection.\n", this.ObjectID);
							try
							{
								dbConnectionInternal = this.UserCreateRequest(owningObject);
							}
							catch
							{
								if (dbConnectionInternal == null)
								{
									Interlocked.Decrement(ref this._waitCount);
								}
								throw;
							}
							finally
							{
								if (dbConnectionInternal != null)
								{
									Interlocked.Decrement(ref this._waitCount);
								}
							}
							if (dbConnectionInternal == null && this.Count >= this.MaxPoolSize && this.MaxPoolSize != 0 && !this.ReclaimEmancipatedObjects())
							{
								num = 2U;
								goto IL_0291;
							}
							goto IL_0291;
						default:
							switch (num4)
							{
							case 128:
								Bid.PoolerTrace("<prov.DbConnectionPool.GetConnection|RES|CPOOL> %d#, Semaphore handle abandonded.\n", this.ObjectID);
								Interlocked.Decrement(ref this._waitCount);
								throw new AbandonedMutexException(0, this._waitHandles.PoolSemaphore);
							case 129:
								Bid.PoolerTrace("<prov.DbConnectionPool.GetConnection|RES|CPOOL> %d#, Error handle abandonded.\n", this.ObjectID);
								Interlocked.Decrement(ref this._waitCount);
								throw new AbandonedMutexException(1, this._waitHandles.ErrorEvent);
							case 130:
								Bid.PoolerTrace("<prov.DbConnectionPool.GetConnection|RES|CPOOL> %d#, Creation handle abandoned.\n", this.ObjectID);
								Interlocked.Decrement(ref this._waitCount);
								throw new AbandonedMutexException(2, this._waitHandles.CreationSemaphore);
							default:
								if (num4 == 258)
								{
									Bid.PoolerTrace("<prov.DbConnectionPool.GetConnection|RES|CPOOL> %d#, Wait timed out.\n", this.ObjectID);
									Interlocked.Decrement(ref this._waitCount);
									return null;
								}
								break;
							}
							break;
						}
						Bid.PoolerTrace("<prov.DbConnectionPool.GetConnection|RES|CPOOL> %d#, WaitForMultipleObjects=%d\n", this.ObjectID, num2);
						Interlocked.Decrement(ref this._waitCount);
						throw ADP.InternalError(ADP.InternalErrorCode.UnexpectedWaitAnyResult);
						IL_0291:;
					}
					finally
					{
						if (2 == num2 && SafeNativeMethods.ReleaseSemaphore(this._waitHandles.CreationHandle.DangerousGetHandle(), 1, IntPtr.Zero) == 0)
						{
							num3 = Marshal.GetHRForLastWin32Error();
						}
						if (flag)
						{
							this._waitHandles.DangerousRelease();
						}
					}
					if (num3 != 0)
					{
						Marshal.ThrowExceptionForHR(num3);
					}
					if (dbConnectionInternal != null)
					{
						goto IL_02DD;
					}
				}
				DbConnectionInternal dbConnectionInternal2;
				return dbConnectionInternal2;
			}
			IL_02DD:
			if (dbConnectionInternal != null)
			{
				lock (dbConnectionInternal)
				{
					dbConnectionInternal.PostPop(owningObject);
				}
				try
				{
					dbConnectionInternal.ActivateConnection(transaction);
				}
				catch (SecurityException)
				{
					this.PutObject(dbConnectionInternal, owningObject);
					throw;
				}
			}
			return dbConnectionInternal;
		}

		// Token: 0x0600212B RID: 8491 RVA: 0x00266090 File Offset: 0x00265490
		private DbConnectionInternal GetFromGeneralPool()
		{
			DbConnectionInternal dbConnectionInternal = this._stackNew.SynchronizedPop();
			if (dbConnectionInternal == null)
			{
				dbConnectionInternal = this._stackOld.SynchronizedPop();
			}
			if (dbConnectionInternal != null)
			{
				Bid.PoolerTrace("<prov.DbConnectionPool.GetFromGeneralPool|RES|CPOOL> %d#, Connection %d#, Popped from general pool.\n", this.ObjectID, dbConnectionInternal.ObjectID);
				this.PerformanceCounters.NumberOfFreeConnections.Decrement();
			}
			return dbConnectionInternal;
		}

		// Token: 0x0600212C RID: 8492 RVA: 0x002660E4 File Offset: 0x002654E4
		private DbConnectionInternal GetFromTransactedPool(out Transaction transaction)
		{
			transaction = ADP.GetCurrentTransaction();
			DbConnectionInternal dbConnectionInternal = null;
			if (null != transaction && this._transactedConnectionPool != null)
			{
				dbConnectionInternal = this._transactedConnectionPool.GetTransactedObject(transaction);
				if (dbConnectionInternal != null)
				{
					Bid.PoolerTrace("<prov.DbConnectionPool.GetFromTransactedPool|RES|CPOOL> %d#, Connection %d#, Popped from transacted pool.\n", this.ObjectID, dbConnectionInternal.ObjectID);
					this.PerformanceCounters.NumberOfFreeConnections.Decrement();
				}
			}
			return dbConnectionInternal;
		}

		// Token: 0x0600212D RID: 8493 RVA: 0x00266144 File Offset: 0x00265544
		private void PoolCreateRequest(object state)
		{
			IntPtr intPtr;
			Bid.PoolerScopeEnter(out intPtr, "<prov.DbConnectionPool.PoolCreateRequest|RES|INFO|CPOOL> %d#\n", this.ObjectID);
			try
			{
				if (DbConnectionPool.State.Running == this._state)
				{
					this.ReclaimEmancipatedObjects();
					if (!this.ErrorOccurred && this.NeedToReplenish)
					{
						if (!this.UsingIntegrateSecurity || this._identity.Equals(DbConnectionPoolIdentity.GetCurrent()))
						{
							bool flag = false;
							int num = 3;
							uint creationTimeout = (uint)this.CreationTimeout;
							RuntimeHelpers.PrepareConstrainedRegions();
							try
							{
								this._waitHandles.DangerousAddRef(ref flag);
								RuntimeHelpers.PrepareConstrainedRegions();
								try
								{
								}
								finally
								{
									num = SafeNativeMethods.WaitForSingleObjectEx(this._waitHandles.CreationHandle.DangerousGetHandle(), creationTimeout, false);
								}
								if (num == 0)
								{
									if (!this.ErrorOccurred)
									{
										while (this.NeedToReplenish)
										{
											DbConnectionInternal dbConnectionInternal = this.CreateObject(null);
											if (dbConnectionInternal == null)
											{
												break;
											}
											this.PutNewObject(dbConnectionInternal);
										}
									}
								}
								else if (258 == num)
								{
									this.QueuePoolCreateRequest();
								}
								else
								{
									Bid.PoolerTrace("<prov.DbConnectionPool.PoolCreateRequest|RES|CPOOL> %d#, PoolCreateRequest called WaitForSingleObject failed %d", this.ObjectID, num);
								}
							}
							catch (Exception ex)
							{
								if (!ADP.IsCatchableExceptionType(ex))
								{
									throw;
								}
								Bid.PoolerTrace("<prov.DbConnectionPool.PoolCreateRequest|RES|CPOOL> %d#, PoolCreateRequest called CreateConnection which threw an exception: " + ex.ToString(), this.ObjectID);
							}
							finally
							{
								if (num == 0)
								{
									num = SafeNativeMethods.ReleaseSemaphore(this._waitHandles.CreationHandle.DangerousGetHandle(), 1, IntPtr.Zero);
								}
								if (flag)
								{
									this._waitHandles.DangerousRelease();
								}
							}
						}
					}
				}
			}
			finally
			{
				Bid.ScopeLeave(ref intPtr);
			}
		}

		// Token: 0x0600212E RID: 8494 RVA: 0x002662FC File Offset: 0x002656FC
		private void ProcessDeactivateQueue(object state)
		{
			IntPtr intPtr;
			Bid.PoolerScopeEnter(out intPtr, "<prov.DbConnectionPool.ProcessDeactivateQueue|RES|INFO|CPOOL> %d#\n", this.ObjectID);
			try
			{
				object[] array;
				lock (this._deactivateQueue.SyncRoot)
				{
					array = this._deactivateQueue.ToArray();
					this._deactivateQueue.Clear();
				}
				foreach (DbConnectionInternal dbConnectionInternal in array)
				{
					this.PerformanceCounters.NumberOfStasisConnections.Decrement();
					this.DeactivateObject(dbConnectionInternal);
				}
			}
			finally
			{
				Bid.ScopeLeave(ref intPtr);
			}
		}

		// Token: 0x0600212F RID: 8495 RVA: 0x002663C0 File Offset: 0x002657C0
		internal void PutNewObject(DbConnectionInternal obj)
		{
			Bid.PoolerTrace("<prov.DbConnectionPool.PutNewObject|RES|CPOOL> %d#, Connection %d#, Pushing to general pool.\n", this.ObjectID, obj.ObjectID);
			this._stackNew.SynchronizedPush(obj);
			this._waitHandles.PoolSemaphore.Release(1);
			this.PerformanceCounters.NumberOfFreeConnections.Increment();
		}

		// Token: 0x06002130 RID: 8496 RVA: 0x00266414 File Offset: 0x00265814
		internal void PutObject(DbConnectionInternal obj, object owningObject)
		{
			this.PerformanceCounters.SoftDisconnectsPerSecond.Increment();
			lock (obj)
			{
				obj.PrePush(owningObject);
			}
			if (this.UseDeactivateQueue)
			{
				Bid.PoolerTrace("<prov.DbConnectionPool.PutObject|RES|CPOOL> %d#, Connection %d#, Queueing for deactivation.\n", this.ObjectID, obj.ObjectID);
				this.PerformanceCounters.NumberOfStasisConnections.Increment();
				bool flag;
				lock (this._deactivateQueue.SyncRoot)
				{
					flag = 0 == this._deactivateQueue.Count;
					this._deactivateQueue.Enqueue(obj);
				}
				if (flag)
				{
					ThreadPool.QueueUserWorkItem(this._deactivateCallback, null);
					return;
				}
			}
			else
			{
				this.DeactivateObject(obj);
			}
		}

		// Token: 0x06002131 RID: 8497 RVA: 0x002664FC File Offset: 0x002658FC
		internal void PutObjectFromTransactedPool(DbConnectionInternal obj)
		{
			Bid.PoolerTrace("<prov.DbConnectionPool.PutObjectFromTransactedPool|RES|CPOOL> %d#, Connection %d#, Transaction has ended.\n", this.ObjectID, obj.ObjectID);
			if (this._state == DbConnectionPool.State.Running && obj.CanBePooled)
			{
				this.PutNewObject(obj);
				return;
			}
			this.DestroyObject(obj);
			this.QueuePoolCreateRequest();
		}

		// Token: 0x06002132 RID: 8498 RVA: 0x00266548 File Offset: 0x00265948
		private void QueuePoolCreateRequest()
		{
			if (DbConnectionPool.State.Running == this._state)
			{
				ThreadPool.QueueUserWorkItem(this._poolCreateRequest);
			}
		}

		// Token: 0x06002133 RID: 8499 RVA: 0x0026656C File Offset: 0x0026596C
		private bool ReclaimEmancipatedObjects()
		{
			bool flag = false;
			Bid.PoolerTrace("<prov.DbConnectionPool.ReclaimEmancipatedObjects|RES|CPOOL> %d#\n", this.ObjectID);
			List<DbConnectionInternal> list = new List<DbConnectionInternal>();
			int num;
			lock (this._objectList)
			{
				num = this._objectList.Count;
				for (int i = 0; i < num; i++)
				{
					DbConnectionInternal dbConnectionInternal = this._objectList[i];
					if (dbConnectionInternal != null)
					{
						bool flag2 = false;
						try
						{
							flag2 = Monitor.TryEnter(dbConnectionInternal);
							if (flag2 && dbConnectionInternal.IsEmancipated)
							{
								dbConnectionInternal.PrePush(null);
								list.Add(dbConnectionInternal);
							}
						}
						finally
						{
							if (flag2)
							{
								Monitor.Exit(dbConnectionInternal);
							}
						}
					}
				}
			}
			num = list.Count;
			for (int j = 0; j < num; j++)
			{
				DbConnectionInternal dbConnectionInternal2 = list[j];
				Bid.PoolerTrace("<prov.DbConnectionPool.ReclaimEmancipatedObjects|RES|CPOOL> %d#, Connection %d#, Reclaiming.\n", this.ObjectID, dbConnectionInternal2.ObjectID);
				this.PerformanceCounters.NumberOfReclaimedConnections.Increment();
				flag = true;
				this.DeactivateObject(dbConnectionInternal2);
			}
			return flag;
		}

		// Token: 0x06002134 RID: 8500 RVA: 0x0026668C File Offset: 0x00265A8C
		internal void Startup()
		{
			Bid.PoolerTrace("<prov.DbConnectionPool.Startup|RES|INFO|CPOOL> %d#, CleanupWait=%d\n", this.ObjectID, this._cleanupWait);
			this._cleanupTimer = this.CreateCleanupTimer();
			if (this.NeedToReplenish)
			{
				this.QueuePoolCreateRequest();
			}
		}

		// Token: 0x06002135 RID: 8501 RVA: 0x002666CC File Offset: 0x00265ACC
		internal void Shutdown()
		{
			Bid.PoolerTrace("<prov.DbConnectionPool.Shutdown|RES|INFO|CPOOL> %d#\n", this.ObjectID);
			this._state = DbConnectionPool.State.ShuttingDown;
			Timer timer = this._cleanupTimer;
			this._cleanupTimer = null;
			if (timer != null)
			{
				timer.Dispose();
			}
			timer = this._errorTimer;
			this._errorTimer = null;
			if (timer != null)
			{
				timer.Dispose();
			}
		}

		// Token: 0x06002136 RID: 8502 RVA: 0x00266720 File Offset: 0x00265B20
		internal void TransactionEnded(Transaction transaction, DbConnectionInternal transactedObject)
		{
			Bid.PoolerTrace("<prov.DbConnectionPool.TransactionEnded|RES|CPOOL> %d#, Transaction %d#, Connection %d#, Transaction Completed\n", this.ObjectID, transaction.GetHashCode(), transactedObject.ObjectID);
			DbConnectionPool.TransactedConnectionPool transactedConnectionPool = this._transactedConnectionPool;
			if (transactedConnectionPool != null)
			{
				transactedConnectionPool.TransactionEnded(transaction, transactedObject);
			}
		}

		// Token: 0x06002137 RID: 8503 RVA: 0x0026675C File Offset: 0x00265B5C
		private DbConnectionInternal UserCreateRequest(DbConnection owningObject)
		{
			DbConnectionInternal dbConnectionInternal = null;
			if (this.ErrorOccurred)
			{
				throw this._resError;
			}
			if ((this.Count < this.MaxPoolSize || this.MaxPoolSize == 0) && ((this.Count & 1) == 1 || !this.ReclaimEmancipatedObjects()))
			{
				dbConnectionInternal = this.CreateObject(owningObject);
			}
			return dbConnectionInternal;
		}

		// Token: 0x0400154A RID: 5450
		internal const Bid.ApiGroup PoolerTracePoints = Bid.ApiGroup.Pooling;

		// Token: 0x0400154B RID: 5451
		private const int MAX_Q_SIZE = 1048576;

		// Token: 0x0400154C RID: 5452
		private const int SEMAPHORE_HANDLE = 0;

		// Token: 0x0400154D RID: 5453
		private const int ERROR_HANDLE = 1;

		// Token: 0x0400154E RID: 5454
		private const int CREATION_HANDLE = 2;

		// Token: 0x0400154F RID: 5455
		private const int BOGUS_HANDLE = 3;

		// Token: 0x04001550 RID: 5456
		private const int WAIT_OBJECT_0 = 0;

		// Token: 0x04001551 RID: 5457
		private const int WAIT_TIMEOUT = 258;

		// Token: 0x04001552 RID: 5458
		private const int WAIT_ABANDONED = 128;

		// Token: 0x04001553 RID: 5459
		private const int WAIT_FAILED = -1;

		// Token: 0x04001554 RID: 5460
		private const int ERROR_WAIT_DEFAULT = 5000;

		// Token: 0x04001555 RID: 5461
		private static readonly Random _random = new Random(5101977);

		// Token: 0x04001556 RID: 5462
		private readonly int _cleanupWait;

		// Token: 0x04001557 RID: 5463
		private readonly DbConnectionPoolIdentity _identity;

		// Token: 0x04001558 RID: 5464
		private readonly DbConnectionFactory _connectionFactory;

		// Token: 0x04001559 RID: 5465
		private readonly DbConnectionPoolGroup _connectionPoolGroup;

		// Token: 0x0400155A RID: 5466
		private readonly DbConnectionPoolGroupOptions _connectionPoolGroupOptions;

		// Token: 0x0400155B RID: 5467
		private DbConnectionPoolProviderInfo _connectionPoolProviderInfo;

		// Token: 0x0400155C RID: 5468
		private DbConnectionPool.State _state;

		// Token: 0x0400155D RID: 5469
		private readonly DbConnectionPool.DbConnectionInternalListStack _stackOld = new DbConnectionPool.DbConnectionInternalListStack();

		// Token: 0x0400155E RID: 5470
		private readonly DbConnectionPool.DbConnectionInternalListStack _stackNew = new DbConnectionPool.DbConnectionInternalListStack();

		// Token: 0x0400155F RID: 5471
		private readonly WaitCallback _poolCreateRequest;

		// Token: 0x04001560 RID: 5472
		private readonly Queue _deactivateQueue;

		// Token: 0x04001561 RID: 5473
		private readonly WaitCallback _deactivateCallback;

		// Token: 0x04001562 RID: 5474
		private int _waitCount;

		// Token: 0x04001563 RID: 5475
		private readonly DbConnectionPool.PoolWaitHandles _waitHandles;

		// Token: 0x04001564 RID: 5476
		private Exception _resError;

		// Token: 0x04001565 RID: 5477
		private volatile bool _errorOccurred;

		// Token: 0x04001566 RID: 5478
		private int _errorWait;

		// Token: 0x04001567 RID: 5479
		private Timer _errorTimer;

		// Token: 0x04001568 RID: 5480
		private Timer _cleanupTimer;

		// Token: 0x04001569 RID: 5481
		private readonly DbConnectionPool.TransactedConnectionPool _transactedConnectionPool;

		// Token: 0x0400156A RID: 5482
		private readonly List<DbConnectionInternal> _objectList;

		// Token: 0x0400156B RID: 5483
		private int _totalObjects;

		// Token: 0x0400156C RID: 5484
		private static int _objectTypeCount;

		// Token: 0x0400156D RID: 5485
		internal readonly int _objectID = Interlocked.Increment(ref DbConnectionPool._objectTypeCount);

		// Token: 0x0200026E RID: 622
		private enum State
		{
			// Token: 0x0400156F RID: 5487
			Initializing,
			// Token: 0x04001570 RID: 5488
			Running,
			// Token: 0x04001571 RID: 5489
			ShuttingDown
		}

		// Token: 0x0200026F RID: 623
		private sealed class TransactedConnectionList : List<DbConnectionInternal>
		{
			// Token: 0x06002139 RID: 8505 RVA: 0x002667C8 File Offset: 0x00265BC8
			internal TransactedConnectionList(int initialAllocation, Transaction tx)
				: base(initialAllocation)
			{
				this._transaction = tx;
			}

			// Token: 0x0600213A RID: 8506 RVA: 0x002667E4 File Offset: 0x00265BE4
			internal void Dispose()
			{
				if (null != this._transaction)
				{
					this._transaction.Dispose();
				}
			}

			// Token: 0x04001572 RID: 5490
			private Transaction _transaction;
		}

		// Token: 0x02000270 RID: 624
		private sealed class TransactedConnectionPool : Hashtable
		{
			// Token: 0x0600213B RID: 8507 RVA: 0x0026680C File Offset: 0x00265C0C
			internal TransactedConnectionPool(DbConnectionPool pool)
			{
				this._pool = pool;
				Bid.PoolerTrace("<prov.DbConnectionPool.TransactedConnectionPool.TransactedConnectionPool|RES|CPOOL> %d#, Constructed for connection pool %d#\n", this.ObjectID, this._pool.ObjectID);
			}

			// Token: 0x170004A1 RID: 1185
			// (get) Token: 0x0600213C RID: 8508 RVA: 0x00266854 File Offset: 0x00265C54
			internal int ObjectID
			{
				get
				{
					return this._objectID;
				}
			}

			// Token: 0x170004A2 RID: 1186
			// (get) Token: 0x0600213D RID: 8509 RVA: 0x00266868 File Offset: 0x00265C68
			internal DbConnectionPool Pool
			{
				get
				{
					return this._pool;
				}
			}

			// Token: 0x0600213E RID: 8510 RVA: 0x0026687C File Offset: 0x00265C7C
			internal DbConnectionInternal GetTransactedObject(Transaction transaction)
			{
				DbConnectionInternal dbConnectionInternal = null;
				DbConnectionPool.TransactedConnectionList transactedConnectionList = (DbConnectionPool.TransactedConnectionList)this[transaction];
				if (transactedConnectionList != null)
				{
					lock (transactedConnectionList)
					{
						int num = transactedConnectionList.Count - 1;
						if (0 <= num)
						{
							dbConnectionInternal = transactedConnectionList[num];
							transactedConnectionList.RemoveAt(num);
						}
					}
				}
				if (dbConnectionInternal != null)
				{
					Bid.PoolerTrace("<prov.DbConnectionPool.TransactedConnectionPool.GetTransactedObject|RES|CPOOL> %d#, Transaction %d#, Connection %d#, Popped.\n", this.ObjectID, transaction.GetHashCode(), dbConnectionInternal.ObjectID);
				}
				return dbConnectionInternal;
			}

			// Token: 0x0600213F RID: 8511 RVA: 0x00266904 File Offset: 0x00265D04
			internal void PutTransactedObject(Transaction transaction, DbConnectionInternal transactedObject)
			{
				Bid.PoolerTrace("<prov.DbConnectionPool.TransactedConnectionPool.PutTransactedObject|RES|CPOOL> %d#, Transaction %d#, Connection %d#, Pushing.\n", this.ObjectID, transaction.GetHashCode(), transactedObject.ObjectID);
				DbConnectionPool.TransactedConnectionList transactedConnectionList = (DbConnectionPool.TransactedConnectionList)this[transaction];
				if (transactedConnectionList != null)
				{
					lock (transactedConnectionList)
					{
						transactedConnectionList.Add(transactedObject);
						this.Pool.PerformanceCounters.NumberOfFreeConnections.Increment();
					}
				}
			}

			// Token: 0x06002140 RID: 8512 RVA: 0x00266988 File Offset: 0x00265D88
			internal void TransactionBegin(Transaction transaction)
			{
				Bid.PoolerTrace("<prov.DbConnectionPool.TransactedConnectionPool.TransactionBegin|RES|CPOOL> %d#, Transaction %d#, Begin.\n", this.ObjectID, transaction.GetHashCode());
				if ((DbConnectionPool.TransactedConnectionList)this[transaction] == null)
				{
					Transaction transaction2 = null;
					try
					{
						transaction2 = transaction.Clone();
						DbConnectionPool.TransactedConnectionList transactedConnectionList = new DbConnectionPool.TransactedConnectionList(2, transaction2);
						Bid.PoolerTrace("<prov.DbConnectionPool.TransactedConnectionPool.TransactionBegin|RES|CPOOL> %d#, Transaction %d#, Adding List to transacted pool.\n", this.ObjectID, transaction.GetHashCode());
						lock (this)
						{
							if ((DbConnectionPool.TransactedConnectionList)this[transaction2] == null)
							{
								DbConnectionPool.TransactedConnectionList transactedConnectionList2 = transactedConnectionList;
								this.Add(transaction2, transactedConnectionList2);
								transaction2 = null;
							}
						}
					}
					finally
					{
						if (null != transaction2)
						{
							transaction2.Dispose();
						}
					}
					Bid.PoolerTrace("<prov.DbConnectionPool.TransactedConnectionPool.TransactionBegin|RES|CPOOL> %d#, Transaction %d#, Added.\n", this.ObjectID, transaction.GetHashCode());
				}
			}

			// Token: 0x06002141 RID: 8513 RVA: 0x00266A70 File Offset: 0x00265E70
			internal void TransactionEnded(Transaction transaction, DbConnectionInternal transactedObject)
			{
				Bid.PoolerTrace("<prov.DbConnectionPool.TransactedConnectionPool.TransactionEnded|RES|CPOOL> %d#, Transaction %d#, Connection %d#, Transaction Completed\n", this.ObjectID, transaction.GetHashCode(), transactedObject.ObjectID);
				DbConnectionPool.TransactedConnectionList transactedConnectionList = (DbConnectionPool.TransactedConnectionList)this[transaction];
				int num = -1;
				if (transactedConnectionList != null)
				{
					lock (transactedConnectionList)
					{
						num = transactedConnectionList.IndexOf(transactedObject);
						if (num >= 0)
						{
							transactedConnectionList.RemoveAt(num);
						}
						if (0 >= transactedConnectionList.Count)
						{
							Bid.PoolerTrace("<prov.DbConnectionPool.TransactedConnectionPool.TransactionEnded|RES|CPOOL> %d#, Transaction %d#, Removing List from transacted pool.\n", this.ObjectID, transaction.GetHashCode());
							lock (this)
							{
								this.Remove(transaction);
							}
							Bid.PoolerTrace("<prov.DbConnectionPool.TransactedConnectionPool.TransactionEnded|RES|CPOOL> %d#, Transaction %d#, Removed.\n", this.ObjectID, transaction.GetHashCode());
							transactedConnectionList.Dispose();
						}
					}
				}
				if (0 <= num)
				{
					this.Pool.PerformanceCounters.NumberOfFreeConnections.Decrement();
					this.Pool.PutObjectFromTransactedPool(transactedObject);
				}
			}

			// Token: 0x04001573 RID: 5491
			private DbConnectionPool _pool;

			// Token: 0x04001574 RID: 5492
			private static int _objectTypeCount;

			// Token: 0x04001575 RID: 5493
			internal readonly int _objectID = Interlocked.Increment(ref DbConnectionPool.TransactedConnectionPool._objectTypeCount);
		}

		// Token: 0x02000271 RID: 625
		private sealed class PoolWaitHandles : DbBuffer
		{
			// Token: 0x06002142 RID: 8514 RVA: 0x00266B80 File Offset: 0x00265F80
			internal PoolWaitHandles(Semaphore poolSemaphore, ManualResetEvent errorEvent, Semaphore creationSemaphore)
				: base(3 * IntPtr.Size)
			{
				bool flag = false;
				bool flag2 = false;
				bool flag3 = false;
				RuntimeHelpers.PrepareConstrainedRegions();
				try
				{
					this._poolSemaphore = poolSemaphore;
					this._errorEvent = errorEvent;
					this._creationSemaphore = creationSemaphore;
					this._poolHandle = poolSemaphore.SafeWaitHandle;
					this._errorHandle = errorEvent.SafeWaitHandle;
					this._creationHandle = creationSemaphore.SafeWaitHandle;
					this._poolHandle.DangerousAddRef(ref flag);
					this._errorHandle.DangerousAddRef(ref flag2);
					this._creationHandle.DangerousAddRef(ref flag3);
					int num = 0;
					int size = IntPtr.Size;
					base.WriteIntPtr(num, this._poolHandle.DangerousGetHandle());
					base.WriteIntPtr(IntPtr.Size, this._errorHandle.DangerousGetHandle());
					base.WriteIntPtr(2 * IntPtr.Size, this._creationHandle.DangerousGetHandle());
				}
				finally
				{
					if (flag)
					{
						this._releaseFlags |= 1;
					}
					if (flag2)
					{
						this._releaseFlags |= 2;
					}
					if (flag3)
					{
						this._releaseFlags |= 4;
					}
				}
			}

			// Token: 0x170004A3 RID: 1187
			// (get) Token: 0x06002143 RID: 8515 RVA: 0x00266CA0 File Offset: 0x002660A0
			internal SafeHandle CreationHandle
			{
				[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
				get
				{
					return this._creationHandle;
				}
			}

			// Token: 0x170004A4 RID: 1188
			// (get) Token: 0x06002144 RID: 8516 RVA: 0x00266CB4 File Offset: 0x002660B4
			internal Semaphore CreationSemaphore
			{
				get
				{
					return this._creationSemaphore;
				}
			}

			// Token: 0x170004A5 RID: 1189
			// (get) Token: 0x06002145 RID: 8517 RVA: 0x00266CC8 File Offset: 0x002660C8
			internal ManualResetEvent ErrorEvent
			{
				get
				{
					return this._errorEvent;
				}
			}

			// Token: 0x170004A6 RID: 1190
			// (get) Token: 0x06002146 RID: 8518 RVA: 0x00266CDC File Offset: 0x002660DC
			internal Semaphore PoolSemaphore
			{
				get
				{
					return this._poolSemaphore;
				}
			}

			// Token: 0x06002147 RID: 8519 RVA: 0x00266CF0 File Offset: 0x002660F0
			protected override bool ReleaseHandle()
			{
				if ((1 & this._releaseFlags) != 0)
				{
					this._poolHandle.DangerousRelease();
				}
				if ((2 & this._releaseFlags) != 0)
				{
					this._errorHandle.DangerousRelease();
				}
				if ((4 & this._releaseFlags) != 0)
				{
					this._creationHandle.DangerousRelease();
				}
				return base.ReleaseHandle();
			}

			// Token: 0x04001576 RID: 5494
			private readonly Semaphore _poolSemaphore;

			// Token: 0x04001577 RID: 5495
			private readonly ManualResetEvent _errorEvent;

			// Token: 0x04001578 RID: 5496
			private readonly Semaphore _creationSemaphore;

			// Token: 0x04001579 RID: 5497
			private readonly SafeHandle _poolHandle;

			// Token: 0x0400157A RID: 5498
			private readonly SafeHandle _errorHandle;

			// Token: 0x0400157B RID: 5499
			private readonly SafeHandle _creationHandle;

			// Token: 0x0400157C RID: 5500
			private readonly int _releaseFlags;
		}

		// Token: 0x02000272 RID: 626
		private class DbConnectionInternalListStack
		{
			// Token: 0x06002148 RID: 8520 RVA: 0x00266D44 File Offset: 0x00266144
			internal DbConnectionInternalListStack()
			{
			}

			// Token: 0x170004A7 RID: 1191
			// (get) Token: 0x06002149 RID: 8521 RVA: 0x00266D58 File Offset: 0x00266158
			internal int Count
			{
				get
				{
					int num = 0;
					lock (this)
					{
						for (DbConnectionInternal dbConnectionInternal = this._stack; dbConnectionInternal != null; dbConnectionInternal = dbConnectionInternal.NextPooledObject)
						{
							num++;
						}
					}
					return num;
				}
			}

			// Token: 0x0600214A RID: 8522 RVA: 0x00266DAC File Offset: 0x002661AC
			internal DbConnectionInternal SynchronizedPop()
			{
				DbConnectionInternal stack;
				lock (this)
				{
					stack = this._stack;
					if (stack != null)
					{
						this._stack = stack.NextPooledObject;
						stack.NextPooledObject = null;
					}
				}
				return stack;
			}

			// Token: 0x0600214B RID: 8523 RVA: 0x00266E04 File Offset: 0x00266204
			internal void SynchronizedPush(DbConnectionInternal value)
			{
				lock (this)
				{
					value.NextPooledObject = this._stack;
					this._stack = value;
				}
			}

			// Token: 0x0400157D RID: 5501
			private DbConnectionInternal _stack;
		}
	}
}
