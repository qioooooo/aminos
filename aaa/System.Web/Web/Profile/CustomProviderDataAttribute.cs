using System;
using System.Security.Permissions;

namespace System.Web.Profile
{
	// Token: 0x02000301 RID: 769
	[AttributeUsage(AttributeTargets.Property)]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class CustomProviderDataAttribute : Attribute
	{
		// Token: 0x06002621 RID: 9761 RVA: 0x000A34C6 File Offset: 0x000A24C6
		public CustomProviderDataAttribute(string customProviderData)
		{
			this._CustomProviderData = customProviderData;
		}

		// Token: 0x170007F0 RID: 2032
		// (get) Token: 0x06002622 RID: 9762 RVA: 0x000A34D5 File Offset: 0x000A24D5
		public string CustomProviderData
		{
			get
			{
				return this._CustomProviderData;
			}
		}

		// Token: 0x06002623 RID: 9763 RVA: 0x000A34DD File Offset: 0x000A24DD
		public override bool IsDefaultAttribute()
		{
			return string.IsNullOrEmpty(this._CustomProviderData);
		}

		// Token: 0x04001D9C RID: 7580
		private string _CustomProviderData;
	}
}
