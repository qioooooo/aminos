using System;
using System.Collections;
using System.Collections.Specialized;
using System.Data.Common;
using System.Threading;

namespace System.Data.ProviderBase
{
	// Token: 0x02000277 RID: 631
	internal sealed class DbConnectionPoolGroup
	{
		// Token: 0x0600215C RID: 8540 RVA: 0x00267518 File Offset: 0x00266918
		internal DbConnectionPoolGroup(DbConnectionOptions connectionOptions, DbConnectionPoolGroupOptions poolGroupOptions)
		{
			this._connectionOptions = connectionOptions;
			this._poolGroupOptions = poolGroupOptions;
			this._poolCollection = new HybridDictionary(1, false);
			this._state = 1;
		}

		// Token: 0x170004A8 RID: 1192
		// (get) Token: 0x0600215D RID: 8541 RVA: 0x00267560 File Offset: 0x00266960
		internal DbConnectionOptions ConnectionOptions
		{
			get
			{
				return this._connectionOptions;
			}
		}

		// Token: 0x170004A9 RID: 1193
		// (get) Token: 0x0600215E RID: 8542 RVA: 0x00267574 File Offset: 0x00266974
		internal int Count
		{
			get
			{
				return this._poolCount;
			}
		}

		// Token: 0x170004AA RID: 1194
		// (get) Token: 0x0600215F RID: 8543 RVA: 0x00267588 File Offset: 0x00266988
		// (set) Token: 0x06002160 RID: 8544 RVA: 0x0026759C File Offset: 0x0026699C
		internal DbConnectionPoolGroupProviderInfo ProviderInfo
		{
			get
			{
				return this._providerInfo;
			}
			set
			{
				this._providerInfo = value;
				if (value != null)
				{
					this._providerInfo.PoolGroup = this;
				}
			}
		}

		// Token: 0x170004AB RID: 1195
		// (get) Token: 0x06002161 RID: 8545 RVA: 0x002675C0 File Offset: 0x002669C0
		internal bool IsDisabled
		{
			get
			{
				return 4 == this._state;
			}
		}

		// Token: 0x170004AC RID: 1196
		// (get) Token: 0x06002162 RID: 8546 RVA: 0x002675D8 File Offset: 0x002669D8
		internal int ObjectID
		{
			get
			{
				return this._objectID;
			}
		}

		// Token: 0x170004AD RID: 1197
		// (get) Token: 0x06002163 RID: 8547 RVA: 0x002675EC File Offset: 0x002669EC
		internal DbConnectionPoolGroupOptions PoolGroupOptions
		{
			get
			{
				return this._poolGroupOptions;
			}
		}

		// Token: 0x170004AE RID: 1198
		// (get) Token: 0x06002164 RID: 8548 RVA: 0x00267600 File Offset: 0x00266A00
		// (set) Token: 0x06002165 RID: 8549 RVA: 0x00267614 File Offset: 0x00266A14
		internal DbMetaDataFactory MetaDataFactory
		{
			get
			{
				return this._metaDataFactory;
			}
			set
			{
				this._metaDataFactory = value;
			}
		}

		// Token: 0x06002166 RID: 8550 RVA: 0x00267628 File Offset: 0x00266A28
		internal void Clear()
		{
			this.ClearInternal(true);
		}

		// Token: 0x06002167 RID: 8551 RVA: 0x00267640 File Offset: 0x00266A40
		private bool ClearInternal(bool clearing)
		{
			bool flag;
			lock (this)
			{
				HybridDictionary poolCollection = this._poolCollection;
				if (0 < poolCollection.Count)
				{
					HybridDictionary hybridDictionary = new HybridDictionary(poolCollection.Count, false);
					foreach (object obj in poolCollection)
					{
						DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
						if (dictionaryEntry.Value != null)
						{
							DbConnectionPool dbConnectionPool = (DbConnectionPool)dictionaryEntry.Value;
							if (clearing || (!dbConnectionPool.ErrorOccurred && dbConnectionPool.Count == 0))
							{
								DbConnectionFactory connectionFactory = dbConnectionPool.ConnectionFactory;
								connectionFactory.PerformanceCounters.NumberOfActiveConnectionPools.Decrement();
								connectionFactory.QueuePoolForRelease(dbConnectionPool, clearing);
							}
							else
							{
								hybridDictionary.Add(dictionaryEntry.Key, dictionaryEntry.Value);
							}
						}
					}
					this._poolCollection = hybridDictionary;
					this._poolCount = hybridDictionary.Count;
				}
				if (!clearing && this._poolCount == 0)
				{
					if (1 == this._state)
					{
						this._state = 2;
						Bid.Trace("<prov.DbConnectionPoolGroup.ClearInternal|RES|INFO|CPOOL> %d#, Idle\n", this.ObjectID);
					}
					else if (2 == this._state)
					{
						this._state = 4;
						Bid.Trace("<prov.DbConnectionPoolGroup.ReadyToRemove|RES|INFO|CPOOL> %d#, Disabled\n", this.ObjectID);
					}
				}
				flag = 4 == this._state;
			}
			return flag;
		}

