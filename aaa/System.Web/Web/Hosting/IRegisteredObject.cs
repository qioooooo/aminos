using System;
using System.Security.Permissions;

namespace System.Web.Hosting
{
	// Token: 0x020000F3 RID: 243
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public interface IRegisteredObject
	{
		// Token: 0x06000B9F RID: 2975
		void Stop(bool immediate);
	}
}
