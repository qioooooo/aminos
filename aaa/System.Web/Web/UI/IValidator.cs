using System;
using System.Security.Permissions;

namespace System.Web.UI
{
	// Token: 0x0200041B RID: 1051
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public interface IValidator
	{
		// Token: 0x17000B31 RID: 2865
		// (get) Token: 0x060032CD RID: 13005
		// (set) Token: 0x060032CE RID: 13006
		bool IsValid { get; set; }

		// Token: 0x17000B32 RID: 2866
		// (get) Token: 0x060032CF RID: 13007
		// (set) Token: 0x060032D0 RID: 13008
		string ErrorMessage { get; set; }

		// Token: 0x060032D1 RID: 13009
		void Validate();
	}
}
