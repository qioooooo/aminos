using System;
using System.Security.Permissions;

namespace System.Web.SessionState
{
	// Token: 0x02000360 RID: 864
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public interface IReadOnlySessionState : IRequiresSessionState
	{
	}
}
