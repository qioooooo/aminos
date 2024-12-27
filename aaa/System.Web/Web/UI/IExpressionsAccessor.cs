using System;
using System.Security.Permissions;

namespace System.Web.UI
{
	// Token: 0x020003BB RID: 955
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public interface IExpressionsAccessor
	{
		// Token: 0x17000A03 RID: 2563
		// (get) Token: 0x06002E4E RID: 11854
		bool HasExpressions { get; }

		// Token: 0x17000A04 RID: 2564
		// (get) Token: 0x06002E4F RID: 11855
		ExpressionBindingCollection Expressions { get; }
	}
}
