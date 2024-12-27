using System;
using System.Security.Permissions;

namespace System.Web.Compilation
{
	// Token: 0x0200018C RID: 396
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class ResourceExpressionFields
	{
		// Token: 0x06001102 RID: 4354 RVA: 0x0004C932 File Offset: 0x0004B932
		internal ResourceExpressionFields(string classKey, string resourceKey)
		{
			this._classKey = classKey;
			this._resourceKey = resourceKey;
		}

		// Token: 0x17000426 RID: 1062
		// (get) Token: 0x06001103 RID: 4355 RVA: 0x0004C948 File Offset: 0x0004B948
		public string ClassKey
		{
			get
			{
				if (this._classKey == null)
				{
					return string.Empty;
				}
				return this._classKey;
			}
		}

		// Token: 0x17000427 RID: 1063
		// (get) Token: 0x06001104 RID: 4356 RVA: 0x0004C95E File Offset: 0x0004B95E
		public string ResourceKey
		{
			get
			{
				if (this._resourceKey == null)
				{
					return string.Empty;
				}
				return this._resourceKey;
			}
		}

		// Token: 0x0400168C RID: 5772
		private string _classKey;

		// Token: 0x0400168D RID: 5773
		private string _resourceKey;
	}
}