		// Token: 0x06002168 RID: 8552 RVA: 0x002677B8 File Offset: 0x00266BB8
		internal DbConnectionPool GetConnectionPool(DbConnectionFactory connectionFactory)
		{
			object obj = null;
			if (this._poolGroupOptions != null)
			{
				DbConnectionPoolIdentity dbConnectionPoolIdentity = DbConnectionPoolIdentity.NoIdentity;
				if (this._poolGroupOptions.PoolByIdentity)
				{
					dbConnectionPoolIdentity = DbConnectionPoolIdentity.GetCurrent();
					if (dbConnectionPoolIdentity.IsRestricted)
					{
						dbConnectionPoolIdentity = null;
					}
				}
				if (dbConnectionPoolIdentity != null)
				{
					HybridDictionary hybridDictionary = this._poolCollection;
					obj = hybridDictionary[dbConnectionPoolIdentity];
					if (obj == null)
					{
						DbConnectionPoolProviderInfo dbConnectionPoolProviderInfo = connectionFactory.CreateConnectionPoolProviderInfo(this.ConnectionOptions);
						DbConnectionPool dbConnectionPool = new DbConnectionPool(connectionFactory, this, dbConnectionPoolIdentity, dbConnectionPoolProviderInfo);
						lock (this)
						{
							hybridDictionary = this._poolCollection;
							obj = hybridDictionary[dbConnectionPoolIdentity];
							if (obj == null && this.MarkPoolGroupAsActive())
							{
								dbConnectionPool.Startup();
								HybridDictionary hybridDictionary2 = new HybridDictionary(1 + hybridDictionary.Count, false);
								foreach (object obj2 in hybridDictionary)
								{
									DictionaryEntry dictionaryEntry = (DictionaryEntry)obj2;
									hybridDictionary2.Add(dictionaryEntry.Key, dictionaryEntry.Value);
								}
								hybridDictionary2.Add(dbConnectionPoolIdentity, dbConnectionPool);
								connectionFactory.PerformanceCounters.NumberOfActiveConnectionPools.Increment();
								this._poolCollection = hybridDictionary2;
								this._poolCount = hybridDictionary2.Count;
								obj = dbConnectionPool;
								dbConnectionPool = null;
							}
						}
						if (dbConnectionPool != null)
						{
							dbConnectionPool.Shutdown();
						}
					}
				}
			}
			if (obj == null)
			{
				lock (this)
				{
					this.MarkPoolGroupAsActive();
				}
			}
			return (DbConnectionPool)obj;
		}

		// Token: 0x06002169 RID: 8553 RVA: 0x0026796C File Offset: 0x00266D6C
		private bool MarkPoolGroupAsActive()
		{
			if (2 == this._state)
			{
				this._state = 1;
				Bid.Trace("<prov.DbConnectionPoolGroup.ClearInternal|RES|INFO|CPOOL> %d#, Active\n", this.ObjectID);
			}
			return 1 == this._state;
		}

		// Token: 0x0600216A RID: 8554 RVA: 0x002679A4 File Offset: 0x00266DA4
		internal bool Prune()
		{
			return this.ClearInternal(false);
		}

		// Token: 0x0400159C RID: 5532
		private const int PoolGroupStateActive = 1;

		// Token: 0x0400159D RID: 5533
		private const int PoolGroupStateIdle = 2;

		// Token: 0x0400159E RID: 5534
		private const int PoolGroupStateDisabled = 4;

		// Token: 0x0400159F RID: 5535
		private readonly DbConnectionOptions _connectionOptions;

		// Token: 0x040015A0 RID: 5536
		private readonly DbConnectionPoolGroupOptions _poolGroupOptions;

		// Token: 0x040015A1 RID: 5537
		private HybridDictionary _poolCollection;

		// Token: 0x040015A2 RID: 5538
		private int _poolCount;

		// Token: 0x040015A3 RID: 5539
		private int _state;

		// Token: 0x040015A4 RID: 5540
		private DbConnectionPoolGroupProviderInfo _providerInfo;

		// Token: 0x040015A5 RID: 5541
		private DbMetaDataFactory _metaDataFactory;

		// Token: 0x040015A6 RID: 5542
		private static int _objectTypeCount;

		// Token: 0x040015A7 RID: 5543
		internal readonly int _objectID = Interlocked.Increment(ref DbConnectionPoolGroup._objectTypeCount);
	}
}
