using System;
using System.Security.Permissions;

namespace System.Web.UI.WebControls
{
	// Token: 0x02000598 RID: 1432
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public interface IPersistedSelector
	{
		// Token: 0x17001134 RID: 4404
		// (get) Token: 0x06004610 RID: 17936
		// (set) Token: 0x06004611 RID: 17937
		DataKey DataKey { get; set; }
	}
}
