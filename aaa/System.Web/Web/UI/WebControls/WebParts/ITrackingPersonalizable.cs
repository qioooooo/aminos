using System;
using System.Security.Permissions;

namespace System.Web.UI.WebControls.WebParts
{
	// Token: 0x020006C5 RID: 1733
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public interface ITrackingPersonalizable
	{
		// Token: 0x170015F1 RID: 5617
		// (get) Token: 0x06005551 RID: 21841
		bool TracksChanges { get; }

		// Token: 0x06005552 RID: 21842
		void BeginLoad();

		// Token: 0x06005553 RID: 21843
		void BeginSave();

		// Token: 0x06005554 RID: 21844
		void EndLoad();

		// Token: 0x06005555 RID: 21845
		void EndSave();
	}
}
