using System;
using System.Web.Hosting;

namespace System.Web.Configuration
{
	// Token: 0x02000207 RID: 519
	internal static class IISMapPath
	{
		// Token: 0x06001C25 RID: 7205 RVA: 0x00080F98 File Offset: 0x0007FF98
		internal static IConfigMapPath GetInstance()
		{
			if (ServerConfig.UseMetabase)
			{
				return (IConfigMapPath)MetabaseServerConfig.GetInstance();
			}
			ProcessHost defaultHost = ProcessHost.DefaultHost;
			IProcessHostSupportFunctions processHostSupportFunctions = null;
			if (defaultHost != null)
			{
				processHostSupportFunctions = defaultHost.SupportFunctions;
			}
			if (processHostSupportFunctions == null)
			{
				processHostSupportFunctions = HostingEnvironment.SupportFunctions;
			}
			return new ProcessHostMapPath(processHostSupportFunctions);
		}

		// Token: 0x06001C26 RID: 7206 RVA: 0x00080FD8 File Offset: 0x0007FFD8
		internal static bool IsSiteId(string siteName)
		{
			if (string.IsNullOrEmpty(siteName))
			{
				return false;
			}
			for (int i = 0; i < siteName.Length; i++)
			{
				if (!char.IsDigit(siteName[i]))
				{
					return false;
				}
			}
			return true;
		}
	}
}
