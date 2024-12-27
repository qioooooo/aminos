using System;
using System.Security.Permissions;

namespace System.Configuration
{
	// Token: 0x020006F8 RID: 1784
	public interface IApplicationSettingsProvider
	{
		// Token: 0x06003708 RID: 14088
		[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
		SettingsPropertyValue GetPreviousVersion(SettingsContext context, SettingsProperty property);

		// Token: 0x06003709 RID: 14089
		[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
		void Reset(SettingsContext context);

		// Token: 0x0600370A RID: 14090
		[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
		void Upgrade(SettingsContext context, SettingsPropertyCollection properties);
	}
}
