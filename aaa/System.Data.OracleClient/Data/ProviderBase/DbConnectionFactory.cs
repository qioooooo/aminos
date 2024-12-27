using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading;

namespace System.Data.ProviderBase
{
	// Token: 0x02000055 RID: 85
	internal abstract class DbConnectionFactory
	{
		// Token: 0x0600035F RID: 863 RVA: 0x000615E0 File Offset: 0x000609E0
		protected DbConnectionFactory()
			: this(DbConnectionPoolCountersNoCounters.SingletonInstance)
		{
		}

		// Token: 0x06000360 RID: 864 RVA: 0x000615F8 File Offset: 0x000609F8
		protected DbConnectionFactory(DbConnectionPoolCounters performanceCounters)
		{
			this._performanceCounters = performanceCounters;
			this._connectionPoolGroups = new Dictionary<string, DbConnectionPoolGroup>();
			this._poolsToRelease = new List<DbConnectionPool>();
			this._poolGroupsToRelease = new List<DbConnectionPoolGroup>();
			this._pruningTimer = this.CreatePruningTimer();
		}

		// Token: 0x1700009B RID: 155
		// (get) Token: 0x06000361 RID: 865 RVA: 0x00061650 File Offset: 0x00060A50
		internal DbConnectionPoolCounters PerformanceCounters
		{
			get
			{
				return this._performanceCounters;
			}
		}

		// Token: 0x1700009C RID: 156
		// (get) Token: 0x06000362 RID: 866
		public abstract DbProviderFactory ProviderFactory { get; }

		// Token: 0x1700009D RID: 157
		// (get) Token: 0x06000363 RID: 867 RVA: 0x00061664 File Offset: 0x00060A64
		internal int ObjectID
		{
			get
			{
				return this._objectID;
			}
		}

		// Token: 0x06000364 RID: 868 RVA: 0x00061678 File Offset: 0x00060A78
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

		// Token: 0x06000365 RID: 869 RVA: 0x00061714 File Offset: 0x00060B14
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

		// Token: 0x06000366 RID: 870 RVA: 0x00061778 File Offset: 0x00060B78
		internal virtual DbConnectionPoolProviderInfo CreateConnectionPoolProviderInfo(DbConnectionOptions connectionOptions)
		{
			return null;
		}

		// Token: 0x06000367 RID: 871 RVA: 0x00061788 File Offset: 0x00060B88
		protected virtual DbMetaDataFactory CreateMetaDataFactory(DbConnectionInternal internalConnection, out bool cacheMetaDataFactory)
		{
			cacheMetaDataFactory = false;
			throw ADP.NotSupported();
		}

		// Token: 0x06000368 RID: 872 RVA: 0x000617A0 File Offset: 0x00060BA0
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

		// Token: 0x06000369 RID: 873 RVA: 0x000617F8 File Offset: 0x00060BF8
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

		// Token: 0x0600036A RID: 874 RVA: 0x00061848 File Offset: 0x00060C48
		internal virtual DbConnectionPoolGroupProviderInfo CreateConnectionPoolGroupProviderInfo(DbConnectionOptions connectionOptions)
		{
			return null;
		}

		// Token: 0x0600036B RID: 875 RVA: 0x00061858 File Offset: 0x00060C58
		private Timer CreatePruningTimer()
		{
			TimerCallback timerCallback = new TimerCallback(this.PruneConnectionPoolGroups);
			return new Timer(timerCallback, null, 240000, 30000);
		}

		// Token: 0x0600036C RID: 876 RVA: 0x00061884 File Offset: 0x00060C84
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

		// Token: 0x0600036D RID: 877 RVA: 0x00061920 File Offset: 0x00060D20
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

		// Token: 0x0600036E RID: 878 RVA: 0x00061988 File Offset: 0x00060D88
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

		// Token: 0x0600036F RID: 879 RVA: 0x00061B10 File Offset: 0x00060F10
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

		// Token: 0x06000370 RID: 880 RVA: 0x00061B40 File Offset: 0x00060F40
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

		// Token: 0x06000371 RID: 881 RVA: 0x00061D9C File Offset: 0x0006119C
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

		// Token: 0x06000372 RID: 882 RVA: 0x00061E08 File Offset: 0x00061208
		internal void QueuePoolGroupForRelease(DbConnectionPoolGroup poolGroup)
		{
			Bid.Trace("<prov.DbConnectionFactory.QueuePoolGroupForRelease|RES|INFO|CPOOL> %d#, poolGroup=%d#\n", this.ObjectID, poolGroup.ObjectID);
			lock (this._poolGroupsToRelease)
			{
				this._poolGroupsToRelease.Add(poolGroup);
			}
			this.PerformanceCounters.NumberOfInactiveConnectionPoolGroups.Increment();
		}

		// Token: 0x06000373 RID: 883
		protected abstract DbConnectionInternal CreateConnection(DbConnectionOptions options, object poolGroupProviderInfo, DbConnectionPool pool, DbConnection owningConnection);

		// Token: 0x06000374 RID: 884
		protected abstract DbConnectionOptions CreateConnectionOptions(string connectionString, DbConnectionOptions previous);

		// Token: 0x06000375 RID: 885
		protected abstract DbConnectionPoolGroupOptions CreateConnectionPoolGroupOptions(DbConnectionOptions options);

		// Token: 0x06000376 RID: 886
		internal abstract DbConnectionPoolGroup GetConnectionPoolGroup(DbConnection connection);

		// Token: 0x06000377 RID: 887
		internal abstract DbConnectionInternal GetInnerConnection(DbConnection connection);

		// Token: 0x06000378 RID: 888
		protected abstract int GetObjectId(DbConnection connection);

		// Token: 0x06000379 RID: 889
		internal abstract void PermissionDemand(DbConnection outerConnection);

		// Token: 0x0600037A RID: 890
		internal abstract void SetConnectionPoolGroup(DbConnection outerConnection, DbConnectionPoolGroup poolGroup);

		// Token: 0x0600037B RID: 891
		internal abstract void SetInnerConnectionEvent(DbConnection owningObject, DbConnectionInternal to);

		// Token: 0x0600037C RID: 892
		internal abstract bool SetInnerConnectionFrom(DbConnection owningObject, DbConnectionInternal to, DbConnectionInternal from);

		// Token: 0x0600037D RID: 893
		internal abstract void SetInnerConnectionTo(DbConnection owningObject, DbConnectionInternal to);

		// Token: 0x04000393 RID: 915
		private const int PruningDueTime = 240000;

		// Token: 0x04000394 RID: 916
		private const int PruningPeriod = 30000;

		// Token: 0x04000395 RID: 917
		private Dictionary<string, DbConnectionPoolGroup> _connectionPoolGroups;

		// Token: 0x04000396 RID: 918
		private readonly List<DbConnectionPool> _poolsToRelease;

		// Token: 0x04000397 RID: 919
		private readonly List<DbConnectionPoolGroup> _poolGroupsToRelease;

		// Token: 0x04000398 RID: 920
		private readonly DbConnectionPoolCounters _performanceCounters;

		// Token: 0x04000399 RID: 921
		private readonly Timer _pruningTimer;

		// Token: 0x0400039A RID: 922
		private static int _objectTypeCount;

		// Token: 0x0400039B RID: 923
		internal readonly int _objectID = Interlocked.Increment(ref DbConnectionFactory._objectTypeCount);
	}
}
