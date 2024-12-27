using System;
using System.Security.Permissions;

namespace System.Web.Management
{
	// Token: 0x020002E1 RID: 737
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public interface IWebEventCustomEvaluator
	{
		// Token: 0x06002537 RID: 9527
		bool CanFire(WebBaseEvent raisedEvent, RuleFiringRecord record);
	}
}
