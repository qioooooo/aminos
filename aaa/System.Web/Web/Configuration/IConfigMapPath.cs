using System;
using System.Security.Permissions;

namespace System.Web.Configuration
{
	// Token: 0x020001F1 RID: 497
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public interface IConfigMapPath
	{
		// Token: 0x06001B3B RID: 6971
		string GetMachineConfigFilename();

		// Token: 0x06001B3C RID: 6972
		string GetRootWebConfigFilename();

		// Token: 0x06001B3D RID: 6973
		void GetPathConfigFilename(string siteID, string path, out string directory, out string baseName);

		// Token: 0x06001B3E RID: 6974
		void GetDefaultSiteNameAndID(out string siteName, out string siteID);

		// Token: 0x06001B3F RID: 6975
		void ResolveSiteArgument(string siteArgument, out string siteName, out string siteID);

		// Token: 0x06001B40 RID: 6976
		string MapPath(string siteID, string path);

		// Token: 0x06001B41 RID: 6977
		string GetAppPathForPath(string siteID, string path);
	}
}
