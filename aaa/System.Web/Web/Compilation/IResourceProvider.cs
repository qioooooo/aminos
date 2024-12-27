using System;
using System.Globalization;
using System.Resources;
using System.Security.Permissions;

namespace System.Web.Compilation
{
	// Token: 0x02000176 RID: 374
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public interface IResourceProvider
	{
		// Token: 0x06001078 RID: 4216
		object GetObject(string resourceKey, CultureInfo culture);

		// Token: 0x17000415 RID: 1045
		// (get) Token: 0x06001079 RID: 4217
		IResourceReader ResourceReader { get; }
	}
}
