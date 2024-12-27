using System;
using System.Security.Permissions;

namespace System.Web.UI
{
	// Token: 0x020003D7 RID: 983
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public interface IStateManager
	{
		// Token: 0x17000A63 RID: 2659
		// (get) Token: 0x06002FDF RID: 12255
		bool IsTrackingViewState { get; }

		// Token: 0x06002FE0 RID: 12256
		void LoadViewState(object state);

		// Token: 0x06002FE1 RID: 12257
		object SaveViewState();

		// Token: 0x06002FE2 RID: 12258
		void TrackViewState();
	}
}
