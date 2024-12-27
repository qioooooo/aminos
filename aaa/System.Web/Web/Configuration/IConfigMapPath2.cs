using System;

namespace System.Web.Configuration
{
	// Token: 0x02000204 RID: 516
	internal interface IConfigMapPath2
	{
		// Token: 0x06001C10 RID: 7184
		void GetPathConfigFilename(string siteID, VirtualPath path, out string directory, out string baseName);

		// Token: 0x06001C11 RID: 7185
		string MapPath(string siteID, VirtualPath path);

		// Token: 0x06001C12 RID: 7186
		VirtualPath GetAppPathForPath(string siteID, VirtualPath path);
	}
}
