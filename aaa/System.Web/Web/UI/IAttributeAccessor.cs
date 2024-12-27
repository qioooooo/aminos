using System;
using System.Security.Permissions;

namespace System.Web.UI
{
	// Token: 0x020003FD RID: 1021
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public interface IAttributeAccessor
	{
		// Token: 0x0600325A RID: 12890
		string GetAttribute(string key);

		// Token: 0x0600325B RID: 12891
		void SetAttribute(string key, string value);
	}
}
