using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading;

namespace System.Data.ProviderBase
{
	// Token: 0x020001D7 RID: 471
	internal abstract class DbConnectionFactory
	{
		// Token: 0x06001A36 RID: 6710 RVA: 0x002423F0 File Offset: 0x002417F0
		protected DbConnectionFactory()
			: this(DbConnectionPoolCountersNoCounters.SingletonInstance)
		{
		}

		// Token: 0x06001A37 RID: 6711 RVA: 0x00242408 File Offset: 0x00241808
		protected DbConnectionFactory(DbConnectionPoolCounters performanceCounters)
		{
			this._performanceCounters = performanceCounters;
			this._connectionPoolGroups = new Dictionary<string, DbConnectionPoolGroup>();
			this._poolsToRelease = new List<DbConnectionPool>();
			this._poolGroupsToRelease = new List<DbConnectionPoolGroup>();
			this._pruningTimer = this.CreatePruningTimer();
		}

		// Token: 0x1700035C RID: 860
		// (get) Token: 0x06001A38 RID: 6712 RVA: 0x00242460 File Offset: 0x00241860
		internal DbConnectionPoolCounters PerformanceCounters
		{
			get
			{
				return this._performanceCounters;
			}
		}

		// Token: 0x1700035D RID: 861
		// (get) Token: 0x06001A39 RID: 6713
		public abstract DbProviderFactory ProviderFactory { get; }

		// Token: 0x1700035E RID: 862
		// (get) Token: 0x06001A3A RID: 6714 RVA: 0x00242474 File Offset: 0x00241874
		internal int ObjectID
		{
			get
			{
				return this._objectID;
			}
		}

		// Token: 0x06001A3B RID: 6715 RVA: 0x00242488 File Offset: 0x00241888
		public void ClearAllPools()
		{
			IntPtr intPtr;
			Bid.ScopeEnter(out intPtr, "<prov.DbConnectionFactory.ClearAllPools|API> ");
			try
			{
				Dictionary<string, DbConnectionPoolGroup> connectionPoolGroups = this._connectionPoolGroups;
				foreach (KeyValuePair<string, DbConnectionPoolGroup> keyValuePair in connectionPoolGroups)
				{
					DbConnectionPoolGroup value = keyValuePair.Value;
					if (value != null)
					{
						value.Clear();
					}
				}
			}
			finally
			{
				Bid.ScopeLeave(ref intPtr);
			}
		}

		// Token: 0x06001A3C RID: 6716 RVA: 0x00242524 File Offset: 0x00241924
		public void ClearPool(DbConnection connection)
		{
			ADP.CheckArgumentNull(connection, "connection");
			IntPtr intPtr;
			Bid.ScopeEnter(out intPtr, "<prov.DbConnectionFactory.ClearPool|API> %d#", this.GetObjectId(connection));
			try
			{
				DbConnectionPoolGroup connectionPoolGroup = this.GetConnectionPoolGroup(connection);
				if (connectionPoolGroup != null)
				{
					connectionPoolGroup.Clear();
				}
			}
			finally
			{
				Bid.ScopeLeave(ref intPtr);
			}
		}

		// Token: 0x06001A3D RID: 6717 RVA: 0x00242588 File Offset: 0x00241988
		public void ClearPool(string connectionString)
		{
			ADP.CheckArgumentNull(connectionString, "connectionString");
			IntPtr intPtr;
			Bid.ScopeEnter(out intPtr, "<prov.DbConnectionFactory.ClearPool|API> connectionString");
			try
			{
				Dictionary<string, DbConnectionPoolGroup> connectionPoolGroups = this._connectionPoolGroups;
				DbConnectionPoolGroup dbConnectionPoolGroup;
				if (connectionPoolGroups.TryGetValue(connectionString, out dbConnectionPoolGroup))
				{
					dbConnectionPoolGroup.Clear();
				}
			}
			finally
			{
				Bid.ScopeLeave(ref intPtr);
			}
		}

		// Token: 0x06001A3E RID: 6718 RVA: 0x002425EC File Offset: 0x002419EC
		internal virtual DbConnectionPoolProviderInfo CreateConnectionPoolProviderInfo(DbConnectionOptions connectionOptions)
		{
			return null;
		}

