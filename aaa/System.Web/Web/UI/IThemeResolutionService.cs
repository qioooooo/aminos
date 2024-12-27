using System;
using System.Security.Permissions;

namespace System.Web.UI
{
	// Token: 0x02000417 RID: 1047
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public interface IThemeResolutionService
	{
		// Token: 0x060032C5 RID: 12997
		ThemeProvider[] GetAllThemeProviders();

		// Token: 0x060032C6 RID: 12998
		ThemeProvider GetThemeProvider();

		// Token: 0x060032C7 RID: 12999
		ThemeProvider GetStylesheetThemeProvider();
	}
}
