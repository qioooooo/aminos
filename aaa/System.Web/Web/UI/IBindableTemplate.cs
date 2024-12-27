using System;
using System.Collections.Specialized;
using System.Security.Permissions;

namespace System.Web.UI
{
	// Token: 0x02000399 RID: 921
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public interface IBindableTemplate : ITemplate
	{
		// Token: 0x06002D08 RID: 11528
		IOrderedDictionary ExtractValues(Control container);
	}
}