		// Token: 0x06001A3F RID: 6719 RVA: 0x002425FC File Offset: 0x002419FC
		protected virtual DbMetaDataFactory CreateMetaDataFactory(DbConnectionInternal internalConnection, out bool cacheMetaDataFactory)
		{
			cacheMetaDataFactory = false;
			throw ADP.NotSupported();
		}

		// Token: 0x06001A40 RID: 6720 RVA: 0x00242614 File Offset: 0x00241A14
		internal DbConnectionInternal CreateNonPooledConnection(DbConnection owningConnection, DbConnectionPoolGroup poolGroup)
		{
			DbConnectionOptions connectionOptions = poolGroup.ConnectionOptions;
			DbConnectionPoolGroupProviderInfo providerInfo = poolGroup.ProviderInfo;
			DbConnectionInternal dbConnectionInternal = this.CreateConnection(connectionOptions, providerInfo, null, owningConnection);
			if (dbConnectionInternal != null)
			{
				this.PerformanceCounters.HardConnectsPerSecond.Increment();
				dbConnectionInternal.MakeNonPooledObject(owningConnection, this.PerformanceCounters);
			}
			Bid.Trace("<prov.DbConnectionFactory.CreateNonPooledConnection|RES|CPOOL> %d#, Non-pooled database connection created.\n", this.ObjectID);
			return dbConnectionInternal;
		}

		// Token: 0x06001A41 RID: 6721 RVA: 0x0024266C File Offset: 0x00241A6C
		internal DbConnectionInternal CreatePooledConnection(DbConnection owningConnection, DbConnectionPool pool, DbConnectionOptions options)
		{
			DbConnectionPoolGroupProviderInfo providerInfo = pool.PoolGroup.ProviderInfo;
			DbConnectionInternal dbConnectionInternal = this.CreateConnection(options, providerInfo, pool, owningConnection);
			if (dbConnectionInternal != null)
			{
				this.PerformanceCounters.HardConnectsPerSecond.Increment();
				dbConnectionInternal.MakePooledConnection(pool);
			}
			Bid.Trace("<prov.DbConnectionFactory.CreatePooledConnection|RES|CPOOL> %d#, Pooled database connection created.\n", this.ObjectID);
			return dbConnectionInternal;
		}

		// Token: 0x06001A42 RID: 6722 RVA: 0x002426BC File Offset: 0x00241ABC
		internal virtual DbConnectionPoolGroupProviderInfo CreateConnectionPoolGroupProviderInfo(DbConnectionOptions connectionOptions)
		{
			return null;
		}

		// Token: 0x06001A43 RID: 6723 RVA: 0x002426CC File Offset: 0x00241ACC
		private Timer CreatePruningTimer()
		{
			TimerCallback timerCallback = new TimerCallback(this.PruneConnectionPoolGroups);
			return new Timer(timerCallback, null, 240000, 30000);
		}

		// Token: 0x06001A44 RID: 6724 RVA: 0x002426F8 File Offset: 0x00241AF8
		protected DbConnectionOptions FindConnectionOptions(string connectionString)
		{
			if (!ADP.IsEmpty(connectionString))
			{
				Dictionary<string, DbConnectionPoolGroup> connectionPoolGroups = this._connectionPoolGroups;
				DbConnectionPoolGroup dbConnectionPoolGroup;
				if (connectionPoolGroups.TryGetValue(connectionString, out dbConnectionPoolGroup))
				{
					return dbConnectionPoolGroup.ConnectionOptions;
				}
			}
			return null;
		}

