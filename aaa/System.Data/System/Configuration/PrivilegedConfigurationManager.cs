using System;
using System.Security.Permissions;

namespace System.Configuration
{
	// Token: 0x0200038F RID: 911
	[ConfigurationPermission(SecurityAction.Assert, Unrestricted = true)]
	internal static class PrivilegedConfigurationManager
	{
		// Token: 0x170007AF RID: 1967
		// (get) Token: 0x060030DB RID: 12507 RVA: 0x002B892C File Offset: 0x002B7D2C
		internal static ConnectionStringSettingsCollection ConnectionStrings
		{
			get
			{
				return ConfigurationManager.ConnectionStrings;
			}
		}

		// Token: 0x060030DC RID: 12508 RVA: 0x002B8940 File Offset: 0x002B7D40
		internal static object GetSection(string sectionName)
		{
			return ConfigurationManager.GetSection(sectionName);
		}
	}
}
