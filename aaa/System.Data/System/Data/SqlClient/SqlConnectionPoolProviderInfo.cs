using System;
using System.Data.ProviderBase;

namespace System.Data.SqlClient
{
	// Token: 0x020002D3 RID: 723
	internal sealed class SqlConnectionPoolProviderInfo : DbConnectionPoolProviderInfo
	{
		// Token: 0x170005A7 RID: 1447
		// (get) Token: 0x06002512 RID: 9490 RVA: 0x0027A5D0 File Offset: 0x002799D0
		// (set) Token: 0x06002513 RID: 9491 RVA: 0x0027A5E4 File Offset: 0x002799E4
		internal string InstanceName
		{
			get
			{
				return this._instanceName;
			}
			set
			{
				this._instanceName = value;
			}
		}

		// Token: 0x04001789 RID: 6025
		private string _instanceName;
	}
}
