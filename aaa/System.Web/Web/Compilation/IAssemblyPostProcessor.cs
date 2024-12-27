using System;
using System.Security.Permissions;

namespace System.Web.Compilation
{
	// Token: 0x02000171 RID: 369
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.High)]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.High)]
	public interface IAssemblyPostProcessor : IDisposable
	{
		// Token: 0x06001066 RID: 4198
		void PostProcessAssembly(string path);
	}
}
