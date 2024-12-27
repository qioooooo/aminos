using System;
using System.Security.Permissions;

namespace System.Web.UI
{
	// Token: 0x020003B9 RID: 953
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public interface IControlBuilderAccessor
	{
		// Token: 0x17000A01 RID: 2561
		// (get) Token: 0x06002E49 RID: 11849
		ControlBuilder ControlBuilder { get; }
	}
}
