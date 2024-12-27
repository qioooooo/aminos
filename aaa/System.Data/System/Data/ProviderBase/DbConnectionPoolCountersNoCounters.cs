using System;

namespace System.Data.ProviderBase
{
	// Token: 0x02000276 RID: 630
	internal sealed class DbConnectionPoolCountersNoCounters : DbConnectionPoolCounters
	{
		// Token: 0x0600215A RID: 8538 RVA: 0x002674EC File Offset: 0x002668EC
		private DbConnectionPoolCountersNoCounters()
		{
		}

		// Token: 0x0400159B RID: 5531
		public static readonly DbConnectionPoolCountersNoCounters SingletonInstance = new DbConnectionPoolCountersNoCounters();
	}
}
