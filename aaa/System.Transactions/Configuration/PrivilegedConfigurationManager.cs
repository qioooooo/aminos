using System;
using System.Security.Permissions;

namespace System.Configuration
{
	// Token: 0x020000CD RID: 205
	[ConfigurationPermission(SecurityAction.Assert, Unrestricted = true)]
	internal static class PrivilegedConfigurationManager
	{
		// Token: 0x170000F1 RID: 241
		// (get) Token: 0x0600056D RID: 1389 RVA: 0x0003F014 File Offset: 0x0003E414
		internal static ConnectionStringSettingsCollection ConnectionStrings
		{
			get
			{
				return ConfigurationManager.ConnectionStrings;
			}
		}

		// Token: 0x0600056E RID: 1390 RVA: 0x0003F028 File Offset: 0x0003E428
		internal static object GetSection(string sectionName)
		{
			return ConfigurationManager.GetSection(sectionName);
		}
	}
}
