using System;
using System.Security.Permissions;

namespace System.Web.UI
{
	// Token: 0x02000397 RID: 919
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public interface ITemplate
	{
		// Token: 0x06002CFB RID: 11515
		void InstantiateIn(Control container);
	}
}
