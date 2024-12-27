using System;
using System.Security.Permissions;

namespace System.Web.Compilation
{
	// Token: 0x02000174 RID: 372
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class ImplicitResourceKey
	{
		// Token: 0x0600106A RID: 4202 RVA: 0x00048FA3 File Offset: 0x00047FA3
		public ImplicitResourceKey()
		{
		}

		// Token: 0x0600106B RID: 4203 RVA: 0x00048FAB File Offset: 0x00047FAB
		public ImplicitResourceKey(string filter, string keyPrefix, string property)
		{
			this._filter = filter;
			this._keyPrefix = keyPrefix;
			this._property = property;
		}

		// Token: 0x17000412 RID: 1042
		// (get) Token: 0x0600106C RID: 4204 RVA: 0x00048FC8 File Offset: 0x00047FC8
		// (set) Token: 0x0600106D RID: 4205 RVA: 0x00048FD0 File Offset: 0x00047FD0
		public string Filter
		{
			get
			{
				return this._filter;
			}
			set
			{
				this._filter = value;
			}
		}

		// Token: 0x17000413 RID: 1043
		// (get) Token: 0x0600106E RID: 4206 RVA: 0x00048FD9 File Offset: 0x00047FD9
		// (set) Token: 0x0600106F RID: 4207 RVA: 0x00048FE1 File Offset: 0x00047FE1
		public string KeyPrefix
		{
			get
			{
				return this._keyPrefix;
			}
			set
			{
				this._keyPrefix = value;
			}
		}

		// Token: 0x17000414 RID: 1044
		// (get) Token: 0x06001070 RID: 4208 RVA: 0x00048FEA File Offset: 0x00047FEA
		// (set) Token: 0x06001071 RID: 4209 RVA: 0x00048FF2 File Offset: 0x00047FF2
		public string Property
		{
			get
			{
				return this._property;
			}
			set
			{
				this._property = value;
			}
		}

		// Token: 0x04001652 RID: 5714
		private string _filter;

		// Token: 0x04001653 RID: 5715
		private string _keyPrefix;

		// Token: 0x04001654 RID: 5716
		private string _property;
	}
}
