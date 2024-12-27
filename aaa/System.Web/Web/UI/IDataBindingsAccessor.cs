using System;
using System.Security.Permissions;

namespace System.Web.UI
{
	// Token: 0x020003B8 RID: 952
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public interface IDataBindingsAccessor
	{
		// Token: 0x170009FF RID: 2559
		// (get) Token: 0x06002E47 RID: 11847
		DataBindingCollection DataBindings { get; }

		// Token: 0x17000A00 RID: 2560
		// (get) Token: 0x06002E48 RID: 11848
		bool HasDataBindings { get; }
	}
}
