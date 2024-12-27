using System;
using System.Security.Permissions;

namespace System.Web.Configuration
{
	// Token: 0x02000205 RID: 517
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public interface IConfigMapPathFactory
	{
		// Token: 0x06001C13 RID: 7187
		IConfigMapPath Create(string virtualPath, string physicalPath);
	}
}