		// Token: 0x06001A45 RID: 6725 RVA: 0x00242728 File Offset: 0x00241B28
		internal DbConnectionInternal GetConnection(DbConnection owningConnection)
		{
			int num = 5;
			DbConnectionInternal dbConnectionInternal;
			for (;;)
			{
				DbConnectionPoolGroup dbConnectionPoolGroup = this.GetConnectionPoolGroup(owningConnection);
				DbConnectionPool connectionPool = this.GetConnectionPool(owningConnection, dbConnectionPoolGroup);
				if (connectionPool == null)
				{
					dbConnectionPoolGroup = this.GetConnectionPoolGroup(owningConnection);
					dbConnectionInternal = this.CreateNonPooledConnection(owningConnection, dbConnectionPoolGroup);
					this.PerformanceCounters.NumberOfNonPooledConnections.Increment();
				}
				else
				{
					dbConnectionInternal = connectionPool.GetConnection(owningConnection);
					if (dbConnectionInternal != null)
					{
						goto IL_0073;
					}
					if (connectionPool.IsRunning)
					{
						break;
					}
					Thread.Sleep(1);
				}
				if (dbConnectionInternal != null || num-- <= 0)
				{
					goto IL_0073;
				}
			}
			Bid.Trace("<prov.DbConnectionFactory.GetConnection|RES|CPOOL> %d#, GetConnection failed because a pool timeout occurred.\n", this.ObjectID);
			throw ADP.PooledOpenTimeout();
			IL_0073:
			if (dbConnectionInternal == null)
			{
				Bid.Trace("<prov.DbConnectionFactory.GetConnection|RES|CPOOL> %d#, GetConnection failed because a pool timeout occurred and all retries were exhausted.\n", this.ObjectID);
				throw ADP.PooledOpenTimeout();
			}
			return dbConnectionInternal;
		}

		// Token: 0x06001A46 RID: 6726 RVA: 0x002427C4 File Offset: 0x00241BC4
		private DbConnectionPool GetConnectionPool(DbConnection owningObject, DbConnectionPoolGroup connectionPoolGroup)
		{
			if (connectionPoolGroup.IsDisabled && connectionPoolGroup.PoolGroupOptions != null)
			{
				Bid.Trace("<prov.DbConnectionFactory.GetConnectionPool|RES|INFO|CPOOL> %d#, DisabledPoolGroup=%d#\n", this.ObjectID, connectionPoolGroup.ObjectID);
				DbConnectionPoolGroupOptions poolGroupOptions = connectionPoolGroup.PoolGroupOptions;
				DbConnectionOptions connectionOptions = connectionPoolGroup.ConnectionOptions;
				string text = connectionOptions.UsersConnectionString(false);
				connectionPoolGroup = this.GetConnectionPoolGroup(text, poolGroupOptions, ref connectionOptions);
				this.SetConnectionPoolGroup(owningObject, connectionPoolGroup);
			}
			return connectionPoolGroup.GetConnectionPool(this);
		}

		// Token: 0x06001A47 RID: 6727 RVA: 0x0024282C File Offset: 0x00241C2C
		internal DbConnectionPoolGroup GetConnectionPoolGroup(string connectionString, DbConnectionPoolGroupOptions poolOptions, ref DbConnectionOptions userConnectionOptions)
		{
			if (ADP.IsEmpty(connectionString))
			{
				return null;
			}
			Dictionary<string, DbConnectionPoolGroup> dictionary = this._connectionPoolGroups;
			DbConnectionPoolGroup dbConnectionPoolGroup;
			if (!dictionary.TryGetValue(connectionString, out dbConnectionPoolGroup) || (dbConnectionPoolGroup.IsDisabled && dbConnectionPoolGroup.PoolGroupOptions != null))
			{
				DbConnectionOptions dbConnectionOptions = this.CreateConnectionOptions(connectionString, userConnectionOptions);
				if (dbConnectionOptions == null)
				{
					throw ADP.InternalConnectionError(ADP.ConnectionError.ConnectionOptionsMissing);
				}
				string text = connectionString;
				if (userConnectionOptions == null)
				{
					userConnectionOptions = dbConnectionOptions;
					text = dbConnectionOptions.Expand();
					if (text != connectionString)
					{
						return this.GetConnectionPoolGroup(text, null, ref userConnectionOptions);
					}
				}
				if (poolOptions == null && ADP.IsWindowsNT)
				{
					if (dbConnectionPoolGroup != null)
					{
						poolOptions = dbConnectionPoolGroup.PoolGroupOptions;
					}
					else
					{
						poolOptions = this.CreateConnectionPoolGroupOptions(dbConnectionOptions);
					}
				}
				DbConnectionPoolGroup dbConnectionPoolGroup2 = new DbConnectionPoolGroup(dbConnectionOptions, poolOptions);
				dbConnectionPoolGroup2.ProviderInfo = this.CreateConnectionPoolGroupProviderInfo(dbConnectionOptions);
				lock (this)
				{
					dictionary = this._connectionPoolGroups;
					if (!dictionary.TryGetValue(text, out dbConnectionPoolGroup))
					{
						Dictionary<string, DbConnectionPoolGroup> dictionary2 = new Dictionary<string, DbConnectionPoolGroup>(1 + dictionary.Count);
						foreach (KeyValuePair<string, DbConnectionPoolGroup> keyValuePair in dictionary)
						{
							dictionary2.Add(keyValuePair.Key, keyValuePair.Value);
						}
						dictionary2.Add(text, dbConnectionPoolGroup2);
						this.PerformanceCounters.NumberOfActiveConnectionPoolGroups.Increment();
						dbConnectionPoolGroup = dbConnectionPoolGroup2;
						this._connectionPoolGroups = dictionary2;
					}
					return dbConnectionPoolGroup;
				}
			}
			if (userConnectionOptions == null)
			{
				userConnectionOptions = dbConnectionPoolGroup.ConnectionOptions;
			}
			return dbConnectionPoolGroup;
		}

