using System;
using System.Collections;
using System.Security.Permissions;

namespace System.Web.UI
{
	// Token: 0x020003BA RID: 954
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public interface IControlDesignerAccessor
	{
		// Token: 0x17000A02 RID: 2562
		// (get) Token: 0x06002E4A RID: 11850
		IDictionary UserData { get; }

		// Token: 0x06002E4B RID: 11851
		IDictionary GetDesignModeState();

		// Token: 0x06002E4C RID: 11852
		void SetDesignModeState(IDictionary data);

		// Token: 0x06002E4D RID: 11853
		void SetOwnerControl(Control owner);
	}
}
