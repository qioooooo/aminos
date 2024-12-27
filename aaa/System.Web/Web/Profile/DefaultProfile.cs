using System;
using System.Security.Permissions;

namespace System.Web.Profile
{
	// Token: 0x02000303 RID: 771
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class DefaultProfile : ProfileBase
	{
	}
}
