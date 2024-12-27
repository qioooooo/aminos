using System;
using System.Collections;
using System.Globalization;
using System.Security.Permissions;

namespace System.Web.Compilation
{
	// Token: 0x02000173 RID: 371
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public interface IImplicitResourceProvider
	{
		// Token: 0x06001068 RID: 4200
		object GetObject(ImplicitResourceKey key, CultureInfo culture);

		// Token: 0x06001069 RID: 4201
		ICollection GetImplicitResourceKeys(string keyPrefix);
	}
}
