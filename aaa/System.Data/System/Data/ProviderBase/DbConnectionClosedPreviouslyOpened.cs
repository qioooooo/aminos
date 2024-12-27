using System;

namespace System.Data.ProviderBase
{
	// Token: 0x0200026C RID: 620
	internal sealed class DbConnectionClosedPreviouslyOpened : DbConnectionClosed
	{
		// Token: 0x0600210D RID: 8461 RVA: 0x00265348 File Offset: 0x00264748
		private DbConnectionClosedPreviouslyOpened()
			: base(ConnectionState.Closed, true, true)
		{
		}

		// Token: 0x04001549 RID: 5449
		internal static readonly DbConnectionInternal SingletonInstance = new DbConnectionClosedPreviouslyOpened();
	}
}
