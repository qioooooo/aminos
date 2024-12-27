using System;

namespace System.Data.ProviderBase
{
	// Token: 0x0200026B RID: 619
	internal sealed class DbConnectionClosedNeverOpened : DbConnectionClosed
	{
		// Token: 0x0600210B RID: 8459 RVA: 0x00265318 File Offset: 0x00264718
		private DbConnectionClosedNeverOpened()
			: base(ConnectionState.Closed, false, true)
		{
		}

		// Token: 0x04001548 RID: 5448
		internal static readonly DbConnectionInternal SingletonInstance = new DbConnectionClosedNeverOpened();
	}
}
