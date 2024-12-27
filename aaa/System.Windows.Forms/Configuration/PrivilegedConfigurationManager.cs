using System;
using System.Security.Permissions;

namespace System.Configuration
{
	// Token: 0x02000042 RID: 66
	[ConfigurationPermission(SecurityAction.Assert, Unrestricted = true)]
	internal static class PrivilegedConfigurationManager
	{
		// Token: 0x17000058 RID: 88
		// (get) Token: 0x060001E5 RID: 485 RVA: 0x000070A8 File Offset: 0x000060A8
		internal static ConnectionStringSettingsCollection ConnectionStrings
		{
			get
			{
				return ConfigurationManager.ConnectionStrings;
			}
		}

		// Token: 0x060001E6 RID: 486 RVA: 0x000070AF File Offset: 0x000060AF
		internal static object GetSection(string sectionName)
		{
			return ConfigurationManager.GetSection(sectionName);
		}
	}
}
