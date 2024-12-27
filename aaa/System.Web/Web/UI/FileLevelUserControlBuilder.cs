using System;
using System.Security.Permissions;

namespace System.Web.UI
{
	// Token: 0x02000482 RID: 1154
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class FileLevelUserControlBuilder : RootBuilder
	{
	}
}
