using System;
using System.Security.Permissions;

namespace System.Web.UI.WebControls.WebParts
{
	// Token: 0x020006C0 RID: 1728
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public interface IWebActionable
	{
		// Token: 0x170015C1 RID: 5569
		// (get) Token: 0x060054EC RID: 21740
		WebPartVerbCollection Verbs { get; }
	}
}
