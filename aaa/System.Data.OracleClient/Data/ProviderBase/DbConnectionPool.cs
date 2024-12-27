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
	// Token: 0x02000093 RID: 147
	internal sealed class DbConnectionPool
	{
		// Token: 0x060007FE RID: 2046 RVA: 0x00072E60 File Offset: 0x00072260
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

		// Token: 0x17000162 RID: 354
		// (get) Token: 0x060007FF RID: 2047 RVA: 0x00072FD0 File Offset: 0x000723D0
		private int CreationTimeout
		{
			get
			{
				return this.PoolGroupOptions.CreationTimeout;
			}
		}

		// Token: 0x17000163 RID: 355
		// (get) Token: 0x06000800 RID: 2048 RVA: 0x00072FE8 File Offset: 0x000723E8
		internal int Count
		{
			get
			{
				return this._totalObjects;
			}
		}

		// Token: 0x17000164 RID: 356
		// (get) Token: 0x06000801 RID: 2049 RVA: 0x00072FFC File Offset: 0x000723FC
		internal DbConnectionFactory ConnectionFactory
		{
			get
			{
				return this._connectionFactory;
			}
		}

		// Token: 0x17000165 RID: 357
		// (get) Token: 0x06000802 RID: 2050 RVA: 0x00073010 File Offset: 0x00072410
		internal bool ErrorOccurred
		{
			get
			{
				return this._errorOccurred;
			}
		}

		// Token: 0x17000166 RID: 358
		// (get) Token: 0x06000803 RID: 2051 RVA: 0x00073028 File Offset: 0x00072428
		private bool HasTransactionAffinity
		{
			get
			{
				return this.PoolGroupOptions.HasTransactionAffinity;
			}
		}

		// Token: 0x17000167 RID: 359
		// (get) Token: 0x06000804 RID: 2052 RVA: 0x00073040 File Offset: 0x00072440
		internal TimeSpan LoadBalanceTimeout
		{
			get
			{
				return this.PoolGroupOptions.LoadBalanceTimeout;
			}
		}

		// Token: 0x17000168 RID: 360
		// (get) Token: 0x06000805 RID: 2053 RVA: 0x00073058 File Offset: 0x00072458
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

		// Token: 0x17000169 RID: 361
		// (get) Token: 0x06000806 RID: 2054 RVA: 0x000730C0 File Offset: 0x000724C0
		internal DbConnectionPoolIdentity Identity
		{
			get
			{
				return this._identity;
			}
		}

		// Token: 0x1700016A RID: 362
		// (get) Token: 0x06000807 RID: 2055 RVA: 0x000730D4 File Offset: 0x000724D4
		internal bool IsRunning
		{
			get
			{
				return DbConnectionPool.State.Running == this._state;
			}
		}

		// Token: 0x1700016B RID: 363
		// (get) Token: 0x06000808 RID: 2056 RVA: 0x000730EC File Offset: 0x000724EC
		private int MaxPoolSize
		{
			get
			{
				return this.PoolGroupOptions.MaxPoolSize;
			}
		}

		// Token: 0x1700016C RID: 364
		// (get) Token: 0x06000809 RID: 2057 RVA: 0x00073104 File Offset: 0x00072504
		private int MinPoolSize
		{
			get
			{
				return this.PoolGroupOptions.MinPoolSize;
			}
		}

		// Token: 0x1700016D RID: 365
		// (get) Token: 0x0600080A RID: 2058 RVA: 0x0007311C File Offset: 0x0007251C
		internal int ObjectID
		{
			get
			{
				return this._objectID;
			}
		}

		// Token: 0x1700016E RID: 366
		// (get) Token: 0x0600080B RID: 2059 RVA: 0x00073130 File Offset: 0x00072530
		internal DbConnectionPoolCounters PerformanceCounters
		{
			get
			{
				return this._connectionFactory.PerformanceCounters;
			}
		}

		// Token: 0x1700016F RID: 367
		// (get) Token: 0x0600080C RID: 2060 RVA: 0x00073148 File Offset: 0x00072548
		internal DbConnectionPoolGroup PoolGroup
		{
			get
			{
				return this._connectionPoolGroup;
			}
		}

		// Token: 0x17000170 RID: 368
		// (get) Token: 0x0600080D RID: 2061 RVA: 0x0007315C File Offset: 0x0007255C
		internal DbConnectionPoolGroupOptions PoolGroupOptions
		{
			get
			{
				return this._connectionPoolGroupOptions;
			}
		}

		// Token: 0x17000171 RID: 369
		// (get) Token: 0x0600080E RID: 2062 RVA: 0x00073170 File Offset: 0x00072570
		internal DbConnectionPoolProviderInfo ProviderInfo
		{
			get
			{
				return this._connectionPoolProviderInfo;
			}
		}

		// Token: 0x17000172 RID: 370
		// (get) Token: 0x0600080F RID: 2063 RVA: 0x00073184 File Offset: 0x00072584
		private bool UseDeactivateQueue
		{
			get
			{
				return this.PoolGroupOptions.UseDeactivateQueue;
			}
		}

		// Token: 0x17000173 RID: 371
		// (get) Token: 0x06000810 RID: 2064 RVA: 0x0007319C File Offset: 0x0007259C
		internal bool UseLoadBalancing
		{
			get
			{
				return this.PoolGroupOptions.UseLoadBalancing;
			}
		}

		// Token: 0x17000174 RID: 372
		// (get) Token: 0x06000811 RID: 2065 RVA: 0x000731B4 File Offset: 0x000725B4
		private bool UsingIntegrateSecurity
		{
			get
			{
				return this._identity != null && DbConnectionPoolIdentity.NoIdentity != this._identity;
			}
		}

		// Token: 0x06000812 RID: 2066 RVA: 0x000731DC File Offset: 0x000725DC
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

		// Token: 0x06000813 RID: 2067 RVA: 0x00073304 File Offset: 0x00072704
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

		// Token: 0x06000814 RID: 2068 RVA: 0x000733E8 File Offset: 0x000727E8
		private Timer CreateCleanupTimer()
		{
			return new Timer(new TimerCallback(this.CleanupCallback), null, this._cleanupWait, this._cleanupWait);
		}

		// Token: 0x06000815 RID: 2069 RVA: 0x00073414 File Offset: 0x00072814
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

		// Token: 0x06000816 RID: 2070 RVA: 0x00073580 File Offset: 0x00072980
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

		// Token: 0x06000817 RID: 2071 RVA: 0x00073684 File Offset: 0x00072A84
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

		// Token: 0x06000818 RID: 2072 RVA: 0x00073770 File Offset: 0x00072B70
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

		// Token: 0x06000819 RID: 2073 RVA: 0x000737C0 File Offset: 0x00072BC0
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

		// Token: 0x0600081A RID: 2074 RVA: 0x00073B78 File Offset: 0x00072F78
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

		// Token: 0x0600081B RID: 2075 RVA: 0x00073BCC File Offset: 0x00072FCC
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

		// Token: 0x0600081C RID: 2076 RVA: 0x00073C2C File Offset: 0x0007302C
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

		// Token: 0x0600081D RID: 2077 RVA: 0x00073DE4 File Offset: 0x000731E4
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

		// Token: 0x0600081E RID: 2078 RVA: 0x00073EA8 File Offset: 0x000732A8
		internal void PutNewObject(DbConnectionInternal obj)
		{
			Bid.PoolerTrace("<prov.DbConnectionPool.PutNewObject|RES|CPOOL> %d#, Connection %d#, Pushing to general pool.\n", this.ObjectID, obj.ObjectID);
			this._stackNew.SynchronizedPush(obj);
			this._waitHandles.PoolSemaphore.Release(1);
			this.PerformanceCounters.NumberOfFreeConnections.Increment();
		}

		// Token: 0x0600081F RID: 2079 RVA: 0x00073EFC File Offset: 0x000732FC
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

		// Token: 0x06000820 RID: 2080 RVA: 0x00073FE4 File Offset: 0x000733E4
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

		// Token: 0x06000821 RID: 2081 RVA: 0x00074030 File Offset: 0x00073430
		private void QueuePoolCreateRequest()
		{
			if (DbConnectionPool.State.Running == this._state)
			{
				ThreadPool.QueueUserWorkItem(this._poolCreateRequest);
			}
		}

		// Token: 0x06000822 RID: 2082 RVA: 0x00074054 File Offset: 0x00073454
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

		// Token: 0x06000823 RID: 2083 RVA: 0x00074174 File Offset: 0x00073574
		internal void Startup()
		{
			Bid.PoolerTrace("<prov.DbConnectionPool.Startup|RES|INFO|CPOOL> %d#, CleanupWait=%d\n", this.ObjectID, this._cleanupWait);
			this._cleanupTimer = this.CreateCleanupTimer();
			if (this.NeedToReplenish)
			{
				this.QueuePoolCreateRequest();
			}
		}

		// Token: 0x06000824 RID: 2084 RVA: 0x000741B4 File Offset: 0x000735B4
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

		// Token: 0x06000825 RID: 2085 RVA: 0x00074208 File Offset: 0x00073608
		internal void TransactionEnded(Transaction transaction, DbConnectionInternal transactedObject)
		{
			Bid.PoolerTrace("<prov.DbConnectionPool.TransactionEnded|RES|CPOOL> %d#, Transaction %d#, Connection %d#, Transaction Completed\n", this.ObjectID, transaction.GetHashCode(), transactedObject.ObjectID);
			DbConnectionPool.TransactedConnectionPool transactedConnectionPool = this._transactedConnectionPool;
			if (transactedConnectionPool != null)
			{
				transactedConnectionPool.TransactionEnded(transaction, transactedObject);
			}
		}

		// Token: 0x06000826 RID: 2086 RVA: 0x00074244 File Offset: 0x00073644
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

		// Token: 0x04000513 RID: 1299
		internal const Bid.ApiGroup PoolerTracePoints = (Bid.ApiGroup)4096U;

		// Token: 0x04000514 RID: 1300
		private const int MAX_Q_SIZE = 1048576;

		// Token: 0x04000515 RID: 1301
		private const int SEMAPHORE_HANDLE = 0;

		// Token: 0x04000516 RID: 1302
		private const int ERROR_HANDLE = 1;

		// Token: 0x04000517 RID: 1303
		private const int CREATION_HANDLE = 2;

		// Token: 0x04000518 RID: 1304
		private const int BOGUS_HANDLE = 3;

		// Token: 0x04000519 RID: 1305
		private const int WAIT_OBJECT_0 = 0;

		// Token: 0x0400051A RID: 1306
		private const int WAIT_TIMEOUT = 258;

		// Token: 0x0400051B RID: 1307
		private const int WAIT_ABANDONED = 128;

		// Token: 0x0400051C RID: 1308
		private const int WAIT_FAILED = -1;

		// Token: 0x0400051D RID: 1309
		private const int ERROR_WAIT_DEFAULT = 5000;

		// Token: 0x0400051E RID: 1310
		private static readonly Random _random = new Random(5101977);

		// Token: 0x0400051F RID: 1311
		private readonly int _cleanupWait;

		// Token: 0x04000520 RID: 1312
		private readonly DbConnectionPoolIdentity _identity;

		// Token: 0x04000521 RID: 1313
		private readonly DbConnectionFactory _connectionFactory;

		// Token: 0x04000522 RID: 1314
		private readonly DbConnectionPoolGroup _connectionPoolGroup;

		// Token: 0x04000523 RID: 1315
		private readonly DbConnectionPoolGroupOptions _connectionPoolGroupOptions;

		// Token: 0x04000524 RID: 1316
		private DbConnectionPoolProviderInfo _connectionPoolProviderInfo;

		// Token: 0x04000525 RID: 1317
		private DbConnectionPool.State _state;

		// Token: 0x04000526 RID: 1318
		private readonly DbConnectionPool.DbConnectionInternalListStack _stackOld = new DbConnectionPool.DbConnectionInternalListStack();

		// Token: 0x04000527 RID: 1319
		private readonly DbConnectionPool.DbConnectionInternalListStack _stackNew = new DbConnectionPool.DbConnectionInternalListStack();

		// Token: 0x04000528 RID: 1320
		private readonly WaitCallback _poolCreateRequest;

		// Token: 0x04000529 RID: 1321
		private readonly Queue _deactivateQueue;

		// Token: 0x0400052A RID: 1322
		private readonly WaitCallback _deactivateCallback;

		// Token: 0x0400052B RID: 1323
		private int _waitCount;

		// Token: 0x0400052C RID: 1324
		private readonly DbConnectionPool.PoolWaitHandles _waitHandles;

		// Token: 0x0400052D RID: 1325
		private Exception _resError;

		// Token: 0x0400052E RID: 1326
		private volatile bool _errorOccurred;

		// Token: 0x0400052F RID: 1327
		private int _errorWait;

		// Token: 0x04000530 RID: 1328
		private Timer _errorTimer;

		// Token: 0x04000531 RID: 1329
		private Timer _cleanupTimer;

		// Token: 0x04000532 RID: 1330
		private readonly DbConnectionPool.TransactedConnectionPool _transactedConnectionPool;

		// Token: 0x04000533 RID: 1331
		private readonly List<DbConnectionInternal> _objectList;

		// Token: 0x04000534 RID: 1332
		private int _totalObjects;

		// Token: 0x04000535 RID: 1333
		private static int _objectTypeCount;

		// Token: 0x04000536 RID: 1334
		internal readonly int _objectID = Interlocked.Increment(ref DbConnectionPool._objectTypeCount);

		// Token: 0x02000094 RID: 148
		private enum State
		{
			// Token: 0x04000538 RID: 1336
			Initializing,
			// Token: 0x04000539 RID: 1337
			Running,
			// Token: 0x0400053A RID: 1338
			ShuttingDown
		}

		// Token: 0x02000095 RID: 149
		private sealed class TransactedConnectionList : List<DbConnectionInternal>
		{
			// Token: 0x06000828 RID: 2088 RVA: 0x000742B0 File Offset: 0x000736B0
			internal TransactedConnectionList(int initialAllocation, Transaction tx)
				: base(initialAllocation)
			{
				this._transaction = tx;
			}

			// Token: 0x06000829 RID: 2089 RVA: 0x000742CC File Offset: 0x000736CC
			internal void Dispose()
			{
				if (null != this._transaction)
				{
					this._transaction.Dispose();
				}
			}

			// Token: 0x0400053B RID: 1339
			private Transaction _transaction;
		}

		// Token: 0x02000096 RID: 150
		private sealed class TransactedConnectionPool : Hashtable
		{
			// Token: 0x0600082A RID: 2090 RVA: 0x000742F4 File Offset: 0x000736F4
			internal TransactedConnectionPool(DbConnectionPool pool)
			{
				this._pool = pool;
				Bid.PoolerTrace("<prov.DbConnectionPool.TransactedConnectionPool.TransactedConnectionPool|RES|CPOOL> %d#, Constructed for connection pool %d#\n", this.ObjectID, this._pool.ObjectID);
			}

			// Token: 0x17000175 RID: 373
			// (get) Token: 0x0600082B RID: 2091 RVA: 0x0007433C File Offset: 0x0007373C
			internal int ObjectID
			{
				get
				{
					return this._objectID;
				}
			}

			// Token: 0x17000176 RID: 374
			// (get) Token: 0x0600082C RID: 2092 RVA: 0x00074350 File Offset: 0x00073750
			internal DbConnectionPool Pool
			{
				get
				{
					return this._pool;
				}
			}

			// Token: 0x0600082D RID: 2093 RVA: 0x00074364 File Offset: 0x00073764
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

			// Token: 0x0600082E RID: 2094 RVA: 0x000743EC File Offset: 0x000737EC
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

			// Token: 0x0600082F RID: 2095 RVA: 0x00074470 File Offset: 0x00073870
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

			// Token: 0x06000830 RID: 2096 RVA: 0x00074558 File Offset: 0x00073958
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

			// Token: 0x0400053C RID: 1340
			private DbConnectionPool _pool;

			// Token: 0x0400053D RID: 1341
			private static int _objectTypeCount;

			// Token: 0x0400053E RID: 1342
			internal readonly int _objectID = Interlocked.Increment(ref DbConnectionPool.TransactedConnectionPool._objectTypeCount);
		}

		// Token: 0x02000097 RID: 151
		private sealed class PoolWaitHandles : DbBuffer
		{
			// Token: 0x06000831 RID: 2097 RVA: 0x00074668 File Offset: 0x00073A68
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

			// Token: 0x17000177 RID: 375
			// (get) Token: 0x06000832 RID: 2098 RVA: 0x00074788 File Offset: 0x00073B88
			internal SafeHandle CreationHandle
			{
				[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
				get
				{
					return this._creationHandle;
				}
			}

			// Token: 0x17000178 RID: 376
			// (get) Token: 0x06000833 RID: 2099 RVA: 0x0007479C File Offset: 0x00073B9C
			internal Semaphore CreationSemaphore
			{
				get
				{
					return this._creationSemaphore;
				}
			}

			// Token: 0x17000179 RID: 377
			// (get) Token: 0x06000834 RID: 2100 RVA: 0x000747B0 File Offset: 0x00073BB0
			internal ManualResetEvent ErrorEvent
			{
				get
				{
					return this._errorEvent;
				}
			}

			// Token: 0x1700017A RID: 378
			// (get) Token: 0x06000835 RID: 2101 RVA: 0x000747C4 File Offset: 0x00073BC4
			internal Semaphore PoolSemaphore
			{
				get
				{
					return this._poolSemaphore;
				}
			}

			// Token: 0x06000836 RID: 2102 RVA: 0x000747D8 File Offset: 0x00073BD8
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

			// Token: 0x0400053F RID: 1343
			private readonly Semaphore _poolSemaphore;

			// Token: 0x04000540 RID: 1344
			private readonly ManualResetEvent _errorEvent;

			// Token: 0x04000541 RID: 1345
			private readonly Semaphore _creationSemaphore;

			// Token: 0x04000542 RID: 1346
			private readonly SafeHandle _poolHandle;

			// Token: 0x04000543 RID: 1347
			private readonly SafeHandle _errorHandle;

			// Token: 0x04000544 RID: 1348
			private readonly SafeHandle _creationHandle;

			// Token: 0x04000545 RID: 1349
			private readonly int _releaseFlags;
		}

		// Token: 0x02000098 RID: 152
		private class DbConnectionInternalListStack
		{
			// Token: 0x06000837 RID: 2103 RVA: 0x0007482C File Offset: 0x00073C2C
			internal DbConnectionInternalListStack()
			{
			}

			// Token: 0x1700017B RID: 379
			// (get) Token: 0x06000838 RID: 2104 RVA: 0x00074840 File Offset: 0x00073C40
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

			// Token: 0x06000839 RID: 2105 RVA: 0x00074894 File Offset: 0x00073C94
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

			// Token: 0x0600083A RID: 2106 RVA: 0x000748EC File Offset: 0x00073CEC
			internal void SynchronizedPush(DbConnectionInternal value)
			{
				lock (this)
				{
					value.NextPooledObject = this._stack;
					this._stack = value;
				}
			}

			// Token: 0x04000546 RID: 1350
			private DbConnectionInternal _stack;
		}
	}
}
