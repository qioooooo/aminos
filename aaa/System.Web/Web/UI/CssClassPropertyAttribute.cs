using System;
using System.Security.Permissions;

namespace System.Web.UI
{
	// Token: 0x020003CA RID: 970
	[AttributeUsage(AttributeTargets.Property)]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class CssClassPropertyAttribute : Attribute
	{
	}
}
