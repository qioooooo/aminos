using System;
using System.Security.Permissions;

namespace System.Web.Util
{
	// Token: 0x02000764 RID: 1892
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public interface IWebPropertyAccessor
	{
		// Token: 0x06005BF6 RID: 23542
		object GetProperty(object target);

		// Token: 0x06005BF7 RID: 23543
		void SetProperty(object target, object value);
	}
}
