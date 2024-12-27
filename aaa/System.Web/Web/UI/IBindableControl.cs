using System;
using System.Collections.Specialized;
using System.Security.Permissions;

namespace System.Web.UI
{
	// Token: 0x02000407 RID: 1031
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public interface IBindableControl
	{
		// Token: 0x06003298 RID: 12952
		void ExtractValues(IOrderedDictionary dictionary);
	}
}
