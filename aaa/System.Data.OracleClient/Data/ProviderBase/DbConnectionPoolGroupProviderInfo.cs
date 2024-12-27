using System;

namespace System.Data.ProviderBase
{
	// Token: 0x0200009D RID: 157
	internal class DbConnectionPoolGroupProviderInfo
	{
		// Token: 0x1700018C RID: 396
		// (get) Token: 0x0600085F RID: 2143 RVA: 0x00075258 File Offset: 0x00074658
		// (set) Token: 0x06000860 RID: 2144 RVA: 0x0007526C File Offset: 0x0007466C
		internal DbConnectionPoolGroup PoolGroup
		{
			get
			{
				return this._poolGroup;
			}
			set
			{
				this._poolGroup = value;
			}
		}

		// Token: 0x04000567 RID: 1383
		private DbConnectionPoolGroup _poolGroup;
	}
}
