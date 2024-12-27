using System;
using System.Security.Permissions;

namespace System.Web.UI
{
	// Token: 0x020004EE RID: 1262
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public interface ICheckBoxControl
	{
		// Token: 0x17000E61 RID: 3681
		// (get) Token: 0x06003D68 RID: 15720
		// (set) Token: 0x06003D69 RID: 15721
		bool Checked { get; set; }

		// Token: 0x14000078 RID: 120
		// (add) Token: 0x06003D6A RID: 15722
		// (remove) Token: 0x06003D6B RID: 15723
		event EventHandler CheckedChanged;
	}
}
