using System;
using System.Security.Permissions;

namespace System.Web.UI
{
	// Token: 0x02000424 RID: 1060
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class MasterPageControlBuilder : UserControlControlBuilder
	{
		// Token: 0x040023DE RID: 9182
		internal static readonly string AutoTemplatePrefix = "Template_";
	}
}
