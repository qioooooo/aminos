using System;
using System.Security.Permissions;

namespace System.Web.UI
{
	// Token: 0x02000408 RID: 1032
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public interface ICallbackEventHandler
	{
		// Token: 0x06003299 RID: 12953
		void RaiseCallbackEvent(string eventArgument);

		// Token: 0x0600329A RID: 12954
		string GetCallbackResult();
	}
}
