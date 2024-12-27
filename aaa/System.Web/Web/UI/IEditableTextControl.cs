using System;
using System.Security.Permissions;

namespace System.Web.UI
{
	// Token: 0x020004CE RID: 1230
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public interface IEditableTextControl : ITextControl
	{
		// Token: 0x14000066 RID: 102
		// (add) Token: 0x06003B08 RID: 15112
		// (remove) Token: 0x06003B09 RID: 15113
		event EventHandler TextChanged;
	}
}
