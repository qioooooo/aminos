using System;

namespace System.Data.ProviderBase
{
	// Token: 0x02000279 RID: 633
	internal sealed class DbConnectionPoolGroupOptions
	{
		// Token: 0x06002176 RID: 8566 RVA: 0x00267D20 File Offset: 0x00267120
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

		// Token: 0x170004B1 RID: 1201
		// (get) Token: 0x06002177 RID: 8567 RVA: 0x00267D7C File Offset: 0x0026717C
		public int CreationTimeout
		{
			get
			{
				return this._creationTimeout;
			}
		}

		// Token: 0x170004B2 RID: 1202
		// (get) Token: 0x06002178 RID: 8568 RVA: 0x00267D90 File Offset: 0x00267190
		public bool HasTransactionAffinity
		{
			get
			{
				return this._hasTransactionAffinity;
			}
		}

		// Token: 0x170004B3 RID: 1203
		// (get) Token: 0x06002179 RID: 8569 RVA: 0x00267DA4 File Offset: 0x002671A4
		public TimeSpan LoadBalanceTimeout
		{
			get
			{
				return this._loadBalanceTimeout;
			}
		}

		// Token: 0x170004B4 RID: 1204
		// (get) Token: 0x0600217A RID: 8570 RVA: 0x00267DB8 File Offset: 0x002671B8
		public int MaxPoolSize
		{
			get
			{
				return this._maxPoolSize;
			}
		}

		// Token: 0x170004B5 RID: 1205
		// (get) Token: 0x0600217B RID: 8571 RVA: 0x00267DCC File Offset: 0x002671CC
		public int MinPoolSize
		{
			get
			{
				return this._minPoolSize;
			}
		}

		// Token: 0x170004B6 RID: 1206
		// (get) Token: 0x0600217C RID: 8572 RVA: 0x00267DE0 File Offset: 0x002671E0
		public bool PoolByIdentity
		{
			get
			{
				return this._poolByIdentity;
			}
		}

		// Token: 0x170004B7 RID: 1207
		// (get) Token: 0x0600217D RID: 8573 RVA: 0x00267DF4 File Offset: 0x002671F4
		public bool UseDeactivateQueue
		{
			get
			{
				return this._useDeactivateQueue;
			}
		}

		// Token: 0x170004B8 RID: 1208
		// (get) Token: 0x0600217E RID: 8574 RVA: 0x00267E08 File Offset: 0x00267208
		public bool UseLoadBalancing
		{
			get
			{
				return this._useLoadBalancing;
			}
		}

		// Token: 0x040015B3 RID: 5555
		private readonly bool _poolByIdentity;

		// Token: 0x040015B4 RID: 5556
		private readonly int _minPoolSize;

		// Token: 0x040015B5 RID: 5557
		private readonly int _maxPoolSize;

		// Token: 0x040015B6 RID: 5558
		private readonly int _creationTimeout;

		// Token: 0x040015B7 RID: 5559
		private readonly TimeSpan _loadBalanceTimeout;

		// Token: 0x040015B8 RID: 5560
		private readonly bool _hasTransactionAffinity;

		// Token: 0x040015B9 RID: 5561
		private readonly bool _useDeactivateQueue;

		// Token: 0x040015BA RID: 5562
		private readonly bool _useLoadBalancing;
	}
}
