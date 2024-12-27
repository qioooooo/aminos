using System;
using System.Security.Permissions;

namespace System.Web.Util
{
	// Token: 0x02000149 RID: 329
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public interface IWebObjectFactory
	{
		// Token: 0x06000F57 RID: 3927
		object CreateInstance();
	}
}
