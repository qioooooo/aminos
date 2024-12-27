using System;
using System.Security.Permissions;

namespace System.Web.Profile
{
	// Token: 0x020002FF RID: 767
	[AttributeUsage(AttributeTargets.Property)]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class ProfileProviderAttribute : Attribute
	{
		// Token: 0x0600261C RID: 9756 RVA: 0x000A348D File Offset: 0x000A248D
		public ProfileProviderAttribute(string providerName)
		{
			this._ProviderName = providerName;
		}

		// Token: 0x170007EE RID: 2030
		// (get) Token: 0x0600261D RID: 9757 RVA: 0x000A349C File Offset: 0x000A249C
		public string ProviderName
		{
			get
			{
				return this._ProviderName;
			}
		}

		// Token: 0x04001D9A RID: 7578
		private string _ProviderName;
	}
}
