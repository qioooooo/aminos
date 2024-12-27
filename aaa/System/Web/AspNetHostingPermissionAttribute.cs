using System;
using System.Security;
using System.Security.Permissions;

namespace System.Web
{
	// Token: 0x0200073A RID: 1850
	[AttributeUsage(AttributeTargets.All, AllowMultiple = true, Inherited = false)]
	[Serializable]
	public sealed class AspNetHostingPermissionAttribute : CodeAccessSecurityAttribute
	{
		// Token: 0x06003857 RID: 14423 RVA: 0x000EDA29 File Offset: 0x000ECA29
		public AspNetHostingPermissionAttribute(SecurityAction action)
			: base(action)
		{
			this._level = AspNetHostingPermissionLevel.None;
		}

		// Token: 0x17000D11 RID: 3345
		// (get) Token: 0x06003858 RID: 14424 RVA: 0x000EDA3A File Offset: 0x000ECA3A
		// (set) Token: 0x06003859 RID: 14425 RVA: 0x000EDA42 File Offset: 0x000ECA42
		public AspNetHostingPermissionLevel Level
		{
			get
			{
				return this._level;
			}
			set
			{
				AspNetHostingPermission.VerifyAspNetHostingPermissionLevel(value, "Level");
				this._level = value;
			}
		}

		// Token: 0x0600385A RID: 14426 RVA: 0x000EDA56 File Offset: 0x000ECA56
		public override IPermission CreatePermission()
		{
			if (base.Unrestricted)
			{
				return new AspNetHostingPermission(PermissionState.Unrestricted);
			}
			return new AspNetHostingPermission(this._level);
		}

		// Token: 0x04003240 RID: 12864
		private AspNetHostingPermissionLevel _level;
	}
}
