using System;
using System.Resources;

namespace System.Security
{
	// Token: 0x02000002 RID: 2
	internal static class SecurityResources
	{
		// Token: 0x06000001 RID: 1 RVA: 0x000020D0 File Offset: 0x000010D0
		internal static string GetResourceString(string key)
		{
			if (SecurityResources.s_resMgr == null)
			{
				SecurityResources.s_resMgr = new ResourceManager("system.security", typeof(SecurityResources).Assembly);
			}
			return SecurityResources.s_resMgr.GetString(key, null);
		}

		// Token: 0x04000001 RID: 1
		private static ResourceManager s_resMgr;
	}
}
