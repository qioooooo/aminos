using System;
using System.Security.Permissions;

namespace System.Web.Profile
{
	// Token: 0x02000300 RID: 768
	[AttributeUsage(AttributeTargets.Property)]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class SettingsAllowAnonymousAttribute : Attribute
	{
		// Token: 0x0600261E RID: 9758 RVA: 0x000A34A4 File Offset: 0x000A24A4
		public SettingsAllowAnonymousAttribute(bool allow)
		{
			this._Allow = allow;
		}

		// Token: 0x170007EF RID: 2031
		// (get) Token: 0x0600261F RID: 9759 RVA: 0x000A34B3 File Offset: 0x000A24B3
		public bool Allow
		{
			get
			{
				return this._Allow;
			}
		}

		// Token: 0x06002620 RID: 9760 RVA: 0x000A34BB File Offset: 0x000A24BB
		public override bool IsDefaultAttribute()
		{
			return !this._Allow;
		}

		// Token: 0x04001D9B RID: 7579
		private bool _Allow;
	}
}
