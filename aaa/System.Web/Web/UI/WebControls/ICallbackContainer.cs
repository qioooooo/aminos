using System;
using System.Security.Permissions;

namespace System.Web.UI.WebControls
{
	// Token: 0x02000555 RID: 1365
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public interface ICallbackContainer
	{
		// Token: 0x06004333 RID: 17203
		string GetCallbackScript(IButtonControl buttonControl, string argument);
	}
}
