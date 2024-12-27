using System;
using System.Security.Permissions;

namespace System.Web.SessionState
{
	// Token: 0x0200035F RID: 863
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public interface IRequiresSessionState
	{
	}
}
