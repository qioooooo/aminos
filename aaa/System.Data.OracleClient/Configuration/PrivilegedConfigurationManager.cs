using System;
using System.Security.Permissions;

namespace System.Configuration
{
	// Token: 0x020000A1 RID: 161
	[ConfigurationPermission(SecurityAction.Assert, Unrestricted = true)]
	internal static class PrivilegedConfigurationManager
	{
		// Token: 0x1700018D RID: 397
		// (get) Token: 0x06000875 RID: 2165 RVA: 0x00075D80 File Offset: 0x00075180
		internal static ConnectionStringSettingsCollection ConnectionStrings
		{
			get
			{
				return ConfigurationManager.ConnectionStrings;
			}
		}

		// Token: 0x06000876 RID: 2166 RVA: 0x00075D94 File Offset: 0x00075194
		internal static object GetSection(string sectionName)
		{
			return ConfigurationManager.GetSection(sectionName);
		}
	}
}
