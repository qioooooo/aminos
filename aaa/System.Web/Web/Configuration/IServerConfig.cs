using System;
using System.Web.Hosting;

namespace System.Web.Configuration
{
	// Token: 0x0200020A RID: 522
	internal interface IServerConfig
	{
		// Token: 0x06001C30 RID: 7216
		string MapPath(IApplicationHost appHost, VirtualPath path);

		// Token: 0x06001C31 RID: 7217
		string GetSiteNameFromSiteID(string siteID);

		// Token: 0x06001C32 RID: 7218
		bool GetUncUser(IApplicationHost appHost, VirtualPath path, out string username, out string password);

		// Token: 0x06001C33 RID: 7219
		string[] GetVirtualSubdirs(VirtualPath path, bool inApp);

		// Token: 0x06001C34 RID: 7220
		long GetW3WPMemoryLimitInKB();
	}
}
