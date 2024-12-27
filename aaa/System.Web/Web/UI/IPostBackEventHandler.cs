using System;
using System.Security.Permissions;

namespace System.Web.UI
{
	// Token: 0x02000412 RID: 1042
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public interface IPostBackEventHandler
	{
		// Token: 0x060032B2 RID: 12978
		void RaisePostBackEvent(string eventArgument);
	}
}
