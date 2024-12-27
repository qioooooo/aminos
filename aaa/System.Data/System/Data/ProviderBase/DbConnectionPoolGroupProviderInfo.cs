using System;

namespace System.Data.ProviderBase
{
	// Token: 0x020001DE RID: 478
	internal class DbConnectionPoolGroupProviderInfo
	{
		// Token: 0x17000379 RID: 889
		// (get) Token: 0x06001ABD RID: 6845 RVA: 0x002441C0 File Offset: 0x002435C0
		// (set) Token: 0x06001ABE RID: 6846 RVA: 0x002441D4 File Offset: 0x002435D4
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

		// Token: 0x04000FB5 RID: 4021
		private DbConnectionPoolGroup _poolGroup;
	}
}