		// Token: 0x06001A48 RID: 6728 RVA: 0x002429B4 File Offset: 0x00241DB4
		internal DbMetaDataFactory GetMetaDataFactory(DbConnectionPoolGroup connectionPoolGroup, DbConnectionInternal internalConnection)
		{
			DbMetaDataFactory dbMetaDataFactory = connectionPoolGroup.MetaDataFactory;
			if (dbMetaDataFactory == null)
			{
				bool flag = false;
				dbMetaDataFactory = this.CreateMetaDataFactory(internalConnection, out flag);
				if (flag)
				{
					connectionPoolGroup.MetaDataFactory = dbMetaDataFactory;
				}
			}
			return dbMetaDataFactory;
		}

		// Token: 0x06001A49 RID: 6729 RVA: 0x002429E4 File Offset: 0x00241DE4
		private void PruneConnectionPoolGroups(object state)
		{
			if (Bid.AdvancedOn)
			{
				Bid.Trace("<prov.DbConnectionFactory.PruneConnectionPoolGroups|RES|INFO|CPOOL> %d#\n", this.ObjectID);
			}
			lock (this._poolsToRelease)
			{
				if (this._poolsToRelease.Count != 0)
				{
					DbConnectionPool[] array = this._poolsToRelease.ToArray();
					foreach (DbConnectionPool dbConnectionPool in array)
					{
						if (dbConnectionPool != null)
						{
							dbConnectionPool.Clear();
							if (dbConnectionPool.Count == 0)
							{
								this._poolsToRelease.Remove(dbConnectionPool);
								if (Bid.AdvancedOn)
								{
									Bid.Trace("<prov.DbConnectionFactory.PruneConnectionPoolGroups|RES|INFO|CPOOL> %d#, ReleasePool=%d#\n", this.ObjectID, dbConnectionPool.ObjectID);
								}
								this.PerformanceCounters.NumberOfInactiveConnectionPools.Decrement();
							}
						}
					}
				}
			}
			lock (this._poolGroupsToRelease)
			{
				if (this._poolGroupsToRelease.Count != 0)
				{
					DbConnectionPoolGroup[] array3 = this._poolGroupsToRelease.ToArray();
					foreach (DbConnectionPoolGroup dbConnectionPoolGroup in array3)
					{
						if (dbConnectionPoolGroup != null)
						{
							dbConnectionPoolGroup.Clear();
							if (dbConnectionPoolGroup.Count == 0)
							{
								this._poolGroupsToRelease.Remove(dbConnectionPoolGroup);
								if (Bid.AdvancedOn)
								{
									Bid.Trace("<prov.DbConnectionFactory.PruneConnectionPoolGroups|RES|INFO|CPOOL> %d#, ReleasePoolGroup=%d#\n", this.ObjectID, dbConnectionPoolGroup.ObjectID);
								}
								this.PerformanceCounters.NumberOfInactiveConnectionPoolGroups.Decrement();
							}
						}
					}
				}
			}
			lock (this)
			{
				Dictionary<string, DbConnectionPoolGroup> connectionPoolGroups = this._connectionPoolGroups;
				Dictionary<string, DbConnectionPoolGroup> dictionary = new Dictionary<string, DbConnectionPoolGroup>(connectionPoolGroups.Count);
				foreach (KeyValuePair<string, DbConnectionPoolGroup> keyValuePair in connectionPoolGroups)
				{
					if (keyValuePair.Value != null)
					{
						if (keyValuePair.Value.Prune())
						{
							this.PerformanceCounters.NumberOfActiveConnectionPoolGroups.Decrement();
							this.QueuePoolGroupForRelease(keyValuePair.Value);
						}
						else
						{
							dictionary.Add(keyValuePair.Key, keyValuePair.Value);
						}
					}
				}
				this._connectionPoolGroups = dictionary;
			}
		}

