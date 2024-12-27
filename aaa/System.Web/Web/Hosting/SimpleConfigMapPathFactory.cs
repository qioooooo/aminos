using System;
using System.Web.Configuration;

namespace System.Web.Hosting
{
	// Token: 0x020002C0 RID: 704
	[Serializable]
	internal class SimpleConfigMapPathFactory : IConfigMapPathFactory
	{
		// Token: 0x06002441 RID: 9281 RVA: 0x0009B314 File Offset: 0x0009A314
		IConfigMapPath IConfigMapPathFactory.Create(string virtualPath, string physicalPath)
		{
			WebConfigurationFileMap webConfigurationFileMap = new WebConfigurationFileMap();
			VirtualPath virtualPath2 = VirtualPath.Create(virtualPath);
			webConfigurationFileMap.VirtualDirectories.Add(virtualPath2.VirtualPathStringNoTrailingSlash, new VirtualDirectoryMapping(physicalPath, true));
			webConfigurationFileMap.VirtualDirectories.Add(HttpRuntime.AspClientScriptVirtualPath, new VirtualDirectoryMapping(HttpRuntime.AspClientScriptPhysicalPathInternal, false));
			return new UserMapPath(webConfigurationFileMap);
		}
	}
}
