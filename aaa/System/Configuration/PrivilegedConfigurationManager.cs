using System;
using System.Security.Permissions;

namespace System.Configuration
{
	// Token: 0x020007A4 RID: 1956
	[ConfigurationPermission(SecurityAction.Assert, Unrestricted = true)]
	internal static class PrivilegedConfigurationManager
	{
		// Token: 0x17000E17 RID: 3607
		// (get) Token: 0x06003C21 RID: 15393 RVA: 0x001010AC File Offset: 0x001000AC
		internal static ConnectionStringSettingsCollection ConnectionStrings
		{
			get
			{
				return ConfigurationManager.ConnectionStrings;
			}
		}

		// Token: 0x06003C22 RID: 15394 RVA: 0x001010B3 File Offset: 0x001000B3
		internal static object GetSection(string sectionName)
		{
			return ConfigurationManager.GetSection(sectionName);
		}
	}
}