		// Token: 0x06001A4A RID: 6730 RVA: 0x00242C40 File Offset: 0x00242040
		internal void QueuePoolForRelease(DbConnectionPool pool, bool clearing)
		{
			pool.Shutdown();
			lock (this._poolsToRelease)
			{
				if (clearing)
				{
					pool.Clear();
				}
				this._poolsToRelease.Add(pool);
			}
			this.PerformanceCounters.NumberOfInactiveConnectionPools.Increment();
		}

		// Token: 0x06001A4B RID: 6731 RVA: 0x00242CAC File Offset: 0x002420AC
		internal void QueuePoolGroupForRelease(DbConnectionPoolGroup poolGroup)
		{
			Bid.Trace("<prov.DbConnectionFactory.QueuePoolGroupForRelease|RES|INFO|CPOOL> %d#, poolGroup=%d#\n", this.ObjectID, poolGroup.ObjectID);
			lock (this._poolGroupsToRelease)
			{
				this._poolGroupsToRelease.Add(poolGroup);
			}
			this.PerformanceCounters.NumberOfInactiveConnectionPoolGroups.Increment();
		}

		// Token: 0x06001A4C RID: 6732
		protected abstract DbConnectionInternal CreateConnection(DbConnectionOptions options, object poolGroupProviderInfo, DbConnectionPool pool, DbConnection owningConnection);

		// Token: 0x06001A4D RID: 6733
		protected abstract DbConnectionOptions CreateConnectionOptions(string connectionString, DbConnectionOptions previous);

		// Token: 0x06001A4E RID: 6734
		protected abstract DbConnectionPoolGroupOptions CreateConnectionPoolGroupOptions(DbConnectionOptions options);

		// Token: 0x06001A4F RID: 6735
		internal abstract DbConnectionPoolGroup GetConnectionPoolGroup(DbConnection connection);

		// Token: 0x06001A50 RID: 6736
		internal abstract DbConnectionInternal GetInnerConnection(DbConnection connection);

		// Token: 0x06001A51 RID: 6737
		protected abstract int GetObjectId(DbConnection connection);

		// Token: 0x06001A52 RID: 6738
		internal abstract void PermissionDemand(DbConnection outerConnection);

		// Token: 0x06001A53 RID: 6739
		internal abstract void SetConnectionPoolGroup(DbConnection outerConnection, DbConnectionPoolGroup poolGroup);

		// Token: 0x06001A54 RID: 6740
		internal abstract void SetInnerConnectionEvent(DbConnection owningObject, DbConnectionInternal to);

		// Token: 0x06001A55 RID: 6741
		internal abstract bool SetInnerConnectionFrom(DbConnection owningObject, DbConnectionInternal to, DbConnectionInternal from);

		// Token: 0x06001A56 RID: 6742
		internal abstract void SetInnerConnectionTo(DbConnection owningObject, DbConnectionInternal to);

		// Token: 0x04000F8F RID: 3983
		private const int PruningDueTime = 240000;

		// Token: 0x04000F90 RID: 3984
		private const int PruningPeriod = 30000;

		// Token: 0x04000F91 RID: 3985
		private Dictionary<string, DbConnectionPoolGroup> _connectionPoolGroups;

		// Token: 0x04000F92 RID: 3986
		private readonly List<DbConnectionPool> _poolsToRelease;

		// Token: 0x04000F93 RID: 3987
		private readonly List<DbConnectionPoolGroup> _poolGroupsToRelease;

		// Token: 0x04000F94 RID: 3988
		private readonly DbConnectionPoolCounters _performanceCounters;

		// Token: 0x04000F95 RID: 3989
		private readonly Timer _pruningTimer;

		// Token: 0x04000F96 RID: 3990
		private static int _objectTypeCount;

		// Token: 0x04000F97 RID: 3991
		internal readonly int _objectID = Interlocked.Increment(ref DbConnectionFactory._objectTypeCount);
	}
}
