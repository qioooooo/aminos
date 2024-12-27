using System;

namespace System.Data.ProviderBase
{
	// Token: 0x0200009C RID: 156
	internal sealed class DbConnectionPoolGroupOptions
	{
		// Token: 0x06000856 RID: 2134 RVA: 0x0007515C File Offset: 0x0007455C
		public DbConnectionPoolGroupOptions(bool poolByIdentity, int minPoolSize, int maxPoolSize, int creationTimeout, int loadBalanceTimeout, bool hasTransactionAffinity, bool useDeactivateQueue)
		{
			this._poolByIdentity = poolByIdentity;
			this._minPoolSize = minPoolSize;
			this._maxPoolSize = maxPoolSize;
			this._creationTimeout = creationTimeout;
			if (loadBalanceTimeout != 0)
			{
				this._loadBalanceTimeout = new TimeSpan(0, 0, loadBalanceTimeout);
				this._useLoadBalancing = true;
			}
			this._hasTransactionAffinity = hasTransactionAffinity;
			this._useDeactivateQueue = useDeactivateQueue;
		}

		// Token: 0x17000184 RID: 388
		// (get) Token: 0x06000857 RID: 2135 RVA: 0x000751B8 File Offset: 0x000745B8
		public int CreationTimeout
		{
			get
			{
				return this._creationTimeout;
			}
		}

		// Token: 0x17000185 RID: 389
		// (get) Token: 0x06000858 RID: 2136 RVA: 0x000751CC File Offset: 0x000745CC
		public bool HasTransactionAffinity
		{
			get
			{
				return this._hasTransactionAffinity;
			}
		}

		// Token: 0x17000186 RID: 390
		// (get) Token: 0x06000859 RID: 2137 RVA: 0x000751E0 File Offset: 0x000745E0
		public TimeSpan LoadBalanceTimeout
		{
			get
			{
				return this._loadBalanceTimeout;
			}
		}

		// Token: 0x17000187 RID: 391
		// (get) Token: 0x0600085A RID: 2138 RVA: 0x000751F4 File Offset: 0x000745F4
		public int MaxPoolSize
		{
			get
			{
				return this._maxPoolSize;
			}
		}

		// Token: 0x17000188 RID: 392
		// (get) Token: 0x0600085B RID: 2139 RVA: 0x00075208 File Offset: 0x00074608
		public int MinPoolSize
		{
			get
			{
				return this._minPoolSize;
			}
		}

		// Token: 0x17000189 RID: 393
		// (get) Token: 0x0600085C RID: 2140 RVA: 0x0007521C File Offset: 0x0007461C
		public bool PoolByIdentity
		{
			get
			{
				return this._poolByIdentity;
			}
		}

		// Token: 0x1700018A RID: 394
		// (get) Token: 0x0600085D RID: 2141 RVA: 0x00075230 File Offset: 0x00074630
		public bool UseDeactivateQueue
		{
			get
			{
				return this._useDeactivateQueue;
			}
		}

		// Token: 0x1700018B RID: 395
		// (get) Token: 0x0600085E RID: 2142 RVA: 0x00075244 File Offset: 0x00074644
		public bool UseLoadBalancing
		{
			get
			{
				return this._useLoadBalancing;
			}
		}

		// Token: 0x0400055F RID: 1375
		private readonly bool _poolByIdentity;

		// Token: 0x04000560 RID: 1376
		private readonly int _minPoolSize;

		// Token: 0x04000561 RID: 1377
		private readonly int _maxPoolSize;

		// Token: 0x04000562 RID: 1378
		private readonly int _creationTimeout;

		// Token: 0x04000563 RID: 1379
		private readonly TimeSpan _loadBalanceTimeout;

		// Token: 0x04000564 RID: 1380
		private readonly bool _hasTransactionAffinity;

		// Token: 0x04000565 RID: 1381
		private readonly bool _useDeactivateQueue;

		// Token: 0x04000566 RID: 1382
		private readonly bool _useLoadBalancing;
	}
}
