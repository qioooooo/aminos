using System;

namespace System.Data.ProviderBase
{
	// Token: 0x02000091 RID: 145
	internal sealed class DbConnectionClosedNeverOpened : DbConnectionClosed
	{
		// Token: 0x060007FA RID: 2042 RVA: 0x00072E00 File Offset: 0x00072200
		private DbConnectionClosedNeverOpened()
			: base(ConnectionState.Closed, false, true)
		{
		}

		// Token: 0x04000511 RID: 1297
		internal static readonly DbConnectionInternal SingletonInstance = new DbConnectionClosedNeverOpened();
	}
}
