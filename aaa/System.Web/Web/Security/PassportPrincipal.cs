using System;
using System.Security.Permissions;
using System.Security.Principal;

namespace System.Web.Security
{
	// Token: 0x0200034E RID: 846
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class PassportPrincipal : GenericPrincipal
	{
		// Token: 0x0600291F RID: 10527 RVA: 0x000B424A File Offset: 0x000B324A
		public PassportPrincipal(PassportIdentity identity, string[] roles)
			: base(identity, roles)
		{
		}
	}
}
