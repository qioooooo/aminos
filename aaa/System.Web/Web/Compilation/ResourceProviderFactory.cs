using System;
using System.Security.Permissions;

namespace System.Web.Compilation
{
	// Token: 0x0200018D RID: 397
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public abstract class ResourceProviderFactory
	{
		// Token: 0x06001105 RID: 4357
		public abstract IResourceProvider CreateGlobalResourceProvider(string classKey);

		// Token: 0x06001106 RID: 4358
		public abstract IResourceProvider CreateLocalResourceProvider(string virtualPath);
	}
}
