using System;
using System.Security.Permissions;

namespace System.Web.UI
{
	// Token: 0x020003D4 RID: 980
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public interface ITextControl
	{
		// Token: 0x17000A5F RID: 2655
		// (get) Token: 0x06002FCC RID: 12236
		// (set) Token: 0x06002FCD RID: 12237
		string Text { get; set; }
	}
}
